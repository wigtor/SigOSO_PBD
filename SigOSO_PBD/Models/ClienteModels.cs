using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

namespace SigOSO_PBD.Models
{
    public class agregarClienteModel : PersonaModel
    {

        [Display(Name = "Giro")]
        [StringLength(40, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 0)]
        public string giro { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }


        public static List<SelectListItem> getRutsClientes()
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
                unidades = DBConector.SELECT("SELECT id_cliente, rut_cliente FROM cliente");


                while (unidades.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = unidades["rut_cliente"],
                        Value = unidades["id_cliente"]
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

        public static int existeCliente(agregarClienteModel nvoCliente)
        {
            int respuesta;
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                string query2 = "SELECT rut_cliente FROM cliente WHERE rut_cliente = '" + nvoCliente.rut + "'";
                lector = DBConector.SELECT(query2);
                if (lector.HasRows)
                {
                    respuesta = funciones.SI;
                }
                else
                {
                    respuesta = funciones.NO;
                }
            }
            catch (Exception)
            {
                respuesta = funciones.ERROR;
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return respuesta;
        }

        public static int insertarCliente(agregarClienteModel nvoCliente) {
            int cantidadInsertada;
            string query = "INSERT INTO cliente (rut_cliente, nombre_cliente, direccion_cliente, comuna_cliente, giro_cliente, tel1_cliente, tel2_cliente, mail_cliente, ciudad_cliente) VALUES ('" + nvoCliente.rut + "', '" + nvoCliente.nombre + "', '" + nvoCliente.direccion + "', '" + nvoCliente.comuna + "', '" + nvoCliente.giro + "', '" + nvoCliente.telefono1 + "', '" + nvoCliente.telefono2 + "', '" + nvoCliente.correo + "', '"+nvoCliente.ciudad+"')";
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                cantidadInsertada = DBConector.INSERT(query);
            }
            catch (Exception)
            {
                return funciones.ERROR;
            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return funciones.SI;
        }
    }


    
    
}