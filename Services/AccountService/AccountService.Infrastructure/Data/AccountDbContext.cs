using AccountService.Domain.Entities;
using Messaging.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyMessagingConfiguration();

            base.OnModelCreating(modelBuilder);
        }
    }
}
