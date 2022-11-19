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
    public class MaterialsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Materials
        public ActionResult Index()
        {

            Materials dorcas_Materials = new Materials();

            //DropdownList


            var ListMaterial = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterial"] = ListMaterial;



            var ListCoach = new SelectList(db.Coach.ToList(), "idCoach", "nameCoach");
            ViewData["ListCoach"] = ListCoach;

            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;


            /* ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");

             ViewData["d11"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
             ViewData["d22"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");*/


            return View(dorcas_Materials);
        }


        public ActionResult listemateriels()
        {
            var abc = from obj in db.Materials
                      select new
                      {
                          idMaterials = obj.idMaterials,
                          nameMaterials = obj.nameMaterials,
                          Availability = obj.Availability.ToString(),
                          MaxOccupation = obj.MaxOccupation,
                         

                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Materials/Details/5
        public ActionResult Details(int id)
        {


            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materials materials = db.Materials.Find(id);
            if (materials == null)
            {
                return HttpNotFound();
            }

            //DropdownList
            var ListCoach = new SelectList(db.Coach.ToList(), "idCoach", "nameCoach");
            ViewData["ListCoach"] = ListCoach;

            var Listcategorie = new SelectList(db.Categorie.ToList(), "IdCategorie", "Libelle");
            ViewData["Listcategorie"] = Listcategorie;

            ViewData["d1"] = DateTime.Parse(materials.Availability.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            //   ViewData["d1"] = DateTime.Parse(materials.Availability.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

            return View(materials);

        }

        // GET: Dorcas_Materials/Create
        public ActionResult Create()
        {
            Materials dorcas_Materials = new Materials();

            //DropdownList
            List<Coach> ListCoach = db.Coach.ToList();
            ViewData["ListCoach"] = ListCoach;

            var Listcategorie = new SelectList(db.Categorie.ToList(), "IdCategorie", "Libelle");
            ViewData["Listcategorie"] = Listcategorie;

            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");

            return View(dorcas_Materials);

        }

        // POST: Dorcas_Materials/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "nameMaterials,Availability,Coach_idCoach,MaxOccupation,IdCategorie")] Materials materials, IEnumerable<int> SelectedCoach)
        {
            if (ModelState.IsValid)
            {
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["imageElement"];
                SecurityServices securityService = new SecurityServices();
                materials.imageElement = securityService.ConvertToBytes(file);

                /*EndInsert image*/


                /*Image/root/rename*/

                Guid guid = Guid.NewGuid();

                string newfileName = guid.ToString();

                string fileextention = Path.GetExtension(file.FileName);

                string fileName = newfileName + fileextention;

                string uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));

                file.SaveAs(uploadpath);

                /*End/Image/root/rename*/

                materials.FileName = fileName;
                materials.imagephysique = file.FileName;
                db.Materials.Add(materials);
                db.SaveChanges();

                /*Add list Coach to base*/
                Machin_Coach machin_Coach = new Machin_Coach();
                foreach (var item in SelectedCoach)
                {
                    machin_Coach = new Machin_Coach();
                    machin_Coach.idCoach = item;
                    machin_Coach.idMaterials = materials.idMaterials;
                    db.Machin_Coach.Add(machin_Coach);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.Coach_idCoach = new SelectList(db.Coach, "idCoach", "nameCoach");
            return View(materials);
        }

        // GET: Dorcas_Materials/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materials materials = db.Materials.Find(id);
            if (materials == null)
            {
                return HttpNotFound();
            }
            //DropdownList
            List<Coach> ListCoach = db.Coach.ToList();
            ViewData["ListCoach"] = ListCoach;

            var Listcategorie = new SelectList(db.Categorie.ToList(), "IdCategorie", "Libelle");
            ViewData["Listcategorie"] = Listcategorie;


            var model = (from c in db.Machin_Coach
                         where c.idMaterials == id
                         select c.idCoach).ToList();
            //zied 
            materials.SelectedCoach = ""; //model

            ViewData["d1"] = DateTime.Parse(materials.Availability.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            return View(materials);
        }

        // POST: Dorcas_Materials/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMaterials,nameMaterials,Availability,Coach_idCoach,MaxOccupation,imageElement,FileName,imagephysique,IdCategorie")] Materials materials, IEnumerable<int> SelectedCoach)
        {
            if (ModelState.IsValid)
            {
                SecurityServices securityService = new SecurityServices();
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["EditimageElement"];
                if (file.FileName != "")
                {
                    materials.imageElement = securityService.ConvertToBytes(file);
                }
                //*EndInsert image*/

                /*Update file*/

                if (file.FileName != "")
                {
                    //Delete Old file from the file system
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles"), materials.FileName);
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
                    materials.FileName = fileName;
                    materials.imagephysique = file.FileName;

                    file.SaveAs(uploadpath);
                }
                /*Insert Update file*/

                /*Update selected Machine to base*/
                Machin_Coach machin_Coach = new Machin_Coach();

                var model = (from c in db.Machin_Coach
                             where c.idMaterials == materials.idMaterials
                             select c.idCoach).ToList();

                foreach (var item in model)
                {
                    machin_Coach = db.Machin_Coach.Find(item, materials.idMaterials);
                    if (machin_Coach.idMaterials == materials.idMaterials && machin_Coach.idCoach == item)
                        db.Machin_Coach.Remove(machin_Coach);
                    db.SaveChanges();
                }

                foreach (var item in SelectedCoach)
                {
                    machin_Coach = new Machin_Coach(); ;
                    machin_Coach.idCoach = item;
                    machin_Coach.idMaterials = materials.idMaterials;
                    db.Machin_Coach.Add(machin_Coach);
                    db.SaveChanges();
                }
                db.Entry(materials).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Coach_idCoach = new SelectList(db.Coach, "idCoach", "nameCoach");
            return View(materials);
        }
        public ActionResult Search()
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "idSession", "etat");
            Materials dorcas_Materials = new Materials();

            //DropdownList


            var ListMaterial = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterial"] = ListMaterial;



            var ListCoach = new SelectList(db.Coach.ToList(), "idCoach", "nameCoach");
            ViewData["ListCoach"] = ListCoach;

            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            ViewData["d11"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            ViewData["d22"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");

            return View(dorcas_Materials);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchMaterials()
        {
            Materials dorcas_Materials = new Materials();

            var d1 = DateTime.Parse(Request["d1"]);

            var nameMaterials = Request["nameMaterials"];
            var Availability = Request["Availability"];


            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["nameMaterials"] = nameMaterials;
            ViewData["Availability"] = Availability;

            //ViewData["idMaterials"] = idMaterials;
            var model = from r in db.Materials

                        where r.Availability >= d1 || r.nameMaterials == nameMaterials
                        select r;
            //DropdownList


            var ListMaterial = new SelectList(db.Materials.ToList(), "idMaterials", "nameMaterials");
            ViewData["ListMaterial"] = ListMaterial;

            List<Materials> ListMaterials = db.Materials.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            return View("Search", dorcas_Materials);
        }
        // GET: Dorcas_Materials/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materials materials = db.Materials.Find(id);
            if (materials == null)
            {
                return HttpNotFound();
            }
            return View(materials);
        }

        // POST: Dorcas_Materials/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Materials materials = db.Materials.Find(id);
            Machin_Coach machin_Coach = new Machin_Coach();
            var model = (from c in db.Machin_Coach
                         where c.idMaterials == materials.idMaterials
                         select c.idCoach).ToList();

            foreach (var item in model)
            {
                machin_Coach = db.Machin_Coach.Find(item, materials.idMaterials);
                if (machin_Coach.idMaterials == materials.idMaterials && machin_Coach.idCoach == item)
                    db.Machin_Coach.Remove(machin_Coach);
                db.SaveChanges();
            }

            db.Materials.Remove(materials);
            db.SaveChanges();
            if (materials.FileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/UploadedFiles"), materials.FileName);
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
            var q = from temp in db.Materials where temp.idMaterials == Id select temp.imageElement;
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


        public ActionResult PlanMachine()
        {
            //DropdownList
            var ListMachine = db.Materials.ToList();
            ViewData["ListMachine"] = ListMachine;

            var Day = DateTime.Parse(DateTime.Now.ToString("yyyy'-'MM'-'dd"));
            var ListSeances1 = db.Session.Where(r => r.DateSESSION == Day).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances1"] = ListSeances1;
            
            ViewData["Day"] = DateTime.Now.ToString("yyyy'-'MM'-'dd");

            var Day1 = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd"));
            var ListSeances2 = db.Session.Where(r => r.DateSESSION == Day1).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances2"] = ListSeances2;
            ViewData["Day1"] = Day1.ToString("yyyy'-'MM'-'dd");

            var Day2 = DateTime.Parse(DateTime.Now.AddDays(2).ToString("yyyy'-'MM'-'dd"));
            var ListSeances3 = db.Session.Where(r => r.DateSESSION == Day2).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances3"] = ListSeances3;
            ViewData["Day2"] = Day2.ToString("yyyy'-'MM'-'dd");

            var Day3 = DateTime.Parse(DateTime.Now.AddDays(3).ToString("yyyy'-'MM'-'dd"));
            var ListSeances4 = db.Session.Where(r => r.DateSESSION == Day3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances4"] = ListSeances4;
            ViewData["Day3"] = Day3.ToString("yyyy'-'MM'-'dd");

            var Day4 = DateTime.Parse(DateTime.Now.AddDays(4).ToString("yyyy'-'MM'-'dd"));
            var ListSeances5 = db.Session.Where(r => r.DateSESSION == Day4).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances5"] = ListSeances5;
            ViewData["Day4"] = Day4.ToString("yyyy'-'MM'-'dd");

            var Day5 = DateTime.Parse(DateTime.Now.AddDays(5).ToString("yyyy'-'MM'-'dd"));
            var ListSeances6 = db.Session.Where(r => r.DateSESSION == Day5).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances6"] = ListSeances6;
            ViewData["Day5"] = Day5.ToString("yyyy'-'MM'-'dd");

            var Day6 = DateTime.Parse(DateTime.Now.AddDays(6).ToString("yyyy'-'MM'-'dd"));
            var ListSeances7 = db.Session.Where(r => r.DateSESSION == Day6).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances7"] = ListSeances7;
            ViewData["Day6"] = Day6.ToString("yyyy'-'MM'-'dd");

            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["d2"] = DateTime.Now.AddDays(6).ToString("yyyy'-'MM'-'dd");
            ViewData["Machine"] = "Machine";
            TempData["Alert"] = "";
            return View();

        }
        public ActionResult PlanMachineChercher()
        {
            //DropdownList
            var ListMachine = db.Materials.ToList();
            ViewData["ListMachine"] = ListMachine;

            var d1 = DateTime.Parse(Request["Datedebut"]);
            var d2 = DateTime.Parse(Request["DateFin"]);
            var d3 = 0;
            if (Request["CodeMachine"].ToString() == "Machine")
            {
                TempData["Alert"] = "Selectionner une machine!";
            }
            else
            {
                d3 = int.Parse(Request["CodeMachine"].ToString());
                ViewData["Machine"] = db.Materials.Find(d3).nameMaterials;
                ViewData["idMaterials"] = d3;
                TempData["Alert"] = "";
            }
            ViewData["Day"] = d1.ToString("yyyy'-'MM'-'dd");

            var ListSeances1 = db.Session.Where(r => r.DateSESSION == d1 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances1"] = ListSeances1;

            var Day1 = d1.AddDays(1);
            var ListSeances2 = db.Session.Where(r => r.DateSESSION == Day1 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances2"] = ListSeances2;
            ViewData["Day1"] = Day1.ToString("yyyy'-'MM'-'dd");

            var Day2 = d1.AddDays(2);
            var ListSeances3 = db.Session.Where(r => r.DateSESSION == Day2 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances3"] = ListSeances3;
            ViewData["Day2"] = Day2.ToString("yyyy'-'MM'-'dd");

            var Day3 = d1.AddDays(3);
            var ListSeances4 = db.Session.Where(r => r.DateSESSION == Day3 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances4"] = ListSeances4;
            ViewData["Day3"] = Day3.ToString("yyyy'-'MM'-'dd");

            var Day4 = d1.AddDays(4);
            var ListSeances5 = db.Session.Where(r => r.DateSESSION == Day4 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances5"] = ListSeances5;
            ViewData["Day4"] = Day4.ToString("yyyy'-'MM'-'dd");

            var Day5 = d1.AddDays(5);
            var ListSeances6 = db.Session.Where(r => r.DateSESSION == Day5 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances6"] = ListSeances6;
            ViewData["Day5"] = Day5.ToString("yyyy'-'MM'-'dd");

            var Day6 = d1.AddDays(6);
            var ListSeances7 = db.Session.Where(r => r.DateSESSION == Day6 && r.Materials_idMaterials == d3).OrderBy(r => r.startHeure).ToList();
            ViewData["ListSeances7"] = ListSeances7;
            ViewData["Day6"] = Day6.ToString("yyyy'-'MM'-'dd");

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd");
            ViewData["d2"] = d2.AddDays(6).ToString("yyyy'-'MM'-'dd");
            
            return View("PlanMachine");

        }
    }
}
