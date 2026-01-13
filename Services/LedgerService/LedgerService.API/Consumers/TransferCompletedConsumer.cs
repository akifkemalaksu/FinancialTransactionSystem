using Messaging.Configurations;
using Messaging.Contracts;
using Messaging.Persistence.Infrastructure;
using Microsoft.Extensions.Options;

namespace LedgerService.API.Consumers
{
    public class TransferCompletedConsumer(
        IOptions<KafkaSettings> kafkaSettings,
        IServiceProvider serviceProvider,
        ILogger<TransferCompletedConsumer> logger
    ) : KafkaConsumerBase<TransferCompletedEvent>(kafkaSettings.Value, serviceProvider, logger)
    {

    }
}
