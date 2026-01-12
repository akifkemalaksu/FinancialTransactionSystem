using System.ComponentModel.DataAnnotations;

namespace NotificationService.Domain.Entities
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(256)]
        public required string Title { get; set; }

        [StringLength(1024)]
        public required string Message { get; set; }
    }
}
