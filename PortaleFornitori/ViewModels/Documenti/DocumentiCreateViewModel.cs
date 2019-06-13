using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortaleFornitori.ViewModels.Documenti
{
    public class DocumentiCreateViewModel : BaseViewModel
    {
        private DocumentiCreateViewModel()
        {

        }

        public static DocumentiCreateViewModel Load(PortaleFornitoriDbContext ctx, int idDocumento, Documento doc = null)
        {
            DocumentiCreateViewModel vm = new DocumentiCreateViewModel();
            if (doc != null)
            {
                vm.Documento = doc;
            }
            else
            {
                vm.Documento = ctx.Documenti.Where(w => w.IdDocumento == idDocumento).FirstOrDefault() ?? new Documento();
            }
                vm.Fornitori = ctx.Fornitori.OrderBy(o => o.RagioneSociale).ToList();
            return vm;
        }

        public Documento Documento { get; set; }
        public List<Fornitore> Fornitori { get; set; }
    }
}