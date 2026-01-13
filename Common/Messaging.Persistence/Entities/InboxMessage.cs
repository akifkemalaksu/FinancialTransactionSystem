namespace Messaging.Persistence.Entities
{
    public class InboxMessage
    {
        public Guid Id { get; set; }
        public required string Type { get; set; }
        public required string Content { get; set; }
        public DateTime ReceivedOnUtc { get; set; }
    }
}
