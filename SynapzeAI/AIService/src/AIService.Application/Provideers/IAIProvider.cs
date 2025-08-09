using AIService.Domain;
using AIService.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AIService.Application.Provideers;

public interface IAIProvider
{
    Task<Result<string, ErrorList>> GenerateContent(
        string userRequest, 
        AIModel aiModel,
        CancellationToken cancellationToken = default);
}