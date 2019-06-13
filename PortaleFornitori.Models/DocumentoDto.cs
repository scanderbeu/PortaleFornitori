using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortaleFornitori.Models
{
    public class DocumentoDto
    {
        public int IdDocumento { get; set; }
        public string Descrizione { get; set; }
        public System.DateTime DataCreazione { get; set; }
        public string NomeFile { get; set; }
        public string RagioneSocialeFornitore { get; set; }
        public bool Attivo { get; set; }
    }
}
