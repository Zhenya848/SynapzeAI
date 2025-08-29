using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsService.Domain.Shared;

namespace TestsService.Presentation;

public static class Extensions
{
    public static ActionResult ToResponse(this Error error)
    {
        var envelope = Envelope.Error(error);
        
        return new ObjectResult(envelope) { StatusCode = GetStatusCode(error) };
    }

    public static ActionResult ToResponse(this ErrorList errors)
    {
        if (errors.Any() == false)
            return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };
        
        Envelope envelope = Envelope.Error(errors);
        
        return new ObjectResult(envelope) { StatusCode = GetStatusCode(errors) };
    }

    private static int GetStatusCode(ErrorList errors)
    {
        var statusCode = errors.Count() > 1 
            ? StatusCodes.Status500InternalServerError
            : errors.First().ErrorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Required => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        return statusCode;
    }
    
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

    public static string GetUserEmailRequired(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Email)?.Value
               ?? throw new UnauthorizedAccessException("User email not found in claims");
    }
    
    public static string? GetUserEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Email)?.Value;
    }

    public static string GetUserNameRequired(this ClaimsPrincipal user)
    {
        return user.FindFirst(CustomClaims.Name)?.Value 
               ?? throw new UnauthorizedAccessException("User name not found in claims");
    }
    
    public static string? GetUserName(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static bool HasRole(this ClaimsPrincipal user, string role)
    {
        return user.IsInRole(role) 
               || user.HasClaim("role", role) 
               || user.HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role);
    }
}