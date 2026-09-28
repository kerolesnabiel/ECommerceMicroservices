using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

internal class UserRepository(UserServiceDbContext dbContext) : IUserRepository
{
    public async Task<User> AddAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> GetByIdAsync(Guid Id)
    {
        return await dbContext.Users
            .SingleOrDefaultAsync(u => u.Id == Id);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await dbContext.Users
            .Include(u => u.SellerAccount)
            .SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<User> UpdateAsync(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
}
