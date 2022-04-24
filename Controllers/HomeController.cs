using CourseChentsov.Helpers;
using CourseChentsov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseChentsov.Controllers
{
    public class HomeController : Controller
    {
        private CourseContext db = new CourseContext();
        // GET: Home
        public ActionResult Index()
        {
            var news = db.News.OrderByDescending(n => n.Id).Take(3);
            return View(news.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult Navigation()
        {
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