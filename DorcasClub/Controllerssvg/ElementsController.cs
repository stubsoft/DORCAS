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
            return View(db.Elements.ToList());

        }

        public ActionResult listeelements()
        {
            var abc = from obj in db.Elements
                      select new
                      {
                          idElement = obj.idElement,
                          nameElement = obj.nameElement,
                          Energie = obj.Energie,
                          Glucide = obj.Glucide,
                          Proteine = obj.Proteine
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
            Elements elements = db.Elements.Find(id);
            if (elements == null)
            {
                return HttpNotFound();
            }
            return View(elements);


        }

        // GET: Dorcas_Elements/Create
        public ActionResult Create()
        {
            Elements elements = new Elements();
            elements.weightUnity = 100;
            return View(elements);


        }

        // POST: Dorcas_Elements/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nameElement,Energie,Kj,Glucide,Proteine,Lipide,price,description,weightUnity")] Elements elements)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /*Insert image*/
                    HttpPostedFileBase file = Request.Files["imageElement"];
                    SecurityServices securityService = new SecurityServices();
                    elements.imageElement = securityService.ConvertToBytes(file);

                    /*EndInsert image*/


                    /*Image/root/rename*/

                    Guid guid = Guid.NewGuid();

                    string newfileName = guid.ToString();

                    string fileextention = Path.GetExtension(file.FileName);

                    string fileName = newfileName + fileextention;

                    string uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));

                    file.SaveAs(uploadpath);

                    /*End/Image/root/rename*/

                    elements.FileName = fileName;
                    elements.imagephysique = file.FileName;
                    elements.insertionDate = DateTime.Now;
                    db.Elements.Add(elements);
                    db.SaveChanges();
                    TempData["SuccessMesage"] = "Elément" + elements.idElement + " est enregistrée! ";
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
                    return View(elements);
                }


                return View(elements);
            }

            return View(elements);
        }

        // GET: Dorcas_Elements/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elements elements = db.Elements.Find(id);
            if (elements == null)
            {
                return HttpNotFound();
            }

            return View(elements);
        }

        // POST: Dorcas_Elements/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idElement,nameElement,Energie,Kj,Glucide,Proteine,Lipide,insertionDate,price,description,weightUnity,imageElement,FileName,imagephysique")] Elements elements)
        {
            SecurityServices securityService = new SecurityServices();
            if (ModelState.IsValid)
            {
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["EditimageElement"];
                if (file.FileName != "")
                { 
                    elements.imageElement = securityService.ConvertToBytes(file);
                }
                //*EndInsert image*/

                /*Update file*/
               
                if (file.FileName != "")
                {
                    //Delete Old file from the file system
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles"), elements.FileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    //ADD NEW file from the file system

                    string uploadpath = "";

                    Guid guid = Guid.NewGuid();

                    string newfileName = guid.ToString();

                    string fileextention = Path.GetExtension(file.FileName);

                    string fileName = newfileName + fileextention;


                    uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));
                    elements.FileName = fileName;
                    elements.imagephysique = file.FileName;

                    file.SaveAs(uploadpath);
                }
                /*Insert Update file*/

                db.Entry(elements).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(elements);
        }

        // GET: Dorcas_Elements/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elements elements = db.Elements.Find(id);

            
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
            Elements elements = db.Elements.Find(id);
            db.Elements.Remove(elements);
            db.SaveChanges();
            //Delete Old file from the file system
            if (elements.FileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/UploadedFiles"), elements.FileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return RedirectToAction("Index");
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
            var q = from temp in db.Elements where temp.idElement == Id select temp.imageElement;
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