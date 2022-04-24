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
    public class DepartmentsController : Controller
    {
        private CourseContext db = new CourseContext();

        // GET: Departments
        public ActionResult Index()
        {
            var departments = db.Departments.Include(d => d.City).OrderBy(d => d.Id);
            ViewBag.City = new SelectList(db.Cities, "Id", "Name");
            ViewBag.Region = new SelectList(db.Regions, "Id", "RegionName");
            return View(departments.ToList());
        }

        public ActionResult PartialIndexRegion(int? regionId)
        {
            if (regionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var city = db.Cities.Where(c => c.RegionId == regionId);
            return View(city.ToList());
        }

        public ActionResult PartialIndexCity(int? cityId)
        {
            if (cityId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var departments = db.Departments.Where(d => d.CityId == cityId && d.StatusDepartmentId == 1);
            return View(departments.ToList());
        }

        public ActionResult IdDepartments(int? cityId)
        {
            if (cityId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var department = db.Departments.Where(d => d.CityId == cityId && d.StatusDepartmentId == 1);
            return View(department.ToList());
        }

        public int GetMaxWeight(int firstDep, int secondDep)
        {
            int firstWeigt = db.Departments.Where(d => d.Id == firstDep).Select(d => d.MaxWeight).FirstOrDefault();
            int secondWeigt = db.Departments.Where(d => d.Id == secondDep).Select(d => d.MaxWeight).FirstOrDefault();
            if (firstWeigt > secondWeigt)
                return secondWeigt;
            else
                return firstWeigt;
        }

        // GET: Departments/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.Region = new SelectList(db.Regions, "Id", "RegionName");
            ViewBag.City = new SelectList(db.Cities.Where(c => c.RegionId == db.Regions.FirstOrDefault().Id), "Id", "Name");
            ViewBag.StatusDepartment = new SelectList(db.StatusDepartments, "StatusName", "StatusName");
            ViewBag.WeekdaysTimeWork = new SelectList(db.Schedules.Select(s => s.WeekdaysTimeWork).Distinct());
            ViewBag.SaturdayTimeWork = new SelectList(db.Schedules.Select(s => s.SaturdayTimeWork).Distinct());
            ViewBag.SundayTimeWork = new SelectList(db.Schedules.Select(s => s.SundayTimeWork).Distinct());
            return View();
        }

        // POST: Departments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ViewDepartment viewDepartment)
        {
            Department department = ValidationCheckDepartment(viewDepartment);
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
                db.SaveChanges();

                string city = "";
                using (CourseContext context = new CourseContext())
                {
                    city = context.Departments.FirstOrDefault(c => c.Id == department.Id).City.Name;
                }

                    News news = new News
                    {
                        DateCreate = DateTime.Now,
                        Title = $"Открытие нового отделения в г. {city}",
                        Content = $"В г. {city} открыто новое отделение № {department.Id} по адресу {viewDepartment.Adress}. Отделение работает " +
                        $"Пн-Пт: {viewDepartment.WeekdaysTimeWork}, Сб: {viewDepartment.SaturdayTimeWork}, " +
                        $"Вс: {viewDepartment.SundayTimeWork}."
                    };

                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", department.CityId);
            return View(department);
        }

        private Department ValidationCheckDepartment(ViewDepartment viewDepartment)
        {
            var cityId = Convert.ToInt32(viewDepartment.City);
            var statusDepartmentId = db.StatusDepartments.FirstOrDefault(sd => sd.StatusName == viewDepartment.StatusDepartment).Id;
            int scheduleId = db.Schedules.Where(s => s.WeekdaysWork == viewDepartment.WeekdaysWork && s.WeekdaysTimeWork == viewDepartment.WeekdaysTimeWork &&
                             s.SaturdayTimeWork == viewDepartment.SaturdayTimeWork && s.SaturdayWork == s.SaturdayWork && s.SundayTimeWork == viewDepartment.SundayTimeWork
                             && s.SundayWork == viewDepartment.SundayWork).Select(s => s.Id).FirstOrDefault();
            Schedule schedule;
            if (scheduleId == 0)
            {
                schedule = new Schedule
                {
                    WeekdaysWork = viewDepartment.WeekdaysWork,
                    WeekdaysTimeWork = viewDepartment.WeekdaysTimeWork,
                    SundayWork = viewDepartment.SundayWork,
                    SundayTimeWork = viewDepartment.SundayTimeWork,
                    SaturdayWork = viewDepartment.SaturdayWork,
                    SaturdayTimeWork = viewDepartment.SaturdayTimeWork
                };
                db.Schedules.Add(schedule);
                db.SaveChanges();
                scheduleId = schedule.Id;
            }
            Department department = new Department
            {
                Adress = viewDepartment.Adress,
                CityId = cityId,
                MaxWeight = viewDepartment.MaxWeight,
                StatusDepartmentId = statusDepartmentId,
                ScheduleId = scheduleId
            };
            return department;
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            ViewBag.City = new SelectList(db.Cities.Where(c => c.RegionId == department.City.RegionId), "Id", "Name", department.CityId);
            ViewBag.StatusDepartment = new SelectList(db.StatusDepartments, "StatusName", "StatusName", department.StatusDepartment.StatusName);
            ViewBag.Region = new SelectList(db.Regions, "Id", "RegionName", department.City.RegionId);
            ViewBag.WeekdaysTimeWork = new SelectList(db.Schedules.Select(s => s.WeekdaysTimeWork).Distinct(), department.Schedule.WeekdaysTimeWork);
            ViewBag.SaturdayTimeWork = new SelectList(db.Schedules.Select(s => s.SaturdayTimeWork).Distinct(), department.Schedule.SaturdayTimeWork);
            ViewBag.SundayTimeWork = new SelectList(db.Schedules.Select(s => s.SundayTimeWork).Distinct(), department.Schedule.SundayTimeWork);

            ViewDepartment viewDepartment = new ViewDepartment
            {
                Id = department.Id,
                Adress = department.Adress,
                City = department.City.Name,
                MaxWeight = department.MaxWeight,
                StatusDepartment = department.StatusDepartment.StatusName,
                SaturdayTimeWork = department.Schedule.SaturdayTimeWork,
                SaturdayWork = department.Schedule.SaturdayWork,
                SundayTimeWork = department.Schedule.SundayTimeWork,
                SundayWork = department.Schedule.SundayWork,
                WeekdaysTimeWork = department.Schedule.WeekdaysTimeWork,
                WeekdaysWork = department.Schedule.WeekdaysWork
            };

            return View(viewDepartment);
        }

        // POST: Departments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewDepartment viewDepartment)
        {
            int depExistingStatusId = 1;
            string depExistingAdress = "";
            int depExistingScheduleId = 1;
            string city = "";
            int newDepId = 1;
            string newDepAdress = "";
            using (CourseContext context = new CourseContext())
            {
                var depExisting = context.Departments.FirstOrDefault(d => d.Id == viewDepartment.Id);
                depExistingStatusId = depExisting.StatusDepartmentId;
                depExistingAdress = depExisting.Adress;
                depExistingScheduleId = depExisting.ScheduleId;
                city = depExisting.City.Name;
                var newDep = context.Departments.Where(d => d.CityId == depExisting.CityId);
                newDepId = newDep.FirstOrDefault(i => i.Id != depExisting.Id).Id; 
                newDepAdress = newDep.FirstOrDefault(i => i.Id != depExisting.Id).Adress;
            }

            Department department = ValidationCheckDepartment(viewDepartment);
            department.Id = viewDepartment.Id;

            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();

                News news = new News
                {
                    DateCreate = DateTime.Now,
                    Title = $"Измение в работе отделения № {department.Id} г. {city}",
                    Content = ""
                };

                if (depExistingStatusId != department.StatusDepartmentId)
                {
                    if (department.StatusDepartmentId == 2)
                    {
                        news.Content += "Отделение временно прекращает работу. Просим прощения за временные неудобства. Доставки с этого отделения перенесены в отделение" +
                        $" №{newDepId} по адресу {newDepAdress}.";
                    }
                    else
                        news.Content += $"Отделение возобновляет работу. Мы по прежнему находимя по адресу {depExistingAdress}";
                    db.News.Add(news);
                    db.SaveChanges();
                }
                else
                {
                    if (department.StatusDepartmentId != 2)
                    {
                        bool toSave = false;
                        if (depExistingScheduleId != department.ScheduleId)
                        {
                            news.Content += "Изменился график работы отделения. Отделение с сегодняшнего дня работает " +
                                $"Пн-Пт: {db.Schedules.FirstOrDefault(s => s.Id == department.ScheduleId).WeekdaysTimeWork}, " +
                                $"Сб: {db.Schedules.FirstOrDefault(s => s.Id == department.ScheduleId).SaturdayTimeWork}, " +
                                $"Вс: {db.Schedules.FirstOrDefault(s => s.Id == department.ScheduleId).SundayTimeWork}.";
                            toSave = true;
                        }
                        if (depExistingAdress != department.Adress)
                        {
                            news.Content += $"Изменился адрес отделения. Теперь мы находимся по адресу {department.Adress}.";
                            toSave = true;
                        }
                        if (toSave)
                        {
                            db.News.Add(news);
                        }
                        db.SaveChanges();
                    }

                }
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", department.City);
            return View(department);
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
