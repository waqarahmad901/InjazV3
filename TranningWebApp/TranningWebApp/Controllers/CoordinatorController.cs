using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
    [AuthorizeUser(AccessLevel = "SuperAdmin,Coordinator")]
    public class CoordinatorController : BaseController
    {
        // GET: Coordinator

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

            var cu = Session["user"] as ContextUser;
            IEnumerable<coordinator_profile> Profile;
            var repository = new CoordinatorRepository();
            if (archive != null)
            {
                var oCoordinator = repository.GetByRowId(archive.Value);
                oCoordinator.IsActive = !oCoordinator.IsActive;
                oCoordinator.user.IsLocked = !oCoordinator.IsActive; 
                repository.Put(oCoordinator.Id, oCoordinator);
            }
            if (cu != null && cu.EnumRole == EnumUserRole.Coordinator)
            {
                Profile = repository.GetSubCoordinator(cu.OUser.coordinator_profile.First().Id);
            }
            else
            {
                Profile = string.IsNullOrEmpty(filter) ? repository.Get() : repository.Get().Where(x => x.school.SchoolName.Contains(filter));
            }
            //Sorting order
            Profile = Profile.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = Profile.Count();

            return View(Profile.ToPagedList(page, pageSize));
        }
        public ActionResult SchoolLocation()
        {
            var cu = Session["user"] as ContextUser;
            var coordinatorRepo = new CoordinatorRepository();
            var cor = coordinatorRepo.Get(cu.OUser.coordinator_profile.First().Id);
            return View(cor.school);
        }
        [HttpPost]
        public ActionResult SchoolLocation(school sc)
        {
            var cu = Session["user"] as ContextUser;
            var coordinatorRepo = new CoordinatorRepository();
            var cor = coordinatorRepo.GetBySchool(sc.Id);
            //cor.school.Lat = sc.Lat;
            //cor.school.Lng = sc.Lng;
            cor.school.SchoolGeoLocation = sc.SchoolGeoLocation;
            coordinatorRepo.Put(cor.Id, cor);
            return RedirectToAction("Index", "Session");
        }
        public ActionResult Edit(Guid? id)
        {
            var cities = new CityRepository().Get().Distinct().Select(x =>
                new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            var distict = new CityRepository().Get().GroupBy(x => x.Region).Select(x => x.First()).Select(x =>
                    new SelectListItem { Text = x.Region + " (" + x.Region_ar + ")", Value = x.Region + "" }).ToList();
            ViewBag.distictdd = distict;

            coordinator_profile coordinator = null;
            var su = Session["user"] as ContextUser;

            if (id == null)
            {
                coordinator = new coordinator_profile();
                if (su.EnumRole == EnumUserRole.Coordinator)
                {
                    coordinator.school = su.OUser.coordinator_profile.First().school;
                }
                else
                {
                    coordinator.school = new school();
                }
                coordinator.IsActive = true;
                coordinator.Password = Membership.GeneratePassword(8, 4);
            }
            else
            {
                var coordinatorRepo = new CoordinatorRepository();
                coordinator = coordinatorRepo.GetByRowId(id.Value);
                coordinator.Password = EncryptionKeys.Decrypt(coordinator.user.Password);
            }

            return View(coordinator);
        }
        [HttpPost]
        public ActionResult Edit(coordinator_profile profile)
        {
            var coordinatorRepo = new CoordinatorRepository();
            var accountRepo = new AccountRepository();
            coordinator_profile coordinator = null;
            var su = Session["user"] as ContextUser;
            var userRole = new RoleRepository().Get().FirstOrDefault(x => x.Code == (int)EnumUserRole.Coordinator);
            user ouser = null;
            if (profile.Id == 0)
            {
                if (accountRepo.EmailExist(profile.CoordinatorEmail))
                {
                    var cities = new CityRepository().Get().Distinct().Select(x =>
                    new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
                    ViewBag.citiesdd = cities;
                    var distict = new CityRepository().Get().GroupBy(x => x.Region).Select(x => x.First()).Select(x =>
                    new SelectListItem { Text = x.Region + " (" + x.Region_ar + ")", Value = x.Region + "" }).ToList();
                    ViewBag.distictdd = distict;
                    ViewBag.EmailExist = true;
                    if(su != null && su.EnumRole == EnumUserRole.Coordinator)
                         profile.school = su.OUser.coordinator_profile.First().school;
                    return View(profile);
                }
                coordinator = new coordinator_profile();
                coordinator.RowGuid = Guid.NewGuid();
                coordinator.CreatedAt = DateTime.Now;
                coordinator.CreatedBy = su.OUser.Id;
              
                coordinator.IsPrimery = true;
                coordinator.FirstLogin = true;
                ouser = new user
                {
                    RowGuid = Guid.NewGuid(),
                 
                    Username = profile.CoordinatorEmail,
                    RegistrationDate = DateTime.Now,
                    FirstName = "",
                    RoleId = userRole.Id,
                    CreatedAt = DateTime.Now,
                    FirstLogin = false,
                    ValidFrom = DateTime.Now,
                    IsMobileVerified = false,
                    IsEmailVerified = false,
                    CreatedBy = su.OUser.Id
                };

                coordinator.user = ouser;
               
                coordinator.IsActive = profile.IsActive;
                coordinator.user.IsLocked = !coordinator.IsActive;
                if (su.EnumRole == EnumUserRole.SuperAdmin)
                {
                    coordinator.school = new school();
                    coordinator.school.RowGuid = Guid.NewGuid();
                    coordinator.school.CreatedBy = su.OUser.Id;
                    coordinator.school.CreatedAt = DateTime.Now;
                    coordinator.school.user = ouser;

                    coordinator.school.SchoolName = profile.school.SchoolName;
                    coordinator.school.City = profile.school.City;
                    coordinator.school.District = profile.school.District;
                    coordinator.school.Region = profile.school.Region;
                    coordinator.school.Status = "Initial";

                }

               else
                {
                    coordinator.ParentId = su.OUser.coordinator_profile.First().Id;
                    coordinator.SchoolId = su.OUser.coordinator_profile.First().school.Id;
                    //coordinator.school.Status = "Approved";
                     coordinator.CoordinatorName = su.OUser.coordinator_profile.First().CoordinatorName;

                }

            }
            else
            {
                coordinator = coordinatorRepo.Get(profile.Id);
                coordinator.UpdatedAt = DateTime.Now;
                coordinator.UpdatedBy = su.OUser.Id;
                coordinator.IsActive = profile.IsActive;
              
                coordinator.user.Email = profile.CoordinatorEmail;
                coordinator.user.Username = profile.CoordinatorEmail;
               

                coordinator.user.Password = EncryptionKeys.Encrypt(profile.Password);
                if (su.EnumRole == EnumUserRole.Coordinator)
                {
                    coordinator.ParentId = su.OUser.coordinator_profile.First().Id;
                    coordinator.SchoolId = su.OUser.coordinator_profile.First().school.Id;
                    coordinator.school.Status = "Approved";
                }
                else
                { 
                    coordinator.school.SchoolName = profile.school.SchoolName;
                    coordinator.school.City = profile.school.City;
                    coordinator.school.District = profile.school.District;
                    coordinator.school.Region = profile.school.Region; 
                    if (coordinator.school.Status == "Pending")
                    {
                        coordinator.school.Status = "Approved"; 
                        NewCoordinatorEmail(coordinator);
                    }
                }
            }
            coordinator.user.Email = profile.CoordinatorEmail;
            coordinator.CoordinatorEmail = profile.CoordinatorEmail;
            coordinator.user.Username = profile.CoordinatorEmail;
            coordinator.user.Password = EncryptionKeys.Encrypt(profile.Password);
            if (profile.Id == 0)
            {
                //  coordinator.school.Status = "Pending";

                if (su.EnumRole == EnumUserRole.Coordinator)
                {
                    NewCoordinatorEmail(coordinator);
                }
                else
                {
                    SchoolRegistrationEmail(coordinator);

                }
                coordinatorRepo.Post(coordinator);
            }
            else
                coordinatorRepo.Put(coordinator.Id, coordinator);
            return RedirectToAction("Index");
        }

        private static void NewCoordinatorEmail(coordinator_profile coordinator)
        {
            string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel model = new EmailTemplateModel { Title = "Complete Profile", RedirectUrl = url, UserName = coordinator.CoordinatorEmail, Password = EncryptionKeys.Decrypt(coordinator.user.Password) ,CoordinatorName = coordinator.CoordinatorName,User = coordinator.user.FirstName};
            string body = Util.RenderViewToString(bogusController.ControllerContext, "CoordinatorProfile", model);
            EmailSender.SendSupportEmail(body, coordinator.CoordinatorEmail);
        }

        private void SchoolRegistrationEmail(coordinator_profile coordinator)
        {
            string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Coordinator/Register/" + coordinator.RowGuid;
            var bogusController = Util.CreateController<EmailTemplateController>();
            EmailTemplateModel model = new EmailTemplateModel { Title = "Coordinator Registraion ", RedirectUrl = url,CoordinatorName = coordinator.school.SchoolName};
            string body = Util.RenderViewToString(bogusController.ControllerContext, "CoordinatorRegistration", model);
            string file = Server.MapPath("~/Uploads/DocumentLibrary/DefaultDocument/CoordinatorRegister.docx");

            EmailSender.SendSupportEmail(body, coordinator.CoordinatorEmail, file);
        }

        [AllowAnonymous]
        public ActionResult Register(Guid id)
        {
            ViewBag.gender = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
            };
            SetRegisterDD();
            var coordinatorRepo = new CoordinatorRepository();
            var coordinator = coordinatorRepo.GetByRowId(id);
            return View(coordinator);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(coordinator_profile profile, IEnumerable<HttpPostedFileBase> file)
        {
            var coordinatorRepo = new CoordinatorRepository();
            var coordinator = coordinatorRepo.Get(profile.Id);
            coordinator.school.Telephone = profile.school.Telephone;
            coordinator.Gender = profile.Gender;
            coordinator.school.Fax = profile.school.Fax;
            coordinator.school.SchoolID = profile.school.SchoolID;
            coordinator.school.Website = profile.school.Website;
            //coordinator.school.Lat = profile.school.Lat;
            //coordinator.school.Lng = profile.school.Lng;
            coordinator.school.SchoolGeoLocation = profile.school.SchoolGeoLocation;

            coordinator.school.SchoolManagerName = profile.school.SchoolManagerName;
            coordinator.school.SchoolManagerEmailAddress = profile.school.SchoolManagerEmailAddress;
            coordinator.school.SchoolManagerMobile = profile.school.SchoolManagerMobile;
            coordinator.school.SchoolGuardMobile = profile.school.SchoolGuardMobile;
            coordinator.school.SchoolGuardName = profile.school.SchoolGuardName;
            coordinator.school.GoogleAddress = profile.school.GoogleAddress;
            coordinator.school.LinkedInAddress = profile.school.LinkedInAddress;
            coordinator.school.TypeOfSchool = profile.school.TypeOfSchool;
            coordinator.school.StageOfSchool = profile.school.StageOfSchool;
            coordinator.school.YouTube = profile.school.YouTube;
            coordinator.school.FaceBook = profile.school.FaceBook;
            var file1 = file.ToArray()[0];
            if (file1 != null)
            {
                string fileName = "~/Uploads/ImageLibrary/" + Guid.NewGuid() + Path.GetExtension(file1.FileName);
                string filePath = Server.MapPath(fileName);
                file1.SaveAs(filePath);
                coordinator.school.PhotoPath = fileName;
            }

            var file2 = file.ToArray()[1];
            if (file2 != null)
            {
                string fileName2 = "~/Uploads/DocumentLibrary/" + Guid.NewGuid() + Path.GetExtension(file2.FileName);
                string filePath2 = Server.MapPath(fileName2);
                file2.SaveAs(filePath2);
                coordinator.school.Status = "Pending";
                coordinator.school.DocumentPath = fileName2;
            }
            coordinator.IsRegisterComplete = true;
            if (coordinator.school.DocumentPath == null || file2 == null)
            {
                ViewBag.NeedFile = true;
                SetRegisterDD();

                return View(profile);
            }
            coordinatorRepo.Put(profile.Id, profile);

            ViewBag.gender = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
            };
            SetRegisterDD();
            ViewBag.CompleteFirstTime = true;
            return RedirectToAction("Edit",new {id = coordinator.RowGuid });
        }

        private void SetRegisterDD()
        {
            ViewBag.typesofschooldd = new List<SelectListItem>
            {
               new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
            };
            ViewBag.stageofschooldd = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = General.Primary, Value =  "Primary"},
                new SelectListItem { Selected = false, Text = General.Middle, Value= "Middle"},
                new SelectListItem { Selected = false, Text = General.Secondary, Value= "Secondary"}
            };
        }

        [AuthorizeUser(AccessLevel = "SuperAdmin,Coordinator")]
        public ActionResult CoordinatorProfile(Guid? id)
        {
            ViewBag.gender = new List<SelectListItem>
            {
               new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
            };
            var cu = Session["user"] as ContextUser;
            var repository = new CoordinatorRepository();
            coordinator_profile oCordinator = null;
            if (id != null)
                oCordinator = repository.GetByRowId(id.Value);
            else
               oCordinator = repository.GetByUserId(cu.OUser.Id);

            return View(oCordinator);
        }
        [HttpPost]
        [AuthorizeUser(AccessLevel = "Coordinator")]
        public ActionResult CoordinatorProfile(coordinator_profile coordinator, HttpPostedFileBase file)
        {
            var repository = new CoordinatorRepository();
            var oCordinator = repository.Get(coordinator.Id);
            oCordinator.CoordinatorName = coordinator.CoordinatorName;
            oCordinator.Gender = coordinator.Gender;
            oCordinator.DateOfBirth = coordinator.DateOfBirth;
            oCordinator.CoordinatorMobile = coordinator.CoordinatorMobile;
            oCordinator.Class = coordinator.Class;
            oCordinator.NationalID = coordinator.NationalID;
            oCordinator.HasTOTCertificate = coordinator.HasTOTCertificate;
            oCordinator.VolunteerActivity1 = coordinator.VolunteerActivity1;
            oCordinator.VolunteerActivity2 = coordinator.VolunteerActivity2;
            oCordinator.VolunteerActivity3 = coordinator.VolunteerActivity3;
            oCordinator.OtherCertificate1 = coordinator.OtherCertificate1;
            oCordinator.OtherCertificate2 = coordinator.OtherCertificate2;
            oCordinator.OtherCertificate3 = coordinator.OtherCertificate3;
            oCordinator.FatherName = coordinator.FatherName;
            oCordinator.FaimlyName = coordinator.FaimlyName;
            oCordinator.AcademicQualification = coordinator.AcademicQualification;
            oCordinator.AcademicQualification2 = coordinator.AcademicQualification2;
            oCordinator.AcademicQualification3 = coordinator.AcademicQualification3;
            oCordinator.IsProfileComplete = true;
            if (file != null)
            {
                string fileName = "~/Uploads/ImageLibrary/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Server.MapPath(fileName);
                file.SaveAs(filePath);
                oCordinator.PhotoPath = fileName;
            }
            repository.Put(coordinator.Id, oCordinator);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult Delete(int id)
        {
            var repository = new FunderRepository();
            repository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}