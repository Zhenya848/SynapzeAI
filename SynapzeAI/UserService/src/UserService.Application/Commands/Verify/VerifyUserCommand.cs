namespace UserService.Application.Commands.Verify;

public record VerifyUserCommand(string Telegram, string Code);