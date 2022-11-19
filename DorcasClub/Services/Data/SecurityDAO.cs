using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;

namespace DorcasClub.Services.Data
{
    public class SecurityDAO
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();
        public Boolean authResponse;
        internal string FindByUser(Utilisateur Utilisateur)
        {

            var utilisateurAuthPassword = db.Utilisateur.Where(s => s.MotDePasse == Utilisateur.MotDePasse && s.NomUtilisateur == Utilisateur.NomUtilisateur);

            var result = "error";
            if (utilisateurAuthPassword.Any())
            {
                result = "authConnection";
            }

            return result;

        }
        internal DorcasClub.Utilisateur FindByUserName(Utilisateur Utilisateur)
        {
            Utilisateur utilisateurAuth = db.Utilisateur.First(s => s.MotDePasse == Utilisateur.MotDePasse && s.NomUtilisateur == Utilisateur.NomUtilisateur);
            return (utilisateurAuth);

        }

        internal int AllListMaterials()
        {
            int model = db.Machine.ToList().Count();
            return model;

        }
        internal int AllListElements()
        {
            var model = db.Element.ToList().Count();
            return model;

        }
        internal int AllListAdherents()
        {
            var model = db.Adherent.ToList().Count();
            return model;

        }
        internal int AllListReservation()
        {
            var model = db.Reservation.ToList().Count();
            return model;

        }
        internal int AllListSessions()
        {
            var model = db.Session.ToList().Count();
            return model;

        }

    }
}
   