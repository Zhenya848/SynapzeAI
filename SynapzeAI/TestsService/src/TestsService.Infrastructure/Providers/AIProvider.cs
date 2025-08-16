using System.Text;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using TestsService.Application.Providers;
using TestsService.Application.Tests.Commands.CreateWithAI;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using TestsService.Presentation;

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
            using var channel = GrpcChannel.ForAddress("http://localhost:5101");
            var client = new Greeter.GreeterClient(channel);

            var request = new GenerateContentRequest() { UserMessage = userRequest };
            var response = await client.GenerateContentAsync(request);
            
            return response.Result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return (ErrorList)Error.Failure("get.users.failure", "Cannot get users, see log errors");
        }
    }
}