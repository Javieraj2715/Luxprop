using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class Recordatorio
{
    public int RecordatorioId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Tipo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Prioridad { get; set; } = null!;

    public DateTime Inicio { get; set; }

    public DateTime? Fin { get; set; }

    public bool TodoElDia { get; set; }

    public int? PropiedadId { get; set; }

    public int? ExpedienteId { get; set; }

    public bool NotificarCorreo { get; set; }

    public int MinutosAntes { get; set; }

    public string? ReglaRecurrencia { get; set; }

    public DateTime? UltimoAviso { get; set; }

    public DateTime CreadoEn { get; set; }

    public DateTime ActualizadoEn { get; set; }

    public int? UsuarioId { get; set; }

    public virtual Expediente? Expediente { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
