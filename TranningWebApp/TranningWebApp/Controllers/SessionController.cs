using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using Microsoft.Ajax.Utilities;
using PagedList;
using TmsWebApp.Common;
using TmsWebApp.HelpingUtilities;
using TmsWebApp.Models;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Models;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;
using TranningWebApp.Resource;

namespace TmsWebApp.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin,Coordinator,Volunteer,Participant")]
    public class SessionController : BaseController
    {
        // GET: Funder
        public ActionResult Index(string sortOrder, string filter, string archived, int page = 1, Guid? archive = null)
        {
            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter.Trim();
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;

            ViewBag.CurrentSort = sortOrder;

            IEnumerable<session> session;
            var repository = new SessionRepository();
            repository.SetOccuredStatus();


            if (archive != null)
            {
                var oSession = repository.GetByRowId(archive.Value);
                oSession.IsActive = !oSession.IsActive;
                repository.Put(oSession.Id, oSession);
            }

            var cu = Session["user"] as ContextUser;

            int corSchoolId = 0;
            if (cu.EnumRole != EnumUserRole.Volunteer)
                corSchoolId = new CoordinatorRepository().GetSchoolIdByUserId(cu.OUser.Id);
            var participantId = 0;
            if (cu.EnumRole == EnumUserRole.Participant)
            {
                participantId = new ParticipiantRepository().GetByUserId(cu.OUser.Id).Id;
            }
            session = repository.Get().Where(x =>
            {
                return ((filter == null || x.ProgramName.ToLower().Contains(filter.ToLower())
                                                  || (x.school != null && (x.school.SchoolName.ToLower().Contains(filter.ToLower()) ||
                                                                           x.school.City.ToLower().Contains(filter.ToLower())))
                                                 )
                                                 && ((cu.EnumRole == EnumUserRole.SuperAdmin
                                                      || (cu.EnumRole == EnumUserRole.Participant && x.session_participant
                                                              .Select(y => y.ParticipantID)
                                                              .Contains(participantId))
                                                      || (cu.EnumRole == EnumUserRole.Volunteer &&
                                                          ((x.VolunteerId == null && x.Status == "Approved") ||
                                                           (x.VolunteerId.HasValue && x.VolunteerId.Value == cu.OUser.Id && (x.Status == "Approved" || x.Status == "Occured"))))
                                                      || (cu.EnumRole == EnumUserRole.Coordinator && x.SchoolID == corSchoolId))));
            });
            //Sorting order
            session = session.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = session.Count();

            return View(session.ToPagedList(page, pageSize));
        }


        public ActionResult Edit(Guid? id, int page = 1)
        {
            SelectListItem defaultselect = new SelectListItem { Text = General.Select, Value = "0" };

            var school = new SchoolRepository().Get().Select(x =>
             new SelectListItem { Text = x.SchoolName, Value = x.Id + "" }).ToList();
            school.Insert(0, defaultselect);
            ViewBag.school = school;

            var volunteer = new VolunteerRepository().GetApprovedVolunteer().Select(x =>
             new SelectListItem { Text = x.VolunteerName, Value = x.Id + "" }).ToList();
            volunteer.Insert(0, defaultselect);
            ViewBag.volunteer = volunteer;

            var certificatesVolunteer = new CertificateRepository().Get().Where(x => x.TypeCertificate == "Volunteer").Select(x =>
              new SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();
            certificatesVolunteer.Insert(0, defaultselect);
            ViewBag.certificatesVolunteer = certificatesVolunteer;

            var certificatesStudent = new CertificateRepository().Get().Where(x => x.TypeCertificate == "Student").Select(x =>
       new SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();
            certificatesStudent.Insert(0, defaultselect);
            ViewBag.certificatesStudent = certificatesStudent;

            var evaluationCatagory = new EvaluationFormRepository().Get().GroupBy(x => x.Catagory).Select(x => x.First()).Select(x =>
                    new SelectListItem { Text = x.Catagory, Value = x.Catagory + "" }).ToList();
            evaluationCatagory.Insert(0, defaultselect);
            ViewBag.evaluationCatagory = evaluationCatagory;

            var cu = Session["user"] as ContextUser;

            session sessionModel;
            if (id == null)
            {
                sessionModel = new session();
                sessionModel.IsActive = true;
                sessionModel.PropesedDateString = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");
                sessionModel.ProposedDateTime = DateTime.Now.AddMonths(1);
                sessionModel.ProposedEndDateTime = DateTime.Now.AddMonths(1).AddDays(3);
            }
            else
            {
                var sessionRepo = new SessionRepository();
                sessionModel = sessionRepo.GetByRowId(id.Value);
                sessionModel.PropesedDateString = sessionModel.ProposedDateTime.ToString("dd/MM/yyyy");
                sessionModel.EnumSessionStatus = (SessionStatus)Enum.Parse(typeof(SessionStatus), sessionModel.Status);
                if (sessionModel.ActualDateTime == null && cu.EnumRole == EnumUserRole.Coordinator && sessionModel.EnumSessionStatus == SessionStatus.Pending)
                {
                    sessionModel.ActualDateTime = sessionModel.ProposedDateTime;
                    sessionModel.ActualDateString = sessionModel.ProposedDateTime.ToString("dd/MM/yyyy");
                    sessionModel.ActualEndDateTime = sessionModel.ProposedEndDateTime;
                    foreach (var item in sessionModel.session_proposed_time)
                    {
                        sessionModel.session_actual_time.Add(new session_actual_time
                        {
                            ActualEndTime = item.ProposedEndTime,
                            ActualStartTime = item.ProposedStartTime,
                            IsActive = item.IsActive,
                            IsHoliday = item.IsHoliday

                        });
                    }
                }
                if (sessionModel.ActualDateTime != null)
                {
                    sessionModel.ActualDateString = sessionModel.ActualDateTime.Value.ToString("dd/MM/yyyy");
                    if ((sessionModel.ActualDateTime.Value.Date - DateTime.Now.Date).Hours >= 72 && sessionModel.EnumSessionStatus == SessionStatus.Approved && cu.EnumRole == EnumUserRole.Coordinator)
                        sessionModel.ChangeDateVisible = true;
                }
                if (cu.EnumRole == EnumUserRole.Participant)
                {
                    string eve_cat = sessionModel.StudentEvaluationCatagory;
                    if (!string.IsNullOrEmpty(eve_cat))
                    {
                        int participantId = new ParticipiantRepository().GetByUserId(cu.OUser.Id).Id;
                        var sParticipant = sessionModel.session_participant.Where(x => x.ParticipantID == participantId).First();
                        sessionModel.EvaluationFormFilled = (sParticipant.IsPreEvaluated ?? false) && (sParticipant.IsPostEvaluated ?? false);
                        sessionModel.IsPreFilledByStudent = (sParticipant.IsPreEvaluated ?? false);
                        sessionModel.IsPostFilledByStudent = (sParticipant.IsPostEvaluated ?? false);
                        sessionModel.IsAttendenseMarked = sParticipant.VolunteerMarkedAttendence;

                    }
                }
                if (cu.EnumRole == EnumUserRole.Coordinator)
                {
                    sessionModel.IsFilledCoordinator = new EvaluationVolunteerRepository().GetEvaluationForm(sessionModel.Id) != null;
                }
                if (cu.EnumRole == EnumUserRole.Volunteer)
                {
                    sessionModel.IsFilledVolunteer = new EvaluationCoordinatorRepository().GetEvaluationForm(sessionModel.Id) != null;
                }
                var session_evaluationform_photo = sessionModel.session_evaluationform_photo.Select(x => x.FilePath).ToArray();
                sessionModel.EvaluationImageLink = string.Join(",", session_evaluationform_photo) + ",";
                var session_photo = sessionModel.session_photo.Select(x => x.FilePath).ToArray();
                sessionModel.SessionImageLink = string.Join(",", session_photo) + ",";

                sessionModel.PagedParticipants = sessionModel.session_participant.Select(x => x.participant_profile).OrderByDescending(x => x.CreatedAt).ToPagedList(page, 5);
                ViewBag.Count = sessionModel.session_participant.Count();
                ViewBag.IsSessionEnabledForVolunteer = true;
            }

            ViewBag.genderdd = new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
                };
            var cities = new CityRepository().Get().Distinct().Select(x =>
                  new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            var countries = new CountryRepository().Get().Select(x =>
                       new SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();
            ViewBag.countries = countries;

            return View(sessionModel);
        }

        private bool CheckIsEvaluationFilled(string eveCat, int participantId, int sessionid)
        {
            bool isPrefilled = false;
            bool isPostfilled = false;
            switch (eveCat)
            {
                case "CP":
                    isPrefilled = new EvaluationCpPreRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    isPostfilled = new EvaluationCpPostRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    break;
                case "LP":
                    isPrefilled = new EvaluationCpPreRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    isPostfilled = new EvaluationCpPostRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    break;
                case "SYC":
                    isPrefilled = new EvaluationSycPreePart1Repository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    isPostfilled = new EvaluationSycPrePart2Repository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    break;
                case "Murshdee":
                    isPrefilled = new EvaluationMurshadeeBeforeRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    isPostfilled = new EvaluationMurshadeeAfterRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    break;
                case "Safeer":
                    isPrefilled = new EvaluationSafeerPreRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    isPostfilled = new EvaluationSafeerPreRepository().Get().Any(x => x.StudentId == participantId && x.SessionId == sessionid);
                    break;
            }
            return isPrefilled && isPostfilled;
        }

        [HttpPost]
        public ActionResult Edit(session session, FormCollection collection)
        {
            var sessionRepo = new SessionRepository();
            session oSession = null;
            var cu = Session["user"] as ContextUser;
            DateTime? oldActualDate = null;
            if (session.Id == 0)
            {
                oSession = new session();
                oSession.RowGUID = Guid.NewGuid();
                oSession.CreatedAt = DateTime.Now;
                oSession.CreatedBy = cu.OUser.Id;
                oSession.Status = SessionStatus.Pending.ToString();

            }
            else
            {
                oSession = sessionRepo.Get(session.Id);
                oSession.UpdatedAt = DateTime.Now;
                oldActualDate = oSession.ActualDateTime;
                // oSession.UpdatedBy = int.Parse(sid);
            }
            var currentStatus = (SessionStatus)Enum.Parse(typeof(SessionStatus), oSession.Status);


            if (cu.EnumRole == EnumUserRole.SuperAdmin && currentStatus == SessionStatus.Pending)
            {
                oSession.ProgramPurpose = "ProgramPurposeNotUsed";
                oSession.TargetGroup = session.TargetGroup;
                oSession.Gender = session.Gender;
                oSession.City = session.City;
                oSession.Country = session.Country;

                if (oSession.SchoolID == null && session.SchoolID != null && session.SchoolID.Value != 0)
                {
                    var cor = new CoordinatorRepository().GetBySchool(session.SchoolID.Value);
                    var user = new AccountRepository().Get(oSession.CreatedBy);
                    var bogusController = Util.CreateController<EmailTemplateController>();
                    EmailTemplateModel emodel =
                        new EmailTemplateModel
                        {
                            Title = "Notification: Session Created For School",
                            User = user.FirstName,
                            CoordinatorName = cor.CoordinatorName,
                            SessionTitle = oSession.ProgramName,
                            UserName = user.Username,
                            Email = cor.CoordinatorEmail
                        };
                    string body =
                        Util.RenderViewToString(bogusController.ControllerContext, "SessionCreatedSchool", emodel);
                    EmailSender.SendSupportEmail(body, cor.CoordinatorEmail);
                }
                oSession.SchoolID = session.SchoolID == 0 ? null : session.SchoolID;
                oSession.ProgramName = session.ProgramName;
                oSession.ProposedDateTime = DateTime.ParseExact(session.PropesedDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                oSession.ProposedEndDateTime = session.ProposedEndDateTime;

                int days = (oSession.ProposedEndDateTime - oSession.ProposedDateTime).Value.Days;

                if (oSession.Id > 0)
                    sessionRepo.RemoveAllPropesedTimes(oSession.Id);
                for (int i = 0; i <= days; i++)
                {
                    DateTime fromTime = DateTime.ParseExact(collection["proFromTime_" + i].ToString(), "hh:mm tt", CultureInfo.InvariantCulture);
                    DateTime toTime = DateTime.ParseExact(collection["proToTime_" + i].ToString(), "hh:mm tt", CultureInfo.InvariantCulture);
                    oSession.session_proposed_time.Add(
                        new session_proposed_time
                        {
                            IsHoliday = collection["procheck_" + i] == "on" ? true : false,
                            ProposedStartTime = fromTime.TimeOfDay,
                            ProposedEndTime = toTime.TimeOfDay,
                        }
                        );
                }


                oSession.VolunteerId = session.VolunteerId == 0 ? null : session.VolunteerId;
                oSession.StudentCertificate = session.StudentCertificate == 0 ? null : session.StudentCertificate;
                oSession.VolunteerCertificate = session.VolunteerCertificate == 0 ? null : session.VolunteerCertificate;
                oSession.StudentEvaluationCatagory = session.StudentEvaluationCatagory;
            }
            if (cu.EnumRole == EnumUserRole.Coordinator && currentStatus == SessionStatus.Pending)
            {
                oSession.ActualEndDateTime = session.ActualEndDateTime;
                oSession.ActualDateTime = session.ActualDateTime;

                int days = (session.ActualEndDateTime - session.ActualDateTime).Value.Days;

                if (oSession.Id > 0)
                    sessionRepo.RemoveAllActualTimes(oSession.Id);
                for (int i = 0; i <= days; i++)
                {
                    DateTime fromTime = DateTime.ParseExact(collection["actFromTime_" + i].ToString(), "hh:mm tt", CultureInfo.InvariantCulture);
                    DateTime toTime = DateTime.ParseExact(collection["actToTime_" + i].ToString(), "hh:mm tt", CultureInfo.InvariantCulture);
                    oSession.session_actual_time.Add(
                        new session_actual_time
                        {
                            IsHoliday = collection["actcheck_" + i] == "on" ? true : false,
                            ActualStartTime = fromTime.TimeOfDay,
                            ActualEndTime = toTime.TimeOfDay,
                        });
                }
                // check date and time change by admin
                bool isNotChange = CheckNotAnyChageInAdminAndCoordinatorTime(oSession);
                if (isNotChange)
                {
                    if (oSession.Status != SessionStatus.Approved.ToString())
                        SendEmailNotificationsApprovedByAdmin(oSession);
                    oSession.Status = SessionStatus.Approved.ToString();
                    oSession.ApprovedByAdmin = true;
                  
                }
                else
                {
                    if (oSession.Status != SessionStatus.DateChanges.ToString())
                        SendEmailNotificationDateChanged(oSession);
                    oSession.Status = SessionStatus.DateChanges.ToString();

                }

            }
            if (session.ActualDateString != null)
                oSession.ActualDateTime = DateTime.ParseExact(session.ActualDateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (currentStatus == SessionStatus.DateChanges)
            {
                if (session.SendKitByMailCourier && !oSession.SendKitByMailCourier)
                {
                    var cor = oSession.school.coordinator_profile.First();
                    var user = new AccountRepository().Get(oSession.CreatedBy);
                    var bogusController = Util.CreateController<EmailTemplateController>();
                    EmailTemplateModel emodel =
                        new EmailTemplateModel
                        {
                            Title = "Injaz: Administrator send kit by mail courier.",
                            User = user.FirstName,
                            CoordinatorName = cor.CoordinatorName,
                            SessionTitle = oSession.ProgramName,

                        };
                    string body =
                        Util.RenderViewToString(bogusController.ControllerContext, "SendKitBymailCourier", emodel);
                    EmailSender.SendSupportEmail(body, cor.CoordinatorEmail);
                }
                oSession.SendKitByMailCourier = session.SendKitByMailCourier;
            }
            if (cu.EnumRole == EnumUserRole.Coordinator && (currentStatus == SessionStatus.Approved || currentStatus == SessionStatus.Rejected))
            {

                if (session.ConfirmKitReceivedBySchool && !oSession.ConfirmKitReceivedBySchool)
                {
                    var cor = oSession.school.coordinator_profile.First();
                    var user = new AccountRepository().Get(oSession.CreatedBy);
                    var bogusController = Util.CreateController<EmailTemplateController>();
                    EmailTemplateModel emodel =
                        new EmailTemplateModel
                        {
                            Title = "Injaz: Confirm Kit Received By School.",
                            CoordinatorName = cor.CoordinatorName,
                            SessionTitle = oSession.ProgramName,
                            User = cor.user.FirstName
                        };
                    string body =
                        Util.RenderViewToString(bogusController.ControllerContext, "ConfirmKitReceivedBySchool", emodel);
                    EmailSender.SendSupportEmail(body, user.Email);
                }
                oSession.ConfirmKitReceivedBySchool = session.ConfirmKitReceivedBySchool;
            }

            if (currentStatus == SessionStatus.Approved && oldActualDate != null && oSession.ActualDateTime != oldActualDate)
            {
                if (oSession.Status != SessionStatus.Approved.ToString())
                    SendEmailNotificationDateChanged(oSession);
                oSession.Status = SessionStatus.DateChanges.ToString();
                oSession.ApprovedByAdmin = false;

            }
            if (session.SubmitButton != null)
            {
                if (session.SubmitButton == "approved")
                {
                    if (oSession.Status != SessionStatus.Approved.ToString())
                        SendEmailNotificationsApprovedByAdmin(oSession);
                    oSession.Status = SessionStatus.Approved.ToString();
                    oSession.ApprovedByAdmin = true;

                }
                if (session.SubmitButton == "submitpre")
                {
                    int participantId = new ParticipiantRepository().GetByUserId(cu.OUser.Id).Id;
                    string eve_cat = oSession.StudentEvaluationCatagory;
                    string form = new EvaluationFormRepository().Get().First(x => x.Catagory == eve_cat && x.SubCatagory == "pre").FormPath;
                    return RedirectToAction(form, "EvaluationForm", new { participantid = participantId, sessionid = oSession.Id });
                }
                if (session.SubmitButton == "submitpost")
                {
                    int participantId = new ParticipiantRepository().GetByUserId(cu.OUser.Id).Id;
                    string eve_cat = oSession.StudentEvaluationCatagory;
                    string form = new EvaluationFormRepository().Get().First(x => x.Catagory == eve_cat && x.SubCatagory == "post").FormPath;
                    return RedirectToAction(form, "EvaluationForm", new { participantid = participantId, sessionid = oSession.Id });
                }
                if (session.SubmitButton == "request")
                {
                    //oSession.MarkedCompletedByVolunteer = true;
                    var volunteer = new VolunteerRepository().GetByGoogleId(cu.GoogleId) ?? new VolunteerRepository().GetByLinkedInId(cu.GoogleId);
                    oSession.VolunteerId = volunteer.Id;

                    var user = new AccountRepository().Get(oSession.CreatedBy);
                    var bogusController = Util.CreateController<EmailTemplateController>();
                    EmailTemplateModel emodel =
                        new EmailTemplateModel
                        {
                            Title = "Injaz:Volunteer Assign Session",
                            VolunteerName = volunteer.VolunteerName,
                            SessionTitle = oSession.ProgramName,
                            User = user.FirstName
                        };
                    string body =
                        Util.RenderViewToString(bogusController.ControllerContext, "VolunteerAssignSession", emodel);
                    EmailSender.SendSupportEmail(body, user.Email);
                }
                if (session.SubmitButton == "confirm")
                {
                    return RedirectToAction("StudentAttendense", new { sessionId = oSession.RowGUID });

                }
                if (session.SubmitButton == "reject")
                {
                    oSession.Status = SessionStatus.Rejected.ToString();
                }
                if (session.SubmitButton == "feedback")
                {
                    var participant = new ParticipiantRepository().GetByUserId(cu.OUser.Id);

                    return RedirectToAction("FeedBack", new { sessionId = oSession.Id, participantId = participant.Id });
                }
                if (session.SubmitButton == "coordinatorform")
                {
                    int corId = new CoordinatorRepository().GetByUserId(oSession.school.CoordinatorUserId).Id;
                    return RedirectToAction("CoordinatorForm", "EvaluationForm", new { sessionId = oSession.Id, volId = oSession.volunteer_profile.Id, corId = corId });
                }
                if (session.SubmitButton == "viewcertificate" || session.SubmitButton == "volunteerviewcertificate")
                {
                    var participant = new ParticipiantRepository().GetByUserId(cu.OUser.Id);
                    string certificatePath = null;

                    List<PdfCoordinatesModel> pdfCoordinates = null;
                    if (cu.EnumRole == EnumUserRole.Participant)
                    {
                        certificatePath = Server.MapPath(oSession.certificate.UploadFilePath);
                        pdfCoordinates = new CertificateDictionary().GetPdfCoordinatesFromDictionary(oSession.certificate.Type);
                    }
                    else
                    {
                        certificatePath = Server.MapPath(oSession.certificate1.UploadFilePath);
                        pdfCoordinates = new CertificateDictionary().GetPdfCoordinatesFromDictionary(oSession.certificate1.Type);
                        oSession.IsVolunteerCertificateGenerated = true;
                    }
                    foreach (var pc in pdfCoordinates)
                    {
                        switch (pc.CertifiacteData)
                        {

                            case CertificateEnum.NameOfStudent:
                                pc.Text = participant.Name + " " + participant.FatherName + " " + participant.Family;
                                break;
                            case CertificateEnum.NameOfCoordinator:
                                pc.Text = oSession.school.coordinator_profile.First().CoordinatorName;
                                break;
                            case CertificateEnum.ProgrammYear:
                                pc.Text = DateTime.Now.Year + "";
                                break;
                            case CertificateEnum.TranningDate:
                                pc.Text = Util.DateConversion(oSession.ActualDateTime.Value.ToShortDateString(), "Hijri", "en-us");
                                break;
                            case CertificateEnum.TranningHour:
                                pc.Text = ConfigurationManager.AppSettings["TranningHours"];
                                break;
                            case CertificateEnum.NameOfVolunteer:
                                pc.Text = oSession.volunteer_profile.VolunteerName;
                                break;
                        }
                    }
                    string fontFilePath = Server.MapPath("~/fonts/Arabic Fonts/trado.ttf");
                    var outputFile = PdfGenerator.GenerateOnflyPdf(certificatePath, pdfCoordinates, fontFilePath);
                    if (session.SubmitButton == "viewcertificate")
                    {
                        EmailSender.SendSupportEmail("Student Template", participant.Email, outputFile);
                        oSession.session_participant.Where(x => x.ParticipantID == participant.Id).First().IsCertificateGenerated = true;
                    }
                    else
                        EmailSender.SendSupportEmail("Volunteer Template", oSession.volunteer_profile.VolunteerEmail, outputFile);

                }
            }
            //oSession.IsActive = session.IsActive;
            if (session.Id == 0)
            {
                sessionRepo.Post(oSession);
            }
            else
                sessionRepo.Put(oSession.Id, oSession);
            return RedirectToAction("Index");
        }

        private bool CheckNotAnyChageInAdminAndCoordinatorTime(session oSession)
        {
            bool isStartChange = oSession.ProposedDateTime == oSession.ActualDateTime;
            bool isEndChange = oSession.ProposedEndDateTime == oSession.ActualEndDateTime;
            if (isStartChange && isEndChange)
            {
                for (int i = 0; i < oSession.session_actual_time.Count; i++)
                {
                    // any change detect in holiday, from and to time
                    if (!(oSession.session_actual_time.ElementAt(i).IsHoliday == oSession.session_proposed_time.ElementAt(i).IsHoliday)
                       && (oSession.session_actual_time.ElementAt(i).ActualEndTime == oSession.session_proposed_time.ElementAt(i).ProposedEndTime)
                        && (oSession.session_actual_time.ElementAt(i).ActualStartTime == oSession.session_proposed_time.ElementAt(i).ProposedStartTime))
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        private static void SendEmailNotificationDateChanged(session oSession)
        {

            var cor = oSession.school.coordinator_profile.First();
            var user = new AccountRepository().Get(oSession.CreatedBy);
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel emodel =
                new EmailTemplateModel
                {
                    Title = "Injaz:Session Date Changed",
                    CoordinatorName = cor.CoordinatorName,
                    SessionTitle = oSession.ProgramName
                };
            string body =
                Util.RenderViewToString(bogusController.ControllerContext, "SessionDateChanged", emodel);
            EmailSender.SendSupportEmail(body, user.Email);
        }

        public ActionResult StudentAttendense(Guid sessionId)
        {
            var sessionRepo = new SessionRepository();
            var students = sessionRepo.GetByRowId(sessionId).session_participant.Select(x => x.participant_profile).ToList();
            return View(students);
        }
        [HttpPost]
        public ActionResult StudentAttendense(List<participant_profile> participants)
        {
            var sessionRepo = new SessionRepository();
            Guid sessionId = Guid.Parse(Request.Form["sessionid"]);
            if (participants == null || participants.Count == 0)
            {
                return RedirectToAction("Index");
            }
            int[] selectedParticipant = participants.Where(x => x.IsSelected).Select(x => x.Id).ToArray();
            var session = sessionRepo.GetByRowId(sessionId);
            var sessionParticipants = session.session_participant.Where(x => selectedParticipant.Contains(x.ParticipantID)).ToList();

            sessionParticipants.ForEach(x => x.VolunteerMarkedAttendence = true);

            // send email to coordinator
            var corEmail = session.school.coordinator_profile.First().CoordinatorEmail;
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel emodel =
                new EmailTemplateModel
                {
                    Title = "Notification: student attendense marked as completed.",
                    VolunteerName = session.volunteer_profile.VolunteerName,
                    SessionTitle = session.ProgramName,
                    CoordinatorName = session.school.coordinator_profile.First().CoordinatorName
                };
            string body =
                Util.RenderViewToString(bogusController.ControllerContext, "VolunteerConfirmAttendense", emodel);
            EmailSender.SendSupportEmail(body, corEmail);

            //send email to manager

            var adminEmail = new AccountRepository().Get(session.CreatedBy).Email;
            EmailSender.SendSupportEmail(body, adminEmail);
            session.VolunteerMarkedStudentAttendenceInSession = true;
            session.MarkedCompletedByVolunteer = true;
            sessionRepo.Put(session.Id, session);
            return RedirectToAction("Index");
        }

        public ActionResult FeedBack(int sessionId, int participantId)
        {
            var feedback = new SessionRepository().Get(sessionId).session_participant.Where(x => x.ParticipantID == participantId).First().FeedBack;
            var sessionFeedBack = new SessionFeedBack { FeedBack = feedback, ParticipantId = participantId, SessionId = sessionId };
            return View(sessionFeedBack);

        }
        [HttpPost]
        public ActionResult FeedBack(SessionFeedBack model)
        {
            var repo = new SessionRepository();
            var session = repo.Get(model.SessionId);
            var sParticipant = session.session_participant.Where(x => x.ParticipantID == model.ParticipantId).First();
            sParticipant.FeedBack = model.FeedBack;
            repo.Put(session.Id, session);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult MarkCompleted(session session)
        {
            var cu = Session["user"] as ContextUser;
            var sessionRepo = new SessionRepository();
            var oSession = sessionRepo.Get(session.Id);
            if (Request.Form["SubmitButton"] == "Upload")
            {
                sessionRepo.RemoveSessionPhoto(session.Id);
                foreach (var item in session.EvaluationImageLink.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    oSession.session_evaluationform_photo.Add(new session_evaluationform_photo { FilePath = item, FileExtension = Path.GetExtension(item) });
                }
                foreach (var item in session.SessionImageLink.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    oSession.session_photo.Add(new session_photo { FilePath = item, FileExtension = Path.GetExtension(item) });
                }
            }
            else if (Request.Form["SubmitButton"] == "fillvolunteerevaluation")
            {
                int corId = new CoordinatorRepository().GetByUserId(cu.OUser.Id).Id;
                return RedirectToAction("VolenteerForm", "EvaluationForm", new { sessionId = oSession.Id, volId = oSession.volunteer_profile.Id, corId = corId });
            }
            else
            {
                oSession.MarkedCompletedByCoordinator = true;

                var volEmail = oSession.volunteer_profile.VolunteerEmail;
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel emodel =
                    new EmailTemplateModel
                    {
                        Title = "Notification: coordinator session marked as completed.",
                        CoordinatorName = oSession.school.coordinator_profile.FirstOrDefault().CoordinatorName,
                        SessionTitle = oSession.ProgramName,
                        VolunteerName = oSession.volunteer_profile.VolunteerName,
                        User = oSession.school.user.FirstName
                    };
                string body =
                    Util.RenderViewToString(bogusController.ControllerContext, "CoordinatorMarkCompleted", emodel);
                EmailSender.SendSupportEmail(body, volEmail);

                var adminEmail = new AccountRepository().Get(oSession.CreatedBy).Email;
                EmailSender.SendSupportEmail(body, adminEmail);

            }
            sessionRepo.Put(oSession.Id, oSession);
            return RedirectToAction("Index");
        }

        public ActionResult RequestSession()
        {
            var cu = Session["user"] as ContextUser;
            int participantId = new ParticipiantRepository().GetByUserId(cu.OUser.Id).Id;
            var repository = new SessionRepository();
            var oSession = repository.Get().Where(x => x.Status == "Approved").ToList();
            foreach (var item in oSession)
            {
                if (item.session_participant.Any(x => x.ParticipantID == participantId))
                {
                    item.IsSelected = true;
                }
            }
            return View(oSession);
        }
        [HttpPost]
        public ActionResult RequestSession(List<session> sessions)
        {
            if (sessions != null && sessions.Count > 0)
            {
                var cu = Session["user"] as ContextUser;
                int participantId = new ParticipiantRepository().GetByUserId(cu.OUser.Id).Id;
                var repository = new SessionRepository();
                var oSession = repository.Get();
                var oSessionSelected = oSession.Where(y => sessions.Where(x => x.IsSelected).Select(x => x.Id).ToList().Contains(y.Id)).ToList();
                var oSessionUnSelected = oSession.Where(y => sessions.Where(x => !x.IsSelected).Select(x => x.Id).ToList().Contains(y.Id)).ToList();

                foreach (var item in oSessionSelected)
                {
                    var sp = item.session_participant.Where(x => x.ParticipantID == participantId).FirstOrDefault();
                    if (sp == null)
                    {
                        item.session_participant.Add(new session_participant { SessionID = item.Id, ParticipantID = participantId });
                    }
                }
                repository.RemoveSessionParticipants(oSessionUnSelected, participantId);

                repository.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public void SendEmailNotificationsApprovedByAdmin(session oSession)
        {
            var cor = oSession.school.coordinator_profile.First();
            var user = new AccountRepository().Get(oSession.CreatedBy);
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel emodel =
                new EmailTemplateModel
                {
                    Title = "Notification: Approved By Admin.",
                    SessionTitle = oSession.ProgramName,
                    CoordinatorName = cor.CoordinatorName,
                    User = cor.user.FirstName
                };
            string body =
                Util.RenderViewToString(bogusController.ControllerContext, "ApprovedByAdmin", emodel);
            EmailSender.SendSupportEmail(body, cor.CoordinatorEmail);
        }
        public ActionResult Delete(int id)
        {
            var repository = new FunderRepository();
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}