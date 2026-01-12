using NotificationService.Domain.Entities;

namespace NotificationService.Application.Services.DataAccessors
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification, CancellationToken cancellationToken);
    }
}
