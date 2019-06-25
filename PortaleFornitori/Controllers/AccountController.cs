using Newtonsoft.Json;
using PortaleFornitori.Helpers;
using PortaleFornitori.Models;
using PortaleFornitori.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PortaleFornitori.Controllers
{
    public class AccountController : BaseController
    {


        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            LoginViewModel vm = new LoginViewModel();
            vm.RememberMe = true;
            vm.ReturnUrl = returnUrl;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                //Effettuo il controllo sull'utente
                var utente = Context.Utenti.Where(w => w.Email == vm.Username).FirstOrDefault();
                if (utente != null)
                {
                    if (utente.Password == vm.Password)
                    {
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, //version 
                        utente.Email,
                        DateTime.Now,
                        DateTime.Now.AddDays(1), //Expiration
                        vm.RememberMe, //Persistent
                        JsonConvert.SerializeObject(
                            CookieHelper.GetUserData(utente), new JsonSerializerSettings {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }));
                        
                        string encTicket = FormsAuthentication.Encrypt(authTicket);

                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                        cookie.Expires = authTicket.Expiration;
                        Response.Cookies.Add(cookie);

                        if (String.IsNullOrEmpty(vm.ReturnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(vm.ReturnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "La password inserita non è valida");
                    }
                }
                else
                {
                    ModelState.AddModelError("Username", "L'utente da te inserito non esiste nell'applicazione");
                }
            }
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}