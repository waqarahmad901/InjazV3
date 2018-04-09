using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using TmsWebApp.Common;
using TmsWebApp.HelpingUtilities;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Models;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;

namespace TmsWebApp.Controllers
{
   // [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult ChangeLanguage(string lan)
        {
            HttpCookie myCookie = new HttpCookie("lan");
            myCookie.Value = lan;
            myCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(myCookie); 
            if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.PathAndQuery);
            return View("Index");
        }

        [HttpPost]
        public ActionResult CheckEmail(string email)
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUploadedFile(string type)
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here 
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory =
                            new DirectoryInfo(string.Format("{0}Images\\Session\\SessionPhoto", Server.MapPath(@"\")));
                        if (type != "sessionphoto")
                            originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Session\\SessionEvaluation",
                                Server.MapPath(@"\")));

                        string pathString = originalDirectory.ToString();

                        bool isExists = System.IO.Directory.Exists(pathString);

                        if (!isExists)
                            System.IO.Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, fName);
                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception )
            {
                isSavedSuccessfully = false;
            }


            if (isSavedSuccessfully)
            {
                return Json(new {Message = fName});
            }
            else
            {
                return Json(new {Message = "Error in saving file"});
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AuthorizeUser(AccessLevel = "SuperAdmin")]
        public ActionResult DashBoard()
        {

            DashBoardRepositry dbRepository = new DashBoardRepositry();
            var model = dbRepository.GetDashBoardData();

            return View(model);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConvertDateCalendar(string dateConv, string calendar, string dateLangCulture)
        {
           
                return Json(Util.DateConversion(dateConv, calendar, dateLangCulture), JsonRequestBehavior.AllowGet);
           }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConvertDateCalendarList(string dateConv, string calendar, string dateLangCulture, int days)
        {
            DashBoardClanderModel clander = new DashBoardClanderModel();
            List<string> list = new List<string>();
            DateTime currDate = DateTime.Parse(dateConv);
            for(int i =0; i < days; i++)
            {
                DateTime tempdate = currDate.AddDays(i);
                if (i == 0)
                    clander.startdate = Util.DateConversion(tempdate.ToShortDateString(), calendar, dateLangCulture,true);
                if (i == days - 1)
                    clander.enddate = Util.DateConversion(tempdate.ToShortDateString(), calendar, dateLangCulture, true);


                list.Add(Util.DateConversion(tempdate.ToShortDateString(), calendar, dateLangCulture).Split('/')[2]);

            }
            clander.dates = list;


            return Json(clander, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCountryIso(int id)
        {
            var repo = new CountryRepository();
            return Json(repo.Get(id).Iso2,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDistrict(string id)
        {
            var repo = new CityRepository();
            return Json(repo.GetByCity(id).Region, JsonRequestBehavior.AllowGet);
        }

        public ActionResult validatevolmobile(string VolunteerMobile)
       {
            var repo = new VolunteerRepository();
            return Json(!repo.ValidateMobileNumber(VolunteerMobile), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChanngeProfile()
        {
            var cu = Session["user"] as ContextUser;
            if (cu != null)
            {
                if (cu.EnumRole == EnumUserRole.Funder)
                {
                    var funderid = new FunderRepository().GetByUserId(cu.OUser.Id).RowGUID;
                    return RedirectToAction("Edit", "Funder",new {id = funderid });
                }
                if (cu.EnumRole == EnumUserRole.SuperAdmin || cu.EnumRole == EnumUserRole.Approver1 || cu.EnumRole == EnumUserRole.Approver2 || cu.EnumRole == EnumUserRole.Approver3)
                {
                    var userid = new AccountRepository().Get(cu.OUser.Id).RowGuid;
                    return RedirectToAction("EditUser", "Account", new { id = userid });
                }
                if (cu.EnumRole == EnumUserRole.Coordinator)
                    return RedirectToAction("CoordinatorProfile", "Coordinator");
                if (cu.EnumRole == EnumUserRole.Participant)
                    return RedirectToAction("ParticipantProfile", "Participant");
                if (cu.EnumRole == EnumUserRole.Volunteer)
                    return RedirectToAction("VolunteerProfile", "Volunteer",new {editprofile = true});
            }
            return null;
        }
    }
}
