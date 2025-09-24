using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Abstractions;
using UserService.Application.Models;
using UserService.Domain.User;
using UserService.Infrastructure.Options;
using UserService.Presentation.Options;
using CustomClaims = UserService.Domain.User.CustomClaims;

namespace UserService.Infrastructure.Providers.Authorization;

public class JwtTokenProvider : ITokenProvider
{
    private readonly AuthOptions _authOptions;
    private readonly RefreshSessionOptions _refreshSessionOptions;
    private readonly AccountsDbContext _accountsDbContext;
    private readonly IKeyProvider _keyProvider;

    public JwtTokenProvider(
        IOptions<AuthOptions> authOptions,
        IOptions<RefreshSessionOptions> refreshSessionOptions,
        AccountsDbContext accountsDbContext,
        IKeyProvider keyProvider)
    {
        _authOptions = authOptions.Value;
        _refreshSessionOptions = refreshSessionOptions.Value;
        _accountsDbContext = accountsDbContext;
        _keyProvider = keyProvider;
    }
    
    public JwtTokenResult GenerateAccessToken(User user)
    {
        var rsaKey = _keyProvider.GetPrivateRsa();
        var key = new RsaSecurityKey(rsaKey);
        
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
        
        var jti = Guid.NewGuid();
        
        var claims = new[]
        {
            new Claim(CustomClaims.Sub, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Telegram, user.Telegram),
            new Claim(CustomClaims.Name, user.UserName!)
        };

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(_authOptions.ExpiredMinutesTime),
            claims: claims,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        return new JwtTokenResult(tokenHandler.WriteToken(token), jti);
    }

    public async Task<Guid> GenerateRefreshToken(
        User user, 
        Guid accessTokenJti, 
        CancellationToken cancellationToken = default)
    {
        var refreshSession = new RefreshSession
        {
            RefreshToken = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(_refreshSessionOptions.ExpiredDaysTime),
            Jti = accessTokenJti
        };

        _accountsDbContext.RefreshSessions.Add(refreshSession);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);

        return refreshSession.RefreshToken;
    }
}