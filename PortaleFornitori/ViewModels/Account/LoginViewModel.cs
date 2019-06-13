using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortaleFornitori.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Devi inserire il campo username")]
        public String Username { get; set; }
        [Required(ErrorMessage = "Devi inserire il campo password")]
        public String Password { get; set; }
        public String ReturnUrl { get; set; }
        public bool RememberMe { get; set; }

    }
}