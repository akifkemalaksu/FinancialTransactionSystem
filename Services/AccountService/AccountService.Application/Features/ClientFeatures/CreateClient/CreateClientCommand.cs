namespace AccountService.Application.Features.ClientFeatures.CreateClient
{
    public record CreateClientCommand
    {
        public required string Name { get; init; }
        public required string Surname { get; init; }
        public required string Email { get; init; }
    }
}
