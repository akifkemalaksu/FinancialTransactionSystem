using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.EventHandlers
{
    public class TransferCreatedHandler(
        ICommandDispatcher _commandDispatcher
    ) : IKafkaHandler<TransferCreatedEvent>
    {
        public async Task HandleAsync(TransferCreatedEvent message)
        {
            var deductCommand = new UpdateAccountBalanceCommand
            {
                AccountNumber = message.SourceAccountNumber,
                Amount = -message.Amount
            };
            await _commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(deductCommand);

            if (message.DestinationAccountNumber is null)
            {
                return;
            }

            var addCommand = new UpdateAccountBalanceCommand
            {
                AccountNumber = message.DestinationAccountNumber,
                Amount = message.Amount
            };
            await _commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(addCommand);
        }
    }
}
