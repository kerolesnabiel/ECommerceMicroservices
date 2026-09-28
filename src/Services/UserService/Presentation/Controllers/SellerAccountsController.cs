using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.SellerAccounts.Commands.AddSellerAccount;
using UserService.Application.SellerAccounts.Commands.UpdateSellerAccount;
using UserService.Application.SellerAccounts.Queries.GetSellerAccount;
using UserService.Application.SellerAccounts.Queries.GetSellerAccountById;

namespace UserService.Presentation.Controllers;

[Route("api/users/me/seller")]
[ApiController]
[Authorize]
public class SellerAccountsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddSellerAccount(AddSellerAccountCommand command)
    {
        await mediator.Send(command);
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSellerAccount(UpdateSellerAccountCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetSellerAccount()
    {
        var res = await mediator.Send(new GetSellerAccountQuery());
        return Ok(res);
    }
    
    [HttpGet("/api/sellers/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSellerAccountById([FromRoute] Guid id)
    {
        var res = await mediator.Send(new GetSellerAccountByIdQuery(id));
        return Ok(res);
    }
}
