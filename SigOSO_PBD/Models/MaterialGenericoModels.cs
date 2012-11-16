using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class MaterialGenericoModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Glosa material")]
        public string glosa_material { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 1)]
        [Display(Name = "Nombre material")]
        public string nombre { get; set; }

        [Display(Name = "Unidad de medida")]
        public string id_unidad { get; set; }
    }

    
}