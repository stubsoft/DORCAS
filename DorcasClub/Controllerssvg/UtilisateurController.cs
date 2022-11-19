using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub;

namespace DorcasClub.Controllers
{
    public class UtilisateurController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Utilisateur
        public ActionResult Index()
        {
            var Utilisateur = db.Utilisateur.Include(u => u.Fonction);
            return View(Utilisateur.ToList());
        }
        public ActionResult listeusers()
        {
            var abc = from obj in db.Utilisateur
                      select new
                      {
                          IdUser = obj.IdUser,
                          Nom = obj.Nom,
                          Prenom = obj.Prenom
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        private static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public ActionResult listeUtilisateurs()
        {
            var jsonResult = Json(db.Utilisateur.ToList(), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Utilisateur/Details/5
        public ActionResult Details(int id)

        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur Utilisateur = db.Utilisateur.Find(id);
            ViewData["Fonction"] =db.Fonction.Find(Utilisateur.CodeFonction).libelle;
            if (Utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(Utilisateur);
        }

        // GET: Utilisateur/Create
        public ActionResult Create()
        {
            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "libelle");
            return View();
        }

        // POST: Utilisateur/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Prenom,MotDePasse,CodeFonction")] Utilisateur Utilisateur)
        {
            if (ModelState.IsValid)
            {
                Utilisateur.CodeSociete = "";
                db.Utilisateur.Add(Utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "libelle", Utilisateur.CodeFonction);
            return View(Utilisateur);
        }

        // GET: Dorcas_Utilisateur/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur Utilisateur = db.Utilisateur.Find(id);
            if (Utilisateur == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "libelle", Utilisateur.CodeFonction);
            return View(Utilisateur);
        }

        // POST: Utilisateur/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NomUtilisateur,Nom,Prenom,MotDePasse,CodeFonction")] Utilisateur Utilisateur)
        {
            if (ModelState.IsValid)
            {
                Utilisateur.CodeSociete = "";
                db.Entry(Utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodeFonction = new SelectList(db.Fonction, "CodeFonction", "libelle", Utilisateur.CodeFonction);
            return View(Utilisateur);
        }

        // GET: Dorcas_Utilisateur/Delete/5
        public ActionResult Delete(int id)

        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Utilisateur Utilisateur = db.Utilisateur.Find(id);
            if (Utilisateur == null)
            {
                return HttpNotFound();
            }
            return View(Utilisateur);
        }

        // POST: Utilisateur/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur Utilisateur = db.Utilisateur.Find(id);
            db.Utilisateur.Remove(Utilisateur);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult CreateFonction(string libelle)
        {
            try
            {
                Fonction Fonction = new Fonction();
                Fonction.libelle = libelle;
                db.Fonction.Add(Fonction);

                db.SaveChanges();

                return Json(new
                {
                    msg = "Fonction ajouté",
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
