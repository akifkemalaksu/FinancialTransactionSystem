using System.ComponentModel.DataAnnotations;

namespace TransactionService.Application.Features.TransferFeatures.CreateTransfer
{
    public record CreateTransferRequest
    {
        [Required]
        [StringLength(20)]
        public required string SourceAccountNumber { get; init; }

        [Required]
        [StringLength(20)]
        public required string DestinationAccountNumber { get; init; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; init; }

        public string? Description { get; init; }
    }
}
