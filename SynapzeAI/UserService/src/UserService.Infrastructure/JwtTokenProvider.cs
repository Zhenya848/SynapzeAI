using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Application;
using UserService.Application.Models;
using UserService.Domain.Shared;
using UserService.Domain.User;
using UserService.Infrastructure.Options;
using UserService.Presentation;
using UserService.Presentation.Options;

namespace UserService.Infrastructure;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly RefreshSessionOptions _refreshSessionOptions;
    private readonly AccountsDbContext _accountsDbContext;

    public JwtTokenProvider(
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshSessionOptions> refreshSessionOptions,
        AccountsDbContext accountsDbContext)
    {
        _jwtOptions = jwtOptions.Value;
        _refreshSessionOptions = refreshSessionOptions.Value;
        _accountsDbContext = accountsDbContext;
    }
    
    public JwtTokenResult GenerateAccessToken(User user)
    {
        var jti = Guid.NewGuid();
        
        var claims = new[]
        {
            new Claim(CustomClaims.Sub, user.Id.ToString()),
            new Claim(CustomClaims.Jti, jti.ToString()),
            new Claim(CustomClaims.Email, user.Email ?? "")
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiredMinutesTime),
            claims: claims,
            signingCredentials: creds
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

    public async Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        
        var validationParameters = TokenValidationParametersFactory.CreateWithoutLifeTime(_jwtOptions);

        var validationResult = await jwtHandler.ValidateTokenAsync(jwtToken, validationParameters);

        if (validationResult.IsValid == false)
            return (ErrorList)Errors.Token.InvalidToken();

        return validationResult.ClaimsIdentity.Claims.ToList();
    }
}