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
        public int IdReservation { get; set; }
        public string Description { get; set; }
        public string Etat { get; set; }
        public string Livraison { get; set; }
        public decimal PrixTotal { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public int IdSession { get; set; }
        public int IdAdherent { get; set; }
        public int IdMenu { get; set; }
    
        public virtual Adherent Adherent { get; set; }
        public virtual Session Session { get; set; }
    }
}
