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
    public class InterventionController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Intervention
        public ActionResult Index()
        {
            var preventiveIntervention = db.PreventiveIntervention.Include(p => p.Materials);
            return View(preventiveIntervention.ToList());
        }

        public ActionResult listeinterventions()
        {
            var abc = from obj in db.PreventiveIntervention
                      select new
                      {
                          idIntervention = obj.idIntervention,
                          title = obj.title,
                          dateIntervention = obj.dateIntervention.ToString(),
                          Description = obj.Description,


                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Intervention/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreventiveIntervention preventiveIntervention = db.PreventiveIntervention.Find(id);
            if (preventiveIntervention == null)
            {
                return HttpNotFound();
            }
            ViewBag.Materials_idMaterials = new SelectList(db.Materials, "idMaterials", "nameMaterials", preventiveIntervention.Materials_idMaterials);
            ViewBag.Materials_Coach_idCoach = new SelectList(db.Coach, "idCoach", "FirstName");
            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            return View(preventiveIntervention);
        }

        // GET: Dorcas_Intervention/Create
        public ActionResult Create()
        {
            ViewBag.Materials_idMaterials = new SelectList(db.Materials, "idMaterials", "nameMaterials");
            ViewBag.Materials_Coach_idCoach = new SelectList(db.Coach, "idCoach", "FirstName");
            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            return View();
        }

        // POST: Dorcas_Intervention/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "dateIntervention,title,Description,Materials_idMaterials,Materials_Coach_idCoach")] PreventiveIntervention preventiveIntervention)
        {
            if (ModelState.IsValid)
            {
                db.PreventiveIntervention.Add(preventiveIntervention);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Materials_idMaterials = new SelectList(db.Materials, "idMaterials", "nameMaterials", preventiveIntervention.Materials_idMaterials);
            return View(preventiveIntervention);
        }

        // GET: Dorcas_Intervention/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreventiveIntervention preventiveIntervention = db.PreventiveIntervention.Find(id);
            
            ViewData["d1"] = DateTime.Parse(preventiveIntervention.dateIntervention.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
        
            if (preventiveIntervention == null)
            {
                return HttpNotFound();
            }
            ViewBag.Materials_idMaterials = new SelectList(db.Materials, "idMaterials", "nameMaterials", preventiveIntervention.Materials_idMaterials);
            ViewBag.Materials_Coach_idCoach = new SelectList(db.Coach, "idCoach", "FirstName");
           

            return View(preventiveIntervention);
        }

        // POST: Dorcas_Intervention/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idIntervention,dateIntervention,title,Description,Materials_idMaterials,Materials_Coach_idCoach")] PreventiveIntervention preventiveIntervention)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preventiveIntervention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Materials_idMaterials = new SelectList(db.Materials, "idMaterials", "nameMaterials", preventiveIntervention.Materials_idMaterials);
            return View(preventiveIntervention);
        }

        // GET: Dorcas_Intervention/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreventiveIntervention preventiveIntervention = db.PreventiveIntervention.Find(id);
            if (preventiveIntervention == null)
            {
                return HttpNotFound();
            }
            return View(preventiveIntervention);
        }

        // POST: Dorcas_Intervention/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PreventiveIntervention preventiveIntervention = db.PreventiveIntervention.Find(id);
            db.PreventiveIntervention.Remove(preventiveIntervention);
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
