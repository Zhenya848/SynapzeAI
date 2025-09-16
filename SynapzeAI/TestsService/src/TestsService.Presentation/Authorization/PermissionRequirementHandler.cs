using Core;
using Framework.Authorization;
using Framework.Extensions;
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
        var userId = context.User.GetUserId();
        
        if (userId is null)
            return;

        context.Succeed(requirement);
    }
}