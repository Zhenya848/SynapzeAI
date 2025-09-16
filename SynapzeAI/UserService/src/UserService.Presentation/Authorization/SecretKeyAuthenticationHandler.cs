using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UserService.Presentation.Authorization;

public class SecretKeyAuthenticationHandler : AuthenticationHandler<SecretKeyAuthenticationOptions>
{
    public SecretKeyAuthenticationHandler(
        IOptionsMonitor<SecretKeyAuthenticationOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder, 
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        
    }

    public SecretKeyAuthenticationHandler(
        IOptionsMonitor<SecretKeyAuthenticationOptions> options, 
        ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.TryGetValue(Options.HeaderName, out var receivedKey) == false)
            return Task.FromResult(AuthenticateResult.NoResult());
        
        if (receivedKey != Options.ExpectedKey)
            return Task.FromResult(AuthenticateResult.Fail("Invalid secret key"));

        var claimsIdentity = new ClaimsIdentity("SecretKey");
        claimsIdentity.AddClaim(new Claim("IsService", "true"));
        
        var principal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}