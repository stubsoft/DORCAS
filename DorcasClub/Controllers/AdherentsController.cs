using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub.Services.Business;

namespace DorcasClub.Controllers
{
    public class AdherentsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Adherents
        public ActionResult Index()
        {  
            return View(db.Adherent.ToList());
        }

        public ActionResult listeAdherents()
        {
            var model = from obj in db.Adherent
                      select new
                      {
                          IdAdherent = obj.IdAdherent,
                          Nom = obj.Nom,
                          Prenom = obj.Prenom,
                          Mobile1 = obj.Mobile1,
                          DateNaissance = obj.DateNaissance.Value.Day.ToString() + "-" + obj.DateNaissance.Value.Month.ToString() + "-" + obj.DateNaissance.Value.Year.ToString(),
                          Poids = obj.Poids,
                          Taille = obj.Taille,
                      };
            var jsonResult = Json(model.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Adherents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Adherent adherent = db.Adherent.Find(id);
            if (adherent == null)
            {
                return HttpNotFound();
            }
            ViewData["d1"] = DateTime.Parse(adherent.DateNaissance.ToString()).ToString("yyyy'-'MM'-'dd");

            return View(adherent);
        }

        // GET: Adherents/Create
        public ActionResult Create()
        {
            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - dd");
            Adherent adherent = new Adherent();
            return View(adherent);
        }

        // POST: Adherents/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Prenom,Poids,Taille,CIN,DateNaissance,Mobile1,Mobile2,Email,Adresse,Facebook,Instagram,DateCreation,Observation,FilePath")] Adherent adherent)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileImage = Request.Files["FileImage"];
                Guid guid = Guid.NewGuid();
                string NewFileName = guid.ToString();
                string FileExtension = Path.GetExtension(FileImage.FileName);
                string FileName = NewFileName + DateTime.Now.ToString("yymmssff") + FileExtension;
                string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(FileName));
                FileImage.SaveAs(FilePath);
                String FilePathDataBase = "~/UploadedFiles/" + FileName;
                adherent.FilePath = FilePathDataBase;

                db.Adherent.Add(adherent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adherent);
        }

        // GET: Adherents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adherent adherent = db.Adherent.Find(id);
            ViewData["d1"] = DateTime.Parse(adherent.DateNaissance.ToString()).ToString("yyyy'-'MM'-'dd");
            if (adherent == null)
            {
                return HttpNotFound();
            }
            return View(adherent);
        }

        // POST: Adherents/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAdherent,Nom, Prenom, Poids, Taille, CIN, DateNaissance, Mobile1, Mobile2, Email, Adresse, Facebook, Instagram, DateCreation, Observation, FilePath")] Adherent adherent)
        {

            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileImage = Request.Files["FileImage"];
                if (FileImage.FileName != "")
                {
                    if (System.IO.File.Exists(Server.MapPath(adherent.FilePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(adherent.FilePath));
                    }
                }
                Guid guid = Guid.NewGuid();
                string NewFileName = guid.ToString();
                string FileExtension = Path.GetExtension(FileImage.FileName);
                string FileName = NewFileName + DateTime.Now.ToString("yymmssff") + FileExtension;
                string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(FileName));
                FileImage.SaveAs(FilePath);
                String FilePathDataBase = "~/UploadedFiles/" + FileName;
                if (FileExtension != "")
                    adherent.FilePath = FilePathDataBase;

                db.Entry(adherent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adherent);
        }

        // GET: Adherents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adherent adherent = db.Adherent.Find(id);
            if (adherent == null)
            {
                return HttpNotFound();
            }
            return View(adherent);
        }

        // POST: Adherents/Delete/5
        //[HttpPost, ActionName("Delete")]
       //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adherent adherent = db.Adherent.Find(id);
            db.Adherent.Remove(adherent);
            db.SaveChanges();

            //Delete Old file from the file system
            if (adherent.FilePath != null)
            {
                adherent.FilePath = adherent.FilePath;
                if (System.IO.File.Exists(Server.MapPath(adherent.FilePath)))
                {
                    System.IO.File.Delete(Server.MapPath(adherent.FilePath));
                }
            }

            return RedirectToAction("Index");
        }

        //public ActionResult RetrieveImage(int id)
        //{
        //    byte[] cover = GetImageFromDataBase(id);
        //    if (cover != null)
        //    {
        //        return File(cover, "image/jpg");
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public byte[] GetImageFromDataBase(int Id)
        //{
        //    var q = from temp in db.Adherent where temp.idAdherent == Id select temp.imageElement;
        //    byte[] cover = q.First();
        //    return cover;
        //}

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
