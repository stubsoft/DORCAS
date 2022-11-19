using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub;

namespace DorcasClub.Controllers
{
    public class SocietesController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Societes
        public ActionResult Index()
        {
            return View(db.Societe.ToList());
        }

        // GET: Societes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // GET: Societes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Societes/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeSociete,CodeForme,RaisonSociale,Responsable,MatriculeFiscale,RegistreCommerce,CodeBanque,RibBanquaire,Adresse,CodePostal,Ville,Pays,Tel1,Tel2,FAX,Mail,SiteWeb,Logo,Observation,CodeCNAM")] Societe societe)
        {
            if (ModelState.IsValid)
            {
                db.Societe.Add(societe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(societe);
        }

        // GET: Societes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // POST: Societes/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeSociete,CodeForme,RaisonSociale,Responsable,MatriculeFiscale,RegistreCommerce,CodeBanque,RibBanquaire,Adresse,CodePostal,Ville,Pays,Tel1,Tel2,FAX,Mail,SiteWeb,Logo,Observation,CodeCNAM")] Societe societe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(societe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(societe);
        }

        // GET: Societes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Societe societe = db.Societe.Find(id);
            if (societe == null)
            {
                return HttpNotFound();
            }
            return View(societe);
        }

        // POST: Societes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Societe societe = db.Societe.Find(id);
            db.Societe.Remove(societe);
            db.SaveChanges();
            return RedirectToAction("Index");
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
