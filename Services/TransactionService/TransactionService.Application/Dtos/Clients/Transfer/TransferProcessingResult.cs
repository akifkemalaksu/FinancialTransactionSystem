namespace TransactionService.Application.Dtos.Clients.Transfer
{
    public class TransferProcessingResult
    {
        public bool IsSuccessful { get; init; }
        public string? FailureReason { get; init; }
    }
}

