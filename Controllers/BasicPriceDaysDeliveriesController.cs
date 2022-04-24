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
    [Authorize(Roles = "admin")]
    public class BasicPriceDaysDeliveriesController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: BasicPriceDaysDeliveries
        public ActionResult Index()
        {
            return View(db.BasicPriceDaysDeliveries.ToList());
        }

        // GET: BasicPriceDaysDeliveries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BasicPriceDaysDeliveries/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BasicPrice,PriceForKg,CountDays")] BasicPriceDaysDelivery basicPriceDaysDelivery)
        {
            if (ModelState.IsValid)
            {
                db.BasicPriceDaysDeliveries.Add(basicPriceDaysDelivery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(basicPriceDaysDelivery);
        }

        // GET: BasicPriceDaysDeliveries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BasicPriceDaysDelivery basicPriceDaysDelivery = db.BasicPriceDaysDeliveries.Find(id);
            if (basicPriceDaysDelivery == null)
            {
                return HttpNotFound();
            }
            return View(basicPriceDaysDelivery);
        }

        // POST: BasicPriceDaysDeliveries/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BasicPrice,PriceForKg,CountDays")] BasicPriceDaysDelivery basicPriceDaysDelivery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basicPriceDaysDelivery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(basicPriceDaysDelivery);
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
