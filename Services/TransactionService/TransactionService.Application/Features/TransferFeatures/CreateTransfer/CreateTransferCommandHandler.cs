using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;
using TransactionService.Application.Dtos.Clients.Account;
using TransactionService.Application.Dtos.Clients.FraudDetection;
using TransactionService.Application.Services.Clients;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Domain.Constants;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Features.TransferFeatures.CreateTransfer
{
    public class CreateTransferCommandHandler(
        IUnitOfWork _unitOfWork,
        IAccountService _accountService,
        IFraudDetectionService _fraudDetectionService,
        IKafkaProducer _kafkaProducer
    ) : ICommandHandler<CreateTransferCommand, ApiResponse<CreateTransferCommandResult>>
    {
        public async Task<ApiResponse<CreateTransferCommandResult>> HandleAsync(CreateTransferCommand command, CancellationToken cancellationToken = default)
        {
            var idempotencyResult = await CheckIdempotencyAsync(command.IdempotencyKey, cancellationToken);
            if (idempotencyResult is not null) return idempotencyResult;

            var (sourceValidationResult, sourceAccount) = await ValidateAndGetSourceAccountAsync(command, cancellationToken);
            if (sourceValidationResult is not null) return sourceValidationResult;

            if (sourceAccount is null)
            {
                return ApiResponse<CreateTransferCommandResult>.Failure(
                    StatusCodes.Status500InternalServerError,
                    "An error occurred while retrieving source account details."
                );
            }

            var currency = sourceAccount.Currency;

            var fraudCheckResult = await CheckFraudAsync(command, currency, cancellationToken);
            if (fraudCheckResult is not null) return fraudCheckResult;

            var destinationValidationResult = await ValidateDestinationAccountAsync(command, currency, cancellationToken);
            if (destinationValidationResult is not null) return destinationValidationResult;

            var transfer = await CreateTransferAsync(command, currency, cancellationToken);

            await PublishTransferEventAsync(transfer, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<CreateTransferCommandResult>.Success(
                StatusCodes.Status201Created,
                new CreateTransferCommandResult
                {
                    TransactionId = transfer.Id,
                    Status = transfer.Status.ToString()
                }
            );
        }

        private async Task<ApiResponse<CreateTransferCommandResult>?> CheckIdempotencyAsync(string idempotencyKey, CancellationToken cancellationToken)
        {
            var existingTransfer = await _unitOfWork.Transfers.GetByIdempotencyKeyAsync(idempotencyKey, cancellationToken);
            if (existingTransfer is not null)
            {
                return ApiResponse<CreateTransferCommandResult>.Success(
                    StatusCodes.Status200OK,
                    new CreateTransferCommandResult
                    {
                        TransactionId = existingTransfer.Id,
                        Status = existingTransfer.Status.ToString()
                    }
                );
            }
            return null;
        }

        private async Task<(ApiResponse<CreateTransferCommandResult>? Result, AccountDto? Account)> ValidateAndGetSourceAccountAsync(CreateTransferCommand command, CancellationToken cancellationToken)
        {
            var sourceAccount = await _accountService.GetByAccountNumberAsync(command.SourceAccountNumber, cancellationToken);
            if (sourceAccount is null)
            {
                return (ApiResponse<CreateTransferCommandResult>.Failure(
                    StatusCodes.Status404NotFound,
                    $"Source account {command.SourceAccountNumber} not found"
                ), null);
            }

            if (string.IsNullOrEmpty(command.DestinationAccountNumber))
            {
                if (command.Amount == 0)
                {
                    return (ApiResponse<CreateTransferCommandResult>.Failure(
                        StatusCodes.Status400BadRequest,
                        "Amount cannot be zero"
                    ), null);
                }

                if (command.Amount < 0)
                {
                    if (sourceAccount.Balance < Math.Abs(command.Amount))
                    {
                        return (ApiResponse<CreateTransferCommandResult>.Failure(
                            StatusCodes.Status400BadRequest,
                            "Insufficient funds for withdrawal"
                        ), null);
                    }
                }
            }
            else
            {
                if (command.Amount <= 0)
                {
                    return (ApiResponse<CreateTransferCommandResult>.Failure(
                        StatusCodes.Status400BadRequest,
                        "Transfer amount must be positive"
                    ), null);
                }

                if (sourceAccount.Balance < command.Amount)
                {
                    return (ApiResponse<CreateTransferCommandResult>.Failure(
                        StatusCodes.Status400BadRequest,
                        "Insufficient funds"
                    ), null);
                }
            }

            return (null, sourceAccount);
        }

        private async Task<ApiResponse<CreateTransferCommandResult>?> CheckFraudAsync(CreateTransferCommand command, string currency, CancellationToken cancellationToken)
        {
            var fraudCheckRequest = new FraudCheckRequest
            {
                Amount = command.Amount,
                Currency = currency,
                SourceAccountNumber = command.SourceAccountNumber,
                DestinationAccountNumber = command.DestinationAccountNumber
            };

            var isFraudulent = await _fraudDetectionService.IsFraudulentAsync(fraudCheckRequest, cancellationToken);
            if (isFraudulent)
            {
                return ApiResponse<CreateTransferCommandResult>.Failure(
                    StatusCodes.Status400BadRequest,
                    "Transaction rejected due to suspected fraud."
                );
            }
            return null;
        }

        private async Task<ApiResponse<CreateTransferCommandResult>?> ValidateDestinationAccountAsync(CreateTransferCommand command, string currency, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(command.DestinationAccountNumber))
            {
                return null;
            }

            if (command.SourceAccountNumber == command.DestinationAccountNumber)
            {
                return ApiResponse<CreateTransferCommandResult>.Failure(
                    StatusCodes.Status400BadRequest,
                    "Source and destination accounts cannot be the same"
                );
            }

            var destinationAccount = await _accountService.GetByAccountNumberAsync(command.DestinationAccountNumber, cancellationToken);
            if (destinationAccount is null)
            {
                return ApiResponse<CreateTransferCommandResult>.Failure(
                    StatusCodes.Status404NotFound,
                    $"Destination account {command.DestinationAccountNumber} not found"
                );
            }

            if (destinationAccount.Currency != currency)
            {
                return ApiResponse<CreateTransferCommandResult>.Failure(
                    StatusCodes.Status400BadRequest,
                    $"Currency mismatch. Destination account currency is {destinationAccount.Currency}, but transfer currency is {currency}"
                );
            }

            return null;
        }

        private async Task<Transfer> CreateTransferAsync(CreateTransferCommand command, string currency, CancellationToken cancellationToken)
        {
            var transfer = new Transfer
            {
                Id = Guid.NewGuid(),
                SourceAccountNumber = command.SourceAccountNumber,
                DestinationAccountNumber = command.DestinationAccountNumber,
                Amount = command.Amount,
                Currency = currency,
                TransactionDate = DateTime.UtcNow,
                Status = TransactionStatusEnum.Pending,
                Description = command.Description,
                IdempotencyKey = command.IdempotencyKey
            };

            await _unitOfWork.Transfers.CreateAsync(transfer, cancellationToken);

            return transfer;
        }

        private async Task PublishTransferEventAsync(Transfer transfer, CancellationToken cancellationToken)
        {
            var eventMessage = new TransferCreatedEvent
            {
                TransactionId = transfer.Id,
                SourceAccountNumber = transfer.SourceAccountNumber,
                DestinationAccountNumber = transfer.DestinationAccountNumber,
                Amount = transfer.Amount,
                Currency = transfer.Currency,
                TransactionDate = transfer.TransactionDate,
                Description = transfer.Description
            };
            await _kafkaProducer.ProduceAsync(eventMessage, cancellationToken);
        }
    }
}
