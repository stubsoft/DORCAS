using System;
using System.Collections.Generic;
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
    public class MembershipsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Memberships
        public ActionResult Index()
        {
            Memberships dorcas_Memberships = new Memberships();

            //DropdownList
            var ListMember = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMember"] = ListMember;

            var ListMembershipType = new SelectList(db.Memberships.ToList(), "idMembers", "membershipType");
            ViewData["ListMembershipType"] = ListMembershipType;

            List<Memberships> ListMembership = db.Memberships.ToList();
            ViewData["ListMembership"] = ListMembership;

            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - dd");
            ViewData["d11"] = DateTime.Now.ToString("yyyy - MM - dd");
            ViewData["d22"] = DateTime.Now.ToString("yyyy - MM - dd");
            return View(dorcas_Memberships);
        }

        // GET: Dorcas_Memberships/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Memberships memberships = db.Memberships.Find(id);
            if (memberships == null)
            {
                return HttpNotFound();
            }
            //DropdownList

            ViewData["d1"] = DateTime.Parse(memberships.DateDu.ToString()).ToString("yyyy'-'MM'-'dd");
            ViewData["d2"] = DateTime.Parse(memberships.DateAu.ToString()).ToString("yyyy'-'MM'-'dd");

            return View(memberships);
        }

        // GET: Dorcas_Memberships/Create
        public ActionResult Create()
        {
            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - dd");
            ViewData["d2"] = DateTime.Now.ToString("yyyy - MM - dd");
            return View();
        }

        // POST: Dorcas_Memberships/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name_Members,login,password,adress_Members,CIN_Members,membershipType,Duree,DateDu,DateAu,mobile,phone,Email,Wieght,Taille")] Memberships memberships)
        {
            if (ModelState.IsValid)
            {
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["imageElement"];
                SecurityServices securityService = new SecurityServices();
                memberships.imageelement = securityService.ConvertToBytes(file);

                /*EndInsert image*/


                /*Image/root/rename*/

                Guid guid = Guid.NewGuid();

                string newfileName = guid.ToString();

                string fileextention = Path.GetExtension(file.FileName);

                string fileName = newfileName + fileextention;

                string uploadpath = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(fileName));

                file.SaveAs(uploadpath);

                /*End/Image/root/rename*/

                memberships.FileName = fileName;
                memberships.imagephysique = file.FileName;
                memberships.DateCreation = DateTime.Now;
                db.Memberships.Add(memberships);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberships);
        }

        // GET: Dorcas_Memberships/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Memberships memberships = db.Memberships.Find(id);
            if (memberships == null)
            {
                return HttpNotFound();
            }

            ViewData["d1"] = DateTime.Parse(memberships.DateDu.ToString()).ToString("yyyy'-'MM'-'dd");
            ViewData["d2"] = DateTime.Parse(memberships.DateAu.ToString()).ToString("yyyy'-'MM'-'dd");

            return View(memberships);
        }

        // POST: Dorcas_Memberships/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idMembers,name_Members,login,password,adress_Members,CIN_Members,membershipType,Duree,DateDu,DateAu,mobile,phone,Email,Wieght,Taille,imageelement,FileName,imagephysique")] Memberships memberships)
        {
            if (ModelState.IsValid)
            {
                SecurityServices securityService = new SecurityServices();
                /*Insert image*/
                HttpPostedFileBase file = Request.Files["EditimageElement"];
                if (file.FileName != "")
                {
                    memberships.imageelement = securityService.ConvertToBytes(file);
                }
                //*EndInsert image*/

                /*Update file*/

                if (file.FileName != "")
                {
                    //Delete Old file from the file system
                    var path = Path.Combine(Server.MapPath("~/UploadedFiles"), memberships.FileName);
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
                    memberships.FileName = fileName;
                    memberships.imagephysique = file.FileName;

                    file.SaveAs(uploadpath);
                }
                /*Insert Update file*/

                db.Entry(memberships).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberships);
        }


        public ActionResult Search()
        {
            Memberships dorcas_Memberships = new Memberships();

            //DropdownList


            var ListMember = new SelectList(db.Memberships.ToList(), "idMembers", "name_Members");
            ViewData["ListMember"] = ListMember;

            var ListMembershipType = new SelectList(db.Memberships.ToList(), "idMembers", "membershipType");
            ViewData["ListMembershipType"] = ListMembershipType;

            List<Memberships> ListMembership = db.Memberships.ToList();
            ViewData["ListMembership"] = ListMembership;

            ViewData["d1"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            ViewData["d11"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");
            ViewData["d22"] = DateTime.Now.ToString("yyyy - MM - ddThh:mm");

            return View(dorcas_Memberships);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchMember()
        {
            Memberships dorcas_Memberships = new Memberships();

            var d11 = DateTime.Parse(Request["d11"]);
            var d22 = DateTime.Parse(Request["d22"]);

            var name_Members = Request["name_Members"];
            var membershipType = Request["membershipType"];


            ViewData["d11"] = d11.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d22"] = d22.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["name_Members"] = name_Members;
            ViewData["membershipType"] = membershipType;

            var model = from r in db.Memberships

                        where r.DateCreation > d11 && r.DateCreation < d22 || r.name_Members == name_Members || r.membershipType == membershipType
                        select r;
            //DropdownList





            List<Memberships> ListMembership = db.Memberships.ToList();
            ViewData["ListMembership"] = ListMembership;

            return View("Search", model.ToList());

        }
        // GET: Dorcas_Memberships/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Memberships memberships = db.Memberships.Find(id);
            if (memberships == null)
            {
                return HttpNotFound();
            }
            return View(memberships);
        }

        // POST: Dorcas_Memberships/Delete/5
        // [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Memberships memberships = db.Memberships.Find(id);
            db.Memberships.Remove(memberships);
            db.SaveChanges();
            //Delete Old file from the file system
            if (memberships.FileName != null)
            {
                var path = Path.Combine(Server.MapPath("~/UploadedFiles"), memberships.FileName);
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
            var q = from temp in db.Memberships where temp.idMembers == Id select temp.imageelement;
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
