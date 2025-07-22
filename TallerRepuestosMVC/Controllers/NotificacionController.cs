using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerRepuestosMVC.DAL;

namespace TallerRepuestosMVC.Controllers
{
    public class NotificacionController : Controller
    {
        NotificacionDAL notificacionDAL = new NotificacionDAL();

        public ActionResult Ver()
        {

            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Usuario"); // Redirige al login si no hay sesión
            }

            int usuarioId = (int)Session["UsuarioId"];
            var notificaciones = notificacionDAL.ObtenerNotificacionesPorUsuario(usuarioId);
            return View(notificaciones);

        }
        [HttpPost]
        public ActionResult MarcarComoLeidas()
        {
            if (Session["UsuarioId"] == null)
            {
                return RedirectToAction("Login", "Usuario");
            }
            int usuarioId = (int)Session["UsuarioId"];
            NotificacionDAL notificacionDAL = new NotificacionDAL();
            notificacionDAL.MarcarComoLeidas(usuarioId);

            return RedirectToAction("Ver"); // Redirige a la lista de notificaciones
        }
    }
}