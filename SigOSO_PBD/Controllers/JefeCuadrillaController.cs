/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using SigOSO_PBD.Models;
using System.Web.Mvc;
using System.Globalization;
using System.Collections.Specialized;*/

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
    [Authorize(Roles = "Jefe_cuadrilla")]
    public class JefeCuadrillaController : Controller
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

        //
        // GET: /JefeCuadrilla/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Trabajador()
        {
            return View();
        }
        public ActionResult solicitarMaterial()
        {
            //Para borrar los datos de creaciones pasadas
            if (Session["listaAgregadosMaterialSolicitado"] != null)
            {
                Session["listaAgregadosMaterialSolicitado"] = null;
            }

            int rut_jefe_cuadrilla = 123124121;
            Session["rut_usuario"]=rut_jefe_cuadrilla;
            ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
            ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados(rut_jefe_cuadrilla);
            ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
            return View();
        }

        public bool materialYaExistente(solicitudMaterialModels servicio)
        {
            List<solicitudMaterialModels> listaAgregadosMaterialSolicitado = (List<solicitudMaterialModels>)Session["listaAgregadosMaterialSolicitado"];
            if (listaAgregadosMaterialSolicitado == null)
            {
                Session["listaAgregadosMaterialSolicitado"] = new List<solicitudMaterialModels>();
                return false;
            }
            else
            {
                for (int i = 0; i < listaAgregadosMaterialSolicitado.Count; i++)
                {
                    if (listaAgregadosMaterialSolicitado[i].id == servicio.id)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public ActionResult agregar_nuevo_material(solicitudMaterialModels servicio)
        {
            if (!materialYaExistente(servicio))//material no solicitado
            {
                List<solicitudMaterialModels> listaAgregadosMaterialSolicitado = (List<solicitudMaterialModels>)Session["listaAgregadosMaterialSolicitado"];
                listaAgregadosMaterialSolicitado.Add(servicio);
                Session["listaAgregadosMaterialSolicitado"] = listaAgregadosMaterialSolicitado;
                ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                int rut_jefe_cuadrilla = 123124121;
                ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados(rut_jefe_cuadrilla);
                ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                ViewBag.respuestaPost = "El tipo de material " + servicio.tipo + " se  agregado a la lista de solicitudes de manera satisfactoria";
                ViewBag.tipoRespuestaPos = "informacion";
                return View();
            }
            else {
                ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                int rut_jefe_cuadrilla = 123124121;
                ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados(rut_jefe_cuadrilla);
                ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                ViewBag.respuestaPost = "El tipo de material " + servicio.tipo + " ya se ha sido solicitada";
                ViewBag.tipoRespuestaPos = "advertencia";
                return View();
            }
        }

        public ActionResult eliminar_nuevo_material(solicitudMaterialModels servicio)
        {
            int rut_jefe_cuadrilla = 123124121;
            string nombreParam = "", valorParam = "", id = "";
            NameValueCollection col = Request.Params;
            for (int i = 0; i < Request.Params.Count; i++)
            {
                nombreParam = col.GetKey(i);
                if (nombreParam.Contains("eliminar_")) {
                    valorParam = col.Get(i);
                    id = nombreParam.Substring("eliminar_".Length);
                    break;
                }
            }
            if (id != "")
            {
                List<solicitudMaterialModels> listaAgregadosMaterialSolicitado = (List<solicitudMaterialModels>)Session["listaAgregadosMaterialSolicitado"];
                for (int i = 0; i < listaAgregadosMaterialSolicitado.Count; i++)
                {
                    if (listaAgregadosMaterialSolicitado[i].id == id)
                    {
                        listaAgregadosMaterialSolicitado.RemoveAt(i);
                        ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                        ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                        ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados(rut_jefe_cuadrilla);
                        ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                        ViewBag.respuestaPost = servicio.tipo + " eliminado de manera satisfactoria";
                        ViewBag.tipoRespuestaPos = "informacion";
                        return View();
                    }
                }
            }
            else {
                ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados(rut_jefe_cuadrilla);
                ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                ViewBag.respuestaPost = "Error al eliminar " + servicio.tipo;
                ViewBag.tipoRespuestaPos = "advertencia";
                return View();
            }
            ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
            ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
            ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados(rut_jefe_cuadrilla);
            ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
            return View();
        }

        [HttpPost]
        public ActionResult guardar_solicitud() {
            if (Session["listaAgregadosMaterialSolicitado"]!=null)
            {
                List<solicitudMaterialModels> listaAgregadosMaterialSolicitado = (List<solicitudMaterialModels>)Session["listaAgregadosMaterialSolicitado"];
                NpgsqlDataReaderWithConection lector = null;
                try
                {                    
                    string cod_producto="ARRAY["+listaAgregadosMaterialSolicitado[0].id;
                    string cantidad_solicitada="ARRAY["+listaAgregadosMaterialSolicitado[0].cantidad;
                    string comentarios_jefe_cuadrilla="ARRAY['"+listaAgregadosMaterialSolicitado[0].detalle+"'";
                    for (int i = 1; i < listaAgregadosMaterialSolicitado.Count; i++){
                        cod_producto += "," + listaAgregadosMaterialSolicitado[i].id;
                        cantidad_solicitada += "," + listaAgregadosMaterialSolicitado[i].cantidad;
                        comentarios_jefe_cuadrilla += ",'" + listaAgregadosMaterialSolicitado[i].detalle + "'";                                            
                    }
                    cod_producto+="]";
                    cantidad_solicitada+="]";
                    comentarios_jefe_cuadrilla+="]";
                    string query = "SELECT sp_solicitud_material(" + 1 + ", " + cod_producto + ", " + (int)Session["rut_usuario"] + "," + cantidad_solicitada + "," + comentarios_jefe_cuadrilla + ")";
                    lector = DBConector.SELECT(query);
                    ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden((int)Session["rut_usuario"]);
                    ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados((int)Session["rut_usuario"]);
                    ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales((int)Session["rut_usuario"]);
                    ViewBag.respuestaPost = "La solicitud de materiales se realizo de manera satisfactoria";
                    ViewBag.tipoRespuestaPos = "satisfactorio";
                    lector.CloseTodo();
                    return View();                                        
                }
                catch 
                {
                    ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                    ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden((int)Session["rut_usuario"]);
                    ViewBag.MaterialesAsignados = solicitudMaterialModels.generarMaterialesAsignados((int)Session["rut_usuario"]);
                    ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales((int)Session["rut_usuario"]);
                    ViewBag.respuestaPost = "Error al enviar la solicitud, vuelva a intentarlo";
                    ViewBag.tipoRespuestaPos = "error";
                    lector.CloseTodo();
                    return View();
                }
            }
            return solicitarMaterial();
        }

        [HttpPost]
        public ActionResult solicitarMaterial(solicitudMaterialModels servicio, string btn_agregar_servicio, string btn_solicitar)
        {
            if (btn_agregar_servicio != null)//agregamos nuevo servicio
            {
                ModelState.Clear();
                return (agregar_nuevo_material(servicio));
            }
            else if (btn_solicitar != null)
            {
                ModelState.Clear();
                return guardar_solicitud();
            } else
            {
                
                ModelState.Clear();
                return (eliminar_nuevo_material(servicio));
            }
        }
        
        public ActionResult CambiarEstadoOTI()
        {
            return View();
        }
        public ActionResult ModificarPerfil()
        {
            return View();
        }
    }
}
