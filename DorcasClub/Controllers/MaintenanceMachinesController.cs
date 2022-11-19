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
    public class MaintenanceMachinesController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_MaintenanceMachine
        public ActionResult Index()
        {
            var maintenanceMachine = db.MaintenanceMachine;
            return View(maintenanceMachine.ToList());
        }
        public ActionResult listemaintenanceMachines()
        {
            var abc = from obj in db.MaintenanceMachine
                      select new
                      {
                          IdMaintenance = obj.IdMaintenance,
                          Titre = obj.Titre,
                          DateMaintenance = obj.DateMaintenance.ToString(),
                          Description = obj.Description,


                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        // GET: Dorcas_MaintenanceMachine/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceMachine maintenanceMachine = db.MaintenanceMachine.Find(id);
            if (maintenanceMachine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMachine = new SelectList(db.Machine, "IdMachine", "Designation", maintenanceMachine.IdMachine);
            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            return View(maintenanceMachine);
        }

        // GET: Dorcas_MaintenanceMachine/Create
        public ActionResult Create()
        {
            ViewBag.IdMachine = new SelectList(db.Machine, "IdMachine", "Designation");
            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            return View();
        }

        // POST: Dorcas_MaintenanceMachine/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DateMaintenance,Titre,Description,IdMachine")] MaintenanceMachine maintenanceMachine)
        {
            if (ModelState.IsValid)
            {
                db.MaintenanceMachine.Add(maintenanceMachine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdMachine = new SelectList(db.Machine, "IdMachine", "Designation", maintenanceMachine.IdMachine);
            return View(maintenanceMachine);
        }

        // GET: Dorcas_Intervention/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceMachine maintenanceMachine = db.MaintenanceMachine.Find(id);
            
            ViewData["d1"] = DateTime.Parse(maintenanceMachine.DateMaintenance.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
        
            if (maintenanceMachine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMachine = new SelectList(db.Machine, "IdMachine", "Designation", maintenanceMachine.IdMachine);
           

            return View(maintenanceMachine);
        }

        // POST: Dorcas_MaintenanceMachine/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMaintenance,DateMaintenance,Titre,Description,IdMachine")] MaintenanceMachine maintenanceMachine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maintenanceMachine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMachine = new SelectList(db.Machine, "IdMachine", "Designation", maintenanceMachine.IdMachine);
            return View(maintenanceMachine);
        }

        // GET: Dorcas_MaintenanceMachine/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceMachine maintenanceMachine = db.MaintenanceMachine.Find(id);
            if (maintenanceMachine == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceMachine);
        }

        // POST: Dorcas_maintenanceMachine/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MaintenanceMachine maintenanceMachine = db.MaintenanceMachine.Find(id);
            db.MaintenanceMachine.Remove(maintenanceMachine);
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
