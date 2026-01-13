using LedgerService.Application.Services.DataAccessors;
using LedgerService.Domain.Entities;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.Features.LedgerFeatures.CreateLedger
{
    public class CreateLedgerCommandHandler(
        IUnitOfWork _unitOfWork
    ) : ICommandHandler<CreateLedgerCommand, ApiResponse<CreateLedgerCommandResult>>
    {
        public async Task<ApiResponse<CreateLedgerCommandResult>> HandleAsync(CreateLedgerCommand command, CancellationToken cancellationToken = default)
        {
            var ledger = new Ledger
            {
                TransactionId = command.TransactionId,
                AccountNumber = command.AccountNumber,
                Amount = command.Amount,
                Currency = command.Currency,
                BalanceAfter = command.BalanceAfter,
                Type = command.Type,
                Description = command.Description,
                CreatedAt = DateTime.UtcNow,
                Id = Guid.NewGuid()
            };

            _unitOfWork.Ledgers.Add(ledger);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<CreateLedgerCommandResult>.Success(
                statusCode: 201,
                data: new CreateLedgerCommandResult
                {
                    LedgerId = ledger.Id
                },
                message: "Ledger created successfully."
            );
        }
    }
}
