namespace LedgerService.Application.Features.LedgerFeatures.GetLedgersByAccountNumber
{
    public record GetLedgersByAccountNumberQuery
    {
        public required string AccountNumber { get; init; }
    }
}

