using LedgerService.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace LedgerService.Domain.Entities
{
    public class Ledger
    {
        public Guid Id { get; set; }

        public Guid TransactionId { get; set; }

        [StringLength(50)]
        public required string AccountNumber { get; set; }

        public decimal Amount { get; set; }

        [StringLength(3)]
        public required string Currency { get; set; }

        public decimal BalanceAfter { get; set; }

        public TransactionDirection Type { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
