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
    
    public partial class Price
    {
        public int idPrice { get; set; }
        public Nullable<System.DateTime> from { get; set; }
        public Nullable<System.DateTime> to { get; set; }
        public string purchase_Price { get; set; }
        public string sale_Price { get; set; }
        public Nullable<System.DateTime> insertionDate { get; set; }
        public string codeDevis { get; set; }
        public string product_id { get; set; }
        public string type_product { get; set; }
        public int Session_idSession { get; set; }
        public int Elements_Meals_idMeal { get; set; }
        public string etat { get; set; }
    
        public virtual Elements Elements { get; set; }
    }
}
