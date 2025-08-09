using AIService.Application.Abstractions;
using AIService.Application.Provideers;
using AIService.Domain;
using AIService.Domain.Shared;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceStack;

namespace AIService.Application.Commands.Generate;

public class GenerateContentHandler : ICommandHandler<GenerateContentCommand, Result<string, ErrorList>>
{
    private readonly IAIProvider  _aiProvider;

    public GenerateContentHandler(IAIProvider aiProvider)
    {
        _aiProvider = aiProvider;
    }
    
    public async Task<Result<string, ErrorList>> Handle(
        GenerateContentCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await _aiProvider.GenerateContent(command.UserRequest, command.AIModel, cancellationToken);

        if (result.IsFailure)
            return result.Error;
        
        return result.Value;
    }
}