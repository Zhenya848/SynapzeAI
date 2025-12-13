namespace UserService.Application.Commands.AddTokenToBalance;

public record AddTokenToBalanceCommand(Guid UserId, bool IsTrialBalance);