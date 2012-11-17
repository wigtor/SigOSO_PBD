using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using SigOSO_PBD.classes;
using SigOSO_PBD.Models;
using System.Globalization;

namespace SigOSO_PBD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {

            ViewBag.primerValor = cargaUnidades();
            return View();
        }


        //Para hacer POST
        [HttpPost]
        public ActionResult AgregarCliente(agregarClienteModel nvoCliente)
        {  
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO cliente (rut_cliente, nombre_cliente, direccion_cliente, comuna_cliente, giro_cliente, tel1_cliente, tel2_cliente, mail_cliente, ciudad_cliente) VALUES ('" + nvoCliente.rut + "', '" + nvoCliente.nombre + "', '" + nvoCliente.direccion + "', '" + nvoCliente.comuna + "', '" + nvoCliente.giro + "', '" + nvoCliente.telefono1 + "', '" + nvoCliente.telefono2 + "', '" + nvoCliente.correo + "', '"+nvoCliente.ciudad+"')";
                NpgsqlDataReader lector = null;
                try
                {
                    string query2 = "SELECT rut_cliente FROM cliente WHERE rut_cliente = '" + nvoCliente.rut + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows) {
                        ModelState.AddModelError( "rut", "Ya existe un cliente con ese rut");
                        lector.Dispose();
                        lector.Close();
                        ViewBag.respuestaPost = "";
                        return View(nvoCliente);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha creado correctamente el cliente";
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }
                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                }

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
                    NpgsqlDataReader lector = null;
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
                            return View(clienteMod);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut", "El rut insertado no existe");
                        }
                        
                    }
                    catch (Exception ex) {

                    }
                    if (lector != null)
                    {
                        lector.Dispose();
                        lector.Close();
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
                    catch (Exception ex)
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

        public List<SelectListItem> getListaPerfilesTrabajadores()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            NpgsqlDataReader servicios = null;
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
            catch (Exception ex)
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
            }
            return items;
        }


        //Para visualizar
        [HttpGet]
        public ActionResult AgregarTrabajador()
        {

            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            return View();
        }


        [HttpPost]
        public ActionResult AgregarTrabajador(agregarTrabajadorModel nvoTrabajador)
        {

            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
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
                NpgsqlDataReader lector = null;
                try
                {
                    string query2 = "SELECT rut_trabajador FROM trabajador WHERE rut_trabajador = '" + nvoTrabajador.rut + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("rut", "Ya existe un trabajador con ese rut");
                        lector.Dispose();
                        lector.Close();
                        ViewBag.respuestaPost = "";
                        return View(nvoTrabajador);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha creado correctamente el trabajador";
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }
                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
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
            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = getAllServicios();
            //ViewBag.respuestaPost = "holaaa!! ;)";

            return View();
        }


        [HttpPost] //NO TERMINADO
        public ActionResult agregarContrato(Contrato nvoContrato, string nombre_cliente, string btn_cargar)
        {
            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = getAllServicios();

            if (btn_cargar == null)
            {
                return View();
            }

            if (btn_cargar.Equals("Cargar")) //Se esta cargando un cliente
            {
                if (ModelState.IsValidField("rutCliente"))
                {
                    string query = "SELECT rut_cliente, nombre_cliente FROM cliente WHERE rut_cliente = '" + nvoContrato.rutCliente + "'";
                    NpgsqlDataReader lector = null;
                    try
                    {
                        lector = DBConector.SELECT(query);
                        if (lector.Read())
                        {
                            ModelState.Clear();
                            //ViewBag.rutCliente = lector.GetInt32(lector.GetOrdinal("rut_cliente")).ToString();
                            nvoContrato.rutCliente = lector.GetInt32(lector.GetOrdinal("rut_cliente")).ToString();
                            ViewBag.nombreCliente = lector.GetString(lector.GetOrdinal("nombre_cliente"));

                            lector.Dispose();
                            lector.Close();
                            return View(nvoContrato);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rutCliente", "El rut insertado no existe");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.respuestaPost = DBConector.msjError;//ex.Message;

                    }
                    if (lector != null) {
                        lector.Dispose();
                        lector.Close();
                    }
                }
                else
                {
                    string mensaje = "El rut ingresado no es válido";
                    ModelState.Clear();
                    ModelState.AddModelError("rutCliente", mensaje);
                }
            }
            else //Se presionó cualquier otra cosa, no se usa
            {

            }
            return View();
        }



        [HttpGet]
        public ActionResult ModificarTrabajador()
        {
            
            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            return View();
        }


        [HttpPost]
        public ActionResult ModificarTrabajador(agregarTrabajadorModel trabajadorMod, string btn_submit, string es_activo)
        {
            
            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
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
                    NpgsqlDataReader lector = null;
                    try
                    {
                        lector = DBConector.SELECT(query);
                        if (lector.Read())
                        {
                            ModelState.Clear();
                            trabajadorMod.rut = lector.GetInt32(lector.GetOrdinal("rut_trabajador")).ToString();
                            trabajadorMod.nombre = lector.GetString(lector.GetOrdinal("nombre_trabajador"));
                            trabajadorMod.id_perfil = lector.GetInt32(lector.GetOrdinal("id_perfil")).ToString();
                            trabajadorMod.telefono1 = lector.GetString(lector.GetOrdinal("tel1_trabajador"));
                            trabajadorMod.telefono2 = lector.GetString(lector.GetOrdinal("tel2_trabajador"));
                            trabajadorMod.correo = lector.GetString(lector.GetOrdinal("mail_trabajador"));
                            trabajadorMod.direccion = lector.GetString(lector.GetOrdinal("direccion_trabajador"));
                            trabajadorMod.comuna = lector.GetString(lector.GetOrdinal("comuna_trabajador"));
                            trabajadorMod.iniciales = lector.GetString(lector.GetOrdinal("iniciales_trabajador"));
                            DateTime fecha_ini_contrato = lector.GetDateTime(lector.GetOrdinal("fecha_ini_contrato_trabajador"));
                            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
                            trabajadorMod.dia_ini_contrato = fecha_ini_contrato.Day.ToString();
                            trabajadorMod.mes_ini_contrato = dtinfo.GetMonthName(fecha_ini_contrato.Month);
                            trabajadorMod.agno_ini_contrato = fecha_ini_contrato.Year.ToString();
                            DateTime fecha_fin_contrato = lector.GetDateTime(lector.GetOrdinal("fecha_fin_contrato_trabajador"));
                            trabajadorMod.dia_fin_contrato = fecha_fin_contrato.Day.ToString();
                            trabajadorMod.mes_fin_contrato = fecha_fin_contrato.Month.ToString();
                            trabajadorMod.agno_fin_contrato = fecha_fin_contrato.Year.ToString();


                            ViewBag.trabajadorActivo = lector.GetBoolean(lector.GetOrdinal("esta_activo"));

                            lector.Dispose();
                            lector.Close();
                            return View(trabajadorMod);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut", "El rut insertado no existe");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.respuestaPost = DBConector.msjError;//ex.Message;

                    }
                    if (lector != null)
                    {
                        lector.Dispose();
                        lector.Close();
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
                    catch (Exception ex)
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

            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            return View();
        }

        [HttpPost]
        public ActionResult EliminarTrabajador(agregarTrabajadorModel trabajadorMod, string btn_submit, string es_activo)
        {

            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
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
                    NpgsqlDataReader lector = null;
                    try
                    {
                        lector = DBConector.SELECT(query);
                        if (lector.Read())
                        {
                            ModelState.Clear();
                            trabajadorMod.rut = lector.GetInt32(lector.GetOrdinal("rut_trabajador")).ToString();
                            trabajadorMod.nombre = lector.GetString(lector.GetOrdinal("nombre_trabajador"));
                            trabajadorMod.id_perfil = lector.GetInt32(lector.GetOrdinal("id_perfil")).ToString();
                            trabajadorMod.telefono1 = lector.GetString(lector.GetOrdinal("tel1_trabajador"));
                            trabajadorMod.telefono2 = lector.GetString(lector.GetOrdinal("tel2_trabajador"));
                            trabajadorMod.correo = lector.GetString(lector.GetOrdinal("mail_trabajador"));
                            trabajadorMod.direccion = lector.GetString(lector.GetOrdinal("direccion_trabajador"));
                            trabajadorMod.comuna = lector.GetString(lector.GetOrdinal("comuna_trabajador"));
                            trabajadorMod.iniciales = lector.GetString(lector.GetOrdinal("iniciales_trabajador"));
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
                            return View(trabajadorMod);
                        }
                        else
                        {
                            ModelState.Clear();
                            ModelState.AddModelError("rut", "El rut insertado no existe");
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    lector.Dispose();
                    lector.Close(); 
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
                    catch (Exception ex)
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
            NpgsqlDataReader unidades = null;
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
            catch (Exception ex)
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
            }
            return items;
        }

        public List<SelectListItem> getAllServicios()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            NpgsqlDataReader unidades = null;
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
            catch (Exception ex)
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
            }
            return items;

        }


        public string generarTablaPerfilesTrabajadores() {
            string respuesta;
            NpgsqlDataReader perfiles = null;
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
            catch (Exception ex)
            {
                respuesta = DBConector.msjError;
            }
            if (perfiles != null)
            {
                perfiles.Dispose();
                perfiles.Close();
            }
            return respuesta;
        }



        public string generarTablaUnidadesMedida()
        {
            string respuesta;
            NpgsqlDataReader unidades = null;
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
            catch (Exception ex)
            {
                respuesta = DBConector.msjError;
            }
            if (unidades != null)
            {
                unidades.Dispose();
                unidades.Close();
            }
            return respuesta;
        }

        public string generarTablaMaterialGenericos()
        {
            string respuesta = "";
            NpgsqlDataReader materialesGen = null;
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
            catch (Exception ex) {

            }

            if (materialesGen != null)
            {
                materialesGen.Dispose();
                materialesGen.Close();
            }
            return respuesta;
        }


        public string generarTablaServicios()
        {
            string respuesta = "";
            NpgsqlDataReader servicios = null;
            try
            {
                servicios = DBConector.SELECT("SELECT * FROM servicio");
                respuesta = "<table class='table table-hover'>";
                respuesta += "<thead>";
                respuesta += "<tr>";
                respuesta += "<td><b>Nombre servicio</b></td>";
                respuesta += "<td><b>Precio pizarra</b></td>";
                respuesta += "<td><b>Factor bono</b></td>";
                respuesta += "<td><b>Visible</b></td>";
                respuesta += "<td><b>Editar</b></td>";
                respuesta += "</thead>";
                respuesta += "</tr>";
                while (servicios.Read())
                {
                    respuesta += "<tr>";
                    respuesta += "<td>" + servicios.GetString(servicios.GetOrdinal("nombre_servicio")) + "</td>";
                    respuesta += "<td>" + servicios.GetInt32(servicios.GetOrdinal("precio_pizarra")).ToString() + "</td>";
                    respuesta += "<td>" + servicios.GetDouble(servicios.GetOrdinal("factor_bono_trabajador")).ToString() + "</td>";
                    if (servicios.GetBoolean(servicios.GetOrdinal("visibilidad_servicio")))
                    {
                        respuesta += "<td>" + "<input type='checkbox' disabled='true' checked>" + "</td>";
                    }
                    else
                    {
                        respuesta += "<td>" + "<input type='checkbox'>" + "</td>";
                    }
                    respuesta += "<td>" + "boton editar" + "</td>";
                    respuesta += "</tr>";
                }
                respuesta += "</table>";
            }
            catch (Exception ex)
            {
                respuesta = DBConector.msjError;
            }

            if (servicios != null)
            {
                servicios.Dispose();
                servicios.Close();
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
            ViewBag.tabla = generarTablaMaterialGenericos();
            ViewBag.lista_unidades = getListaUnidades();

            if (ModelState.IsValid)
            {
                NpgsqlDataReader lector = null;
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
                        ViewBag.respuestaPost = "";
                        return View(nvoMat);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha agregado correctamente el material genérico";
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }

                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                }

                return View();
                //return RedirectToAction("MantMaterialGenericos", "home");
            }
            else
            {
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
            ViewBag.tabla = generarTablaUnidadesMedida();

            if (ModelState.IsValid)
            {
                NpgsqlDataReader lector = null;
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
                        ViewBag.respuestaPost = "";
                        return View(nvaUnidad);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha agregado correctamente la unidad de medida";
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }

                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                }

                return View();
                //return RedirectToAction("MantUnidadesMedida", "home");
            }
            else
            {
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
            ViewBag.tabla = generarTablaPerfilesTrabajadores();

            if (ModelState.IsValid)
            {
                NpgsqlDataReader lector = null;
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
                        ViewBag.respuestaPost = "";
                        return View(nvoCargo);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha agregado correctamente el perfil de trabajador";
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }

                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                }

                return View();
                //return RedirectToAction("MantperfilTrabajadores", "home");
            }
            else
            {
                return View(nvoCargo);
            }

        }



        //DEL ADOLFO


        [HttpGet]
        public ActionResult MantServiciosPrestados()
        {
            ViewBag.respuestaPost = "";
            
            ViewBag.tabla = generarTablaServicios();
            return View();
            /*ViewBag.respuestaPost = "";
            NpgsqlDataReader servicios = DBConector.SELECT("SELECT * FROM servicio");
            string respuesta="<div class='contenedor_lista_servicios'>";
            respuesta += "<div class='fila_contenedor_lista_servicios_titulos'>";
                respuesta += "<div class='columna_contenedor_lista_servicios1'>Nombre servicio</div>";
                respuesta += "<div class='columna_contenedor_lista_servicios2'>Precio pizarra</div>";
                respuesta += "<div class='columna_contenedor_lista_servicios3'>Factor bono</div>";
                respuesta += "<div class='columna_contenedor_lista_servicios4'>Visible</div>";
                respuesta += "<div class='columna_contenedor_lista_servicios5'>Editar</div>";
            respuesta += "</div>";
            while(servicios.Read()){
                respuesta += "<div class='fila_contenedor_lista_servicios'>";
                    respuesta += "<div class='columna_contenedor_lista_servicios1'>" + servicios.GetString(servicios.GetOrdinal("nombre_servicio")) + "</div>";
                    respuesta += "<div class='columna_contenedor_lista_servicios2'>" + servicios.GetInt32(servicios.GetOrdinal("precio_pizarra")).ToString() + "</div>";
                    respuesta += "<div class='columna_contenedor_lista_servicios3'>" + servicios.GetDouble(servicios.GetOrdinal("factor_bono_trabajador")).ToString() + "</div>";
                    if (servicios.GetBoolean(servicios.GetOrdinal("visibilidad_servicio")))
                    {
                        respuesta += "<div class='columna_contenedor_lista_servicios4'>" + "<input type='checkbox' disabled='true' checked>" + "</div>";
                    }else{
                        respuesta += "<div class='columna_contenedor_lista_servicios4'>" + "<input type='checkbox'>" + "</div>";
                    }
                    respuesta += "<div class='columna_contenedor_lista_servicios5'>" + "boton editar" + "</div>";
                respuesta += "</div>";
            }
            respuesta += "</div>";
            ViewBag.tabla = respuesta;
            return View();*/
        }

        [HttpPost]
        public ActionResult MantServiciosPrestados(agregarServicioModel nvoServicio)
        {
            ViewBag.respuestaPost = "";
            ViewBag.tabla = generarTablaServicios();

            if (ModelState.IsValid)
            {
                string activado = "true";
                string query = "INSERT INTO servicio (nombre_servicio, precio_pizarra, factor_bono_trabajador, visibilidad_servicio) VALUES ('" + nvoServicio.nombreServicio + "','" + nvoServicio.precioPizarra + "','" + nvoServicio.factorBono + "','" + activado + "')";
                NpgsqlDataReader lector = null;
                try
                {
                    string query2 = "SELECT nombre_servicio FROM servicio WHERE nombre_servicio = '" + nvoServicio.nombreServicio + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("rut", "Ya existe un servicio con ese nombre");
                        lector.Dispose();
                        lector.Close();
                        ViewBag.respuestaPost = "";
                        return View(nvoServicio);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha creado correctamente el servicio";

                    return View();
                    //return RedirectToAction("MantServiciosPrestados", "home");
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
                }
                if (lector != null)
                {
                    lector.Dispose();
                    lector.Close();
                }
            }
            else 
            {
                ViewBag.respuestaPost = "";
                return View(nvoServicio);
            }
            return View(nvoServicio);
        }















        //HASTA ACÁ EL ADOLFO


        public string cargaUnidades()
        {
            return "NADA";

        }
    }


}