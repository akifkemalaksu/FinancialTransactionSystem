namespace Messaging.Abstractions
{
    public interface IEvent
    {
        Guid Key { get; }
    }
}
