using System.Security.Claims;
using CSharpFunctionalExtensions;
using UserService.Application.Models;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application;

public interface ITokenProvider
{
    JwtTokenResult GenerateAccessToken(User user);
    Task<Guid> GenerateRefreshToken(User user, Guid accessTokenJti, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<Claim>, ErrorList>> GetUserClaims(string jwtToken);
}