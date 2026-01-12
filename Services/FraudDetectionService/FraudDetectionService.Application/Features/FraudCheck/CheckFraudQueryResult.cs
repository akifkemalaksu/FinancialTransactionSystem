namespace FraudDetectionService.Application.Features.FraudCheck
{
    public record CheckFraudQueryResult
    {
        public bool IsFraudulent { get; init; }
        public string? Reason { get; init; }
    }
}
