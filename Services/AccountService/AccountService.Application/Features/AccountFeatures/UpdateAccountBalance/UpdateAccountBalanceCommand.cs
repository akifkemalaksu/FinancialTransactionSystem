namespace AccountService.Application.Features.AccountFeatures.UpdateAccountBalance
{
    public record UpdateAccountBalanceCommand
    {
        public required string AccountNumber { get; init; }
        public decimal Amount { get; init; }
    }
}
