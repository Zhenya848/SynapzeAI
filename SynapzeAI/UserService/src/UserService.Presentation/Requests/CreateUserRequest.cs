namespace UserService.Presentation.Requests;

public record CreateUserRequest(
    string Username, 
    string Nickname, 
    string Email, 
    string Telegram, 
    string Password);