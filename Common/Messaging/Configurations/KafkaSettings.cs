namespace Messaging.Configurations
{
    public record KafkaSettings
    {
        public required string BootstrapServers { get; set; }
        public required string GroupId { get; set; }
    }
}
