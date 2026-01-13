using LedgerService.Domain.Entities;

namespace LedgerService.Application.Services.DataAccessors
{
    public interface ILedgerRepository
    {
        void Add(Ledger ledger);
    }
}
