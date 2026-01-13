namespace AccountService.Application.Dtos.Clients
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
