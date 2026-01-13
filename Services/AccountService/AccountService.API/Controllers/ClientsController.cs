using AccountService.Application.Features.AccountFeatures.GetAccountsByClientId;
using AccountService.Application.Features.ClientFeatures.CreateClient;
using AccountService.Application.Features.ClientFeatures.GetClients;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(
        IQueryDispatcher _queryDispatcher,
        ICommandDispatcher _commandDispatcher
    ) : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateClientAsync([FromBody] CreateClientCommand command)
        {
            var response = await _commandDispatcher.DispatchAsync<CreateClientCommand, ApiResponse<CreateClientCommandResult>>(command);
            return CreateResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsAsync()
        {
            var query = new GetClientsQuery();
            var response = await _queryDispatcher.DispatchAsync<GetClientsQuery, ApiResponse<GetClientsQueryResult>>(query);
            return CreateResult(response);
        }

        [HttpGet("{clientId}/accounts")]
        public async Task<IActionResult> GetAccountsByClientId(Guid clientId)
        {
            var getAccountsByClientIdQuery = new GetAccountsByClientIdQuery
            {
                ClientId = clientId
            };
            var response = await _queryDispatcher.DispatchAsync<GetAccountsByClientIdQuery, ApiResponse<GetAccountsByClientIdQueryResult>>(getAccountsByClientIdQuery);

            return CreateResult(response);
        }
    }
}
