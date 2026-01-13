namespace AccountService.Application.Features.AccountFeatures.CreateAccount
{
    public record CreateAccountCommandResult
    {
        public Guid Id { get; init; }
        public required string AccountNumber { get; init; }
    }
}
