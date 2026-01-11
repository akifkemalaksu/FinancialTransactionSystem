using AccountService.Application.Dtos.AccountDtos;

namespace AccountService.Application.Features.AccountFeatures.GetAccountsByClientId
{
    public record GetAccountsByClientIdQueryResult
    {
        public List<AccountDto>? Accounts { get; set; }
    }
}
