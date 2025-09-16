using Microsoft.AspNetCore.Authorization;

namespace UserService.Presentation.Authorization;

public class ServiceAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
    public string Code;
    
    public ServiceAuthorizeAttribute() : base()
    {
        
    }
}