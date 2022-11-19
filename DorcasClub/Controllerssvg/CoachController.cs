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
    public class CoachController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Coach
        public ActionResult Index()
        {
            return View(db.Coach.ToList());
        }

        public ActionResult listeentreneurs()
        {
            var abc = from obj in db.Coach
                      select new
                      {
                          idCoach = obj.idCoach,
                          FirstName = obj.FirstName,
                          LastName = obj.LastName,
                          DateBirdh = obj.DateBirdh.ToString(),


                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Coach/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coach.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // GET: Dorcas_Coach/Create
        public ActionResult Create()
        {
            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            return View();
        }

        // POST: Dorcas_Coach/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,DateBirdh,mobile,phone,Email,Adresse,start_Date")] Coach coach, IEnumerable<int> SelectedMachine)
        {
            if (ModelState.IsValid)
            {
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["imageElement"];
                SecurityServices securityService = new SecurityServices();
                coach.imageelement = securityService.ConvertToBytes(file);

                /*EndInsert image*/


                /*Image/root/rename*/

                Guid guid = Guid.NewGuid();

                string newfileName = guid.ToString();

                string fileextention = Path.GetExtension(file.FileName);

                string fileName = newfileName + fileextention;

                string uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));

                file.SaveAs(uploadpath);

                /*End/Image/root/rename*/

                coach.FileName = fileName;
                coach.imagephysique = file.FileName;

                db.Coach.Add(coach);
                db.SaveChanges();

                /*Add list Coach to base*/

                /*Add list Coach to base*/
                Machin_Coach machin_Coach = new Machin_Coach();
                foreach (var item in SelectedMachine)
                {
                    machin_Coach = new Machin_Coach();
                    machin_Coach.idMaterials = item;
                    machin_Coach.idCoach = coach.idCoach;
                    db.Machin_Coach.Add(machin_Coach);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(coach);
        }

        // GET: Dorcas_Coach/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coach.Find(id);

            //DropdownList
            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            var model = (from c in db.Machin_Coach
                         where c.idCoach == id
                         select c.idMaterials).ToList();
            //zied
            coach.SelectedMachine = ""; //model

            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // POST: Dorcas_Coach/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCoach,FirstName,LastName,DateBirdh,mobile,phone,Email,Adresse,login,password,start_Date,imageelement,FileName,imagephysique")] Coach coach, IEnumerable<int> SelectedMachine)
        {
            if (ModelState.IsValid)
            {
                SecurityServices securityService = new SecurityServices();
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["EditimageElement"];
                if (file.FileName != "")
                {
                    coach.imageelement = securityService.ConvertToBytes(file);
                }
                //*EndInsert image*/

                /*Update file*/

                if (file.FileName != "")
                {
                    //Delete Old file from the file system
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles"), coach.FileName);
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
                    coach.FileName = fileName;
                    coach.imagephysique = file.FileName;

                    file.SaveAs(uploadpath);
                }
                /*End Insert Update file*/

                /*Update selected Machine to base*/
                Machin_Coach machin_Coach = new Machin_Coach();

                var model = (from c in db.Machin_Coach
                             where c.idCoach == coach.idCoach
                             select c.idMaterials).ToList();

                foreach (var item in model)
                {
                    machin_Coach = db.Machin_Coach.Find(coach.idCoach,item);
                    if (machin_Coach.idCoach == coach.idCoach && machin_Coach.idMaterials == item)
                        db.Machin_Coach.Remove(machin_Coach);
                    db.SaveChanges();
                }

                foreach (var item in SelectedMachine)
                {
                    machin_Coach = new Machin_Coach(); ;
                    machin_Coach.idCoach = coach.idCoach;
                    machin_Coach.idMaterials = item;
                    db.Machin_Coach.Add(machin_Coach);
                    db.SaveChanges();
                }

                db.Entry(coach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coach);
        }

        // GET: Dorcas_Coach/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coach coach = db.Coach.Find(id);
            if (coach == null)
            {
                return HttpNotFound();
            }
            return View(coach);
        }

        // POST: Dorcas_Coach/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coach coach = db.Coach.Find(id);
            Machin_Coach machin_Coach = new Machin_Coach();

            var model = (from c in db.Machin_Coach
                         where c.idCoach == coach.idCoach
                         select c.idMaterials).ToList();

            foreach (var item in model)
            {
                machin_Coach = db.Machin_Coach.Find(coach.idCoach, item);
                if (machin_Coach.idCoach == coach.idCoach && machin_Coach.idMaterials == item)
                    db.Machin_Coach.Remove(machin_Coach);
                db.SaveChanges();
            }
            db.Coach.Remove(coach);
            db.SaveChanges();
            //Delete Old file from the file system
            if (coach.FileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/UploadedFiles"), coach.FileName);
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
            var q = from temp in db.Coach where temp.idCoach == Id select temp.imageelement;
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
