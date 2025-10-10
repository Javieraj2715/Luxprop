using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class AlertaVencimiento
{
    public int AlertaId { get; set; }

    public int DocumentoId { get; set; }

    public DateOnly? FechaProgramada { get; set; }

    public string? Tipo { get; set; }

    public string? Estado { get; set; }

    public virtual Documento Documento { get; set; } = null!;
}
