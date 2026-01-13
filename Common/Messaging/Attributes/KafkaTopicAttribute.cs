namespace Messaging.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KafkaTopicAttribute : Attribute
    {
        public string Name { get; }
        public KafkaTopicAttribute(string name)
        {
            Name = name;
        }
    }
}