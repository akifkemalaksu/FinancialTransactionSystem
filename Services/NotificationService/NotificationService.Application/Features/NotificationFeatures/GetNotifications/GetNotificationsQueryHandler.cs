using Microsoft.AspNetCore.Http;
using NotificationService.Application.Services.DataAccessors;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace NotificationService.Application.Features.NotificationFeatures.GetNotifications
{
    public class GetNotificationsQueryHandler(
        IUnitOfWork _unitOfWork
    ) : IQueryHandler<GetNotificationsQuery, ApiResponse<GetNotificationsQueryResult>>
    {
        public async Task<ApiResponse<GetNotificationsQueryResult>> HandleAsync(GetNotificationsQuery query, CancellationToken cancellationToken = default)
        {
            var notifications = await _unitOfWork.Notifications.GetAllAsync(cancellationToken);

            var result = new GetNotificationsQueryResult
            {
                Notifications = notifications
            };

            return ApiResponse<GetNotificationsQueryResult>.Success(
                StatusCodes.Status200OK,
                result
            );
        }
    }
}
