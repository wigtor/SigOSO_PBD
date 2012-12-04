using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using SigOSO_PBD.Models;
using System.Web;
using System.Web.Mvc;

namespace SigOSO_PBD.Controllers
{
    [Authorize(Roles = "Jefe_bodega")]
    public class JefeBodegaController : Controller
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



        public ActionResult Index()
        {
            return View();
        }


        public ActionResult RetiroMat() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult RetiroMat(retiroMaterialModel retiroMaterial, string btn_cargar)
        {
            if (btn_cargar!= null)
            {
                int precioInt;
                if (!Int32.TryParse(retiroMaterial.RUT, out precioInt))
                {
                    ModelState.AddModelError("RUT", "El RUT es invalido");
                }
                if (ModelState.IsValid)
                {

                    string query = "SELECT id_trabajo_interno FROM trabajador NATURAL JOIN cuadrilla NATURAL JOIN trabajo_interno NATURAL JOIN estado_ot NATURAL JOIN autom_estado WHERE rut_trabajador=" + retiroMaterial.RUT + " AND habilitada=true AND final_normal!=true AND final_inesperado=true AND estado_ot.id_estado=autom_estado.id_estado_pasado AND estado_ot.id_estado=trabajo_interno.id_estado_actual_ti;";

                }else{
                    
                }
            }
            return View();
        }

        public ActionResult DevolucionMat()
        {
            return View();

        }

        
    }
}
