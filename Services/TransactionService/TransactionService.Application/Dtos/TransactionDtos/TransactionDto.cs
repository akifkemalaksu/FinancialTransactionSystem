using TransactionService.Domain.Constants;

namespace TransactionService.Application.Dtos.TransactionDtos
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string SourceAccountNumber { get; set; } = string.Empty;
        public string? DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public TransactionStatusEnum Status { get; set; }
        public string? Description { get; set; }
    }
}
