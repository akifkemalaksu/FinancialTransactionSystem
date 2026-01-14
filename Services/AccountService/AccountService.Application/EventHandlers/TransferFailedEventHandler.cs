using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.EventHandlers
{
    public class TransferFailedEventHandler(
        ICommandDispatcher commandDispatcher
    ) : IKafkaHandler<TransferFailedEvent>
    {
        public async Task HandleAsync(TransferFailedEvent message)
        {
            var type = (TransactionType)message.Type;

            if (string.IsNullOrEmpty(message.DestinationAccountNumber))
            {
                await CompensateSingleAccountAsync(message, type);
                return;
            }

            await CompensateTransferAsync(message);
        }

        private async Task CompensateSingleAccountAsync(TransferFailedEvent message, TransactionType type)
        {
            decimal amount;

            if (type == TransactionType.Withdraw)
            {
                amount = message.Amount;
            }
            else
            {
                amount = -message.Amount;
            }

            await UpdateAccountBalanceAsync(message.SourceAccountNumber, amount);
        }

        private async Task CompensateTransferAsync(TransferFailedEvent message)
        {
            var sourceAmount = message.Amount;
            var destinationAmount = -message.Amount;

            await UpdateAccountBalanceAsync(message.SourceAccountNumber, sourceAmount);
            await UpdateAccountBalanceAsync(message.DestinationAccountNumber!, destinationAmount);
        }

        private async Task UpdateAccountBalanceAsync(string accountNumber, decimal amount)
        {
            var command = new UpdateAccountBalanceCommand
            {
                AccountNumber = accountNumber,
                Amount = amount
            };
            var response = await commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(command);

            if (!response.IsSuccess)
            {
                throw new Exception($"Failed to compensate account balance for account {accountNumber}: {response.Message}");
            }
        }
    }
}

