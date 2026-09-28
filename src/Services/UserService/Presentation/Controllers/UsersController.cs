using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Users.Commands.ChangePassword;
using UserService.Application.Users.Commands.UpdateUser;
using UserService.Application.Users.Queries.GetUser;

namespace UserService.Presentation.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> UpdateUserAsync()
    {
        var user = await mediator.Send(new GetUserQuery());
        return Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateUserAsync(UpdateUserCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpPut("me/change-password")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
