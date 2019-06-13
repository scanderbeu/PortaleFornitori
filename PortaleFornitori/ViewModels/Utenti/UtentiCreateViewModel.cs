using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortaleFornitori.ViewModels.Utenti
{
    public class UtentiCreateViewModel : BaseViewModel
    {
        public Utente Utente { get; set; }
        public List<Ruolo> Ruoli { get; set; }
        public List<Fornitore> Fornitori { get; set; }
    }
}