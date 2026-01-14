namespace LedgerService.Application.Services.DataAccessors
{
    public interface IUnitOfWork
    {
        ILedgerRepository Ledgers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
