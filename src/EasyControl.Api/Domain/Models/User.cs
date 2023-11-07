using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace EasyControl.Api.Domain.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "The name field is mandatory.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "The e-mail field is mandatory.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "The password field is mandatory.")]
        public string Password { get; set; } = string.Empty;
    }
}
