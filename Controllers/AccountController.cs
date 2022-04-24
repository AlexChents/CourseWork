using CourseChentsov.Helpers;
using CourseChentsov.Models;
using CourseChentsov.Models.ViewModel;
using CourseChentsov.Providers;
using Microsoft.AspNet.Identity;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CourseChentsov.Controllers
{
    public class AccountController : Controller
    { 
        private CourseContext db = new CourseContext();

        private string CreateRefreshToken()
        {
            byte[] refreshToken = new byte[64];

            SecureRandom secureRandom = new SecureRandom();
            secureRandom.NextBytes(refreshToken);

            return Base64.ToBase64String(refreshToken);
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", null);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserViewLogin model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    // создаем файл Cookie проверки подлинности для полученного имени пользователя
                    FormsAuthentication.SetAuthCookie(model.Email, false); // файл Cookie не сохраняется между сеансами браузера
                    if (Url.IsLocalUrl(returnUrl)) // защита от ложной маршрутизации?
                    {
                        return Redirect(returnUrl);
                    }
                    else if (((CustomMembershipProvider)Membership.Provider).ConfirmedEmail(model.Email))
                    {
                        return RedirectToAction("Index", "Packages");
                    }
                    else
                    {
                        return RedirectToAction("Confirm", new { Email = model.Email });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", null);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserViewRegister user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MembershipUser membership = ((CustomMembershipProvider)Membership.Provider).CreateUser(user.Email, user.Password);
                if (membership != null)
                {
                    // создаем файл Cookie проверки подлинности для полученного имени пользователя
                    FormsAuthentication.SetAuthCookie(user.Email, false); // файл Cookie не сохраняется между сеансами браузера
                    if (Url.IsLocalUrl(returnUrl)) // защита от ложной маршрутизации?
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return SendMail(user.Email, "Подтверждение почты", "ConfirmEmail");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином существует.");
                }
            }
            return View(user);
        }

        [AttrConfirmEmailIsTrue]
        [HttpPost]
        public ActionResult SendMail(string userEmail, string confirmationEmail, string action)
        {
            // наш email с заголовком письма
            MailAddress from = new MailAddress("user@gmail.com");
            // кому отправляем
            MailAddress to = new MailAddress(userEmail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = confirmationEmail;
            // текст письма - включаем в него ссылку
            var ul = db.UserLogins.FirstOrDefault(u => u.Email == userEmail);
            string token = CreateRefreshToken();
            ul.Token = token;
            db.SaveChanges();
            if (action == "ResetPassword")
            {
                m.Body = string.Format("Для сброса пароля, перейдите по ссылке: " +
                                            "<a href=\"{0}\" title=\"Сброс пароля\">{0}</a>",
                                Url.Action(action, "Account", new { Token = token, Email = userEmail }, Request.Url.Scheme));
            }
            else if(action == "ConfirmEmail")
            {
                m.Body = string.Format("Для завершения регистрации перейдите по ссылке: " +
                            "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>",
                    Url.Action(action, "Account", new { Token = token, Email = userEmail }, Request.Url.Scheme));
            }
            m.IsBodyHtml = true;
            // адрес smtp-сервера, с которого мы и будем отправлять письмо
            SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new System.Net.NetworkCredential("user@gmail.com", "password");
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Timeout = 20000;
            try
            {
                smtp.Send(m);
            }
            catch (Exception ex)
            {
                return new HtmlResult(ex.Message);
            }
            if (action == "ResetPassword")
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            else if (action == "ConfirmEmail")
                return RedirectToAction("Confirm", new { Email = userEmail });
            else
               return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Confirm(string Email, bool ConfirmedEmail = false)
        {
            if (ConfirmedEmail)
                ViewBag.Message = "Спасибо! Ваша почта " + Email + " подтверждена.";
            else
                ViewBag.Message = "На почтовый адрес " + Email + " Вам высланы дальнейшие инструкции по завершению регистрации.";
            ViewBag.ConfirmedEmail = ConfirmedEmail;
            return View();
        }

        [Authorize]
        public ActionResult ConfirmEmail(string Token, string Email)
        {
            UserLogin userLogin = db.UserLogins.FirstOrDefault(ul => ul.Email == Email);
            if (userLogin != null && userLogin.Token == Token)
            {
                if (userLogin.Email == Email)
                {
                    userLogin.ConfirmedEmail = true;
                    userLogin.Token = null;
                    db.Entry(userLogin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Confirm", "Account", new { Email = userLogin.Email, ConfirmedEmail = userLogin.ConfirmedEmail });
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = userLogin.Email, ConfirmedEmail = false });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { Email = "" });
            }
        }

        [Authorize]
        public ActionResult ContactData(string returnUrl)
        {
            string login = User.Identity.Name;
            ViewBag.ReturnUrl = returnUrl;
            var idUser = db.Users.FirstOrDefault(u => u.UserLogin.Email == login);
            if (idUser == null)
            {
                ViewBag.RegionId = new SelectList(db.Regions, "Id", "RegionName");
                ViewBag.CityId = new SelectList(db.Cities.Where(c => c.RegionId == 1), "Id", "Name");
                ViewBag.DepartmentId = new SelectList(db.Departments.Where(d => d.CityId == db.Cities.FirstOrDefault(c => c.RegionId == 1).Id && d.StatusDepartmentId == 1)
                    .Select(d => d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)"));
               return View();
            }
            else
            {
                var myDepartment = db.Users.FirstOrDefault(md => md.Id == idUser.Id);
                int myDepartmentId;
                if (myDepartment != null)
                    myDepartmentId = (int)myDepartment.DepartmentId;
                else
                    myDepartmentId = 1;

                int myCity = db.Departments.FirstOrDefault(mc => mc.Id == myDepartmentId).CityId;
                int myRegion = db.Cities.FirstOrDefault(mr => mr.Id == myCity).RegionId;

                ViewBag.CityId = new SelectList(db.Cities.Where(c => c.RegionId == myRegion), "Id", "Name", myCity);
                var contactPerson = db.Users.Where(u => u.Id == idUser.Id).Select(u => u).FirstOrDefault();
                ViewBag.RegionId = new SelectList(db.Regions, "Id", "RegionName", myRegion);
                string adress = db.Departments.FirstOrDefault(d => d.Id == myDepartmentId).Adress;
                double weight = db.Departments.FirstOrDefault(d => d.Id == myDepartmentId).MaxWeight;
                ViewBag.DepartmentId = new SelectList(db.Departments.Where(d => d.CityId == myCity)
                                                            .Select(d => d.Id + ", " + d.Adress + ", (отделение до " + d.MaxWeight + " кг.)"),
                                                            myDepartmentId + ", " + adress + ", (отделение до " + weight + " кг.)");

                ContactData contactData = new ContactData
                {
                    FirstName = contactPerson.FirstName,
                    LastName = contactPerson.LastName,
                    ThirdName = contactPerson.ThirdName,
                    PhoneNumber = contactPerson.PhoneNumber,
                    SenderName = contactPerson.SenderName
                };


                return View(contactData);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactData(ContactData model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Url.IsLocalUrl(returnUrl)) // защита от ложной маршрутизации?
                {
                    return Redirect(returnUrl);
                }
                int idUserLogin = db.UserLogins.FirstOrDefault(ul => ul.Email == User.Identity.Name).Id;

                var user = db.Users.FirstOrDefault(u => u.Id == idUserLogin);
                int idDep = Convert.ToInt32(model.DepartmentId.Substring(0, model.DepartmentId.IndexOf(',')));
                if (user == null)
                {
                    User newUser = new User
                    {
                        DepartmentId = idDep,
                        FirstName = model.FirstName,
                        Id = idUserLogin,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        SenderName = model.SenderName,
                        ThirdName = model.ThirdName
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                }
                else
                {
                    User existingUser = db.Users.Find(idUserLogin);
                    existingUser.DepartmentId = idDep;
                    existingUser.FirstName = model.FirstName;
                    existingUser.LastName = model.LastName;
                    existingUser.PhoneNumber = model.PhoneNumber;
                    existingUser.SenderName = model.SenderName;
                    existingUser.ThirdName = model.ThirdName;
                    db.Entry(existingUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Packages");
            }
            return View(model);
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.UserLogins.FirstOrDefault(ul => ul.Email == model.Email);
                if (user == null || !user.ConfirmedEmail)
                {
                    // Не показывать, что пользователь не существует или не подтвержден
                    return View("ForgotPasswordConfirmation");
                }
                SendMail(model.Email, "Сброс пароля", "ResetPassword");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string Token, string Email)
        {
            UserLogin userLogin = db.UserLogins.FirstOrDefault(ul => ul.Email == Email);
            if (userLogin != null && userLogin.Token == Token)
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserLogin userLogin = db.UserLogins.FirstOrDefault(ul => ul.Email == model.Email);
            if (userLogin != null)
            {
                userLogin.Token = null;
                string pass = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(model.Password))).Replace("-", String.Empty).ToLower();

                userLogin.PasswordHash = pass;
                db.Entry(userLogin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            else
            {
                return RedirectToAction("Index", "Home", new { Email = "" });
            }
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
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