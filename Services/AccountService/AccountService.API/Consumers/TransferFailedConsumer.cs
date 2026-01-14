using Messaging.Configurations;
using Messaging.Contracts;
using Messaging.Persistence.Infrastructure;
using Microsoft.Extensions.Options;

namespace AccountService.API.Consumers
{
    public class TransferFailedConsumer(
        IOptions<KafkaSettings> kafkaSettings,
        IServiceProvider serviceProvider,
        ILogger<TransferFailedConsumer> logger
    ) : KafkaConsumerBase<TransferFailedEvent>(kafkaSettings.Value, serviceProvider, logger)
    {
    }
}

