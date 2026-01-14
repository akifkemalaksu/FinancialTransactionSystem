using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;
using NotificationService.Application.Features.NotificationFeatures.GetNotifications;

namespace NotificationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(
        IQueryDispatcher _queryDispatcher
    ) : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotifications(CancellationToken cancellationToken)
        {
            var query = new GetNotificationsQuery();

            var result = await _queryDispatcher.DispatchAsync<GetNotificationsQuery, ApiResponse<GetNotificationsQueryResult>>(query, cancellationToken);

            return CreateResult(result);
        }
    }
}

