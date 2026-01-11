namespace TransactionService.Application.Services.DataAccessors
{
    public interface IUnitOfWork : IDisposable
    {
        ITransactionRepository Transactions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
