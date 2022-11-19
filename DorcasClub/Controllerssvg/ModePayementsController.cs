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
    public class ModePayementsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: ModePayements
        public ActionResult Index()
        {
            return View(db.ModePayement.ToList());
        }

        public ActionResult listepaiements()
        {
            var abc = from obj in db.ModePayement
                      select new
                      {
                          idModePayment = obj.idModePayment,
                          Mode = obj.Mode
                         
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: ModePayements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModePayement modePayement = db.ModePayement.Find(id);
            if (modePayement == null)
            {
                return HttpNotFound();
            }
            return View(modePayement);
        }

        // GET: ModePayements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModePayements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idModePayment,Mode")] ModePayement modePayement)
        {
            if (ModelState.IsValid)
            {
                db.ModePayement.Add(modePayement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modePayement);
        }

        // GET: ModePayements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModePayement modePayement = db.ModePayement.Find(id);
            if (modePayement == null)
            {
                return HttpNotFound();
            }
            return View(modePayement);
        }

        // POST: ModePayements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idModePayment,Mode")] ModePayement modePayement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modePayement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modePayement);
        }

        // GET: ModePayements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModePayement modePayement = db.ModePayement.Find(id);
            if (modePayement == null)
            {
                return HttpNotFound();
            }
            return View(modePayement);
        }

        // POST: ModePayements/Delete/5
        //[HttpPost, ActionName("Delete")]
       // s[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModePayement modePayement = db.ModePayement.Find(id);
            db.ModePayement.Remove(modePayement);
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
