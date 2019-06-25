using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortaleFornitori.ViewModels.Fornitori
{
    public class FornitoriIndexViewModel : BaseViewModel
    {
        private FornitoriIndexViewModel()
        {

        }

        public static FornitoriIndexViewModel Load(PortaleFornitoriDbContext ctx, int paginaCorrente, int pageSize)
        {
            FornitoriIndexViewModel vm = new FornitoriIndexViewModel();
            vm.PaginaCorrente = paginaCorrente;
            var numeroFornitori = ctx.Fornitori.Count();
            vm.NumeroPagine = (int)Math.Ceiling(numeroFornitori / (decimal)pageSize);
            vm.Fornitori = ctx.Fornitori
                .OrderBy(o => o.IdFornitore)
                .Skip((vm.PaginaCorrente - 1) * pageSize)
                .Take(pageSize)
                .ToList();
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
                if (vm.NumeroPagine > 2)
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


            return vm;
        }

        public int CodiceFornitore { get; set; }
        public String NomeFile { get; set; }

        
        public List<Fornitore> Fornitori { get; set; }

        public int PaginaCorrente { get; set; }
        public int NumeroPagine { get; set; }
        public List<int> PagineDaVisualizzare { get; internal set; }
    }
}