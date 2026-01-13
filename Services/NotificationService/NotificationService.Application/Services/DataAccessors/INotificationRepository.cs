using NotificationService.Domain.Entities;

namespace NotificationService.Application.Services.DataAccessors
{
    public interface INotificationRepository
    {
        void Add(Notification notification);
    }
}
