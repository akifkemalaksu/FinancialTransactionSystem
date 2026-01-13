using AccountService.Application.Dtos.Clients;
using AccountService.Domain.Entities;

namespace AccountService.Application.Services.DataAccessors
{
    public interface IClientRepository
    {
        Task AddAsync(Client client, CancellationToken cancellationToken = default);
        Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
