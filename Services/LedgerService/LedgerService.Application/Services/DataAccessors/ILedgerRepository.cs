using LedgerService.Domain.Entities;

namespace LedgerService.Application.Services.DataAccessors
{
    public interface ILedgerRepository
    {
        void Add(Ledger ledger);

        Task<List<Ledger>> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    }
}
