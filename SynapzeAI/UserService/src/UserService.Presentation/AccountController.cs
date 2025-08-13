using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Commands.CreateUser;
using UserService.Application.Commands.GetUser;
using UserService.Application.Commands.GetUsers;
using UserService.Application.Commands.LoginUser;
using UserService.Application.Commands.LogoutUser;
using UserService.Application.Commands.RefreshTokens;
using UserService.Domain.Shared;
using UserService.Presentation.Requests;

namespace UserService.Presentation;

[ApiController]
[Route("api/[controller]")]
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
            request.Email,
            request.Password);
        
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(Envelope.Ok(null));
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
        
        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());

        return new ObjectResult(Envelope.Ok(result.Value)) { StatusCode = StatusCodes.Status201Created };
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokens(
        [FromServices] RefreshTokensHandler handler,
        CancellationToken cancellationToken = default)
    {
        if (HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) == false)
            return BadRequest("Refresh token was missing!");

        if (Guid.TryParse(refreshToken, out var refreshTokenGuid) == false)
            return Errors.Token.InvalidToken().ToResponse();
        
        var result = await handler.Handle(refreshTokenGuid, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        
        HttpContext.Response.Cookies.Append("refreshToken", result.Value.RefreshToken.ToString());
        
        return Ok(Envelope.Ok(result.Value));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromServices] LogoutUserHandler handler, 
        CancellationToken cancellationToken = default)
    {
        if (HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) == false)
            return BadRequest("Refresh token was missing!");

        if (Guid.TryParse(refreshToken, out var refreshTokenGuid) == false)
            return Errors.Token.InvalidToken().ToResponse();
        
        var result = await handler.Handle(refreshTokenGuid, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();
        
        HttpContext.Response.Cookies.Delete("refreshToken");
        
        return Ok();
    }
    
    [HttpPost("users")]
    public async Task<IActionResult> GetUsers(
        [FromBody] IEnumerable<Guid> userIds,
        [FromServices] GetUsersHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(userIds, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("user/{email}")]
    public async Task<IActionResult> GetUserByEmail(
        [FromRoute] string email,
        [FromServices] GetUserByEmailHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(email, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}