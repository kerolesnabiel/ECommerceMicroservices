using AutoMapper;
using UserService.Application.Addresses.Commands.AddAddress;
using UserService.Application.Addresses.Commands.UpdateAddress;
using UserService.Domain.Entities;

namespace UserService.Application.Addresses.DTOs;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<AddAddressCommand, Address>();

        CreateMap<UpdateAddressCommand, Address>()
            .ForAllMembers(options =>
                options.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Address, AddressDto>();
    }
}
