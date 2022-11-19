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
    public class FonctionsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Fonctions
        public ActionResult Index()
        {
            return View(db.Fonction.ToList());
        }

        // GET: Fonctions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fonction fonction = db.Fonction.Find(id);
            if (fonction == null)
            {
                return HttpNotFound();
            }
            return View(fonction);
        }

        // GET: Fonctions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fonctions/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeFonction,libelle")] Fonction fonction)
        {
            if (ModelState.IsValid)
            {
                db.Fonction.Add(fonction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fonction);
        }

        // GET: Fonctions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fonction fonction = db.Fonction.Find(id);
            if (fonction == null)
            {
                return HttpNotFound();
            }
            return View(fonction);
        }

        // POST: Fonctions/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeFonction,libelle")] Fonction fonction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fonction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fonction);
        }

        // GET: Fonctions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fonction fonction = db.Fonction.Find(id);
            if (fonction == null)
            {
                return HttpNotFound();
            }
            return View(fonction);
        }

        // POST: Fonctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Fonction fonction = db.Fonction.Find(id);
            db.Fonction.Remove(fonction);
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
