using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class Documento
{
    public int DocumentoId { get; set; }

    public string? Nombre { get; set; }

    public string? TipoDocumento { get; set; }

    public string? Estado { get; set; }

    public DateOnly? FechaCarga { get; set; }

    public int? ExpedienteId { get; set; }

    public string? UrlArchivo { get; set; }

    public string? Etiquetas { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public int? UsuarioId { get; set; }

    public virtual ICollection<AlertaVencimiento> AlertaVencimientos { get; set; } = new List<AlertaVencimiento>();

    public virtual ICollection<AlertasDocumento> AlertasDocumentos { get; set; } = new List<AlertasDocumento>();

    public virtual Expediente? Expediente { get; set; }
}
