﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace CourseChentsov.Helpers
{
    public class AttrConfirmEmail : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            using (CourseContext db = new CourseContext())
            {
                var isConfirm = db.UserLogins.FirstOrDefault(ul => ul.Email == user.Identity.Name);
                if (user == null || !user.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary {
                            { "controller", "Account" }, { "action", "Login" }
                       });
                }
                else if (!isConfirm.ConfirmedEmail)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary {
                            { "controller", "Account" }, { "action", "Confirm" }, { "email", $"{user.Identity.Name}" }
                       });
                }  
            }
        }
    }
}