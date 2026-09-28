using AutoMapper;
using UserService.Application.Users.Commands.UpdateUser;
using UserService.Domain.Entities;

namespace UserService.Application.Users.DTOs;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserCommand, User>()
            .ForAllMembers(options =>
                options.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<User, UserDto>();
    }
}
