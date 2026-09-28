using MediatR;
using UserService.Application.Users.DTOs;

namespace UserService.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest<UserTokenDto>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
