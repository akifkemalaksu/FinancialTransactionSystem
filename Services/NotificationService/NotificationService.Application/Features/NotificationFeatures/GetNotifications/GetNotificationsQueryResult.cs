using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.NotificationFeatures.GetNotifications
{
    public record GetNotificationsQueryResult
    {
        public required List<Notification> Notifications { get; init; }
    }
}

