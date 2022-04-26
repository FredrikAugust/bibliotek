using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class MigrationHelper
{
    public static async Task Migrate(IServiceProvider provider)
    {
        using var scope = provider.CreateScope();

        var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

        if (dbContext == null) return;
        
        if (dbContext.Database.IsSqlServer())
            await dbContext.Database.MigrateAsync();
    }
}