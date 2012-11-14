using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigOSO_PBD.classes
{
    public class Cliente
    {
        public string ciudad { get; set; }
        public string comuna { get; set; }
        public string direccion { get; set; }
        public string telefono1 { get; set; }
        public string telefono2 { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public int rut { get; set; }
        public string giro { get; set; }
    }
}