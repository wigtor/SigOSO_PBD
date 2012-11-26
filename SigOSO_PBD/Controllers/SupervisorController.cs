using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigOSO_PBD.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
    {
        //
        // GET: /Supervisor/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AsigMat()
        {
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
