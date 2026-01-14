using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;
using TransactionService.Application.Features.TransferFeatures.CreateTransfer;
using TransactionService.Application.Features.TransferFeatures.GetTransferHistory;

namespace TransactionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("transaction-create")]
    public class TransactionsController(
        ICommandDispatcher _commandDispatcher,
        IQueryDispatcher _queryDispatcher
    ) : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateTransfer(
            [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey,
            [FromBody] CreateTransferRequest request)
        {
            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                return BadRequest("Idempotency key is required.");
            }

            var transactionType = !string.IsNullOrEmpty(request.DestinationAccountNumber)
                ? TransactionType.Transfer
                : (request.Type ?? TransactionType.Withdraw);

            var command = new CreateTransferCommand
            {
                IdempotencyKey = idempotencyKey,
                SourceAccountNumber = request.SourceAccountNumber,
                DestinationAccountNumber = request.DestinationAccountNumber,
                Amount = request.Amount,
                Description = request.Description,
                Type = transactionType
            };

            var result = await _commandDispatcher.DispatchAsync<CreateTransferCommand, ApiResponse<CreateTransferCommandResult>>(command);

            return CreateResult(result);
        }

        [HttpGet("by-account-number/{accountNumber}")]
        public async Task<IActionResult> GetTransferHistoryByAccountNumber(string accountNumber, CancellationToken cancellationToken)
        {
            var query = new GetTransferHistoryQuery
            {
                AccountNumber = accountNumber
            };

            var result = await _queryDispatcher.DispatchAsync<GetTransferHistoryQuery, ApiResponse<GetTransferHistoryQueryResult>>(query, cancellationToken);

            return CreateResult(result);
        }
    }
}
