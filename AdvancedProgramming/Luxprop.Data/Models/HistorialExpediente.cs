using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxprop.Data.Models
{
    public class HistorialExpediente
    {
        public int HistorialId { get; set; }
        public int ExpedienteId { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public string? EstadoNuevo { get; set; }
        public string? Descripcion { get; set; }
        public virtual Expediente? Expediente { get; set; }
        public virtual Usuario? Usuario { get; set; }
        

    }
}
