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
    public class CitiesController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: Cities
        public ActionResult Index()
        {
            var cities = db.Cities.Include(c => c.Region);
            return View(cities.ToList());
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "RegionName");
            return View();
        }

        // POST: Cities/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,RegionId")] City city)
        {
            City newCity = db.Cities.FirstOrDefault(c => c.RegionId == city.RegionId && c.Name == city.Name);
            if (newCity == null)
            {
                if (ModelState.IsValid)
                {
                    db.Cities.Add(city);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "RegionName", city.RegionId);
            return View(city);
        }

        // GET: Cities/Edit/5
        //[Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "RegionName", city.RegionId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,RegionId")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RegionId = new SelectList(db.Regions, "Id", "RegionName", city.RegionId);
            return View(city);
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
