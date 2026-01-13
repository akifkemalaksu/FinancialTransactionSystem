using AccountService.Domain.Entities;
using AccountService.Application.Dtos.Clients;

namespace AccountService.Application.Services.DataAccessors
{
    public interface IClientRepository
    {
        Task AddAsync(Client client, CancellationToken cancellationToken = default);
        Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
