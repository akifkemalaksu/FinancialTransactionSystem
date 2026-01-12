using AccountService.Application.Dtos.Accounts;

namespace AccountService.Application.Features.AccountFeatures.GetAccountById
{
    public record GetAccountByIdQueryResult
    {
        public required AccountDto Account { get; set; }
    }
}
