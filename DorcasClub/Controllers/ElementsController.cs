using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DorcasClub;
using System.Collections.Generic;
using System.Data;
using System.Web;
using DorcasClub.Services.Business;
using System.IO;

namespace DorcasClub.Controllers
{
    public class ElementsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Elements
        public ActionResult Index()
        {
            return View(db.Element.ToList());

        }
        public ActionResult listeelements()
        {
            var abc = from obj in db.Element
                      select new
                      {
                          IdElement = obj.IdElement,
                          Nom = obj.Nom,
                          Energie = obj.Energie,
                          kj = obj.Kj,
                          Glucide = obj.Glucide,
                          Proteine = obj.Proteine,
                          Prix = obj.Prix,
                          Description = obj.Description,
                          Poids = obj.Poids,
                          Lipide = obj.Lipide,
                          FilePath = obj.FilePath,
                          
                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        // GET: Dorcas_Elements/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Element element = db.Element.Find(id);
            if (element == null)
            {
                return HttpNotFound();
            }
            return View(element);


        }

        // GET: Dorcas_Elements/Create
        public ActionResult Create()
        {
            Element element = new Element();
            element.Poids = 100;
            return View(element);


        }

        // POST: Dorcas_Elements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Energie,Kj,Glucide,Proteine,Lipide,DateCreation,Prix,Description,Poids,FilePath")] Element element)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase FileImage = Request.Files["FileImage"];
                    Guid guid = Guid.NewGuid();
                    string NewFileName = guid.ToString();
                    string FileExtension = Path.GetExtension(FileImage.FileName);
                    string FileName = NewFileName + DateTime.Now.ToString("yymmssff") + FileExtension;
                    string FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(FileName));
                    FileImage.SaveAs(FilePath);
                    String FilePathDataBase = "~/UploadedFiles/" + FileName;
                    element.FilePath = FilePathDataBase;

                    db.Element.Add(element);
                    db.SaveChanges();
                    TempData["SuccessMesage"] = "Elément" + element.IdElement + " est enregistrée! ";
                    return RedirectToAction("Index");
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }

            }
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                   foreach (var modelError in modelState.Errors)
                        {
                                modelErrors.Add(modelError.ErrorMessage);
                         
                            modelErrors.Add(modelError.ErrorMessage);
                        }
                    return View(element);
                }


                return View(element);
            }

            return View(element);
        }

        // GET: Dorcas_Elements/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Element element = db.Element.Find(id);
            if (element == null)
            {
                return HttpNotFound();
            }

            return View(element);
        }

        // POST: Dorcas_Elements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdElement,Nom,Energie,Kj,Glucide,Proteine,Lipide,DateCreation,Prix,Description,Poids,FilePath")] Element element)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileImage = Request.Files["FileImage"];
                if (FileImage.FileName != "")
                {
                    if (System.IO.File.Exists(Server.MapPath(element.FilePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(element.FilePath));
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
                    element.FilePath = FilePathDataBase;

                db.Entry(element).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(element);
        }

        // GET: Dorcas_Elements/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Element elements = db.Element.Find(id);

            
            if (elements == null)
            {
                return HttpNotFound();
            }
            return View(elements);
        }

        // POST: Dorcas_Elements/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Element element = db.Element.Find(id);
            db.Element.Remove(element);
            db.SaveChanges();

            //Delete Old file from the file system
            if (element.FilePath != null)
            {
                element.FilePath = element.FilePath;
                if (System.IO.File.Exists(Server.MapPath(element.FilePath)))
                {
                    System.IO.File.Delete(Server.MapPath(element.FilePath));
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
        //    var q = from temp in db.Element where temp.IdElement == Id select temp.FilePath;

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