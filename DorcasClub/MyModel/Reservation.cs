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
    
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            this.Payment = new HashSet<Payment>();
        }
    
        public int idReservation { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateDu { get; set; }
        public Nullable<System.DateTime> DateAu { get; set; }
        public int Membership_idMembers { get; set; }
        public int Session_idSession { get; set; }
        public int Session_Materials_idMaterials { get; set; }
        public int Element_Meals_idMeal { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string etat { get; set; }
    
        public virtual Memberships Memberships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual Session Session { get; set; }
        public virtual Reservation Reservation1 { get; set; }
        public virtual Reservation Reservation2 { get; set; }
    }
}
