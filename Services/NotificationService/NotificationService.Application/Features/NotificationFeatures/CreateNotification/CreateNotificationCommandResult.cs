namespace NotificationService.Application.Features.NotificationFeatures.CreateNotification
{
    public record CreateNotificationCommandResult
    {
        public Guid NotificationId { get; init; }
    }
}
