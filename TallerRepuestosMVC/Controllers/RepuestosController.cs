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

            //LISTAR REPUESTOS INGRESADOS EN BD
            public ActionResult Listar()
            {
                if (Session["Correo"] == null) return RedirectToAction("Login", "Usuarios");

                RepuestoDAL dal = new RepuestoDAL();
                var lista = dal.ObtenerTodos();
                return View(lista);
            }



            public ActionResult Ingresar()
            {
                if (Session["Correo"] == null) return RedirectToAction("Login", "Usuarios");
            RepuestoDAL dal = new RepuestoDAL();
            ViewBag.Repuestos = dal.ObtenerTodos();
            return View();
            }


            [HttpPost]
            public ActionResult Ingresar(Repuesto r)
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Mensaje = "Por favor revise los datos.";
                // Recargar la lista de repuestos para que no esté vacía al renderizar la tabla
                RepuestoDAL dalError = new RepuestoDAL();
                ViewBag.Repuestos = dalError.ObtenerTodos();
                return View(r);
                }

                RepuestoDAL dal = new RepuestoDAL();
                bool ok = dal.InsertarRepuesto(r);
                ViewBag.Mensaje = ok ? "Repuesto ingresado exitosamente." : "Error al guardar el repuesto.";
                 // Volver a cargar la lista actualizada de repuestos
                 ViewBag.Repuestos = dal.ObtenerTodos();


            return View();
            }

        }
    }