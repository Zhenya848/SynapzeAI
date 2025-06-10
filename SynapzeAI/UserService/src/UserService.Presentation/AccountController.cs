using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands.CreateUser;
using UserService.Application.Commands.GetInfoAboutUser;
using UserService.Application.Commands.GetUsers;
using UserService.Application.Commands.LoginUser;
using UserService.Application.Commands.RefreshTokens;
using UserService.Presentation.Requests;

namespace UserService.Presentation;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register(
        [FromBody] CreateUserRequest request,
        [FromServices] CreateUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateUserCommand(
            request.Username, 
            request.Nickname, 
            request.Email,
            request.Telegram, 
            request.Password);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Created();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] LoginUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return new ObjectResult(result.Value) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromBody] RefreshTokensRequest request,
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RefreshTokensCommand(request.AccessToken, request.RefreshToken);
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInfoAboutUser(
        [FromRoute] Guid id,
        [FromServices] GetInfoAboutUserHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPost("users")]
    public async Task<IActionResult> GetUsers(
        [FromBody] GetUsersRequest request,
        [FromServices] GetUsersHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new GetUsersCommand(request.Users, request.Roles);
        var result = await handler.Handle(command, cancellationToken);

        return Ok(result);
    }
}