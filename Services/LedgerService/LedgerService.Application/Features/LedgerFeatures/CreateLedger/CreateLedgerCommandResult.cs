namespace LedgerService.Application.Features.LedgerFeatures.CreateLedger
{
    public record CreateLedgerCommandResult
    {
        public Guid LedgerId { get; set; }
    }
}
