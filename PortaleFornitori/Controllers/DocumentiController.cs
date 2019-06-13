using PortaleFornitori.Models;
using PortaleFornitori.ViewModels.Documenti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortaleFornitori.Controllers
{
    
    public class DocumentiController : BaseController
    {
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? paginaCorrente)
        {
            DocumentiIndexViewModel vm = DocumentiIndexViewModel
                .Load(Context, paginaCorrente.HasValue ? paginaCorrente.Value : 1
                , PageSize);
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List(int paginaCorrente)
        {
            System.Threading.Thread.Sleep(5000);
            DocumentiIndexViewModel vm = DocumentiIndexViewModel
            .Load(Context, paginaCorrente, PageSize);
            return PartialView("_List", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create(int id)
        {
            DocumentiCreateViewModel vm = DocumentiCreateViewModel.Load(Context, id);
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create([Bind(Prefix ="Documento")] Documento doc)
        {
            bool success = false;
            if (Request.Files.Count == 0)
            {
                ModelState.AddModelError("File", "Devi selezionare il file da caricare");
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase currentFile = Request.Files[0];
                    if (currentFile.ContentLength == 0)
                    {
                        ModelState.AddModelError("File", "Devi selezionare il file da caricare");
                    }
                    else
                    {
                        //TODO Altre regole di validazione se necessarie
                        byte[] fileContent = new byte[currentFile.ContentLength];
                        currentFile.InputStream.Read(fileContent, 0, fileContent.Length);
                        doc.Contenuto = fileContent;
                        doc.DataCreazione = DateTime.Now;
                        doc.NomeFile = currentFile.FileName;
                        doc.ContentType = currentFile.ContentType;
                        Context.Documenti.Add(doc);
                        Context.SaveChanges();
                        success = true;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Errore", "Errore Generico : " + ex.Message);
                }
            }

            if (success)
            {
                return RedirectToAction("Index");
            }

            DocumentiCreateViewModel vm = DocumentiCreateViewModel.Load(Context, doc.IdDocumento,doc);
            return View(vm);
        }


        [Authorize]
        [HttpGet]
        public ActionResult Download(int id)
        {
            var documento = Context.Documenti.Where(w => w.IdDocumento == id).FirstOrDefault();
            //TODO : Verificare se l'utente attualmente loggato ha i privilegi per scaricare 
            //questo documento
            if (documento == null)
            {
                //Il documento non esiste, devo restituire un messaggio di errore
                return new HttpStatusCodeResult(500, "Documento non esistente");
            }

            return File(documento.Contenuto, documento.ContentType);//,documento.NomeFile);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var documento = Context.Documenti.Where(w => w.IdDocumento == id).FirstOrDefault();
            if (documento == null)
            {
                return new HttpStatusCodeResult(500, "Documento inesistente");
            }
            DocumentoDeleteViewModel vm = new DocumentoDeleteViewModel();
            vm.Documento = documento;
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete([Bind(Prefix = "Documento")]Documento documento) //int idUser,FormCollection values)
        {
            bool success = false;
            try
            {
                if (documento != null)
                {
                    var documento1 = Context.Documenti.Where(w => w.IdDocumento == documento.IdDocumento).FirstOrDefault();
                    documento1.Attivo = false;
                    Context.Entry(documento1).State = System.Data.Entity.EntityState.Modified;
                    Context.SaveChanges();
                    success = true;
                }
                else
                {
                    ModelState.AddModelError("Error", "Documento sconosciuto");
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