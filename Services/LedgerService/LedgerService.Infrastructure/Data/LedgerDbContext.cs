using LedgerService.Domain.Entities;
using Messaging.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace LedgerService.Infrastructure.Data
{
    public class LedgerDbContext : DbContext
    {
        public LedgerDbContext(DbContextOptions<LedgerDbContext> options) : base(options)
        {
        }

        public DbSet<Ledger> Ledgers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyMessagingConfiguration();

            base.OnModelCreating(modelBuilder);
        }
    }
}
