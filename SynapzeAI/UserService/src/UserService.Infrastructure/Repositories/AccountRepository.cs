using Core;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Repositories;
using UserService.Domain;
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
            return Errors.General.NotFound();
        
        return user;
    }

    public async Task<Result<User, Error>> FindUserByTelegram(
        string telegram, 
        CancellationToken cancellationToken = default)
    {
        var userResult = await _accountsDbContext.Users
            .FirstOrDefaultAsync(t => t.Telegram == telegram, cancellationToken);
        
        if (userResult == null)
            return Errors.General.NotFound();
        
        return userResult;
    }

    public Guid CreateVerification(Verification verification)
    {
        var addResult = _accountsDbContext.Verifications.Add(verification);
        
        return verification.Id;
    }

    public Guid DeleteVerification(Verification verification)
    {
        var deleteResult = _accountsDbContext.Verifications.Remove(verification);
        
        return verification.Id;
    }

    public async Task<Result<Verification, Error>> GetVerificationByUserId(
        Guid userId, CancellationToken 
            cancellationToken = default)
    {
        var verificationResult = await _accountsDbContext.Verifications
            .FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);

        if (verificationResult is null)
            return Errors.General.NotFound();
        
        return verificationResult;
    }
}