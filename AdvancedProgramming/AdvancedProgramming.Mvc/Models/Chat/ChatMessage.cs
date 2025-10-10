using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedProgramming.Mvc.Models.Chat
{
    [Table("ChatMessages")]
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public int ChatThreadId { get; set; }
        public string Sender { get; set; }   // Client | Agent | Bot
        public string Text { get; set; }
        public DateTime SentUtc { get; set; } = DateTime.UtcNow;

        [ForeignKey("ChatThreadId")]
        public virtual ChatThread Thread { get; set; }
    }
}
