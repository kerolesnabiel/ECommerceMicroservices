using MediatR;
using UserService.Application.Services;
using UserService.Application.Users.DTOs;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler(
    ILogger<LoginUserCommandHandler> logger,
    IUserRepository userRepository,
    ISellerAccountRepository sellerAccountRepository,
    JwtService jwtService) 
        : IRequestHandler<LoginUserCommand, UserTokenDto>
{
    public async Task<UserTokenDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Login user: {@Email}", request.Email);

        var user = await userRepository.GetByEmailAsync(request.Email) 
            ?? throw new UnauthorizedAccessException("Invalid email or password");

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Invalid email or password");

        user.SellerAccount = await sellerAccountRepository.GetByUserIdAsync(user.Id);
        ;
        var token = jwtService.GetUserTokenDto(user);

        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(token.RefreshTokenExpiryDays);
        await userRepository.UpdateAsync(user);

        return token;
    }
}
