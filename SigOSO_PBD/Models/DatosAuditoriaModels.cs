using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class DatosAuditoriaModel
    {

        [Display(Name = "Nombre tabla")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string nombreTabla { get; set; }
        
        [Display(Name = "operacion")]
        public string operacion { get; set; }

        [Required]
        [Display(Name = "dia_ini")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string dia_ini { get; set; }

        [Required]
        [Display(Name = "mes_ini")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string mes_ini { get; set; }

        [Required]
        [Display(Name = "agno_ini")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string agno_ini { get; set; }

        [Required]
        [Display(Name = "dia_fin")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string dia_fin { get; set; }

        [Required]
        [Display(Name = "mes_fin")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string mes_fin { get; set; }

        [Required]
        [Display(Name = "agno_fin")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string agno_fin { get; set; }

        [Required]
        [Display(Name = "cuantosVer")]
        public string cuantosVer { get; set; }
        
    }

    public class DatoLogForTabla
    {
        public string nombreTabla;
        public string operacion;
        public string timestamp;
        public string datosAntes;
        public string datosDespues;

    }
    
}