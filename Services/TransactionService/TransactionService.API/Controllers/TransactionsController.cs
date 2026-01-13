using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;
using TransactionService.Application.Features.TransferFeatures.CreateTransfer;

namespace TransactionService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(
        ICommandDispatcher _commandDispatcher
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

            var command = new CreateTransferCommand
            {
                IdempotencyKey = idempotencyKey,
                SourceAccountNumber = request.SourceAccountNumber,
                DestinationAccountNumber = request.DestinationAccountNumber,
                Amount = request.Amount,
                Description = request.Description
            };

            var result = await _commandDispatcher.DispatchAsync<CreateTransferCommand, ApiResponse<CreateTransferCommandResult>>(command);

            return CreateResult(result);
        }
    }
}
