namespace AccountService.Application.Features.AccountFeatures.GetAccountById
{
    public record GetAccountByIdQuery
    {
        public Guid AccountId { get; init; }
    }
}
