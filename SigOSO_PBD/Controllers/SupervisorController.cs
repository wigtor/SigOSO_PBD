using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using SigOSO_PBD.Models;
using System.Web.Mvc;

namespace SigOSO_PBD.Controllers
{
    [Authorize(Roles = "Supervisor")]
    public class SupervisorController : Controller
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
