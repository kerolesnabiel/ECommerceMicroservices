using AutoMapper;
using MediatR;
using UserService.Application.SellerAccounts.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using BuildingBlocks.Exceptions;

namespace UserService.Application.SellerAccounts.Queries.GetSellerAccountById;

public class GetSellerAccountByIdQueryHandler(
        ILogger<GetSellerAccountByIdQueryHandler> logger,
        ISellerAccountRepository sellerAccountRepository,
        IMapper mapper
    ) : IRequestHandler<GetSellerAccountByIdQuery, SellerAccountMiniDto>
{
    public async Task<SellerAccountMiniDto> Handle(GetSellerAccountByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting seller account by id: {SellerAccountId}", request.Id);

        var sellerAccount = await sellerAccountRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(SellerAccount), request.Id.ToString());

        return mapper.Map<SellerAccountMiniDto>(sellerAccount);
    }
}
