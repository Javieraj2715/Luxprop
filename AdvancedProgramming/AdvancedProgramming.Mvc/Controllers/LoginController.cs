using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvancedProgramming.Mvc.Models;
using AdvancedProgramming.Mvc.Services;

namespace AdvancedProgramming.Mvc.Controllers
{
    public class LoginController : Controller
    {

        //private readonly LuxpropEntities db = new LuxpropEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        //POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            string hashed = PasswordHelper.HashPassword(vm.Password);

           

            /*var usuario = db.Usuarios
                    .FirstOrDefault(u => u.Email == vm.Email && u.Password == hashed);  

            if(usuario != null)
            {
                Session["UsuarioId"] = usuario.Usuario_ID;
                Session["UsuarioNombre"] = usuario.Nombre;

                return RedirectToAction("Dashboard", "Home");
            }*/

            ViewBag.Error = "Credenciales Incorrectas.";
            return View(vm);
        }

        //Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Login");
        }

    }
}