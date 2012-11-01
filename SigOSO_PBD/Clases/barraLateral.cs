using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigOSO_PBD.Clases
{
    public class barraLateral
    {
        public const int MENUTRABAJADOR=4;
        public static string mostrarMenu(int tipoMenu){
            string resultado="";
            switch (tipoMenu){ 
                case MENUTRABAJADOR:


                    
                    break;
                default:
                    resultado = "";
                    break;
            }
            return resultado;
        }
    }
}