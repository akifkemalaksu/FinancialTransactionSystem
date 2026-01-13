using NotificationService.Application.Services.DataAccessors;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Infrastructure.Services.DataAccessors
{
    public class NotificationRepository(NotificationDbContext _context) : INotificationRepository
    {
        public void Add(Notification notification) => _context.Notifications.Add(notification);

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
