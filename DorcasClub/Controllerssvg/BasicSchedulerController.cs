using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DorcasClub.Controllers
{
  public class BasicSchedulerController : Controller
    {
        private SportRestaurantEntities db = new SportRestaurantEntities();
        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Terrace;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.InitialDate = DateTime.Now;
            return View(sched);
        }
 
        public ContentResult Data()
        {
            return (new SchedulerAjaxData(
                new SportRestaurantEntities().Events
                .Select(e => new { e.id, e.text, e.start_date, e.end_date })
                )
                );
        }
 
        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            var changedEvent = DHXEventsHelper.Bind<Events>(actionValues);
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        db.Events.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        changedEvent = db.Events.FirstOrDefault(ev => ev.id == action.SourceId);
                        db.Events.Remove(changedEvent);
                        break;
                    default:// "update"
                        var target = db.Events.Single(e => e.id == changedEvent.id);
                        DHXEventsHelper.Update(target, changedEvent, new List<string> { "id" });
                        break;
                }
                db.SaveChanges();
                action.TargetId = changedEvent.id;
            }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }
 
            return (new AjaxSaveResponse(action));
        }
    }
}