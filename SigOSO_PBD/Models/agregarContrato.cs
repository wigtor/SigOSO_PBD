using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class Contrato
    {

        [Required]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 6)]
        [Display(Name = "rutCliente")]
        [Range(1000000, 1000000000, ErrorMessage = "El {0} ingresado no es válido")]
        public string rutCliente { get; set; }

        [Display(Name = "dia_ini_contrato")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string dia_ini_contrato { get; set; }

        [Display(Name = "mes_ini_contrato")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string mes_ini_contrato { get; set; }

        [Display(Name = "agno_ini_contrato")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string agno_ini_contrato { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "dia_caducidad_contrato")]
        public string dia_caducidad_contrato { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "mes_caducidad_contrato")]
        public string mes_caducidad_contrato { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "agno_caducidad_contrato")]
        public string agno_caducidad_contrato { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "breve_descripcion")]
        public string breve_descripcion { get; set; }
    }

    
}