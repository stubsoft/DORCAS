using DorcasClub.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DorcasClub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }

            SecurityServices securityService = new SecurityServices();
            ViewData["Material"] = securityService.GetAllListMaterials();
            // NOMBRE Material
            ViewData["Aliments"] = securityService.GetAllListElements();
            // NOMBRE Aliments
            ViewData["Adherents"] = securityService.GetAllListmembers();
            // NOMBRE Adherents
            ViewData["Reservation"] = securityService.GetAllListReservation(); 
            // NOMBRE Sessions
            ViewData["Sessions"] = securityService.GetAllListSessions();
            return View();
        }


    }
}