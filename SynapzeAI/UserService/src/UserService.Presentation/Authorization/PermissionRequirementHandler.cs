using Core;
using Framework.Authorization;
using Framework.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Presentation.Authorization;

public class PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory) 
    : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionAttribute permission)
    {
        var userId = context.User.GetUserId();
        
        if (userId is null)
            return;

        context.Succeed(permission);
    }
}