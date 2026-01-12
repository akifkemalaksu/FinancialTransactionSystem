using FraudDetectionService.Application.Features.FraudCheck;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace FraudDetectionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudCheckController(
        IQueryDispatcher _queryDispatcher
    ) : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CheckFraud([FromBody] CheckFraudQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<CheckFraudQuery, ApiResponse<CheckFraudQueryResult>>(query);
            return CreateResult(result);
        }
    }
}
