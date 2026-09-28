using MediatR;
using UserService.Application.SellerAccounts.DTOs;

namespace UserService.Application.SellerAccounts.Queries.GetSellerAccountById;

public class GetSellerAccountByIdQuery(Guid id) : IRequest<SellerAccountMiniDto>
{
    public Guid Id { get; set; } = id;
}
