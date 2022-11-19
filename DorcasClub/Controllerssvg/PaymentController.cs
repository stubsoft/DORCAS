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
    public class PaymentController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();

        // GET: Dorcas_Payment
        public ActionResult Index()
        {
            return View(db.Payment.ToList());
        }

        public ActionResult listepaiements()
        {
            var abc = from obj in db.Payment
                      select new
                      {
                          idPayment = obj.idPayment,
                          dateReception = obj.dateReception.ToString(),
                          dateEcheance = obj.dateEcheance.ToString(),
                          montant = obj.montant,
                          etatPaiement = obj.etatPaiement

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
            Payment payment = db.Payment.Find(id);
            var ListPaiement = new SelectList(db.ModePayement.ToList(), "idModePayment", "Mode");
            ViewData["ListPaiement"] = ListPaiement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["d1"] = DateTime.Parse(payment.dateReception.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(payment.dateEcheance.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Dorcas_Payment/Create
        public ActionResult Create()
        {
            Payment payment = new Payment();
            var ListPaiement = new SelectList(db.ModePayement.ToList(), "idModePayment", "Mode");
            ViewData["ListModePaiement"] = ListPaiement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["idreservation"] = "--";
            ViewData["price"] = "--";
          
            return View(payment);
        }

        // POST: Dorcas_Payment/Create
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idReservation,idModePayment,Devise,dateReception,etatPaiement,dateEcheance,montant")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payment.Add(payment);
                db.SaveChanges();
                Reservation reservation = db.Reservation.Find(payment.idReservation);
                reservation.etat = "Payée";

                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment);
        }

        // GET: Dorcas_Payment/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payment.Find(id);

            var ListPaiement = new SelectList(db.ModePayement.ToList(), "idModePayment", "Mode");
            ViewData["ListPaiement"] = ListPaiement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;

            ViewData["d1"] = DateTime.Parse(payment.dateReception.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Parse(payment.dateEcheance.ToString()).ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Dorcas_Payment/Edit/5
        // Afin de déjouer les attaques par survalidation, activez les propriétés spécifiques auxquelles vous voulez établir une liaison. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPayment,idReservation,idModePayment,Devise,dateReception,etatPaiement,dateEcheance,montant")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment);
        }

        // GET: Dorcas_Payment/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payment.Find(id);
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
            Payment payment = db.Payment.Find(id);
            db.Payment.Remove(payment);
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
            Payment payment = new Payment();
            var ListPaiement = new SelectList(db.ModePayement.ToList(), "idModePayment", "Mode");
            ViewData["ListModePaiement"] = ListPaiement;

            List<Reservation> ListReservation = db.Reservation.ToList();
            ViewData["ListReservation"] = ListReservation;
            ViewData["d1"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");
            ViewData["d2"] = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm");

            ViewData["idreservation"] = payment.idReservation = ID;
            ViewData["price"]= (db.Reservation.Find(ID)).Price;
            payment.montant = (db.Reservation.Find(ID)).Price;
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

            //rd.SetParameterValue("NomSociete", "STUBSOFT SOCIETE");

            int idPayment = ID;
            string ConditionFiltre = "";

            if (idPayment != 0)
            {
                ConditionFiltre = " {Payment.idPayment} = " + idPayment + " ";
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
