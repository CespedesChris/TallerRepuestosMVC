using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerRepuestosMVC.DAL;
using TallerRepuestosMVC.Models;

namespace TallerRepuestosMVC.Controllers
{
    public class SolicitudRepuestoController : Controller
    {

        // Instancia única del DAL
        private SolicitudRepuestoDAL solicitudDAL = new SolicitudRepuestoDAL();
        private SolicitudRepuestoDAL solicitudDAL1 = new SolicitudRepuestoDAL();
        private RepuestoDAL RepuestoDAL = new RepuestoDAL();



        // GET: SolicitudRepuesto/Pendientes
        public ActionResult Pendientes()
        {
            var solicitudes = solicitudDAL.ObtenerSolicitudes();
            return View(solicitudes);
        }


        // Mostrar formulario para crear solicitud
        public ActionResult Crear()
        {
            ViewBag.Repuestos = new SelectList(RepuestoDAL.ObtenerTodos(), "Id", "Nombre");
            return View();
        }

        // Procesar envío del formulario de solicitud

        [HttpPost]
        public ActionResult Crear(SolicitudRepuesto solicitud)
        {

            // Obtener el ID del usuario desde la sesión Login (NUEVO)
            if (Session["UsuarioId"] != null)
            {
                solicitud.UsuarioId = (int)Session["UsuarioId"];
            }
            else
            {
                TempData["Mensaje"] = "Usuario no autenticado.";
                return RedirectToAction("Crear");
            }



            // Validar disponibilidad

            int disponible = solicitudDAL.ObtenerCantidadDisponible(solicitud.RepuestoId);

            if (solicitud.CantidadSolicitada <= disponible)
            {
                solicitud.FechaSolicitud = DateTime.Now;
                bool guardado = solicitudDAL.AgregarSolicitud(solicitud);
                if (guardado)
                {
                    TempData["Mensaje"] = "Solicitud realizada correctamente.";
                    return RedirectToAction("Crear");
                }
                else
                {
                    ModelState.AddModelError("", "Error al guardar la solicitud.");
                }
            }
            else
            {
                TempData["Mensaje"] = "Cantidad Deseada No Disponible.";
                //ModelState.AddModelError("", "No hay suficiente cantidad disponible.");
            }

            ViewBag.Repuestos = new SelectList(RepuestoDAL.ObtenerTodos(), "Id", "Nombre");
            return View(solicitud);
        }
    

        // Vista que muestra las solicitudes pendientes (perfil bodega)
        public ActionResult SolicitudesPendientes()
        {
            List<SolicitudRepuesto> solicitudes = solicitudDAL.ObtenerSolicitudes();
            return View(solicitudes);
        }

        // Acción para marcar una solicitud como entregada (perfil bodega) y crear la notificación al usuario
        [HttpPost]
        public ActionResult Entregar(int id)
        {
            string entregadoPor = User.Identity.Name ?? "Encargado Bodega";

            bool exito = solicitudDAL.RegistrarEntrega(id, entregadoPor);

            if (exito)
            { 
                // Obtener la solicitud entregada para generar la notificación
                var solicitud = solicitudDAL1.ObtenerSolicitudPorId(id);
                 if (solicitud != null)
                  {
                        string mensaje = $"Tu solicitud del repuesto '{solicitud.NombreRepuesto}' fue entregada el {DateTime.Now:dd/MM/yyyy}.";
                       // Crear la notificación
                       solicitudDAL1.CrearNotificacion(solicitud.UsuarioId, mensaje);
                  }
                TempData["Mensaje"] = "Repuesto entregado correctamente.";
            }

            else
                TempData["Error"] = "Ocurrió un error al procesar la entrega.";

            return RedirectToAction("SolicitudesPendientes");
        }
    }
}

