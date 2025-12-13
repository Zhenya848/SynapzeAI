using System.Net.Http.Json;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestsService.Application.Providers;
using TestsService.Presentation.Ai;

namespace TestsService.Infrastructure.Providers;

public class OpenRouterResponse
{
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    public Message Message { get; set; }
}

public class Message
{
    public string Content { get; set; }
}

public class AiProvider : IAiProvider
{
    private readonly HttpClient _httpClient;
    private readonly AiOptions _aiOptions;
    private readonly ILogger<AiProvider> _logger;

    public AiProvider(HttpClient httpClient, IOptions<AiOptions> aiOptions, ILogger<AiProvider> logger)
    {
        _httpClient = httpClient;
        _aiOptions = aiOptions.Value;
        _logger = logger;
    }
    
    public async Task<Result<string, Error>> SendToAi(
        string request, 
        string? base64File = null,
        CancellationToken cancellationToken = default)
    {
        var apiKey = _aiOptions.ApiKey;
        
        try
        {
            var openRouterRequest = new
            {
                model = _aiOptions.Model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = CreateContent(request, base64File)
                    }
                },
                plugins = new[]
                {
                    new
                    {
                        id = "file-parser",
                        pdf = new { engine = "mistral-ocr" }
                    }
                }
            };

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsJsonAsync(
                "https://openrouter.ai/api/v1/chat/completions", 
                openRouterRequest,
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenRouterResponse>(cancellationToken);

                if (result is null)
                    return Error.Failure("response.parse.failure", "Cannot parse response of result");
                
                return result.Choices[0].Message.Content;
            }
            
            _logger.LogError(
                "Internal server error: {ex}", 
                await response.Content.ReadAsStringAsync(cancellationToken));

            return Errors.General.Failure();
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ex}", ex.Message);
            
            return Errors.General.Failure();
        }
    }
    
    private object CreateContent(string request, string? filePath = null)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return new[]
            {
                new { type = "text", text = request }
            };
        }

        if (filePath.StartsWith("data:image/"))
        {
            return new List<object>
            {
                new { type = "text", text = request },
                new 
                { 
                    type = "image_url", 
                    image_url = new { url = filePath }
                }
            };
        }
        
        if (filePath.StartsWith("data:application/pdf"))
        {
            return new List<object>
            {
                new { type = "text", text = request },
                new 
                { 
                    type = "file", 
                    file = new 
                    { 
                        filename = "document.pdf", 
                        file_data = filePath 
                    }
                }
            };
        }

        return new[] { new { type = "text", text = request } };
    }
}