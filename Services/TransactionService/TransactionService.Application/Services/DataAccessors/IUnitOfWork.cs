namespace TransactionService.Application.Services.DataAccessors
{
    public interface IUnitOfWork
    {
        ITransferRepository Transfers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
