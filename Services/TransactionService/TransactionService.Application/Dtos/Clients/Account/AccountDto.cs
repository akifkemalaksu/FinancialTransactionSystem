namespace TransactionService.Application.Dtos.Clients.Account
{
    public record AccountDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public required string AccountNumber { get; set; }

        public required string Currency { get; set; }

        public decimal Balance { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}