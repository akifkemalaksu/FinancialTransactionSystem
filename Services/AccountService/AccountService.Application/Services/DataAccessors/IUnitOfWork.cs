namespace AccountService.Application.Services.DataAccessors
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }
        IClientRepository Clients { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
