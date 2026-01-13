using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using MassTransit;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.API.Consumers
{
    public class TransferCreatedConsumer(
        ICommandDispatcher _commandDispatcher
    ) : IConsumer<TransferCreatedEvent>
    {
        public async Task Consume(ConsumeContext<TransferCreatedEvent> context)
        {
            var deductCommand = new UpdateAccountBalanceCommand
            {
                AccountNumber = context.Message.SourceAccountNumber,
                Amount = -context.Message.Amount
            };
            await _commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(deductCommand, context.CancellationToken);
            
            if(context.Message.DestinationAccountNumber is null)
            {
                return;
            }
            
            var addCommand = new UpdateAccountBalanceCommand
            {
                AccountNumber = context.Message.DestinationAccountNumber,
                Amount = context.Message.Amount
            };
            await _commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(addCommand, context.CancellationToken);
        }
    }
}