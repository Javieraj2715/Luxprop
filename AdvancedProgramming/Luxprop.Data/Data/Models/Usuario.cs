using System;
using System.Collections.Generic;

namespace Luxprop.Data.Data.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public bool? Activo { get; set; }

    public string Password { get; set; } = null!;

    public string? ResetPasswordToken { get; set; }

    public DateTime? ResetPasswordExpiration { get; set; }

    public virtual ICollection<Agente> Agentes { get; set; } = new List<Agente>();

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Expediente> ExpedienteAgentes { get; set; } = new List<Expediente>();

    public virtual ICollection<Expediente> ExpedienteCreadoPors { get; set; } = new List<Expediente>();

    public virtual ICollection<Expediente> ExpedienteModificadoPors { get; set; } = new List<Expediente>();

    public virtual ICollection<HistorialExpediente> HistorialExpedientes { get; set; } = new List<HistorialExpediente>();

    public virtual ICollection<Recordatorio> Recordatorios { get; set; } = new List<Recordatorio>();

    public virtual ICollection<UsuarioRol> UsuarioRols { get; set; } = new List<UsuarioRol>();
}
