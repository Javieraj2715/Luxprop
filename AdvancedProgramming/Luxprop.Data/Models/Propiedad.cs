using System;
using System.Collections.Generic;


namespace Luxprop.Data.Models;

public partial class Propiedad
{
    public int PropiedadId { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public decimal? AreaConstruccion { get; set; }

    public decimal? AreaTerreno { get; set; }

    public string? EstadoPublicacion { get; set; }

    public int? AgenteId { get; set; }

    public int? UbicacionId { get; set; }

    public virtual Usuario? Agente { get; set; }

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual Ubicacion? Ubicacion { get; set; }
}
