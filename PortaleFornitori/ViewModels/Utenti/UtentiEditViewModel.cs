using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortaleFornitori.Models;

namespace PortaleFornitori.ViewModels.Utenti
{
    public class UtentiEditViewModel : BaseViewModel
    {
        public Utente Utente { get; set; }
        public List<Ruolo> Ruoli { get; set; }
        public List<Fornitore> Fornitori { get; set; }
    }
}