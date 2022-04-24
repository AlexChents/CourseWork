using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseChentsov.Helpers;
using CourseChentsov.Models.ViewModel;

namespace CourseChentsov.Controllers
{
    public class ViewCostDayDeliveriesController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: ViewCostDayDeliveries
        public ActionResult Index()
        {
            ViewBag.CityRecipientId = new SelectList(db.Cities.Where(c => c.RegionId == 1), "Id", "Name");
            ViewBag.CitySendId = new SelectList(db.Cities.Where(c => c.RegionId == 1), "Id", "Name");
            ViewBag.RegionRecipientId = new SelectList(db.Regions, "Id", "RegionName");
            ViewBag.RegionSendId = new SelectList(db.Regions, "Id", "RegionName");
            return View();
        }

        public ActionResult GetCostDayDelivery(int? regionSendId, int? regionRecepientId, double? weight)
        {
            if (regionSendId == null || regionRecepientId == null || weight == null)
            {
                return new HtmlResult("Данные неверно заполенено!");
            }

            var basicPriceDaysDelivery = db.DeliveryDistances.FirstOrDefault(b => b.FirstRegionId == regionSendId && b.SecondRegionId == regionRecepientId);
            if (basicPriceDaysDelivery == null)
            {
                basicPriceDaysDelivery = db.DeliveryDistances.FirstOrDefault(b => b.SecondRegionId == regionSendId && b.FirstRegionId == regionRecepientId);
            }

            int idDistance = basicPriceDaysDelivery.BasicPriceDaysDeliveryId;

            var basicData = db.BasicPriceDaysDeliveries.FirstOrDefault(b => b.Id == idDistance);

            double cost = 0;
            if (weight <= 15)
            {
                cost = basicData.BasicPrice;
            }
            else
            {
                cost = basicData.BasicPrice + basicData.PriceForKg * ((double)weight - 15);
            }
            DateTime dateTime = DateTime.Now.AddDays(basicData.CountDays);

            ViewBag.Date = dateTime.ToShortDateString();
            ViewBag.Cost = cost;
            return View();
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
