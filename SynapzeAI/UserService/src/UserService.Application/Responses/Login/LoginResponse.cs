namespace UserService.Application.Responses.Login;

public record LoginResponse(string AccessToken, Guid RefreshToken, Guid UserId, string Email, string UserName);