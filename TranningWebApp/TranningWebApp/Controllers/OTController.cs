using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TmsWebApp.Common;
using TmsWebApp.HelpingUtilities;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;
using TranningWebApp.Resource;

namespace TmsWebApp.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin")]
    public class OTController : BaseController
    {
        // GET: OT
        public ActionResult Index(string sortOrder, string filter, string archived, int page = 1)
        {

            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter;
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;

            ViewBag.CurrentSort = sortOrder;

            IEnumerable<orientation_training> ots;
            var repository = new OTRepository();

            if (string.IsNullOrEmpty(filter))
            {
                ots = repository.Get();
            }
            else
            {
                ots = repository.Get().Where(x => x.Subject.Contains(filter));
            }
            //Sorting order
            ots = ots.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = ots.Count();

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

            }
            else
            {
                var otRepo = new OTRepository();
                otModel = otRepo.GetByRowId(id.Value);
            }
            var cities = new CityRepository().Get().Distinct().Select(x =>
                new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            ViewBag.genderdd = new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
                };
            return View(otModel);
        }
        [HttpPost]
        public ActionResult Edit(orientation_training profile)
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
            ot.ContactPersonName = profile.ContactPersonName;
            ot.ContactPersonPhone = profile.ContactPersonPhone;
         
            if (profile.Id == 0)
            {
                otRepo.Post(ot);
            }
            else
                otRepo.Put(ot.Id, ot);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var repository = new OTRepository();
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}