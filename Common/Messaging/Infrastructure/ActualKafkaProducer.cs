using Confluent.Kafka;
using Messaging.Abstractions;
using Messaging.Configurations;
using Microsoft.Extensions.Options;

namespace Messaging.Infrastructure
{
    public class ActualKafkaProducer : IActualKafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public ActualKafkaProducer(
            IOptions<KafkaSettings> _kafkaSettings
        )
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.Value.BootstrapServers,
            };
            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishToKafkaAsync(string topic, string key, string content, CancellationToken cancellationToken)
        {
            var message = new Message<string, string> { Key = key, Value = content };
            await _producer.ProduceAsync(topic, message, cancellationToken: cancellationToken);
        }
    }
}
