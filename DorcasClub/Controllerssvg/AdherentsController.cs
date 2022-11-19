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
            var abc = from obj in db.Adherent
                      select new
                      {
                          idAdherent = obj.idAdherent,
                          Nom = obj.Nom,
                          Prenom = obj.Prenom
                      };
            var jsonResult = Json(abc.ToList()
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
            var list = db.Ordonnance.Where(c => c.idAdherent == id).ToList();
            ViewData["ListOrdonnance"] = list;

            return View(adherent);
        }

        // GET: Adherents/Create
        public ActionResult Create()
        {
            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - dd");
            ViewData["d2"] = DateTime.Now.ToString("yyyy - MM - dd");
            return View();
        }

        // POST: Adherents/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nom,Prenom,CIN,Email1,Email2,Tel1,Tel2,Adresse,Ville,Pays,Facebook,Instagram,Genre,Poids,Taille,DateNaissance,Type,Note,DateCreation")] Adherent adherent)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["Image"];
                SecurityServices securityService = new SecurityServices();
                adherent.Image = securityService.ConvertToBytes(file);

                /*EndInsert image*/


                /*Image/root/rename*/

                Guid guid = Guid.NewGuid();

                string newfileName = guid.ToString();

                string fileextention = Path.GetExtension(file.FileName);

                string fileName = newfileName + fileextention;

                string uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));

                file.SaveAs(uploadpath);

                /*End/Image/root/rename*/

                adherent.FileName = fileName;
                adherent.imagephysique = file.FileName;
                adherent.DateCreation = DateTime.Now;
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
        public ActionResult Edit([Bind(Include = "idAdherent,Nom,Prenom,CIN,Email1,Email2,Tel1,Tel2,Adresse,Ville,Pays,Facebook,Instagram,Genre,Poids,Taille,DateNaissance,Type,Note,DateCreation,Image,FileName,imagephysique")] Adherent adherent)
        {
            if (ModelState.IsValid)
            {
                SecurityServices securityService = new SecurityServices();
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["EditimageElement"];
                if (file.FileName != "")
                {
                    adherent.Image = securityService.ConvertToBytes(file);
                }
                //*EndInsert image*/

                /*Update file*/

                if (file.FileName != "")
                {
                    //Delete Old file from the file system
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles"), adherent.FileName);
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
                    adherent.FileName = fileName;
                    adherent.imagephysique = file.FileName;

                    file.SaveAs(uploadpath);
                }

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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adherent adherent = db.Adherent.Find(id);
            db.Adherent.Remove(adherent);
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
