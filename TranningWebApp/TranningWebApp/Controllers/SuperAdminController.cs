using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TmsWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TmsWebApp.Controllers
{
    [Authorize (Roles = "SuperAdmin")]
    public class SuperAdminController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Schools(string sortOrder, string filter, string archived, int page = 1, int pageSize = 5)
        {
            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter;
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
             
            ViewBag.CurrentSort = sortOrder;

            IEnumerable<school> schools;
            var repository = new SchoolRepository();

            //filtered or all records
            if (string.IsNullOrEmpty(filter))
            {
                schools = repository.Get();
            }
            else
            {
                schools = repository.Get().Where(x => x.SchoolName.Contains(filter) );
            }
             
            //Sorting order
            schools = schools.OrderBy(x => x.SchoolName);
            ViewBag.Count = schools.Count();
            return View(schools.ToPagedList(page, pageSize));
        }


    }
}