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
    [Authorize(Roles = "administrador")]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult verUsuarios()
        {
            ViewBag.listaUsuarios = Membership.GetAllUsers();
            return View();
        }

        [HttpPost]
        public ActionResult verUsuarios(RegisterModel t)
        {

            string username;
            NameValueCollection col = Request.Params;
            string nombreParam = "";
            for (int i = 0; i < Request.Params.Count; i++)
            {
                nombreParam = col.GetKey(i); //Con esto accedo al nombre del parámetro
                if (nombreParam.Contains("quitar_")) //Con esto omito los parámetros que no me importan
                {
                    username = nombreParam.Substring("quitar_".Length);
                    if (Membership.DeleteUser(username))
                    {
                        ViewBag.respuestaPost = "Se ha eliminado satisfactoriamente el usuario";
                        ViewBag.tipoRespuestaPost = "satisfactorio";
                    }
                    else
                    {
                        ViewBag.respuestaPost = "Ha ocurrido un error al eliminar el usuario";
                        ViewBag.tipoRespuestaPost = "error";
                    }
                }
            }
            


            ViewBag.listaUsuarios = Membership.GetAllUsers();
            return View();
        }



        public ActionResult Register()
        {
            List<SelectListItem> tiposDeUsuario = new List<SelectListItem>();
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "administrador",
                Value = "Administrador"
            });
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Jefe de bodega",
                Value = "Jefe_bodega"
            });
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Jefe de cuadrilla",
                Value = "Jefe_cuadrilla"
            });
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Supervisor",
                Value = "Supervisor"
            });
            ViewBag.listaTiposUsuarios = tiposDeUsuario;


            ViewBag.listaRuts = ListarTrabajadorModel.getAllTrabajadores();


            return View();
        }


        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            List<SelectListItem> tiposDeUsuario = new List<SelectListItem>();
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Administrador",
                Value = "Administrador"
            });
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Jefe de bodega",
                Value = "Jefe_bodega"
            });
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Jefe de cuadrilla",
                Value = "Jefe_cuadrilla"
            });
            tiposDeUsuario.Add(new SelectListItem
            {
                Text = "Supervisor",
                Value = "Supervisor"
            });
            ViewBag.listaTiposUsuarios = tiposDeUsuario;


            if (model.tipoUsuario.Equals("Jefe_cuadrilla"))
            {
                int enteroTemp;
                if (Int32.TryParse(model.rut, out enteroTemp))
                {
                    if (enteroTemp < 0)
                        ModelState.AddModelError("rut", "El rut no es válido");
                    model.UserName = model.rut;

                }
                else
                {
                    ModelState.AddModelError("rut", "El rut no es válido");
                }
            }
            else
            {
                if (!ModelState.IsValidField("rut"))
                {
                    ModelState.Remove("rut");
                }
            }

            
            if (ModelState.IsValid)
            {
                
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                try
                {
                    Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
                    Roles.AddUserToRole(model.UserName, model.tipoUsuario);
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        //FormsAuthentication.SetAuthCookie(model.UserName, false); //Hace que ahora quede logueado con el usuario recién creado
                        ViewBag.RespuestaPost = "Usuario creado correctamente";
                        ViewBag.tipoRespuestaPos = "satisfactorio";
                    }
                    else
                    {
                        ModelState.AddModelError("", AccountController.ErrorCodeToString(createStatus));
                    }
                }
                catch (Exception) {
                    ModelState.AddModelError("", "Ha ocurrido un error al crear el nuevo usuario");
                }
                
            }

            // If we got this far, something failed, redisplay form
            ViewBag.listaRuts = ListarTrabajadorModel.getAllTrabajadores();
            return View(model);
        }


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


        [Authorize]
        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        //Para hacer POST
        [HttpPost]
        public ActionResult AgregarCliente(agregarClienteModel nvoCliente)
        {  
            if (ModelState.IsValid)
            {
                int respuesta;
                respuesta = agregarClienteModel.existeCliente(nvoCliente);
                if (respuesta == funciones.SI )
                {
                    ModelState.AddModelError("rut", "Ya existe un cliente con ese rut");
                    ViewBag.respuestaPost = "";
                    return View(nvoCliente);
                }
                else if (respuesta == funciones.ERROR)
                {
                    ViewBag.respuestaPost = DBConector.msjError;
                    return View(nvoCliente);
                }

                respuesta = agregarClienteModel.insertarCliente(nvoCliente);
                if (respuesta == funciones.ERROR)
                {
                    ViewBag.respuestaPost = DBConector.msjError;
                    return View(nvoCliente);
                }
                ViewBag.respuestaPost = "Se ha creado correctamente el cliente";
                return View();
                //return RedirectToAction("AgregarCliente", "home");
            }
            else
            {
                return View(nvoCliente);
            }
        }

        //Para visualizar
        [HttpGet]
        public ActionResult AgregarCliente()
        {
            return View();
        }


        [HttpGet]
        public ActionResult ModificarCliente()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ModificarCliente(agregarClienteModel clienteMod, string btn_submit)
        {
            if (btn_submit == null)
            {
                return View(clienteMod);
            }

            if (btn_submit.Equals("Cargar")) //Se esta cargando un cliente
            {
                if (ModelState.IsValidField("rut"))
                {
                    string query = "SELECT * FROM cliente WHERE rut_cliente = '" + clienteMod.rut + "'";
                    NpgsqlDataReaderWithConection lector = null;
                    try
                    {
                        lector = DBConector.SELECT(query);
                        if (lector.Read())
                        {
                            ModelState.Clear();
                            clienteMod.rut = lector.GetInt32(lector.GetOrdinal("rut_cliente")).ToString();
                            clienteMod.nombre = lector.GetString(lector.GetOrdinal("nombre_cliente"));
                            clienteMod.telefono1 = lector.GetString(lector.GetOrdinal("tel1_cliente"));
                            clienteMod.telefono2 = lector.GetString(lector.GetOrdinal("tel2_cliente"));
                            clienteMod.correo = lector.GetString(lector.GetOrdinal("mail_cliente"));
                            clienteMod.direccion = lector.GetString(lector.GetOrdinal("direccion_cliente"));
                            clienteMod.comuna = lector.GetString(lector.GetOrdinal("comuna_cliente"));
                            clienteMod.ciudad = lector.GetString(lector.GetOrdinal("ciudad_cliente"));
                            clienteMod.giro = lector.GetString(lector.GetOrdinal("giro_cliente"));
                            lector.Dispose();
                            lector.Close();
                            lector.closeConection();
                            return View(clienteMod);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut", "El rut insertado no existe");
                        }
                        
                    }
                    catch (Exception) {

                    }
                    if (lector != null)
                    {
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                    }
                }
                else
                {
                    string mensaje = "El rut ingresado no es válido";
                    ModelState.Clear();
                    ModelState.AddModelError("rut", mensaje);
                }
            }
            else if (btn_submit.Equals("Guardar cambios")) //Se presionó el botón para guardar cambios
            {
                if (ModelState.IsValid)
                {

                    string query = "UPDATE cliente SET nombre_cliente='" + clienteMod.nombre + "', direccion_cliente='" + clienteMod.direccion + "', comuna_cliente='" + clienteMod.comuna + "', giro_cliente='" + clienteMod.giro + "', tel1_cliente='" + clienteMod.telefono1 + "', tel2_cliente='" + clienteMod.telefono2 + "', mail_cliente='" + clienteMod.correo + "', ciudad_cliente='" + clienteMod.ciudad + "' WHERE rut_cliente='"+clienteMod.rut+"'";

                    try
                    {
                        

                        int cantidadInsertada = DBConector.UPDATE(query);

                        ViewBag.respuestaPost = "Se han guardado correctamente los datos del cliente";
                    }
                    catch (Exception)
                    {
                        ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                    }

                    return View();
                    //return RedirectToAction("Index", "home");
                }
                else
                {
                    return View(clienteMod);
                }


            }
            else //Se presionó cualquier otra cosa, no se usa
            {

            }
            return View();
        }


        //Para visualizar
        [HttpGet]
        public ActionResult AgregarTrabajador()
        {

            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            return View();
        }


        [HttpPost]
        public ActionResult AgregarTrabajador(agregarTrabajadorModel nvoTrabajador)
        {

            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();

            int diaEscogido, mesContrato, agno_contrato, diasDelMes;

            if (!Int32.TryParse(nvoTrabajador.dia_ini_contrato, out diaEscogido))
            {
                ModelState.AddModelError("dia_ini_contrato", "No ha seleccionado un día del mes");
                return View(nvoTrabajador);
            }
            if (!Int32.TryParse(nvoTrabajador.mes_ini_contrato, out mesContrato))
            {
                ModelState.AddModelError("mes_ini_contrato", "No ha seleccionado un mes");
                return View(nvoTrabajador);
            }
            if (!Int32.TryParse(nvoTrabajador.agno_ini_contrato, out agno_contrato))
            {
                ModelState.AddModelError("agno_ini_contrato", "El año introducido no es válido");
                return View(nvoTrabajador);
            }

            diasDelMes = DateTime.DaysInMonth(agno_contrato, mesContrato);
            if (diasDelMes < diaEscogido)
            {
                ModelState.AddModelError("dia_ini_contrato", "El día seleccionado no es válido para el més seleccionado");
                return View(nvoTrabajador);
            }

            if (agno_contrato < 1900 || agno_contrato > 2100)
            {
                ModelState.AddModelError("agno_ini_contrato", "Que año más extraño, ¿Está seguro?");
                return View(nvoTrabajador);
            }

            if (mesContrato < 1 || mesContrato > 12)
            {
                ModelState.AddModelError("mes_ini_contrato", "El més seleccionado no es válido");
                return View(nvoTrabajador);
            }

            if (ModelState.IsValid)
            {
                
                string fecha_ini_contrato = nvoTrabajador.dia_ini_contrato+"-"+nvoTrabajador.mes_ini_contrato+"-"+nvoTrabajador.agno_ini_contrato;

                string query = "INSERT INTO trabajador (id_perfil, rut_trabajador, nombre_trabajador, iniciales_trabajador, direccion_trabajador, comuna_trabajador, tel1_trabajador, tel2_trabajador, mail_trabajador, fecha_ini_contrato_trabajador, esta_activo) VALUES ( '" + nvoTrabajador.id_perfil + "','" + nvoTrabajador.rut + "', '" + nvoTrabajador.nombre + "', '" + nvoTrabajador.iniciales + "', '" + nvoTrabajador.direccion + "', '" + nvoTrabajador.comuna + "', '" + nvoTrabajador.telefono1 + "', '" + nvoTrabajador.telefono2 + "', '" + nvoTrabajador.correo + "', '"+fecha_ini_contrato+"', 'TRUE')";
                NpgsqlDataReaderWithConection lector = null;
                try
                {
                    string query2 = "SELECT rut_trabajador FROM trabajador WHERE rut_trabajador = '" + nvoTrabajador.rut + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("rut", "Ya existe un trabajador con ese rut");
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                        ViewBag.respuestaPost = "";
                        return View(nvoTrabajador);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha creado correctamente el trabajador";
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }
                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }
                return View();
                //return RedirectToAction("AgregarTrabajador", "home");
            }
            else
            {
                return View(nvoTrabajador);
            }
        }


        [HttpGet]
        public ActionResult agregarContrato()
        {
            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = Contrato.getAllServicios();
            ViewBag.precioReferencia = "0";
            if (Session["listaServicios"] != null)
            {
                ((List<ServicioListado>)Session["listaServicios"]).Clear();
                Session["listaServicios"] = null;
            }
            return View();
        }


        [HttpPost]
        public ActionResult agregarContrato(Contrato nvoContrato, string nombre_cliente, string btn_cargar, string btn_agregarServicio, string precioPorContrato, string condicion_servicio, string btn_agregarContrato, string tieneTermino_contrato)
        {
            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = Contrato.getAllServicios();
            int idServicio = 0;
            if (Int32.TryParse(nvoContrato.servicioSeleccionado, out idServicio))
            {
                //ViewBag.selectedServicio = idServicio.ToString();
                if (idServicio > 0)
                {
                    ViewBag.precioReferencia = Contrato.getPrecioServicio(idServicio).ToString();
                }
                else
                    ViewBag.precioReferencia = "0";
            }
            else
            {
                ViewBag.precioReferencia = "0";
            }

            if (btn_cargar != null)
            {

                ViewBag.listaServiciosAgregados = Session["listaServicios"];
                
                if (btn_cargar.Equals("Cargar")) //Se esta cargando un cliente
                {
                    if (ModelState.IsValidField("rutCliente"))
                    {
                        int rut = 0;
                        bool satisf = false;
                        if (!Int32.TryParse(nvoContrato.rutCliente, out rut)) {
                            ModelState.Clear();
                            ModelState.AddModelError("rutCliente", "El rut ingresado no es válido");
                            ViewBag.nombreCliente = "";
                        }
                        else {
                            ViewBag.nombreCliente = Contrato.getNombreCliente(rut, out satisf);
                            if (!satisf)
                            {
                                ModelState.Clear();
                                ModelState.AddModelError("rutCliente", "El rut insertado no existe");
                            }
                        }
                    }
                    else
                    {
                        string mensaje = "El rut ingresado no es válido";
                        ModelState.Clear();
                        ModelState.AddModelError("rutCliente", mensaje);
                    }


                    

                }
            }
            else if (btn_agregarServicio != null)
            {
                ModelState.Clear();

                int enteroTemp = 0;
                if (!Int32.TryParse(nvoContrato.servicioSeleccionado, out enteroTemp))
                {
                    string mensaje = "El servicio seleccionado no es válido";
                    ModelState.AddModelError("servicioSeleccionado", mensaje);
                }
                else if (enteroTemp < 0)
                {
                    string mensaje = "El servicio seleccionado no es válido";
                    ModelState.AddModelError("servicioSeleccionado", mensaje);

                }
                else if (!Int32.TryParse(precioPorContrato, out enteroTemp))
                {
                    string mensaje = "El precio del servicio no es válido";
                    ModelState.AddModelError("precioPorContrato", mensaje);
                }
                else if (enteroTemp < 0)
                {
                    string mensaje = "El precio del servicio no es válido";
                    ModelState.AddModelError("precioPorContrato", mensaje);
                }
                else
                {

                    List<ServicioListado> listaTemp = null;
                    if (Session["listaServicios"] == null)
                    {
                        listaTemp = new List<ServicioListado>();
                        Session["listaServicios"] = listaTemp;
                    }
                    else
                    {
                        listaTemp = (List<ServicioListado>)Session["listaServicios"];
                    }

                    idServicio = 0;
                    if (Int32.TryParse(nvoContrato.servicioSeleccionado, out idServicio))
                    {
                        //ViewBag.selectedServicio = idServicio.ToString();
                        ViewBag.precioReferencia = Contrato.getPrecioServicio(idServicio).ToString();
                        
                    }
                    else
                    {
                        ViewBag.precioReferencia = "0";
                    }

                    ServicioListado servicioNvo = new ServicioListado();
                    servicioNvo.descripcion = condicion_servicio;
                    int idServicioSeleccionado = 0;
                    Int32.TryParse(nvoContrato.servicioSeleccionado, out idServicioSeleccionado);
                    servicioNvo.nombre_servicio = ServicioListado.getNombreServicio(idServicioSeleccionado);
                    servicioNvo.precio_acordado = precioPorContrato;
                    servicioNvo.id_servicio = nvoContrato.servicioSeleccionado;
                    bool esta = false;
                    foreach (ServicioListado servTemp in listaTemp)
                    {
                        if (servTemp.id_servicio.Equals(servicioNvo.id_servicio)) {
                            esta = true;
                        }
                    }
                    if (esta)
                    {
                        ModelState.AddModelError("servicioSeleccionado", "El servicio seleccionado ya estaba agregado");
                    }
                    else
                    {
                        listaTemp.Add(servicioNvo);
                    }
                    ViewBag.listaServiciosAgregados = listaTemp;
                }
            }
            else if (btn_agregarContrato != null) //Agrego el contrato
            {
                if (ModelState.IsValidField("rutCliente"))
                {
                    int rut = 0;
                    bool satisf = false;
                    if (!Int32.TryParse(nvoContrato.rutCliente, out rut))
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("rutCliente", "El rut ingresado no es válido");
                        ViewBag.nombreCliente = "";
                    }
                    else
                    {
                        ViewBag.nombreCliente = Contrato.getNombreCliente(rut, out satisf);
                        if (!satisf)
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rutCliente", "El rut insertado no existe");
                        }
                        else
                        {
                            List<ServicioListado> listaTemp;
                            if (Session["listaServicios"] == null)
                            {
                                ViewBag.respuestaPost = "No posee una lista de servicios para el nuevo contrato";

                            }
                            else if (((List<ServicioListado>)Session["listaServicios"]).Count == 0)
                            {
                                ViewBag.respuestaPost = "Posee una lista de servicios para el nuevo contrato vacia";

                            }
                            else
                            {
                                listaTemp = (List<ServicioListado>)Session["listaServicios"];

                                //ahora compruebo las fechas de contrato
                                int agnoInt = 0;
                                if (Int32.TryParse(nvoContrato.agno_ini_contrato, out agnoInt))
                                {
                                    if ((agnoInt >= 1900) && (agnoInt <= 2100))
                                    {
                                        string fecha_ini = nvoContrato.dia_ini_contrato + "-" + nvoContrato.mes_ini_contrato + "-" + nvoContrato.agno_ini_contrato;
                                        string fecha_fin = null;
                                        //Camino CORRECTO HASTA AHORA
                                        if (tieneTermino_contrato.Contains("true"))
                                        {
                                            //Camino correcto, tomo en cuenta la fecha de fin del contrato
                                            fecha_fin = nvoContrato.dia_caducidad_contrato + "-" + nvoContrato.mes_caducidad_contrato + "-" + nvoContrato.agno_caducidad_contrato;
                                        }
                                        else
                                        {
                                            //Camino correcto, no tomo en cuenta la fecha de fin de contrato
                                        }
                                        ViewBag.respuestaPost = "Se ha creado el contrato";
                                        if (Contrato.insertContrato(rut, fecha_ini, fecha_fin, nvoContrato.breve_descripcion, listaTemp) <= 0)
                                        {
                                            ViewBag.respuestaPost = "No se ha podido realizar la creación del contrato";
                                        }

                                        //CAMINO FELIZ
                                        return RedirectToAction("Index", "home");


                                    }
                                    else
                                    {
                                        ModelState.AddModelError("agno_ini_contrato", "El año de inicio de contrato no está en el rango");
                                        
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("agno_ini_contrato", "El año de inicio de contrato no es válido");
                                }
                            }
                        }
                    }
                }
                else
                {
                    string mensaje = "El rut ingresado no es válido";
                    ModelState.Clear();
                    ModelState.AddModelError("rutCliente", mensaje);
                }



                List<ServicioListado> listaTemp2 = null;
                if (Session["listaServicios"] == null)
                {
                    listaTemp2 = new List<ServicioListado>();
                    Session["listaServicios"] = listaTemp2;
                }
                else
                {
                    listaTemp2 = (List<ServicioListado>)Session["listaServicios"];
                }
                ViewBag.listaServiciosAgregados = listaTemp2;
            }
            else
            {
                //Verifico si es un botón "quitar"
                NameValueCollection col = Request.Params;
                int idServ = 0;
                string nombreParam = "", idServStr;
                for (int i = 0; i < Request.Params.Count; i++)
                {

                    nombreParam = col.GetKey(i); //Con esto accedo al nombre del parámetro
                    if (nombreParam.Contains("quitar_")) //Con esto omito los parámetros que no me importan
                    {
                        if (Session["listaServicios"] != null)
                        {


                            idServStr = nombreParam.Substring("quitar_".Length);
                            if (Int32.TryParse(idServStr, out idServ))
                            {
                                List<ServicioListado> listaTemp = (List<ServicioListado>)Session["listaServicios"];
                                bool encontrado = false;
                                int posicion = 0;
                                foreach (ServicioListado temp in listaTemp)
                                {
                                    if (temp.id_servicio.Equals(idServStr))
                                    {
                                        encontrado = true;
                                        break;
                                    }
                                    posicion++;
                                }

                                if (encontrado)
                                {
                                    listaTemp.RemoveAt(posicion);
                                    ViewBag.respuestaPost = "Se ha quitado el servicio de la lista de servicios para el contrato";
                                }
                                else
                                {
                                    ViewBag.respuestaPost = "El servicio no estaba agregado a la lista de servicios para el contrato";
                                }
                            }
                        }
                    }
                }



                ViewBag.listaServiciosAgregados = Session["listaServicios"];

                ModelState.Clear();
                idServicio = 0;
                if (Int32.TryParse(nvoContrato.servicioSeleccionado, out idServicio))
                {
                    //ViewBag.selectedServicio = idServicio.ToString();
                    ViewBag.precioReferencia = Contrato.getPrecioServicio(idServicio).ToString();
                }
                else
                {
                    ViewBag.precioReferencia = "0";
                }

                return View(nvoContrato);
            }
            return View(nvoContrato);
        }


        [HttpGet]
        public ActionResult ModificarTrabajador()
        {

            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            return View();
        }


        [HttpPost]
        public ActionResult ModificarTrabajador(agregarTrabajadorModel trabajadorMod, string btn_submit, string es_activo)
        {

            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();

            if (btn_submit == null)
            {
                return View(trabajadorMod);
            }

            if (btn_submit.Equals("Cargar")) //Se esta cargando un trabajador
            {
                if (ModelState.IsValidField("rut"))
                {
                    string query = "SELECT * FROM trabajador WHERE rut_trabajador = '" + trabajadorMod.rut + "'";
                    NpgsqlDataReaderWithConection lector = null;
                    try
                    {
                        lector = DBConector.SELECT(query);
                        if (lector.Read())
                        {
                            ModelState.Clear();
                            trabajadorMod.rut = lector["rut_trabajador"];
                            trabajadorMod.nombre = lector["nombre_trabajador"];
                            trabajadorMod.id_perfil = lector["id_perfil"];
                            trabajadorMod.telefono1 = lector["tel1_trabajador"];
                            trabajadorMod.telefono2 = lector["tel2_trabajador"];
                            trabajadorMod.correo = lector["mail_trabajador"];
                            trabajadorMod.direccion = lector["direccion_trabajador"];
                            trabajadorMod.comuna = lector["comuna_trabajador"];
                            trabajadorMod.iniciales = lector["iniciales_trabajador"];
                            DateTime fecha_ini_contrato = lector.GetDateTime(lector.GetOrdinal("fecha_ini_contrato_trabajador"));
                            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
                            trabajadorMod.dia_ini_contrato = fecha_ini_contrato.Day.ToString();
                            trabajadorMod.mes_ini_contrato = dtinfo.GetMonthName(fecha_ini_contrato.Month);
                            trabajadorMod.agno_ini_contrato = fecha_ini_contrato.Year.ToString();
                            DateTime fecha_fin_contrato = lector.GetDateTime(lector.GetOrdinal("fecha_fin_contrato_trabajador"));
                            trabajadorMod.dia_fin_contrato = fecha_fin_contrato.Day.ToString();
                            trabajadorMod.mes_fin_contrato = fecha_fin_contrato.Month.ToString();
                            trabajadorMod.agno_fin_contrato = fecha_fin_contrato.Year.ToString();


                            ViewBag.trabajadorActivo = lector["esta_activo"];

                            lector.Dispose();
                            lector.Close();
                            lector.closeConection();
                            return View(trabajadorMod);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut", "El rut insertado no existe");
                        }
                    }
                    catch (Exception)
                    {
                        ViewBag.respuestaPost = DBConector.msjError;//ex.Message;

                    }
                    if (lector != null)
                    {
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                    }
                }
                else
                {
                    string mensaje = "El rut ingresado no es válido";
                    ModelState.Clear();
                    ModelState.AddModelError("rut", mensaje);
                }
            }
            else if (btn_submit.Equals("Guardar cambios")) //Se presionó el botón para guardar cambios
            {
                bool es_activoBool = true;
                if (es_activo.Equals("false"))
                {
                    es_activoBool = false;
                    //SI ES FALSO ENTONCES HAY QUE COMPROBAR SI YA PERTENECE A UNA CUADRILLA
                }

                string fecha_fin_contrato;
                int diaEscogido, mesContrato, agno_contrato, diasDelMes;
                if (trabajadorMod.dia_fin_contrato != null || trabajadorMod.mes_fin_contrato != null || trabajadorMod.agno_fin_contrato != null)
                {

                    if (!Int32.TryParse(trabajadorMod.dia_fin_contrato, out diaEscogido))
                    {
                        ModelState.AddModelError("dia_fin_contrato", "No ha seleccionado un día del mes");
                        return View(trabajadorMod);
                    }
                    if (!Int32.TryParse(trabajadorMod.mes_fin_contrato, out mesContrato))
                    {
                        ModelState.AddModelError("mes_fin_contrato", "No ha seleccionado un mes");
                        return View(trabajadorMod);
                    }
                    if (!Int32.TryParse(trabajadorMod.agno_fin_contrato, out agno_contrato))
                    {
                        ModelState.AddModelError("agno_fin_contrato", "El año introducido no es válido");
                        return View(trabajadorMod);
                    }

                    diasDelMes = DateTime.DaysInMonth(agno_contrato, mesContrato);
                    if (diasDelMes < diaEscogido)
                    {
                        ModelState.AddModelError("dia_fin_contrato", "El día seleccionado no es válido para el més seleccionado");
                        return View(trabajadorMod);
                    }

                    if (agno_contrato < 1900 || agno_contrato > 2100)
                    {
                        ModelState.AddModelError("agno_fin_contrato", "Que año más extraño, ¿Está seguro?");
                        return View(trabajadorMod);
                    }

                    if (mesContrato < 1 || mesContrato > 12)
                    {
                        ModelState.AddModelError("mes_fin_contrato", "El més seleccionado no es válido");
                        return View(trabajadorMod);
                    }

                    fecha_fin_contrato = trabajadorMod.dia_fin_contrato + "-" + trabajadorMod.mes_fin_contrato + "-" + trabajadorMod.agno_fin_contrato;
                    
                }
                else
                {
                    fecha_fin_contrato = null;
                }


                if (ModelState.IsValid)
                {
                    string query = "UPDATE trabajador SET id_perfil='" + trabajadorMod.id_perfil + "', nombre_trabajador='" + trabajadorMod.nombre + "', iniciales_trabajador='" + trabajadorMod.iniciales + "', direccion_trabajador='" + trabajadorMod.direccion + "', comuna_trabajador='" + trabajadorMod.comuna + "', tel1_trabajador='" + trabajadorMod.telefono1 + "', tel2_trabajador='" + trabajadorMod.telefono2 + "', mail_trabajador='" + trabajadorMod.correo + "', esta_activo='" + es_activoBool + "'";
                    if (fecha_fin_contrato != null)
                    {
                        query +=  ", fecha_fin_contrato_trabajador='" + fecha_fin_contrato + "'";
                    }
                    query += " WHERE rut_trabajador='" + trabajadorMod.rut + "'";


                    try
                    {
                        int cantidadInsertada = DBConector.UPDATE(query);

                        ViewBag.respuestaPost = "Se han guardado correctamente los datos del trabajador";
                    }
                    catch (Exception)
                    {
                        ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                    }

                    return View();
                    //return RedirectToAction("Index", "home");
                }
                else
                {
                    return View(trabajadorMod);
                }


            }
            else //Se presionó cualquier otra cosa, no se usa
            {

            }
            return View();
        }


        [HttpGet]
        public ActionResult EliminarTrabajador()
        {

            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            return View();
        }


        [HttpPost]
        public ActionResult EliminarTrabajador(agregarTrabajadorModel trabajadorMod, string btn_submit, string es_activo)
        {

            ViewBag.listaPerfiles = perfilTrabajadorModel.getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();

            if (btn_submit == null)
            {
                return View(trabajadorMod);
            }
            if (btn_submit.Equals("Cargar")) //Se esta cargando un trabajador
            {
                if (ModelState.IsValidField("rut"))
                {
                    string query = "SELECT * FROM trabajador WHERE rut_trabajador = '" + trabajadorMod.rut + "'";
                    NpgsqlDataReaderWithConection lector = null;
                    try
                    {
                        lector = DBConector.SELECT(query);
                        if (lector.Read())
                        {
                            ModelState.Clear();
                            trabajadorMod.rut = lector["rut_trabajador"];
                            trabajadorMod.nombre = lector["nombre_trabajador"];
                            trabajadorMod.id_perfil = lector["id_perfil"];
                            trabajadorMod.telefono1 = lector["tel1_trabajador"];
                            trabajadorMod.telefono2 = lector["tel2_trabajador"];
                            trabajadorMod.correo = lector["mail_trabajador"];
                            trabajadorMod.direccion = lector["direccion_trabajador"];
                            trabajadorMod.comuna = lector["comuna_trabajador"];
                            trabajadorMod.iniciales = lector["iniciales_trabajador"];
                            DateTime fecha_ini_contrato = lector.GetDateTime(lector.GetOrdinal("fecha_ini_contrato_trabajador"));
                            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
                            trabajadorMod.dia_ini_contrato = fecha_ini_contrato.Day.ToString();
                            trabajadorMod.mes_ini_contrato = dtinfo.GetMonthName(fecha_ini_contrato.Month);
                            trabajadorMod.agno_ini_contrato = fecha_ini_contrato.Year.ToString();
                            if (lector.GetOrdinal("fecha_fin_contrato_trabajador") < 0)
                            {
                                DateTime fecha_fin_contrato = lector.GetDateTime(lector.GetOrdinal("fecha_fin_contrato_trabajador"));
                                trabajadorMod.dia_fin_contrato = fecha_fin_contrato.Day.ToString();
                                trabajadorMod.mes_fin_contrato = fecha_fin_contrato.Month.ToString();
                                trabajadorMod.agno_fin_contrato = fecha_fin_contrato.Year.ToString();

                            }
                            
                            ViewBag.trabajadorActivo = lector.GetBoolean(lector.GetOrdinal("esta_activo"));

                            lector.Dispose();
                            lector.Close();
                            lector.closeConection();
                            return View(trabajadorMod);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut", "El rut insertado no existe");
                        }
                    }
                    catch (Exception)
                    {


                    }
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }
                else
                {
                    string mensaje = "El rut ingresado no es válido";
                    ModelState.Clear();
                    ModelState.AddModelError("rut", mensaje);
                }

            }
            else if (btn_submit.Equals("Eliminar"))
            {
                if (ModelState.IsValidField("rut"))
                {
                    try
                    {
                        string query = "DELETE FROM trabajador WHERE rut_trabajador = '" + trabajadorMod.rut + "'";
                        DBConector.DELETE(query);
                    }
                    catch (Exception)
                    {
                    }

                }
                else
                {
                    return View(trabajadorMod);
                }
                

            }

            return View();
            //return RedirectToAction("EliminarTrabajador", "home");
        }


        public List<SelectListItem> getListaDias()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 1; i <= 31; i++)
            {
                items.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            return items;
        }


        public List<SelectListItem> getListaMeses()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            DateTimeFormatInfo ci = new CultureInfo("es-ES").DateTimeFormat;
            int i = 1;
            foreach (String temp in ci.MonthNames)
            {
                if (i <= 12)
                {
                    items.Add(new SelectListItem
                    {
                        Text = temp,
                        Value = i.ToString()
                    });
                }
                i++;
            }
            return items;
        }


        public List<SelectListItem> getListaUnidades()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            NpgsqlDataReaderWithConection unidades = null;
            try
            {
                unidades = DBConector.SELECT("SELECT id_unidad, nombre_unidad, abreviatura_unidad FROM unidad_material");


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


        public string generarTablaPerfilesTrabajadores() {
            string respuesta;
            NpgsqlDataReaderWithConection perfiles = null;
            try
            {
                perfiles = DBConector.SELECT("SELECT nombre_cargo FROM perfil_trabajador");
                respuesta = "<table class='table contenedor_lista_servicios'>";
                respuesta += "<thead>";
                respuesta += "<tr class='fila_contenedor_lista_servicios_titulos'>";
                respuesta += "<td class='columna_contenedor_lista_servicios1'><b>Nombre cargo o perfil</b></td>";
                respuesta += "</tr>";
                respuesta += "</thead>";
                while (perfiles.Read())
                {
                    respuesta += "<tr class='fila_contenedor_lista_servicios'>";
                    respuesta += "<td class='columna_contenedor_lista_servicios1'>" + perfiles.GetString(perfiles.GetOrdinal("nombre_cargo")) + "</td>";
                    respuesta += "</tr>";
                }
                respuesta += "</table>";
            }
            catch (Exception)
            {
                respuesta = DBConector.msjError;
            }
            if (perfiles != null)
            {
                perfiles.Dispose();
                perfiles.Close();
                perfiles.closeConection();
            }
            return respuesta;
        }


        public string generarTablaUnidadesMedida()
        {
            string respuesta;
            NpgsqlDataReaderWithConection unidades = null;
            try
            {
                unidades = DBConector.SELECT("SELECT nombre_unidad, abreviatura_unidad FROM unidad_material");
                respuesta = "<table class='table contenedor_lista_servicios'>";
                respuesta += "<thead>";
                respuesta += "<b>";
                respuesta += "<tr class='fila_contenedor_lista_servicios_titulos'>";
                respuesta += "<td class='columna_contenedor_lista_servicios1'><b>Nombre unidad</b></td>";
                respuesta += "<td class='columna_contenedor_lista_servicios2'><b>Abreviatura unidad</b></td>";
                respuesta += "</tr>";
                respuesta += "</b>";
                respuesta += "</thead>";
                while (unidades.Read())
                {
                    respuesta += "<tr class='fila_contenedor_lista_servicios'>";
                    respuesta += "<td class='columna_contenedor_lista_servicios1'>" + unidades.GetString(unidades.GetOrdinal("nombre_unidad")) + "</td>";
                    respuesta += "<td class='columna_contenedor_lista_servicios2'>" + unidades.GetString(unidades.GetOrdinal("abreviatura_unidad")) + "</td>";
                    respuesta += "</tr>";
                }
                respuesta += "</table>";
            }
            catch (Exception)
            {
                respuesta = DBConector.msjError;
            }
            if (unidades != null)
            {
                unidades.Dispose();
                unidades.Close();
                unidades.closeConection();
            }
            return respuesta;
        }


        public string generarTablaMaterialGenericos()
        {
            string respuesta = "";
            NpgsqlDataReaderWithConection materialesGen = null;
            try {
                materialesGen = DBConector.SELECT("SELECT nombre_tipo_material, glosa_tipo_material, nombre_unidad FROM material_generico NATURAL JOIN unidad_material");
                respuesta = "<table class='table contenedor_lista_servicios'>";
                respuesta += "<thead>";
                respuesta += "<tr class='fila_contenedor_lista_servicios_titulos'>";
                respuesta += "<td class='columna_contenedor_lista_servicios1'><b>Nombre material</b></td>";
                respuesta += "<td class='columna_contenedor_lista_servicios2'><b>Glosa material</b></td>";
                respuesta += "<td class='columna_contenedor_lista_servicios2'><b>Unidad de medida</b></td>";
                respuesta += "</tr>";
                respuesta += "</thead>";
                while (materialesGen.Read())
                {
                    respuesta += "<tr class='fila_contenedor_lista_servicios'>";
                    respuesta += "<td class='columna_contenedor_lista_servicios1'>" + materialesGen.GetString(materialesGen.GetOrdinal("nombre_tipo_material")) + "</td>";
                    respuesta += "<td class='columna_contenedor_lista_servicios2'>" + materialesGen.GetString(materialesGen.GetOrdinal("glosa_tipo_material")) + "</td>";
                    respuesta += "<td class='columna_contenedor_lista_servicios2'>" + materialesGen.GetString(materialesGen.GetOrdinal("nombre_unidad")) + "</td>";
                    respuesta += "</tr>";
                }
                respuesta += "</table>";
            }
            catch (Exception) {

            }

            if (materialesGen != null)
            {
                materialesGen.Dispose();
                materialesGen.Close();
                materialesGen.closeConection();
            }
            return respuesta;
        }


        [HttpGet]
        public ActionResult MantMaterialGenericos()
        {
            ViewBag.lista_unidades = getListaUnidades();
            ViewBag.tabla = generarTablaMaterialGenericos();

            return View();
        }


        [HttpPost]
        public ActionResult MantMaterialGenericos(MaterialGenericoModel nvoMat)
        {
            
            ViewBag.lista_unidades = getListaUnidades();

            if (ModelState.IsValid)
            {
                NpgsqlDataReaderWithConection lector = null;
                string query = "INSERT INTO material_generico (nombre_tipo_material, glosa_tipo_material, id_unidad) VALUES ('" + nvoMat.nombre + "', '" + nvoMat.glosa_material + "', '"+nvoMat.id_unidad+"')";
                try
                {
                    string query2 = "SELECT nombre_tipo_material FROM material_generico WHERE nombre_tipo_material ILIKE '" + nvoMat.nombre + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("nombre", "Ya existe este material genérico");
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                        ViewBag.respuestaPost = "";
                        ViewBag.tabla = generarTablaMaterialGenericos();
                        return View(nvoMat);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha agregado correctamente el material genérico";
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }

                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }

                ViewBag.tabla = generarTablaMaterialGenericos();
                return View();
                //return RedirectToAction("MantMaterialGenericos", "home");
            }
            else
            {
                ViewBag.tabla = generarTablaMaterialGenericos();
                return View(nvoMat);
            }

        }


        [HttpGet]
        public ActionResult MantUnidadesMedida()
        {
            
            ViewBag.tabla = generarTablaUnidadesMedida();

            return View();
        }


        [HttpPost]
        public ActionResult MantUnidadesMedida(agregarUnidadModel nvaUnidad)
        {
            

            if (ModelState.IsValid)
            {
                NpgsqlDataReaderWithConection lector = null;
                string query = "INSERT INTO unidad_material (nombre_unidad, abreviatura_unidad) VALUES ('"+nvaUnidad.nombre+"', '"+nvaUnidad.abreviatura+"')";
                try
                {
                    string query2 = "SELECT nombre_unidad FROM unidad_material WHERE nombre_unidad ILIKE '" + nvaUnidad.nombre + "' OR abreviatura_unidad ILIKE'" + nvaUnidad.abreviatura + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("nombre", "Ya existe esta unidad de medida");
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                        ViewBag.respuestaPost = "";
                        ViewBag.tabla = generarTablaUnidadesMedida();
                        return View(nvaUnidad);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha agregado correctamente la unidad de medida";
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }

                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }

                ViewBag.tabla = generarTablaUnidadesMedida();
                return View();
                //return RedirectToAction("MantUnidadesMedida", "home");
            }
            else
            {
                ViewBag.tabla = generarTablaUnidadesMedida();
                return View(nvaUnidad);
            }

        }


        [HttpGet]
        public ActionResult MantperfilTrabajadores()
        {

            ViewBag.tabla = generarTablaPerfilesTrabajadores();

            return View();
        }


        [HttpPost]
        public ActionResult MantperfilTrabajadores(perfilTrabajadorModel nvoCargo)
        {
            

            if (ModelState.IsValid)
            {
                NpgsqlDataReaderWithConection lector = null;
                string query = "INSERT INTO perfil_trabajador (nombre_cargo) VALUES ('" + nvoCargo.nombre + "')";
                try
                {
                    string query2 = "SELECT nombre_cargo FROM perfil_trabajador WHERE nombre_cargo ILIKE '" + nvoCargo.nombre + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("nombre", "Ya existe este perfil de trabajador");
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                        ViewBag.respuestaPost = "";
                        ViewBag.tabla = generarTablaPerfilesTrabajadores();
                        return View(nvoCargo);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha agregado correctamente el perfil de trabajador";
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }

                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }

                ViewBag.tabla = generarTablaPerfilesTrabajadores();
                return View();
                //return RedirectToAction("MantperfilTrabajadores", "home");
            }
            else
            {
                ViewBag.tabla = generarTablaPerfilesTrabajadores();
                return View(nvoCargo);
            }

        }


        //DEL ADOLFO

        [HttpGet]
        public ActionResult MantServiciosPrestados()
        {
            ViewBag.ScriptOcultar= agregarServicioModel.ocultarModificarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = agregarServicioModel.generarTablaServicios();
            return View();
        }
        
        
        
        public ActionResult cargarServicio(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio) {
            //Agrego a la lista de agregados en caso que se haya presionado un botón agregar
            NameValueCollection col = Request.Params;
            string nombreParam = "", valorParam = "", id_servicio_actual="";
            for (int i = 0; i < Request.Params.Count; i++)
            {
                nombreParam = col.GetKey(i); //Con esto accedo al nombre del parámetro
                if (nombreParam.Contains("editar_")) //Con esto omito los parámetros que no me importan
                {
                    valorParam = col.Get(i); //Con esto accedo al valor del parámetro, debiese tener el texto del botón
                    //Acá ya se que botón de agregar fue el presionado
                    id_servicio_actual = nombreParam.Substring("editar_".Length);                   
                }
            }
            string query = "SELECT * FROM servicio WHERE id_servicio='" + id_servicio_actual + "'";
            NpgsqlDataReaderWithConection lector = null;
            try
            {
                lector = DBConector.SELECT(query);
                if (lector.Read())
                {
                    ModelState.Clear();
                    nvoServicio.nombreServicio = lector.GetString(lector.GetOrdinal("nombre_servicio"));
                    nvoServicio.precioPizarra = lector.GetInt32(lector.GetOrdinal("precio_pizarra")).ToString();
                    nvoServicio.factorBono = lector.GetDouble(lector.GetOrdinal("factor_bono_trabajador")).ToString();
                    ViewBag.VisibilidadServicio = lector.GetBoolean(lector.GetOrdinal("visibilidad_servicio"));
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                    ViewBag.ScriptOcultar = agregarServicioModel.ocultarAgregarServicios();
                    ViewBag.respuestaPost = "";
                    ViewBag.tabla = agregarServicioModel.generarTablaServicios();
                    Session["servicioEditar"] = id_servicio_actual;                    
                    return View(nvoServicio);
                }
                else
                {
                    ModelState.Clear();
                }
            }
            catch (Exception)
            {
                ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
            }
            if (lector != null)
            {
                lector.Dispose();
                lector.Close();
                lector.closeConection();
            }
            ViewBag.ScriptOcultar = agregarServicioModel.ocultarAgregarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = agregarServicioModel.generarTablaServicios();
            Session["servicioEditar"] = id_servicio_actual;
            lector.Dispose();
            lector.Close();
            lector.closeConection();
            return View(nvoServicio);
        }

        

        public ActionResult agregarServicio(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio)
        {
            if (ModelState.IsValid)
            {
                string activado = visibilidad1;
                string query = "INSERT INTO servicio (nombre_servicio, precio_pizarra, factor_bono_trabajador, visibilidad_servicio) VALUES ('" + nvoServicio.nombreServicio + "','" + nvoServicio.precioPizarra + "','" + nvoServicio.factorBono + "','" + activado + "')";
                NpgsqlDataReaderWithConection lector = null;
                try
                {
                    string query2 = "SELECT nombre_servicio FROM servicio WHERE nombre_servicio = '" + nvoServicio.nombreServicio + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("nombreServicio", "Ya existe un servicio con ese nombre");
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                        ViewBag.ScriptOcultar = agregarServicioModel.ocultarModificarServicios();
                        ViewBag.respuestaPost = "";
                        ViewBag.tabla = agregarServicioModel.generarTablaServicios();
                        return View(nvoServicio);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);                    
                    ViewBag.respuestaPost = "El servicio '" + nvoServicio.nombreServicio + "' se creo de manera satisfactoria";
                    ModelState.Clear();
                    ViewBag.ScriptOcultar = agregarServicioModel.ocultarModificarServicios();
                    ViewBag.tabla = agregarServicioModel.generarTablaServicios();
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }
                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }
            }
            ViewBag.ScriptOcultar = agregarServicioModel.ocultarModificarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = agregarServicioModel.generarTablaServicios();
            return View(nvoServicio);
        }


        public ActionResult modificarServicio(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio)
        {           
            if (ModelState.IsValid)
            {
                string activado = visibilidad2;
                string query = "UPDATE servicio SET nombre_servicio= '" + nvoServicio.nombreServicio + "' , precio_pizarra=" + nvoServicio.precioPizarra + ", factor_bono_trabajador=" + nvoServicio.factorBono + ", visibilidad_servicio=" + visibilidad1 + " WHERE id_servicio=" + Session["servicioEditar"];
                NpgsqlDataReaderWithConection lector = null;
                try
                {
                    string query2 = "SELECT id_servicio FROM servicio WHERE nombre_servicio = '" + nvoServicio.nombreServicio + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        
                        query2 = "SELECT id_servicio FROM servicio WHERE nombre_servicio = '" + nvoServicio.nombreServicio + "' AND id_servicio = '" + Session["servicioEditar"] + "'";
                        lector = DBConector.SELECT(query2);

                        if (!lector.HasRows)
                        {
                            ModelState.AddModelError("nombreServicio", "Ya existe un servicio con ese nombre");
                            lector.Dispose();
                            lector.Close();
                            lector.closeConection();
                            ViewBag.ScriptOcultar = agregarServicioModel.ocultarAgregarServicios();
                            ViewBag.tabla = agregarServicioModel.generarTablaServicios();
                            return View();
                        }
                    }
                    
                    int cantidadInsertada = DBConector.UPDATE(query);
                    ViewBag.respuestaPost = "El servicio '" + nvoServicio.nombreServicio + "' se actualizo de manera satisfactoria";
                    ModelState.Clear();
                    ViewBag.tabla = agregarServicioModel.generarTablaServicios();
                    ViewBag.ScriptOcultar = agregarServicioModel.ocultarModificarServicios();
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                    return View();
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }
                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                    lector.closeConection();
                }
            }
            ViewBag.ScriptOcultar = agregarServicioModel.ocultarModificarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = agregarServicioModel.generarTablaServicios();
            return View();
        }


        [HttpPost]
        public ActionResult MantServiciosPrestados(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio)
        {
            if (btn_submit != null)
            {
                if (btn_submit.Equals("Agregar Servicio"))
                {
                    return agregarServicio(nvoServicio, btn_submit, visibilidad1, visibilidad2, id_servicio);
                }
                if (btn_submit.Equals("Guardar cambios"))
                {
                    return modificarServicio(nvoServicio, btn_submit, visibilidad1, visibilidad2, id_servicio);
                }
            }
            else {
                return cargarServicio(nvoServicio, btn_submit, visibilidad1, visibilidad2, id_servicio);                
            }
            ViewBag.respuestaPost = "";
            ViewBag.ScriptOcultar = agregarServicioModel.ocultarModificarServicios();
            ViewBag.tabla = agregarServicioModel.generarTablaServicios();
            return View();
        }

        //HASTA ACÁ EL ADOLFO


        public string cargaUnidades()
        {
            return "NADA";

        }


        //AUDITORÍA
        public List<string> getTablasDB()
        {
            List<string> result = new List<string>();
            string query = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_name NOT LIKE '%auditoria%' ORDER BY table_name";
            NpgsqlDataReaderWithConection lector = null;
            try {
                lector = DBConector.SELECT(query);
                while (lector.Read())
                {
                    result.Add(lector.GetString(0));
                }
            }
            catch (Exception) {

            }
            if (lector != null)
            {
                lector.CloseTodo();
            }
            return result;
        }


        public List<logModel> cargaTablaAuditoria()
        {
            string query2Part = "SELECT general_activo, insert_activo, update_activo, delete_Activo FROM tablas_auditoria WHERE nombre_tabla='";
            
            
            NpgsqlDataReaderWithConection lector2 = null;
            List<logModel> resultadoAuditoria = new List<logModel>();
            logModel logTem;
            try
            {
                List<string> listaTablas = getTablasDB();
                foreach (string nombreTabla in listaTablas)
                {
                    logTem = new logModel();
                    logTem.nombre = nombreTabla;

                    lector2 = DBConector.SELECT(query2Part+nombreTabla+"'");
                    if (lector2.Read())
                    {
                        logTem.general_activo = lector2.GetBoolean(0);
                        logTem.insert_activo = lector2.GetBoolean(1);
                        logTem.update_activo = lector2.GetBoolean(2);
                        logTem.delete_activo = lector2.GetBoolean(3);
                    }
                    else
                    {
                        logTem.general_activo = false;
                        logTem.insert_activo = false;
                        logTem.update_activo = false;
                        logTem.delete_activo = false;
                    }
                    lector2.CloseTodo();
                    resultadoAuditoria.Add(logTem);
                }
            }
            catch (Exception)
            {
                return resultadoAuditoria;
            }
            if (lector2 != null)
            {
                lector2.CloseTodo();
            }
            return resultadoAuditoria;
        }


        [HttpGet]
        public ActionResult confAuditoria()
        {
            ViewBag.tabla = cargaTablaAuditoria();
            return View();

        }


        [HttpPost]
        public ActionResult confAuditoria(string btn_guardar)
        {
            string identificadorParamValido = "nombreTabla___";
            string SEPARADOR = "___";
            NpgsqlDataReaderWithConection ejecutorSp = null;
            NameValueCollection col = Request.Params;
            string nombreParam, valorParam, nombreTabla, stringTemp, tipo_operacion;
            for (int i = 0; i < Request.Params.Count; i++ )
            {
                
                nombreParam = col.GetKey(i); //Con esto accedo al nombre del parámetro
                if (!nombreParam.Contains(identificadorParamValido)) //Con esto omito los parámetros que no me importan
                {
                    continue;
                }
                valorParam = col.Get(i); //Con esto accedo al valor del parámetro
                if (valorParam.Contains("true"))
                {
                    valorParam = "true";
                }

                //Acá se tiene algo válido
                stringTemp = nombreParam.Substring(identificadorParamValido.Length);
                tipo_operacion = stringTemp.Substring(stringTemp.IndexOf(SEPARADOR)+SEPARADOR.Length);
                nombreTabla = stringTemp.Substring(0, stringTemp.IndexOf(SEPARADOR));

                //Llamo a un procedimiento almacenado pasandole la tabla y el valorParam
                try
                {
                    ejecutorSp = DBConector.SELECT("SELECT sp_update_conf_auditoria('" + nombreTabla + "', '" + tipo_operacion + "', " + valorParam + ")");
                    ejecutorSp.CloseTodo();
                    /*
                    if (valorParam.Equals("true"))
                    {
                        //Create trigger
                        if (tipo_operacion.Equals("UPD"))
                            ejecutorSp = DBConector.SELECT("CREATE TRIGGER trigg_audit_upd AFTER UPDATE ON " + nombreTabla + " FOR EACH ROW EXECUTE PROCEDURE sp_auditoria_ins()");
                        else if (tipo_operacion.Equals("INS"))
                            ejecutorSp = DBConector.SELECT("CREATE TRIGGER trigg_audit_ins AFTER INSERT ON " + nombreTabla + " FOR EACH ROW EXECUTE PROCEDURE sp_auditoria_ins()");
                        else if (tipo_operacion.Equals("DEL"))
                            ejecutorSp = DBConector.SELECT("CREATE TRIGGER trigg_audit_del AFTER DELETE ON " + nombreTabla + " FOR EACH ROW EXECUTE PROCEDURE sp_auditoria_ins()");
                        else if (tipo_operacion.Equals("GEN"))
                        {
                            ejecutorSp = DBConector.SELECT("SELECT count(update_activo) FROM tablas_auditoria WHERE nombre_tabla = nombreTabla AND update_activo = TRUE");
                            ejecutorSp.Read();
                            if (ejecutorSp.GetInt32(0) > 0)
                            {
                                ejecutorSp.CloseTodo();
                                ejecutorSp = DBConector.SELECT("CREATE TRIGGER trigg_audit_upd AFTER UPDATE ON "+nombreTabla+" FOR EACH ROW EXECUTE PROCEDURE sp_auditoria_ins()");
                                ejecutorSp.CloseTodo();
                            }

                            ejecutorSp = DBConector.SELECT("SELECT count(insert_activo) FROM tablas_auditoria WHERE nombre_tabla = nombreTabla AND insert_activo = TRUE");
                            ejecutorSp.Read();
                            if (ejecutorSp.GetInt32(0) > 0)
                            {
                                ejecutorSp.CloseTodo();
                                ejecutorSp = DBConector.SELECT("CREATE TRIGGER trigg_audit_ins AFTER INSERT ON " + nombreTabla + " FOR EACH ROW EXECUTE PROCEDURE sp_auditoria_ins()");
                                ejecutorSp.CloseTodo();
                            }

                            ejecutorSp = DBConector.SELECT("SELECT count(delete_activo) FROM tablas_auditoria WHERE nombre_tabla = nombreTabla AND delete_activo = TRUE");
                            ejecutorSp.Read();
                            if (ejecutorSp.GetInt32(0) > 0)
                            {
                                ejecutorSp.CloseTodo();
                                ejecutorSp = DBConector.SELECT("CREATE TRIGGER trigg_audit_del AFTER DELETE ON " + nombreTabla + " FOR EACH ROW EXECUTE PROCEDURE sp_auditoria_ins()");
                                ejecutorSp.CloseTodo();
                            }
                        }
                        ejecutorSp.CloseTodo();
                    }
                    else
                    {
                        //DELETE TRIGGER
                        if (tipo_operacion.Equals("UPD"))
                            ejecutorSp = DBConector.SELECT("DROP TRIGGER IF EXISTS trigg_audit_upd ON " + nombreTabla);
                        else if (tipo_operacion.Equals("INS"))
                            ejecutorSp = DBConector.SELECT("DROP TRIGGER IF EXISTS trigg_audit_ins ON " + nombreTabla); 
                        else if (tipo_operacion.Equals("DEL"))
                            ejecutorSp = DBConector.SELECT("DROP TRIGGER IF EXISTS trigg_audit_del ON " + nombreTabla);
                        else if (tipo_operacion.Equals("GEN"))
                        {
                            ejecutorSp = DBConector.SELECT("DROP TRIGGER IF EXISTS trigg_audit_upd ON " + nombreTabla);
                            ejecutorSp.CloseTodo();
                            ejecutorSp = DBConector.SELECT("DROP TRIGGER IF EXISTS trigg_audit_ins ON " + nombreTabla);
                            ejecutorSp.CloseTodo();
                            ejecutorSp = DBConector.SELECT("DROP TRIGGER IF EXISTS trigg_audit_del ON " + nombreTabla);
                            ejecutorSp.CloseTodo();
                        }
                    }
                    */
                }
                catch (Exception)
                {
                    ViewBag.respuestaPost = DBConector.msjError;
                }
                if (ejecutorSp != null)
                {
                    ejecutorSp.CloseTodo();
                    ejecutorSp = null;
                }

                //ViewBag.mensaje += " " + nombreTabla + "=" + valorParam;
            }
            if (ViewBag.respuestaPost != null)
            {
                ViewBag.respuestaPost = "Se han guardado correctamente los cambios";
            }
            ViewBag.tabla = cargaTablaAuditoria();
            return View();

        }


        [HttpGet]
        public ActionResult verAuditoria()
        {
            //ViewBag.tabla = cargaTablaAuditoria();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            List<string> listaTablas = getTablasDB();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Todas",
                Value = "Todas"
            });
            foreach (string nombreTabla in listaTablas)
            {
                items.Add(new SelectListItem
                {
                    Text = nombreTabla,
                    Value = nombreTabla
                });
            }
            

            ViewBag.listaTablas = items;
            ViewBag.listaOperaciones = getListaOperaciones();
            DatosAuditoriaModel objeto = new DatosAuditoriaModel();
            objeto.agno_fin = DateTime.Now.Year.ToString();
            objeto.mes_fin = DateTime.Now.Month.ToString();
            objeto.dia_fin = DateTime.Now.Day.ToString();

            objeto.agno_ini = DateTime.Now.Year.ToString();
            objeto.mes_ini = DateTime.Now.Month.ToString();
            objeto.dia_ini = DateTime.Now.Day.ToString();

            
            ViewBag.cuantosDatosVer = getIntervaloLimit();

            return View(objeto);

        }


        [HttpPost]
        public ActionResult verAuditoria(DatosAuditoriaModel objeto, string btn_cargar)
        {
            //ViewBag.tabla = cargaTablaAuditoria();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            List<string> listaTablas = getTablasDB();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Todas",
                Value = "Todas"
            });
            foreach (string nombreTabla in listaTablas)
            {
                items.Add(new SelectListItem
                {
                    Text = nombreTabla,
                    Value = nombreTabla
                });
            }
            ViewBag.listaTablas = items;

            ViewBag.listaOperaciones = getListaOperaciones();

            if (btn_cargar.Equals("Cargar"))
            {
                ViewBag.tablaLogs = getLogAuditoria(objeto.nombreTabla, objeto.operacion, objeto.dia_ini + "-" + objeto.mes_ini + "-" + objeto.agno_ini, objeto.dia_fin + "-" + objeto.mes_fin + "-" + objeto.agno_fin, objeto.cuantosVer);
            }

            ViewBag.cuantosDatosVer = getIntervaloLimit();



            return View(objeto);

        }


        public List<SelectListItem> getIntervaloLimit()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "10",
                Value = "10"
            });
            items.Add(new SelectListItem
            {
                Text = "50",
                Value = "50"
            });
            items.Add(new SelectListItem
            {
                Text = "100",
                Value = "100"
            });
            items.Add(new SelectListItem
            {
                Text = "500",
                Value = "500"
            });
            items.Add(new SelectListItem
            {
                Text = "Todos",
                Value = "Todos"
            });
            return items;
        }


        public List<SelectListItem> getListaOperaciones()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Todas",
                Value = "Todas"
            });
            items.Add(new SelectListItem
            {
                Text = "INSERT",
                Value = "I"
            });
            items.Add(new SelectListItem
            {
                Text = "UPDATE",
                Value = "U"
            });
            items.Add(new SelectListItem
            {
                Text = "DELETE",
                Value = "D"
            });
            return items;
        }


        public List<DatoLogForTabla> getLogAuditoria(string nombreTabla, string operacion, string fechaIni, string fechaFin, string cuantosVer)
        {
            List<DatoLogForTabla> resultado = new List<DatoLogForTabla>();
            DatoLogForTabla temp;
            NpgsqlDataReaderWithConection lector = null;
            string query = "SELECT nombre_tabla, log_timestamp, operacion, datos_anteriores, datos_nuevos FROM log_auditoria";
            bool si = false;
            try
            {
                
                query += " WHERE";
                if (!nombreTabla.Equals("Todas"))
                {
                    query += " nombre_tabla='" + nombreTabla + "'";
                    si = true;
                }
                if (!operacion.Equals("Todas"))
                {
                    if (si)
                        query += " AND";
                    query += " operacion='"+operacion[0]+"'";
                    si = true;
                }
                

                //Agrego las comprobaciones de rangos de fecha
                if (si)
                    query += " AND";
                query += " log_timestamp > timestamp '" + fechaIni + "' AND log_timestamp < timestamp '" + fechaFin + " 23:59:59'";

                if (!cuantosVer.Equals("Todos"))
                {
                    query += " LIMIT " + cuantosVer;
                }

                lector = DBConector.SELECT(query);
                while (lector.Read())
                {
                    temp = new DatoLogForTabla();
                    temp.nombreTabla = lector.GetString(0);
                    temp.timestamp = lector.GetDateTime(1).ToString();
                    temp.operacion = lector.GetString(2);
                    if (temp.operacion.Equals("U"))
                    {
                        temp.operacion = "UPDATE";
                    }
                    else if (temp.operacion.Equals("D"))
                    {
                        temp.operacion = "DELETE";
                    }
                    else if (temp.operacion.Equals("I"))
                    {
                        temp.operacion = "INSERT";
                    }
                    if (lector.IsDBNull(3))
                        temp.datosAntes = "";
                    else 
                        temp.datosAntes = lector.GetString(3);
                    if (lector.IsDBNull(4))
                        temp.datosDespues = "";
                    else 
                        temp.datosDespues = lector.GetString(4);
                    resultado.Add(temp);
                }
                

            }
            catch (Exception)
            {

            }
            if (lector != null)
            {
                lector.CloseTodo();
            }

            return resultado;
        }


        [HttpGet]
        public ActionResult CrearCuadrilla()
        {
            //Para borrar los datos de creaciones pasadas
            if (Session["listaAgregadosCuadrilla"] != null)
            {
                List<int> listaAgregadosSesion = (List<int>)Session["listaAgregadosCuadrilla"];
                listaAgregadosSesion.Clear();
                Session["listaAgregadosCuadrilla"] = null;
            }


            return View();
        }


        [HttpPost]
        public ActionResult CrearCuadrilla(string rut_trabajador, string nombre_trabajador, string btn_cargar, string btn_crear_cuadrilla)
        {
            if (btn_crear_cuadrilla == null) //Si se presionó cualquier botón distinto del de crear cuadrilla 
            {
                ModelState.Clear();
                if (rut_trabajador == null)
                {
                    ModelState.AddModelError("rut_trabajador", "No ha ingresado un rut");
                }

                if (nombre_trabajador == null)
                {
                    ModelState.AddModelError("nombre_trabajador", "No ha ingresado un nombre");
                }

                if ((rut_trabajador != null) && (nombre_trabajador != null))
                {

                    if ((rut_trabajador.Trim().Length == 0) && (nombre_trabajador.Trim().Length == 0))
                    {
                        ViewBag.listaTrabajadores = ListarTrabajadorModel.getTrabajadoresForTable(null, null);
                    }
                    else if (nombre_trabajador.Trim().Length > 0) //puso nombre entonces
                    {
                        ViewBag.listaTrabajadores = ListarTrabajadorModel.getTrabajadoresForTable(nombre_trabajador, "nombre_trabajador");
                    }

                    else if (rut_trabajador.Trim().Length > 0) //puso rut entonces
                    {
                        int rut_int = 0;
                        if (Int32.TryParse(rut_trabajador, out rut_int))
                        {
                            ViewBag.listaTrabajadores = ListarTrabajadorModel.getTrabajadoresForTable(rut_trabajador, "rut_trabajador");

                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut_trabajador", "El rut ingresado no es válido");
                        }
                    }
                }


                //Agrego a la lista de agregados en caso que se haya presionado un botón agregar
                NameValueCollection col = Request.Params;
                int rutInt = 0;
                string nombreParam = "", valorParam = "", rutStr;
                for (int i = 0; i < Request.Params.Count; i++)
                {

                    nombreParam = col.GetKey(i); //Con esto accedo al nombre del parámetro
                    if (nombreParam.Contains("agregar_")) //Con esto omito los parámetros que no me importan
                    {
                        valorParam = col.Get(i); //Con esto accedo al valor del parámetro, debiese tener el texto del botón

                        //Acá ya se que botón de agregar fue el presionado
                        rutStr = nombreParam.Substring("agregar_".Length);
                        if (Int32.TryParse(rutStr, out rutInt))
                        {
                            if (Session["listaAgregadosCuadrilla"] == null)
                            {
                                Session["listaAgregadosCuadrilla"] = new List<int>();
                            }
                            
                            List<int> listaTemp = (List<int>)Session["listaAgregadosCuadrilla"];
                            if (listaTemp.Contains(rutInt))
                            {
                                ViewBag.respuestaPost = "Ya ha agregado al trabajador con rut: " + rutInt + " a la lista";
                            }
                            else
                            {
                                listaTemp.Add(rutInt);
                                ViewBag.respuestaPost = "Se ha agregado el trabajador con rut: " + rutInt + " a la lista";
                            }
                        }
                        else
                        {
                            ViewBag.respuestaPost = "No fue posible agregar el trabajador, problemas con su rut";

                        }
                        break;
                    }
                    else if (nombreParam.Contains("quitar_")) //Con esto omito los parámetros que no me importan
                    {
                        if (Session["listaAgregadosCuadrilla"] != null)
                        {
                            rutStr = nombreParam.Substring("quitar_".Length);
                            if (Int32.TryParse(rutStr, out rutInt))
                            {
                                List<int> listaTemp = (List<int>)Session["listaAgregadosCuadrilla"];
                                if (listaTemp.Remove(rutInt))
                                {
                                    ViewBag.respuestaPost = "Se ha quitado el trabajador de la lista que conformará la cuadrilla";
                                }
                                else
                                {
                                    ViewBag.respuestaPost = "El trabajador que desea quitar no estaba agregado a la lista";
                                }
                            }
                        }
                    }
                }


            }
            else //Se presionó el botón crear_cuadrilla
            {
                string respuesta = "";
                if (Session["listaAgregadosCuadrilla"] != null)
                {
                    List<int> listaAgregadosSesion = (List<int>)Session["listaAgregadosCuadrilla"];
                    bool satisfactorio = false;
                    respuesta = ListarTrabajadorModel.crearCuadrilla(listaAgregadosSesion, out satisfactorio);
                    if (satisfactorio)
                    {
                        listaAgregadosSesion.Clear();
                        Session["listaAgregadosCuadrilla"] = null;
                    }
                }
                else
                {
                    respuesta = "No ha agregado trabajadores para formar la cuadrilla";
                }
                
                ViewBag.respuestaPost = respuesta;
            }



            if (Session["listaAgregadosCuadrilla"] != null)
            {
                List<ListarTrabajadorModel> listaTrabajadoresAgregados = new List<ListarTrabajadorModel>();
                List<int> listaAgregadosSesion = (List<int>)Session["listaAgregadosCuadrilla"];
                ListarTrabajadorModel temp;
                foreach (int rut in listaAgregadosSesion)
                {
                    temp = ListarTrabajadorModel.getTrabajadorByRut(rut);
                    if (temp != null)
                    {
                        listaTrabajadoresAgregados.Add(temp);
                    }
                }
                ViewBag.listaTrabajadoresAgregados = listaTrabajadoresAgregados;
            }
            

            return View();
        }


        [HttpGet]
        public ActionResult ingresoOT()
        {
            //Para borrar los datos de creaciones pasadas
            if (Session["listaServiciosNvaOT"] != null)
            {
                List<ServicioListado> listaAgregadosSesion = (List<ServicioListado>)Session["listaServiciosNvaOT"];
                listaAgregadosSesion.Clear();
                Session["listaServiciosNvaOT"] = null;
            }
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = new List<SelectListItem>();
            
            ViewBag.listaRuts = OrdenTrabajoModel.getClientes();
            List<SelectListItem> contratosSelectedList = new List<SelectListItem>();
            //List<Contrato> contratos = OrdenTrabajo.getContratosSegunCliente(-1);

            ViewBag.listaContratos = contratosSelectedList;


            //Cargo los valores por defecto de la vista
            OrdenTrabajoModel ordenTrabajo = new OrdenTrabajoModel();
            ordenTrabajo.ciudad_ot = "Santiago";
            DateTime ahora = DateTime.Now;
            ordenTrabajo.dia_ini_ot = ahora.Day.ToString();
            ordenTrabajo.mes_ini_ot = ahora.Month.ToString();
            ordenTrabajo.agno_ini_ot = ahora.Year.ToString();

            ordenTrabajo.dia_fin_ot = ahora.Day.ToString();
            ordenTrabajo.mes_fin_ot = ahora.Month.ToString();
            ordenTrabajo.agno_fin_ot = ordenTrabajo.agno_ini_ot;
            ordenTrabajo.precioReferenciaContrato = "0";
            ordenTrabajo.precioFinal = "0";
            ordenTrabajo.cantidadDelServicio = "1";


            return View(ordenTrabajo);
        }

        [HttpPost]
        public ActionResult ingresoOT(OrdenTrabajoModel ordenTrabajo, string btn_crear_ot, string btn_agregarServicio)
        {
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            
            ViewBag.listaRuts = OrdenTrabajoModel.getClientes();
            List<SelectListItem> contratosSelectedList = new List<SelectListItem>();
            List<Contrato> contratos;

            List<ServicioListado> listaAgregadosSesion;
            if (Session["listaServiciosNvaOT"] == null)
            {
                listaAgregadosSesion = new List<ServicioListado>();
            }
            else
            {
                listaAgregadosSesion = (List<ServicioListado>)Session["listaServiciosNvaOT"];
            }


            int id_servicio = 0;
            if (btn_crear_ot == null) //Si se presionó cualquier botón distinto del de crear la OT
            {
                ModelState.Clear();
                if (btn_agregarServicio != null) {
                    ServicioListado nvoServ = new ServicioListado();
                    
                    nvoServ.descripcion = ordenTrabajo.breve_descripcion;
                    nvoServ.id_servicio = ordenTrabajo.servicioSeleccionado;

                    if (!Int32.TryParse(nvoServ.id_servicio, out id_servicio))
                    {
                        ViewBag.RespuestaPost = "No se ha podido cargar el nombre del servicio";
                        ViewBag.tipoRespuestaPost = "error";
                    }

                    nvoServ.nombre_servicio = ServicioListado.getNombreServicio(id_servicio);
                    nvoServ.precio_acordado = ordenTrabajo.precioFinal;
                    nvoServ.cantidad = ordenTrabajo.cantidadDelServicio;
                    
                    //Controlo que el servicio no haya sido agregado anteriormente
                    bool yaAgregado = false;
                    foreach (ServicioListado servTemp in listaAgregadosSesion)
                    {
                        if (servTemp.id_servicio.Equals(nvoServ.id_servicio))
                        {
                            ViewBag.RespuestaPost = "Ya tiene agregado el servicio";
                            ViewBag.tipoRespuestaPost = "advertencia";
                            yaAgregado = true;
                        }
                    }

                    if (!yaAgregado)
                    {
                        listaAgregadosSesion.Add(nvoServ);
                    }
                    Session["listaServiciosNvaOT"] = listaAgregadosSesion;


                }
                else {
                    
                    //Quito de la lista de agregados en caso que se haya presionado un botón quitar
                    NameValueCollection col = Request.Params;
                    string nombreParam = "", id_servicioStr;
                    for (int i = 0; i < Request.Params.Count; i++)
                    {

                        nombreParam = col.GetKey(i); //Con esto accedo al nombre del parámetro
                        if (nombreParam.Contains("quitar_")) //Con esto omito los parámetros que no me importan
                        {
                            if (Session["listaServiciosNvaOT"] != null)
                            {
                                id_servicioStr = nombreParam.Substring("quitar_".Length);
                                
                                if (Int32.TryParse(id_servicioStr, out id_servicio))
                                {
                                    bool re = false;
                                    List<ServicioListado> listaTemp = (List<ServicioListado>)Session["listaServiciosNvaOT"];
                                    int pos = 0;
                                    foreach (ServicioListado t in listaTemp)
                                    {
                                        if (t.id_servicio.Equals(id_servicioStr))
                                        {
                                            re = true;
                                            break;
                                        }
                                        pos++;
                                    }
                                    
                                    if (re)
                                    {
                                        listaTemp.RemoveAt(pos);
                                        ViewBag.respuestaPost = "Se ha quitado el servicio de la lista de servicio de la orden de trabajo";
                                        ViewBag.tipoRespuetaPost = "information";
                                    }
                                    else
                                    {
                                        ViewBag.respuestaPost = "El servicio que desea quitar no se encontraba en la lista";
                                        ViewBag.tipoRespuetaPost = "error";
                                    }
                                }
                                
                            }
                        }
                    }
                }
            }
            else //Se presionó el botón crear OT
            {
                string respuesta = "";
                if (Session["listaServiciosNvaOT"] != null)
                {
                    if (listaAgregadosSesion.Count == 0)
                    {
                        respuesta = "No ha agregado servicios a la orden de trabajo";
                        ViewBag.tipoRespuestaPos = "informacion";
                    }
                    else
                    {
                        //Camino feliz
                        int enteroTemp;
                        bool satisfactorio = true;
                        if (!(satisfactorio = satisfactorio && Int32.TryParse(ordenTrabajo.agno_ini_ot, out enteroTemp)))
                        {
                            ModelState.AddModelError("agno_ini_ot", "El año de inicio de la orden de trabajo no es válido");
                        }
                        if (!(satisfactorio = satisfactorio && Int32.TryParse(ordenTrabajo.agno_fin_ot, out enteroTemp)))
                        {
                            ModelState.AddModelError("agno_fin_ot", "El año de finalización de la orden de trabajo no es válido");
                        }
                        if (!(satisfactorio = satisfactorio && Int32.TryParse(ordenTrabajo.nro_orden_segun_cliente, out enteroTemp)))
                        {
                            ModelState.AddModelError("nro_orden_segun_cliente", "El N° de la orden de trabajo no es válido");
                        }
                        if (ModelState.IsValid)
                        {
                            respuesta = OrdenTrabajoModel.insertOrdenTrabajo(ordenTrabajo, listaAgregadosSesion);
                            ViewBag.tipoRespuestaPos = "informacion";
                        }
                        
                    }
                }
                else
                {
                    respuesta = "No ha agregado servicios a la orden de trabajo";
                    ViewBag.tipoRespuestaPos = "informacion";
                }

                ViewBag.respuestaPost = respuesta;
            }


            //cargo los contratos que correspondan
            if (Int32.TryParse(ordenTrabajo.cliente, out id_servicio))
            {
                contratos = OrdenTrabajoModel.getContratosSegunCliente(id_servicio);
            }
            else
            {
                contratosSelectedList = new List<SelectListItem>();
                ViewBag.RespuestaPost = "Problemas con el rut del cliente";
                ViewBag.tipoRespuestaPost = "error";
                contratos = new List<Contrato>();
            }
            ordenTrabajo.descripcion_contrato = "";
            bool coincideAlguno = false;
            foreach (Contrato t in contratos)
            {
                if (ordenTrabajo.contrato != null)
                {
                    if (ordenTrabajo.contrato.Equals(t.id_contrato.ToString()))
                    {
                        coincideAlguno = true;
                        ordenTrabajo.descripcion_contrato = t.breve_descripcion;
                    }
                }
                contratosSelectedList.Add(new SelectListItem
                {
                    Text = t.id_contrato,
                    Value = t.id_contrato
                });
            }
            if (!coincideAlguno && contratos.Count > 0)
            {
                ordenTrabajo.contrato = contratos[0].id_contrato;
                ordenTrabajo.descripcion_contrato = contratos[0].breve_descripcion;
            }
            else if (!coincideAlguno)
            {
                ordenTrabajo.descripcion_contrato = "";
            }



            //Cargo la lista de servicios para el contrato y su precio correspondiente
            int id_contrato_seleccionado = -1;
            Int32.TryParse(ordenTrabajo.contrato, out id_contrato_seleccionado);
            List<SelectListItem> serviciosDelContrato = servicioContrato.getServiciosDeContrato(id_contrato_seleccionado);
            ViewBag.listaServicios = serviciosDelContrato;
            coincideAlguno = false;
            foreach (SelectListItem t in serviciosDelContrato)
            {
                if (ordenTrabajo.servicioSeleccionado != null)
                {
                    if (ordenTrabajo.servicioSeleccionado.Equals(t.Value))
                    {
                        coincideAlguno = true;
                        ordenTrabajo.precioReferenciaContrato = servicioContrato.getPrecioAcordadoServicio(Int32.Parse(t.Value), id_contrato_seleccionado).ToString();
                        ordenTrabajo.precioFinal = ordenTrabajo.precioReferenciaContrato;
                    }
                }
            }
            if (!coincideAlguno && contratos.Count > 0)
            {
                ordenTrabajo.servicioSeleccionado = serviciosDelContrato[0].Value;
                ordenTrabajo.precioReferenciaContrato = servicioContrato.getPrecioAcordadoServicio(Int32.Parse(serviciosDelContrato[0].Value), id_contrato_seleccionado).ToString();
                ordenTrabajo.precioFinal = ordenTrabajo.precioReferenciaContrato;
            }
            else if (!coincideAlguno)
            {
                ordenTrabajo.precioReferenciaContrato = "0";
                ordenTrabajo.precioFinal = "0";
            }


            ViewBag.listaContratos = contratosSelectedList;
            ViewBag.listaServiciosAgregados = listaAgregadosSesion;


            return View(ordenTrabajo);
        }


        [HttpGet]
        public ActionResult Contratos()
        {
            ViewBag.listaContratos = Contrato.getAllContratos();
            ViewBag.contratoDetallado = null;

            return View();
        }

        [HttpPost]
        public ActionResult Contratos(Contrato contrato)
        {
            ModelState.Clear();
            int idContrato = -1;
            if (!Int32.TryParse(contrato.id_contrato, out idContrato))
            {
                ModelState.AddModelError("id_contrato", "N° de contrato no válido");
            }
            ViewBag.listaContratos = Contrato.getAllContratos();
            ViewBag.contratoDetallado = Contrato.getDetalleContrato(idContrato);
            ViewBag.serviciosDelContrato = Contrato.getServiciosDelContrato(idContrato);

            return View(contrato);
        }
    }
}
