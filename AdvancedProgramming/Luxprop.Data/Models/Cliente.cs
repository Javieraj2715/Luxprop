using System;
using System.Collections.Generic;

namespace Luxprop.Data.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Cedula { get; set; } = null!;

    public string? TipoCliente { get; set; }

    public int? UsuarioId { get; set; }

    public virtual ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

    public virtual Usuario? Usuario { get; set; }
}
