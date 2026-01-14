using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountService.Domain.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        [StringLength(50)]
        public required string AccountNumber { get; set; }

        [StringLength(3)]
        public required string Currency { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; } = null!;
    }
}
