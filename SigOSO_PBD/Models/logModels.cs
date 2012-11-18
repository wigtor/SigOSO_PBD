using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class logModel
    {

        [Display(Name = "Nombre tabla")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "general_activo")]
        public bool general_activo { get; set; }

        [Required]
        [Display(Name = "insert_activo")]
        public bool insert_activo { get; set; }

        [Required]
        [Display(Name = "update_activo")]
        public bool update_activo { get; set; }

        [Required]
        [Display(Name = "delete_activo")]
        public bool delete_activo { get; set; }
    }

    
}