using CSharpFunctionalExtensions;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Repositories;

public interface IAccountRepository
{
    public Guid CreateParticipant(ParticipantAccount participantAccount);
    
    Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken,
        CancellationToken cancellationToken = default);
    
    void Delete(RefreshSession refreshSession);
    
    public Task<Result<User, Error>> GetInfoAboutUser(
        Guid userId,
        CancellationToken cancellationToken = default);
}