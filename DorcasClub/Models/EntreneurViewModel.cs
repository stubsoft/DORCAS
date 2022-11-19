using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DorcasClub.Models
{
    public class EntreneurViewModel
    {

        public int IdEntreneur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string CIN { get; set; }
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
        public string Email { get; set; }
        public string Adresse { get; set; }
        public string FilePath { get; set; }

        public HttpApplicationStateBase FileImage { get; set; }
    }
}