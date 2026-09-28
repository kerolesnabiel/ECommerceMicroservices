using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid Id);
    Task<User?> GetByRefreshTokenAsync(string refreshToken);
    Task<User> UpdateAsync(User user);
}
