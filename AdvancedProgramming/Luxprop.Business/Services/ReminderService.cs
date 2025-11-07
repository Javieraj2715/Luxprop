using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

public class ReminderService : IReminderService
{
    private readonly LuxpropContext _db;
    public ReminderService(LuxpropContext db) => _db = db;

    public async Task<Recordatorio> CreateAsync(Recordatorio r, CancellationToken ct = default)
    {
        r.CreadoEn = DateTime.UtcNow;
        r.ActualizadoEn = DateTime.UtcNow;
        _db.Recordatorios.Add(r);
        await _db.SaveChangesAsync(ct);
        return r;
    }

    public Task<Recordatorio?> GetAsync(int id, CancellationToken ct = default) =>
        _db.Recordatorios
           .Include(x => x.Propiedad)
           .Include(x => x.Expediente)
           .FirstOrDefaultAsync(x => x.RecordatorioId == id, ct);

    public Task<List<Recordatorio>> ListAsync(int? usuarioId = null, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
    {
        var q = _db.Recordatorios.AsQueryable();
        if (usuarioId is not null) q = q.Where(x => x.UsuarioId == usuarioId);
        if (from is not null) q = q.Where(x => x.Inicio >= from);
        if (to is not null) q = q.Where(x => x.Inicio <= to);
        return q.OrderBy(x => x.Inicio).ToListAsync(ct);
    }

    public async Task<bool> UpdateAsync(Recordatorio r, CancellationToken ct = default)
    {
        _db.Recordatorios.Update(r);
        r.ActualizadoEn = DateTime.UtcNow;
        return await _db.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var dbR = await _db.Recordatorios.FindAsync([id], ct);
        if (dbR is null) return false;
        _db.Recordatorios.Remove(dbR);
        return await _db.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> SetEstadoAsync(int id, string nuevoEstado, CancellationToken ct = default)
    {
        var r = await _db.Recordatorios.FindAsync([id], ct);
        if (r is null) return false;
        r.Estado = nuevoEstado;
        r.ActualizadoEn = DateTime.UtcNow;
        return await _db.SaveChangesAsync(ct) > 0;
    }


    // 🔥 NUEVO: CREAR un recordatorio
    public async Task AddAsync(Recordatorio recordatorio)
    {
        // Asegurar valores por defecto
        if (string.IsNullOrWhiteSpace(recordatorio.Estado))
            recordatorio.Estado = "Pendiente";

        // por si viene hora local, lo podemos guardar UTC (tu decides si quieres esto)
        // recordatorio.Inicio = recordatorio.Inicio.ToUniversalTime();

        _db.Recordatorios.Add(recordatorio);
        await _db.SaveChangesAsync();
    }

    // 🔥 NUEVO: OBTENER uno por ID (para la vista de edición)
    public async Task<Recordatorio?> GetByIdAsync(int id)
    {
        return await _db.Recordatorios
            .FirstOrDefaultAsync(r => r.RecordatorioId == id);
    }

    // 🔥 NUEVO: ACTUALIZAR un recordatorio existente
    public async Task UpdateAsync(Recordatorio recordatorio)
    {
        // buscamos el registro real en DB
        var existing = await _db.Recordatorios
            .FirstOrDefaultAsync(r => r.RecordatorioId == recordatorio.RecordatorioId);

        if (existing == null)
            return;

        // campos editables
        existing.Titulo = recordatorio.Titulo;
        existing.Descripcion = recordatorio.Descripcion;
        existing.Tipo = recordatorio.Tipo;
        existing.Inicio = recordatorio.Inicio; // asegúrate que ya viene con fecha+hora combinadas
        existing.Estado = recordatorio.Estado;
        existing.PropiedadId = recordatorio.PropiedadId;

        await _db.SaveChangesAsync();
    }
}
