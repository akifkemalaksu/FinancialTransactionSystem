using AccountService.Application.Services.DataAccessors;
using AccountService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.ClientFeatures.CreateClient
{
    public class CreateClientCommandHandler(
        IUnitOfWork _unitOfWork
    ) : ICommandHandler<CreateClientCommand, ApiResponse<CreateClientCommandResult>>
    {
        public async Task<ApiResponse<CreateClientCommandResult>> HandleAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
        {
            var client = new Client
            {
                Name = command.Name,
                Surname = command.Surname,
                Email = command.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Clients.AddAsync(client, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<CreateClientCommandResult>.Success(
                StatusCodes.Status201Created,
                new CreateClientCommandResult
                {
                    Id = client.Id
                }
            );
        }
    }
}
