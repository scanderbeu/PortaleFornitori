using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortaleFornitori.ViewModels.Utenti
{
    public class UtentiIndexViewModel : BaseViewModel
    {
        public IEnumerable<PortaleFornitori.Models.Utente> Utenti { get; set; }
    }
}