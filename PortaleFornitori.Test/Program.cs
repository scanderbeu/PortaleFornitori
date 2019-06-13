using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortaleFornitori.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PortaleFornitoriDbContext ctx = new PortaleFornitoriDbContext();
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ValidateOnSaveEnabled = false;
            /*
            //Prendo l'elenco di tutti i fornitori
            var fornitori = ctx.Fornitori.ToList();

            GetFornitoriConNumero(ctx);

            GetFornitori(ctx);

            GetFornitoriCitta(ctx);

            InserisciFornitore(ctx);
            */

            //CaricaDocumento(ctx);

            AggiungiDocumentoSenzaDescrizione(ctx);

            //var fornitore = ctx.Fornitori.Find(4);
            var fornitore = ctx.Fornitori
                .Include("Documenti")
                .Where(w => w.IdFornitore == 4).FirstOrDefault();
            if (fornitore != null) {
                fornitore.RagioneSociale = "Cambiata tramite Update";
                ctx.SaveChanges();
            }

        }

        private static void AggiungiDocumentoSenzaDescrizione(PortaleFornitoriDbContext ctx)
        {
            Documento d = new Documento();
            d.Descrizione = String.Empty;
            d.DataCreazione = DateTime.Now;
            d.IdFornitore = 4;
            d.NomeFile = "IT00737810150_00042.xml";
            d.Contenuto = System.IO.File.ReadAllBytes(@"C:\tmp\IT00737810150_00042.xml");

            ctx.Documenti.Add(d);
            ctx.SaveChanges();
        }

        private static void CaricaDocumento(PortaleFornitoriDbContext ctx)
        {
            Documento d = new Documento();
            d.Descrizione = "File Xml di esempio";
            d.DataCreazione = DateTime.Now;
            d.IdFornitore = 4;
            d.NomeFile = "IT00737810150_00042.xml";
            d.Contenuto = System.IO.File.ReadAllBytes(@"C:\tmp\IT00737810150_00042.xml");

            ctx.Documenti.Add(d);
            ctx.SaveChanges();

        }

        private static void GetFornitoriConNumero(PortaleFornitoriDbContext ctx)
        {
            //Tutti i fornitori che hanno come Città Camerino
            IQueryable<Fornitore> query = ctx.Fornitori.Where(w => w.Citta == "Camerino");
            query = query.Where(w => w.Telefono.Contains("01"));
            var fornitoriCamertiConNumeroTelefono = query.ToList();
        }

        private static void GetFornitoriCitta(PortaleFornitoriDbContext ctx)
        {
            var fornitoriCitta = (from f in ctx.Fornitori
                                  group f by f.Citta into fgrouped
                                  select new
                                  {
                                      Citta = fgrouped.Key
                                  ,
                                      Fornitori = fgrouped.ToList()
                                  }).ToList();
        }

        private static void GetFornitori(PortaleFornitoriDbContext ctx)
        {
            /* Sintassi Fluida */
            var fornitoriCamerti = ctx.Fornitori.Where(w =>
            w.Citta == "Camerino"
            ).ToList();
            /* Sintassi Linq*/
            var fornitoriCamertiLinq = (from f in ctx.Fornitori
                                        where f.Citta == "Camerino"
                                        select f).ToList();
        }

        private static void InserisciFornitore(PortaleFornitoriDbContext ctx)
        {
            /*
            int currentMaxIdFornitore = ctx.Fornitori
                .Max(s => s.IdFornitore);
                */
            Fornitore forn = new Fornitore();
            /*            forn.IdFornitore = currentMaxIdFornitore + 1;*/
            forn.RagioneSociale = "Fornitore inserito da console";
            forn.Telefono = "01010101";
            forn.Indirizzo = "Via Le Mosse";
            forn.Citta = "Camerino";


            ctx.Fornitori.Add(forn);
            ctx.SaveChanges();
        }
    }
}
