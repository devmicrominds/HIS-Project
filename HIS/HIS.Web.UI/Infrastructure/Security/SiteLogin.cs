using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HIS.Web.UI.Security
{
    public class SiteLogin
    {
        public static bool PerformAuthentication(string userName, string password)
        {
            if (Membership.ValidateUser(userName, password))
            {
                //RedirectToDefaultPage();
                return true;
            }
            return false;
        }

        public static void RedirectToDefaultPage(string userName)
        {

            FormsAuthentication.RedirectFromLoginPage(userName, false, "/");
        }

        /// <summary>
        /// Redirects the current user based on role
        /// </summary>
        public static void RedirectToDefaultPage()
        {
            HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl);
        }

        public static void RedirectToLoginPage()
        {
            HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
        }

        public static void LogOff()
        {
            // Put user code to initialize the page here
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
            //Set the current user as null
            RemoveCookies();
            HttpContext.Current.Cache.Remove("SiteMap");
            HttpContext.Current.User = null;
            RedirectToLoginPage();
        }

        private static void RemoveCookies()
        {
            int cookieCount = HttpContext.Current.Request.Cookies.Count - 1;
            for (int i = 0; i < cookieCount; i++)
            {
                string name = HttpContext.Current.Request.Cookies[i].Name;
                var cookie = new HttpCookie(name);
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);

            }
        }
    }
}