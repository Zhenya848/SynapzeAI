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
        
        var userExist = await _accountRepository
            .FindUserByTelegram(command.Telegram, cancellationToken);

        if (userExist.IsSuccess)
            return (ErrorList)Error.Failure("user.already exist", "User with that name already exist");
        
        var role = await _roleManager.FindByNameAsync(AccountRoles.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {AccountRoles.PARTICIPANT} does not exist");
        
        var usersCount = _userManager.Users.Count();
        
        var user = User.CreateParticipant(command.Username, command.Telegram, role, usersCount);
        
        var participantAccount = ParticipantAccount.CreateParticipant(command.Username, user);

        var verificationCode = GenerateVerificationCode().ToString();
        
        using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

        try
        {
            var result = await _userManager.CreateAsync(user, command.Password);
            
            if (result.Succeeded == false)
                return (ErrorList)result.Errors
                    .Select(e => Error.Failure(e.Code, e.Description)).ToList();
            
            var verificationResult = Verification.Create(
                user.Id,
                verificationCode,
                DateTime.UtcNow.AddMinutes(VerificationConstants.ExpiresMinutes));
        
            if (verificationResult.IsFailure)
                return (ErrorList)verificationResult.Error;

            _accountRepository.CreateParticipant(participantAccount);
            _accountRepository.CreateVerification(verificationResult.Value);
            
            await _unitOfWork.SaveChanges(cancellationToken);
            
            var sendResult = await _messageProvider.SendCode(verificationCode, command.Telegram);

            if (sendResult.IsFailure)
            {
                transaction.Rollback();
                
                return (ErrorList)sendResult.Error;   
            }
            
            transaction.Commit();
        
            _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            _logger.LogError(ex, "Error creating user in transaction");
            
            return (ErrorList)Error.Failure("user.creation_failed", ex.Message);
        }
        
        return Result.Success<ErrorList>();
    }

    private int GenerateVerificationCode() => 
        new Random().Next(10000, 99999);
}