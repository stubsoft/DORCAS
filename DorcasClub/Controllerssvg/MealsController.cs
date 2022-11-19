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
    public class MealsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Meals
        public ActionResult Index()
        {
            return View(db.Meals.ToList());
        }

        public ActionResult listeMeals()
        {
            var jsonResult = Json(db.Meals.ToList(), JsonRequestBehavior.AllowGet);
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
            Meals meals = db.Meals.Find(id);

            var model = (from c in db.Meals_Elements
                         where c.idMeals == id
                         select c.idelements).ToList();

            List <Elements> elements = new List<Elements>();

            foreach(var item in model)
            {
               Elements aliment= db.Elements.Find(item);
                elements.Add(aliment);
            }

            ViewData["listselected"] = elements;
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

        // GET: Meals/Create
        public ActionResult Create()
        {
            ViewData["listAliments"] = db.Elements.ToList();
            Meals meals = new Meals();
            return View(meals);
        }

        // POST: Meals/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idMeal,nameMeal,description,price")] Meals meals, string ListAliment)
        {
            if (ModelState.IsValid)
            {
                db.Meals.Add(meals);
                db.SaveChanges();

                string List = ListAliment;

                string[] MenuAliments = List.Split(',');

                foreach (var item in MenuAliments)
                {
                    Meals_Elements meals_Elements = new Meals_Elements();
                    meals_Elements.idMeals = meals.idMeal;
                    meals_Elements.idelements = Int32.Parse(item);
                    db.Meals_Elements.Add(meals_Elements);
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
            Meals meals = db.Meals.Find(id);
            ViewData["listAliments"] = db.Elements.ToList();

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

        // POST: Meals/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMeal,nameMeal,description,price")] Meals meals, string ListAliment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meals).State = EntityState.Modified;
                db.SaveChanges();

                var model = (from c in db.Meals_Elements
                             where c.idMeals == meals.idMeal
                             select c.idelements).ToList();

                Meals_Elements meals_Element = new Meals_Elements();
                foreach (var item in model)
                {
                   meals_Element = db.Meals_Elements.Find(item, meals.idMeal);
                    if (meals_Element.idMeals == meals.idMeal && meals_Element.idelements == item)
                        db.Meals_Elements.Remove(meals_Element);
                    db.SaveChanges();
                }

                string List = ListAliment.Remove(0, 1);

                string[] MenuAliments = List.Split(',');

                foreach (var item in MenuAliments)
                {
                    Meals_Elements meals_Elements = new Meals_Elements();
                    meals_Elements.idMeals = meals.idMeal;
                    meals_Elements.idelements = Int32.Parse(item);
                    db.Meals_Elements.Add(meals_Elements);
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
            Meals meals = db.Meals.Find(id);
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
            var model = (from c in db.Meals_Elements
                         where c.idMeals == id
                         select c.idelements).ToList();
            Meals_Elements meals_Elements = new Meals_Elements();
            foreach (var item in model)
            {
                meals_Elements = db.Meals_Elements.Find(item, id);
                if (meals_Elements.idMeals == id && meals_Elements.idelements == item)
                    db.Meals_Elements.Remove(meals_Elements);
                db.SaveChanges();
            }
            Meals meals = db.Meals.Find(id);
            db.Meals.Remove(meals);
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
