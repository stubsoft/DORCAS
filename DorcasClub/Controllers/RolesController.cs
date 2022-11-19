using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DorcasClub.Controllers
{
    public class RolesController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Role.ToList());
        }

        public ActionResult listeRoles()
        {
            var abc = from obj in db.Role
                      select new
                      {
                          IdRole = obj.IdRole,
                          Libelle = obj.Libelle
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role Role = db.Role.Find(id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            return View(Role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRole,Libelle")] Role Role)
        {
            if (ModelState.IsValid)
            {
                db.Role.Add(Role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role Role = db.Role.Find(id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            return View(Role);
        }

        // POST: Fonctions/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRole,Libelle")] Role Role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role Role = db.Role.Find(id);
            if (Role == null)
            {
                return HttpNotFound();
            }
            return View(Role);
        }

        // POST: Roles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Role Role = db.Role.Find(id);
            db.Role.Remove(Role);
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
