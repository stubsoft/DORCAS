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
    
    public partial class Users
    {
        public int idUsers { get; set; }
        public string userName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> roleCode { get; set; }
        public string HashPassword { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
