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
    public class MachinesController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Machines
        public ActionResult Index()
        {
            return View(db.Machine.ToList());
        }

        public ActionResult listeMachines()
        {
            var model = from obj in db.Machine
                        select new
                        {
                            IdMachine = obj.IdMachine,
                            Designation = obj.Designation,
                            MaxOccupation = obj.MaxOccupation,
                        };
            var jsonResult = Json(model.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }

        // GET: Dorcas_Machines/Details/5
        public ActionResult Details(int id)
        {

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = db.Machine.Find(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            var Listcategorie = new SelectList(db.Categorie.ToList(), "IdCategorie", "Libelle");
            ViewData["Listcategorie"] = Listcategorie;
            return View(machine);

        }

        // GET: Dorcas_Machines/Create
        public ActionResult Create()
        {
            var Listcategorie = new SelectList(db.Categorie.ToList(), "IdCategorie", "Libelle");
            ViewData["Listcategorie"] = Listcategorie;
            return View();

        }

        // POST: Dorcas_Machines/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Designation,MaxOccupation,FilePath,IdCategorie")] Machine machine)
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
                machine.FilePath = FilePathDataBase;

                db.Machine.Add(machine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(machine);
        }

        // GET: Dorcas_Machines/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = db.Machine.Find(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            var Listcategorie = new SelectList(db.Categorie.ToList(), "IdCategorie", "Libelle");
            ViewData["Listcategorie"] = Listcategorie;

            return View(machine);
        }

        // POST: Dorcas_Machines/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMachine,Designation,MaxOccupation,FilePath,IdCategorie")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase FileImage = Request.Files["FileImage"];
                if (FileImage.FileName != "")
                {
                    if (System.IO.File.Exists(Server.MapPath(machine.FilePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(machine.FilePath));
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
                    machine.FilePath = FilePathDataBase;


                db.Entry(machine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(machine);
        }

        // GET: Dorcas_Machines/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Machine machine = db.Machine.Find(id);
            if (machine == null)
            {
                return HttpNotFound();
            }
            return View(machine);
        }

        // POST: Dorcas_Machines/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Machine machine = db.Machine.Find(id);

            db.Machine.Remove(machine);
            db.SaveChanges();
            //Delete Old file from the file system
            if (machine.FilePath != null)
            {
                machine.FilePath = machine.FilePath;
                if (System.IO.File.Exists(Server.MapPath(machine.FilePath)))
                {
                    System.IO.File.Delete(Server.MapPath(machine.FilePath));
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
        //    var q = from temp in db.Machine where temp.IdMachine == Id select temp.imageElement;
        //    byte[] cover = q.First();
        //    return cover;
        //}


        public ActionResult Search()
        {
            ViewBag.Session_idSession = new SelectList(db.Session, "IdSession", "Etat");
            Machine machine = new Machine();

            //DropdownList


            var ListMaterial = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterial"] = ListMaterial;


            List<Machine> ListMaterials = db.Machine.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            ViewData["d11"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            ViewData["d22"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");

            return View(machine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchMaterials()
        {
            Machine machine = new Machine();

            var d1 = DateTime.Parse(Request["d1"]);

            var nameMaterials = Request["Designation"];
            var Availability = Request["Disponibilite"];


            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["Designation"] = nameMaterials;
            ViewData["Disponibilite"] = Availability;




            var ListMaterial = new SelectList(db.Machine.ToList(), "IdMachine", "Designation");
            ViewData["ListMaterial"] = ListMaterial;

            List<Machine> ListMaterials = db.Machine.ToList();
            ViewData["ListMaterials"] = ListMaterials;

            return View("Search", machine);
        }
        public ActionResult PlanMachine()
        {
            //DropdownList
            var ListMachine = db.Machine.ToList();
            ViewData["ListMachine"] = ListMachine;

            var Day = DateTime.Parse(DateTime.Now.ToString("yyyy'-'MM'-'dd"));
            var ListSeances1 = db.Session.Where(r => r.DateSession == Day).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances1"] = ListSeances1;

            ViewData["Day"] = DateTime.Now.ToString("yyyy'-'MM'-'dd");

            var Day1 = DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy'-'MM'-'dd"));
            var ListSeances2 = db.Session.Where(r => r.DateSession == Day1).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances2"] = ListSeances2;
            ViewData["Day1"] = Day1.ToString("yyyy'-'MM'-'dd");

            var Day2 = DateTime.Parse(DateTime.Now.AddDays(2).ToString("yyyy'-'MM'-'dd"));
            var ListSeances3 = db.Session.Where(r => r.DateSession == Day2).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances3"] = ListSeances3;
            ViewData["Day2"] = Day2.ToString("yyyy'-'MM'-'dd");

            var Day3 = DateTime.Parse(DateTime.Now.AddDays(3).ToString("yyyy'-'MM'-'dd"));
            var ListSeances4 = db.Session.Where(r => r.DateSession == Day3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances4"] = ListSeances4;
            ViewData["Day3"] = Day3.ToString("yyyy'-'MM'-'dd");

            var Day4 = DateTime.Parse(DateTime.Now.AddDays(4).ToString("yyyy'-'MM'-'dd"));
            var ListSeances5 = db.Session.Where(r => r.DateSession == Day4).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances5"] = ListSeances5;
            ViewData["Day4"] = Day4.ToString("yyyy'-'MM'-'dd");

            var Day5 = DateTime.Parse(DateTime.Now.AddDays(5).ToString("yyyy'-'MM'-'dd"));
            var ListSeances6 = db.Session.Where(r => r.DateSession == Day5).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances6"] = ListSeances6;
            ViewData["Day5"] = Day5.ToString("yyyy'-'MM'-'dd");

            var Day6 = DateTime.Parse(DateTime.Now.AddDays(6).ToString("yyyy'-'MM'-'dd"));
            var ListSeances7 = db.Session.Where(r => r.DateSession == Day6).OrderBy(r => r.HeureDebut).ToList();
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
            var ListMachine = db.Machine.ToList();
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
                ViewData["Machine"] = db.Machine.Find(d3).Designation;
                ViewData["IdMachine"] = d3;
                TempData["Alert"] = "";
            }
            ViewData["Day"] = d1.ToString("yyyy'-'MM'-'dd");

            var ListSeances1 = db.Session.Where(r => r.DateSession == d1 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances1"] = ListSeances1;

            var Day1 = d1.AddDays(1);
            var ListSeances2 = db.Session.Where(r => r.DateSession == Day1 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances2"] = ListSeances2;
            ViewData["Day1"] = Day1.ToString("yyyy'-'MM'-'dd");

            var Day2 = d1.AddDays(2);
            var ListSeances3 = db.Session.Where(r => r.DateSession == Day2 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances3"] = ListSeances3;
            ViewData["Day2"] = Day2.ToString("yyyy'-'MM'-'dd");

            var Day3 = d1.AddDays(3);
            var ListSeances4 = db.Session.Where(r => r.DateSession == Day3 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances4"] = ListSeances4;
            ViewData["Day3"] = Day3.ToString("yyyy'-'MM'-'dd");

            var Day4 = d1.AddDays(4);
            var ListSeances5 = db.Session.Where(r => r.DateSession == Day4 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances5"] = ListSeances5;
            ViewData["Day4"] = Day4.ToString("yyyy'-'MM'-'dd");

            var Day5 = d1.AddDays(5);
            var ListSeances6 = db.Session.Where(r => r.DateSession == Day5 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances6"] = ListSeances6;
            ViewData["Day5"] = Day5.ToString("yyyy'-'MM'-'dd");

            var Day6 = d1.AddDays(6);
            var ListSeances7 = db.Session.Where(r => r.DateSession == Day6 && r.IdMaChine == d3).OrderBy(r => r.HeureDebut).ToList();
            ViewData["ListSeances7"] = ListSeances7;
            ViewData["Day6"] = Day6.ToString("yyyy'-'MM'-'dd");

            ViewData["d1"] = d1.ToString("yyyy'-'MM'-'dd");
            ViewData["d2"] = d2.AddDays(6).ToString("yyyy'-'MM'-'dd");

            return View("PlanMachine");

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
