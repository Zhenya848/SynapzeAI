namespace UserService.Presentation.Requests;

public record VerifyUserRequest(string Telegram, string Code);