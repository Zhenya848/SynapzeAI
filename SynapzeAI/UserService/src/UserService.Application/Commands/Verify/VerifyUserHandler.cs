using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.Verify;

public class VerifyUserHandler : ICommandHandler<VerifyUserCommand, UnitResult<ErrorList>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyUserHandler(
        IAccountRepository accountRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        VerifyUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var userResult = await _accountRepository
            .FindUserByTelegram(command.Telegram, cancellationToken);
        
        if (userResult.IsFailure)
            return (ErrorList)userResult.Error;
        
        var user = userResult.Value;
        
        if (user.IsVerified)
            return (ErrorList)Error.Conflict("user.already.verified", "User already verified");
        
        var verificationResult = await _accountRepository
            .GetVerificationByUserId(user.Id, cancellationToken);
        
        if (verificationResult.IsFailure)
            return (ErrorList)verificationResult.Error;
        
        var verification = verificationResult.Value;
        
        var verifyResult = verification.Verify(command.Code);

        if (verifyResult.IsFailure
            && verifyResult.Error.Code is not ("verification.expired" or "verification.blocked"))
        {
            await _unitOfWork.SaveChanges(cancellationToken);
            
            return (ErrorList)verifyResult.Error;
        }
        
        _accountRepository.DeleteVerification(verification);

        if (verifyResult.IsFailure)
        {
            await _userManager.DeleteAsync(user);
            
            return (ErrorList)verifyResult.Error;
        }
        
        user.VerifyUser();
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success<ErrorList>();
    }
}