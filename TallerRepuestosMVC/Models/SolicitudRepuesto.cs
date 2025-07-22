using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerRepuestosMVC.Models
{

    public class SolicitudRepuesto
    {
        public int Id { get; set; }
        public int RepuestoId { get; set; }
        public string NombreRepuesto { get; set; } 
        public int CantidadSolicitada { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; }
        public string Solicitante { get; set; }
        public int UsuarioId { get; set; }       // CAMPO QUE AGREGUE AL DARME CUENTA QUE OCUPO ID USUARIO
        public DateTime? FechaEntrega { get; set; }
        public string EntregadoPor { get; set; }
    }
}