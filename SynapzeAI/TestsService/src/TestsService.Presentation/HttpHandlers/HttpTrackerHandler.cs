using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TestsService.Presentation.Authorization;

namespace TestsService.Presentation.HttpHandlers;

public class HttpTrackerHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthOptions _authOptions;

    public HttpTrackerHandler(
        IHttpContextAccessor httpContextAccessor, 
        IOptions<AuthOptions> authOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _authOptions = authOptions.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        const string authKey = "X-Internal-Service-Key";
        const string authorization = "Authorization";

        if (_httpContextAccessor.HttpContext is null ||
            _httpContextAccessor.HttpContext.Request.Headers
                .TryGetValue(authKey, out var jwtValues) == false ||
                    string.IsNullOrWhiteSpace(jwtValues.FirstOrDefault()))
        {
            request.Headers.Add(authKey, _authOptions.SecretKey);
        }
        else
        {
            request.Headers.Add(authorization, jwtValues.FirstOrDefault());
        }
        
        return base.SendAsync(request, cancellationToken);
    }
}