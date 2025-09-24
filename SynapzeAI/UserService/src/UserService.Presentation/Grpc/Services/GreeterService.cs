using Grpc.Core;
using UserService.Application.Commands.SubtractTokenFromBalance;

namespace UserService.Presentation.Grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly SubtractTokenFromBalanceHandler _handler;

    public GreeterService(SubtractTokenFromBalanceHandler handler)
    {
        _handler = handler;
    }
    
    public override async Task<SubtractTokenFromBalanceResponse> SubtractTokenFromBalance(
        SubtractTokenFromBalanceRequest request, 
        ServerCallContext context)
    {
        if (Guid.TryParse(request.UserId, out var userId) == false)
            throw new RpcException(new Status(
                StatusCode.InvalidArgument, 
                "Invalid user id"
            ));

        var result = await _handler.Handle(userId, context.CancellationToken);
        
        if (result.IsFailure)
            throw new RpcException(new Status(
                StatusCode.FailedPrecondition, 
                result.Error.Message
            ));
        
        return new SubtractTokenFromBalanceResponse();
    }
}