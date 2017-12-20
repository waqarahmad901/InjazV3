using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TmsWebApp.Common;
using TranningWebApp.Common;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;

namespace TranningWebApp.Controllers
{
    public class EvaluationResultController : BaseController
    {
        // GET: EvaluationResult
        public ActionResult Index(string sortOrder, string filter, string archived, int page = 1, Guid? archive = null)
        {
            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter;
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;

            ViewBag.CurrentSort = sortOrder;

            var cu = Session["user"] as ContextUser;
            IEnumerable<school> Profile;
            var repository = new CoordinatorRepository();
            if (archive != null)
            {
                var oCoordinator = repository.GetByRowId(archive.Value);
                oCoordinator.IsActive = !oCoordinator.IsActive;
                repository.Put(oCoordinator.Id, oCoordinator);
            }
           
                Profile = repository.GetAllSchools();
           
            //Sorting order
            Profile = Profile.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = Profile.Count();

            return View(Profile.ToPagedList(page, pageSize));
        }

        public ActionResult SessionDetails(Guid id)
        {
         
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;
             
            IEnumerable<session> session;
            var repository = new SessionRepository();
            repository.SetOccuredStatus();
             
            var cu = Session["user"] as ContextUser;
            session = repository.Get().Where(x => x.school != null && x.school.RowGuid == id && (x.Status == "Approved" || x.Status == "Occured"));
            //Sorting order
            session = session.OrderByDescending(x => x.CreatedAt);
            ViewBag.schoolname = session.First().school.SchoolName;
            ViewBag.Count = session.Count();

            return View(session);
        }

        public ActionResult ParticipantDetail(Guid id)
        {
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;

           
            var repository = new SessionRepository();
            repository.SetOccuredStatus();

            var cu = Session["user"] as ContextUser;
            var session = repository.Get().First(x => x.RowGUID == id);
            var participants = session.session_participant.Select(x => x.participant_profile).ToList();
            //Sorting order
            string eveCat = session.StudentEvaluationCatagory;
            var forms = new EvaluationFormRepository().Get().Where(x => x.Catagory == eveCat).ToList();
            var lkEvaluationForm = forms.FirstOrDefault(x => x.SubCatagory == "pre");
            if (lkEvaluationForm != null)
                ViewBag.preform = lkEvaluationForm.FormPath;
            var evaluationForm = forms.FirstOrDefault(x => x.SubCatagory == "post");
            if (evaluationForm != null)
                ViewBag.postform = evaluationForm.FormPath;
            ViewBag.sessionid = session.Id;
            ViewBag.sessionname = session.ProgramName;
            ViewBag.schoolname = session.school.SchoolName;
            return View(participants);
        }
    }
}