namespace UserService.Application.Responses;

public record LoginResponse(string AccessToken, Guid RefreshToken);