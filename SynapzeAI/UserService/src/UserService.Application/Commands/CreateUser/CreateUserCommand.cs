namespace UserService.Application.Commands.CreateUser;

public record CreateUserCommand(
    string Username, 
    string Nickname,
    string Email, 
    string Telegram, 
    string Password);