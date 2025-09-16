using UserService.Application.Models;
using UserService.Domain.User;

namespace UserService.Application.Abstractions;

public interface ITokenProvider
{
    JwtTokenResult GenerateAccessToken(User user);
    Task<Guid> GenerateRefreshToken(User user, Guid accessTokenJti, CancellationToken cancellationToken = default);
}