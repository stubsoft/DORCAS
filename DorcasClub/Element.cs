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
    using System.Web;
    using System.Collections.Generic;
    
    public partial class Element
    {
        public int IdElement { get; set; }
        public string Nom { get; set; }
        public decimal Energie { get; set; }
        public decimal Kj { get; set; }
        public decimal Glucide { get; set; }
        public decimal Proteine { get; set; }
        public System.DateTime DateCreation { get; set; }
        public decimal Prix { get; set; }
        public string Description { get; set; }
        public decimal Poids { get; set; }
        public decimal Lipide { get; set; }
        public string FilePath { get; set; }

        public HttpApplicationStateBase FileImage { get; set; }

    }
}
