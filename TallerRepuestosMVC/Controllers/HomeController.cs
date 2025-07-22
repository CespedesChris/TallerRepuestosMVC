using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TallerRepuestosMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult NoAutorizado()
        {
            return View();
        }
        public ActionResult VistaAdministrador()
        {
            return View();
        }
        public ActionResult VistaBodeguero()
        {
            return View();
        }
        public ActionResult VistaMecanico()
        {
            return View();
        }

        //Para la acción de regresar al menú nuevamente
        public ActionResult RegresarAlMenu()
        {
            string rol = Session["Rol"]?.ToString();

            switch (rol)
            {
                case "Administrador":
                    return RedirectToAction("VistaAdministrador", "Home");

                case "Mecánico":
                    return RedirectToAction("VistaMecanico", "Home");

                case "Bodeguero":
                    return RedirectToAction("VistaBodeguero", "Home");

                default:
                    return RedirectToAction("Login", "Usuarios"); 
            }
        }




        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Pagina Taller de Repuestos.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "INICIO DE SESION";

            return View();
        }
    }
}