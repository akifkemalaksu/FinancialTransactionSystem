namespace LedgerService.Application.Services.DataAccessors
{
    public interface IUnitOfWork : IDisposable
    {
        ILedgerRepository Ledgers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
