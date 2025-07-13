namespace UserService.Application.Commands.CreateUser;

public record CreateUserCommand(
    string Username, 
    string Email,
    string Password);