using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class Expediente
{
    public int ExpedienteId { get; set; }

    public string? TipoOcupacion { get; set; }

    public string? Estado { get; set; }

    public int? PropiedadId { get; set; }

    public int? ClienteId { get; set; }

    public DateOnly? FechaApertura { get; set; }

    public DateOnly? FechaCierre { get; set; }

    public string? Codigo { get; set; }

    public string? Prioridad { get; set; }

    public string? Categoria { get; set; }

    public string? Descripcion { get; set; }

    public string? Observaciones { get; set; }

    public int? AgenteId { get; set; }

    public DateTime? UltimaActualizacion { get; set; }

    public int? CreadoPorId { get; set; }

    public int? ModificadoPorId { get; set; }

    public virtual Usuario? Agente { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Cliente? Cliente { get; set; }

    public virtual Usuario? CreadoPor { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual ICollection<HistorialExpediente> HistorialExpedientes { get; set; } = new List<HistorialExpediente>();

    public virtual Usuario? ModificadoPor { get; set; }

    public virtual Propiedad? Propiedad { get; set; }

    public virtual ICollection<Recordatorio> Recordatorios { get; set; } = new List<Recordatorio>();

    public virtual ICollection<TareaTramite> TareaTramites { get; set; } = new List<TareaTramite>();
}
