using LedgerService.Application.Features.LedgerFeatures.GetLedgersByAccountNumber;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace LedgerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedgersController(
        IQueryDispatcher _queryDispatcher
    ) : ApiControllerBase
    {
        [HttpGet("by-account-number/{accountNumber}")]
        public async Task<IActionResult> GetByAccountNumber(string accountNumber, CancellationToken cancellationToken)
        {
            var query = new GetLedgersByAccountNumberQuery
            {
                AccountNumber = accountNumber
            };

            var result = await _queryDispatcher.DispatchAsync<GetLedgersByAccountNumberQuery, ApiResponse<GetLedgersByAccountNumberQueryResult>>(query, cancellationToken);

            return CreateResult(result);
        }
    }
}

