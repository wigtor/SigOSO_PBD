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
                    int cantidadInsertada = DBConector.INSERT(query);

                    ViewBag.respuestaPost = "Se ha creado correctamente el cliente";
                }
                catch (Exception ex)
                {
                    ViewBag.respuestaPost = "Error al realizar la petición a la base de datos";//ex.Message;
                }
                return View();
            }
            else
            {
                ViewBag.respuestaPost = "";
                return View(nvoCliente);
            }
        }

        //Para visualizar
        [HttpGet]
        public ActionResult AgregarCliente()
        {
            ViewBag.respuestaPost = "Correctamente cargado";
            return View();
        }

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
