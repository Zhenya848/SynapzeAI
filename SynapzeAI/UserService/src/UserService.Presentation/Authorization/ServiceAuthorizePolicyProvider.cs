using Microsoft.AspNetCore.Authorization;

namespace UserService.Presentation.Authorization;

public class ServiceAuthorizePolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy?> GetPolicyAsync()
    {
        var policy = new AuthorizationPolicyBuilder(SecretKeyDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .AddRequirements(new ServiceAuthorizeAttribute())
            .Build();
        
        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        throw new NotImplementedException();
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        Task.FromResult(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser().Build());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy?>(null);
}