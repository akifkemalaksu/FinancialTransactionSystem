using ServiceDefaults.Enums;
using TransactionService.Domain.Constants;

namespace TransactionService.Application.Dtos.Transfers
{
    public record TransferDto
    {
        public Guid Id { get; set; }
        public string SourceAccountNumber { get; set; } = string.Empty;
        public string? DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public TransactionStatusEnum Status { get; set; }
        public TransactionType Type { get; set; }
        public string? Description { get; set; }
    }
}
