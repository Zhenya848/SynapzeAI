namespace UserService.Application.Commands.CreateUser;

public record CreateUserCommand(
    string Username, 
    string Telegram,
    string Password,
    string Code);