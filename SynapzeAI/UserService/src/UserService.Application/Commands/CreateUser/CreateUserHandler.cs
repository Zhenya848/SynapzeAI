using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Abstractions;
using UserService.Application.Commands.LoginUser;
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

    public CreateUserHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<CreateUserHandler> logger,
        IAccountRepository accountRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _accountRepository = accountRepository;
    }
    
    public async Task<UnitResult<ErrorList>> Handle(
        CreateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Username))
            return (ErrorList)Errors.General.ValueIsRequired("имя пользователя");
        
        if (EmailValidator.IsVaild(command.Email) == false)
            return (ErrorList)Errors.General.ValueIsInvalid("почта");
        
        var userExist = await _userManager.FindByEmailAsync(command.Email);

        if (userExist != null)
            return (ErrorList)Errors.User.AlreadyExist();
        
        var role = await _roleManager.FindByNameAsync(AccountRoles.PARTICIPANT)
                   ?? throw new ApplicationException($"Role {AccountRoles.PARTICIPANT} does not exist");
        
        var usersCount = _userManager.Users.Count();
        
        var user = User.CreateParticipant(command.Username, command.Email, role, usersCount);
        
        var participantAccount = ParticipantAccount.CreateParticipant(command.Username, user);

        _accountRepository.CreateParticipant(participantAccount);
        
        var result = await _userManager.CreateAsync(user, command.Password);

        if (result.Succeeded == false)
            return (ErrorList)result.Errors
                .Select(e => Error.Failure(e.Code, e.Description)).ToList();
        
        _logger.LogInformation("User created: {userName} a new account with password.", user.UserName);
        
        return Result.Success<ErrorList>();
    }
}