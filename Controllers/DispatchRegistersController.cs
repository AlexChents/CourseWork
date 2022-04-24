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
using Microsoft.AspNet.Identity;

namespace CourseChentsov.Controllers
{
    [AttrConfirmEmail]
    public class DispatchRegistersController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: DispatchRegisters
        public ActionResult Index()
        {
            string login = User.Identity.GetUserName();
            if (User.IsInRole("user"))
            {
                var dispatchRegisters = db.DispatchRegisters.Where(dr => dr.User.UserLogin.Email == login).Include(d => d.User).Include(d => d.Packages).OrderByDescending(d => d.DateCreate);
                return View(dispatchRegisters.ToList());
            }
            else
            { 
                var dispatchRegisters = db.DispatchRegisters.Include(d => d.User).Include(d => d.Packages).OrderByDescending(d => d.DateCreate);
                return View(dispatchRegisters.ToList());
            }
        }

        // GET: DispatchRegisters/Details/5
        public ActionResult Details(int? id)
        {
            string login = User.Identity.GetUserName();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispatchRegister dispatchRegister = db.DispatchRegisters.Find(id);
            if (dispatchRegister == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("user") && dispatchRegister.User.UserLogin.Email != login)
                return HttpNotFound();

            var packages = db.Packages.Where(p => p.DispatchRegisterId == dispatchRegister.Id).Select(p => p);
            ViewBag.packagesCount = db.Packages.Where(p => p.DispatchRegisterId == dispatchRegister.Id).Select(p => p).Count();
            ViewBag.NumberId = id;
            ViewBag.DateRegister = db.DispatchRegisters.FirstOrDefault(r => r.Id == id).DateCreate.ToShortDateString();
            return View(packages.ToList());
        }

        // GET: DispatchRegisters1/Edit/5
        public ActionResult Edit(int? id)
        {
            string login = User.Identity.GetUserName();
            if (id != null)
            {

                DispatchRegister dispatchRegister = db.DispatchRegisters.Find(id);
                if (dispatchRegister != null)
                {
                    if (User.IsInRole("admin") || User.IsInRole("manager"))
                    {
                        dispatchRegister.InDepartmentSend = true;
                        db.SaveChanges();
                        var packages = db.Packages.Where(p => p.DispatchRegisterId == id).Select(p => p).ToList();
                        foreach (var item in packages)
                        {
                            item.StatusPackageId = 2;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                return View("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: DispatchRegisters1/Delete/5
        public ActionResult Delete(int? id)
        {
            string login = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispatchRegister dispatchRegister = db.DispatchRegisters.Find(id);
            if (dispatchRegister == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("user") && dispatchRegister.User.UserLogin.Email != login)
                return HttpNotFound();


            if (dispatchRegister.IsPrint)
            {
                return new HtmlResult("Данный реестр распечатан и не может быть изменен. Удаление невозможно!");
            }

            else
            {
                return View(dispatchRegister);
            }
        }

        // POST: DispatchRegisters1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DispatchRegister dispatchRegister = db.DispatchRegisters.Find(id);
            var packages = db.Packages.Where(p => p.DispatchRegisterId == id).Select(p => p).ToList();
            foreach (var item in packages)
            {
                item.InRegister = false;
                item.DispatchRegisterId = null;
                db.SaveChanges();
            }

            db.DispatchRegisters.Remove(dispatchRegister);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeletePackageFromRegister(int? id)
        {
            string login = User.Identity.GetUserName();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Package package = db.Packages.Find(id);

            if (package == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("user") && package.User.UserLogin.Email != login)
                return HttpNotFound();

            int idRegister = (int)package.DispatchRegisterId;
            DispatchRegister dispatchRegister = db.DispatchRegisters.Find(package.DispatchRegisterId);

            if (dispatchRegister.IsPrint || dispatchRegister.InDepartmentSend)
            {
                return new HtmlResult("Данный реестр распечатан и не может быть изменен. Удаление невозможно!");
            }

            package.DispatchRegisterId = null;
            package.InRegister = false;
            db.SaveChanges();

            int registersId = db.Packages.Where(p => p.DispatchRegisterId == idRegister).Select(p => p.Id).FirstOrDefault();
            if (registersId == 0)
            {
                db.DispatchRegisters.Remove(dispatchRegister);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction($"Details/{idRegister}");
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
