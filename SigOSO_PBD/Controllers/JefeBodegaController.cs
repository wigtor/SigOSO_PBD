using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using SigOSO_PBD.classes;
using SigOSO_PBD.Models;
using System.Web.Security;
using System.Globalization;
using System.Collections.Specialized;

namespace SigOSO_PBD.Controllers
{
    [Authorize(Roles = "Jefe_bodega")]
    public class JefeBodegaController : Controller
    {
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "La actual contraseña es incorrecta o la nueva contraseña es inválida");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }



        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RetiroMat() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult RetiroMat(retiroMaterialModel retiroMaterial, string btn_cargar)
        {
            if (btn_cargar!= null)
            {
                int precioInt;
                if (!Int32.TryParse(retiroMaterial.RUT, out precioInt))
                {
                    ModelState.AddModelError("RUT", "El RUT es invalido");
                }
                if (ModelState.IsValid)
                {
                    NpgsqlDataReaderWithConection lector = null;
                    try
                    {
                        string query = "SELECT id_trabajo_interno, nombre_trabajador, glosa_ti FROM trabajador NATURAL JOIN cuadrilla NATURAL JOIN trabajo_interno NATURAL JOIN estado_ot NATURAL JOIN autom_estado WHERE rut_trabajador=" + retiroMaterial.RUT + " AND habilitada=true AND final_normal!=true AND final_inesperado=true AND estado_ot.id_estado=autom_estado.id_estado_pasado AND estado_ot.id_estado=trabajo_interno.id_estado_actual_ti;";
                        lector = DBConector.SELECT(query);
                        if (lector.HasRows)
                        {
                            lector.Read();
                            string id_trabajo_interno = lector["id_trabajo_interno"];
                            string nombre_trabajador = lector["nombre_trabajador"];
                            string glosa_ti = lector["glosa_ti"];

                            List<string> nombre_material = new List<string>();
                            List<string> asignado = new List<string>();
                            List<string> unidad = new List<string>();
                            List<string> disponible = new List<string>();

                            ViewBag.nombre_material = new List<string>();
                            ViewBag.asignado = new List<string>();
                            ViewBag.unidad = new List<string>();
                            ViewBag.disponible = new List<string>();
                            query = "SELECT nombre_tipo_material, abreviatura_unidad, cantidad_asignada, id_detalle_material FROM asignacion_material NATURAL JOIN detalle_material NATURAL JOIN material_generico NATURAL JOIN unidad_material WHERE id_trabajo_interno=" + id_trabajo_interno;
                            lector = DBConector.SELECT(query);
                            NpgsqlDataReaderWithConection lector2 = null;
                            //NpgsqlDataReaderWithConection lector3 = null;
                            string id_detalle_material = "";
                            int cantidad_retirada_temp;
                            if(lector.HasRows){
                                while (lector.Read())
                                {
                                    nombre_material.Add(lector["nombre_tipo_material"]);
                                    asignado.Add(lector["cantidad_asignada"]);
                                    unidad.Add(lector["abreviatura_unidad"]);
                                    id_detalle_material = (lector["id_detalle_material"]);
                                    lector2 = DBConector.SELECT("SELECT SUM(cantidad_retirada) FROM retiro_material NATURAL JOIN detalle_material WHERE id_trabajo_interno=" + id_trabajo_interno + " AND id_detalle_material=" + id_detalle_material);
                                    //lector3 = DBConector.SELECT("SELECT SUM(cantidad_retirada) FROM retiro_material NATURAL JOIN detalle_material WHERE id_trabajo_interno=" + id_trabajo_interno + " AND id_detalle_material=" + id_detalle_material);
                                    
                                    if (lector2.Read())
                                    {
                                        cantidad_retirada_temp=Convert.ToInt32(lector2.GetInt32(0));
                                    }
                                    else {
                                        cantidad_retirada_temp=0;
                                    }
                                    disponible.Add(((Convert.ToInt32(lector["cantidad_asignada"])) - (cantidad_retirada_temp)).ToString());
                                }
                            }


                            ViewBag.id_trabajo_interno = id_trabajo_interno;
                            ViewBag.nombre_trabajador = nombre_trabajador;
                            ViewBag.glosa_ti = glosa_ti;


                            ViewBag.nombre_material = nombre_material;
                            ViewBag.asignado = asignado;
                            ViewBag.unidad = unidad;
                            ViewBag.disponible = disponible;
                        }
                        else {
                            ViewBag.respuestaPost = "No tiene orden de trabajo asiganada";
                            ViewBag.tipoRespuestaPost = "informacion";
                        }

                    }
                    catch
                    { 
                    
                    }
                }
            }
            return View();
        }

        public ActionResult DevolucionMat()
        {
            return View();

        }

        
    }
}
