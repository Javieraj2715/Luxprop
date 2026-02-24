using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class Ubicacion
{
    public int UbicacionId { get; set; }

    public string? Provincia { get; set; }

    public string? Canton { get; set; }

    public string? Distrito { get; set; }

    public string? DetalleDireccion { get; set; }
}
