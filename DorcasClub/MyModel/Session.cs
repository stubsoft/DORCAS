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
    
    public partial class Session
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Session()
        {
            this.Reservation = new HashSet<Reservation>();
        }
    
        public int idSession { get; set; }
        public string etat { get; set; }
        public string startHeure { get; set; }
        public string endHeure { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public string UserCreator { get; set; }
        public int Materials_idMaterials { get; set; }
        public decimal Price { get; set; }
        public Nullable<System.DateTime> DateSESSION { get; set; }
        public string Dure { get; set; }
        public Nullable<int> Coach_idCoach { get; set; }
    
        public virtual Materials Materials { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual Coach Coach { get; set; }
    }
}
