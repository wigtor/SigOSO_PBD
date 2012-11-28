using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using SigOSO_PBD.classes;

namespace SigOSO_PBD.Models
{
    public class perfilTrabajadorModel
    {


        [Required]
        [StringLength(40, ErrorMessage = "El {0} debe tener al menos {2} caracteres de longitud y máximo {1}", MinimumLength = 1)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        public static List<SelectListItem> getListaPerfilesTrabajadores()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            NpgsqlDataReaderWithConection servicios = null;
            try
            {
                servicios = DBConector.SELECT("SELECT id_perfil, nombre_cargo FROM perfil_trabajador");
                int id_perfil;
                string nombre_cargo;
                while (servicios.Read())
                {
                    id_perfil = servicios.GetInt32(0);
                    nombre_cargo = servicios.GetString(1);
                    items.Add(new SelectListItem
                    {
                        Text = nombre_cargo,
                        Value = id_perfil.ToString()
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
            if (servicios != null)
            {
                servicios.Dispose();
                servicios.Close();
                servicios.closeConection();
            }
            return items;
        }
    }

    
}