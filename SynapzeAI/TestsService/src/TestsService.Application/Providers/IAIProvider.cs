using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;

namespace TestsService.Application.Providers;

public interface IAIProvider
{
    Task<Result<string, ErrorList>> GenerateContent(
        string userRequest,
        CancellationToken cancellationToken = default);
}