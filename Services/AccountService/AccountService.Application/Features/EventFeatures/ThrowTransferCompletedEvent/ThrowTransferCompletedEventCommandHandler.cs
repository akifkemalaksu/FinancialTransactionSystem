using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.EventFeatures.ThrowTransferCompletedEvent
{
    public class ThrowTransferCompletedEventCommandHandler(
        IKafkaProducer _kafkaProducer
    ) : ICommandHandler<ThrowTransferCompletedEventCommand, ApiResponse<ThrowTransferCompletedEventCommandResult>>
    {
        public async Task<ApiResponse<ThrowTransferCompletedEventCommandResult>> HandleAsync(ThrowTransferCompletedEventCommand command, CancellationToken cancellationToken = default)
        {
            var eventMessage = new TransferCompletedEvent
            {
                TransactionId = command.TransactionId,
                SourceAccountNumber = command.SourceAccountNumber,
                DestinationAccountNumber = command.DestinationAccountNumber,
                Amount = command.Amount,
                Currency = command.Currency,
                Description = command.Description,
                Type = command.Type
            };
            await _kafkaProducer.ProduceAsync(eventMessage, cancellationToken);

            var result = new ThrowTransferCompletedEventCommandResult
            {
                EventId = eventMessage.Id
            };

            return ApiResponse<ThrowTransferCompletedEventCommandResult>.Success(200, result, "TransferCompletedEvent thrown successfully.");
        }
    }
}
