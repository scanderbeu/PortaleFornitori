using PortaleFornitori.Attributes;
using PortaleFornitori.Models;
using PortaleFornitori.ViewModels.Utenti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortaleFornitori.Controllers
{
    [MyAuthorize(Roles ="Admin")]
        //UserFunctions = "Utenti.Edit,Utenti.Read" )]
    public class UtentiController : BaseController
    {
        // GET: Utenti
        public ActionResult Index()
        {
            //Devo ottenere la lista di utenti
            var utenti = Context.Utenti.ToList();
            UtentiIndexViewModel vm = new UtentiIndexViewModel();
            vm.Utenti = utenti;
            return View(vm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Utente u = new Utente();
            UtentiCreateViewModel vm = new UtentiCreateViewModel();
            vm.Utente = u;
            vm.Ruoli = Context.Ruoli.OrderBy(o => o.DescrizioneRuolo).ToList();
            vm.Fornitori = Context.Fornitori.OrderBy(o => o.RagioneSociale).ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create([Bind(Prefix="Utente")]Utente utente)
        {
            if (utente.IdRuolo==2 && utente.IdFornitore.HasValue == false)
            {
                ModelState.AddModelError("IdFornitore", "Per questo ruolo, il campo fornitore è obbligatorio");
            }
            if (!String.IsNullOrEmpty(utente.Email))
            {
                var utenteEsistente = Context.Utenti
                    .Where(w => w.Email == utente.Email).FirstOrDefault();
                if (utenteEsistente != null)
                {
                    ModelState.AddModelError("Email", "La email da te selezionata è già esistente");
                }
            }
            //Validazione dell'input
            if (ModelState.IsValid)
            {
                //Inserimento all'interno del database
                Context.Utenti.Add(utente);
                Context.SaveChanges();

                //Faccio la redirect alla lista degli utenti
                return RedirectToAction("Index");
            }

            UtentiCreateViewModel vm = new UtentiCreateViewModel();
            vm.Utente = utente;
            vm.Ruoli = Context.Ruoli.OrderBy(o => o.DescrizioneRuolo).ToList();
            vm.Fornitori = Context.Fornitori.OrderBy(o => o.RagioneSociale).ToList();


            return View(vm);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var utente = Context.Utenti.Where(w => w.IdUser == id).FirstOrDefault();
            if (utente == null)
            {
                return new HttpStatusCodeResult(500,"Utente inesistente");
            }
            
            UtentiEditViewModel vm = new UtentiEditViewModel();
            vm.Utente = utente;
            vm.Ruoli = Context.Ruoli.OrderBy(o => o.DescrizioneRuolo).ToList();
            vm.Fornitori = Context.Fornitori.OrderBy(o => o.RagioneSociale).ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Prefix="Utente")]Utente xx)
        {
            if (xx.IdRuolo == 2 && xx.IdFornitore.HasValue == false)
            {
                ModelState.AddModelError("IdFornitore", "Per questo ruolo, il campo fornitore è obbligatorio");
            }
            if (ModelState.IsValid)
            {
                Context.Entry(xx).State = System.Data.Entity.EntityState.Modified;
                Context.SaveChanges();

                return RedirectToAction("Index");
            }

            UtentiEditViewModel vm = new UtentiEditViewModel();
            vm.Utente = xx;
            vm.Ruoli = Context.Ruoli.OrderBy(o => o.DescrizioneRuolo).ToList();
            vm.Fornitori = Context.Fornitori.OrderBy(o => o.RagioneSociale).ToList();
            return View(vm);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var utente = Context.Utenti.Where(w => w.IdUser == id).FirstOrDefault();
            if (utente == null)
            {
                return new HttpStatusCodeResult(500, "Utente inesistente");
            }
            UtentiDeleteViewModel vm = new UtentiDeleteViewModel();
            vm.Utente = utente;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Delete([Bind(Prefix="Utente")]Utente utente) //int idUser,FormCollection values)
        {
            bool success = false;
            //var utente = Context.Utenti.Where(w => w.IdUser == idUser).FirstOrDefault();
            try
            {
                if (utente != null)
                {
                    Context.Entry(utente).State = System.Data.Entity.EntityState.Deleted;
                    //Context.Utenti.Remove(utente);
                    Context.SaveChanges();
                    success = true;
                }
                else
                {
                    ModelState.AddModelError("Error", "Utente sconosciuto");
                }
            }
            catch (Exception ex)
            {
                success = false;
                ModelState.AddModelError("Error", ex.Message);
            }
            if (success)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }


    
    }
}