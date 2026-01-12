using System.ComponentModel.DataAnnotations;
using TransactionService.Domain.Constants;

namespace TransactionService.Domain.Entities
{
    public class Transfer
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(20)]
        public required string SourceAccountNumber { get; set; }

        [StringLength(20)]
        public required string DestinationAccountNumber { get; set; }

        public decimal Amount { get; set; }

        [StringLength(3)]
        public required string Currency { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionStatusEnum Status { get; set; } = TransactionStatusEnum.Pending;

        [StringLength(255)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? IdempotencyKey { get; set; }
    }
}
