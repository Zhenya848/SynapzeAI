namespace UserService.Presentation.Requests;

public record CreateUserRequest(
    string Username,
    string Telegram,
    string Password,
    string Code);