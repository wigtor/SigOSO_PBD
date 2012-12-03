using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

namespace SigOSO_PBD.Models
{
    public class Contrato
    {

        public string id_contrato { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 6)]
        [Display(Name = "rutCliente")]
        [Range(1000000, 1000000000, ErrorMessage = "El {0} ingresado no es válido")]
        public string rutCliente { get; set; }

        [Display(Name = "dia_ini_contrato")]
        [StringLength(2, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 1)]
        public string dia_ini_contrato { get; set; }

        [Display(Name = "mes_ini_contrato")]
        [StringLength(2, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 1)]
        public string mes_ini_contrato { get; set; }


        [Display(Name = "agno_ini_contrato")]
        [StringLength(4, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 1)]
        public string agno_ini_contrato { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 0)]
        [Display(Name = "dia_caducidad_contrato")]
        public string dia_caducidad_contrato { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 0)]
        [Display(Name = "mes_caducidad_contrato")]
        public string mes_caducidad_contrato { get; set; }

        
        [StringLength(4, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 0)]
        [Display(Name = "agno_caducidad_contrato")]
        public string agno_caducidad_contrato { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "breve_descripcion")]
        public string breve_descripcion { get; set; }

        public string nombreCliente { get; set; }

        public string tieneTermino { get; set; }

        public string servicioSeleccionado { get; set; }

        public static List<ServicioListado> getServiciosDelContrato(int idContrato)
        {
            List<ServicioListado> res = new List<ServicioListado>();
            ServicioListado temp;
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                string query = "SELECT id_contrato, id_servicio, nombre_servicio, precio_acordado, detalle_condicion FROM contrato NATURAL JOIN precio_servicio NATURAL JOIN servicio WHERE id_contrato=" + idContrato;
                lector = DBConector.SELECT(query);

                while (lector.Read())
                {
                    temp = new ServicioListado();
                    temp.id_servicio = lector["id_servicio"];
                    temp.nombre_servicio = lector["nombre_servicio"];
                    temp.precio_acordado = lector["precio_acordado"];
                    temp.descripcion = lector["detalle_condicion"];

                    res.Add(temp);
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


        public static List<SelectListItem> getAllServicios()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "",
                Value = "-1"
            });
            NpgsqlDataReaderWithConection unidades = null;
            try
            {
                unidades = DBConector.SELECT("SELECT id_servicio, nombre_servicio, precio_pizarra FROM servicio");


                while (unidades.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = unidades.GetString(1),
                        Value = unidades.GetInt32(0).ToString()
                    });
                }
            }
            catch (Exception)
            {
                items.Add(new SelectListItem
                {
                    Text = DBConector.msjError,
                    Value = "-1"
                });
            }
            if (unidades != null)
            {
                unidades.CloseTodo();
            }
            return items;

        }


        public static int getPrecioServicio(int idServicio) {
            NpgsqlDataReaderWithConection precio = null;
            int resultado = 0;
            try
            {
                precio = DBConector.SELECT("SELECT precio_pizarra FROM servicio WHERE id_servicio=" + idServicio);

                if (precio.Read())
                {
                    resultado = precio.GetInt32(0);
                }
            }
            catch (Exception)
            {
                resultado = 0;
            }
            if (precio != null)
            {
                precio.CloseTodo();
            }
            return resultado;
        }


        public static Contrato getDetalleContrato(int idContrato)
        {
            Contrato res = null;
            DateTime t;
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT("SELECT contrato.id_contrato, nombre_cliente, breve_descripcion, fecha_inicio_contrato, fecha_caducidad_contrato, rut_cliente FROM contrato NATURAL JOIN cliente WHERE id_contrato=" + idContrato);

                while (lector.Read())
                {
                    res = new Contrato();
                    res.id_contrato = lector["id_contrato"];
                    res.nombreCliente = lector["nombre_cliente"];

                    
                    res.breve_descripcion = lector["breve_descripcion"];
                    t = lector.GetDateTime(3);
                    res.dia_ini_contrato = t.Day.ToString();
                    res.mes_ini_contrato = t.Month.ToString();
                    res.agno_ini_contrato = t.Year.ToString();

                    if (lector.IsDBNull(4))
                    {
                        res.tieneTermino = "false";
                    }
                    else
                    {
                        res.tieneTermino = "true";
                        t = lector.GetDateTime(4);
                        res.dia_caducidad_contrato = t.Day.ToString();
                        res.mes_caducidad_contrato = t.Month.ToString();
                        res.agno_caducidad_contrato = t.Year.ToString();
                    }
                    
                    
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


        public static List<SelectListItem> getAllContratos()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "",
                Value = "-1"
            });
            NpgsqlDataReaderWithConection unidades = null;
            try
            {
                unidades = DBConector.SELECT("SELECT contrato.id_contrato, cliente.nombre_cliente FROM contrato NATURAL JOIN cliente");

                while (unidades.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = unidades.GetInt32(0) + " - Contrato con: " + unidades.GetString(1),
                        Value = unidades.GetInt32(0).ToString()
                    });
                }
            }
            catch (Exception)
            {
            }
            if (unidades != null)
            {
                unidades.CloseTodo();
            }
            return items;

        }



        public static string getNombreCliente(int rut, out bool bienHecho) {
            string query = "SELECT nombre_cliente FROM cliente WHERE rut_cliente = '" + rut + "'";
            string resultado = "";
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                if (lector.Read())
                {
                    bienHecho = true;
                    resultado = lector["nombre_cliente"];

                }
                else
                {
                    bienHecho = false;
                    resultado = "";
                }
            }
            catch (Exception)
            {
                bienHecho = false;
                resultado = "";

            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return resultado;
        }


        public static int insertContrato(int rut_cliente, string fecha_inicio, string fecha_termino, string descripcion, List<ServicioListado> listaServicios)
        {
            NpgsqlDataReaderWithConection lector = null;
            string msjResultado = "";
            int idContrato = 0;

            string tieneTermino = "TRUE";
            if (fecha_termino == null)
            {
                fecha_termino = fecha_inicio;
                tieneTermino = "FALSE";
            }
            string query = "SELECT sp_new_contrato('" + rut_cliente + "', '" + fecha_inicio + "', '" + fecha_termino + "', '" + tieneTermino + "', '" + descripcion + "', ";
            string id_servs = "ARRAY[";
            string precios = "ARRAY[";
            string descripciones = "ARRAY[";
            foreach (ServicioListado temp in listaServicios)
            {
                if (temp != listaServicios[0])
                {
                    id_servs += ", ";
                    precios += ", ";
                    descripciones += ", ";
                }
                id_servs += temp.id_servicio;
                precios += temp.precio_acordado;
                descripciones += "'" + temp.descripcion + "'";
            }
            query += id_servs + "], " + precios + "], " + descripciones + "])";

            try
            {
                lector = DBConector.SELECT(query);

                if (lector.Read())
                {
                    msjResultado = lector.GetString(0);
                }

            }
            catch (Exception)
            {
                msjResultado = "Error al conectar a la base de datos";
            }
            if (lector != null)
            {
                lector.CloseTodo();

            }

            //ACÁ HAGO UNA LLAMADA A LA FUNCIÓN QUE CREA LA RELACIÓN DE SERVICIOS CON CONTRATOS EN LA BASE DE DATOS
            //insertarPrecioServicios(idContrato, listaServicios);


            return 0;

        }

    }



    public class ServicioListado
    {
        public string id_servicio { get; set; }
        public string nombre_servicio { get; set; }
        public string precio_acordado { get; set; }
        public string descripcion { get; set; }
        public string cantidad { get; set; }


        public static string getNombreServicio(int idServicio)
        {
            NpgsqlDataReaderWithConection precio = null;
            string resultado = "";
            try
            {
                precio = DBConector.SELECT("SELECT nombre_servicio FROM servicio WHERE id_servicio=" + idServicio);

                if (precio.Read())
                {
                    resultado = precio.GetString(0);
                }
            }
            catch (Exception)
            {
                resultado = "";
            }
            if (precio != null)
            {
                precio.CloseTodo();
            }
            return resultado;

        }

        
    }
    
}