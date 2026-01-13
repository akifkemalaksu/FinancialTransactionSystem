namespace AccountService.Application.Features.AccountFeatures.CreateAccount
{
    public record CreateAccountCommand
    {
        public Guid ClientId { get; init; }

        public required string Currency { get; init; }
    }
}
