using Core;
using CSharpFunctionalExtensions;
using Grpc.Core;
using TestsService.Application.Abstractions;
using UserService.Presentation;

namespace TestsService.Presentation.Grpc.Services;

public class GreeterService : IGreeterService
{
    private readonly Greeter.GreeterClient _client;

    public GreeterService(Greeter.GreeterClient client)
    {
        _client = client;
    }
    
    public async Task<Result<string, ErrorList>> SubtractTokenFromBalance(
        Guid userId, 
        bool isTrialBalance,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new SubtractTokenFromBalanceRequest() 
                { UserId = userId.ToString(), IsTrialBalance = isTrialBalance };

            var result = await _client
                .SubtractTokenFromBalanceAsync(request, cancellationToken: cancellationToken);

            return result.SubtractedFrom;
        }
        catch (RpcException ex)
        {
            return (ErrorList)Error.Failure("subtract.token.failure", ex.Status.Detail);
        }
    }

    public async Task<UnitResult<ErrorList>> AddTokenToBalance(
        Guid userId, 
        bool isTrialBalance,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new AddTokenToBalanceRequest() 
                { UserId = userId.ToString(), IsTrialBalance = isTrialBalance };

            await _client.AddTokenToBalanceAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            return (ErrorList)Error.Failure("add.token.failure", ex.Status.Detail);
        }
        
        return Result.Success<ErrorList>();
    }
}