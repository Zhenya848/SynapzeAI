using CSharpFunctionalExtensions;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Application.Responses;
using UserService.Application.Responses.Login;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.RefreshTokens;

public class RefreshTokensHandler : ICommandHandler<Guid, Result<LoginResponse, ErrorList>>
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
        Guid refreshToken, 
        CancellationToken cancellationToken = default)
    {
        var oldRefreshSession = await _accountRepository
            .GetByRefreshToken(refreshToken, cancellationToken);

        if (oldRefreshSession.IsFailure)
            return (ErrorList)oldRefreshSession.Error;

        if (oldRefreshSession.Value.ExpiresIn < DateTime.UtcNow)
            return (ErrorList)Errors.RefreshSessions.ExpiredToken();
        
        _accountRepository.Delete(oldRefreshSession.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        var accessToken = _tokenProvider
            .GenerateAccessToken(oldRefreshSession.Value.User);
        
        var newRefreshToken = await _tokenProvider
            .GenerateRefreshToken(oldRefreshSession.Value.User, accessToken.Jti, cancellationToken);

        var userData = new UserInfo()
        {
            Id = oldRefreshSession.Value.User.Id,
            Email = oldRefreshSession.Value.User.Email!,
            UniqueUserName = oldRefreshSession.Value.User.UniqueUserName,
            UserName = oldRefreshSession.Value.User.UserName!
        };
        
        return new LoginResponse(accessToken.AccessToken, newRefreshToken, userData);
    }
}