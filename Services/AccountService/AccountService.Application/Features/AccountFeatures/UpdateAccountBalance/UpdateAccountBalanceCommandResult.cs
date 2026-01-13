namespace AccountService.Application.Features.AccountFeatures.UpdateAccountBalance
{
    public record UpdateAccountBalanceCommandResult
    {
        public bool IsSuccess { get; init; }
        public decimal NewBalance { get; init; }
    }
}
