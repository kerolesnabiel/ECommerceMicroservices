using AutoMapper;
using UserService.Application.SellerAccounts.Commands.AddSellerAccount;
using UserService.Application.SellerAccounts.Commands.UpdateSellerAccount;
using UserService.Domain.Entities;

namespace UserService.Application.SellerAccounts.DTOs;

public class SellerAccountProfile : Profile
{
    public SellerAccountProfile()
    {
        CreateMap<AddSellerAccountCommand, SellerAccount>()
            .ForMember(s => s.DateOfBirth, options => 
                options.MapFrom(c => c.DateOfBirth.ToUniversalTime()));

        CreateMap<UpdateSellerAccountCommand, SellerAccount>()
            .ForMember(s => s.DateOfBirth, options =>
                options.MapFrom(c => c.DateOfBirth.HasValue ? c.DateOfBirth.Value.ToUniversalTime() : (DateTime?)null))
            .ForAllMembers(options =>
                options.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<SellerAccount, SellerAccountDto>();
        CreateMap<SellerAccount, SellerAccountMiniDto>();
    }
}
