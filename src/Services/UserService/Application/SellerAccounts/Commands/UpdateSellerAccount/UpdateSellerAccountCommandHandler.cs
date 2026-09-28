using AutoMapper;
using MediatR;
using UserService.Domain.Interfaces;
using BuildingBlocks.Exceptions;
using BuildingBlocks.User;

namespace UserService.Application.SellerAccounts.Commands.UpdateSellerAccount;

public class UpdateSellerAccountCommandHandler(
        ILogger<UpdateSellerAccountCommandHandler> logger,
        ISellerAccountRepository sellerAccountRepository,
        IUserContext userContext,
        IMapper mapper
    ) : IRequestHandler<UpdateSellerAccountCommand>
{

    public async Task Handle(UpdateSellerAccountCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Updating seller account for user: {UserId}", currentUser.Id);

        var sellerAccount = await sellerAccountRepository.GetByUserIdAsync(currentUser.Id)
            ?? throw new NotFoundException("User doesn't have a seller account");

        mapper.Map(request, sellerAccount);
        sellerAccount.UpdatedAt = DateTime.UtcNow;

        await sellerAccountRepository.UpdateAsync(sellerAccount);
    }
}
