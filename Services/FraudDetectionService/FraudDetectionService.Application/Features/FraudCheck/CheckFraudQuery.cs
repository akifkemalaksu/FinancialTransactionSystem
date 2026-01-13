namespace FraudDetectionService.Application.Features.FraudCheck
{
    public record CheckFraudQuery
    {
        public required string SourceAccountNumber { get; init; }
        public string? DestinationAccountNumber { get; init; }
        public decimal Amount { get; init; }
        public required string Currency { get; init; }
    }
}
