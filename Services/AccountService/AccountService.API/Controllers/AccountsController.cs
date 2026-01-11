using AccountService.Application.Features.AccountFeatures.GetAccountById;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(
        IQueryDispatcher _queryDispatcher
    ) : ApiControllerBase
    {
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccountByIdAsync(Guid accountId)
        {
            var getAccountByIdQuery = new GetAccountByIdQuery
            {
                AccountId = accountId
            };
            var response = await _queryDispatcher.DispatchAsync<GetAccountByIdQuery, ApiResponse<GetAccountByIdQueryResult>>(getAccountByIdQuery);
            return CreateResult(response);
        }
    }
}
