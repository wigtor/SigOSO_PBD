using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class agregarTrabajadorModel : PersonaModel
    {
        [Required]
        [Display(Name = "Perfil")]
        public string id_perfil { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 1)]
        [Display(Name = "Abreviatura")]
        public string iniciales { get; set; }

        [Required]
        [Display(Name = "Fecha inicio de contrato")]
        public string fecha_ini_contrato { get; set; }
    }

    
}