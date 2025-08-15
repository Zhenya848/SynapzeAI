using Grpc.Core;
using UserService.API;
using UserService.Application.Commands.GetUser;
using UserService.Application.Commands.GetUsers;
using UserService.Domain.User;

namespace UserService.Presentation.Grpc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly GetUsersHandler  _getUsersHandler;
    private readonly GetUserByEmailHandler _getUserByEmailHandler;

    public GreeterService(
        GetUsersHandler getUsersHandler, 
        GetUserByEmailHandler getUserByEmailHandler)
    {
        _getUsersHandler = getUsersHandler;
        _getUserByEmailHandler = getUserByEmailHandler;
    }
    
    public override async Task<GetUsersResponse> GetUsers(
        GetUsersRequest request, 
        ServerCallContext context)
    {
        var userIds = request.UserIds.Select(i => Guid.Parse(i));
        var cancellationToken = context.CancellationToken;
        
        var result = await _getUsersHandler.Handle(userIds, cancellationToken);
        
        var response = new GetUsersResponse();
        
        response.Users.AddRange(result.Select(u => new UserInfoResponse
        {
            Id = u.Id.ToString(),
            UserName = u.UserName,
            Email = u.Email
        }));
    
        return response;
    }

    public override async Task<GetUserByEmailResponse> GetUserByEmail(
        GetUserByEmailRequest request,
        ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        
        var result = await _getUserByEmailHandler.Handle(request.Email, cancellationToken);

        if (result.IsFailure)
            return new GetUserByEmailResponse();

        var user = result.Value;
        
        var response = new GetUserByEmailResponse();
        response.User = new UserInfoResponse() 
            { Id = user.Id.ToString(), UserName = user.UserName, Email = user.Email };
        
        return response;
    }
}