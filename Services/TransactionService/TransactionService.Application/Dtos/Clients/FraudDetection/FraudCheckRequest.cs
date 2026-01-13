namespace TransactionService.Application.Dtos.Clients.FraudDetection
{
    public record FraudCheckRequest
    {
        public required string SourceAccountNumber { get; init; }
        public string? DestinationAccountNumber { get; init; }
        public decimal Amount { get; init; }
        public required string Currency { get; init; }
    }
}
