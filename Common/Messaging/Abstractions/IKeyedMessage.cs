namespace Messaging.Abstractions
{
    public interface IKeyedMessage
    {
        string Key { get; }
    }
}