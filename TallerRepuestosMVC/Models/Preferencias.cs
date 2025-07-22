using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerRepuestosMVC.Models
{
    public class Preferencias
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Tema { get; set; }  // "Claro" o "Oscuro"
    }
}