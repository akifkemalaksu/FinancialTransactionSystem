namespace AccountService.Application.Features.AccountFeatures.GetAccountByNumber
{
    public record GetAccountByNumberQuery
    {
        public required string AccountNumber { get; init; }
    }
}
