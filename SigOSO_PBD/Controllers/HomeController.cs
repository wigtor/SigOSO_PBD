using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using SigOSO_PBD.classes;
using SigOSO_PBD.Models;

namespace SigOSO_PBD.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC! caca";

            return View();
        }

        public ActionResult About()
        {

            ViewBag.primerValor = cargaUnidades();
            return View();
        }

        public ActionResult AgregarTrabajador()
        {
            
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
                    ViewBag.respuestaPost = "Error al realizar la petición a la base de datos";//ex.Message;
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
                        ViewBag.respuestaPost = "Error al realizar la petición a la base de datos";//ex.Message;
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



        //DEL ADOLFO


        [HttpGet]
        public ActionResult MantServiciosPrestados()
        {
            ViewBag.respuestaPost = "";
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
            return View();
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
                    ViewBag.respuestaPost = "Error al realizar la petición a la base de datos";//ex.Message;
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
