using AccountService.Application.Features.AccountFeatures.GetAccountByNumber;
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
        [HttpGet("by-account-number/{accountNumber}")]
        public async Task<IActionResult> GetAccountByNumberAsync(string accountNumber)
        {
            var getAccountByNumberQuery = new GetAccountByNumberQuery
            {
                AccountNumber = accountNumber
            };
            var response = await _queryDispatcher.DispatchAsync<GetAccountByNumberQuery, ApiResponse<GetAccountByNumberQueryResult>>(getAccountByNumberQuery);
            return CreateResult(response);
        }
    }
}
