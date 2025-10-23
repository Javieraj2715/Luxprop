using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvancedProgramming.Mvc.Models
{
    public class CreateUserVM
    {
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(100)]
        public string Apellido { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        // Si en tu EDMX Activo es nullable, cambia a bool? y pon valor por defecto en el controlador
        public bool Activo { get; set; } = true;
        

        [Required(ErrorMessage = "Seleccione un rol")]
        public int RolId { get; set; }                // el rol elegido

        public IEnumerable<SelectListItem> Roles { get; set; } // para llenar el <select>
    }
}
