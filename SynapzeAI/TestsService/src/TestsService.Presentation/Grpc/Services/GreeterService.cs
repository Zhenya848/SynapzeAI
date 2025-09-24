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
    
    public async Task<UnitResult<ErrorList>> SubtractTokenFromBalance(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new SubtractTokenFromBalanceRequest() { UserId = userId.ToString() };

            await _client.SubtractTokenFromBalanceAsync(request, cancellationToken: cancellationToken);
        }
        catch (RpcException ex)
        {
            return (ErrorList)Error.Failure("subtract.token.failure", ex.Status.Detail);
        }
        
        return Result.Success<ErrorList>();
    }
}