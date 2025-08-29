using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions;
using UserService.Domain;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.UpdateUser;

public class UpdateUserHandler : ICommandHandler<UpdateUserCommand, Result<Guid, ErrorList>>
{
    private readonly UserManager<User> _userManager;

    public UpdateUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var userResult = await _userManager
            .FindByIdAsync(command.UserId.ToString());
        
        if (userResult is null)
            return (ErrorList)Errors.User.NotFound();
        
        if (string.IsNullOrWhiteSpace(command.Username))
            return (ErrorList)Errors.General.ValueIsRequired("имя пользователя");
        
        userResult.UpdateUsername(command.Username);

        var result = await _userManager.UpdateAsync(userResult);
        
        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();

        return userResult.Id;
    }
}