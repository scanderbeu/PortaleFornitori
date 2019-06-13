using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortaleFornitori.Models
{
    [MetadataType(typeof(DocumentoMetadata))]
    public partial class Documento
    {
        public String Estensione { get
            {
                return System.IO.Path.GetExtension(NomeFile);
            }
        }

        public class DocumentoMetadata {
            [Required]
            public string Descrizione { get; set; }
        }
    }
}
