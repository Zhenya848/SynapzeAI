namespace UserService.Application.Commands.RefreshTokens;

public record RefreshTokensCommand(string AccessToken, Guid RefreshToken);