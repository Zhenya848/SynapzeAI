namespace UserService.Presentation.Requests;

public record RefreshTokensRequest(string AccessToken, Guid RefreshToken);