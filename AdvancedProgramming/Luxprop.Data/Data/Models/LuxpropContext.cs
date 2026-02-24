using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Luxprop.Data.Data.Models;

public partial class LuxpropContext : DbContext
{
    public LuxpropContext()
    {
    }

    public LuxpropContext(DbContextOptions<LuxpropContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agente> Agentes { get; set; }

    public virtual DbSet<AlertaVencimiento> AlertaVencimientos { get; set; }

    public virtual DbSet<AlertasDocumento> AlertasDocumentos { get; set; }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<ChatThread> ChatThreads { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public virtual DbSet<HistorialExpediente> HistorialExpedientes { get; set; }

    public virtual DbSet<PropertyTour360> PropertyTour360s { get; set; }

    public virtual DbSet<Propiedad> Propiedads { get; set; }

    public virtual DbSet<Recordatorio> Recordatorios { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TareaTramite> TareaTramites { get; set; }

    public virtual DbSet<Ubicacion> Ubicacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:luxprop-sql-server.database.windows.net,1433;Initial Catalog=Luxprop;Persist Security Info=False;User ID=sqladmin;Password=Luxprop2025!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agente>(entity =>
        {
            entity.HasKey(e => e.AgenteId).HasName("PK__Agente__29E28221338F1B7E");

            entity.ToTable("Agente");

            entity.Property(e => e.AgenteId).HasColumnName("Agente_ID");
            entity.Property(e => e.CodigoAgente)
                .HasMaxLength(50)
                .HasColumnName("Codigo_Agente");
            entity.Property(e => e.Sucursal).HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Agentes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Agente__Usuario___245D67DE");
        });

        modelBuilder.Entity<AlertaVencimiento>(entity =>
        {
            entity.HasKey(e => e.AlertaId).HasName("PK__AlertaVe__35E6F643505EFAD0");

            entity.ToTable("AlertaVencimiento");

            entity.Property(e => e.AlertaId).HasColumnName("Alerta_ID");
            entity.Property(e => e.DocumentoId).HasColumnName("Documento_ID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaProgramada).HasColumnName("Fecha_Programada");
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Documento).WithMany(p => p.AlertaVencimientos)
                .HasForeignKey(d => d.DocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AlertaVen__Docum__2645B050");
        });

        modelBuilder.Entity<AlertasDocumento>(entity =>
        {
            entity.HasKey(e => e.AlertaId).HasName("PK__AlertasD__35E6F643E4EB7115");

            entity.ToTable("AlertasDocumento");

            entity.Property(e => e.AlertaId).HasColumnName("Alerta_ID");
            entity.Property(e => e.DocumentoId).HasColumnName("Documento_ID");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnName("Fecha_Registro");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Documento).WithMany(p => p.AlertasDocumentos)
                .HasForeignKey(d => d.DocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alerta_Documento");
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.AuditoriaId).HasName("PK__Auditori__D7259D3289D73B68");

            entity.Property(e => e.AuditoriaId).HasColumnName("Auditoria_ID");
            entity.Property(e => e.Accion).HasMaxLength(100);
            entity.Property(e => e.Entidad).HasMaxLength(100);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Auditoria__Usuar__2739D489");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.ChatMessageId).HasName("PK__ChatMess__9AB61035A4D6AB59");

            entity.Property(e => e.Sender).HasMaxLength(20);
            entity.Property(e => e.SentUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ChatThread).WithMany(p => p.ChatMessages)
                .HasForeignKey(d => d.ChatThreadId)
                .HasConstraintName("FK_ChatMessages_ChatThreads");
        });

        modelBuilder.Entity<ChatThread>(entity =>
        {
            entity.HasKey(e => e.ChatThreadId).HasName("PK__ChatThre__32405D65DD5BC5D3");

            entity.Property(e => e.ClientEmail).HasMaxLength(200);
            entity.Property(e => e.ClientName).HasMaxLength(150);
            entity.Property(e => e.ClosedUtc).HasColumnType("datetime");
            entity.Property(e => e.CreatedUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(20)
                .HasDefaultValue("Open");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.CitaId).HasName("PK__Cita__992D0A2550B3A8B6");

            entity.Property(e => e.CitaId).HasColumnName("Cita_ID");
            entity.Property(e => e.Asunto).HasMaxLength(150);
            entity.Property(e => e.Canal).HasMaxLength(50);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");

            entity.HasOne(d => d.Expediente).WithMany(p => p.Cita)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cita__Expediente__29221CFB");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__EB683FB4CBAF48CC");

            entity.ToTable("Cliente");

            entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");
            entity.Property(e => e.Cedula).HasMaxLength(50);
            entity.Property(e => e.TipoCliente)
                .HasMaxLength(50)
                .HasColumnName("Tipo_Cliente");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Cliente__Usuario__2A164134");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.DocumentoId).HasName("PK__Document__FBEBB4608851AA7A");

            entity.ToTable("Documento");

            entity.Property(e => e.DocumentoId).HasColumnName("Documento_ID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.Etiquetas).HasMaxLength(255);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaCarga).HasColumnName("Fecha_Carga");
            entity.Property(e => e.FechaVencimiento).HasColumnName("Fecha_Vencimiento");
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(100)
                .HasColumnName("Tipo_Documento");
            entity.Property(e => e.UrlArchivo).HasMaxLength(500);

            entity.HasOne(d => d.Expediente).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.ExpedienteId)
                .HasConstraintName("FK__Documento__Exped__2B0A656D");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.ExpedienteId).HasName("PK__Expedien__0AAD7FACC62710A7");

            entity.ToTable("Expediente");

            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.AgenteId).HasColumnName("Agente_ID");
            entity.Property(e => e.Categoria).HasMaxLength(50);
            entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.CreadoPorId).HasColumnName("CreadoPor_ID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaApertura).HasColumnName("Fecha_Apertura");
            entity.Property(e => e.FechaCierre).HasColumnName("Fecha_Cierre");
            entity.Property(e => e.ModificadoPorId).HasColumnName("ModificadoPor_ID");
            entity.Property(e => e.Prioridad).HasMaxLength(30);
            entity.Property(e => e.PropiedadId).HasColumnName("Propiedad_ID");
            entity.Property(e => e.TipoOcupacion)
                .HasMaxLength(50)
                .HasColumnName("Tipo_Ocupacion");
            entity.Property(e => e.UltimaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Ultima_Actualizacion");

            entity.HasOne(d => d.Agente).WithMany(p => p.ExpedienteAgentes)
                .HasForeignKey(d => d.AgenteId)
                .HasConstraintName("FK_Expediente_Agente");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Expedient__Clien__2BFE89A6");

            entity.HasOne(d => d.CreadoPor).WithMany(p => p.ExpedienteCreadoPors)
                .HasForeignKey(d => d.CreadoPorId)
                .HasConstraintName("FK_Expediente_CreadoPor");

            entity.HasOne(d => d.ModificadoPor).WithMany(p => p.ExpedienteModificadoPors)
                .HasForeignKey(d => d.ModificadoPorId)
                .HasConstraintName("FK_Expediente_ModificadoPor");

            entity.HasOne(d => d.Propiedad).WithMany(p => p.Expedientes)
                .HasForeignKey(d => d.PropiedadId)
                .HasConstraintName("FK__Expedient__Propi__59063A47");
        });

        modelBuilder.Entity<HistorialExpediente>(entity =>
        {
            entity.HasKey(e => e.HistorialId).HasName("PK__Historia__97213B8A3D3D98A5");

            entity.ToTable("HistorialExpediente");

            entity.Property(e => e.HistorialId).HasColumnName("Historial_ID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EstadoAnterior).HasMaxLength(100);
            entity.Property(e => e.EstadoNuevo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Modificacion");
            entity.Property(e => e.Ipregistro)
                .HasMaxLength(100)
                .HasColumnName("IPRegistro");
            entity.Property(e => e.Observacion).HasMaxLength(100);
            entity.Property(e => e.TipoAccion).HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Expediente).WithMany(p => p.HistorialExpedientes)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__Exped__31B762FC");

            entity.HasOne(d => d.Usuario).WithMany(p => p.HistorialExpedientes)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__Usuar__32AB8735");
        });

        modelBuilder.Entity<PropertyTour360>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Property__3214EC0753766F53");

            entity.ToTable("PropertyTour360");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.TourUrl).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy).HasMaxLength(100);

            entity.HasOne(d => d.Property).WithMany(p => p.PropertyTour360s)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PropertyTour360_Property");
        });

        modelBuilder.Entity<Propiedad>(entity =>
        {
            entity.HasKey(e => e.PropiedadId).HasName("PK__Propieda__D578EA75B764735E");

            entity.ToTable("Propiedad");

            entity.Property(e => e.PropiedadId).HasColumnName("Propiedad_ID");
            entity.Property(e => e.AgenteId).HasColumnName("Agente_ID");
            entity.Property(e => e.AreaConstruccion)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Area_Construccion");
            entity.Property(e => e.AreaTerreno)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Area_Terreno");
            entity.Property(e => e.EstadoPublicacion)
                .HasMaxLength(50)
                .HasColumnName("Estado_Publicacion");
            entity.Property(e => e.Latitud).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.Longitud).HasColumnType("decimal(10, 6)");
            entity.Property(e => e.MlsId).HasMaxLength(50);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Recorrido360Url).HasMaxLength(2048);
            entity.Property(e => e.TipoPropiedad)
                .HasMaxLength(250)
                .HasDefaultValue("Homes")
                .HasColumnName("Tipo_Propiedad");
            entity.Property(e => e.Titulo).HasMaxLength(150);
            entity.Property(e => e.UbicacionId).HasColumnName("Ubicacion_ID");
        });

        modelBuilder.Entity<Recordatorio>(entity =>
        {
            entity.HasKey(e => e.RecordatorioId).HasName("PK__Recordat__9E99466849428A4E");

            entity.ToTable("Recordatorio");

            entity.HasIndex(e => e.Inicio, "IX_Recordatorio_Inicio");

            entity.HasIndex(e => new { e.PropiedadId, e.ExpedienteId }, "IX_Recordatorio_Prop_Exp");

            entity.Property(e => e.ActualizadoEn)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreadoEn)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fin).HasPrecision(0);
            entity.Property(e => e.Inicio).HasPrecision(0);
            entity.Property(e => e.MinutosAntes).HasDefaultValue(60);
            entity.Property(e => e.NotificarCorreo).HasDefaultValue(true);
            entity.Property(e => e.Prioridad)
                .HasMaxLength(10)
                .HasDefaultValue("Media");
            entity.Property(e => e.ReglaRecurrencia).HasMaxLength(200);
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .HasDefaultValue("Cita");
            entity.Property(e => e.Titulo).HasMaxLength(150);
            entity.Property(e => e.UltimoAviso).HasPrecision(0);

            entity.HasOne(d => d.Expediente).WithMany(p => p.Recordatorios)
                .HasForeignKey(d => d.ExpedienteId)
                .HasConstraintName("FK_Recordatorio_Expediente");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Recordatorios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_Recordatorio_Usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__795EBD69CA5F41D8");

            entity.ToTable("Rol");

            entity.Property(e => e.RolId).HasColumnName("Rol_ID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TareaTramite>(entity =>
        {
            entity.HasKey(e => e.TareaId).HasName("PK__Tarea_Tr__327AB98A26EF2530");

            entity.ToTable("Tarea_Tramite");

            entity.Property(e => e.TareaId).HasColumnName("Tarea_ID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.ExpedienteId).HasColumnName("Expediente_ID");
            entity.Property(e => e.FechaCierre).HasColumnName("Fecha_Cierre");
            entity.Property(e => e.FechaCompromiso).HasColumnName("Fecha_Compromiso");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_Inicio");
            entity.Property(e => e.Prioridad).HasMaxLength(50);
            entity.Property(e => e.Titulo).HasMaxLength(150);

            entity.HasOne(d => d.Expediente).WithMany(p => p.TareaTramites)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tarea_Tra__Exped__367C1819");
        });

        modelBuilder.Entity<Ubicacion>(entity =>
        {
            entity.HasKey(e => e.UbicacionId).HasName("PK__Ubicacio__AE143812935C0528");

            entity.ToTable("Ubicacion");

            entity.Property(e => e.UbicacionId).HasColumnName("Ubicacion_ID");
            entity.Property(e => e.Canton).HasMaxLength(50);
            entity.Property(e => e.DetalleDireccion).HasColumnName("Detalle_Direccion");
            entity.Property(e => e.Distrito).HasMaxLength(50);
            entity.Property(e => e.Provincia).HasMaxLength(50);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__77111335335777C9");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D1053474E5806A").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasDefaultValue("");
            entity.Property(e => e.ResetPasswordExpiration).HasColumnType("datetime");
            entity.Property(e => e.ResetPasswordToken).HasMaxLength(200);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.UsuarioRolId).HasName("PK__Usuario___72F8818706FC9FA5");

            entity.ToTable("Usuario_Rol");

            entity.Property(e => e.UsuarioRolId).HasColumnName("UsuarioRol_ID");
            entity.Property(e => e.RolId).HasColumnName("Rol_ID");
            entity.Property(e => e.UsuarioId).HasColumnName("Usuario_ID");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_R__Rol_I__3864608B");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_R__Usuar__37703C52");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
