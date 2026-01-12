using Microsoft.EntityFrameworkCore;

namespace LedgerService.Infrastructure.Data
{
    public class LedgerDbContext : DbContext
    {
        public LedgerDbContext(DbContextOptions<LedgerDbContext> options) : base(options)
        {
        }
    }
}
