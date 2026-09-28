using AutoMapper;
using MediatR;
using UserService.Application.SellerAccounts.DTOs;
using UserService.Domain.Interfaces;
using BuildingBlocks.Exceptions;
using BuildingBlocks.User;

namespace UserService.Application.SellerAccounts.Queries.GetSellerAccount;

public class GetSellerAccountQueryHandler(
        ILogger<GetSellerAccountQueryHandler> logger,
        ISellerAccountRepository sellerAccountRepository,
        IUserContext userContext,
        IMapper mapper) : IRequestHandler<GetSellerAccountQuery, SellerAccountDto>
{
    public async Task<SellerAccountDto> Handle(GetSellerAccountQuery request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Getting seller account for user: {UserId}", currentUser.Id);

        var sellerAccount = await sellerAccountRepository.GetByUserIdAsync(currentUser.Id)
            ?? throw new NotFoundException("User doesn't have a seller account");

        return mapper.Map<SellerAccountDto>(sellerAccount);
    }
}
