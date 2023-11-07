using System.ComponentModel.DataAnnotations;

namespace EasyControl.Api.Domain.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
