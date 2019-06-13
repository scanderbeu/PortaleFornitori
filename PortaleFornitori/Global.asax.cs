using PortaleFornitori.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace PortaleFornitori
{
    public class Global : System.Web.HttpApplication
    {
        public override void Init()
        {
            this.PostAuthenticateRequest += Global_PostAuthenticateRequest;

            base.Init();
        }

        private void Global_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            
            if (authCookie != null)
            {
                string encTicket = authCookie.Value;
                if (!String.IsNullOrEmpty(encTicket))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encTicket);
                    MyUserIdentity id = new MyUserIdentity(ticket);
                    GenericPrincipal prin = new GenericPrincipal(id,id.Roles);
                    HttpContext.Current.User = prin;
                }
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //1 Motore di Rotte (Obbligatorio)
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            //2 Bundles ?
            //3 Filtri Globali?
        }
    }
}