using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvancedProgramming.Data;
using AdvancedProgramming.Mvc.Models;
using AdvancedProgramming.Mvc.Services;

namespace AdvancedProgramming.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly LuxpropEntities db = new LuxpropEntities();

        private readonly AuditoriaService auditoriaService;

        public UserController()
        {
            auditoriaService = new AuditoriaService(db);
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        
        //Get: /Usuarios/Create
        public ActionResult Create()
        {
            var vm = new CreateUserVM
            {
                Roles = db.Rols
                    .Select(r => new SelectListItem
                    {
                        Value = r.Rol_ID.ToString(),
                        Text = r.Nombre
                    })
                    .ToList()
            };
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (CreateUserVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Roles = db.Rols.Select(r => new SelectListItem
                {
                    Value = r.Rol_ID.ToString(),
                    Text = r.Nombre
                }).ToList();

                return View(vm);
            }

            var usuario = new Usuario
            {
                Nombre = vm.Nombre,
                Apellido = vm.Apellido,
                Email = vm.Email,
                Password = PasswordHelper.HashPassword(vm.Password),
                Telefono = vm.Telefono,
                Activo = vm.Activo
               
            };

            db.Usuarios.Add(usuario);
            db.SaveChanges();

            var ur = new Usuario_Rol
            {
                Usuario_ID = usuario.Usuario_ID,
                Rol_ID = vm.RolId
            };

            db.Usuario_Rol.Add(ur);
            db.SaveChanges();

            var rol = db.Rols.Find(vm.RolId);
            auditoriaService.RegistrarAuditoria(
                usuario.Usuario_ID,
                    "Crear",
                    "Usuario",
                    $"Usuario creado {usuario.Nombre} {usuario.Apellido} con el rol {rol.Nombre}"
            );

            TempData["ok"] = "Usuario creado y rol asignado.";
            return RedirectToAction("Index", "Home");
        }

      
        
    }
}