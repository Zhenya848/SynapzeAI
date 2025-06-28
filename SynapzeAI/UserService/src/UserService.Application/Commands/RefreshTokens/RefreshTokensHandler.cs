using CSharpFunctionalExtensions;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Application.Responses;
using UserService.Application.Responses.Login;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.RefreshTokens;

public class RefreshTokensHandler : ICommandHandler<RefreshTokensCommand, Result<LoginResponse, ErrorList>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokensHandler(
        IAccountRepository accountRepository,
        ITokenProvider tokenProvider,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _tokenProvider = tokenProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        RefreshTokensCommand command, 
        CancellationToken cancellationToken = default)
    {
        var oldRefreshSession = await _accountRepository
            .GetByRefreshToken(command.RefreshToken, cancellationToken);

        if (oldRefreshSession.IsFailure)
            return (ErrorList)oldRefreshSession.Error;

        if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
            return (ErrorList)Errors.RefreshSessions.ExpiredToken();
        
        var userClaims = await _tokenProvider.GetUserClaims(command.AccessToken);
        
        if (userClaims.IsFailure)
            return userClaims.Error;

        var userIdStr = userClaims.Value.FirstOrDefault(s => s.Type == CustomClaims.Sub)?.Value;

        if (Guid.TryParse(userIdStr, out var userId) == false)
            return (ErrorList)Errors.General.Failure(userIdStr);
        
        if (oldRefreshSession.Value.UserId != userId)
            return (ErrorList)Errors.Token.InvalidToken();
        
        var userJtiStr = userClaims.Value.FirstOrDefault(s => s.Type == CustomClaims.Jti)?.Value;
        
        if (Guid.TryParse(userJtiStr, out var userJti) == false)
            return (ErrorList)Errors.General.Failure(userJtiStr);
        
        if (userJti != oldRefreshSession.Value.Jti)
            return (ErrorList)Errors.Token.InvalidToken();
        
        _accountRepository.Delete(oldRefreshSession.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = _tokenProvider
            .GenerateAccessToken(oldRefreshSession.Value.User);
        
        var newRefreshToken = await _tokenProvider
            .GenerateRefreshToken(oldRefreshSession.Value.User, accessToken.Jti, cancellationToken);

        return new LoginResponse(accessToken.AccessToken, newRefreshToken);
    }
}