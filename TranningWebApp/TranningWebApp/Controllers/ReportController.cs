using System;
using System.Collections.Generic;
using TranningWebApp.Pdf_Generation;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using TranningWebApp.Repository;
using TranningWebApp.Repository.Lookup;
using TranningWebApp.Models;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using TranningWebApp.Common;

namespace TranningWebApp.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin,Funder")]
    public class ReportController : BaseController
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SchoolRegandNumberOfStudentChart()
        {
            var cities = new CityRepository().Get().Distinct().Select(x =>
                new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            return View();
        }

        public ActionResult SessionParticipantsChart()
        {

            return View();
        }

        [HttpPost]
        public ActionResult GetReportData(string datefrom, string dateto, string city)
        {

            var sp = new ReportRepository().GetSchoolParticipants(datefrom, dateto, city);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetSessionParticipantData(string datefrom, string dateto)
        {

            var sp = new ReportRepository().GetSessionParticipants(datefrom, dateto);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }
        public ActionResult PartnerFundingChart()
        {

            return View();
        }
        [HttpPost]
        public ActionResult GetPartnerFundingData()
        {

            var sp = new ReportRepository().GetPartnerFunding();
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PartnerTypeChart()
        {

            return View();
        }

        public ActionResult GetPartnerTypeData()
        {

            var sp = new ReportRepository().GetPartnerType();
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult VolunteerDetailChart()
        {

            return View();
        }

        public ActionResult GetVolunteerDetailData()
        {

            var sp = new ReportRepository().GetVolunteerDetail();
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TotalSchoolParticipantChart()
        {
            var cities = new CityRepository().Get().Distinct().Select(x =>
                new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            return View();
        }

        [HttpPost]
        public ActionResult GetTotalSchoolParticipantData(string datefrom, string dateto, string city)
        {

            var sp = new ReportRepository().GetTotalSchoolParticipants(datefrom, dateto, city);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TotalSessionInSchoolParticipantChart()
        {
            var cities = new CityRepository().Get().Distinct().Select(x =>
                new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            return View();
        }

        [HttpPost]
        public ActionResult GetSessionInTotalSchoolParticipantData(string datefrom, string dateto, string city)
        {

            var sp = new ReportRepository().GetTotalSessionInSchoolParticipants(datefrom, dateto, city);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TotalCourseChart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetTotalCourseData(string YearSelected)
        {

            var sp = new ReportRepository().GetTotalCourse(YearSelected);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TotalCourseByYearChart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetTotalCourseByYearData(string SelectYear)
        {

            var sp = new ReportRepository().GetTotalCourseByYear(SelectYear);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult StudentsWithVolunteerChart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetStudentsWithVolunteerData(string datefrom, string dateto)
        {

            var sp = new ReportRepository().GetStudentsWithVolunteer(datefrom, dateto);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult NumberOfVolunteerSessionChart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetNumberOfVolunteerSessionData(string datefrom, string dateto)
        {

            var sp = new ReportRepository().GetNumberOfVolunteerSession(datefrom, dateto);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }
        public ActionResult TotalCourseByStageChart()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetTotalCourseByStageData(string datefrom, string dateto)
        {

            var sp = new ReportRepository().GetTotalCourseByStage(datefrom, dateto);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GeneralReport()
        {
            var Values = new GeneralReportModel();
            var cities = new CityRepository().Get().Distinct().Select(x =>
                 new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            return View(Values);
        }

        [HttpPost]
        public ActionResult GetGeneralReportData(string datefrom, string dateto,string city)
        {

            var sp = new ReportRepository().GetGeneralReport(datefrom, dateto,city);
            return Json(sp, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GeneratePdf(string graphString)
        {
            if (string.IsNullOrEmpty(graphString))
                return Json("");

            int index = graphString.IndexOf("}%%");
            var part1 = index < 3 ? "" : graphString.Substring(0, graphString.IndexOf("}%%") + 3);
            graphString = graphString.Replace(part1, "");

            string image = graphString.Replace("data:image/png;base64,", "");

            PdfDataModel model = new PdfDataModel();
            var pdf = new pdfGenerator();

            part1 = part1.Replace("}%%", "").Replace("%%{", "");
            var lines = Regex.Split(part1, "}{");

            var title = lines[0];
            var parameters = lines[1].Split(';');
            var results = lines[2].Split(';'); ;

            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["lan"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                    Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                    null;
            string fontFilePath = Server.MapPath("~/fonts/Arabic Fonts/trado.ttf");
            string pdfPath = Server.MapPath("~/Uploads/pdfs");
            var filePath = pdf.GeneratePdf(image, title, parameters.ToList(), results.ToList(), cultureName, fontFilePath, pdfPath);
            return Json(filePath);
        }

        [HttpGet]
        public ActionResult DownloadPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Json("");

            Response.Headers.Add("content-disposition", "attachment; filename=Report.pdf");
            return File(new FileStream(filePath, FileMode.Open),
                        "application/pdf");
        }


    }
}