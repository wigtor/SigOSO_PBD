using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

namespace SigOSO_PBD.Models
{
    public class solicitudMaterialModels
    {

        [Required]
        [StringLength(4, ErrorMessage = "La cantidad de materiales a solicitar debe ser positivo un numero positivo", MinimumLength = 1)]
        public string cantidad { get; set; }
        public string detalle { get; set; }        
        public string id { get; set; }
        public string tipo { get; set; }
        public string unidad { get; set; }
        


        public static string generarNumeroOrden(int rut_jefe_cuadrilla)
        {
            string query = "SELECT id_cuadrilla FROM cuadrilla NATURAL JOIN trabajador WHERE habilitada=true and rut_trabajador=" + rut_jefe_cuadrilla;
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                if (lector.Read())
                {
                    string respuesta = "<div>N° orden de trabajo interno ";
                    respuesta = respuesta + lector.GetInt32(lector.GetOrdinal("id_cuadrilla")).ToString() + "</div>";
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                    return (respuesta);
                }
            }
            catch (Exception)
            {
                return DBConector.msjError;//ex.Message;
            }
            return "";
        }


        public static string generarSolicitudDeMateriales(int rut_jefe_cuadrilla)
        {
            string script_respuesta = "";
            string respuesta = "";
            string option = "";
            string query = "SELECT cod_producto, nombre_tipo_material, glosa_tipo_material, abreviatura_unidad FROM material_generico NATURAL JOIN unidad_material";
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                script_respuesta = "<script type='text/javascript'>";
                script_respuesta += "var var_cod_producto = new Array();";
                script_respuesta += "var var_tipoMaterial = new Array();";
                script_respuesta += "var var_unidad = new Array();";
                script_respuesta += "var var_detalle = new Array();";
                option += "<select id='nombre_servicios' onclick='cargarDatos(this)'>";
                option += "<option value='" + -10 + "'>"  + "</option>";
                while (lector.Read())
                {
                    script_respuesta += "var_cod_producto.push('" + lector["cod_producto"] + "');";
                    script_respuesta += "var_tipoMaterial.push('" + lector["nombre_tipo_material"] + "');";
                    script_respuesta += "var_unidad.push('" + lector["abreviatura_unidad"] + "');";
                    script_respuesta += "var_detalle.push('" + lector["glosa_tipo_material"] + "');";

                    option += "<option value='" + lector["cod_producto"] + "'>" + lector["nombre_tipo_material"] + "</option>";
                }
                script_respuesta += "</script>";
                option += "</select>";


            }
            catch (Exception)
            {
                respuesta = DBConector.msjError;//ex.Message;
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return (respuesta + script_respuesta + option);
        }
    }

    public class materialSolicitado
    {
        public string nombreMaterial { get; set; }
        public string abrevUnidad { get; set; }
        public string cantidadAsignada { get; set; }
        public string cantidadDisponible { get; set; }


        public static List<materialSolicitado> getSolicitudMaterial(int idOti) {
            List<materialSolicitado> resultado = new List<materialSolicitado>();
            materialSolicitado res = new materialSolicitado();
            res.nombreMaterial = "Cemento";
            res.cantidadAsignada = "2";
            res.cantidadDisponible = "1";
            res.abrevUnidad = "Sacos";

            resultado.Add(res);

            return resultado;
        }
    }

}