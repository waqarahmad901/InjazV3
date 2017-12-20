using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using TmsWebApp.Controllers;
using TmsWebApp.HelpingUtilities;
using TmsWebApp.Models;
using TranningWebApp.Common;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;

namespace TranningWebApp.Controllers
{
    [Authorize]
    public class EvaluationFormController : BaseOnlyArabicController
    {
        // GET: EvaluationForm
        public ActionResult CPPre(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_cp_pre ecpp = new EvaluationCpPreRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if(ecpp == null)
                    ecpp = new evaluation_cp_pre();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.Name + " " + participant.FatherName + " " + participant.Family;
                if (participant.DateOfBirth != null)
                {
                    string date = participant.DateOfBirth.Value.ToString("ddMMyyyy");
                    ecpp.F2_1 = date.ToArray()[1] + "";
                    ecpp.F2_2 = date.ToArray()[0] + "";
                    ecpp.F2_3 = date.ToArray()[3] + "";
                    ecpp.F2_4 = date.ToArray()[2] + "";
                    ecpp.F2_5 = date.ToArray()[7] + "";
                    ecpp.F2_6 = date.ToArray()[6] + "";
                    ecpp.F2_7 = date.ToArray()[5] + "";
                    ecpp.F2_8 = date.ToArray()[4] + "";
                }
                ecpp.F3 = participant.Gender;
                return View(ecpp);
            }
            return View();
        }

        [HttpPost]
        public ActionResult CPPre(evaluation_cp_pre ecpp)
        {
            var repo = new EvaluationCpPreRepository();
            ecpp.RowId = Guid.NewGuid();
            ecpp.CreateAt = DateTime.Now; 
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPreEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }


        public ActionResult CPPost(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_cp_post ecpp = new EvaluationCpPostRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_cp_post();
                ecpp.F1 = participant.Name + " " + participant.FatherName + " " + participant.Family;
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                if (participant.DateOfBirth != null)
                {
                    string date = participant.DateOfBirth.Value.ToString("ddMMyyyy");
                    ecpp.F2_1 = date.ToArray()[1] + "";
                    ecpp.F2_2 = date.ToArray()[0] + "";
                    ecpp.F2_3 = date.ToArray()[3] + "";
                    ecpp.F2_4 = date.ToArray()[2] + "";
                    ecpp.F2_5 = date.ToArray()[7] + "";
                    ecpp.F2_6 = date.ToArray()[6] + "";
                    ecpp.F2_7 = date.ToArray()[5] + "";
                    ecpp.F2_8 = date.ToArray()[4] + "";
                }
                ecpp.F3 = participant.Gender;
               
                return View(ecpp);
            }
            return View();

        }

        [HttpPost]
        public ActionResult CPPost(evaluation_cp_post ecpp)
        {
            var repo = new EvaluationCpPostRepository();
            ecpp.RowId = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository(); 
            sessionrepo.SetPostEvaluationStatus(ecpp.StudentId, ecpp.SessionId); 
            return RedirectToAction("Index", "Session");


        }

        public ActionResult MrshdyBefore(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_murshadee_before ecpp = new EvaluationMurshadeeBeforeRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_murshadee_before();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName; 
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View(); 
             
        }

        [HttpPost]
        public ActionResult MrshdyBefore(evaluation_murshadee_before ecpp)
        {
            var repo = new EvaluationMurshadeeBeforeRepository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);

            var sessionrepo = new SessionRepository();
            sessionrepo.SetPreEvaluationStatus(ecpp.StudentId, ecpp.SessionId);

            return RedirectToAction("Index", "Session");
        }


        public ActionResult MrshdyAfter(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_murshadee_after ecpp = new EvaluationMurshadeeAfterRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_murshadee_after();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View();
        }

        [HttpPost]
        public ActionResult MrshdyAfter(evaluation_murshadee_after ecpp)
        {
            var repo = new EvaluationMurshadeeAfterRepository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPostEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult SafeerPre(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_safeer_pre ecpp = new EvaluationSafeerPreRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_safeer_pre();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp); 
            }
            return View();
        }

        [HttpPost]
        public ActionResult SafeerPre(evaluation_safeer_pre ecpp)
        {
            var repo = new EvaluationSafeerPreRepository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPreEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }

   
        public ActionResult SafferPost(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_safeer_post ecpp = new EvaluationSafeerPostRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_safeer_post();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View();
        }

        [HttpPost]
        public ActionResult SafferPost(evaluation_safeer_post ecpp)
        {
            var repo = new EvaluationSafeerPostRepository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPostEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }
 
        public ActionResult SYCPostP1(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_syc_post_part1 ecpp = new EvaluationSycPostPart1Repository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_syc_post_part1();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View();
        }
        [HttpPost]
        public ActionResult SYCPostP1(evaluation_syc_post_part1 ecpp)
        {

            var repo = new EvaluationSycPostPart1Repository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAT = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPostEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }
        public ActionResult SYCPostP2(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_syc_post_part2 ecpp = new EvaluationSycPostPart2Repository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_syc_post_part2();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View();
        }
        [HttpPost]
        public ActionResult SYCPostP2(evaluation_syc_post_part2 ecpp)
        {

            var repo = new EvaluationSycPostPart2Repository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPostEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }
        public ActionResult SYCPreP1(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_syc_pre_part1 ecpp = new EvaluationSycPreePart1Repository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_syc_pre_part1();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View();
        }
        [HttpPost]
        public ActionResult SYCPreP1(evaluation_syc_pre_part1 ecpp)
        {

            var repo = new EvaluationSycPreePart1Repository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPreEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }
        public ActionResult SYCPreP2(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_syc_pre_part2 ecpp = new EvaluationSycPrePart2Repository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_syc_pre_part2();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.school.SchoolName;
                ecpp.F5 = participant.Name;
                ecpp.F6 = participant.FatherName;
                ecpp.F7 = participant.Family;
                ecpp.F8 = participant.Mobile;
                ecpp.F9 = participant.Email;
                ecpp.F10 = participant.FacebookAddress;
                ecpp.F11 = participant.TwitterAddress;
                return View(ecpp);
            }
            return View();
        }
        [HttpPost]
        public ActionResult SYCPreP2(evaluation_syc_pre_part2 ecpp)
        {

            var repo = new EvaluationSycPrePart2Repository();
            ecpp.Rowld = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPreEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult PFPost(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var oSession = new SessionRepository().Get(sessionid.Value);
                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_pf_post ecpp = new EvaluationPFPostRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_pf_post();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.Name + " "+ participant.FatherName + " " + participant.Family;
                ecpp.F2 = participant.Email;
                ecpp.F4 = participant.Gender;
                if (participant.DateOfBirth != null)
                {
                    string date = participant.DateOfBirth.Value.ToString("ddMMyyyy");
                    ecpp.F5_1 = date.ToArray()[1] + "";
                    ecpp.F5_2 = date.ToArray()[0] + "";
                    ecpp.F5_3 = date.ToArray()[3] + "";
                    ecpp.F5_4 = date.ToArray()[2] + "";
                    ecpp.F5_5 = date.ToArray()[7] + "";
                    ecpp.F5_6 = date.ToArray()[6] + "";
                    ecpp.F5_7 = date.ToArray()[5] + "";
                    ecpp.F5_8 = date.ToArray()[4] + "";
                }
                ecpp.F6 = participant.school.SchoolName;
                ecpp.F7 = oSession.volunteer_profile.VolunteerName;
                return View(ecpp);
            }
            return View();
        }
        [HttpPost]
        public ActionResult PFPost(evaluation_pf_post ecpp)
        {

            var repo = new EvaluationPFPostRepository();
            ecpp.RowId = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPostEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }


        public ActionResult PFPre(int? participantid, int? sessionid)
        {
            if (participantid != null)
            {
                var oSession = new SessionRepository().Get(sessionid.Value);

                var participant = new ParticipiantRepository().Get(participantid.Value);
                evaluation_pf_pre ecpp = new EvaluationPFPreRepository()
                    .Get().FirstOrDefault(x => x.SessionId == sessionid && x.StudentId == participantid);
                if (ecpp == null)
                    ecpp = new evaluation_pf_pre();
                ecpp.StudentId = participantid.Value;
                ecpp.SessionId = sessionid.Value;
                ecpp.F1 = participant.Name + " " + participant.FatherName + " " + participant.Family;
                ecpp.F2 = participant.Email;
                ecpp.F4 = participant.Gender;
                if (participant.DateOfBirth != null)
                {
                    string date = participant.DateOfBirth.Value.ToString("ddMMyyyy");
                    ecpp.F5_1 = date.ToArray()[1] + "";
                    ecpp.F5_2 = date.ToArray()[0] + "";
                    ecpp.F5_3 = date.ToArray()[3] + "";
                    ecpp.F5_4 = date.ToArray()[2] + "";
                    ecpp.F5_5 = date.ToArray()[7] + "";
                    ecpp.F5_6 = date.ToArray()[6] + "";
                    ecpp.F5_7 = date.ToArray()[5] + "";
                    ecpp.F5_8 = date.ToArray()[4] + "";
                }
                ecpp.F6 = participant.school.SchoolName;
                ecpp.F7 = oSession.volunteer_profile.VolunteerName;
                return View(ecpp);
            }
            return View();
        }
        [HttpPost]
        public ActionResult PFPre(evaluation_pf_pre ecpp)
        {

            var repo = new EvaluationPFPreRepository();
            ecpp.RowId = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            var sessionrepo = new SessionRepository();
            sessionrepo.SetPreEvaluationStatus(ecpp.StudentId, ecpp.SessionId);
            return RedirectToAction("Index", "Session");
        }


        public ActionResult VolCorEvaluation(int sessionId,int? volId, int? corId,bool? iscoordinator)
        {
            var ecpp = new evaluation_volunteer_coordinator();
            //if (iscoordinator != null)
            //{
            //    var repo = new EvaluationVolunteerCoordinatorRepository();
            //    var ecpp1 = repo.GetEvaluationForm(sessionId, iscoordinator.Value);
            //    return View(ecpp1);
            //}
            ecpp.SessionId = sessionId;
            ecpp.VolunteerId = volId;
            ecpp.CoordinatorId = corId;
            return View(ecpp);

            //return View();
        }
        
        [HttpPost]
        public ActionResult VolCorEvaluation(evaluation_volunteer_coordinator ecpp)
        { 
            var repo = new EvaluationVolunteerCoordinatorRepository();
            ecpp.RowGuid = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;  
            var cu = Session["user"] as ContextUser;
            if (cu.EnumRole == TmsWebApp.Common.EnumUserRole.Coordinator)
            {
                ecpp.IsCoordinator = false;
                var session = new SessionRepository().Get(ecpp.SessionId);
                var userId = session.CreatedBy;
                var admin = new AccountRepository().Get(userId);
                var corName = new CoordinatorRepository().Get(ecpp.CoordinatorId.Value).CoordinatorName;

                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login/";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel model = new EmailTemplateModel { Title = "Evaluation Form Completion", VolunteerName = session.volunteer_profile.VolunteerName, CoordinatorName = corName, SessionTitle = session.ProgramName,User = admin.FirstName };
                string body = Util.RenderViewToString(bogusController.ControllerContext, "VolunteerFeedBack", model);

                EmailSender.SendSupportEmail(body, admin.Email);
            }
            else
            {
                ecpp.IsCoordinator = true;
            }
            repo.Post(ecpp);

            return RedirectToAction("Index","Session");
        }
        public ActionResult CoordinatorForm(int sessionId, int? volId, int? corId)
        { 
            evaluation_coordinator ecpp = new EvaluationCoordinatorRepository().GetEvaluationForm(sessionId); 
            if (ecpp == null)
            {
                ecpp = new evaluation_coordinator();
                ecpp.SessionId = sessionId;
                if(volId != null)
                    ecpp.VolunteerId = volId.Value;
                if (corId != null)
                {
                    ecpp.CoordinatorId = corId.Value;
                    var cor = new CoordinatorRepository().Get(corId.Value);
                    ecpp.F1 = cor.CoordinatorName + " "+  cor.FatherName + " " + cor.FaimlyName;
                    ecpp.F2 = cor.CoordinatorEmail;
                    ecpp.F2 = cor.CoordinatorEmail;
                    ecpp.F3 = cor.CoordinatorMobile;
                    ecpp.F4 = cor.school.Region;
                    ecpp.F5 = cor.school.City;
                    ecpp.F6 = cor.school.SchoolName;

                }
            }
            var cities = new CityRepository().Get().Distinct().Select(x =>
              new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            var distict = new CityRepository().Get().GroupBy(x => x.Region).Select(x => x.First()).Select(x =>
                    new SelectListItem { Text = x.Region + " (" + x.Region_ar + ")", Value = x.Region + "" }).ToList();
            ViewBag.distictdd = distict;

            return View(ecpp); 
        }
        [HttpPost]
        public ActionResult CoordinatorForm(evaluation_coordinator ecpp)
        {
            var repo = new EvaluationCoordinatorRepository();
            ecpp.RowId = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult VolenteerForm(int sessionId, int? volId, int? corId)
        {

            evaluation_volunteer ecpp = new EvaluationVolunteerRepository().GetEvaluationForm(sessionId);
            if (ecpp == null)
            {
                ecpp = new evaluation_volunteer();
               
             
                ecpp.SessionId = sessionId;
                if (volId != null)
                {
                    var schoolName = new SessionRepository().Get(sessionId).school.SchoolName;
                    ecpp.VolunteerId = volId.Value;
                    var vol = new VolunteerRepository().Get(volId.Value);
                    ecpp.F1 = schoolName;
                    ecpp.F2 = vol.Region;
                    ecpp.F3 = vol.City;
                    ecpp.F5 = vol.VolunteerName;
                }
                if (corId != null)
                    ecpp.CoordinatorId = corId.Value;
            }
            return View(ecpp);
        }
        [HttpPost]
        public ActionResult VolenteerForm(evaluation_volunteer ecpp)
        {
            var repo = new EvaluationVolunteerRepository();
            ecpp.RowId = Guid.NewGuid();
            ecpp.CreatedAt = DateTime.Now;
            repo.Post(ecpp);
             
            var session = new SessionRepository().Get(ecpp.SessionId);
            var userId = session.CreatedBy;
            var adminEmail = new AccountRepository().Get(userId).Email;
            var corName = new CoordinatorRepository().Get(ecpp.CoordinatorId).CoordinatorName;

            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel model = new EmailTemplateModel { Title = "Evaluation Form Completion", VolunteerName = session.volunteer_profile.VolunteerName, CoordinatorName = corName, SessionTitle = session.ProgramName };
            string body = Util.RenderViewToString(bogusController.ControllerContext, "VolunteerFeedBack", model);

            EmailSender.SendSupportEmail(body, adminEmail);

            return RedirectToAction("Index", "Session");
        }


    }
}