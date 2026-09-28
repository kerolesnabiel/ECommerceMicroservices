using Microsoft.EntityFrameworkCore;

namespace NotificationService.Data.Seeders;

internal class Seeder(NotificationDbContext dbContext)
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
