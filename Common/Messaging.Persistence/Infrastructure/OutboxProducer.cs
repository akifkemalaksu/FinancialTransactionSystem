using Messaging.Abstractions;
using Messaging.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Messaging.Persistence.Infrastructure
{
    public class OutboxProducer(
        DbContext _context
    ) : IKafkaProducer
    {
        public async Task ProduceAsync<T>(T message, CancellationToken cancellationToken) where T : IEvent
        {
            var outboxMessage = new OutboxMessage
            {
                Id = message.Key,
                Type = typeof(T).FullName,
                Content = JsonSerializer.Serialize(message),
                OccurredOnUtc = DateTime.UtcNow,
                RetryCount = 0
            };

            await _context.Set<OutboxMessage>().AddAsync(outboxMessage);
        }
    }
}
