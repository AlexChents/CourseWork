using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CourseChentsov.Helpers;
using CourseChentsov.Models;
using CourseChentsov.Models.ViewModel;
using Microsoft.AspNet.Identity;

namespace CourseChentsov.Controllers
{
    [AttrConfirmEmail]
    public class PackagesController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: Packages
        public ActionResult Index()
        {
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = dateFrom.AddDays(1);
            return DataPackages(dateFrom, dateTo);
        }

        //[HttpPost]
        public ActionResult IndexPartial(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            return DataPackages(dateTimeFrom, dateTimeTo.AddDays(1));
        }   

        private ActionResult DataPackages(DateTime dateFrom, DateTime dateTo)
        {
            string login = User.Identity.GetUserName();
            
            var userLogin = db.UserLogins.FirstOrDefault(ul => ul.Email == login);
            if (userLogin == null)
            {
                return new HttpNotFoundResult();
            }
            int userId = userLogin.Id;
            int roleId = userLogin.RoleId;
            ViewBag.dateTimeFrom = dateFrom.ToString("yyyy-MM-dd");
            ViewBag.dateTimeTo = dateTo.AddDays(-1).ToString("yyyy-MM-dd");
            if (roleId == 3)
            {
                var packages = db.Packages.Where(p => p.DateSend >= dateFrom.Date && p.DateSend <= dateTo.Date && p.UserId == userId).OrderByDescending(p => p.DateSend)
                .Include(p => p.DepartmentRecipient).Include(p => p.DispatchRegister).Include(p => p.Recipient).Include(p => p.User);
                return View(packages.ToList());
            }
            else
            {
                var packages = db.Packages.Where(p => p.DateSend >= dateFrom.Date && p.DateSend <= dateTo.Date).OrderByDescending(p => p.DateSend)
                .Include(p => p.DepartmentRecipient).Include(p => p.DispatchRegister).Include(p => p.Recipient).Include(p => p.User);
                return View(packages.ToList());
            }
        }

        public int AddDeclarationsToRegister(string arrayDeclaration)
        {
            var id = arrayDeclaration.Split(',');
            List<int> idInts = new List<int>();
            for (int i = 0; i < id.Length; i++)
            {
                idInts.Add(Convert.ToInt32(id[i]));
            }
            int idInt = idInts[0];
            int userId = db.Packages.Where(p => p.Id == idInt).Select(p => p.UserId).FirstOrDefault();

            for (int i = 0; i < idInts.Count(); i++)
            {
                idInt = idInts[i];
                Package package = db.Packages.Where(p => p.Id == idInt).Select(p => p).FirstOrDefault();
                if (package.InRegister)
                {
                    return 0;
                }
            }

            DispatchRegister dispatchRegister = new DispatchRegister
            {
                DateCreate = DateTime.Now,
                UserId = userId
            };
            db.DispatchRegisters.Add(dispatchRegister);
            db.SaveChanges();

            for (int i = 0; i < idInts.Count(); i++)
            {
                idInt = idInts[i];
                Package package = db.Packages.Where(p => p.Id == idInt).Select(p => p).FirstOrDefault();
                package.DispatchRegisterId = dispatchRegister.Id;
                package.InRegister = true;
                db.SaveChanges();
            }

            return dispatchRegister.Id;
        }


        // GET: Packages/Create
        public ActionResult Create()
        {
            string login = User.Identity.GetUserName();
            DataForCreateEditPackage(login);
            ViewBag.DepartmentIdRecipient = new SelectList(db.Departments.Where(d => d.CityId == 1)
                                                            .Select(d => d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)"));
            ViewBag.CityRecipient = new SelectList(db.Cities.Where(c => c.RegionId == 1), "Id", "Name");
            ViewBag.RegionRecipient = new SelectList(db.Regions, "Id", "RegionName");
            return View();
        }

        private void DataForCreateEditPackage(string login)
        {
            int idLogin = db.UserLogins.FirstOrDefault(ul => ul.Email == login).Id;
            var myDepartment = db.Users.FirstOrDefault(md => md.Id == idLogin);
            int myDepartmentId;
            if (myDepartment != null)
                myDepartmentId = (int)myDepartment.DepartmentId;
            else
                myDepartmentId = 1;

            int myCity = db.Departments.FirstOrDefault(mc => mc.Id == myDepartmentId).CityId;
            int myRegion = db.Cities.FirstOrDefault(mr => mr.Id == myCity).RegionId;

            ViewBag.CitySender = new SelectList(db.Cities.Where(c => c.RegionId == myRegion), "Id", "Name", myCity);
            var contactPerson = db.Users.FirstOrDefault(u => u.Id == idLogin);
            if (contactPerson != null)
            {
                ViewBag.SenderName = contactPerson.SenderName;
                ViewBag.LastName = contactPerson.LastName;
                ViewBag.FirstName = contactPerson.FirstName;
                ViewBag.ThirdName = contactPerson.ThirdName;
                ViewBag.PhoneNumber = contactPerson.PhoneNumber;
            }
            ViewBag.RegionSender = new SelectList(db.Regions, "Id", "RegionName", myRegion);
            string adress = db.Departments.FirstOrDefault(d => d.Id == myDepartmentId).Adress;
            double weight = db.Departments.FirstOrDefault(d => d.Id == myDepartmentId).MaxWeight;
            ViewBag.DepartmentIdSender = new SelectList(db.Departments.Where(d => d.CityId == myCity).Select(d => d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)"),
                myDepartmentId + ", " + adress + ", (отделение до " + weight + " кг.)");

            
        }

        // POST: Packages/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewPackage viewPackage)
        {
            Package package = ValidationCheckPackage(viewPackage);
            if (ModelState.IsValid)
            {
                db.Packages.Add(package);
                db.SaveChanges();
                package.NumberDelivery = 10000000000 + package.Id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewPackage);
        }

        private Package ValidationCheckPackage(ViewPackage viewPackage)
        {

            int idDepSend = db.Departments.Where(d => (d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)") == viewPackage.DepartmentIdSender).Select(d => d.Id).FirstOrDefault();
            int idDepRecepient = db.Departments.Where(d => (d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)") == viewPackage.DepartmentIdRecipient).Select(d => d.Id).FirstOrDefault();
            int idRegionSend = db.Regions.Where(c => c.Id == viewPackage.RegionSender).Select(c => c.Id).FirstOrDefault();
            int idRegionRecipient = db.Regions.Where(c => c.Id == viewPackage.RegionRecipient).Select(c => c.Id).FirstOrDefault();
            int distance = db.DeliveryDistances.Where(dd => dd.FirstRegionId == idRegionSend && dd.SecondRegionId == idRegionRecipient).Select(dd => dd.BasicPriceDaysDeliveryId).FirstOrDefault();
            if (distance == 0)
                distance = db.DeliveryDistances.Where(dd => dd.FirstRegionId == idRegionRecipient && dd.SecondRegionId == idRegionSend).Select(dd => dd.BasicPriceDaysDeliveryId).FirstOrDefault();
            var recipientId = db.Recipients.FirstOrDefault(r => r.PhoneNumber == viewPackage.NumberPhoneRecipient);
            Recipient recipient;
            if (recipientId == null)
            {
                recipient = new Recipient
                {
                    FirstName = viewPackage.FirstNameRecipient,
                    LastName = viewPackage.LastNameRecipient,
                    ThirdName = viewPackage.ThirdNameRecipient,
                    PhoneNumber = viewPackage.NumberPhoneRecipient,
                    RecipientName = viewPackage.RecipientName
                };
                db.Recipients.Add(recipient);
                db.SaveChanges();
            }
            string login = User.Identity.Name;
            
            if(viewPackage.Id != 0 && User.IsInRole("admin"))
            {
                using (CourseContext context = new CourseContext())
                {
                    login = context.Packages.FirstOrDefault(p => p.Id == viewPackage.Id).User.UserLogin.Email;
                }
            }

            var senderId = db.Users.FirstOrDefault(u => u.UserLogin.Email == login);
            User user;
            if (senderId == null)
            {
                user = new User
                {
                    DepartmentId = idDepSend,
                    FirstName = viewPackage.FirstNameSender,
                    LastName = viewPackage.LastNameSender,
                    PhoneNumber = viewPackage.NumberPhoneSender,
                    ThirdName = viewPackage.ThirdNameSender,
                    SenderName = viewPackage.SenderName,
                    Id = db.UserLogins.FirstOrDefault(u => u.Email == login).Id
                };
                db.Users.Add(user);
                db.SaveChanges();
            }
            double weight = 0;
            if (viewPackage.Weight > 15)
                weight = viewPackage.Weight - 15;
            Package package = new Package
            {
                Comment = viewPackage.Comment,
                CountSeats = viewPackage.CountSeats,
                DepartmentSendId = idDepSend,
                DepartmentRecipientId = idDepRecepient,
                DeliveryCost = db.BasicPriceDaysDeliveries.FirstOrDefault(d => d.Id == distance).BasicPrice + weight * db.BasicPriceDaysDeliveries.FirstOrDefault(d => d.Id == distance).PriceForKg,
                Description = viewPackage.Description,
                DateSend = DateTime.Now,
                DateDelivery = DateTime.Now.AddDays(db.BasicPriceDaysDeliveries.FirstOrDefault(d => d.Id == distance).CountDays),
                RecipientId = db.Recipients.FirstOrDefault(r => r.PhoneNumber == viewPackage.NumberPhoneRecipient).Id,
                UserId = db.UserLogins.FirstOrDefault(u => u.Email == login).Id,
                Weight = viewPackage.Weight,
                StatusPackageId = 1
            };
            return package;
        }

        // GET: Packages/Edit/5
        public ActionResult Edit(int? id)
        {
            string login = User.Identity.GetUserName();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            
            if (package == null || (!User.IsInRole("admin") && package.User.UserLogin.Email != login))
            {
                return HttpNotFound();
            }
            if (package.IsPrint)
            {
                return new HtmlResult("Данная декларация распечатана, редактировать невозможно!");
            }

            if (package.InRegister)
            {
                return new HtmlResult("Данная декларация добавлена в реестр, редактировать невозможно!");
            }

            ViewPackage viewPackage = new ViewPackage
            {
                CityRecipient = package.DepartmentRecipientId,
                CitySender = package.DepartmentSendId,
                Comment = package.Comment,
                CountSeats = package.CountSeats,
                DepartmentIdRecipient = package.DepartmentRecipientId.ToString(),
                Description = package.Description, 
                DepartmentIdSender = package.DepartmentSendId.ToString(),
                FirstNameRecipient = package.Recipient.FirstName,
                FirstNameSender = package.User.FirstName,
                Id = package.Id,
                LastNameRecipient = package.Recipient.LastName,
                LastNameSender = package.User.LastName,
                NumberPhoneRecipient = package.Recipient.PhoneNumber,
                NumberPhoneSender = package.User.PhoneNumber,
                RecipientName = package.Recipient.RecipientName,
                RegionRecipient = package.DepartmentRecipient.City.RegionId,
                RegionSender = package.DepartmentSend.City.RegionId,
                SenderName = package.User.SenderName,
                ThirdNameRecipient = package.Recipient.ThirdName,
                ThirdNameSender = package.User.ThirdName,
                Weight = package.Weight
            };
            if (User.IsInRole("admin"))
            {
                login = package.User.UserLogin.Email;
            }
            DataForCreateEditPackage(login);
            ViewBag.RegionRecipient = new SelectList(db.Regions, "Id", "RegionName", package.DepartmentRecipient.City.RegionId);
            ViewBag.CityRecipient = new SelectList(db.Cities.Where(c => c.RegionId == package.DepartmentRecipient.City.RegionId), "Id", "Name", package.DepartmentRecipient.CityId);
            ViewBag.DepartmentIdRecipient = new SelectList(db.Departments.Where(d => d.CityId == package.DepartmentRecipient.CityId).
                Select(d => d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)"), 
                $"{package.DepartmentRecipientId}, {package.DepartmentRecipient.Adress}, (отделение до {package.DepartmentRecipient.MaxWeight} кг.)");
            return View(viewPackage);
        }

        // POST: Packages/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewPackage viewPackage)
        {
            //string login = User.Identity.GetUserName();
            Package package = ValidationCheckPackage(viewPackage);
            package.Id = viewPackage.Id;
            package.NumberDelivery = 10000000000 + viewPackage.Id;
            if (ModelState.IsValid)
            {
                db.Entry(package).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(package);
        }

        // GET: Packages/Delete/5
        public ActionResult Delete(int? id)
        {
            string login = User.Identity.GetUserName();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Package package = db.Packages.Find(id);
            if (package == null || (!User.IsInRole("admin") && package.User.UserLogin.Email != login))
            {
                return HttpNotFound();
            }
            if (package.InRegister && package.DispatchRegister.IsPrint)
            {
                return new HtmlResult("Данная декларация в добавлена в распечатанный реестр, удаление невозможно!");
            }
            if (package.StatusPackageId > 1)
            {
                return new HtmlResult("Удаление невозможно, посылка в отделении!");
            }

            return View(package);
        }

        // POST: Packages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Package package = db.Packages.Find(id);
            int? idReg = package.DispatchRegisterId;
            db.Packages.Remove(package);
            db.SaveChanges();
            int registersId = db.Packages.Where(p => p.DispatchRegisterId == idReg).Select(p => p.Id).FirstOrDefault();
            if (registersId == 0)
            {
                DispatchRegister dispatchRegister = db.DispatchRegisters.Find(idReg);
                db.DispatchRegisters.Remove(dispatchRegister);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult ViewPackageToSend()
        {
            var manager = db.Users.FirstOrDefault(u => u.UserLogin.Email == User.Identity.Name);
            int id = 1;
            if (manager != null && manager.DepartmentId != null)
            {
                id = (int)manager.DepartmentId;
            }
            var packages = db.Packages.Where(p => p.DepartmentSendId == id && p.StatusPackageId >= 2 && p.StatusPackageId <= 3)
                .Select(p => p).OrderBy(p => p.StatusPackageId);
            ViewBag.IdDep = id;

            if (User.IsInRole("admin"))
            {
                packages = db.Packages.Where(p => p.StatusPackageId >= 2 && p.StatusPackageId <= 3).Select(p => p).OrderBy(p => p.StatusPackageId);
                ViewBag.IdDep = " - все отделения";
            }
            ViewBag.StatusPackage = new SelectList(db.StatusPackages.Where(s => s.Id >=2 && s.Id <= 3), "Id", "StatusName");
            return View(packages.ToList());
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult ViewPackageToRecepient()
        {
            var manager = db.Users.FirstOrDefault(u => u.UserLogin.Email == User.Identity.Name);
            int id = 1;
            if (manager != null && manager.DepartmentId != null)
            {
                id = (int)manager.DepartmentId;
            }
            var packages = db.Packages.Where(p => p.DepartmentRecipientId == id && p.StatusPackageId >= 3 && p.StatusPackageId <= 4)
                .Select(p => p).OrderByDescending(p => p.StatusPackageId);
            ViewBag.IdDep = id;
            if (User.IsInRole("admin"))
            {
                packages = db.Packages.Where(p => p.StatusPackageId >= 3 && p.StatusPackageId <= 4)
                .Select(p => p).OrderByDescending(p => p.StatusPackageId);
                ViewBag.IdDep = " - все отделения";
            }

            ViewBag.StatusPackagetoDep = new SelectList(db.StatusPackages.Where(s => s.Id >= 3 && s.Id <= 5), "Id", "StatusName");
            ViewBag.StatusPackageIndep = new SelectList(db.StatusPackages.Where(s => s.Id >= 4 && s.Id <= 5), "Id", "StatusName");
            return View(packages.ToList());
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult ChangeStatus(int? id, int? idStatus)
        {
            if (id == null || idStatus == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var package = db.Packages.Find(id);
            if (package == null)
            {
                return HttpNotFound();
            }
            package.StatusPackageId = idStatus;
            db.SaveChanges();
            /*
            if (package.StatusPackageId == 4)
            {
                SMSC sMSC = new SMSC();
                string message = $"Ваша посылка №{package.NumberDelivery} прибыла в отделение №{package.DepartmentRecipientId}, г. {package.DepartmentRecipient.City.Name}, {package.DepartmentRecipient.Adress}.";
                sMSC.send_sms(package.Recipient.PhoneNumber, message);
            }
            */
            return RedirectToAction("ViewPackageToSend");
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
