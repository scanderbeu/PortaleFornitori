using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortaleFornitori.Attributes
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        public string UserFunctions { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated==false)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            MyUserIdentity currentIdentity = filterContext.RequestContext.HttpContext.User.Identity as MyUserIdentity;

            if (!String.IsNullOrEmpty(UserFunctions))
            {
                //Se ho impostato l'accesso mediante privilegi, allora li verifico, altrimenti passa e si effettueranno i controlli 
                //standard dell'authorize attribute

                /* TODO : Mettere i privilegi su una tabella del database e salvarli all'ìnterno del ticket di autenticazione (Cookie)*/
                List<String> currentUserFunctions = new List<string>();
                switch (currentIdentity.Name)
                {
                    case "federico.paoloni@gmail.com":
                        currentUserFunctions.Add("Utenti.Edit");
                        break;
                }

                string[] listOfNeededFunctions = UserFunctions.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                bool isUserEnabled = false;
                foreach (var function in listOfNeededFunctions)
                {
                    if (currentUserFunctions.Contains(function))
                    {
                        isUserEnabled = true;
                        break;
                    }
                }

                if (isUserEnabled == false)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                }
            }

            base.OnAuthorization(filterContext);
        }
    }
}