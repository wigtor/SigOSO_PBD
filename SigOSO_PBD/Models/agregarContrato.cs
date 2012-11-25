﻿using System;
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

        public string servicioSeleccionado { get; set; }


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
                unidades.Dispose();
                unidades.Close();
                unidades.closeConection();
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
                precio.Dispose();
                precio.Close();
                precio.closeConection();
            }
            return resultado;
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
                lector.Dispose();
                lector.Close();
                lector.closeConection();
            }
            return resultado;
        }


        public static int insertContrato(int rut_cliente, string fecha_inicio, string fecha_termino, string descripcion, List<ServicioListado> listaServicios)
        {
            NpgsqlDataReaderWithConection id_clienteLector = null;
            int resultado = 0;
            int id_cliente = 0;
            try
            {
                string query;
                id_clienteLector = DBConector.SELECT("SELECT id_cliente FROM cliente WHERE rut_cliente=" + rut_cliente);

                if (id_clienteLector.Read())
                {
                    id_cliente = id_clienteLector.GetInt32(0);
                }
                if (fecha_termino != null)
                {
                    query = "INSERT INTO contrato(id_cliente, fecha_inicio_contrato, fecha_caducidad_contrato, breve_descripcion) VALUES ('";
                    query += id_cliente + "', '";
                    query += fecha_inicio + "', '";
                    query += fecha_termino + "', '";
                    query += descripcion + "'";
                }
                else
                {
                    query = "INSERT INTO contrato(id_cliente, fecha_inicio_contrato, breve_descripcion) VALUES ('";
                    query += id_cliente + "', '";
                    query += fecha_inicio + "', '";
                    query += descripcion + "')";
                }
                if (id_clienteLector != null)
                {
                    id_clienteLector.CloseTodo();

                }

                resultado = DBConector.INSERT(query);

            }
            catch (Exception)
            {
                resultado = -1;
            }

            //ACÁ HAGO UNA LLAMADA A LA FUNCIÓN QUE CREA LA RELACIÓN DE SERVICIOS CON CONTRATOS EN LA BASE DE DATOS


            return resultado;

        }

    }


    public class ServicioListado
    {
        public string id_servicio { get; set; }
        public string nombre_servicio { get; set; }
        public string precio_acordado { get; set; }
        public string descripcion { get; set; }


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
                precio.Dispose();
                precio.Close();
                precio.closeConection();
            }
            return resultado;

        }

        
    }
    
}