using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigOSO_PBD.Controllers
{
    [Authorize(Roles = "Jefe_bodega")]
    public class JefeBodegaController : Controller
    {
        //
        // GET: /JefeBodega/

        public ActionResult Index()
        {
            return View();
        }

    }
}
