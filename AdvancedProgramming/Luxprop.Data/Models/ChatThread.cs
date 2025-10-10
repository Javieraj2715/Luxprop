using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class ChatThread
{
    public int ChatThreadId { get; set; }

    public string State { get; set; } = null!;

    public string? ClientName { get; set; }

    public string? ClientEmail { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? ClosedUtc { get; set; }

    public bool NeedsAgent { get; set; }

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
