using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerRepuestosMVC.DAL;
using TallerRepuestosMVC.Models;

namespace TallerRepuestosMVC.Controllers
{
    public class RepuestosController : Controller
    {

        public ActionResult Ingresar()
        {
            if (Session["Correo"] == null) return RedirectToAction("Login", "Usuarios");
            return View();
        }


        [HttpPost]
        public ActionResult Ingresar(Repuesto r)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mensaje = "Por favor revise los datos.";
                return View(r);
            }

            RepuestoDAL dal = new RepuestoDAL();
            bool ok = dal.InsertarRepuesto(r);
            ViewBag.Mensaje = ok ? "Repuesto ingresado exitosamente." : "Error al guardar el repuesto.";
            return View();
        }

    }
}