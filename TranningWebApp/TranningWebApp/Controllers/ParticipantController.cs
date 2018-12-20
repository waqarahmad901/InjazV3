using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using OfficeOpenXml;
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
    [AuthorizeUser(AccessLevel = "SuperAdmin,Coordinator,Participant")]
    public class ParticipantController : BaseController
    {
        // GET: Funder
        public ActionResult Index(string sortOrder, string filter, string archived, int page = 1, Guid? archive = null)
        {
            //PdfGenerator.PdfGenerator pdf = new PdfGenerator.PdfGenerator();
            //pdf.GenerateOnflyPdf();

            //ExcelReader.Read(@"C:\Users\malikwaqar\Desktop\bredford code analysic.xlsx");

            ViewBag.searchQuery = string.IsNullOrEmpty(filter) ? "" : filter;
            ViewBag.showArchived = (archived ?? "") == "on";

            page = page > 0 ? page : 1;
            int pageSize = 0;
            pageSize = pageSize > 0 ? pageSize : 10;

            ViewBag.CurrentSort = sortOrder;

            IEnumerable<participant_profile> participiant;
            var repository = new ParticipiantRepository();
            if (archive != null)
            {
                var oParticipiant = repository.GetByRowId(archive.Value);
                oParticipiant.isActive = !oParticipiant.isActive;
                oParticipiant.user.IsLocked = !oParticipiant.isActive;

                repository.Put(oParticipiant.Id, oParticipiant);
            }
            if (string.IsNullOrEmpty(filter))
            {
                participiant = repository.GetAll();
            }
            else
            {
                participiant = repository.GetAll().Where(x => x.Name.ToLower
                ().Contains(filter.ToLower()) || x.NationalID.Contains(filter)
              
                || x.Mobile.ToLower().Contains(filter.ToLower())
                || x.Email.ToLower().Contains(filter.ToLower())
                || (x.City != null && x.City.ToLower().Contains(filter.ToLower()))
                || (x.school != null && x.school.SchoolName.ToLower().Contains(filter.ToLower()))
                || (x.session_participant.Any() && x.session_participant.Select(y => y.session).Last().ProgramName.ToLower().Contains(filter.ToLower()))
                || (x.session_participant.Any() && x.session_participant.Select(y => y.session).Last().volunteer_profile != null && x.session_participant.Select(y => y.session).Last().volunteer_profile.VolunteerName.ToLower().Contains(filter.ToLower()))
                || (filter == "issued" && x.session_participant.Where(y => y.IsCertificateGenerated != null && y.IsCertificateGenerated.Value).Any())
                || (filter == "inprogress" && x.session_participant.Where(y => y.IsCertificateGenerated == null).Any())
                );
            }
            //Sorting order
            participiant = participiant.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = participiant.Count();

            return View(participiant.ToPagedList(page, pageSize));
        }

        public ActionResult Edit(Guid? id, int sessionId = 0)
        {

            participant_profile participaintModel = null;
            if (id == null)
            {
                participaintModel = new participant_profile();
                participaintModel.isActive = true;
                participaintModel.Password = Membership.GeneratePassword(8, 4);

            }
            else
            {
                var participiantRepo = new ParticipiantRepository();
                participaintModel = participiantRepo.GetByRowId(id.Value);
                participaintModel.Password = EncryptionKeys.Decrypt(participaintModel.user.Password);
            }
            if (sessionId > 0)
            {
                participaintModel.SessionId = sessionId;
                return PartialView("_ManageParticipant", participaintModel);
            }
            return View(participaintModel);
        }
        [HttpGet]
        public ActionResult CheckEmail(string email,string nid)
        {
            var accountRepo = new AccountRepository();
            bool isEmailExist = accountRepo.EmailExist(email);
            bool isNIDExist = accountRepo.EmailNationalIdExist(nid);

            return Json(new {email= isEmailExist, nid = isNIDExist }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Edit(participant_profile profile)
        {
            var accountRepo = new AccountRepository();
            var participantRepo = new ParticipiantRepository();
            participant_profile participant = null;
            var cu = Session["user"] as ContextUser;
            if (profile.Id == 0)
            {
                if (accountRepo.EmailExist(profile.Email))
                {
                    ViewBag.EmailExist = true;
                    return View(profile);
                }
                participant = participantRepo.GetParticipant(profile.NationalID);
                if (participant == null)
                {
                    participant = new participant_profile
                    {
                        RowGuid = Guid.NewGuid(),
                        CreatedAt = DateTime.Now,
                        CreatedBy = cu.OUser.Id,
                        Email = profile.Email,


                    };
                }
                if (profile.SessionId > 0)
                {
                    participant.session_participant.Add(new session_participant { SessionID = profile.SessionId, ParticipantID = participant.Id });
                }

                var role = new RoleRepository().GetByCode((int)EnumUserRole.SuperAdmin);

                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel model = new EmailTemplateModel { Title = "Coordinator create student", RedirectUrl = url };
                string body = Util.RenderViewToString(bogusController.ControllerContext, "CoordinatorCreateStudent", model);
                EmailSender.SendSupportEmail(body, role.users.FirstOrDefault().Email);
                return RedirectToAction("Index", "Session");

            }
            else
            {
                participant = participantRepo.Get(profile.Id);
                participant.UpdatedAt = DateTime.Now;
                participant.UpdatedBy = cu.OUser.Id;
            }

            var userRole = new RoleRepository().Get().Where(x => x.Code == (int)EnumUserRole.Participant).FirstOrDefault();
            if (participant.ParticipantUserID == 0)
                participant.user = new user
                {
                    RowGuid = Guid.NewGuid(),
                    Email = profile.Email,
                    Username = profile.Email,
                    RegistrationDate = DateTime.Now,
                    FirstName = profile.Name,
                    RoleId = userRole.Id,
                    CreatedAt = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    FirstLogin = false,
                    IsMobileVerified = false,
                    IsEmailVerified = false,
                    CreatedBy = cu.OUser.Id,
                    Password = EncryptionKeys.Encrypt(profile.Password)
                };
            participant.Name = profile.Name;
            participant.FatherName = profile.FatherName;
            participant.Family = profile.Family;
            participant.NationalID = profile.NationalID;
            if (profile.MobileNo != null)
                participant.Mobile = profile.MobileNo;
            else
                participant.Mobile = profile.Mobile;
            participant.isActive = profile.isActive;
            participant.user.IsLocked = !participant.isActive;
            if (participant.Id == 0)
            {
                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel model = new EmailTemplateModel { Title = "Complete Profile", RedirectUrl = url, UserName = participant.Email, Password = EncryptionKeys.Decrypt(participant.user.Password), ParticipantName = participant.Name, User = participant.user.FirstName };
                string body = Util.RenderViewToString(bogusController.ControllerContext, "ParticipantProfile", model);
                EmailSender.SendSupportEmail(body, participant.Email);
                participant.IsEmailSent = true;
                participantRepo.Post(participant);

            }
            else
                participantRepo.Put(participant.Id, participant);
            if (Request["participant"] == "true")
            {
                var rowId = new SessionRepository().Get(profile.SessionId).RowGUID;
                return RedirectToAction("Edit", "Session", new { id = rowId });
            }
            return RedirectToAction("Index");
        }

        [AuthorizeUser(AccessLevel = "Participant")]
        public ActionResult ParticipantProfile()
        {

            ViewBag.gender = new List<SelectListItem>
            {
                 new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
            };
            var cities = new CityRepository().Get().Distinct().Select(x =>
               new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            ViewBag.stageofschooldd = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = General.Primary, Value =  "Primary "},
                new SelectListItem { Selected = false, Text = General.Middle, Value= "Middle"},
                new SelectListItem { Selected = false, Text = General.Secondary, Value= "Secondary"}
            };
            var school = new SchoolRepository().Get().Select(x =>
            new SelectListItem { Text = x.SchoolName, Value = x.Id + "" }).ToList();

            ViewBag.school = school;

            var cu = Session["user"] as ContextUser;
            var repository = new ParticipiantRepository();
            var oParticipiant = repository.GetByUserId(cu.OUser.Id);

            if (oParticipiant.session_participant != null && oParticipiant.session_participant.Count > 0)
            {
                var sessions = oParticipiant.session_participant.Select(x => x.session).Select(x =>
                   new SelectListItem { Text = x.ProgramName, Value = x.ProgramName }).ToList();

                ViewBag.sessions = sessions;
            }
            else
            {

                var sessions = new SessionRepository().Get().Select(x =>
                new SelectListItem { Text = x.ProgramName, Value = x.ProgramName }).ToList();

                ViewBag.sessions = sessions;
            }

            return View(oParticipiant);
        }

        [HttpPost]
        [AuthorizeUser(AccessLevel = "Participant")]
        public ActionResult ParticipantProfile(participant_profile participant, HttpPostedFileBase file)
        {
            var participantRepo = new ParticipiantRepository();
            var oparticipant = participantRepo.Get(participant.Id);
            oparticipant.Mobile = participant.Mobile;
            oparticipant.Gender = participant.Gender;
            oparticipant.DateOfBirth = participant.DateOfBirth;
            oparticipant.ProgrammName1 = participant.ProgrammName1;
            oparticipant.FatherMobile = participant.Mobile;
            oparticipant.City = participant.City;
            oparticipant.SchoolId = participant.SchoolId;
            oparticipant.FacebookAddress = participant.FacebookAddress;
            oparticipant.TwitterAddress = participant.TwitterAddress;
            oparticipant.SnapChatAddress = participant.SnapChatAddress;
            oparticipant.Stage = participant.Stage;
            oparticipant.user.FirstLogin = true;
            oparticipant.IsProfileComplete = true;
            oparticipant.Instagram = participant.Instagram;
            oparticipant.Class = participant.Class;
            if (file != null)
            {
                string fileName = "~/Uploads/ImageLibrary/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Server.MapPath(fileName);
                file.SaveAs(filePath);
                oparticipant.PhotoPath = fileName;
            }
            participantRepo.Put(participant.Id, oparticipant);
            return RedirectToAction("Index", "Session");

        }

        public ActionResult Delete(int id)
        {
            var repository = new FunderRepository();
            repository.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult UploadExcel(int sessionId = 0)
        {
            ExcelModel model = new ExcelModel { SessionId = sessionId };
            return PartialView("_UploadExcel", model);
        }
        [HttpPost]
        public ActionResult UploadExcel(ExcelModel model, HttpPostedFileBase file)
        {

            var session = new SessionRepository().Get(model.SessionId);
            try
            {
                string fileName = "~/Uploads/" + file.FileName;
                string filePath = Server.MapPath(fileName);
                file.SaveAs(filePath);
                var participantRepo = new ParticipiantRepository();
                participant_profile participant = null;
                var cu = Session["user"] as ContextUser;
                List<participant_profile> profileList = new List<participant_profile>();
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(filePath)))
                {
                    var sheet = xlPackage.Workbook.Worksheets[1];
                    var rowCnt = sheet.Dimension.End.Row;
                    for (int row = 2; row <= rowCnt; row++)
                    {

                        participant_profile profile = new participant_profile();
                        profile.Name = GetValue(sheet, row, 1);
                        if (string.IsNullOrEmpty(profile.Name))
                            continue;
                        profile.FatherName = GetValue(sheet, row, 2);
                        profile.Family = GetValue(sheet, row, 3);
                        profile.NationalID = GetValue(sheet, row, 4);
                        profile.Mobile = GetValue(sheet, row, 5);
                        profile.Email = GetValue(sheet, row, 6);
                        profileList.Add(profile);
                    }

                   string error = ValidateParticipantRecords(profileList);
                    if (error != null)
                    {
                        return RedirectToAction("Edit", "Session", new { id = session.RowGUID, excelerror = true, error = error });
                    }

                    if (session.session_participant.Count + profileList.Count > session.NumberOfActualStudents)
                    {
                       
                            return RedirectToAction("Edit", "Session", new { id = session.RowGUID, excelerror = true, error = "Students is not allowed more than " + (session.NumberOfActualStudents - session.session_participant.Count) });
                        
                    }
                }
                foreach (var profile in profileList)
                {

                    participant = participantRepo.GetParticipant(profile.NationalID);

                    if (participant == null)
                    {
                        participant = new participant_profile
                        {
                            RowGuid = Guid.NewGuid(),
                            CreatedAt = DateTime.Now,
                            CreatedBy = cu.OUser.Id,
                            Email = profile.Email
                        };
                    }
                    var isSessionAttached = participant.session_participant.Where(x => x.SessionID == model.SessionId).Any();
                    if (model.SessionId > 0 && !isSessionAttached)
                    {
                        participant.session_participant.Add(
                            new session_participant { SessionID = model.SessionId, ParticipantID = participant.Id });
                    }

                    var userRole = new RoleRepository().Get().Where(x => x.Code == (int)EnumUserRole.Participant)
                        .FirstOrDefault();
                    if (participant.ParticipantUserID == 0)
                        participant.user = new user
                        {
                            RowGuid = Guid.NewGuid(),
                            Email = profile.Email,
                            Username = profile.Email,
                            RegistrationDate = DateTime.Now,
                            FirstName = profile.Name,
                            RoleId = userRole.Id,
                            CreatedAt = DateTime.Now,
                            ValidFrom = DateTime.Now,
                            FirstLogin = false,
                            IsMobileVerified = false,
                            IsEmailVerified = false,
                            CreatedBy = cu.OUser.Id,
                            Password = EncryptionKeys.Encrypt(Membership.GeneratePassword(8, 4))
                        };
                    participant.Name = profile.Name;
                    participant.FatherName = profile.FatherName;
                    participant.Family = profile.Family;
                    participant.NationalID = profile.NationalID;
                    participant.Mobile = profile.Mobile;
                    participant.isActive = true;
                    if (participant.Id == 0)
                    {
                        string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                     "/Account/Login";
                        var bogusController = Util.CreateController<EmailTemplateController>();
                        EmailTemplateModel emodel =
                            new EmailTemplateModel
                            {
                                Title = "Complete Profile",
                                RedirectUrl = url,
                                UserName = participant.Email,
                                User = participant.Email,
                                Password = EncryptionKeys.Decrypt(participant.user.Password)
                            };
                        string body =
                            Util.RenderViewToString(bogusController.ControllerContext, "CoordinatorProfile", emodel);
                        EmailSender.SendSupportEmail(body, participant.Email);
                        participant.IsEmailSent = true;
                        participantRepo.Post(participant);

                    }
                    else
                        participantRepo.Put(participant.Id, participant);
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "Session", new { id = session.RowGUID, excelerror = true, error = Participant.UploadError });
                throw ex;
            }
            return RedirectToAction("Index", "Session");
        }

        private string ValidateParticipantRecords(List<participant_profile> profileList)
        {
            int line = 2;
            foreach (var item in profileList)
            {
                if (string.IsNullOrEmpty(item.Email))
                    return General.Emailempty + " " + line;
                if(!IsValidEmail(item.Email))
                    return General.emailvalid + " " + line;
                if (string.IsNullOrEmpty(item.Mobile))
                    return General.Mobileempty + " " + line;
                if (!(item.Mobile.Length >= 9 && item.Mobile.Length <= 10))
                    return General.mobilevalid + " " + line;
                if (string.IsNullOrEmpty(item.NationalID))
                    return General.nationalidempty + " " + line;
                if (!(item.NationalID.Length == 10))
                    return General.nationalidvalid + " " + line;
                line++;
            }
            return null;
        }

       private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GetValue(ExcelWorksheet sheet, int row, int col)
        {
            if (sheet.Cells[row, col].Value == null)
            {
                return "";
            }
            return sheet.Cells[row, col].Value.ToString();
        }
    }
}