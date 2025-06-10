using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Repositories;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountsDbContext _accountsDbContext;

    public AccountRepository(AccountsDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public Guid CreateParticipant(
        ParticipantAccount participantAccount)
    {
        var addResult = _accountsDbContext.ParticipantAccounts
            .Add(participantAccount);
        
        return participantAccount.Id;
    }

    public async Task<Result<RefreshSession, Error>> GetByRefreshToken(
        Guid refreshToken, 
        CancellationToken cancellationToken = default)
    {
        var refreshSession = await _accountsDbContext.RefreshSessions
            .Include(u => u.User)
            .FirstOrDefaultAsync(r => r.RefreshToken == refreshToken, cancellationToken);

        if (refreshSession == null)
            return Errors.General.NotFound(refreshToken);

        return refreshSession;
    }

    public void Delete(RefreshSession refreshSession)
    {
        _accountsDbContext.RefreshSessions.Remove(refreshSession);
    }

    public async Task<Result<User, Error>> GetInfoAboutUser(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _accountsDbContext.Users
            .Include(p => p.ParticipantAccount)
            .Include(a => a.AdminAccount)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        
        if (user == null)
            return Errors.User.NotFound();
        
        return user;
    }

    public async Task<IEnumerable<User>> GetUsers(
        IEnumerable<string> users, 
        IEnumerable<string> roles, 
        CancellationToken cancellationToken = default)
    {
        var usersExist = await _accountsDbContext.Users
            .Include(p => p.ParticipantAccount)
            .Include(a => a.AdminAccount)
            .Include(r => r.Roles)
            .Where(u => users.Contains(u.UserName) || u.Roles.Any(role => roles.Contains(role.Name)))
            .ToListAsync(cancellationToken);

        return usersExist;
    }
}