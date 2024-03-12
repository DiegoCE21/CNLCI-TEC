﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web;
using System.Web.Mvc;

namespace CNLCI.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Unauthorized()
        {
            return View("SignOutCallback");
        }
        public void SignIn()
        {
            // Enviar una solicitud de inicio de sesión a OpenID Connect.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                // Redirigir a la página principal si el usuario está autenticado.
                return RedirectToAction("Index", "Home");
            }

           // return RedirectToAction("Login", "Login");
            return RedirectToAction("/", "/");
        }

       




    }
}
