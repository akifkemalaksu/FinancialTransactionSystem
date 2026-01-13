using AccountService.Application.Features.AccountFeatures.CreateAccount;
using AccountService.Application.Features.AccountFeatures.GetAccountById;
using AccountService.Application.Features.AccountFeatures.GetAccountByNumber;
using AccountService.Application.Features.AccountFeatures.GetAccountsByClientId;
using Microsoft.AspNetCore.Mvc;
using ServiceDefaults.Controllers;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController(
        IQueryDispatcher _queryDispatcher,
        ICommandDispatcher _commandDispatcher
    ) : ApiControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountCommand command)
        {
            var response = await _commandDispatcher.DispatchAsync<CreateAccountCommand, ApiResponse<CreateAccountCommandResult>>(command);
            return CreateResult(response);
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccountByNumberAsync(Guid accountId)
        {
            var getAccountByIdQuery = new GetAccountByIdQuery
            {
                AccountId = accountId
            };
            var response = await _queryDispatcher.DispatchAsync<GetAccountByIdQuery, ApiResponse<GetAccountByIdQueryResult>>(getAccountByIdQuery);
            return CreateResult(response);
        }

        [HttpGet("by-account-number/{accountNumber}")]
        public async Task<IActionResult> GetAccountByNumberAsync(string accountNumber)
        {
            var getAccountByNumberQuery = new GetAccountByNumberQuery
            {
                AccountNumber = accountNumber
            };
            var response = await _queryDispatcher.DispatchAsync<GetAccountByNumberQuery, ApiResponse<GetAccountByNumberQueryResult>>(getAccountByNumberQuery);
            return CreateResult(response);
        }
    }
}
