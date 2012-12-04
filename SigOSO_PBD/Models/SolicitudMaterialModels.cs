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
                option += "<select id='nombre_servicios' onclick='cargarDatos(this)' value='-10'>";
                option += "<option value='-10'> </option>";
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


        public static List<materialSolicitado> getSolicitudMaterial(int idOti, string rut) {
            List<materialSolicitado> resultado = new List<materialSolicitado>();
            materialSolicitado res;
            NpgsqlDataReaderWithConection datos = null;
            NpgsqlDataReaderWithConection lector2 = null;
            try
            {


                string query = "SELECT id_trabajo_interno, nombre_trabajador, glosa_ti FROM trabajador NATURAL JOIN cuadrilla NATURAL JOIN trabajo_interno NATURAL JOIN estado_ot NATURAL JOIN autom_estado WHERE rut_trabajador=" + rut + " AND habilitada=true AND final_normal!=true AND final_inesperado=true AND estado_ot.id_estado=autom_estado.id_estado_pasado AND estado_ot.id_estado=trabajo_interno.id_estado_actual_ti;";
                datos = DBConector.SELECT(query);
                if (datos.HasRows)
                {
                    datos.Read();
                    string id_trabajo_interno = datos["id_trabajo_interno"];
                    string nombre_trabajador = datos["nombre_trabajador"];
                    string glosa_ti = datos["glosa_ti"];


                    query = "SELECT nombre_tipo_material, abreviatura_unidad, cantidad_asignada, id_detalle_material FROM asignacion_material NATURAL JOIN detalle_material NATURAL JOIN material_generico NATURAL JOIN unidad_material WHERE id_trabajo_interno=" + id_trabajo_interno;
                    datos = DBConector.SELECT(query);
                    
                    //NpgsqlDataReaderWithConection lector3 = null;
                    string id_detalle_material = "";
                    int cantidad_retirada_temp;
                    if(datos.HasRows){
                        while (datos.Read())
                        {
                            res = new materialSolicitado();
                            res.nombreMaterial = datos["nombre_tipo_material"].ToString();
                            res.cantidadAsignada=datos["cantidad_asignada"];
                            res.abrevUnidad=datos["abreviatura_unidad"];
                            id_detalle_material = (datos["id_detalle_material"]);
                            lector2 = DBConector.SELECT("SELECT SUM(cantidad_retirada) FROM retiro_material NATURAL JOIN detalle_material WHERE id_trabajo_interno=" + id_trabajo_interno + " AND id_detalle_material=" + id_detalle_material);
                            //lector3 = DBConector.SELECT("SELECT SUM(cantidad_retirada) FROM retiro_material NATURAL JOIN detalle_material WHERE id_trabajo_interno=" + id_trabajo_interno + " AND id_detalle_material=" + id_detalle_material);
                                    
                            if (lector2.Read())
                            {
                                cantidad_retirada_temp=Convert.ToInt32(lector2.GetInt32(0));
                            }
                            else {
                                cantidad_retirada_temp=0;
                            }
                            res.cantidadDisponible=((Convert.ToInt32(datos["cantidad_asignada"])) - (cantidad_retirada_temp)).ToString();
                            resultado.Add(res);
                        }
                    }













                    
                }
            }
            catch (Exception)
            {
            }
            if (datos != null)
            {
                datos.CloseTodo();
            }
            if (lector2 != null)
            {
                lector2.CloseTodo();
            }
            return resultado;
        }
    }

}