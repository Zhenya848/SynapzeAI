using Core;
using CSharpFunctionalExtensions;

namespace TestsService.Application.Providers;

public interface IAiProvider
{
    public Task<Result<string, Error>> SendToAi(
        string request, 
        string? base64File = null,
        CancellationToken cancellationToken = default);
}