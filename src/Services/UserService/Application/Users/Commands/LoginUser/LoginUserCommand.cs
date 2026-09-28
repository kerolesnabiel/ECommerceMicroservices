using MediatR;
using UserService.Application.Users.DTOs;

namespace UserService.Application.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<UserTokenDto>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
