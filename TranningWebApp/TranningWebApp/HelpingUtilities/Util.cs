using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TmsWebApp.HelpingUtilities
{
    public class Util
    {
         
        public static T CreateController<T>(RouteData routeData = null)
            where T : Controller, new()
        {
            // create a disconnected controller instance
            T controller = new T();

            // get context wrapper from HttpContext if available
            HttpContextBase wrapper;
            if (System.Web.HttpContext.Current != null)
                wrapper = new HttpContextWrapper(System.Web.HttpContext.Current);
            else
                throw new InvalidOperationException(
                    "Can't create Controller Context if no " +
                    "active HttpContext instance is available.");

            if (routeData == null)
                routeData = new RouteData();

            // add the controller routing if not existing
            if (!routeData.Values.ContainsKey("controller") &&
                !routeData.Values.ContainsKey("Controller"))
                routeData.Values.Add("controller",
                    controller.GetType()
                        .Name.ToLower().Replace("controller", ""));

            controller.ControllerContext = new ControllerContext(wrapper, routeData, controller);
            return controller;
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static string DateConversion(string dateConv, string calendar, string dateLangCulture,bool hijriformat = false)
        {
            try
            {
                dateLangCulture = dateLangCulture.ToLower();
                // We can't have the hijri date writen in English. We will get a runtime error - LAITH - 11/13/2005 1:01:45 PM -

                if (calendar == "Hijri" && dateLangCulture.StartsWith("en-"))
                {
                    dateLangCulture = "ar-sa";
                }

                // Set the date time format to the given culture - LAITH - 11/13/2005 1:04:22 PM -
                var dtFormat = new System.Globalization.CultureInfo(dateLangCulture, false).DateTimeFormat;

                // Set the calendar property of the date time format to the given calendar - LAITH - 11/13/2005 1:04:52 PM -
                switch (calendar)
                {
                    case "Hijri":
                        dtFormat.Calendar = new System.Globalization.HijriCalendar();

                        DateTime dt = DateTime.ParseExact(dateConv, dateConv.Contains("-") ? "d-M-yyyy" : "dd/MM/yyyy",
                            new CultureInfo("en-US"));

                        if (!hijriformat)
                            return dt.Date.ToString("yyyy/MM/dd", dtFormat);
                        else
                            
                        return dt.Date.ToString("dd MMM yyyy", dtFormat);

                    case "Gregorian":
                        DateTime tempDate;
                        CultureInfo arCi = new CultureInfo("ar-SA");
                        tempDate = DateTime.ParseExact(dateConv, dateConv.Contains("-") ? "d-M-yyyy" : "dd/MM/yyyy",
                            arCi.DateTimeFormat, DateTimeStyles.AllowInnerWhite);
                        return tempDate.Date.ToString("dd/MM/yyyy");

                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
    }
}
