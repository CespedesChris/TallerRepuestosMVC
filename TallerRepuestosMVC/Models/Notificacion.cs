using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TallerRepuestosMVC.Models
{
    public class Notificacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }  // NUEVO CAMPO
        public string CorreoUsuarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Leido { get; set; }
        public string NombreUsuario { get; set; } // Para mostrar el nombre
    }
}