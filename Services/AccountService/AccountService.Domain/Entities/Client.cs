using System.ComponentModel.DataAnnotations;

namespace AccountService.Domain.Entities
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [StringLength(100)]
        public required string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
