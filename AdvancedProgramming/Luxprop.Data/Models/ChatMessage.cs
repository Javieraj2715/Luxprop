using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class ChatMessage
{
    public int ChatMessageId { get; set; }

    public int ChatThreadId { get; set; }

    public string Sender { get; set; } = null!;

    public string Text { get; set; } = null!;

    public DateTime SentUtc { get; set; }

    public virtual ChatThread ChatThread { get; set; } = null!;
}
