using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

internal class SellerAccountRepository(UserServiceDbContext dbContext) : ISellerAccountRepository
{
    public async Task<SellerAccount> AddAsync(SellerAccount sellerAccount)
    {
        dbContext.SellerAccounts.Add(sellerAccount);
        await dbContext.SaveChangesAsync();
        return sellerAccount;
    }

    public async Task<SellerAccount?> GetByIdAsync(Guid Id)
    {
        return await dbContext.SellerAccounts.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<SellerAccount?> GetByUserIdAsync(Guid userId)
    {
        return await dbContext.SellerAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<SellerAccount> UpdateAsync(SellerAccount sellerAccount)
    {
        dbContext.SellerAccounts.Update(sellerAccount);
        await dbContext.SaveChangesAsync();
        return sellerAccount;
    }
}
