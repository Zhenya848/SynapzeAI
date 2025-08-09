using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using TestsService.Application.Providers;
using TestsService.Application.Tests.Commands.CreateWithAI;
using TestsService.Domain.Shared;

namespace TestsService.Infrastructure.Providers;

public class AIProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AIProvider> _logger;

    public AIProvider(ILogger<AIProvider> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }
    
    public async Task<Result<string, ErrorList>> GenerateContent(
        string userRequest, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _httpClient.Timeout = TimeSpan.FromMinutes(5);

            var request = JsonSerializer.Serialize(new GenerateContentRequest(userRequest));
            var content = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5101/api/AI", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            return responseBody;
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка: {errorCode}", ex.Message);
            
            return (ErrorList)Errors.General.Failure(ex.Message);
        }
    }
    
    public record GenerateContentRequest(string UserRequest);
}