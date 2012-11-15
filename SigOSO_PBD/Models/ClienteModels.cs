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

        
    }

    
}