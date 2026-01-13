using Messaging.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Persistence.EntityConfigurations
{
    public static class MessagingEntityConfiguration
    {
        public static void ApplyMessagingConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboxMessage>(builder =>
            {
                builder.ToTable("OutboxMessages");
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Type).HasMaxLength(256).IsRequired();
                builder.Property(x => x.Content).IsRequired();
            });

            modelBuilder.Entity<InboxMessage>(builder =>
            {
                builder.ToTable("InboxMessages");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Type).HasMaxLength(256).IsRequired();
                builder.Property(x => x.Content).IsRequired();
            });
        }
    }
}
