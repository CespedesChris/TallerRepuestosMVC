using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerRepuestosMVC.DAL;
using TallerRepuestosMVC.Models;

namespace TallerRepuestosMVC.Controllers
{
    public class UsuariosController : Controller
    {
        // PREFERENCIAS DEL USUARIO
       
        public ActionResult Preferencias()
        {
            if (Session["Correo"] == null) return RedirectToAction("Login");

            UsuarioDAL uDal = new UsuarioDAL();
            Usuario u = uDal.ObtenerPorCorreo(Session["Correo"].ToString());

            PreferenciaDAL pDal = new PreferenciaDAL();
            string tema = pDal.ObtenerTema(u.Id);

            ViewBag.Tema = tema;
            return View(u);
        }
        [HttpPost]
        public ActionResult Preferencias(int Id, string tema)
        {
            PreferenciaDAL pDal = new PreferenciaDAL();
            bool ok = pDal.GuardarTema(Id, tema);

            ViewBag.Mensaje = ok ? "Preferencias actualizadas." : "Error al guardar.";
            ViewBag.Tema = tema;

            Usuario u = new UsuarioDAL().ObtenerPorCorreo(Session["Correo"].ToString());
            Session["Tema"] = tema;
            return View(u);
        }
       
        //LOGIN
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario u)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            Usuario usuario = usuarioDAL.VerificarUsuario(u.Correo, u.Contraseña);

            if (usuario != null)
            {
                Session["UsuarioId"] = usuario.Id; //Nuevo al agregar el uso de este campo en otras tablas
                Session["Usuario"] = usuario.Nombre;
                Session["Rol"] = usuario.Rol;
                Session["Correo"] = usuario.Correo;

                //Obtener el tema desde la tabla Preferencias
                PreferenciaDAL preferenciaDAL = new PreferenciaDAL();
                string tema = preferenciaDAL.ObtenerTema(usuario.Id);
                Session["Tema"] = tema ?? "Claro"; // Tema por defecto si es null


                // Redirección según rol
                switch (usuario.Rol)
                {
                    case "Administrador":
                        return RedirectToAction("VistaAdministrador", "Home");
                    case "Mecánico":
                        return RedirectToAction("VistaMecanico", "Home");
                    case "Bodeguero":
                        return RedirectToAction("VistaBodeguero", "Home");
                    default:
                        return RedirectToAction("Perfil");
                }
            }
            else
            {
                ViewBag.Mensaje = "Datos Incorrectos.";
                return View();
            }
        }

        //Método Perfil
        public ActionResult Perfil()
        {
            // Solo permitir el acceso si hay un usuario en sesión
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        //Cerrar sesión
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        //Editar Perfil

        public ActionResult EditarPerfil()
        {
            if (Session["Correo"] == null) return RedirectToAction("Login");

            UsuarioDAL usuarioDAL = new UsuarioDAL();
            Usuario usuario = usuarioDAL.ObtenerPorCorreo(Session["Correo"].ToString());

            return View(usuario);
        }

        [HttpPost]
        public ActionResult EditarPerfil(Usuario u)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();
            bool actualizado = usuarioDAL.ActualizarPerfil(u);

            if (actualizado)
            {
                ViewBag.Mensaje = "Perfil actualizado exitosamente.";
                Session["Usuario"] = u.Nombre;
                Session["Correo"] = u.Correo;
                Session["Rol"] = u.Rol;
            }
            else
            {
                ViewBag.Mensaje = "Ocurrió un error al actualizar.";
            }

            return View(u);
        }

        // GET: Usuarios

        private UsuarioDAL usuarioDAL = new UsuarioDAL();
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                bool guardado = usuarioDAL.InsertarUsuario(usuario);

                if (guardado)
                {
                    ViewBag.Mensaje = "Se agregó usuario correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "Hubo un error al ingresar usuario";
                }
            }
            return View();
        }
    }
}