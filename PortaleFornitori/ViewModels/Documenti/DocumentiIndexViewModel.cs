using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortaleFornitori.ViewModels.Documenti
{
    public class DocumentiIndexViewModel : BaseViewModel
    {
        private DocumentiIndexViewModel()
        {

        }

        public static DocumentiIndexViewModel Load(PortaleFornitoriDbContext ctx, int paginaCorrente, int pageSize, 
            Func<Documento,bool> filter, bool bloccaEdit)
        {
            DocumentiIndexViewModel vm = new DocumentiIndexViewModel();
            vm.PaginaCorrente = paginaCorrente;
            var numeroDiDocumenti = ctx.Documenti.Count();
            vm.NumeroPagine = (int)Math.Ceiling(numeroDiDocumenti / (decimal)pageSize);
            vm.Documenti = ctx.Documenti
                .Where(filter)
                .OrderBy(o => o.IdDocumento)
                .Skip((vm.PaginaCorrente - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new DocumentoDto
                {
                    DataCreazione = s.DataCreazione,
                    Descrizione = s.Descrizione,
                    IdDocumento = s.IdDocumento,
                    NomeFile = s.NomeFile,
                    RagioneSocialeFornitore = s.Fornitori.RagioneSociale
                }).ToList();
            vm.Fornitori = ctx.Fornitori.OrderBy(o => o.RagioneSociale).ToList();

            ImpostaPagineDaVisualizzare(vm);

            vm.BloccaEdit = bloccaEdit;

            return vm;
        }

        
        public int CodiceFornitore { get; set; }
        public String NomeFile { get; set; }

        public List<DocumentoDto> Documenti { get; set; }

        public List<Fornitore> Fornitori { get; set; }

        public int PaginaCorrente { get; set; }
        public int NumeroPagine { get; set; }
        public List<int> PagineDaVisualizzare { get; internal set; }
        public bool BloccaEdit { get; private set; }

        private static void ImpostaPagineDaVisualizzare(DocumentiIndexViewModel vm) {
            vm.PagineDaVisualizzare = new List<int>();

            if (vm.PaginaCorrente == 1)
            {
                vm.PagineDaVisualizzare.Add(1);
                if (vm.NumeroPagine > 1)
                {
                    vm.PagineDaVisualizzare.Add(2);
                }
                if (vm.NumeroPagine > 2)
                {
                    vm.PagineDaVisualizzare.Add(3);
                }
            }
            else if (vm.PaginaCorrente == vm.NumeroPagine)
            {
                if (vm.NumeroPagine > vm.PaginaCorrente - 2)
                {
                    vm.PagineDaVisualizzare.Add(vm.PaginaCorrente - 2);
                }
                vm.PagineDaVisualizzare.Add(vm.PaginaCorrente - 1);
                vm.PagineDaVisualizzare.Add(vm.PaginaCorrente);
            }
            else
            {
                vm.PagineDaVisualizzare.Add(vm.PaginaCorrente - 1);
                vm.PagineDaVisualizzare.Add(vm.PaginaCorrente);
                vm.PagineDaVisualizzare.Add(vm.PaginaCorrente + 1);
            }
        }
    }
}