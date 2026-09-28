using MediatR;
using UserService.Application.Services;
using UserService.Application.Users.DTOs;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands.RefreshToken;

public class RefreshTokenCommandHandler
    (IUserRepository userRepository,
    JwtService jwtService) : IRequestHandler<RefreshTokenCommand, UserTokenDto>
{
    public async Task<UserTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByRefreshTokenAsync(request.RefreToken);

        if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        var token = jwtService.GetUserTokenDto(user);

        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(token.RefreshTokenExpiryDays);

        await userRepository.UpdateAsync(user);

        return token;
    }
}
