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
            ViewBag.Message = "Welcome to ASP.NET MVC!";

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
                try
                {
                    string query2 = "SELECT rut_cliente FROM cliente WHERE rut_cliente = '" + nvoCliente.rut + "'";
                    NpgsqlDataReader lector = DBConector.SELECT(query2);
                    if (lector.HasRows) {
                        ModelState.AddModelError( "rut", "Ya existe un cliente con ese rut");
                        lector.Dispose();
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

                return RedirectToAction("AgregarCliente", "home");
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
                    NpgsqlDataReader lector = DBConector.SELECT(query);
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
                        return View(clienteMod);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("rut", "El rut insertado no existe");
                    }
                    lector.Dispose();

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

                    return RedirectToAction("Index", "home");
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
                servicios.Dispose();

            }
            catch (Exception ex)
            {
                items.Add(new SelectListItem
                {
                    Text = DBConector.msjError,
                    Value = "-1"
                });
                if (servicios != null)
                    servicios.Dispose();
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
                try
                {
                    string query2 = "SELECT rut_trabajador FROM trabajador WHERE rut_trabajador = '" + nvoTrabajador.rut + "'";
                    NpgsqlDataReader lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("rut", "Ya existe un trabajador con ese rut");
                        lector.Dispose();
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

                return RedirectToAction("AgregarTrabajador", "home");
            }
            else
            {
                return View(nvoTrabajador);
            }
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
                    NpgsqlDataReader lector = DBConector.SELECT(query);
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
                        return View(trabajadorMod);
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("rut", "El rut insertado no existe");
                    }
                    lector.Dispose();

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

                    return RedirectToAction("Index", "home");
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



        public List<SelectListItem> getListaDias()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            for (int i = 1; i <= 31; i++)
            {
                items.Add(new SelectListItem
                {
                    Text = i + "",
                    Value = i + ""
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
                        Value = i + ""
                    });
                }
                i++;
            }
            return items;
        }

        




        //DEL ADOLFO


        [HttpGet]
        public ActionResult MantServiciosPrestados()
        {
            ViewBag.respuestaPost = "";
            NpgsqlDataReader servicios = DBConector.SELECT("SELECT * FROM servicio");
            string respuesta = "<table class='table contenedor_lista_servicios'>";
            respuesta += "<thead>";
            respuesta += "<tr class='fila_contenedor_lista_servicios_titulos'>";
            respuesta += "<td class='columna_contenedor_lista_servicios1'>Nombre servicio</td>";
            respuesta += "<td class='columna_contenedor_lista_servicios2'>Precio pizarra</td>";
            respuesta += "<td class='columna_contenedor_lista_servicios3'>Factor bono</td>";
            respuesta += "<td class='columna_contenedor_lista_servicios4'>Visible</td>";
            respuesta += "<td class='columna_contenedor_lista_servicios5'>Editar</td>";
            respuesta += "</thead>";
            respuesta += "</tr>";
            while (servicios.Read())
            {
                respuesta += "<tr class='fila_contenedor_lista_servicios'>";
                respuesta += "<td class='columna_contenedor_lista_servicios1'>" + servicios.GetString(servicios.GetOrdinal("nombre_servicio")) + "</td>";
                respuesta += "<td class='columna_contenedor_lista_servicios2'>" + servicios.GetInt32(servicios.GetOrdinal("precio_pizarra")).ToString() + "</td>";
                respuesta += "<td class='columna_contenedor_lista_servicios3'>" + servicios.GetDouble(servicios.GetOrdinal("factor_bono_trabajador")).ToString() + "</td>";
                if (servicios.GetBoolean(servicios.GetOrdinal("visibilidad_servicio")))
                {
                    respuesta += "<td class='columna_contenedor_lista_servicios4'>" + "<input type='checkbox' disabled='true' checked>" + "</td>";
                }
                else
                {
                    respuesta += "<td class='columna_contenedor_lista_servicios4'>" + "<input type='checkbox'>" + "</td>";
                }
                respuesta += "<td class='columna_contenedor_lista_servicios5'>" + "boton editar" + "</td>";
                respuesta += "</tr>";
            }
            respuesta += "</table>";
            ViewBag.tabla = respuesta;
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
            if (ModelState.IsValid)
            {
                string activado = "true";
                string query = "INSERT INTO servicio (nombre_servicio, precio_pizarra, factor_bono_trabajador, visibilidad_servicio) VALUES ('" + nvoServicio.nombreServicio + "','" + nvoServicio.precioPizarra + "','" + nvoServicio.factorBono + "','" + activado + "')";
                try
                {
                    string query2 = "SELECT nombre_servicio FROM servicio WHERE nombre_servicio = '" + nvoServicio.nombreServicio + "'";
                    NpgsqlDataReader lector = DBConector.SELECT(query2);
                    if (lector.HasRows)
                    {
                        ModelState.AddModelError("rut", "Ya existe un servicio con ese nombre");
                        lector.Dispose();
                        ViewBag.respuestaPost = "";
                        return View(nvoServicio);
                    }
                    int cantidadInsertada = DBConector.INSERT(query);
                    ViewBag.respuestaPost = "Se ha creado correctamente el servicio";
                    return RedirectToAction("MantServiciosPrestados", "home");
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = DBConector.msjError;//ex.Message;
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
            string resultado = "";
            NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5432;UserId=SigOSO_user;Password=pbd2012;Database=SigOSO");
            try
            {
                con.Open();

            }
            catch (Exception)
            {
                return "ERROR EN CONEXIÓN A POSTGRE";
            }
            //Un insert o un update o un delete
            /*
            NpgsqlCommand comando = new NpgsqlCommand("INSERT INTO unidad_material (nombre_unidad, abreviatura_unidad) VALUES ('kilogramos', 'kg')", con);
            int resultado = comando.ExecuteNonQuery();
            */

            //Un select
            NpgsqlCommand comando = new NpgsqlCommand("SELECT * FROM unidad_material", con);
            NpgsqlDataReader resultQuery =  comando.ExecuteReader();
            if (resultQuery.Read()) {       
                resultado = resultQuery.GetString(1);
            }
            return resultado;

        }
    }


}
