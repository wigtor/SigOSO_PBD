﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class PersonaModel
    {

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Comuna")]
        public string comuna { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "El {0} debe tener al menos {2} dígitos de longitud y máximo {1}", MinimumLength = 5)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono 1")]
        public string telefono1 { get; set; }

        
        [StringLength(15, ErrorMessage = "El {0} debe tener al menos {2} dígitos de longitud y máximo {1}.", MinimumLength = 5)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono 2")]
        public string telefono2 { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 5)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^(?<user>[^@]+)@(?<host>.+)$", ErrorMessage = "El {0} ingresado no es válido")]
        [Display(Name = "Correo electrónico")]
        public string correo { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "El {0} ingresado no es válido", MinimumLength = 7)]
        [Display(Name = "Rut")]
        public string rut { get; set; }

    }

    
}