using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Repositories;
using UserService.Domain;
using UserService.Domain.User;

namespace UserService.Application.Commands.SubtractTokenFromBalance;

public class SubtractTokenFromBalanceHandler 
    : ICommandHandler<SubtractTokenFromBalanceCommand, Result<BalanceType, Error>>
{
    private readonly UserManager<User> _userManager;

    public SubtractTokenFromBalanceHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<BalanceType, Error>> Handle(
        SubtractTokenFromBalanceCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByIdAsync(command.UserId.ToString());

        if (user is null)
            return Errors.User.NotFound(command.UserId.ToString());

        if (command.IsTrialBalance)
        {
            var setTrialBalanceResult = user.SetTrialBalance(user.TrialBalance - 1);

            if (setTrialBalanceResult.IsSuccess)
            {
                await _userManager.UpdateAsync(user);
                return BalanceType.Trial;
            }
        }

        var result = user.SetBalance(user.Balance - 1);

        if (result.IsFailure)
            return Error.Conflict("subtract.balance.conflict", "Недостаточно средств");
        
        await _userManager.UpdateAsync(user);

        return BalanceType.Normal;
    }
}