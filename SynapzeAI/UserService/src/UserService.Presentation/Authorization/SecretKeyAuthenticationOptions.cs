using Microsoft.AspNetCore.Authentication;

namespace UserService.Presentation.Authorization;

public class SecretKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string HeaderName { get; set; } = "X-Internal-Service-Key";
    public string ExpectedKey { get; set; } = string.Empty;
}