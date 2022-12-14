//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DorcasClub
{
    using System;
    using System.Collections.Generic;
    
    public partial class Materials
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materials()
        {
            this.PreventiveIntervention = new HashSet<PreventiveIntervention>();
            this.Session = new HashSet<Session>();
        }
    
        public int idMaterials { get; set; }
        public string nameMaterials { get; set; }
        public string MaxOccupation { get; set; }
        public Nullable<System.DateTime> Availability { get; set; }
        public Nullable<int> idCoach { get; set; }
        public byte[] imageElement { get; set; }
        public string FileName { get; set; }
        public string imagephysique { get; set; }
        public int IdCategorie { get; set; }
        public string SelectedCoach { get; set; }
        public Nullable<int> DureeSeances { get; set; }
        public Nullable<decimal> PrixSeances { get; set; }
    
        public virtual Categorie Categorie { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreventiveIntervention> PreventiveIntervention { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Session { get; set; }
    }
}
