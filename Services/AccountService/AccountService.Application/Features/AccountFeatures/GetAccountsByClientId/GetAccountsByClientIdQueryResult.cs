using AccountService.Application.Dtos.Accounts;

namespace AccountService.Application.Features.AccountFeatures.GetAccountsByClientId
{
    public record GetAccountsByClientIdQueryResult
    {
        public List<AccountDto>? Accounts { get; set; }
    }
}
