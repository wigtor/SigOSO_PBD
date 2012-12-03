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
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        public ActionResult CrearCuadrilla()
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

        //
        // GET: /Supervisor/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AsigMat()
        {

            string query = "SELECT * FROM solicitud_material NATURAL JOIN trabajo_interno NATURAL JOIN cuadrilla NATURAL JOIN trabajador NATURAL JOIN detalle_material NATURAL JOIN material_generico WHERE revisada=false";
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                ViewBag.fecha_solicitud_material = new List<string>();
                ViewBag.rut_trabajador = new List<string>();
                ViewBag.nombre_tipo_material = new List<string>();
                ViewBag.id_orden = new List<string>();
                ViewBag.id_trabajo_interno = new List<string>();

                ViewBag.id_solicitud_material = new List<string>();
                ViewBag.glosa_tipo_material = new List<string>();
                
                ViewBag.cantidad_solicitada = new List<string>();
                ViewBag.comentarios_jefe_cuadrilla = new List<string>();
                while (lector.Read())
                {
                    ViewBag.fecha_solicitud_material.Add(lector["fecha_solicitud_material"].ToString());//0
                    ViewBag.rut_trabajador.Add(lector["rut_trabajador"].ToString());//1
                    ViewBag.nombre_tipo_material.Add(lector["nombre_tipo_material"].ToString());//2
                    ViewBag.id_orden.Add(lector["id_orden"].ToString());//3
                    ViewBag.id_trabajo_interno.Add(lector["id_trabajo_interno"].ToString());//4
                    ViewBag.id_solicitud_material.Add(lector["id_solicitud_material"].ToString());//5
                    ViewBag.cantidad_solicitada.Add(lector["cantidad_solicitada"].ToString());//6
                        ViewBag.comentarios_jefe_cuadrilla.Add(lector["comentarios_jefe_cuadrilla"].ToString());//7
                    ViewBag.glosa_tipo_material.Add(lector["glosa_tipo_material"].ToString());//8
                }
            }
            catch (Exception)
            {
                //DBConector.msjError;
                lector.CloseTodo();
                return View();//ex.Message;
            }
            lector.CloseTodo();
            return View();
        }

        [HttpPost]
        public ActionResult AsigMat(AsigMatSupervisorModels respuesta){
            
            string query = "UPDATE solicitud_material SET cantidad_aprobada_material="+respuesta.cantidad_asignada+", fecha_respuesta='"+ DateTime.Now +"', comentarios_supervisor='"+respuesta.detalle_respuesta+"', revisada=true WHERE id_solicitud_material="+respuesta.id_solicitud;
            try
            {
                DBConector.UPDATE(query);                
            }
            catch 
            {
             
            }
        
            return View();
        }
        public ActionResult AsigOTI()
        {
            return View();
        }

        public ActionResult BuscarCuadrilla()
        {
            return View();
        }

        public ActionResult EliminarCuadrilla()
        {
            return View();
        }

        public ActionResult EstadosOTI()
        {
            return View();
        }

        public ActionResult GenOTI()
        {
            return View();
        }

        public ActionResult ModificarCuadrilla()
        {
            return View();
        }

        public ActionResult modPerfil()
        {
            return View();
        }

     
    }
}
