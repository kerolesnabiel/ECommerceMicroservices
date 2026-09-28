using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.LoginUser;
using UserService.Application.Users.Commands.LogoutUser;
using UserService.Application.Users.Commands.RefreshToken;
using UserService.Application.Users.Commands.RegisterUser;

namespace UserService.Presentation.Controllers;

[Route("api/users")]
[ApiController]
public class AuthController(
    IMediator mediator, 
    IWebHostEnvironment env, 
    IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(RegisterUserCommand command)
    {
        var token = await mediator.Send(command);
        return Ok(token.ToObject());
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(LoginUserCommand command)
    {
        var token = await mediator.Send(command);
        return Ok(token.ToObject());
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
    {
        var token = await mediator.Send(command);
        return Ok(token.ToObject());
    }


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await mediator.Send(new LogoutUserCommand());
        return NoContent();
    }

    [HttpGet("public-key")]
    public IActionResult GetPublicKey()
    {
        string publicKeyPath = Path.Combine(env.ContentRootPath, "public_key.pem");

        if (!System.IO.File.Exists(publicKeyPath))
            return NotFound(new { error = "Public key not found" });

        var publicKey = System.IO.File.ReadAllText(publicKeyPath);
        return Ok(new { 
            publicKey,
            issuer = configuration["JwtSettings:Issuer"],
            audience = configuration["JwtSettings:Audience"]
        });
    }
}
