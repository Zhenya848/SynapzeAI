using System.Security.Claims;
using TestsService.Domain.Shared;

namespace TestsService.Presentation.Extensions;

public static class UserClaimsExtensions
{
    public static Guid GetUserIdRequired(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(CustomClaims.Sub);

        if (userIdClaim is null || Guid.TryParse(userIdClaim.Value, out var userId) == false)
            throw new UnauthorizedAccessException("User ID not found in claims");

        return userId;
    }
    
    public static Guid? GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(CustomClaims.Sub);

        if (userIdClaim is null || Guid.TryParse(userIdClaim.Value, out var userId) == false)
            return null;

        return userId;
    }

    public static string GetUserTelegramRequired(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Telegram)?.Value
               ?? throw new UnauthorizedAccessException("User telegram not found in claims");
    }
    
    public static string? GetUserTelegram(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Telegram)?.Value;
    }

    public static string GetUserNameRequired(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Name)?.Value 
               ?? throw new UnauthorizedAccessException("User name not found in claims");
    }
    
    public static string? GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Name)?.Value;
    }

    public static bool HasRole(this ClaimsPrincipal user, string role)
    {
        return user.IsInRole(role) 
               || user.HasClaim("role", role) 
               || user.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role);
    }
}