using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

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


        [Display(Name = "id")]
        public string id_trabajador { get; set; }

        public static List<ListarTrabajadorModel> getTrabajadoresForTable(string palabraBusqueda, string criterioBusqueda)
        {
            string query = "SELECT id_trabajador, id_perfil, rut_trabajador, nombre_trabajador, iniciales_trabajador, fecha_ini_contrato_trabajador, fecha_fin_contrato_trabajador, esta_activo, mail_trabajador, tel1_trabajador, tel2_trabajador, direccion_trabajador, comuna_trabajador FROM trabajador";
            if (criterioBusqueda != null) {
                query += " WHERE "+criterioBusqueda+"::text ILIKE '%"+palabraBusqueda+"%'";
            }


            List<ListarTrabajadorModel> resultado = new List<ListarTrabajadorModel>();
            ListarTrabajadorModel temp;
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                while (lector.Read())
                {
                    temp = new ListarTrabajadorModel();
                    temp.id_trabajador = lector["id_trabajador"];
                    temp.rut = lector["rut_trabajador"];
                    temp.nombre = lector["nombre_trabajador"];
                    temp.telefono1 = lector["tel1_trabajador"];
                    temp.telefono2 = lector["tel2_trabajador"];
                    temp.estado = lector["esta_activo"];
                    
                    resultado.Add(temp);
                }
            }
            catch (Exception)
            {
                temp = new ListarTrabajadorModel();
                temp.id_trabajador = "0";
                temp.nombre = "Error en la base de datos";
                temp.telefono1 = "0";
                temp.telefono2 = "0";
                temp.estado = "Error en la DB";
                resultado.Add(temp);
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return resultado;


        }

        public static ListarTrabajadorModel getTrabajadorByRut(int rut)
        {
            string query = "SELECT id_trabajador, id_perfil, rut_trabajador, nombre_trabajador, iniciales_trabajador, fecha_ini_contrato_trabajador, fecha_fin_contrato_trabajador, esta_activo, mail_trabajador, tel1_trabajador, tel2_trabajador, direccion_trabajador, comuna_trabajador FROM trabajador";
            query += " WHERE rut_trabajador = " + rut;
            ListarTrabajadorModel temp = null;
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                if (lector.Read())
                {
                    temp = new ListarTrabajadorModel();
                    temp.id_trabajador = lector["id_trabajador"];
                    temp.rut = lector["rut_trabajador"];
                    temp.nombre = lector["nombre_trabajador"];
                    temp.telefono1 = lector["tel1_trabajador"];
                    temp.telefono2 = lector["tel2_trabajador"];
                    temp.estado = lector["esta_activo"];

                }
                else {
                    temp = null;
                }
            }
            catch (Exception)
            {
                temp = new ListarTrabajadorModel();
                temp.id_trabajador = "0";
                temp.nombre = "Error en la base de datos";
                temp.telefono1 = "0";
                temp.telefono2 = "0";
                temp.estado = "Error en la DB";
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return temp;

        }


        public static List<SelectListItem> getAllTrabajadores()
        {
            string query = "SELECT id_trabajador, rut_trabajador, nombre_trabajador FROM trabajador WHERE esta_activo = TRUE";
            NpgsqlDataReaderWithConection lector = null;
            List<SelectListItem> res = new List<SelectListItem>();
            res.Add(new SelectListItem
            {
                Text = "",
                Value = "-1"
            });
            try
            {
                lector = DBConector.SELECT(query);
                while (lector.Read())
                {
                    res.Add(new SelectListItem
                    {
                        Text = lector["rut_trabajador"] + " - " + lector["nombre_trabajador"],
                        Value = lector["rut_trabajador"]
                    });
                }
            }
            catch (Exception)
            {
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return res;

        }


        public static string crearCuadrilla(List<int> listaAgregadosSesion, out bool satisfactorio) {
            
            if (listaAgregadosSesion.Count < 2)
            {
                satisfactorio = false;
                return "No puede crear una cuadrilla de menos de 2 trabajadores";
            }
            string query = "SELECT sp_new_cuadrilla(ARRAY[";
            foreach (int temp in listaAgregadosSesion)
            {
                if (!temp.Equals(listaAgregadosSesion[0]))
                {
                    query += ", ";
                }
                query += temp;
            }
            query += "])";

            string mensaje = "";
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                if (lector.Read())
                {
                    if (lector.GetBoolean(0))
                    {
                        satisfactorio = true;
                        mensaje = "Se ha creado la cuadrilla satisfactoriamente";
                    }
                    else
                    {
                        satisfactorio = true;
                        mensaje = "No se ha podido crear la cuadrilla";
                    }
                }
                else
                {
                    satisfactorio = false;
                    mensaje =  "Ha ocurrido un error al crear la cuadrilla";
                }
            }
            catch (Exception)
            {
                satisfactorio = false;
                mensaje = "Ha ocurrido un error al crear la cuadrilla";
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }

            return mensaje;
        }
    }

    
}