using Core;
using CSharpFunctionalExtensions;

namespace TestsService.Application.Abstractions;

public interface IGreeterService
{
    public Task<UnitResult<ErrorList>> SubtractTokenFromBalance(
        Guid userId,
        CancellationToken cancellationToken = default);
}