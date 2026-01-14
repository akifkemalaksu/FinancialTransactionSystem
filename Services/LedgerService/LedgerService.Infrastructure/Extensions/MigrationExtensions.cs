using LedgerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerService.Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LedgerDbContext>();
            dbContext.Database.Migrate();
        }
    }
}
