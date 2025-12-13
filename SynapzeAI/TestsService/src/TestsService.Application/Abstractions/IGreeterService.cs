using Core;
using CSharpFunctionalExtensions;

namespace TestsService.Application.Abstractions;

public interface IGreeterService
{
    public Task<Result<string, ErrorList>> SubtractTokenFromBalance(
        Guid userId,
        bool isTrialBalance,
        CancellationToken cancellationToken = default);
    
    public Task<UnitResult<ErrorList>> AddTokenToBalance(
        Guid userId,
        bool isTrialBalance,
        CancellationToken cancellationToken = default);
}