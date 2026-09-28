using MediatR;
using UserService.Application.Addresses.DTOs;

namespace UserService.Application.Addresses.Queries.GetAddressById;

public class GetAddressByIdCommand(Guid id) : IRequest<AddressDto>
{
    public Guid Id { get; set; } = id;
}
