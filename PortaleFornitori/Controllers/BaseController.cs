using PortaleFornitori.Models;
using PortaleFornitori.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortaleFornitori.Controllers
{
    public class BaseController : Controller
    {
        public PortaleFornitoriDbContext Context { get; set; }
        public int PageSize { get; set; }
        public BaseController()
        {
            Context = new PortaleFornitoriDbContext();
            PageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                ViewResult currentResult = filterContext.Result as ViewResult;
                if (currentResult.Model is BaseViewModel)
                {
                    BaseViewModel currentVm = currentResult.Model as BaseViewModel;
                    currentVm.Menu = Context.Funzioni.Where(w => w.IdFunzionePadre.HasValue == false).ToList();

                }
            }
//            ViewBag.Menu = Context.Funzioni.Where(w => w.IdFunzionePadre.HasValue == false).ToList();
            base.OnActionExecuted(filterContext);
        }



        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }


    }
}