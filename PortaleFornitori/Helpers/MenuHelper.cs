using PortaleFornitori.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortaleFornitori.Helpers
{
    public static class MenuHelper
    {
        public static MvcHtmlString Menu(this HtmlHelper helper,
            List<Funzione> funzioni)
        {
            //Sto creando un tag ul
            TagBuilder ul = new TagBuilder("ul");
            //Aggiungo la classe sidebar-menu al tag ul
            ul.AddCssClass("sidebar-menu");
            //Aggiungo la classe tree al tag ul
            ul.AddCssClass("tree");
            ul.MergeAttribute("data-widget", "tree");

            MyUserIdentity currentIdentity = HttpContext.Current.User.Identity as MyUserIdentity;
            int currentIdRuolo = currentIdentity.CurrentUser.IdRuolo;
            foreach (var funzione in funzioni.Where(f => f.IdRuolo == currentIdRuolo || f.IdRuolo == null)) {
                AddLi(helper, ul, funzione);
            }

            return new MvcHtmlString(ul.ToString());
        }

        private static bool AddLi(HtmlHelper helper, TagBuilder ul, Funzione funzione)
        {
            String action = helper.ViewContext.RouteData.Values["action"].ToString();
            String controller = helper.ViewContext.RouteData.Values["controller"].ToString();

            String[] values =  funzione.Url.Split("/".ToCharArray());

            String currentFunctionController = "Home";
            if (values.Length > 1)
            {
                currentFunctionController = values[1];
            }
            String currentFunctionAction = "Index";
            if (values.Length > 2)
            {
                currentFunctionAction = values[2];
            }

            bool active = currentFunctionController
                .Equals(controller, StringComparison.InvariantCultureIgnoreCase)
                && currentFunctionAction.Equals(action, StringComparison.InvariantCultureIgnoreCase)
                && !String.IsNullOrEmpty(funzione.Url);
            //Creo il li base
            TagBuilder li = new TagBuilder("li");
            if (active)
            {
                li.AddCssClass("active");
            }
            if (funzione.Figli.Count() > 0)
            {
                //Se ho dei figli assegno al li la classe treeview
                li.AddCssClass("treeview");
            }
            //Genero l'ancora per gestire l'url all'interno del li
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", funzione.Url);
            TagBuilder i = new TagBuilder("i");
            i.AddCssClass(funzione.Icona);
            a.InnerHtml += i.ToString();
            TagBuilder span = new TagBuilder("span");
            span.InnerHtml = funzione.Descrizione;
            a.InnerHtml += span.ToString();

            if (funzione.Figli.Count() > 0)
            {
                TagBuilder spanToggle = new TagBuilder("span");
                spanToggle.AddCssClass("pull-right-container");

                TagBuilder iToggle = new TagBuilder("i");
                iToggle.AddCssClass("fa fa-angle-left pull-right");

                spanToggle.InnerHtml = iToggle.ToString();
                a.InnerHtml += spanToggle;
            }
              
            
            //Nel contenuto del li aggiungo l'ancora
            li.InnerHtml += a;

            if (funzione.Figli.Count() > 0)
            {
                //Genero l'ul figlio
                TagBuilder ulChild = new TagBuilder("ul");
                ulChild.AddCssClass("treeview-menu");
                foreach(var figlio in funzione.Figli)
                {
                    bool isSonActive = AddLi(helper,ulChild, figlio);
                    if (isSonActive)
                    {
                        li.AddCssClass("active");
                    }
                }
                //Aggiungo al li padre l'ul figlio
                li.InnerHtml += ulChild;
            }

            //Metto il li appena creato all'interno dell'ul
            ul.InnerHtml += li;
            return active;
        }
    }
}