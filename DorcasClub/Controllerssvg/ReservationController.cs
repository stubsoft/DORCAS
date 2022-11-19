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
    public class ReservationController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Reservation
        public ActionResult Index()

        {
            Reservation dorcas_Reservation = new Reservation();
            var reservation = db.Reservation.Include(r => r.Session);
            /**/
            //DropdownList
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            List<Meals> ListMeals = db.Meals.ToList();
            ViewData["ListMeals"] = ListMeals;

            List<Payment> ListPayement = db.Payment.ToList();
            ViewData["ListPayment"] = ListPayement;

            var ListSession = new SelectList(db.Session.ToList(), "idSession", "etat");
            ViewData["ListSession"] = ListPayement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["dc"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            return View(dorcas_Reservation);
        }

        public ActionResult listeres()
        {
            var abc = from obj in db.Reservation
                      select new
                      {
                          idReservation = obj.idReservation,
                          DateDu = obj.DateDu.ToString(),
                          DateAu = obj.DateAu.ToString(),
                          DateCreation = obj.DateCreation.ToString(),
                          Price = obj.Price,
                          etat = obj.etat
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

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
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;
             List<Meals> ListMeals = db.Meals.ToList();
            ViewData["ListMeals"] = ListMeals;

            
            List<Session> ListSession = db.Session.ToList();
            ViewData["ListSession"] = ListSession;

            ViewData["dc"] = DateTime.Parse(reservation.DateCreation.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["d1"] = DateTime.Parse(reservation.DateDu.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(reservation.DateAu.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            Meals meals = db.Meals.Find(reservation.Element_Meals_idMeal);
            ViewData["namemenu"] = meals.nameMeal; ViewData["prixmenu"] = meals.price;
            Materials materials = db.Materials.Find(reservation.Session_Materials_idMaterials);
            ViewData["namamachine"] = materials.nameMaterials;
            Session session = db.Session.Find(reservation.Session_idSession);
            ViewData["Du"] = session.startHeure;
            ViewData["Au"] = session.endHeure;
            ViewData["Prix"] = session.Price;
            return View(reservation);

        }

        // GET: Dorcas_Reservation/Create
        public ActionResult Create()
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat");
            Reservation dorcas_Reservation = new Reservation();

            //DropdownList
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            List<Materials> ListMaterials = db.Materials.ToList();
            //var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            //var ListMeals = new SelectList(db.Meals.ToList(), "idMeal", "nameMeal");
            List<Meals> ListMeals = db.Meals.ToList();
            ViewData["ListMeals"] = ListMeals;

            //var ListSession = new SelectList(db.Session.ToList(), "idSession", "idSession");
            List<Session> ListSession = db.Session.ToList();
            ViewData["ListSession"] = ListSession;

            ViewData["listselected"] = db.Elements.ToList();

            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["dc"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["stardate"] = "--";
            ViewData["enddate"] = "--";
            ViewData["Machine"] = "--";
            ViewData["NameMachine"] = "--";
            ViewData["Price"] = "--";
            ViewData["prixmenu"] = "--";
            ViewData["namemenu"] = "--";
            return View(dorcas_Reservation);
        }

        public ActionResult CreateR(int SessionID)
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat");
            Reservation dorcas_Reservation = new Reservation();

            //DropdownList
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            List<Materials> ListMaterials = db.Materials.ToList();
            //var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            //var ListMeals = new SelectList(db.Meals.ToList(), "idMeal", "nameMeal");
            List<Meals> ListMeals = db.Meals.ToList();
            ViewData["ListMeals"] = ListMeals;

            //var ListSession = new SelectList(db.Session.ToList(), "idSession", "idSession");
            List<Session> ListSession = db.Session.ToList();
            ViewData["ListSession"] = ListSession;

            ViewData["listselected"] = db.Elements.ToList();

            //ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["dc"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
           

            dorcas_Reservation.Session_idSession = SessionID;
            ViewData["stardate"]=db.Session.Find(SessionID).startHeure;
            ViewData["enddate"] = db.Session.Find(SessionID).endHeure;
            ViewData["Machine"] = db.Session.Find(SessionID).Materials_idMaterials;
            ViewData["Price"] = db.Session.Find(SessionID).Price;
            dorcas_Reservation.Session_Materials_idMaterials = db.Session.Find(SessionID).Materials_idMaterials;
            dorcas_Reservation.DateDu = db.Session.Find(SessionID).DateSESSION;
            dorcas_Reservation.DateAu = db.Session.Find(SessionID).DateSESSION;
            ViewData["d1"] = DateTime.Parse(dorcas_Reservation.DateDu.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(dorcas_Reservation.DateAu.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["NameMachine"] = db.Materials.Find(dorcas_Reservation.Session_Materials_idMaterials).nameMaterials;
            return View("Create",dorcas_Reservation);
        }
        public ActionResult Search()
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat");
            Reservation dorcas_Reservation = new Reservation();

            //DropdownList
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            var ListMeals = new SelectList(db.Meals.ToList(), "idMeal", "nameMeal");
            ViewData["ListMeals"] = ListMeals;

            var ListSession = new SelectList(db.Session.ToList(), "idSession", "etat");
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
            var idMembers = Request["idMembers"];
            var Session_Materials_idMaterials = Request["Session_Materials_idMaterials"];
            var Membership_idMembers = Request["Membership_idMembers"];
            var Element_Meals_idMeal = Request["Element_Meals_idMeal"];
            var idMaterials = Request["idMaterials"];
            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["idMembers"] = idMembers;
            ViewData["Session_Materials_idMaterials"] = Session_Materials_idMaterials;
            ViewData["Element_Meals_idMeal"] = Element_Meals_idMeal;
            ViewData["Membership_idMembers"] = Membership_idMembers;
            ViewData["idMaterials"] = idMaterials;
            var model = from r in db.Reservation

                        where r.DateCreation > d1 && r.DateCreation < d2 || r.Session_Materials_idMaterials.ToString() == Session_Materials_idMaterials || r.Membership_idMembers.ToString() == Membership_idMembers || r.Element_Meals_idMeal.ToString() == Element_Meals_idMeal
                        select r;
            //DropdownList
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            var ListMeals = new SelectList(db.Meals.ToList(), "idMeal", "nameMeal");
            ViewData["ListMeals"] = ListMeals;

            var ListSession = new SelectList(db.Session.ToList(), "idSession", "etat");
            ViewData["ListSession"] = ListSession;

            List<Reservation> ListReservation = model.ToList();
            ViewData["ListReservation"] = ListReservation;

            return View("Search", dorcas_Reservation);
        }


        // POST: Dorcas_Reservation/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,DateDu,DateAu,Session_idSession,Session_Materials_idMaterials,Element_Meals_idMeal,Membership_idMembers,Price")] Reservation reservation)
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
            var ListMembers = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMembers"] = ListMembers;

            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            List<Meals> ListMeals = db.Meals.ToList();
            ViewData["ListMeals"] = ListMeals;

            List<Session> ListSession = db.Session.ToList();
            ViewData["ListSession"] = ListSession;

            ViewData["dc"] = DateTime.Parse(reservation.DateCreation.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["d1"] = DateTime.Parse(reservation.DateDu.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(reservation.DateAu.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            Meals meals = db.Meals.Find(reservation.Element_Meals_idMeal); 
            ViewData["namemenu"] = meals.nameMeal; ViewData["prixmenu"] = meals.price;
            Materials materials = db.Materials.Find(reservation.Session_Materials_idMaterials); 
            ViewData["namamachine"] = materials.nameMaterials;
            Session session = db.Session.Find(reservation.Session_idSession);
            ViewData["Du"] = session.startHeure;
            ViewData["Au"] = session.endHeure;
            ViewData["Prix"] = session.Price;
            return View(reservation);
        }

        // POST: Dorcas_Reservation/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idReservation,Description,DateDu,DateAu,Session_idSession,DateCreation,Session_Materials_idMaterials,Element_Meals_idMeal,Membership_idMembers,Price")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                /*Update selected Machine to base*/
               ////Reservation_Machine reservation_Machine = new Reservation_Machine();

               //// var model = (from c in db.Reservation_Machine
               ////              where c.idReservation == reservation.idReservation
               ////              select c.idMaterials).ToList();

               //// foreach (var item in model)
               //// {
               ////     reservation_Machine = db.Reservation_Machine.Find(reservation.idReservation,item);
               ////     if (reservation_Machine.idReservation == reservation.idReservation && reservation_Machine.idMaterials == item)
               ////         db.Reservation_Machine.Remove(reservation_Machine);
               ////     db.SaveChanges();
               //// }

               //// foreach (var item in SelectedMachine)
               //// {
               ////     reservation_Machine = new Reservation_Machine(); ;
               ////     reservation_Machine.idMaterials = item;
               ////     reservation_Machine.idReservation = reservation.idReservation;
               ////     db.Reservation_Machine.Add(reservation_Machine);
               ////     db.SaveChanges();
               //// }



                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat", reservation.Session_idSession);
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

      [HttpPost]
        public ActionResult DetailsMeals(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meals meals = db.Meals.Find(id);

            var model = (from c in db.Meals_Elements
                         where c.idMeals == id
                         select c.idelements).ToList();

            List<Elements> elements = new List<Elements>();

            foreach (var item in model)
            {
                Elements aliment = db.Elements.Find(item);
                elements.Add(aliment);
            }

            ViewData["listselected"] = elements;
            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
            
        }


        public ActionResult SearchSeance()
        {
            var d1 = DateTime.Now;
            var d2 = DateTime.Now;

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            List<Materials> machine = db.Materials.ToList();
            ViewData["listselected"] = machine;
            ViewData["Nbre"] = db.Session.Count();

            List<Categorie> ListCategorie = db.Categorie.ToList();
            ViewData["ListCategorie"] = ListCategorie;

            return View(db.Session.ToList());
        }
        public ActionResult SearchSeancedate()
        {
            var d1 = DateTime.Parse(Request["d1"]);
            var d2 = DateTime.Parse(Request["d2"]);
           
            var d3 = Request["Materials_idMaterials"].ToString();

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = d2.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            System.Linq.IQueryable<DorcasClub.Session> model ;

            if (string.IsNullOrEmpty(d3))
            {
                model = from r in db.Session
                        where r.DateSESSION > d1 && r.DateSESSION < d2
                        select r;

            }
            else
            {
                var d4 = int.Parse(d3);
                model = from r in db.Session
                        where r.DateSESSION > d1 && r.DateSESSION < d2 && r.Materials_idMaterials == d4
                        select r;
            }
            
            var ListMaterials = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterials"] = ListMaterials;

            List<Materials> machine = db.Materials.ToList();
            ViewData["listselected"] = machine;

            List<Categorie> ListCategorie = db.Categorie.ToList();
            ViewData["ListCategorie"] = ListCategorie;

            ViewData["Nbre"] = model.Count();
            return View("SearchSeance", model.ToList());
        }


        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Materials where temp.idMaterials == Id select temp.imageElement;
            byte[] cover = q.First();
            return cover;
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
