using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class HistorialExpediente
{
    public int HistorialId { get; set; }

    public int ExpedienteId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime FechaModificacion { get; set; }

    public string? Descripcion { get; set; }

    public string? EstadoNuevo { get; set; }

    public string? EstadoAnterior { get; set; }

    public string? Ipregistro { get; set; }

    public string? TipoAccion { get; set; }

    public string? Observacion { get; set; }

    public virtual Expediente Expediente { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
