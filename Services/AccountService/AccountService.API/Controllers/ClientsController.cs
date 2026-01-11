using AccountService.Application.Features.AccountFeatures.GetAccountsByClientId;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(
        IQueryDispatcher _queryDispatcher
    ) : ApiControllerBase
    {
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
