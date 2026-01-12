namespace TransactionService.Application.Services.DataAccessors
{
    public interface IUnitOfWork : IDisposable
    {
        ITransferRepository Transfers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
