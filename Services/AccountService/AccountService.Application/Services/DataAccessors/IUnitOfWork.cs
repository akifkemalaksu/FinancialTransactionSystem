namespace AccountService.Application.Services.DataAccessors
{
    public interface IUnitOfWork: IDisposable
    {
        IAccountRepository Accounts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
