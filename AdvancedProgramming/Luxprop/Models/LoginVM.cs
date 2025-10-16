using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models
{
    public class LoginVM
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
