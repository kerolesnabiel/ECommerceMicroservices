using AutoMapper;
using MediatR;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using BuildingBlocks.Exceptions;
using BuildingBlocks.User;

namespace UserService.Application.SellerAccounts.Commands.AddSellerAccount;

public class AddSellerAccountCommandHandler(
    ILogger<AddSellerAccountCommandHandler> logger,
    ISellerAccountRepository sellerAccountRepository,
    IUserContext userContext,
    IMapper mapper
) : IRequestHandler<AddSellerAccountCommand>
{
    public async Task Handle(AddSellerAccountCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Creating a seller account for user: {UserId}", currentUser.Id);

        var sellerAccount = await sellerAccountRepository.GetByIdAsync(currentUser.Id);

        if (sellerAccount != null)
            throw new ConflictException("User already has a seller account");

        sellerAccount = mapper.Map<SellerAccount>(request);
        sellerAccount.Id = currentUser.Id;
        await sellerAccountRepository.AddAsync(sellerAccount);
    }
}
