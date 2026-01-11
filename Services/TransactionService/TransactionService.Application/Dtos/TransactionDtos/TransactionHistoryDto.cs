using TransactionService.Application.Dtos.TransactionDtos;

namespace TransactionService.Application.Dtos.TransactionDtos
{
    public class TransactionHistoryDto
    {
        public List<TransactionDto> IncomingTransactions { get; set; } = [];
        public List<TransactionDto> OutgoingTransactions { get; set; } = [];
    }
}
