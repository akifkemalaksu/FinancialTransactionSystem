using AccountService.Application.Dtos.AccountDtos;

namespace AccountService.Application.Features.AccountFeatures.GetAccountById
{
    public record GetAccountByIdQueryResult
    {
        public required AccountDto Account { get; set; }
    }
}
