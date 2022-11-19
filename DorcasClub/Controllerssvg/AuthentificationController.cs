using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub;
using Microsoft.Owin.Security.Cookies;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using DorcasClub.Services.Business;

namespace DorcasClub.Controllers
{
    
    public class AuthentificationController : Controller
    {
     

        // GET: /Authentification/Index
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // POST: /Authentification/Index
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Utilisateur Utilisateur)
        {
            if (Session["UserNom"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            SecurityServices securityService = new SecurityServices();
            string success = "";
            Fonction Fonction = new Fonction();
       
            success = securityService.Authenticate(Utilisateur);
            

            if (success == "authConnection")
                {
                Utilisateur utilisateurAuth = securityService.FindByUserName(Utilisateur);
                // Emitting forms authentication cookie
                    FormsAuthentication.SetAuthCookie(Utilisateur.MotDePasse, false);

                    Session["UserID"] = utilisateurAuth.IdUser.ToString();
                    Session["UserRole"] = utilisateurAuth.CodeFonction.ToString();
                    Session["UserNom"] = utilisateurAuth.Nom.ToString();
                    Session["UserPrenom"] = utilisateurAuth.Prenom.ToString();
                    //Session["codeResponsable"] = utilisateurAuth.CodeRepresentant;
               
                return RedirectToAction("Index", "Home");
                }
           
            
            if (success == "error")
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "error" });
                
            }

            //return "success authentificate";

            return RedirectToAction("Index", "Home");

        }


        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Remove("UserID");
            Session.Remove("UserNom");
            Session.Remove("UserPrenom");
            Session.Remove("UserRole");
            return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
        }

     
    }
}
