namespace AccountService.Application.Features.AccountFeatures.GetAccountsByClientId
{
    public record GetAccountsByClientIdQuery
    {
        public Guid ClientId { get; set; }
    }
}
