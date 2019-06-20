using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortaleFornitori.Models;

namespace PortaleFornitori.ViewModels.Fornitori
{
    public class FornitoriDeleteViewModel
    {
        public Fornitore Fornitore { get; internal set; }
        public int PaginaCorrente { get; internal set; }
        public bool UltimoFornitore { get; internal set; }
    }
}