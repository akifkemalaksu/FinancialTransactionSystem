namespace TransactionService.Application.Dtos.Clients.FraudDetection
{
    public record FraudCheckResponse
    {
        public bool IsFraudulent { get; init; }
        public string? Reason { get; init; }
    }
}
