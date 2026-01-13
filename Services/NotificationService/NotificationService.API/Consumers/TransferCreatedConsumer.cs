using Messaging.Configurations;
using Messaging.Contracts;
using Messaging.Persistence.Infrastructure;
using Microsoft.Extensions.Options;

namespace NotificationService.API.Consumers
{
    public class TransferCreatedConsumer(
        IOptions<KafkaSettings> kafkaSettings,
        IServiceProvider serviceProvider,
        ILogger<TransferCreatedConsumer> logger
    ) : KafkaConsumerBase<TransferCreatedEvent>(kafkaSettings.Value, serviceProvider, logger)
    {
    }
}