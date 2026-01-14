namespace NotificationService.Application.Services.DataAccessors
{
    public interface IUnitOfWork
    {
        INotificationRepository Notifications { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}