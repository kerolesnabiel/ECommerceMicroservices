using AutoMapper;
using BuildingBlocks.User;
using MediatR;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Addresses.Commands.AddAddress;

public class AddAddressCommandHandler(
    ILogger<AddAddressCommandHandler> logger,
    IAddressRepository addressRepository,
    IUserContext userContext,
    IMapper mapper
    ) : IRequestHandler<AddAddressCommand>
{
    public async Task Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Adding new address to user: {UserId}", currentUser.Id);

        var address = mapper.Map<Address>(request);
        address.UserId = currentUser.Id;

        await addressRepository.AddAsync(address);
    }
}
