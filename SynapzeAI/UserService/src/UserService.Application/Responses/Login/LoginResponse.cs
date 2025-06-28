namespace UserService.Application.Responses.Login;

public record LoginResponse(string AccessToken, Guid RefreshToken);