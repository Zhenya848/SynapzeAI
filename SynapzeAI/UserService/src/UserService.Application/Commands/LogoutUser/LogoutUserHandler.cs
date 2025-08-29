using CSharpFunctionalExtensions;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain.Shared;

namespace UserService.Application.Commands.LogoutUser;

public class LogoutUserHandler : ICommandHandler<Guid, UnitResult<ErrorList>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        Guid refreshToken, 
        CancellationToken cancellationToken = default)
    {
        var oldRefreshSession = await _accountRepository
            .GetByRefreshToken(refreshToken, cancellationToken);

        if (oldRefreshSession.IsFailure)
            return (ErrorList)oldRefreshSession.Error;
        
        _accountRepository.Delete(oldRefreshSession.Value);
        await _unitOfWork.SaveChanges(cancellationToken);

        return Result.Success<ErrorList>();
    }
}