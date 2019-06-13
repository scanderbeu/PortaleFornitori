using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortaleFornitori.Models
{
    [MetadataType(typeof(UtenteValidation))]
    public partial class Utente
    {
        public class UtenteValidation
        {
            [Required(AllowEmptyStrings =false,ErrorMessage ="Devi selezionare l'username")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Required(AllowEmptyStrings =false,ErrorMessage ="Campo password mancate")]
            public string Password { get; set; }
            [Required(AllowEmptyStrings =false,ErrorMessage ="Campo Nome obbligatorio")]
            public string Nome { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Cognome obbligatorio")]
            public string Cognome { get; set; }

            public Nullable<int> IdFornitore { get; set; }
            [Required(ErrorMessage ="Devi selezionare il ruolo")]
            public int IdRuolo { get; set; }
        }
    }
}
