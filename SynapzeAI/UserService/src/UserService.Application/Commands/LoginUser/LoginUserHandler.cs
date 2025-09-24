using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Application.Responses.Login;
using UserService.Domain.User;
using static System.String;

namespace UserService.Application.Commands.LoginUser;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<LoginResponse, ErrorList>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<User> _logger;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserHandler(
        IAccountRepository accountRepository, 
        UserManager<User> userManager,
        ILogger<User> logger, 
        ITokenProvider tokenProvider)
    {
        _accountRepository = accountRepository;
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        LoginUserCommand userCommand, 
        CancellationToken cancellationToken = default)
    {
        var userResult = await _accountRepository
            .FindUserByTelegram(userCommand.Telegram, cancellationToken);

        if (userResult.IsFailure)
            return (ErrorList)Errors.User.WrongCredentials();
        
        var user = userResult.Value;

        if (user.IsVerified == false)
        {
            var result = await _userManager.DeleteAsync(user);
            
            if (result.Succeeded == false)
                _logger.LogError(Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            
            return (ErrorList)Error.Failure("user.login.failure", "Пользователь без подтверждения. Попробуйте зарегистрироваться заново");
        }
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, userCommand.Password);

        if (passwordConfirmed == false)
            return (ErrorList)Errors.User.WrongCredentials();

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider
            .GenerateRefreshToken(user, accessToken.Jti, cancellationToken);
        
        _logger.LogInformation("Login successfully");

        var userData = new UserInfo()
        {
            Id = user.Id,
            Telegram = user.Telegram,
            UniqueUserName = user.UserName!,
            UserName = user.Name,
            Balance = user.Balance
        };
        
        return new LoginResponse(accessToken.AccessToken, refreshToken, userData);
    }
}