namespace NotificationService.Application.Services.DataAccessors
{
    public interface IUnitOfWork: IDisposable
    {
        INotificationRepository Notifications { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}