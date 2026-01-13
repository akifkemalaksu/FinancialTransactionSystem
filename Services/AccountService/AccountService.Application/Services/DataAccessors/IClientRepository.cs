using AccountService.Application.Dtos.Clients;
using AccountService.Domain.Entities;

namespace AccountService.Application.Services.DataAccessors
{
    public interface IClientRepository
    {
        void Add(Client client);
        Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
