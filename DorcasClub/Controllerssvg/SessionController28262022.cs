using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub;
using Newtonsoft.Json;

namespace DorcasClub.Controllers
{
    public class SessionController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Session
        public ActionResult Index()
        {
            var d1 = DateTime.Now.AddMonths(-1).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            var d2 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d1"] = d1;
            ViewData["d2"] = d2;
            var d11 = DateTime.Parse(d1);
            var d22 = DateTime.Parse(d2);
            return View(db.Session.ToList());
        }

        public ActionResult listeseances()
        {
            var abc = from obj in db.Session
                      select new
                      {
                          idSession = obj.idSession,
                          DateSESSION = obj.DateSESSION.ToString(),
                          startHeure = obj.startHeure,
                          endHeure = obj.endHeure,
                          Price = obj.Price
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Session/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Session.Find(id);
            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            ViewData["d1"] = DateTime.Parse(session.DateSESSION.ToString()).ToString("yyyy'-'MM'-'dd");

            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Dorcas_Session/Create
        public ActionResult Create()
        {
            Session dorcas_Session = new Session();

            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Coach.ToList(), "idCoach", "FirstName");
            ViewData["ListCoach"] = ListCoach;

            
            return View(dorcas_Session);
        }
        public ActionResult SearchIndex()
        {
         /*   if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }*/
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var model = from r in db.Session
                        where r.DateCreation > d1 && r.DateCreation < d2 
                        select r;

                model = from r in db.Session
                        where r.DateCreation > d1 && r.DateCreation < d2
                        select r;
            


            return View("Index", model.ToList());
        }

        // POST: Dorcas_Session/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "etat,Materials_idMaterials,Coach_idCoach,DateSESSION,Price")] Session session, String[] startHeure, string[] endHeure)
        {
            if (ModelState.IsValid)
            {
                session.DateCreation = DateTime.Now;
                for(int i=0 ; i<startHeure.Length; i++)
                {
                    Session sesion = new Session();
                    sesion.DateCreation = DateTime.Now;
                    sesion.DateSESSION = session.DateSESSION;
                    sesion.Dure = "";
                    sesion.Price = session.Price;
                    sesion.startHeure = startHeure[i];
                    sesion.endHeure = endHeure[i];
                    sesion.Materials_idMaterials = session.Materials_idMaterials;
                    sesion.Coach_idCoach = session.Coach_idCoach;
                    sesion.etat = session.etat;
                    sesion.UserCreator = Session["UserNom"].ToString();
                    db.Session.Add(sesion);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Dorcas_Session/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Session.Find(id);

            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Coach.ToList(), "idCoach", "FirstName");
            ViewData["ListCoach"] = ListCoach;

            ViewData["d1"] = DateTime.Parse(session.DateSESSION.ToString()).ToString("yyyy'-'MM'-'dd");
      
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }
        public ActionResult Search()
        {
            return View("Search", db.Session.ToList());
        }
        // POST: Dorcas_Session/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idSession,etat,Materials_idMaterials,startHeure,endHeure,Price,DateCreation,DateSESSION")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        // GET: Dorcas_Session/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = db.Session.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Dorcas_Session/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Session.Find(id);
            db.Session.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Calendrier()
        {

            return View("Calendrier");
        }
        public ActionResult listesessionCalendar(Session session)
        {

            var json = JsonConvert.SerializeObject(db.Session);

            return Json(json, JsonRequestBehavior.AllowGet);
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
