//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortaleFornitori.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Funzione
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Funzione()
        {
            this.Figli = new HashSet<Funzione>();
        }
    
        public int IdFunzione { get; set; }
        public string Icona { get; set; }
        public string Descrizione { get; set; }
        public string Url { get; set; }
        public bool Abilitato { get; set; }
        public Nullable<int> IdFunzionePadre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Funzione> Figli { get; set; }
        public virtual Funzione Padre { get; set; }
    }
}