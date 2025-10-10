using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Agente
{
    public int AgenteId { get; set; }

    public int UsuarioId { get; set; }

    public string? CodigoAgente { get; set; }

    public string? Sucursal { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
