using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.IO;
using System.Windows;

namespace DorcasClub.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            var DateDebut = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            var DateFin = DateTime.Now.ToString("yyyy'-'MM'-'dd");
            ViewData["DateDebut"] = DateDebut;
            ViewData["DateFin"] = DateFin;
            return View();
        }
        public ActionResult Impression()
        {
            string PrintSelect = Request.Form["PrintSelect"].ToString();
            string fileName = "";
            string ConditionFiltre = "";
            ReportDocument rd = new ReportDocument();
            //MessageBox.Show(PrintSelect);
            //MessageBox.Show(RepasLivrer);
            //MessageBox.Show(SeancesMachine);
            if (PrintSelect == "RepasPreparer")
            {
                fileName = "Etat_RepasPreparer.rpt";
            }
            if (PrintSelect == "RepasLivrer")
            {
                fileName = "Etat_RepasLivrer.rpt";
            }
            if (PrintSelect == "SeancesMachine")
            {
                fileName = "Etat_SeancesMachine.rpt";
            }
            if (PrintSelect == "RepasKcal")
            {
                fileName = "Etat_RepasKcal.rpt";
            }
            if (PrintSelect == "ClientRecouvrement")
            {
                fileName = "Etat_ClientRecouvrement.rpt";
            }
           
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports"), fileName));
            string Serveur = ConfigurationManager.AppSettings["Serveur"];
            string Db = ConfigurationManager.AppSettings["Db"];
            string Usr = ConfigurationManager.AppSettings["Usr"];
            string Pwd = ConfigurationManager.AppSettings["Pwd"];
            setDbInfo(rd, Serveur, Db, Usr, Pwd);

            // condition filtre
            string DateDebut = "";
            string DateFin = "";
            
            if (Request["DateDebut"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateDebut"].ToString());
                DateDebut = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;
            }
            if (Request["DateFin"] != "")
            {
                DateTime oDate = DateTime.Parse(Request["DateFin"].ToString());
                DateFin = oDate.Day + "/" + oDate.Month + "/" + oDate.Year;

            }
            

            // rd.SetParameterValue("Etat", Etat);

            rd.SetParameterValue("NomSociete", "DORCAS CLUB");
            rd.SetParameterValue("@DateDebut", DateDebut);
            rd.SetParameterValue("@DateFin", DateFin);

            ConditionFiltre = "  {Reservation.DateDu} in CDATE('" + @DateDebut + "' ) to CDATE('" + @DateFin + "' ) ";
            
            rd.RecordSelectionFormula = ConditionFiltre;

            // choisir quel type de document à télécharger
            Stream stream = rd.ExportToStream(ExportFormatType.PortableDocFormat);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            // type de document à construire
            Response.ContentType = "application/pdf";
            stream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            stream.Read(buffer, 0, (int)stream.Length);
            Response.BinaryWrite(buffer);
            Response.End();

            // retour téléchargement pdf
            return File(stream, "application/pdf", "TacheParClient.pdf");

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