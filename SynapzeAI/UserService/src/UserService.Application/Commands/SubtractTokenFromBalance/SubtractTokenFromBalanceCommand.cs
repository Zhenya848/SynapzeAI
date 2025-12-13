namespace UserService.Application.Commands.SubtractTokenFromBalance;

public record SubtractTokenFromBalanceCommand(Guid UserId, bool IsTrialBalance);