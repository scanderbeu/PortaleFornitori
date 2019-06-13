using Newtonsoft.Json;
using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace PortaleFornitori
{
    public class MyUserIdentity : IIdentity
    {
        private FormsAuthenticationTicket ticket;
        public Utente CurrentUser { get; set; }
        public MyUserIdentity(FormsAuthenticationTicket ticket)
        {
            this.ticket = ticket;
            CurrentUser = JsonConvert.DeserializeObject<Utente>(ticket.UserData);
        }


        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return CurrentUser.Email; }
        }

        
        public string[] Roles
        {
            get
            {
                if (CurrentUser.Ruolo == null)
                {
                    return new String[] { };
                }
                return new string[] { CurrentUser.Ruolo.DescrizioneRuolo };
            }
        }

        public string AuthenticationType
        {
            get { return "MyAuth"; }
        }
    }
}