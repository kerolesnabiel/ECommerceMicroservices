using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;

internal class UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : DbContext(options)
{
    public DbSet<Address> Addresses { get; set; } = default!;
    public DbSet<SellerAccount> SellerAccounts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Address>()
            .HasIndex(a => a.UserId);
    }
}