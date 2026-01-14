using LedgerService.Domain.Entities;

namespace LedgerService.Application.Features.LedgerFeatures.GetLedgersByAccountNumber
{
    public record GetLedgersByAccountNumberQueryResult
    {
        public required List<Ledger> Ledgers { get; init; }
    }
}

