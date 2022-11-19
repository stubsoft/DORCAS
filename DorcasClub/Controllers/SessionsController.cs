using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub;
using Newtonsoft.Json;

namespace DorcasClub.Controllers
{
    public class SessionsController : Controller
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
            return View(db.Session.GroupBy(x => x.DateSession).Select(y => y.FirstOrDefault()).ToList());
           
        }
        public ActionResult listeseances()
        {
            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Entreneur.ToList(), "IdEntreneur", "Nom");
            ViewData["ListCoach"] = ListCoach;

            var abc = from obj in db.Session
                      join b in db.Entreneur on obj.IdEntreneur equals b.IdEntreneur
                      join c in db.Machine on obj.IdMaChine equals c.IdMachine
                      select new
                      {
                          IdSession = obj.IdSession,
                          DateSession = obj.DateSession.ToString(),
                          HeureDebut = obj.HeureDebut.ToString().Substring(0,5),
                          HeureFin = obj.HeureFin.ToString().Substring(0, 5),
                          Dure = obj.Dure,
                          Prix = obj.Prix,
                          Machine = c.Designation.ToString(),
                          Entreneur = b.Prenom.ToString() + " " + b.Nom.ToString(),
                          nbPersonne = c.MaxOccupation.ToString()
                      };
            var jsonResult = Json(abc.ToList(), JsonRequestBehavior.AllowGet);
            //var jsonResult = Json(abc.GroupBy(x => x.DateSession).Select(y => y.FirstOrDefault()).ToList(), JsonRequestBehavior.AllowGet);

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
            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Entreneur.ToList(), "IdEntreneur", "Nom");
            ViewData["ListCoach"] = ListCoach;

            //DateTime currentDate = DateTime.Parse(session.DateSession.ToString());
            //var list = db.Session.Where(e => e.DateSession == currentDate).ToList();
            //ViewData["ListSessionDate"] = list;


            ViewData["d1"] = DateTime.Parse(session.DateSession.ToString()).ToString("yyyy'-'MM'-'dd");

            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Dorcas_Session/Create
        public ActionResult Create(string returnUrl)
        {
            Session dorcas_Session = new Session();

            //var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            //ViewData["ListMaterials"] = ListMaterials;

            ViewData["ListMaterials"] =
            new SelectList((from s in db.Machine.ToList()
                            select new
                            {
                                IdMachine = s.IdMachine,
                                NameMachine = s.Designation+ " " +"|" +" " + s.MaxOccupation,
                            }),
                "IdMachine",
                "NameMachine");


            //var ListCoach = new SelectList(db.Entreneur.ToList(), "IdEntreneur", "Nom");
            //ViewData["ListCoach"] = ListCoach;

            ViewData["ListCoach"] =
            new SelectList((from s in db.Entreneur.ToList()
                    select new
                    {
                        IdEntreneur = s.IdEntreneur,
                        FullName = s.Nom + " " +"|" + " " + s.Prenom
                    }),
                "IdEntreneur",
                "FullName");

            ViewBag.ReturnUrl = returnUrl;
            return View(dorcas_Session);
        }
        public ActionResult SearchIndex()
        {
            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Entreneur.ToList(), "IdEntreneur", "Nom");
            ViewData["ListCoach"] = ListCoach;

            var IdEntreneur = Request["IdEntreneur"];
            var IdMachine = Request["IdMachine"];
            ViewData["IdEntreneur"] = IdEntreneur;
            ViewData["IdMachine"] = IdMachine;

            int IdEnt = Convert.ToInt32(IdEntreneur);
            int IdMach = Convert.ToInt32(IdMachine);

            var model = from r in db.Session
                        where  r.IdEntreneur.Equals(IdEnt) || r.IdMaChine.Equals(IdMach)
                        select r;

            return View("Calendrier", model.ToList());
            //return View("Index", model.ToList());
        }

        // POST: Dorcas_Session/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DateSession,HeureDebut,HeureFin,Dure,Etat,Prix,DateCreation,Createur,IdMaChine,IdEntreneur")] Session session)
        {
            if (ModelState.IsValid)
            {
                var model = from a in db.Session
                             where a.IdMaChine == session.IdMaChine && a.DateSession == session.DateSession
                                  && ( session.HeureDebut >= a.HeureDebut && session.HeureDebut <= a.HeureFin)
                             select new
                             {
                                 IdSession = a.IdSession,
                                 DateSession = a.DateSession.ToString(),
                                 HeureDebut = a.HeureDebut,
                                 HeureFin = a.HeureFin,
                                 Dure = a.Dure,
                                 Prix = a.Prix
                             };
                if(model.ToList().Count()==0)
                {
                    session.DateCreation = DateTime.Now;
                    session.HeureFin = session.HeureDebut.Add(new TimeSpan(Convert.ToInt32(session.Dure) / 60, Convert.ToInt32(session.Dure) % 60, 0));
                    session.Createur = Session["UserNom"].ToString();
                    db.Session.Add(session);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Create", new { returnUrl = "error" });
                }
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

            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Entreneur.ToList(), "IdEntreneur", "Nom");
            ViewData["ListCoach"] = ListCoach;

            //DateTime currentDate = DateTime.Parse(session.DateSession.ToString());
            //var list = db.Session.Where(e => e.DateSession == currentDate).ToList();
            //ViewData["ListSessionDate"] = list;

            ViewData["d1"] = DateTime.Parse(session.DateSession.ToString()).ToString("yyyy'-'MM'-'dd");

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
        public ActionResult Edit([Bind(Include = "IdSession,DateSession,HeureDebut,HeureFin,Dure,Etat,Prix,DateCreation,Createur,IdMaChine,IdEntreneur")] Session session)
        {
            if (ModelState.IsValid)
            {
               // Session SessionEncours = db.Session.Find(session.IdSession);
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
            return RedirectToAction("Calendrier");
        }

        public ActionResult Calendrier()
        {
            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;
            var ListCoach = new SelectList(db.Entreneur.ToList(), "IdEntreneur", "Nom");
            ViewData["ListCoach"] = ListCoach;

            return View("Calendrier");
        }
        public ActionResult listesessionCalendar(Session session)
        {

            var model = from a in db.Session
                        join b in db.Entreneur on a.IdEntreneur equals b.IdEntreneur
                        join c in db.Machine on a.IdMaChine equals c.IdMachine
                        select new
                        {
                            IdSession = a.IdSession,
                            DateSession = a.DateSession,
                            HeureDebut = a.HeureDebut.ToString().Substring(0,2) +":"+ a.HeureDebut.ToString().Substring(3,2),
                            HeureFin = a.HeureFin.ToString().Substring(0,2) + ":" + a.HeureFin.ToString().Substring(3,2),
                            Dure = a.Dure,
                            Prix = a.Prix,
                            Machine = c.Designation.ToString(),
                            Entreneur = b.Nom.ToString(),
                            nbPersonne = c.MaxOccupation.ToString(),
                            Color = !a.Etat.Equals("Publiée") ? "red" : a.DateSession >= DateTime.Now ? "#a2d416" : "#faa61a",
                            NbrReservation = (from d in db.Reservation where d.IdSession == a.IdSession select d.IdReservation).Count()
                        };
                     

            var jsonResult = Json(model.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
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
