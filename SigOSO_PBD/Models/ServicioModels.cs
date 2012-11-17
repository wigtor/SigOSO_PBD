using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class agregarServicioModel
    {

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "Nombre servicio")]
        public string nombreServicio { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Precio pizarra")]
        public string precioPizarra { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Factor bono")]
        public string factorBono { get; set; }

     

    }

    
}