using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

namespace SigOSO_PBD.Models
{
    public class OrdenTrabajoModel
    {

        [Required]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 1)]
        [Display(Name = "Cliente")]
        public string cliente { get; set; }

        [Required]
        [Display(Name = "N° de contrato")]
        public string contrato { get; set; }

        [Display(Name = "Descripción del contrato")]
        public string descripcion_contrato { get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public string ciudad_ot { get; set; }

        [Required]
        [Display(Name = "Comuna")]
        public string comuna_ot { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string direccion_ot { get; set; }

        [Display(Name = "dia_ini_contrato")]
        [StringLength(2, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 1)]
        public string dia_ini_ot { get; set; }

        [Display(Name = "mes_ini_contrato")]
        [StringLength(2, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 1)]
        public string mes_ini_ot { get; set; }


        [Display(Name = "agno_ini_contrato")]
        [StringLength(4, ErrorMessage = "El {0} debe tener máximo {1} caracteres de longitud", MinimumLength = 1)]
        public string agno_ini_ot { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 0)]
        [Display(Name = "dia_caducidad_contrato")]
        public string dia_fin_ot { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 0)]
        [Display(Name = "mes_caducidad_contrato")]
        public string mes_fin_ot { get; set; }

        
        [StringLength(4, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 0)]
        [Display(Name = "agno_caducidad_contrato")]
        public string agno_fin_ot { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}.", MinimumLength = 2)]
        [Display(Name = "Descripción del trabajo")]
        public string breve_descripcion { get; set; }


        public string servicioSeleccionado { get; set; }

        [Display(Name = "Precio según contrato")]
        public string precioReferenciaContrato { get; set; }

        [Display(Name = "Cantidad")]
        public string cantidadDelServicio { get; set; }

        [Display(Name = "Precio final")]
        public string precioFinal { get; set; }

        public static List<SelectListItem> getClientes()
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
                unidades = DBConector.SELECT("SELECT id_cliente, rut_cliente, nombre_cliente FROM cliente");


                while (unidades.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = unidades["rut_cliente"] + " - " + unidades["nombre_cliente"],
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

        public static List<Contrato> getContratosSegunCliente(int id_cliente)
        {

            List<Contrato> items = new List<Contrato>();
            NpgsqlDataReaderWithConection contratos = null;
            Contrato temp;
            try
            {
                contratos = DBConector.SELECT("SELECT id_contrato, breve_descripcion FROM contrato WHERE fecha_inicio_contrato < '" + DateTime.Now + "' AND (fecha_caducidad_contrato > '" + DateTime.Now + "' OR fecha_caducidad_contrato IS NULL) AND id_cliente = " + id_cliente);

                while (contratos.Read())
                {
                    temp = new Contrato();
                    temp.breve_descripcion = contratos["breve_descripcion"];
                    temp.id_contrato = contratos["id_contrato"];
                    items.Add(temp);
                }
            }
            catch (Exception)
            {
            }
            if (contratos != null)
            {
                contratos.CloseTodo();
            }
            return items;
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

    }


    public class servicioContrato
    {
        public string id_servicio;
        public string nombre_servicio;

        public static List<SelectListItem> getServiciosDeContrato(int id_contrato) {
            List<SelectListItem> items = new List<SelectListItem>();
            NpgsqlDataReaderWithConection unidades = null;
            try
            {
                unidades = DBConector.SELECT("SELECT id_servicio, nombre_servicio FROM servicio NATURAL JOIN precio_servicio NATURAL JOIN contrato WHERE contrato.id_contrato = "+id_contrato);
                while (unidades.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = unidades["nombre_servicio"],
                        Value = unidades["id_servicio"]
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

        public static int getPrecioAcordadoServicio(int id_servicio, int id_contrato)
        {
            NpgsqlDataReaderWithConection precio = null;
            int resultado = 0;
            try
            {
                precio = DBConector.SELECT("SELECT precio_acordado FROM precio_servicio WHERE id_servicio=" + id_servicio + " AND id_contrato="+id_contrato);

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

    }
}