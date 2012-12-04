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

        [HttpGet] //Lo cambié, no tenia nada
        public ActionResult solicitarMaterial()
        {
            //Para borrar los datos de creaciones pasadas
            if (Session["listaAgregadosMaterialSolicitado"] != null)
            {
                Session["listaAgregadosMaterialSolicitado"] = null;
            }
            NpgsqlDataReaderWithConection lector = null;
            try{
                string query = "SELECT id_trabajo_interno FROM trabajador NATURAL JOIN cuadrilla NATURAL JOIN trabajo_interno NATURAL JOIN estado_ot NATURAL JOIN autom_estado WHERE rut_trabajador=" + Convert.ToInt32(User.Identity.Name) + " AND habilitada=true AND final_normal!=true AND final_inesperado=true AND estado_ot.id_estado=autom_estado.id_estado_pasado AND estado_ot.id_estado=trabajo_interno.id_estado_actual_ti;";
                
                lector = DBConector.SELECT(query);
                if (lector.HasRows)
                {
                    int rut_jefe_cuadrilla = Convert.ToInt32(User.Identity.Name);
                    Session["rut_usuario"] = rut_jefe_cuadrilla;
                    ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                    ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
                    ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                    lector.CloseTodo();
                    return View();
                }else{

                    ViewBag.respuestaPost = "No tiene orden de trabajo asiganada";
                    ViewBag.tipoRespuestaPost = "informacion";
                    ViewBag.NumeroOrden = "<script type='text/javascript'>$('#form1').css('display','none');</script>";
                    ModelState.Clear();
                    lector.CloseTodo();
                    return View();
                }
            }
            catch
            {
                lector.CloseTodo();
            }


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
            ModelState.Clear();
            int precioInt;
            if (!Int32.TryParse(servicio.cantidad, out precioInt))
            {
                ModelState.AddModelError("cantidad", "La cantidad ingresada debe ser un numero entero positivo");
            }
            else if (Convert.ToInt32(servicio.cantidad)<=0)
            {
                ModelState.AddModelError("cantidad", "La cantidad ingresada debe ser un numero entero positivo");
            }
            if (servicio.id == null || servicio.id == "-10")
            {
                ModelState.AddModelError("tipo", "ingresa un material valido");
            }
            if (ModelState.IsValid)
            {
                if (!materialYaExistente(servicio))//material no solicitado
                {
                    List<solicitudMaterialModels> listaAgregadosMaterialSolicitado = (List<solicitudMaterialModels>)Session["listaAgregadosMaterialSolicitado"];
                    listaAgregadosMaterialSolicitado.Add(servicio);
                    Session["listaAgregadosMaterialSolicitado"] = listaAgregadosMaterialSolicitado;
                    ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                    int rut_jefe_cuadrilla = Convert.ToInt32(User.Identity.Name);
                    ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                    ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
                    ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                    ViewBag.respuestaPost = "El tipo de material " + servicio.tipo + " se  agregado a la lista de solicitudes de manera satisfactoria";
                    ViewBag.tipoRespuestaPos = "informacion";
                    return View();
                }
                else
                {
                    ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                    int rut_jefe_cuadrilla = Convert.ToInt32(User.Identity.Name);
                    ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                    ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
                    ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                    ViewBag.respuestaPost = "El tipo de material " + servicio.tipo + " ya se ha sido solicitada";
                    ViewBag.tipoRespuestaPos = "advertencia";
                    return View();
                }
            }
            else {
                ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                int rut_jefe_cuadrilla = Convert.ToInt32(User.Identity.Name);
                ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
                ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
                ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                return View();
            }

        }

        public ActionResult eliminar_nuevo_material(solicitudMaterialModels servicio)
        {
            ModelState.Clear();
            int rut_jefe_cuadrilla = Convert.ToInt32(User.Identity.Name);
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
                        ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
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
                ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
                ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
                ViewBag.respuestaPost = "Error al eliminar " + servicio.tipo;
                ViewBag.tipoRespuestaPos = "advertencia";
                return View();
            }
            ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
            ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden(rut_jefe_cuadrilla);
            ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial(rut_jefe_cuadrilla, User.Identity.Name);
            ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales(rut_jefe_cuadrilla);
            return View();
        }

        [HttpPost]
        public ActionResult guardar_solicitud() {
            ModelState.Clear();
            if (Session["listaAgregadosMaterialSolicitado"] != null)
            {
                List<solicitudMaterialModels> listaAgregadosMaterialSolicitado = (List<solicitudMaterialModels>)Session["listaAgregadosMaterialSolicitado"];
                NpgsqlDataReaderWithConection lector = null;
                try
                {
                    string cod_producto = "ARRAY[" + listaAgregadosMaterialSolicitado[0].id;
                    string cantidad_solicitada = "ARRAY[" + listaAgregadosMaterialSolicitado[0].cantidad;
                    string comentarios_jefe_cuadrilla = "ARRAY['" + listaAgregadosMaterialSolicitado[0].detalle + "'";
                    for (int i = 1; i < listaAgregadosMaterialSolicitado.Count; i++)
                    {
                        cod_producto += "," + listaAgregadosMaterialSolicitado[i].id;
                        cantidad_solicitada += "," + listaAgregadosMaterialSolicitado[i].cantidad;
                        comentarios_jefe_cuadrilla += ",'" + listaAgregadosMaterialSolicitado[i].detalle + "'";
                    }
                    cod_producto += "]";
                    cantidad_solicitada += "]";
                    comentarios_jefe_cuadrilla += "]";
                    string query = "SELECT sp_solicitud_material(" + 1 + ", " + cod_producto + ", " + (int)Session["rut_usuario"] + "," + cantidad_solicitada + "," + comentarios_jefe_cuadrilla + ")";
                    lector = DBConector.SELECT(query);
                    ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden((int)Session["rut_usuario"]);
                    ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial((int)Session["rut_usuario"], User.Identity.Name);
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
                    ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial((int)Session["rut_usuario"], User.Identity.Name);
                    ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales((int)Session["rut_usuario"]);
                    ViewBag.respuestaPost = "Error al enviar la solicitud, vuelva a intentarlo";
                    ViewBag.tipoRespuestaPos = "error";
                    lector.CloseTodo();
                    return View();
                }
            }
            else
            {
                ViewBag.listaAgregadosMaterialSolicitado = Session["listaAgregadosMaterialSolicitado"];
                ViewBag.NumeroOrden = solicitudMaterialModels.generarNumeroOrden((int)Session["rut_usuario"]);
                ViewBag.MaterialesAsignados = materialSolicitado.getSolicitudMaterial((int)Session["rut_usuario"], User.Identity.Name);
                ViewBag.SolicitudDeMateriales = solicitudMaterialModels.generarSolicitudDeMateriales((int)Session["rut_usuario"]);
                ViewBag.respuestaPost = "Debe solicitar por lo menos";
                ViewBag.tipoRespuestaPos = "error";
                return View();
            }
        }

        [HttpPost]
        public ActionResult solicitarMaterial(solicitudMaterialModels servicio, string btn_agregar_servicio, string btn_solicitar)
        {
            if (btn_agregar_servicio != null)//agregamos nuevo servicio
            {                
                return (agregar_nuevo_material(servicio));
            }
            else if (btn_solicitar != null)
            {
                return guardar_solicitud();
            } else
            {                
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

        [Authorize]
        public ActionResult VerSolicitudesMat()
        {
            NpgsqlDataReaderWithConection lector = null;
            
            int rut_jefe_cuadrilla = Convert.ToInt32(User.Identity.Name);
            string query1 = "SELECT id_trabajo_interno FROM trabajador NATURAL JOIN cuadrilla NATURAL JOIN trabajo_interno NATURAL JOIN estado_ot NATURAL JOIN autom_estado WHERE rut_trabajador=" + rut_jefe_cuadrilla + " AND habilitada=true AND final_normal!=true AND final_inesperado=true AND estado_ot.id_estado=autom_estado.id_estado_pasado AND estado_ot.id_estado=trabajo_interno.id_estado_actual_ti;";
            lector = DBConector.SELECT(query1);
            if(lector.Read()){                
                string query2 = "SELECT cantidad_solicitada, fecha_solicitud_material, cantidad_aprobada_material, fecha_respuesta, comentarios_jefe_cuadrilla, comentarios_supervisor, revisada, nombre_tipo_material  FROM solicitud_material NATURAL JOIN detalle_material NATURAL JOIN material_generico WHERE id_trabajo_interno = " + lector["id_trabajo_interno"];
            
                lector = DBConector.SELECT(query2);
                
                List<string> cantidad_solicitada = new List<string>();
                List<string> fecha_solicitud_material = new List<string>();
                List<string> cantidad_aprobada_material = new List<string>();
                List<string> fecha_respuesta = new List<string>();
                List<string> comentarios_jefe_cuadrilla = new List<string>();
                List<string> comentarios_supervisor = new List<string>();
                List<string> revisada = new List<string>();
                List<string> nombre_tipo_material = new List<string>();
                
                while(lector.Read()){
                    cantidad_solicitada.Add(lector["cantidad_solicitada"]);
                    fecha_solicitud_material.Add(lector["fecha_solicitud_material"]);
                    cantidad_aprobada_material.Add(lector["cantidad_aprobada_material"]);
                    fecha_respuesta.Add(lector["fecha_respuesta"]);
                    comentarios_jefe_cuadrilla.Add(lector["comentarios_jefe_cuadrilla"]);
                    comentarios_supervisor.Add(lector["comentarios_supervisor"]);
                    revisada.Add(lector["revisada"]);
                    nombre_tipo_material.Add(lector["nombre_tipo_material"]);
                    
                }
                ViewBag.cantidad_solicitada = cantidad_solicitada;
                ViewBag.fecha_solicitud_material = fecha_solicitud_material;
                ViewBag.cantidad_aprobada_material = cantidad_aprobada_material;
                ViewBag.fecha_respuesta = fecha_respuesta;
                ViewBag.comentarios_jefe_cuadrilla = comentarios_jefe_cuadrilla;
                ViewBag.comentarios_supervisor = comentarios_supervisor;
                ViewBag.revisada = revisada;
                ViewBag.nombre_tipo_material = nombre_tipo_material;
                
            }
            ViewBag.respuestaPost = "No tiene orden de trabajo asiganada";
            lector.CloseTodo();
            return View();
        }
    }
}
