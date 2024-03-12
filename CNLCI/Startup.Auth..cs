/*using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Security.Claims;

namespace CNLCI
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string authority = aadInstance + tenantId + "/v2.0";

        public void ConfigureAuth(IAppBuilder app)
        {

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {

                ExpireTimeSpan = TimeSpan.FromMinutes(1),

                SlidingExpiration = true,

                //LoginPath = new PathString("/Login/Login"),
                LoginPath = new PathString("/"),
                CookieSameSite = SameSiteMode.None
                    
            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        SecurityTokenValidated = (context) =>
                        {
                            string name = context.AuthenticationTicket.Identity.FindFirst("name").Value;
                            context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, name, string.Empty));
                            return System.Threading.Tasks.Task.FromResult(0);


                        }
                    }
                });


        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}

using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Security.Claims;

namespace CNLCI
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string authority = aadInstance + tenantId + "/v2.0";

        public void ConfigureAuth(IAppBuilder app)
        {

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {

                ExpireTimeSpan = TimeSpan.FromMinutes(1),

                SlidingExpiration = true,

                //LoginPath = new PathString("/Login/Login"),
                LoginPath = new PathString("/"),
                CookieSameSite = SameSiteMode.None

            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = (context) =>
                        {
                            var email = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Email)?.Value;

                            if (string.IsNullOrEmpty(email))
                            {
                                context.Response.Redirect("/NoCorreo");
                                context.HandleResponse(); // Previene la ejecuci�n de m�s eventos
                            }
                            else if (!email.Equals("l20660044@matehuala.tecnm.mx", StringComparison.OrdinalIgnoreCase))
                            {

                                context.OwinContext.Authentication.SignOut(
                               OpenIdConnectAuthenticationDefaults.AuthenticationType,
                               CookieAuthenticationDefaults.AuthenticationType);
                                context.HandleResponse(); // Previene la ejecuci�n de m�s eventos
                            }
                            else
                            {
                                string name = context.AuthenticationTicket.Identity.FindFirst("name").Value;
                                context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, name, string.Empty));
                            }

                            return System.Threading.Tasks.Task.FromResult(0);
                        }
                    }
                });



        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Security.Claims;
using CNLCI.Models; // Aseg�rate de que este namespace sea el correcto para tus modelos y contexto de base de datos
using System.Linq;
using Microsoft.Owin.Security;

namespace CNLCI
{
    public partial class Startup
    {
        // Aseg�rate de tener configuradas estas variables con los valores correctos de tu aplicaci�n
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string authority = aadInstance + tenantId + "/v2.0";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = async (context) =>
                        {
                            var emailClaim = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Email);
                            if (emailClaim == null)
                            {
                                // Si no se encuentra el claim de correo electr�nico, redirige a una p�gina de error
                                context.Response.Redirect("/Error");
                                context.HandleResponse();
                                return;
                            }

                            var email = emailClaim.Value;
                            using (var db = new DB()) // Aseg�rate de que 'DB' es el nombre correcto de tu contexto de base de datos
                            {
                                var usuario = db.Usuarios.FirstOrDefault(u => u.Correo.Equals(email, StringComparison.OrdinalIgnoreCase));
                                if (usuario == null)
                                {
                                    // Si el correo electr�nico no se encuentra en la base de datos, desconectar y redirigir a una p�gina de acceso denegado
                                    context.OwinContext.Authentication.SignOut(
                                        OpenIdConnectAuthenticationDefaults.AuthenticationType,
                                        CookieAuthenticationDefaults.AuthenticationType);
                                    context.Response.Redirect("/AccesoDenegado");
                                    context.HandleResponse();
                                }
                                else
                                {
                                    string name = context.AuthenticationTicket.Identity.FindFirst("name").Value;
                                    context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, name, string.Empty));
                                }
                            }
                        }
                    }
                });
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }
            return value;
        }
    }
}
*/

using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Security.Claims;
using CNLCI.Models; // Aseg�rate de que este namespace sea correcto
using System.Linq; // Para poder usar LINQ
using System.Threading.Tasks; // Para Task

namespace CNLCI
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string authority = aadInstance + tenantId + "/v2.0";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromMinutes(1),
                SlidingExpiration = true,
                LoginPath = new PathString("/"),
                CookieSameSite = SameSiteMode.None
            });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = async context =>
                        {
                            var email = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Email)?.Value;
                            if (email == null)
                            {
                                context.Response.Redirect("/Error");
                                context.HandleResponse();
                                return;
                            }

                            // Utiliza tu contexto de base de datos aqu�
                            using (var db = new DB()) // Aseg�rate de que 'DB' es el nombre correcto de tu contexto de base de datos
                            {
                                var usuario = db.Usuarios.FirstOrDefault(u => u.Correo.Equals(email, StringComparison.OrdinalIgnoreCase));
                                if (usuario != null)
                                {
                                    // Si el usuario existe en la base de datos, asigna el correo como su nombre
                                    // Aqu� podr�as ajustar para asignar otro valor del usuario como el nombre
                                    context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, email));
                                }
                                else
                                {
                                    // Si el usuario no existe en la base de datos, desconectar
                                    context.OwinContext.Authentication.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
                                    context.Response.Redirect("/AccesoDenegado");
                                    context.HandleResponse();
                                }
                            }
                        }
                    }
                });
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}
