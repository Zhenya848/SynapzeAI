namespace UserService.Application.Commands.UpdateUser;

public record UpdateUserCommand(
    Guid UserId,
    string Username);