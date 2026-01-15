using Messaging.Abstractions;
using Messaging.Attributes;
using Messaging.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace Messaging.Persistence.Infrastructure
{
    public class OutboxProducer(
        DbContext _context,
        IActualKafkaProducer _kafkaProducer
    ) : IKafkaProducer
    {
        public async Task ProduceAsync<T>(T message, CancellationToken cancellationToken) where T : IEvent
        {
            string content = SerializeMessage(message);
            var msgType = typeof(T);

            var outboxMessage = CreateOutboxMessage(message.Id, msgType, content);

            _context.Set<OutboxMessage>().Add(outboxMessage);

            string topicName = GetTopicName(msgType);

            await TryPublishAndUpdateOutboxAsync(topicName, message.Id.ToString(), content, outboxMessage, cancellationToken);
        }

        private static string SerializeMessage<T>(T message)
        {
            return JsonSerializer.Serialize(message);
        }

        private static OutboxMessage CreateOutboxMessage(Guid messageId, Type msgType, string content)
        {
            return new OutboxMessage
            {
                Id = messageId,
                Type = msgType.FullName,
                Content = content,
                OccurredOnUtc = DateTime.UtcNow,
                RetryCount = 0
            };
        }

        private static string GetTopicName(Type msgType)
        {
            var topicAttr = msgType.GetCustomAttribute<KafkaTopicAttribute>();

            if (topicAttr is null)
            {
                throw new Exception($"KafkaTopicAttribute not found on type '{msgType.Name}'.");
            }

            return topicAttr.Name;
        }

        private async Task TryPublishAndUpdateOutboxAsync(
            string topicName,
            string messageKey,
            string content,
            OutboxMessage outboxMessage,
            CancellationToken cancellationToken)
        {
            try
            {
                await _kafkaProducer.PublishToKafkaAsync(topicName, messageKey, content, cancellationToken);
                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                outboxMessage.RetryCount++;
                outboxMessage.Error = ex.Message;
            }
        }
    }
}
