using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.User;

namespace UserService.Application.Commands.AddTokenToBalance;

public class AddTokenToBalanceHandler : ICommandHandler<AddTokenToBalanceCommand, UnitResult<Error>>
{
    private readonly UserManager<User> _userManager;

    public AddTokenToBalanceHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<Error>> Handle(
        AddTokenToBalanceCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Errors.User.NotFound(command.UserId.ToString());

        if (command.IsTrialBalance)
        {
            var setTrialBalanceResult = user.SetTrialBalance(user.TrialBalance + 1);

            if (setTrialBalanceResult.IsFailure)
                return setTrialBalanceResult.Error;
        }
        else
        {
            var setBalanceResult = user.SetBalance(user.Balance + 1);

            if (setBalanceResult.IsFailure)
                return setBalanceResult.Error;
        }
        
        await _userManager.UpdateAsync(user);

        return Result.Success<Error>();
    }
}