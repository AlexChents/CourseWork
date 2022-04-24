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
using CourseChentsov.Models.ViewModel;

namespace CourseChentsov.Controllers
{
    public class ViewFindPackagesController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: ViewFindPackages
        public ActionResult Index()
        {
            return View();
        }
        // GET: ViewFindPackages/Edit/5
        public ActionResult DataInfo(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Package package = db.Packages.FirstOrDefault(p => p.NumberDelivery == id);
            if (package == null)
            {
                return new HtmlResult($"Декларация № {id} не найдена!");
            }

            return View(package);
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
