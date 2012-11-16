﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class agregarUnidadModel
    {

        [Required]
        [StringLength(5, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 1)]
        [Display(Name = "Abreviatura")]
        public string abreviatura { get; set; }


        [Required]
        [StringLength(20, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

    }

    
}