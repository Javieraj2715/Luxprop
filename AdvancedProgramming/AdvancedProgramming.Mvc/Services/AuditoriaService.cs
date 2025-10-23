using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvancedProgramming.Data;

namespace AdvancedProgramming.Mvc.Services
{
    public class AuditoriaService
    {
        private readonly LuxpropEntities _db;

        public AuditoriaService(LuxpropEntities db)
        {
            _db = db;
        }

        public void RegistrarAuditoria(int usuarioId, string accion, string entidad, string detalle)
        {
            var auditoria = new Auditoria
            {
                Usuario_ID = usuarioId,
                Fecha = DateTime.Now,
                Accion = accion,
                Entidad = entidad,
                Detalle = detalle
            };

            _db.Auditorias.Add(auditoria);
            _db.SaveChanges();
        }
    }
}