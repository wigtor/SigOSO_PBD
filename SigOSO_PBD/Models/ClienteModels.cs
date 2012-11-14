using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class agregarClienteModel
    {
        
        
        public string ciudad;
        public string comuna;
        public string direccion;
        public string telefono1;
        public string telefono2;
        public string correo;

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [Display(Name = "Nombre")]
        public string nombre;

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [Display(Name = "Rut")]
        public int rut { get; set; }

        
        [Display(Name = "Giro")]
        public string giro { get; set; }

    }

    
}