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
        [Display(Name = "día inicio contrato")]
        public string dia_ini_contrato { get; set; }

        [Required]
        [Display(Name = "mes inicio contrato")]
        public string mes_ini_contrato { get; set; }

        [Required]
        [Display(Name = "año inicio contrato")]
        public string agno_ini_contrato { get; set; }


        [Display(Name = "día fin contrato")]
        public string dia_fin_contrato { get; set; }

        [Display(Name = "mes fin contrato")]
        public string mes_fin_contrato { get; set; }

        [Display(Name = "año fin contrato")]
        public string agno_fin_contrato { get; set; }

    }

    public class buscarTrabajadorModel
    {
        [Display(Name = "rut")]
        [StringLength(10, ErrorMessage = "El {0} ingresado no es válido", MinimumLength = 0)]
        public string rut_trabajador { get; set; }

        [Display(Name = "Nombre trabajador")]
        public string nombre_trabajador { get; set; }
    }

    public class ListarTrabajadorModel : PersonaModel
    {
        [Display(Name = "estado")]
        public string estado { get; set; }

    }
}