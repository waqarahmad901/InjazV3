using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace TranningWebApp.Controllers
{
    public class BaseOnlyArabicController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = "ar";
            var userCookie = new HttpCookie("lan", "ar");
            userCookie.Expires.AddDays(365);
            Request.Cookies.Add(userCookie);
            Response.Cookies.Add(userCookie);
            // Modify current thread's cultures            
            // Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
            ViewBag.IsLanAr = true;
            return base.BeginExecuteCore(callback, state);
        }
    }
}