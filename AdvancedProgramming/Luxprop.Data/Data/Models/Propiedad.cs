using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

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

    public string? Recorrido360Url { get; set; }

    public string? MlsId { get; set; }

    public string TipoPropiedad { get; set; } = null!;

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual ICollection<PropertyTour360> PropertyTour360s { get; set; } = new List<PropertyTour360>();
}
