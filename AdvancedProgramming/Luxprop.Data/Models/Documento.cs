using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Documento
{
    public int DocumentoId { get; set; }

    public string? Nombre { get; set; }

    public string? TipoDocumento { get; set; }

    public string? Estado { get; set; }

    public DateOnly? FechaCarga { get; set; }

    public int? ExpedienteId { get; set; }
    public string? UrlArchivo { get; set; }

    public virtual ICollection<AlertaVencimiento> AlertaVencimientos { get; set; } = new List<AlertaVencimiento>();

    public virtual Expediente? Expediente { get; set; }
}
