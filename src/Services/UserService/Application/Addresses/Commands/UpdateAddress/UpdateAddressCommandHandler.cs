using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.User;
using MediatR;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandHandler(
            ILogger<UpdateAddressCommandHandler> logger,
    IAddressRepository addressRepository,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<UpdateAddressCommand>
    {
        public async Task Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();
            logger.LogInformation("Updating address: {AddressId} for user: {UserId}", request.Id, currentUser.Id);

            var address = await addressRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Address), request.Id.ToString());

            if (address.UserId != currentUser.Id)
                throw new ForbiddenException();

            mapper.Map(request, address);
            await addressRepository.UpdateAsync(address);
        }
    }
}
