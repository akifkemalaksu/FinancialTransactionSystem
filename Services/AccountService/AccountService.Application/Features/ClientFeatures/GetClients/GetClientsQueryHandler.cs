using AccountService.Application.Services.DataAccessors;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.ClientFeatures.GetClients
{
    public class GetClientsQueryHandler(
        IUnitOfWork _unitOfWork
    ) : IQueryHandler<GetClientsQuery, ApiResponse<GetClientsQueryResult>>
    {
        public async Task<ApiResponse<GetClientsQueryResult>> HandleAsync(GetClientsQuery query, CancellationToken cancellationToken = default)
        {
            var clients = await _unitOfWork.Clients.GetAllAsync(cancellationToken);

            return ApiResponse<GetClientsQueryResult>.Success(
                StatusCodes.Status200OK,
                new GetClientsQueryResult { Clients = clients }
            );
        }
    }
}
