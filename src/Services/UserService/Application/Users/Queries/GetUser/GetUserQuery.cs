using MediatR;
using UserService.Application.Users.DTOs;

namespace UserService.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<UserDto>
{
}
