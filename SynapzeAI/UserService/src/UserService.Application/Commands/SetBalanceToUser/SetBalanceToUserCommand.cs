namespace UserService.Application.Commands.SetBalanceToUser;

public record SetBalanceToUserCommand(Guid UserId, int Pack);