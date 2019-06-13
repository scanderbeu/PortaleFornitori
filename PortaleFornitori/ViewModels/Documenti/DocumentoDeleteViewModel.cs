using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortaleFornitori.Models;

namespace PortaleFornitori.ViewModels.Documenti
{
    public class DocumentoDeleteViewModel : BaseViewModel
    {
        public Documento Documento { get; internal set; }
    }
}