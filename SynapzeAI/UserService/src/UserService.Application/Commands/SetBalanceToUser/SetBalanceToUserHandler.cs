using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.SetBalanceToUser;

public class SetBalanceToUserHandler : ICommandHandler<SetBalanceToUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;

    public SetBalanceToUserHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        SetBalanceToUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return (ErrorList)Errors.User.NotFound(command.UserId.ToString());

        var setBalanceResult = user.SetBalance(user.Balance + command.Pack);
        await _userManager.UpdateAsync(user);

        if (setBalanceResult.IsFailure)
            return (ErrorList)setBalanceResult.Error;

        return Result.Success<ErrorList>();
    }
}