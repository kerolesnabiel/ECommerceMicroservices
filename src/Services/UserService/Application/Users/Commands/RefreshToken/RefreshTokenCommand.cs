using MediatR;
using UserService.Application.Users.DTOs;

namespace UserService.Application.Users.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<UserTokenDto>
{
    public string RefreToken { get; set; } = default!;
}
