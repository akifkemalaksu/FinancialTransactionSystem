using AccountService.Application.Dtos.Clients;

namespace AccountService.Application.Features.ClientFeatures.GetClients
{
    public record GetClientsQueryResult
    {
        public required List<ClientDto> Clients { get; init; }
    }
}
