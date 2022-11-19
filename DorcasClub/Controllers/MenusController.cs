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
    public class MenusController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Meals
        public ActionResult Index()
        {
            return View(db.Menu.ToList());
        }

        public ActionResult listeMeals()
        {
            var jsonResult = Json(db.Menu.ToList(), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Meals/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu meals = db.Menu.Find(id);

            var model = (from c in db.Element
                         join e in db.Menu_Element on c.IdElement equals e.IdElement
                         where e.IdMenu == id
                         select c).ToList();
            ViewData["listselected"] = model;
            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
        }

        //Details Modal

        public ActionResult DetailsModal(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu meals = db.Menu.Find(id);
            var model = (from c in db.Element
                         join e in db.Menu_Element on c.IdElement equals e.IdElement
                         where e.IdMenu == id
                         select c).ToList();
            ViewData["listselected"] = model;

            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
        }

        // GET: Meals/Create
        public ActionResult Create()
        {
            ViewData["listAliments"] = db.Element.ToList();
            Menu meals = new Menu();
            return View(meals);
        }

        // POST: Meals/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Description,Prix")] Menu meals, string ListAliment)
        {
            if (ModelState.IsValid)
            {
                db.Menu.Add(meals);
                db.SaveChanges();

                string List = ListAliment;

                string[] MenuAliments = List.Split(',');

                foreach (var item in MenuAliments)
                {
                    Menu_Element meals_Elements = new Menu_Element();
                    meals_Elements.IdMenu = meals.IdMenu;
                    meals_Elements.IdElement = Int32.Parse(item);
                    db.Menu_Element.Add(meals_Elements);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(meals);
        }

        // GET: Meals/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu meals = db.Menu.Find(id);
            ViewData["listAliments"] = db.Element.ToList();

            var model = (from c in db.Element
                         join e in db.Menu_Element on c.IdElement equals e.IdElement
                         where e.IdMenu == id
                         select c).ToList();
            ViewData["listselected"] = model;

            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
           
        }

        // POST: Meals/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMenu,Nom,Description,Prix")] Menu meals, string ListAliment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meals).State = EntityState.Modified;
                db.SaveChanges();

                var model = (from c in db.Menu_Element
                             where c.IdMenu == meals.IdMenu
                             select c.IdElement).ToList();

                Menu_Element meals_Element = new Menu_Element();
                foreach (var item in model)
                {
                   meals_Element = db.Menu_Element.Find(item, meals.IdMenu);
                    if (meals_Element.IdMenu == meals.IdMenu && meals_Element.IdElement == item)
                        db.Menu_Element.Remove(meals_Element);
                    db.SaveChanges();
                }

                string List = ListAliment;

                string[] MenuAliments = List.Split(',');

                foreach (var item in MenuAliments)
                {
                    Menu_Element meals_Elements = new Menu_Element();
                    meals_Elements.IdMenu = meals.IdMenu;
                    meals_Elements.IdElement = Int32.Parse(item);
                    db.Menu_Element.Add(meals_Elements);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(meals);
        }

        // GET: Meals/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu meals = db.Menu.Find(id);
            if (meals == null)
            {
                return HttpNotFound();
            }
            return View(meals);
        }

        // POST: Meals/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var model = (from c in db.Menu_Element
                         where c.IdMenu == id
                         select c.IdElement).ToList();
            Menu_Element meals_Elements = new Menu_Element();
            foreach (var item in model)
            {
                meals_Elements = db.Menu_Element.Find(item, id);
                if (meals_Elements.IdMenu == id && meals_Elements.IdElement == item)
                    db.Menu_Element.Remove(meals_Elements);
                db.SaveChanges();
            }
            Menu meals = db.Menu.Find(id);
            db.Menu.Remove(meals);
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
