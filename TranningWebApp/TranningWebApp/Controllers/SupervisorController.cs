using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PagedList;
using TmsWebApp.Common;
using TmsWebApp.HelpingUtilities;
using TmsWebApp.Models;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;


namespace TmsWebApp.Controllers
{
    [RoutePrefix("Volunteer")]
    [AuthorizeUser(AccessLevel = "SuperAdmin,Approver1,Approver2,Approver3,Volunteer")]
    public class SupervisorController : BaseController
    {
        [Route("Index")]
        public ActionResult Index( string status = "pending", int page = 1, Guid? archive = null,string filter=null)
        {
            IEnumerable<volunteer_profile> volunteers;
            var repository = new VolunteerRepository();

            var cu = Session["user"] as ContextUser;
            volunteers = repository.Get(cu.EnumRole, status,filter).OrderByDescending(x=>x.CreatedAt);
            ViewBag.Count = volunteers.Count();
            
            if (archive != null)
            {
                var volunteer = repository.GetbyGuid(archive.Value);
                volunteer.IsActive = volunteer.IsActive == null ? true : !volunteer.IsActive.Value;
                repository.Put(volunteer.Id, volunteer);
            }

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;
            ViewBag.searchQuery = filter;
            return View(volunteers.ToPagedList(page, pageSize));
        }
        [Route("Edit")]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var repository = new VolunteerRepository();
            var volunteer = repository.GetbyGuid(id);
            volunteer.OTDateString = volunteer.OTDateTime != null ? volunteer.OTDateTime.Value.ToShortDateString() : null;
            var Ots = new OTRepository().Get();

            var Ots1 = Ots.Where(x => (volunteer.orientation_training != null && volunteer.orientation_training.Id == x.Id) || x.OTDateTime != null && x.OTDateTime.Value.Date >= DateTime.Now.Date).Select(x =>
             new SelectListItem { Text = x.Subject + " - " + (x.OTDateTime != null ? x.OTDateTime.Value.ToShortDateString() : null), Value = x.Id + "" }).ToList();
            if (volunteer.orientation_training == null)
                volunteer.OTId = Ots.Last().Id;
            ViewBag.Otsdd = Ots1;
            return View(volunteer);
        }

        public ActionResult ApproveRejectByCustomUrl(Guid rowGuid, string reqStatus,string comment)
        {
            var repository = new VolunteerRepository();
            var oVolunteer = repository.GetbyGuid(rowGuid);
            var cu = Session["user"] as ContextUser;
            if (reqStatus == "approved")
            {
                ApprovedBylevel(new volunteer_profile { }, repository, oVolunteer, cu);
        //        if (cu.EnumRole == EnumUserRole.SuperAdmin)
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            else
            {
                oVolunteer.IsRejected = true;
                oVolunteer.RejectedBy = cu.OUser.Username;
                if (cu.EnumRole == EnumUserRole.Approver1)
                    oVolunteer.ApprovedAtLevel1Comments = comment;
                if (cu.EnumRole == EnumUserRole.Approver2)
                    oVolunteer.ApprovedAtLevel2Comments = comment;
                if (cu.EnumRole == EnumUserRole.Approver3)
                    oVolunteer.ApprovedAtLevel3Comments = comment;

                repository.Put(oVolunteer.Id, oVolunteer);

                SendRejectedMailToVolunteer(oVolunteer, cu);

            }
            return RedirectToAction("Index");
        }
        [Route("Edit")]

        [HttpPost]
        public ActionResult Edit(volunteer_profile volunteer)
        {
            var repository = new VolunteerRepository();
            var oVolunteer = repository.GetbyGuid(volunteer.RowGuid);
            var cu = Session["user"] as ContextUser;
            if (volunteer.SubmitButton == "reject")
            {
                oVolunteer.IsRejected = true;
                oVolunteer.RejectedBy = cu.OUser.Username;
                oVolunteer.ApprovedAtLevel1Comments = volunteer.ApprovedAtLevel1Comments;
                oVolunteer.ApprovedAtLevel2Comments = volunteer.ApprovedAtLevel2Comments;
                oVolunteer.ApprovedAtLevel3Comments = volunteer.ApprovedAtLevel3Comments;

                repository.Put(oVolunteer.Id, oVolunteer);
                

                SendRejectedMailToVolunteer(oVolunteer, cu);

                return RedirectToAction("Index");
            }
            if (volunteer.SubmitButton == "otdate")
            {
                //   oVolunteer.OTDateTime = DateTime.Parse(volunteer.OTDateString);
                oVolunteer.OTId = volunteer.OTId;
                oVolunteer.OTIdAssigner = cu.OUser.Id;
                repository.Put(oVolunteer.Id, oVolunteer);
                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel emodel =
                    new EmailTemplateModel
                    {
                        Title = "Volunteer Approved", 
                        VolunteerName = oVolunteer.VolunteerName 
                    };
                string body =
                    Util.RenderViewToString(bogusController.ControllerContext, "VolunteerApproved", emodel);
                EmailSender.SendSupportEmail(body, oVolunteer.VolunteerEmail);
                return RedirectToAction("Index", new { status = "approved" });
            }
            if (volunteer.SubmitButton == "accept")
            {
                //todo: send notification to super admin
                if (oVolunteer.OTIdAssigner != null)
                {
                    var ot = new OTRepository().Get(oVolunteer.OTId.Value);
                    var admin = new AccountRepository().Get(oVolunteer.OTIdAssigner.Value);
                    var bogusController = Util.CreateController<EmailTemplateController>();
                    EmailTemplateModel emodel =
                        new EmailTemplateModel
                        {
                            Title = "Injaz: accepted by volunteer.",
                            VolunteerName = oVolunteer.VolunteerName,
                            User = admin.FirstName,
                            OTSubject = ot.Subject
                        };
                    string body =
                        Util.RenderViewToString(bogusController.ControllerContext, "VolunteerAcceptsOT", emodel);
                    EmailSender.SendSupportEmail(body, admin.Email);

                }

                oVolunteer.OTAcceptedByVolunteer = volunteer.OTAcceptedByVolunteer;
                repository.Put(oVolunteer.Id, oVolunteer);
                return Redirect("/Volunteer/Edit?id=" + oVolunteer.RowGuid);
            }
            if (volunteer.SubmitButton == "otattendense")
            {
                oVolunteer.OTAttendenceForVolunteer = volunteer.OTAttendenceForVolunteer;
                repository.Put(oVolunteer.Id, oVolunteer);
                return RedirectToAction("Index", new { status = "approved" });
            }
            ApprovedBylevel(volunteer, repository, oVolunteer, cu);
            return RedirectToAction("Index");
        }

        private void SendRejectedMailToVolunteer(volunteer_profile oVolunteer, ContextUser cu)
        {

            string comment = "";
            if (cu.EnumRole == EnumUserRole.Approver1)
            {
                comment = oVolunteer.ApprovedAtLevel1Comments; 
                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel emodel =
                    new EmailTemplateModel
                    {
                        Title = "Volunteer Rejected",
                        RedirectUrl = url,
                        VolunteerName = oVolunteer.VolunteerName,
                        Comment = comment
                    };
                string body =
                    Util.RenderViewToString(bogusController.ControllerContext, "VolunteerRejected", emodel);
                EmailSender.SendSupportEmail(body, oVolunteer.VolunteerEmail);
            }
            if (cu.EnumRole == EnumUserRole.Approver2)
                comment = oVolunteer.ApprovedAtLevel2Comments;
            if (cu.EnumRole == EnumUserRole.Approver3)
                comment = oVolunteer.ApprovedAtLevel3Comments;
        }

        private static void ApprovedBylevel(volunteer_profile volunteer, VolunteerRepository repository, volunteer_profile oVolunteer, ContextUser cu)
        {

            if (cu.EnumRole == EnumUserRole.SuperAdmin)
            {
                oVolunteer.RejectedBy = null;
                oVolunteer.IsRejected = false;
            }
            if (cu.EnumRole == EnumUserRole.Approver1)
            {
                oVolunteer.IsApprovedAtLevel1 = true;
                oVolunteer.ApprovedAtLevel1Comments = volunteer.ApprovedAtLevel1Comments;
            }
            if (cu.EnumRole == EnumUserRole.Approver2)
            {
                oVolunteer.IsApprovedAtLevel2 = true;
                oVolunteer.ApprovedAtLevel2Comments = volunteer.ApprovedAtLevel2Comments;
                oVolunteer.RejectedBy = null;
                oVolunteer.IsRejected = false;
            }
            if (cu.EnumRole == EnumUserRole.Approver3)
            {
                oVolunteer.RejectedBy = null;
                oVolunteer.IsRejected = false;
                oVolunteer.IsApprovedAtLevel3 = true;

                oVolunteer.ApprovedAtLevel3Comments = volunteer.ApprovedAtLevel3Comments;
                string password = EncryptionKeys.Decrypt(oVolunteer.user.Password);
                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel emodel =
                    new EmailTemplateModel
                    {
                        Title = "Volunteer Approved",
                        RedirectUrl = url,
                        VolunteerName = oVolunteer.VolunteerName,
                        UserName = oVolunteer.user.Username,
                        Password = password
                    };
                string body =
                    Util.RenderViewToString(bogusController.ControllerContext, "VolunteerCreated", emodel);
                EmailSender.SendSupportEmail(body, oVolunteer.VolunteerEmail);
            }
            repository.Put(oVolunteer.Id, oVolunteer);
        }
    }
}