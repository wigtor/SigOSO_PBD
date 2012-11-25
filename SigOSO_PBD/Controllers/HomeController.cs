﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using SigOSO_PBD.classes;
using SigOSO_PBD.Models;
using System.Globalization;
using System.Collections.Specialized;

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
                NpgsqlDataReaderWithConection lector = null;
                try
                {
                    string query2 = "SELECT rut_cliente FROM cliente WHERE rut_cliente = '" + nvoCliente.rut + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows) {
                        ModelState.AddModelError( "rut", "Ya existe un cliente con ese rut");
                        lector.Dispose();
                        lector.Close();
                        lector.closeConection();
                        ViewBag.respuestaPost = "";
                        return View(nvoCliente);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha creado correctamente el cliente";
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

        public List<SelectListItem> getListaPerfilesTrabajadores()
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
            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = Contrato.getAllServicios();
            ViewBag.precioReferencia = "0";

            return View();
        }


        [HttpPost] //NO TERMINADO
        public ActionResult agregarContrato(Contrato nvoContrato, string nombre_cliente, string btn_cargar, string btn_agregarServicio, string precioPorContrato, string condicion_servicio)
        {
            ViewBag.listaPerfiles = getListaPerfilesTrabajadores();
            ViewBag.listaDias = getListaDias();
            ViewBag.listaMeses = getListaMeses();
            ViewBag.listaServicios = Contrato.getAllServicios();
            ViewBag.precioReferencia = "0";

            if (btn_cargar != null)
            {
                if (btn_cargar.Equals("Cargar")) //Se esta cargando un cliente
                {
                    if (ModelState.IsValidField("rutCliente"))
                    {
                        string query = "SELECT rut_cliente, nombre_cliente FROM cliente WHERE rut_cliente = '" + nvoContrato.rutCliente + "'";
                        NpgsqlDataReaderWithConection lector = null;
                        try
                        {
                            lector = DBConector.SELECT(query);
                            if (lector.Read())
                            {
                                ModelState.Clear();
                                nvoContrato.rutCliente = lector["rut_cliente"];
                                ViewBag.nombreCliente = lector["nombre_cliente"];

                                lector.Dispose();
                                lector.Close();
                                lector.closeConection();
                                return View(nvoContrato);
                            }
                            else
                            {
                                ModelState.Clear();
                                ModelState.AddModelError("rutCliente", "El rut insertado no existe");
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
                        ModelState.AddModelError("rutCliente", mensaje);
                    }
                }
            }
            else if (btn_agregarServicio != null)
            {
                //COMPROBAR SI HAY PRECIO ACORDADO

                ModelState.Clear();
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

                ServicioListado servicioNvo = new ServicioListado();
                servicioNvo.descripcion = condicion_servicio;
                servicioNvo.precio_acordado = precioPorContrato;
                servicioNvo.id_servicio = nvoContrato.servicioSeleccionado;
                listaTemp.Add(servicioNvo);
                ViewBag.listaServiciosAgregados = listaTemp;

            }
            else
            {
                ModelState.Clear();
                int idServicio = 0;
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

        public string generarTablaServicios()
        {
            string respuesta = "";
            NpgsqlDataReaderWithConection servicios = null;
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
                        respuesta += "<td>" + "<input type='checkbox' disabled='true'>" + "</td>";
                    }
                    respuesta += "<td>" + "<input name='btn_submit' type='submit' value='editar " + servicios.GetInt32(servicios.GetOrdinal("id_servicio")).ToString() + "'/>" + "</td>";
                    respuesta += "</tr>";
                }
                respuesta += "</table>";
            }
            catch (Exception)
            {
                respuesta = DBConector.msjError;
            }

            if (servicios != null)
            {
                servicios.Dispose();
                servicios.Close();
                servicios.closeConection();
            }
            return respuesta;
        }

        public string ocultarAgregarServicios() {
            return "<script>$('.condetenedor_agregar_servicio').hide();</script>";
        }

        public string ocultarModificarServicios() {
            return "<script>$('.condetenedor_modificar_servicio').hide();</script>";
        }

        [HttpGet]
        public ActionResult MantServiciosPrestados()
        {
            ViewBag.ScriptOcultar=ocultarModificarServicios();
            ViewBag.respuestaPost = "";            
            ViewBag.tabla = generarTablaServicios();
            return View();
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
                        ViewBag.ScriptOcultar = ocultarModificarServicios();
                        ViewBag.respuestaPost = "";
                        ViewBag.tabla = generarTablaServicios();
                        return View(nvoServicio);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "";
                    ViewBag.ScriptOcultar = ocultarModificarServicios();
                    ViewBag.tabla = generarTablaServicios();
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
            ViewBag.ScriptOcultar = ocultarModificarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = generarTablaServicios();
            return View(nvoServicio);
        }

        public ActionResult cargarServicio(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio) {
            string query = "SELECT * FROM servicio WHERE id_servicio='" + btn_submit.Split(' ')[1] + "'";
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
                    lector.Dispose();
                    lector.Close();
                    ViewBag.ScriptOcultar = ocultarAgregarServicios();
                    ViewBag.respuestaPost = "";
                    ViewBag.tabla = generarTablaServicios();
                    ViewBag.id_servicio = "<input name='id_servicio'  type='submit' value='" + btn_submit.Split(' ')[1] + "'/>";
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
            ViewBag.ScriptOcultar = ocultarAgregarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = generarTablaServicios();
            ViewBag.id_servicio = "<input name='id_servicio'  type='submit' value='" + btn_submit.Split(' ')[1] + "'/>";
            return View(nvoServicio);
        }

        public ActionResult modificarServicio(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio)
        {         
            if (ModelState.IsValid)
            {
                string activado = visibilidad2;
                string query = "UPDATE servicio SET nombre_servicio= " + nvoServicio.nombreServicio + ", precio_pizarra=" + nvoServicio.nombreServicio + ", factor_bono_trabajador=" + nvoServicio.factorBono + ", visibilidad_servicio=" + activado + " WHERE id_servicio=" + id_servicio;
                NpgsqlDataReaderWithConection lector = null;
                try
                {
                    string query2 = "SELECT id_servicio FROM servicio WHERE nombre_servicio = '" + nvoServicio.nombreServicio + "'";
                    lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        if (lector.GetInt32(lector.GetOrdinal("id_servicio")).ToString() != id_servicio.ToString())
                        {
                            ModelState.AddModelError("nombreServicio", "Ya existe un servicio con ese nombre");
                            lector.Dispose();
                            lector.Close();
                            lector.closeConection();
                            ViewBag.ScriptOcultar = ocultarModificarServicios();
                            ViewBag.respuestaPost = "";
                            ViewBag.tabla = generarTablaServicios();                         
                            ViewBag.id_servicio = "<input name='id_servicio'  type='submit' value='" + btn_submit.Split(' ')[1] +"'/>";
                            return View(nvoServicio);
                        }
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "";
                    ViewBag.tabla = generarTablaServicios();
                    ViewBag.id_servicio = "<input name='id_servicio'  type='submit' value='" + btn_submit.Split(' ')[1] + "'/>";
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
            ViewBag.ScriptOcultar = ocultarModificarServicios();
            ViewBag.respuestaPost = "";
            ViewBag.tabla = generarTablaServicios();
            return View(nvoServicio);            
        }

        [HttpPost]
        public ActionResult MantServiciosPrestados(agregarServicioModel nvoServicio, string btn_submit, string visibilidad1, string visibilidad2, string id_servicio)
        {
            if (btn_submit.Equals("Agregar Servicio"))
            {
                return agregarServicio(nvoServicio, btn_submit, visibilidad1, visibilidad2, id_servicio);            
            }
            if (btn_submit.Split(' ')[0].Equals("editar"))
            {
                return cargarServicio(nvoServicio, btn_submit, visibilidad1, visibilidad2, id_servicio);
            }
            if (btn_submit.Equals("Guardar cambios"))
            {
                return modificarServicio(nvoServicio, btn_submit, visibilidad1, visibilidad2, id_servicio);
            }
            ViewBag.respuestaPost = "";
            ViewBag.ScriptOcultar = ocultarModificarServicios();
            ViewBag.tabla = generarTablaServicios();
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






    }

}
