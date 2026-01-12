using NotificationService.Application.Services.DataAccessors;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Infrastructure.Services.DataAccessors
{
    public class UnitOfWork(
        NotificationDbContext _context
    ) : IUnitOfWork
    {
        private INotificationRepository? _notifications;

        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();
    }
}