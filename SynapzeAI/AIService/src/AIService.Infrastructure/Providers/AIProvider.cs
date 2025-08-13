using System.Net.Http.Headers;
using AIService.Application.Commands.Generate;
using AIService.Application.Provideers;
using AIService.Domain;
using AIService.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceStack;

namespace AIService.Infrastructure.Providers;

public class AIProvider : IAIProvider
{
    private readonly IOptions<APIKeys> _apiKeys;
    private readonly ILogger<GenerateContentHandler> _logger;
    private readonly HttpClient _httpClient;

    public AIProvider(
        IOptions<APIKeys> apiKeys, 
        ILogger<GenerateContentHandler> logger,
        HttpClient httpClient)
    {
        _apiKeys = apiKeys;
        _logger = logger;
        _httpClient = httpClient;
    }
    
    public async Task<Result<string, ErrorList>> GenerateContent(
        string userRequest, 
        AIModel aiModel,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userRequest))
            return (ErrorList)Errors.General.ValueIsInvalid(nameof(userRequest));
        
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKeys.Value.AIKey);
        _httpClient.BaseAddress = new Uri("https://openrouter.ai/api/v1");
        _httpClient.Timeout = TimeSpan.FromMinutes(5);

        var client = new JsonApiClient(_httpClient);
        
        try
        {
            var request = new OpenAiChatCompletion
            {
                Model = AIModelByName.GetModel(aiModel),
                Messages =
                [
                    new() { Role = "user", Content = userRequest }
                ]
            };

            var response = await client.PostAsync<OpenAiChatResponse>("/chat/completions", request, cancellationToken);

            return response.Choices[0].Message.Content;
        }
        catch (WebServiceException ex)
        {
            _logger.LogError("Ошибка: {errorCode} - {errorMessage}", ex.ErrorCode, ex.ErrorMessage);

            if (ex.ResponseBody != null)
                _logger.LogInformation("Детали: {responseBody}", ex.ResponseBody);

            return (ErrorList)Errors.General.Failure(ex.ErrorMessage);
        }
    }
}