using CourseChentsov.Helpers;
using CourseChentsov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;

namespace CourseChentsov.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            using (CourseContext db = new CourseContext())
            {
                try
                {
                    string pass = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", String.Empty).ToLower();
                    UserLogin user = (from l in db.UserLogins
                                 where l.Email == username && l.PasswordHash == pass
                                 select l).FirstOrDefault();
                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public override MembershipUser GetUser(string userEmail, bool userIsOnline)
        {
            try
            {
                using (CourseContext db = new CourseContext())
                {
                    UserLogin login = (from l in db.UserLogins
                                   where l.Email == userEmail
                                   select l).FirstOrDefault();
                    if (login == null)
                    {
                        MembershipUser membershipUser = new MembershipUser("CustomMembershipProvider", userEmail, null, null, null, null, false, false,
                            DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                        return membershipUser;
                    }
                }
            }
            catch { return null; }
            return null;
        }

        public MembershipUser CreateUser(string userEmail, string password)
        {
            MembershipUser membershipUser = GetUser(userEmail, false);
            if (membershipUser != null)
            {
                try
                {
                    string pass = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", String.Empty).ToLower();
                    using (CourseContext db = new CourseContext())
                    {
                        db.UserLogins.Add(new UserLogin { Email = userEmail, PasswordHash = pass, RoleId = 3 });
                        db.SaveChanges();
                        //membershipUser = GetUser(userEmail, false);
                        return membershipUser; // получаю MembershipUser
                    }
                }
                catch
                {
                    return null;
                }
            }
            return null;
            /*
            MembershipUser membershipUser = CreateUser(username, password);
            if (membershipUser == null)
                status = MembershipCreateStatus.DuplicateEmail;
            else
                status = MembershipCreateStatus.Success;
            return membershipUser;*/
        }

        public bool ConfirmedEmail(string userEmail)
        {
            bool isConfirmedEmail = false;
            using (CourseContext db = new CourseContext())
            {
                try
                {
                    UserLogin user = (from l in db.UserLogins
                                      where l.Email == userEmail
                                      select l).FirstOrDefault();
                    if (user != null && user.ConfirmedEmail)
                    {
                        isConfirmedEmail = true;
                    }
                }
                catch
                {
                    isConfirmedEmail = false;
                }
            }
            return isConfirmedEmail;
        }

        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
    }
}