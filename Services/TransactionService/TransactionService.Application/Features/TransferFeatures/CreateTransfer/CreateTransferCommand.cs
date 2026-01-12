using System.ComponentModel.DataAnnotations;

namespace TransactionService.Application.Features.TransferFeatures.CreateTransfer
{
    public record CreateTransferCommand
    {
        public required string SourceAccountNumber { get; init; }

        public required string DestinationAccountNumber { get; init; }

        public decimal Amount { get; init; }

        public string? Description { get; init; }

        [Required]
        [StringLength(255)]
        public required string IdempotencyKey { get; init; }
    }
}
