using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Addresses.Commands.AddAddress;
using UserService.Application.Addresses.Commands.UpdateAddress;
using UserService.Application.Addresses.Queries.GetAddressById;
using UserService.Application.Addresses.Queries.GetAddresses;

namespace UserService.Presentation.Controllers;

[Route("api/users/me/addresses")]
[ApiController]
[Authorize]
public class AddressesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAddress(AddAddressCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAddresses()
    {
        var addresses = await mediator.Send(new GetAddressesCommand());
        return Ok(addresses);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAddressById([FromRoute] Guid id)
    {
        var addresses = await mediator.Send(new GetAddressByIdCommand(id));
        return Ok(addresses);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAddress([FromRoute] Guid id, UpdateAddressCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }
}
