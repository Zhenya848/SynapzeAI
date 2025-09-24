using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Repositories;
using UserService.Domain.User;

namespace UserService.Application.Commands.SubtractTokenFromBalance;

public class SubtractTokenFromBalanceHandler : ICommandHandler<Guid, UnitResult<Error>>
{
    private readonly UserManager<User> _userManager;

    public SubtractTokenFromBalanceHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnitResult<Error>> Handle(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager
            .FindByIdAsync(userId.ToString());

        if (user is null)
            return Errors.User.NotFound(userId.ToString());

        var result = user.SetBalance(user.Balance - 1);

        if (result.IsFailure)
            return Error.Conflict("subtract.balance.conflict", "Недостаточно средств");
        
        await _userManager.UpdateAsync(user);

        return Result.Success<Error>();
    }
}