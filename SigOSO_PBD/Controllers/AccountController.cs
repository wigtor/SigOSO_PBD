using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SigOSO_PBD.Models;

namespace SigOSO_PBD.Controllers
{

    public class AccountController : Controller
    {

        public ActionResult About()
        {
            return View();
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("administrador"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (User.IsInRole("Supervisor"))
                {
                    return RedirectToAction("Index", "Supervisor");
                }
                else if (User.IsInRole("Jefe_cuadrilla"))
                {
                    return RedirectToAction("Index", "JefeCuadrilla");
                }
                else if (User.IsInRole("Jefe_bodega"))
                {
                    return RedirectToAction("Index", "JefeBodega");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        
                        if (User.IsInRole("administrador"))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (User.IsInRole("Supervisor"))
                        {
                            return RedirectToAction("Index", "Supervisor");
                        }
                        else if (User.IsInRole("Jefe_cuadrilla"))
                        {
                            return RedirectToAction("Index", "JefeCuadrilla");
                        }
                        else if (User.IsInRole("Jefe_bodega"))
                        {
                            return RedirectToAction("Index", "JefeBodega");
                        }
                        return RedirectToAction("LogOn", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El nombre de usuario o contraseña introducido son incorrectos.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Roles.DeleteCookie();
            Session.Clear();

            return RedirectToAction("LogOn", "Account");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    if (User.IsInRole("administrador"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (User.IsInRole("Supervisor"))
                    {
                        return RedirectToAction("Index", "Supervisor");
                    }
                    else if (User.IsInRole("Jefe_cuadrilla"))
                    {
                        return RedirectToAction("Index", "JefeCuadrilla");
                    }
                    else if (User.IsInRole("Jefe_bodega"))
                    {
                        return RedirectToAction("Index", "JefeBodega");
                    }
                    return RedirectToAction("LogOn", "Account"); //No debiese pasar nunca
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Nombre de usuario ya existe. Porfavor ingrese un nuevo nombre de usuario.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña ingresada es inválida. Porfavor ingrese una contraseña válida.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario ingresado es inválido. Porfavor reviselo e intente nuevamente.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
