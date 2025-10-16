using System.ComponentModel.DataAnnotations;

namespace Luxprop.Models.Chat
{
    public class ChatInitModel
    {
        [Required]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string ClientEmail { get; set; } = string.Empty;
    }
}
