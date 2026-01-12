using Microsoft.AspNetCore.Http;
using NotificationService.Application.Services.DataAccessors;
using NotificationService.Domain.Entities;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace NotificationService.Application.Features.NotificationFeatures.CreateNotification
{
    public class CreateNotificationCommandHandler(
        IUnitOfWork _unitOfWork
    ) : ICommandHandler<CreateNotificationCommand, ApiResponse<CreateNotificationCommandResult>>
    {
        public async Task<ApiResponse<CreateNotificationCommandResult>> HandleAsync(CreateNotificationCommand command, CancellationToken cancellationToken = default)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                Message = command.Message
            };

            await _unitOfWork.Notifications.AddAsync(notification, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<CreateNotificationCommandResult>.Success(
                StatusCodes.Status201Created,
                new CreateNotificationCommandResult
                {
                    NotificationId = notification.Id
                }
            );
        }
    }
}
