using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerRepuestosMVC.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }
        public string Tema { get; set; }
    }
}