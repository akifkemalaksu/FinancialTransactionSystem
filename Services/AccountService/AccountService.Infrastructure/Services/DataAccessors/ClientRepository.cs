using AccountService.Application.Dtos.Clients;
using AccountService.Application.Services.DataAccessors;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Services.DataAccessors
{
    public class ClientRepository(AccountDbContext _context) : IClientRepository
    {
        public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
        {
            await _context.Clients.AddAsync(client, cancellationToken);
        }

        public async Task<List<ClientDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    Email = c.Email,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }
    }
}
