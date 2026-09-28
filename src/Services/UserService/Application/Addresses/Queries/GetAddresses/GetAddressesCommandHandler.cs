using AutoMapper;
using BuildingBlocks.User;
using MediatR;
using UserService.Application.Addresses.DTOs;
using UserService.Domain.Interfaces;

namespace UserService.Application.Addresses.Queries.GetAddresses;

public class GetAddressesCommandHandler(
    ILogger<GetAddressesCommandHandler> logger,
    IAddressRepository addressRepository,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<GetAddressesCommand, IEnumerable<AddressDto>>
{
    public async Task<IEnumerable<AddressDto>> Handle(GetAddressesCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Getting addresses for user: {UserId}", currentUser.Id);

        var addresses = await addressRepository.GetByUserIdAsync(currentUser.Id);

        return mapper.Map<IEnumerable<AddressDto>>(addresses);
    }
}
