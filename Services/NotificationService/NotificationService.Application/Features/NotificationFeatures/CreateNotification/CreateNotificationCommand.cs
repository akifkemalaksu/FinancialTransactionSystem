namespace NotificationService.Application.Features.NotificationFeatures.CreateNotification
{
    public record CreateNotificationCommand
    {
        public required string Title { get; init; }
        public required string Message { get; init; }
    }
}
