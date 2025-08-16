using AIService.Application.Commands.Generate;
using AIService.Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace AIService.Presentation.Grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly GenerateContentHandler _generateContentHandler;
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(GenerateContentHandler generateContentHandler, ILogger<GreeterService> logger)
    {
        _generateContentHandler = generateContentHandler;
        _logger = logger;
    }

    public override async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest request, ServerCallContext context)
    {
        var command = new GenerateContentCommand(request.UserMessage, AIModel.Deepseek);
        var cancellationToken = context.CancellationToken;
        
        var result = await _generateContentHandler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(
                "Error generating content: {error}", 
                string.Join(", ", result.Error.Select(e => e.Message)));

            return new GenerateContentResponse { Result = "none" };
        }

        return new GenerateContentResponse { Result = result.Value };
    }
}