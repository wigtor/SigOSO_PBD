using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class agregarClienteModel : PersonaModel
    {

        [Display(Name = "Giro")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string giro { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }
    }

    
}