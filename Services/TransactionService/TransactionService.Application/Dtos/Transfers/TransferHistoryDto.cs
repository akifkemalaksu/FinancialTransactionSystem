namespace TransactionService.Application.Dtos.Transfers
{
    public record TransferHistoryDto
    {
        public List<TransferDto> IncomingTransfers { get; set; } = [];
        public List<TransferDto> OutgoingTransfers { get; set; } = [];
    }
}
