using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DorcasClub;

namespace DorcasClub.Controllers
{
    public class PaimentsController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Payment
        public ActionResult Index()
        {
            return View(db.Paiment.ToList());
        }
        public ActionResult listepaiements()
        {
            var abc = from obj in db.Paiment
                      select new
                      {
                          IdPaiment = obj.IdPaiment,
                          DateReception = obj.DateReception.ToString(),
                          DateEcheance = obj.DateEcheance.ToString(),
                          Montant = obj.Montant,
                          EtatPaiement = obj.EtatPaiement

                      };
            var jsonResult = Json(abc.ToList()
            , JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        // GET: Dorcas_Payment/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paiment payment = db.Paiment.Find(id);
           

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["d1"] = DateTime.Parse(payment.DateReception.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(payment.DateEcheance.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["ModePaiment"] = db.ModePaiement.Find(payment.IdModePaiment).Libelle;
            if (payment == null)
            {
                return HttpNotFound();
            }

            var ListModePaiement = new SelectList(db.ModePaiement.ToList(), "IdModePaiment", "Libelle");
            ViewData["ListModePaiement"] = ListModePaiement;
            return View(payment);
        }

        // GET: Dorcas_Payment/Create
        public ActionResult Create()
        {
            Paiment payment = new Paiment();


            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["idreservation"] = "--";
            ViewData["price"] = "--";

            var ListModePaiement = new SelectList(db.ModePaiement.ToList(), "IdModePaiment", "Libelle");
            ViewData["ListModePaiement"] = ListModePaiement;


            return View(payment);
        }

        // POST: Dorcas_Payment/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdReservation,IdModePaiment,Devise,DateReception,EtatPaiement,DateEcheance,Montant,Createur")] Paiment Paiment)
        {
            if (ModelState.IsValid)
            {
                Paiment.Createur = Session["UserNom"].ToString();
                db.Paiment.Add(Paiment);

                db.SaveChanges();
                Reservation reservation = db.Reservation.Find(Paiment.IdReservation);
                reservation.Etat = "Payée";

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Paiment);
        }

        // GET: Dorcas_Payment/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paiment payment = db.Paiment.Find(id);

            var ListPaiement = new SelectList(db.ModePaiement.ToList(), "IdModePaiment", "Libelle");
            ViewData["ListPaiement"] = ListPaiement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["d1"] = DateTime.Parse(payment.DateReception.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(payment.DateEcheance.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            if (payment == null)
            {
                return HttpNotFound();
            }
            var ListModePaiement = new SelectList(db.ModePaiement.ToList(), "IdModePaiment", "Libelle");
            ViewData["ListModePaiement"] = ListModePaiement;

            return View(payment);
        }

        // POST: Dorcas_Payment/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPaiment,IdReservation,IdModePaiment,Devise,DateReception,EtatPaiement,DateEcheance,Montant,Createur")] Paiment Paiment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Paiment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Paiment);
        }

        // GET: Dorcas_Payment/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paiment payment = db.Paiment.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Dorcas_Payment/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paiment payment = db.Paiment.Find(id);
            db.Paiment.Remove(payment);
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

        public ActionResult CreateP(int ID)
        {
            Paiment payment = new Paiment();
            var ListPaiement = new SelectList(db.ModePaiement.ToList(), "IdModePaiment", "Libelle");
            ViewData["ListModePaiement"] = ListPaiement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;
            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["idreservation"] = payment.IdReservation = ID;
            ViewData["price"]= (db.Reservation.Find(ID)).PrixTotal;
            payment.Montant = (db.Reservation.Find(ID)).PrixTotal;
            return View("Create", payment);

        }

        public ActionResult ImpressionRecu(int ID)
        {

            if (Session["UserNom"] == null)
            {
                return RedirectToAction("Index", "Authentification", new { returnUrl = "" });
            }

            ReportDocument rd = new ReportDocument();

            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), "Etat_RecuClient.rpt"));

            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            //rd.SetParameterValue("NomSociete", "DORCAS CLUB");

            int idPayment = ID;
            string ConditionFiltre = "";

            if (idPayment != 0)
            {
                ConditionFiltre = " {Paiment.IdPaiment} = " + idPayment + " ";
            }

            rd.RecordSelectionFormula = ConditionFiltre;

            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            // type de document à construire
            Response.ContentType = "application/pdf";

            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, (int)stream.Length);

            Response.BinaryWrite(buffer);


            Response.End();
            return File(stream, "application/pdf", "Recu.pdf");
        }

        protected void setDbInfo(ReportDocument rptDoc, string Server, string dbName, string UserId, string Pwd)
        {

            TableLogOnInfo logoninfo = new TableLogOnInfo();
            foreach (Table tbl in rptDoc.Database.Tables)
            {

                logoninfo = tbl.LogOnInfo;
                logoninfo.ReportName = rptDoc.Name;
                logoninfo.ConnectionInfo.ServerName = Server;
                logoninfo.ConnectionInfo.DatabaseName = dbName;
                logoninfo.ConnectionInfo.UserID = UserId;
                logoninfo.ConnectionInfo.Password = Pwd;
                //logoninfo.TableName = "Crm_TacheReclamation";
                tbl.ApplyLogOnInfo(logoninfo);
                //tbl.Location = "Crm_TacheReclamation";
            }
        }

    }
}
