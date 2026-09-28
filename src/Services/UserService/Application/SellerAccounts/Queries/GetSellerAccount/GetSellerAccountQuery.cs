using MediatR;
using UserService.Application.SellerAccounts.DTOs;

namespace UserService.Application.SellerAccounts.Queries.GetSellerAccount;

public class GetSellerAccountQuery: IRequest<SellerAccountDto>
{
}
