using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TallerRepuestosMVC.Models
{
    public class Repuesto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe indicar la cantidad")]
        [Range(1, 1000, ErrorMessage = "Cantidad entre 1 y 1000")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Debe indicar el precio")]
        [Range(0.01, 100000, ErrorMessage = "Precio debe ser mayor a cero")]
        public decimal Precio { get; set; }
    }
}