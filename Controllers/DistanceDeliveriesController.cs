using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CourseChentsov.Helpers;
using CourseChentsov.Models;

namespace CourseChentsov.Controllers
{
    public class DistanceDeliveriesController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: DistanceDeliveries
        public ActionResult Index()
        {
            var deliveryDistances = db.DeliveryDistances.Include(d => d.BasicPriceDaysDelivery).Include(d => d.RegionFirst).Include(d => d.RegionSecond);
            ViewBag.BasicDeliveryDistance = db.BasicPriceDaysDeliveries;
            ViewBag.Region = new SelectList(db.Regions, "Id", "RegionName");
            return View(deliveryDistances.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult IndexEdit()
        {
            var deliveryDistances = db.DeliveryDistances.Include(d => d.BasicPriceDaysDelivery).Include(d => d.RegionFirst).Include(d => d.RegionSecond);
            ViewBag.Region = new SelectList(db.Regions, "Id", "RegionName");
            return View(deliveryDistances.ToList());
        }

        // GET: DistanceDeliveries/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DistanceDelivery distanceDelivery = db.DeliveryDistances.Find(id);
            if (distanceDelivery == null)
            {
                return HttpNotFound();
            }
            ViewBag.BasicPriceDaysDeliveryId = new SelectList(db.BasicPriceDaysDeliveries, "Id", "Id", distanceDelivery.BasicPriceDaysDeliveryId);
            ViewBag.FirstRegionId = new SelectList(db.Regions, "Id", "RegionName", distanceDelivery.FirstRegionId);
            ViewBag.SecondRegionId = new SelectList(db.Regions, "Id", "RegionName", distanceDelivery.SecondRegionId);
            return View(distanceDelivery);
        }

        // POST: DistanceDeliveries/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstRegionId,SecondRegionId,BasicPriceDaysDeliveryId")] DistanceDelivery distanceDelivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(distanceDelivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexEdit");
            }
            ViewBag.BasicPriceDaysDeliveryId = new SelectList(db.BasicPriceDaysDeliveries, "Id", "Id", distanceDelivery.BasicPriceDaysDeliveryId);
            ViewBag.FirstRegionId = new SelectList(db.Regions, "Id", "RegionName", distanceDelivery.FirstRegionId);
            ViewBag.SecondRegionId = new SelectList(db.Regions, "Id", "RegionName", distanceDelivery.SecondRegionId);
            return View(distanceDelivery);
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
