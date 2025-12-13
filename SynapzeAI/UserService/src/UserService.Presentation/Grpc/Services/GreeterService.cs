using Grpc.Core;
using UserService.Application.Commands.AddTokenToBalance;
using UserService.Application.Commands.SubtractTokenFromBalance;
using UserService.Domain;

namespace UserService.Presentation.Grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly SubtractTokenFromBalanceHandler _subtractTokenFromBalanceHandler;
    private readonly AddTokenToBalanceHandler _addTokenToBalanceHandler;

    public GreeterService(
        SubtractTokenFromBalanceHandler subtractTokenFromBalanceHandler,
        AddTokenToBalanceHandler addTokenToBalanceHandler)
    {
        _subtractTokenFromBalanceHandler = subtractTokenFromBalanceHandler;
        _addTokenToBalanceHandler = addTokenToBalanceHandler;
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

        var command = new SubtractTokenFromBalanceCommand(userId, request.IsTrialBalance);

        var result = await _subtractTokenFromBalanceHandler
            .Handle(command, context.CancellationToken);
        
        if (result.IsFailure)
            throw new RpcException(new Status(
                StatusCode.FailedPrecondition, 
                result.Error.Message
            ));
        
        return new SubtractTokenFromBalanceResponse() 
            { SubtractedFrom = result.Value.ToString() };
    }

    public override async Task<AddTokenToBalanceResponse> AddTokenToBalance(
        AddTokenToBalanceRequest request, 
        ServerCallContext context)
    {
        if (Guid.TryParse(request.UserId, out var userId) == false)
            throw new RpcException(new Status(
                StatusCode.InvalidArgument, 
                "Invalid user id"
            ));
        
        var command = new AddTokenToBalanceCommand(userId, request.IsTrialBalance);

        var result = await _addTokenToBalanceHandler.Handle(command, context.CancellationToken);
        
        if (result.IsFailure)
            throw new RpcException(new Status(
                StatusCode.FailedPrecondition, 
                result.Error.Message
            ));
        
        return new AddTokenToBalanceResponse();
    }
}