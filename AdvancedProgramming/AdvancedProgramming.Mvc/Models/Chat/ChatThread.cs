using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProgramming.Mvc.Models.Chat
{
    [Table("ChatThreads")]
    public class ChatThread
    {
        public int ChatThreadId { get; set; }
        public string State { get; set; } = "Open";   // Open | Closed
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? ClosedUtc { get; set; }
        public bool NeedsAgent { get; set; } = false;

        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
