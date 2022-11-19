using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using DorcasClub;
using Newtonsoft.Json;

namespace DorcasClub.Controllers
{
    public class ReservationsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Reservation
        public ActionResult Index()
        {
            Reservation reservation = new Reservation();
            return View(reservation);
        }
        public ActionResult listereservations()
        {
            var model = from a in db.Reservation
                        join b in db.Adherent on a.IdAdherent equals b.IdAdherent
                        join c in db.Session on a.IdSession equals c.IdSession
                        join d in db.Entreneur on c.IdEntreneur equals d.IdEntreneur
                        join e in db.Machine on c.IdMaChine equals e.IdMachine
                        select new
                        {
                            IdReservation = a.IdReservation,
                            DateCreation = a.DateCreation.Value.Year.ToString() + "-" + a.DateCreation.Value.Month.ToString() + "-" + a.DateCreation.Value.Day.ToString(),
                            Description = a.Description,
                            Etat = a.Etat,
                            Livraison = a.Livraison,
                            PrixTotal = a.PrixTotal,
                            Entreneur = d.Nom + "" + d.Prenom,
                            Machine = e.Designation,
                        };

            var jsonResult = Json(model.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;


        }
       
       

        // GET: Dorcas_Reservation/Create
        public ActionResult Create(int SessionID)
       {
            Reservation reservation = new Reservation();
            //DropdownList
            var ListMembers = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListMembers"] = ListMembers;

            List<Menu> ListMeals = db.Menu.ToList();
            ViewData["ListMeals"] = ListMeals;

            ViewData["listselected"] = db.Element.ToList();

            var model = (from a in db.Session
                         join b in db.Machine on a.IdMaChine equals b.IdMachine
                         where a.IdSession == SessionID
                         select new
                         {
                             IdSession = a.IdSession,
                             DateSession = a.DateSession,
                             HeureDebut = a.HeureDebut,
                             Dure = a.Dure,
                             Prix = a.Prix,
                             IdEntreneur = a.IdEntreneur,
                             IdMaChine = a.IdMaChine,
                             FilePath = b.FilePath,
                         }).ToList();

            ViewData["SessionIdSession"] = model.FirstOrDefault().IdSession.ToString();
            ViewData["SessionImageMachine"] = model.FirstOrDefault().FilePath.ToString();
            ViewData["SessionDateSession"] = model.FirstOrDefault().DateSession.ToString();
            ViewData["SessionHeureDebut"] = model.FirstOrDefault().HeureDebut.ToString();
            ViewData["SessionDure"] = model.FirstOrDefault().Dure.ToString();
            ViewData["SessionPrix"] = model.FirstOrDefault().Prix.ToString();
            ViewData["SessionIdEntreneur"] = model.FirstOrDefault().IdEntreneur.ToString();
            ViewData["SessionIdMaChine"] = model.FirstOrDefault().IdMaChine.ToString();

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,Etat,Livraison,PrixTotal,DateCreation,IdSession,IdAdherent,IdMenu")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.DateCreation = DateTime.Now;
                db.Reservation.Add(reservation);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // GET: Dorcas_Reservation/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            //DropdownList
            var ListMembers = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListMembers"] = ListMembers;

            List<Machine> ListMaterials = db.Machine.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            List<Menu> ListMeals = db.Menu.ToList();
            ViewData["ListMeals"] = ListMeals;

            List<Session> ListSession = db.Session.ToList();
            ViewData["ListSession"] = ListSession;

            ViewData["dc"] = DateTime.Parse(reservation.DateCreation.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");


            Menu meals = db.Menu.Find(reservation.IdMenu);
            if (meals != null)
            {
                ViewData["Libelle"] = meals.Nom;
                ViewData["Prix"] = meals.Prix;
            }
            Session session = db.Session.Find(reservation.IdSession);
            ViewData["Du"] = session.HeureDebut;
            ViewData["Au"] = session.HeureFin;
            ViewData["Prix"] = session.Prix;
            return View(reservation);
        }

        // POST: Dorcas_Reservation/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdReservation,Description,Etat,Livraison,PrixTotal,DateCreation,IdSession,IdMenu,IdAdherent")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Session_idSession = new SelectList(db.Session, "IdSession", "Etat", reservation.IdSession);
            return View(reservation);
        }
        // GET: Dorcas_Reservation/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();

            }

            //DropdownList
            var ListMembers = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListMembers"] = ListMembers;

            List<Menu> ListMeals = db.Menu.ToList();
            ViewData["ListMeals"] = ListMeals;

            List<Session> ListSession = db.Session.ToList();
            ViewData["ListSession"] = ListSession;

            ViewData["dc"] = DateTime.Parse(reservation.DateCreation.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            Menu meals = db.Menu.Find(reservation.IdMenu);
            if (meals != null)
            {
                ViewData["Libelle"] = meals.Nom;
                ViewData["Prix"] = meals.Prix;
            }
            Machine materials = db.Machine.Find(reservation.Session.IdMaChine);
            ViewData["Designation"] = materials.Designation;
            Session session = db.Session.Find(reservation.IdSession);
            ViewData["Du"] = session.HeureDebut;
            ViewData["Au"] = session.HeureFin;
            ViewData["Prix"] = session.Prix;
            return View(reservation);

        }
        // GET: Dorcas_Reservation/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }
        // POST: Dorcas_Reservation/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservation.Find(id);
            db.Reservation.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string RetrieveImage(int Id)
        {
            var q = from temp in db.Machine where temp.IdMachine == Id select temp.FilePath;
            string cover = q.First();
            return cover;

        }

        [HttpPost]
        public ActionResult DetailsMeals(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu meals = db.Menu.Find(id);

            var model = (from c in db.Menu_Element
                         where c.IdMenu == id
                         select c.IdElement).ToList();

            List<Element> elements = new List<Element>();

            foreach (var item in model)
            {
                Element aliment = db.Element.Find(item);
                elements.Add(aliment);
            }

            ViewData["listselected"] = elements;
            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
            
        }
        public ActionResult Search()
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "IdSession", "Etat");
            Reservation dorcas_Reservation = new Reservation();

            //DropdownList
            var ListMembers = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListMembers"] = ListMembers;

            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;

            var ListMeals = new SelectList(db.Menu.ToList(), "IdMeal", "nameMeal");
            ViewData["ListMeals"] = ListMeals;

            var ListSession = new SelectList(db.Session.ToList(), "IdSession", "Etat");
            ViewData["ListSession"] = ListSession;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;


            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d11"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d22"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            return View(dorcas_Reservation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchReservation()
        {
            Reservation dorcas_Reservation = new Reservation();
            var d1 = DateTime.Parse(Request["d11"]);
            var d2 = DateTime.Parse(Request["d22"]);
            var IdAdherent = Request["IdAdherent"];
            var IdMachine = Request["IdMachine"];
            var IdEntreneur = Request["IdEntreneur"];
            var IdMenu = Request["IdMeal"];
            var idMaterials = Request["IdMachine"];
            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["IdAdherent"] = IdAdherent;
            ViewData["IdMachine"] = IdMachine;
            ViewData["IdMeal"] = IdMenu;
            ViewData["IdAdherent"] = IdEntreneur;
            var model = from r in db.Reservation

                        where r.DateCreation > d1 && r.DateCreation < d2 || r.IdAdherent.ToString() == IdAdherent || r.IdMenu.ToString() == IdMenu
                        select r;
            //DropdownList
            var ListMembers = new SelectList(db.Adherent.ToList(), "IdAdherent", "Nom");
            ViewData["ListMembers"] = ListMembers;

            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;

            var ListMeals = new SelectList(db.Menu.ToList(), "IdMeal", "nameMeal");
            ViewData["ListMeals"] = ListMeals;

            var ListSession = new SelectList(db.Session.ToList(), "IdSession", "Etat");
            ViewData["ListSession"] = ListSession;

            List<Reservation> ListReservation = model.ToList();
            ViewData["ListReservation"] = ListReservation;

            return View("Search", dorcas_Reservation);
        }
        public ActionResult SearchSeance()
        {
            var d1 = DateTime.Now.Date.AddMonths(-1);
            var d2 = DateTime.Now;

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;

            List<Entreneur> ListCoach = db.Entreneur.ToList();
            ViewData["ListCoach"] = ListCoach;


            ViewData["Nbre"] = db.Session.Count();


            return View(db.Session.Where(e=>e.Etat == "Publiée" && e.DateSession >= d1 && e.DateSession >= d1).ToList().OrderByDescending(i => i.DateSession));


        }
        public ActionResult SearchSeancedate()
        {
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);
           
            var IdMachine = Request["IdMachine"].ToString();

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["IdMachine"] = IdMachine;

            System.Linq.IQueryable<DorcasClub.Session> model ;

            if (string.IsNullOrEmpty(IdMachine))
            {
                model = from r in db.Session
                        where r.DateSession >= d1 && r.DateSession <= d2 && r.Etat == "Publiée"
                        select r;

            }
            else
            {
                var Machine = int.Parse(IdMachine);
                model = from r in db.Session
                        where r.DateSession >= d1 && r.DateSession <= d2 && r.IdMaChine == Machine && r.Etat == "Publiée"
                        select r;
            }

            var ListMaterials = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterials"] = ListMaterials;
            
            List<Entreneur> ListCoach = db.Entreneur.ToList();
            ViewData["ListCoach"] = ListCoach;


            ViewData["Nbre"] = model.Count();

            return View("SearchSeance", model.ToList().OrderByDescending(i => i.DateSession));
        }
        public ActionResult Confirmation()
        {
            var IdReservation = Request["id"];
            //MessageBox.Show(IdReservation);
            Reservation reservation = db.Reservation.Find(Int32.Parse(IdReservation));
            reservation.Etat = "Confirmer";
            db.Entry(reservation).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //importer réservations
        public ActionResult Importer()
        {
            //importer liste des clients
            // var json = new WebClient().DownloadString("http://127.0.0.1:8000/clientsList");
            //importer liste des réservations
            var json = new WebClient().DownloadString("http://127.0.0.1:8000/reservationList");

            dynamic dynJson = JsonConvert.DeserializeObject(json);
            foreach (var item in dynJson)
            {
                // MessageBox.Show(""+item.id+"");
                Reservation reservation = new Reservation();
                reservation.Etat = "enattente";
                reservation.IdMenu = item.idMeal;
                reservation.IdAdherent = item.idMembers;
                reservation.IdSession = item.idSession;
                reservation.PrixTotal = item.prix;

                reservation.DateCreation = DateTime.Now;
                db.Reservation.Add(reservation);
                db.SaveChanges();
                /* Console.WriteLine("{0} {1} {2} {3}\n", item.id, item.prix,
                                    item.heure, item.nom);*/
            }
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
