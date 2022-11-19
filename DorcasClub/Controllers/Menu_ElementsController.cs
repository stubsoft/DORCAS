using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DorcasClub.Controllers
{
    public class Menu_ElementsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();
        // GET: Meals_Element_
        public ActionResult Index()
        {
            var jsonResult = Json(db.Menu_Element.ToList(), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult; 
        }

        // GET: Meals_Element_/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Meals_Element_/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Meals_Element_/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Meals_Element_/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Meals_Element_/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Meals_Element_/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Meals_Element_/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
