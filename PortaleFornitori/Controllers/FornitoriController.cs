using PortaleFornitori.Models;
using PortaleFornitori.ViewModels.Fornitori;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortaleFornitori.Controllers
{
    [Authorize(Roles="Admin")]
    public class FornitoriController : BaseController
    {
        
        public ActionResult Index()
        {
            FornitoriIndexViewModel vm = FornitoriIndexViewModel.Load(Context, 1, PageSize);
            return View(vm);
        }


        public ActionResult List(int paginaCorrente)
        {
            FornitoriIndexViewModel vm = FornitoriIndexViewModel.Load(Context, paginaCorrente, PageSize);
            return PartialView("_List",vm);
        }

        [HttpGet]
        public ActionResult Delete(int id, bool ultimoFornitore, int paginaCorrente = 1)
        {
            FornitoriDeleteViewModel vm = new FornitoriDeleteViewModel();
            vm.Fornitore = Context.Fornitori.Where(w => w.IdFornitore == id).FirstOrDefault();
            vm.PaginaCorrente = paginaCorrente;
            vm.UltimoFornitore = ultimoFornitore;

            return PartialView("_Delete", vm);
        }

        [HttpPost]
        public ActionResult Delete([Bind(Prefix="Fornitore")]Fornitore fornitore, bool ultimoFornitore, int paginaCorrente = 1)
        {
            bool success = false;
            try
            {
                if (fornitore != null)
                {
                    if (fornitore.Documenti.Count() == 0)
                    {
                        Context.Entry(fornitore).State = System.Data.Entity.EntityState.Deleted;
                        //Context.Utenti.Remove(utente);
                        Context.SaveChanges();
                        success = true;
                    }
                    else {
                        ModelState.AddModelError("Error", "Impossibile eliminare il fornitore");
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Fornitore sconosciuto");
                }
            }
            catch (Exception ex)
            {
                success = false;
                ModelState.AddModelError("Error", ex.Message);
            }
            if (success)
            {
                FornitoriIndexViewModel vm = FornitoriIndexViewModel.Load(Context, ultimoFornitore && paginaCorrente > 1 ? paginaCorrente - 1 : paginaCorrente, PageSize);
                return Json(new { CloseDialog = true,
                        Html = RenderRazorViewToString("_List",vm),
                        TargetId = "table_container",
                        Success = true
                }, JsonRequestBehavior.DenyGet);
            }

            FornitoriDeleteViewModel vmErrore = new FornitoriDeleteViewModel();
            vmErrore.Fornitore = fornitore;


            return Json(new
            {
                CloseDialog = false,
                Html = RenderRazorViewToString("_Delete", vmErrore),
                TargetId = "modalContent",
                Success = false
            }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ActionResult Edit(int id, int paginaCorrente = 1)
        {
            FornitoriEditViewModel vm = new FornitoriEditViewModel();
            vm.Fornitore = Context.Fornitori.Where(w => w.IdFornitore == id)
                .FirstOrDefault() ?? new Models.Fornitore();
            vm.PaginaCorrente = paginaCorrente;
            
            return PartialView("_Edit", vm);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix="Fornitore")]Fornitore fornitore, int paginaCorrente = 1)
        {
            bool success = false;
            if (String.IsNullOrEmpty(fornitore.RagioneSociale))
            {
                ModelState.AddModelError("Fornitore.RagioneSociale", "La ragione sociale è obbligatoria");
            }
            if (String.IsNullOrEmpty(fornitore.Citta))
            {
                ModelState.AddModelError("Fornitore.Citta", "La città è obbligatoria");
            }
            if (String.IsNullOrEmpty(fornitore.Indirizzo))
            {
                ModelState.AddModelError("Fornitore.Indirizzo", "L'indirizzo è obbligatorio");
            }
            if (String.IsNullOrEmpty(fornitore.Telefono))
            {
                fornitore.Telefono = String.Empty;
            }
            //Questa validazione equivale a dei required sui tre campi tramite data annotation
            if (ModelState.IsValid)
            {
                try
                {
                    if (fornitore.IdFornitore == 0)
                    {
                        Context.Fornitori.Add(fornitore);
                    }
                    else
                    {
                        Context.Entry(fornitore).State = System.Data.Entity.EntityState.Modified;
                    }
                    Context.SaveChanges();
                    success = true;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Errore", "Errore generico : " + ex.Message);
                }
            }

            if (success)
            {
                //Se ho successo, bisogna che aggiorno la tabella dei fornitori
                FornitoriIndexViewModel vm = FornitoriIndexViewModel.Load(Context, paginaCorrente, PageSize);
                return Json(new
                {
                    Success = true,
                    Html = RenderRazorViewToString("_List",vm),
                    TargetId = "table_container",
                    CloseDialog = true
                }, JsonRequestBehavior.DenyGet);
                 // PartialView("_List",vm);
            }

            FornitoriEditViewModel vmErrore = new FornitoriEditViewModel();
            vmErrore.Fornitore = fornitore;
            return Json(new
            {
                Success = false,
                Html = RenderRazorViewToString("_Edit", vmErrore),
                TargetId = "modalContent",
                CloseDialog = false
            }, JsonRequestBehavior.DenyGet);
        }

    }
}