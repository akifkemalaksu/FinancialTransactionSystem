using AccountService.Application.Dtos.Accounts;

namespace AccountService.Application.Features.AccountFeatures.GetAccountByNumber
{
    public record GetAccountByNumberQueryResult
    {
        public required AccountDto Account { get; set; }
    }
}
