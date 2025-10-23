using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AdvancedProgramming.Mvc.Controllers
{
    public class PropertiesController : Controller
    {
        //private LuxpropEntities db = new LuxpropEntities();

        // GET: Propiedades
        public ActionResult Index()
        {
            var propiedades = db.Propiedades.ToList();
            return View(propiedades);
        }

        // GET: Propiedades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propiedad propiedad = db.Propiedades.Find(id);
            if (propiedad == null)
            {
                return HttpNotFound();
            }
            return View(propiedad);
        }

        // GET: Propiedades/Create
        public ActionResult Create()
        {
            ViewBag.Agente_ID = new SelectList(db.Usuarios, "Usuario_ID", "Nombre");
            ViewBag.Ubicacion_ID = new SelectList(db.Ubicaciones, "Ubicacion_ID", "Provincia");
            return View();
        }

        // POST: Propiedades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Propiedad_ID,Titulo,Descripcion,Precio,Area_Construccion,Area_Terreno,Estado_Publicacion,Agente_ID,Ubicacion_ID")] Propiedad propiedad)
        {
            if (ModelState.IsValid)
            {
                db.Propiedades.Add(propiedad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Agente_ID = new SelectList(db.Usuarios, "Usuario_ID", "Nombre", propiedad.Agente_ID);
            ViewBag.Ubicacion_ID = new SelectList(db.Ubicaciones, "Ubicacion_ID", "Provincia", propiedad.Ubicacion_ID);
            return View(propiedad);
        }

        // GET: Propiedades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propiedad propiedad = db.Propiedades.Find(id);
            if (propiedad == null)
            {
                return HttpNotFound();
            }
            ViewBag.Agente_ID = new SelectList(db.Usuarios, "Usuario_ID", "Nombre", propiedad.Agente_ID);
            ViewBag.Ubicacion_ID = new SelectList(db.Ubicaciones, "Ubicacion_ID", "Provincia", propiedad.Ubicacion_ID);
            return View(propiedad);
        }

        // POST: Propiedades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Propiedad_ID,Titulo,Descripcion,Precio,Area_Construccion,Area_Terreno,Estado_Publicacion,Agente_ID,Ubicacion_ID")] Propiedad propiedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propiedad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Agente_ID = new SelectList(db.Usuarios, "Usuario_ID", "Nombre", propiedad.Agente_ID);
            ViewBag.Ubicacion_ID = new SelectList(db.Ubicaciones, "Ubicacion_ID", "Provincia", propiedad.Ubicacion_ID);
            return View(propiedad);
        }

        // GET: Propiedades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Propiedad propiedad = db.Propiedades.Find(id);
            if (propiedad == null)
            {
                return HttpNotFound();
            }
            return View(propiedad);
        }

        // POST: Propiedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Propiedad propiedad = db.Propiedades.Find(id);
            db.Propiedades.Remove(propiedad);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
