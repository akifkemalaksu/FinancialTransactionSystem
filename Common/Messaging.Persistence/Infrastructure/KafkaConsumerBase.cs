using Confluent.Kafka;
using Messaging.Abstractions;
using Messaging.Attributes;
using Messaging.Configurations;
using Messaging.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System.Reflection;
using System.Text.Json;

namespace Messaging.Persistence.Infrastructure
{
    public abstract class KafkaConsumerBase<TMessage> : BackgroundService
        where TMessage : IEvent
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<KafkaConsumerBase<TMessage>> _logger;
        private readonly string _topicName;
        protected KafkaConsumerBase(
            KafkaSettings kafkaSettings,
            IServiceProvider serviceProvider,
            ILogger<KafkaConsumerBase<TMessage>> logger
        )
        {
            _kafkaSettings = kafkaSettings;
            _serviceProvider = serviceProvider;
            _logger = logger;

            var topicAttr = typeof(TMessage).GetCustomAttribute<KafkaTopicAttribute>();

            if (topicAttr == null)
                throw new Exception($"KafkaTopicAttribute was not found on message type '{typeof(TMessage).Name}'.");

            _topicName = topicAttr.Name;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = _kafkaSettings.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(_topicName);

            _logger.LogInformation("Consumer started for {Topic}.", _topicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    if (result?.Message == null) continue;

                    var retryPolicy = Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));

                    await retryPolicy.ExecuteAsync(async () =>
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
                        var handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<TMessage>>();

                        string kafkaMessageKey = result.Message.Key;

                        if (!Guid.TryParse(kafkaMessageKey, out Guid messageId))
                        {
                            _logger.LogWarning("Message Key (ID) is empty! Idempotency (Inbox) check cannot be performed.");

                            throw new InvalidOperationException($"Invalid message key: '{kafkaMessageKey}'. Expected a valid GUID for idempotency check.");
                        }

                        var alreadyProcessed = await dbContext.Set<InboxMessage>()
                            .AnyAsync(x => x.Id == messageId, stoppingToken);

                        if (alreadyProcessed)
                        {
                            _logger.LogInformation("Message already processed, skipping: {MessageId}", messageId);
                            consumer.Commit(result);
                            return;
                        }

                        var message = JsonSerializer.Deserialize<TMessage>(result.Message.Value);
                        if (message != null)
                        {
                            await handler.HandleAsync(message);

                            await dbContext.Set<InboxMessage>().AddAsync(new InboxMessage
                            {
                                Id = messageId,
                                Type = typeof(TMessage).Name,
                                ReceivedOnUtc = DateTime.UtcNow,
                                Content = result.Message.Value
                            }, stoppingToken);

                            await dbContext.SaveChangesAsync(stoppingToken);
                        }

                        consumer.Commit(result);
                    });
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Critical error occurred while processing message.");
                }
            }
        }
    }
}
