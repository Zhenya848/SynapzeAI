using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.Application.Abstractions;
using UserService.Application.Responses;
using UserService.Application.Responses.Login;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.LoginUser;

public class LoginUserHandler : ICommandHandler<LoginUserCommand, Result<LoginResponse, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<User> _logger;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserHandler(UserManager<User> userManager, ILogger<User> logger, ITokenProvider tokenProvider)
    {
        _userManager = userManager;
        _logger = logger;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<Result<LoginResponse, ErrorList>> Handle(
        LoginUserCommand userCommand, 
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(userCommand.Email);
        
        if (user == null)
            return (ErrorList)Errors.User.NotFound(userCommand.Email);
        
        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, userCommand.Password);

        if (passwordConfirmed == false)
            return (ErrorList)Errors.User.WrongCredentials();

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = await _tokenProvider.GenerateRefreshToken(user, accessToken.Jti, cancellationToken);
        
        _logger.LogInformation("Login successfully");

        var userData = new UserInfo()
        {
            Id = user.Id,
            Email = user.Email!,
            UniqueUserName = user.UniqueUserName,
            UserName = user.UserName!
        };
        
        return new LoginResponse(accessToken.AccessToken, refreshToken, userData);
    }
}