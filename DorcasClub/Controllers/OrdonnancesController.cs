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
    public class OrdonnancesController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Ordonnances
        public ActionResult Index()
        {
            return View(db.Ordonnance.ToList());
        }
        public ActionResult listeOrdonnances()
        {
            var abc = from obj in db.Ordonnance

                      select new
                      {
                          IdOrdonance = obj.IdOrdonance,
                          IdAdherent = obj.IdAdherent,
                          NbCalorie = obj.NbCalorie,
                          NomMedecin = obj.NomMedecin
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        // GET: Ordonnances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ListAdherent = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListAdherent"] = ListAdherent;
            Ordonnance ordonnance = db.Ordonnance.Find(id);
            if (ordonnance == null)
            {
                return HttpNotFound();
            }
            return View(ordonnance);
        }

        // GET: Ordonnances/Create
        public ActionResult Create()
        {
            var ListAdherent = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListAdherent"] = ListAdherent;
            
            return View();
        }

        // POST: Ordonnances/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAdherent,NbCalorie,NomMedecin,DateOrdonnance")] Ordonnance ordonnance)
        {
            if (ModelState.IsValid)
            {
                db.Ordonnance.Add(ordonnance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ordonnance);
        }

        // GET: Ordonnances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ListAdherent = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListAdherent"] = ListAdherent;
            Ordonnance ordonnance = db.Ordonnance.Find(id);
            if (ordonnance == null)
            {
                return HttpNotFound();
            }
            return View(ordonnance);
        }

        // POST: Ordonnances/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrdonance,IdAdherent,NbCalorie,NomMedecin,DateOrdonnance")] Ordonnance ordonnance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordonnance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ordonnance);
        }

        // GET: Ordonnances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordonnance ordonnance = db.Ordonnance.Find(id);
            if (ordonnance == null)
            {
                return HttpNotFound();
            }
            return View(ordonnance);
        }

        // POST: Ordonnances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordonnance ordonnance = db.Ordonnance.Find(id);
            db.Ordonnance.Remove(ordonnance);
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
