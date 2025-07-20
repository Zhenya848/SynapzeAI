using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Domain.Shared;

namespace TestsService.Presentation.Authorization;

public class PermissionRequirementHandler(IServiceScopeFactory scopeFactory) 
    : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionAttribute requirement)
    {
        var httpContext = (context.Resource as HttpContext)!;
        var userIdString = context.User.Claims
            .FirstOrDefault(c => c.Type == CustomClaims.Sub)?.Value;
        
        Guid userId;

        if (userIdString is null || Guid.TryParse(userIdString, out userId) == false)
        {
            httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:5173");
            httpContext.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            
            context.Fail();
            return;
        }
        
        context.Succeed(requirement);
        
        /*var scope = scopeFactory.CreateScope();
        var accountContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();
        
        var permissions = await accountContract.GetUserPermissionCodes(userId);
        
        if (permissions.Contains(requirement.Code))
            context.Succeed(requirement);*/
    }
}