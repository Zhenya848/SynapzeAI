using Microsoft.AspNetCore.Authorization;

namespace UserService.Presentation.Authorization;

public class SecretKeyRequirementHandler : AuthorizationHandler<ServiceAuthorizeAttribute>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ServiceAuthorizeAttribute requirement)
    {
        if (context.User.HasClaim(c => c is { Type: "IsService", Value: "true" }))
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}