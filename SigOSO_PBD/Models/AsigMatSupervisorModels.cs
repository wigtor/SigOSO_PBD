using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;


namespace SigOSO_PBD.Models
{
    public class AsigMatSupervisorModels
    {

        public string id_solicitud { get; set; }
        public string fecha { get; set; }
        public string id_trabajo_interno { get; set; }
        public string material_solicitado { get; set; }
        public string cantidad_solicitada { get; set; }
        public string detalle_solicitud { get; set; }
        public string glosa_material { get; set; }

        public string glosa_trabajo_interno { get; set; }

        public string cantidad_asignada { get; set; }
        public string detalle_respuesta { get; set; }


    }
}