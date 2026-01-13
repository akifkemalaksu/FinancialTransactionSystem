using Confluent.Kafka;
using Messaging.Abstractions;
using Messaging.Attributes;
using Messaging.Configurations;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Messaging.Infrastructure
{
    public class ActualKafkaProducer : IActualKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public ActualKafkaProducer(
            IOptions<KafkaSettings> _kafkaSettings
        )
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.Value.BootstrapServers,
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task ProduceAsync<T>(T message, CancellationToken cancellationToken) where T : class
        {
            var topicAttr = typeof(T).GetCustomAttribute<KafkaTopicAttribute>();
            var topic = topicAttr?.Name ?? throw new Exception($"Topic is not defined!, {typeof(T).Name}");

            var serializedMessage = System.Text.Json.JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedMessage }, cancellationToken: cancellationToken);
        }

        public async Task PublishToKafkaAsync(string topic, string content, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = content }, cancellationToken: cancellationToken);
        }
    }
}
