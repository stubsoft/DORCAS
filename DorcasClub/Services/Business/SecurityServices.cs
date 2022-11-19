using DorcasClub.Services.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DorcasClub.Services.Business
{
    public class SecurityServices
    {
        SecurityDAO daoServices = new SecurityDAO();

        public string Authenticate(Utilisateur Utilisateur)
        {
            return daoServices.FindByUser(Utilisateur);
        }

        public DorcasClub.Utilisateur FindByUserName(Utilisateur Utilisateur)
        {
            return daoServices.FindByUserName(Utilisateur);
        }
        public int GetAllListMaterials()
        {
            return daoServices.AllListMaterials();
        }
        public int GetAllListElements()
        {
            return daoServices.AllListElements();
        }
        public int GetAllListAdherents()
        {
            return daoServices.AllListAdherents();
        }
        public int GetAllListReservation()
        {
            return daoServices.AllListReservation();
        }
        public int GetAllListSessions()
        {
            return daoServices.AllListSessions();
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}