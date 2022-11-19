using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DorcasClub.Models
{
    public class MachineViewModel
    {
        public int IdMachine { get; set; }
        public string Designation { get; set; }
        public string MaxOccupation { get; set; }
        public string FilePath { get; set; }
        public int IdCategorie { get; set; }

        public HttpApplicationStateBase FileImage { get; set; }
        public virtual Categorie Categorie { get; set; }
    }
}