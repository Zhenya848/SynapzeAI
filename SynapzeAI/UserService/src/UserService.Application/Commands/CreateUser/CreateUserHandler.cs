using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Providers;
using UserService.Application.Repositories;
using UserService.Domain;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.CreateUser;

public class CreateUserHandler : ICommandHandler<CreateUserCommand, UnitResult<ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageProvider _messageProvider;

    public CreateUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<CreateUserHandler> logger,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork,
        IMessageProvider messageProvider)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _messageProvider = messageProvider;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        CreateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return (ErrorList)Errors.General.ValueIsRequired("имя пользователя");
        
        if (string.IsNullOrWhiteSpace(command.Telegram))
            return (ErrorList)Errors.General.ValueIsInvalid("telegram");
        
        var telegramWithoutAt = command.Telegram.TrimStart('@');
        var telegramWithAt = "@" + telegramWithoutAt;
        
        var userExist = await _accountRepository
            .FindUserByTelegram(telegramWithAt, cancellationToken);

        if (userExist.IsSuccess)
            return (ErrorList)Errors.User.AlreadyExist();
        
        var role = await _roleManager.FindByNameAsync(AccountRoles.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {AccountRoles.PARTICIPANT} does not exist");
        
        var user = User.CreateParticipant(command.Username, telegramWithAt, role);
        var participantAccount = ParticipantAccount.CreateParticipant(user);

        var verificationResult = await _accountRepository
            .GetVerificationByUsername(telegramWithoutAt, cancellationToken);
        
        if (verificationResult.IsFailure)
            return (ErrorList)verificationResult.Error;
        
        var verification = verificationResult.Value;
        
        var verifyResult = verification.Verify(command.Code);

        if (verifyResult.IsFailure)
        {
            if (verifyResult.Error.Code is "verification.expired" or "verification.blocked")
                _accountRepository.DeleteVerification(verification);
            
            await _unitOfWork.SaveChanges(cancellationToken);
            return (ErrorList)verifyResult.Error;
        }
        
        var result = await _userManager.CreateAsync(user, command.Password);
            
        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        _accountRepository.CreateParticipant(participantAccount);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success<ErrorList>();
    }
}