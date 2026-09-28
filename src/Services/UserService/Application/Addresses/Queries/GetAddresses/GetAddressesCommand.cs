using MediatR;
using UserService.Application.Addresses.DTOs;

namespace UserService.Application.Addresses.Queries.GetAddresses;

public class GetAddressesCommand : IRequest<IEnumerable<AddressDto>>
{
}
