using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace SigOSO_PBD.Models
{
    public class retiroMaterialModel
    {                
        [Required]
        [Display(Name = "RUT")]
        public string RUT { get; set; }
        [Display(Name = "NumOrdenInterna")]
        public string NumOrdenInterna { get; set; }
        [Display(Name = "NomJefeCuadrilla")]
        public string NomJefeCuadrilla { get; set; }
        [Display(Name = "DetalleOrdenTrabajo")]
        public string DetalleOrdenTrabajo { get; set; }


    }

    
}