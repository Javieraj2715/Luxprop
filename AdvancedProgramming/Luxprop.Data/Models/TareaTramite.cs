using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class TareaTramite
{
    public int TareaId { get; set; }

    public int ExpedienteId { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public string? Prioridad { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaCierre { get; set; }

    public DateOnly? FechaCompromiso { get; set; }

    public virtual Expediente Expediente { get; set; } = null!;
}
