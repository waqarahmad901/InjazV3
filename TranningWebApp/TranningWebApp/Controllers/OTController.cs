using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TmsWebApp.Common;
using TmsWebApp.HelpingUtilities;
using TmsWebApp.Models;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;
using TranningWebApp.Resource;

namespace TmsWebApp.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin,Approver1,Approver2")]
    public class OTController : BaseController
    {
        // GET: OT
        public ActionResult Index(string sortOrder, string filter, string archived, int page = 1, Guid? archive = null)
        {

            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter;
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;
            var repository = new OTRepository();
            ViewBag.CurrentSort = sortOrder;
            if (archive != null)
            {
                var ot = repository.GetByRowId(archive.Value);
                ot.IsActive = !ot.IsActive;
                repository.Put(ot.Id, ot);
            }
            IEnumerable<orientation_training> ots;
          

            if (string.IsNullOrEmpty(filter))
            {
                ots = repository.Get();
            }
            else
            {
               
                ots = repository.Get().Where(x => x.Subject != null && x.Subject.ToLower().Contains(filter.ToLower()));
            }
            //Sorting order
            ots = ots.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = ots.Count();

            foreach (var item in ots)
            {
                item.IsOTOccured = repository.IsOtOccures(item);
            }


            return View(ots.ToPagedList(page, pageSize));
        }

        public ActionResult Edit(Guid? id)
        {
            SelectListItem defaultselect = new SelectListItem { Text = General.Select, Value = "0" };

            var school = new SchoolRepository().Get().Select(x =>
                         new SelectListItem { Text = x.SchoolName, Value = x.Id + "" }).ToList();
            school.Insert(0, defaultselect);
            ViewBag.school = school;

            var session = new SessionRepository().Get().Select(x =>
             new SelectListItem { Text = x.ProgramName, Value = x.Id + "" }).ToList();
            session.Insert(0, defaultselect);
            ViewBag.session = session;

            orientation_training otModel = null;
            if (id == null)
            {
                otModel = new orientation_training();
                otModel.OTDateTime = DateTime.Now.AddMonths(1);
                otModel.OTDateEnd = DateTime.Now.AddMonths(1).AddDays(3);

            }
            else
            {
                var otRepo = new OTRepository();
                otModel = otRepo.GetByRowId(id.Value);
            }
            if (otModel.Region == null)
            {
                var cities = new CityRepository().Get().Distinct().Where(x => x.Region == "Makkah Region").Select(x =>
                       new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
                ViewBag.citiesdd = cities;
            }
            else
            {
                var cities = new CityRepository().Get().Distinct().Where(x => x.Region == otModel.Region).Select(x =>
                       new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
                ViewBag.citiesdd = cities;
            }
            ViewBag.genderdd = new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
                };
            var distict = new CityRepository().Get().GroupBy(x => x.Region).Select(x => x.First()).Select(x =>
        new SelectListItem { Text = x.Region + " (" + x.Region_ar + ")", Value = x.Region + "", Selected = x.Region == "Makkah Region" }).ToList();
            ViewBag.distictdd = distict;

            int[] otVolunteers = !string.IsNullOrEmpty(otModel.VolunteersIds) ? otModel.VolunteersIds.Split(',').Select(x=>int.Parse(x)).ToArray() : new int[] { };
            int[] otSchools = !string.IsNullOrEmpty(otModel.SchoolIds ) ? otModel.SchoolIds.Split(',').Select(x => int.Parse(x)).ToArray() : new int[] { };

            string city = string.IsNullOrEmpty(otModel.City) ? "Jeddah" : otModel.City;
            string gender = string.IsNullOrEmpty(otModel.Gender) ? "Male" : otModel.Gender;

            otModel.Volunteers = new VolunteerRepository().GetApprovedVolunteer().Where(x=>x.City == city && x.Gender == gender).Select(x =>
            new SelectListItem { Text = x.VolunteerName, Value = x.Id + "",Selected = otVolunteers.Contains(x.Id) }).ToList();

            otModel.Schools = new SchoolRepository().GetByFilters(otModel.City, otModel.Gender).Select(x =>
            new SelectListItem { Text = x.SchoolName, Value = x.Id + "", Selected = otSchools.Contains(x.Id) }).ToList();



            return View(otModel);
        }

        public ActionResult GetSchoolsVolunteerByFilter(string city, string gender)
        {
            var school = new SchoolRepository().GetByFilters(city, gender).Select(x =>
             new SelectListItem { Text = x.SchoolName, Value = x.Id + "" }).ToList();
            var volunteer = new VolunteerRepository().GetApprovedVolunteer().Where(x => x.City == city && x.Gender == gender).Select(x =>
             new SelectListItem { Text = x.VolunteerName, Value = x.Id + "" }).ToList();
            var result = new { volunteers = volunteer, schools = school };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(orientation_training profile,FormCollection collection)
        {
            var otRepo = new OTRepository();
            orientation_training ot = null;
            var cu = Session["user"] as ContextUser;
            if (profile.Id == 0)
            {
                ot = new orientation_training();
                ot.RowGuid = Guid.NewGuid();
                ot.CreatedAt = DateTime.Now;
                ot.CreatedBy = cu.OUser.Id;
                ot.IsActive = true;

                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel model = new EmailTemplateModel { Title = "OT is created", RedirectUrl = url };
                string body = Util.RenderViewToString(bogusController.ControllerContext, "OTCreated", model);
                EmailSender.SendSupportEmail(body, cu.OUser.Email);
            }
            else
            {
                ot = otRepo.Get(profile.Id);
                ot.UpdatedAt = DateTime.Now;
                ot.UpdatedBy = cu.OUser.Id;
            }


            ot.Location = profile.Location;
            ot.Subject = profile.Subject;
            ot.OTDateTime = profile.OTDateTime;
            ot.OTDateEnd = profile.OTDateEnd;
            ot.Region = profile.Region;
            ot.Gender = profile.Gender;
            ot.City = profile.City;
            ot.SchoolIds = profile.Schools != null ? string.Join(",", profile.Schools.Where(x => x.Selected == true).Select(x => x.Value).ToArray()) : default(string);
            ot.VolunteersIds = profile.Volunteers != null ? string.Join(",", profile.Volunteers.Where(x => x.Selected == true).Select(x => x.Value).ToArray()) : default(string);
            if (!string.IsNullOrEmpty(ot.VolunteersIds))
            {
                foreach (var item in ot.VolunteersIds.Split(','))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var vol = new VolunteerRepository().Get(int.Parse(item));
                        OTLinkWithVolunteerEmail(vol, ot.Subject);
                    }
                }
            }
            if (!string.IsNullOrEmpty(ot.SchoolIds))
            {
                foreach (var item in ot.SchoolIds.Split(','))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var sch = new SchoolRepository().Get(int.Parse(item));
                        OTLinkWithSchoolEmail(sch, ot.Subject);
                    }

                }
            }
            ot.ContactPersonName = profile.ContactPersonName;
            ot.ContactPersonPhone = profile.ContactPersonPhone;

            int days = (profile.OTDateEnd - profile.OTDateTime).Value.Days;

            if (ot.Id > 0)
                otRepo.RemoveAllTimes(ot.Id);
            for (int i = 0; i <= days; i++)
            {
                DateTime fromTime = DateTime.ParseExact(collection["proFromTime_" + i].ToString(), "hh:mm tt", CultureInfo.InvariantCulture);
                DateTime toTime = DateTime.ParseExact(collection["proToTime_" + i].ToString(), "hh:mm tt", CultureInfo.InvariantCulture);
                ot.ot_time.Add(
                    new ot_time
                    {
                        IsActive = collection["procheck_" + i] == "on" ? true : false,
                        ActualStartTime = fromTime.TimeOfDay,
                        ActualEndTime = toTime.TimeOfDay,
                    }
                    );
            }


            if (profile.Id == 0)
            {
                otRepo.Post(ot);
            }
            else
                otRepo.Put(ot.Id, ot);
            return RedirectToAction("Index");
        }

        private void OTLinkWithSchoolEmail(school sch, string OTName)
        {
            string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel model = new EmailTemplateModel { Title = "OT link with school", RedirectUrl = url, VolunteerName = sch.coordinator_profile.First().CoordinatorName, OTName = OTName };
            string body = Util.RenderViewToString(bogusController.ControllerContext, "OTLink", model);
            EmailSender.SendSupportEmail(body, sch.coordinator_profile.First().CoordinatorEmail);
        }

        private static void OTLinkWithVolunteerEmail(volunteer_profile volunteer,string OTName)
        {
           
            string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel model = new EmailTemplateModel { Title = "OT link with volunteer", RedirectUrl = url, VolunteerName = volunteer.VolunteerName ,OTName = OTName};
            string body = Util.RenderViewToString(bogusController.ControllerContext, "OTLink", model);
            EmailSender.SendSupportEmail(body, volunteer.VolunteerEmail);
        }

        public ActionResult Delete(int id)
        {
            var repository = new OTRepository();
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}