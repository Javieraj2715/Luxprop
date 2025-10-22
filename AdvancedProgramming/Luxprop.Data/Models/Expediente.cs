using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Expediente
{
    public int ExpedienteId { get; set; }

    public string? TipoOcupacion { get; set; }

    public string? Estado { get; set; }

    public int? PropiedadId { get; set; }

    public int? ClienteId { get; set; }

    public DateOnly? FechaApertura { get; set; }

    public DateOnly? FechaCierre { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual Propiedad? Propiedad { get; set; }

    public virtual ICollection<TareaTramite> TareaTramites { get; set; } = new List<TareaTramite>();

    public virtual ICollection<HistorialExpediente>? HistorialExpedientes { get; set; }


}
