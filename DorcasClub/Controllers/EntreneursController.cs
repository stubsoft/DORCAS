using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DorcasClub;
using DorcasClub.Services.Business;

namespace DorcasClub.Controllers
{
    public class EntreneursController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Entreneur
        public ActionResult Index()
        {
            return View(db.Entreneur.ToList());
        }
        public ActionResult listeentreneurs()
        {
            var model = from obj in db.Entreneur
                      select new
                      {
                          IdEntreneur = obj.IdEntreneur,
                          Nom = obj.Nom,
                          Prenom = obj.Prenom,
                      };
            var jsonResult = Json(model.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Entreneur/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entreneur Entreneur = db.Entreneur.Find(id);
            if (Entreneur == null)
            {
                return HttpNotFound();
            }
            return View(Entreneur);
        }

        // GET: Dorcas_Entreneur/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dorcas_Entreneur/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Prenom,CIN,Mobile1,Mobile2,Email,Adresse,FilePath")] Entreneur entreneur)
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
                entreneur.FilePath = FilePathDataBase;

                db.Entreneur.Add(entreneur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entreneur);
        }

        // GET: Dorcas_Entreneur/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entreneur entreneur = db.Entreneur.Find(id);

            if (entreneur == null)
            {
                return HttpNotFound();
            }
            return View(entreneur);
        }

        // POST: Dorcas_Entreneur/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEntreneur,Nom,Prenom,CIN,Mobile1,Mobile2,Email,Adresse,FilePath")] Entreneur entreneur)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileImage = Request.Files["FileImage"];
                if (FileImage.FileName != "")
                {
                    if (System.IO.File.Exists(Server.MapPath(entreneur.FilePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(entreneur.FilePath));
                    }
                }
                
                Guid guid = Guid.NewGuid();
                string NewFileName = guid.ToString();
                string FileExtension = Path.GetExtension(FileImage.FileName);
                string FileName = NewFileName + DateTime.Now.ToString("yymmssff") + FileExtension;
                string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(FileName));
                FileImage.SaveAs(FilePath);
                String FilePathDataBase = "~/UploadedFiles/" + FileName;
                if(FileExtension!="")
                   entreneur.FilePath = FilePathDataBase;

                db.Entry(entreneur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entreneur);
        }

        // GET: Dorcas_Entreneur/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entreneur entreneur = db.Entreneur.Find(id);
            if (entreneur == null)
            {
                return HttpNotFound();
            }
            return View(entreneur);
        }

        // POST: Dorcas_Entreneur/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entreneur entreneur = db.Entreneur.Find(id);

            db.Entreneur.Remove(entreneur);
            db.SaveChanges();
            //Delete Old file from the file system
            if (entreneur.FilePath != null)
            {
                entreneur.FilePath = entreneur.FilePath;
                if (System.IO.File.Exists(Server.MapPath(entreneur.FilePath)))
                {
                    System.IO.File.Delete(Server.MapPath(entreneur.FilePath));
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
        //    var q = from temp in db.Entreneur where temp.IdEntreneur == Id select temp.imageelement;
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
