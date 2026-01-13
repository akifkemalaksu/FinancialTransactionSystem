using LedgerService.Domain.Constants;

namespace LedgerService.Application.Features.LedgerFeatures.CreateLedger
{
    public record CreateLedgerCommand
    {
        public Guid TransactionId { get; set; }

        public required string AccountNumber { get; set; }

        public decimal Amount { get; set; }

        public required string Currency { get; set; }

        public decimal BalanceAfter { get; set; }

        public TransactionDirection Type { get; set; }

        public string? Description { get; set; }
    }
}
