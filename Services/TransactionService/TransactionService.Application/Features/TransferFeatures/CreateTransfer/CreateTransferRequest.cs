using ServiceDefaults.Enums;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Application.Features.TransferFeatures.CreateTransfer
{
    public record CreateTransferRequest
    {
        [Required]
        [StringLength(50)]
        public required string SourceAccountNumber { get; init; }

        [StringLength(50)]
        public string? DestinationAccountNumber { get; init; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; init; }

        public string? Description { get; init; }

        public TransactionType? Type { get; init; }
    }
}
