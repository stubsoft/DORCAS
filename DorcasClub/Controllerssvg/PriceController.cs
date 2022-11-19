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
    public class PriceController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Price
        public ActionResult Index()
        {
            //var price = db.Price.Include(p => p.Session);
            return View(db.Price.ToList());
        }

        // GET: Dorcas_Price/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Price price = db.Price.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        // GET: Dorcas_Price/Create
        public ActionResult Create()
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat");
            return View();
        }

        // POST: Dorcas_Price/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPrice,from,to,purchase_Price,sale_Price,insertionDate,codeDevis,product_id,type_product,Session_idSession")] Price price)
        {
            if (ModelState.IsValid)
            {
                db.Price.Add(price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat", price.Session_idSession);
            return View(price);
        }

        // GET: Dorcas_Price/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Price price = db.Price.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat", price.Session_idSession);
            return View(price);
        }

        // POST: Dorcas_Price/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPrice,from,to,purchase_Price,sale_Price,insertionDate,codeDevis,product_id,type_product,Session_idSession")] Price price)
        {
            if (ModelState.IsValid)
            {
                db.Entry(price).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat", price.Session_idSession);
            return View(price);
        }

        // GET: Dorcas_Price/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Price price = db.Price.Find(id);
            if (price == null)
            {
                return HttpNotFound();
            }
            return View(price);
        }

        // POST: Dorcas_Price/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Price price = db.Price.Find(id);
            db.Price.Remove(price);
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
