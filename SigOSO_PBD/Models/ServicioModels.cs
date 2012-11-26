using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

namespace SigOSO_PBD.Models
{
    public class agregarServicioModel 
    {

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "Nombre servicio")]
        public string nombreServicio { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Precio pizarra")]
        public string precioPizarra { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 2)]
        [Display(Name = "Factor bono")]
        public string factorBono { get; set; }
        
        
        public static string generarTablaServicios()
        {
            string respuesta = "";
            NpgsqlDataReaderWithConection servicios = null;
            try
            {
                servicios = DBConector.SELECT("SELECT * FROM servicio ORDER BY  nombre_servicio ASC");
                respuesta = "<table class='table table-hover'>";
                respuesta += "<thead>";
                respuesta += "<tr>";
                respuesta += "<td><b>Nombre servicio</b></td>";
                respuesta += "<td><b>Precio pizarra</b></td>";
                respuesta += "<td><b>Factor bono</b></td>";
                respuesta += "<td><b>Visible</b></td>";
                respuesta += "<td><b>Editar</b></td>";
                respuesta += "</thead>";
                respuesta += "</tr>";
                while (servicios.Read())
                {
                    respuesta += "<tr>";
                    respuesta += "<td>" + servicios.GetString(servicios.GetOrdinal("nombre_servicio")) + "</td>";
                    respuesta += "<td>" + servicios.GetInt32(servicios.GetOrdinal("precio_pizarra")).ToString() + "</td>";
                    respuesta += "<td>" + servicios.GetDouble(servicios.GetOrdinal("factor_bono_trabajador")).ToString() + "</td>";
                    if (servicios.GetBoolean(servicios.GetOrdinal("visibilidad_servicio")))
                    {
                        respuesta += "<td>" + "<input type='checkbox' disabled='true' checked>" + "</td>";
                    }
                    else
                    {
                        respuesta += "<td>" + "<input type='checkbox' disabled='true'>" + "</td>";
                    }
                    respuesta += "<td>" + "<input name='editar_" + servicios.GetInt32(servicios.GetOrdinal("id_servicio")).ToString() + "' type='submit' value='editar '/>" + "</td>";
                    respuesta += "</tr>";
                }
                respuesta += "</table>";
            }
            catch (Exception)
            {
                respuesta = DBConector.msjError;
            }
            if (servicios != null)
            {
                servicios.Dispose();
                servicios.Close();
                servicios.closeConection();
            }
            return respuesta;
        }

        public static string ocultarAgregarServicios()
        {
            return "<script>$('.condetenedor_agregar_servicio').hide();</script>";
        }

        public static string ocultarModificarServicios()
        {
            return "<script>$('.condetenedor_modificar_servicio').hide();</script>";
        }

        
    }




}