namespace Messaging.Configurations
{
    public record KafkaSettings
    {
        public required string Host { get; set; }
        public required string GroupId { get; set; }
        public required Dictionary<string, string> TopicMap { get; set; }
    }
}
