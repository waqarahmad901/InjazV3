﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
   
    // [AuthorizeUser(AccessLevel = "Volunteer")]
    public class VolunteerController : BaseController
    {
        // GET: Volunteer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult VolunteerProfile()
        {

            volunteer_profile oVolunteer = null;
            var cu = Session["user"] as ContextUser;
            var repository = new VolunteerRepository();
            if (cu != null)
            {
                if (cu.OUser.Id != 0)
                    oVolunteer = repository.Get(cu.OUser.Id);
                if (oVolunteer == null)
                    oVolunteer = repository.GetByUserId(cu.OUser.Id);
                if (oVolunteer == null)
                    oVolunteer = repository.GetByLinkedInId(cu.LinkedInId);
                if (oVolunteer == null)
                    oVolunteer = repository.GetByGoogleId(cu.GoogleId);
                if (oVolunteer == null)
                {
                    oVolunteer = new volunteer_profile();
                    oVolunteer.VolunteerEmail = cu.OUser.Email;
                    if (cu.FullName.Split(' ').Length > 3)
                    {
                        oVolunteer.VolunteerName = cu.FullName.Split(' ')[0];
                        oVolunteer.SecondName = cu.FullName.Split(' ')[1];
                        oVolunteer.ThirdName = cu.FullName.Split(' ')[2];
                        oVolunteer.LastName = cu.FullName.Split(' ')[3];
                    }
                    if (cu.FullName.Split(' ').Length > 2)
                    {
                        oVolunteer.VolunteerName = cu.FullName.Split(' ')[0];
                        oVolunteer.SecondName = cu.FullName.Split(' ')[1];
                        oVolunteer.ThirdName = cu.FullName.Split(' ')[2]; 
                    }
                    if (cu.FullName.Split(' ').Length > 1)
                    {
                        oVolunteer.VolunteerName = cu.FullName.Split(' ')[0];
                        oVolunteer.SecondName = cu.FullName.Split(' ')[1]; 
                    }
                    oVolunteer.CompanyName = "Individual";
                    oVolunteer.LinkedIn = cu.ProfileUrl;
                }
                oVolunteer.GoogleSigninId = cu.GoogleId;
                oVolunteer.LinkedInSignInId = cu.LinkedInId;
            }
            else
            {
                oVolunteer = new volunteer_profile();
            }
            ViewBag.genderdd = new List<SelectListItem>
                {
                    new SelectListItem { Selected = true, Text = General.Male, Value =  "Male"},
                new SelectListItem { Selected = false, Text = General.Female, Value= "Female"}
                };
            var distict = new CityRepository().Get().GroupBy(x => x.Region).Select(x => x.First()).Select(x =>
           new SelectListItem { Text = x.Region + " (" + x.Region_ar + ")", Value = x.Region + "", Selected = x.Region == "Makkah Region" }).ToList();
            ViewBag.distictdd = distict;
            HttpCookie lanCookie = Request.Cookies["lan"];
            var lan = lanCookie == null ? "en" : lanCookie.Value;
            var specilization = new SpecializationRepository().Get().Select(x =>
                            new SelectListItem { Text = (lan == "ar" ? x.Name_Arabic : x.Name), Value = x.Name + "" }).ToList();
            ViewBag.specialization = specilization;
            var universites = new UniversityRepository().Get().Select(x =>
                             new SelectListItem { Text = (lan == "ar" ? x.Name_Arabic : x.Name), Value = x.Name + "" }).ToList();
            ViewBag.universites = universites;
            if (oVolunteer.Region == null)
            {
                var cities = new CityRepository().Get().Distinct().Where(x => x.Region == "Makkah Region").Select(x =>
                       new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
                ViewBag.citiesdd = cities;
            }
            else
            {
                var cities = new CityRepository().Get().Distinct().Where(x => x.Region == oVolunteer.Region).Select(x =>
                       new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
                ViewBag.citiesdd = cities;
            }
            if (oVolunteer.Id == 0)
            {
                oVolunteer.VolExp = "";
                oVolunteer.DateOfBirth = DateTime.Now.AddYears(-10);
            }
            else
                oVolunteer.VolExp = !string.IsNullOrEmpty(oVolunteer.VolunteerExperince1) ? "Yes" : "No";
            oVolunteer.SelectedExp = !string.IsNullOrEmpty(oVolunteer.VolunteerExperince1) ? oVolunteer.VolunteerExperince1.Split(',') : new string[] { };
            return View(oVolunteer);
        }
        public ActionResult GetFilterCities(string region)
        {
            var cities = new CityRepository().Get().Distinct().Where(x => x.Region == region).Select(x =>
                                    new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult VolunteerProfile(volunteer_profile volunteer, HttpPostedFileBase file)
        {
            var cu = Session["user"] as ContextUser;
            var repository = new VolunteerRepository();
            volunteer_profile oVolunteer = null;
            if (cu != null)
                oVolunteer = repository.GetByGoogleId(cu.GoogleId) ?? repository.GetByLinkedInId(cu.LinkedInId);

            if (oVolunteer == null)
            {
                oVolunteer = new volunteer_profile();
                oVolunteer.CreatedAt = DateTime.Now;
                oVolunteer.CreatedBy = 1;
                oVolunteer.FirstLogin = true;
                oVolunteer.RowGuid = Guid.NewGuid();
                oVolunteer.IsActive = true;
            }
            else
            {
                oVolunteer.UpdatedAt = DateTime.Now;
                oVolunteer.UpdatedBy = 1;
            }
            oVolunteer.NationalID = volunteer.NationalID;
            oVolunteer.VolunteerName = volunteer.VolunteerName;
            oVolunteer.SecondName = volunteer.SecondName;
            oVolunteer.ThirdName = volunteer.ThirdName;
            oVolunteer.LastName = volunteer.LastName;
            oVolunteer.GoogleSigninId = cu != null ? cu.GoogleId : "";
            oVolunteer.LinkedInSignInId = cu != null ? cu.LinkedInId : "";
            oVolunteer.VolunteerEmail = volunteer.VolunteerEmail;
            oVolunteer.VolunteerMobile = volunteer.VolunteerMobile;
            oVolunteer.Gender = volunteer.Gender;
            oVolunteer.DateOfBirth = volunteer.DateOfBirth;
            oVolunteer.AcademicQualification = volunteer.AcademicQualification;
            oVolunteer.AcademicQualification1 = volunteer.AcademicQualification1;
            oVolunteer.AcademicQualification2 = volunteer.AcademicQualification2;
            oVolunteer.CompanyName = volunteer.CompanyName;
            oVolunteer.VolunteerExperince1 = volunteer.VolExp == "Yes" ? string.Join(",", volunteer.SelectedExp == null ? new string[] { } : volunteer.SelectedExp) : "";
            oVolunteer.Telephone = volunteer.Telephone;
            oVolunteer.Region = volunteer.Region;
            oVolunteer.City = volunteer.City;
            oVolunteer.VolunteerActivity1 = volunteer.VolunteerActivity1;
            oVolunteer.VolunteerActivity2 = volunteer.VolunteerActivity2;
            oVolunteer.VolunteerActivity3 = volunteer.VolunteerActivity3;
            oVolunteer.HasTOTCertificate = volunteer.HasTOTCertificate;
            oVolunteer.OtherCertificate1 = volunteer.OtherCertificate1;
            oVolunteer.OtherCertificate2 = volunteer.OtherCertificate2;
            oVolunteer.OtherCertificate3 = volunteer.OtherCertificate3;
            oVolunteer.City = volunteer.City;
            if (file != null)
            {
                string fileName = "~/Uploads/ImageLibrary/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                string filePath = Server.MapPath(fileName);
                file.SaveAs(filePath);
                oVolunteer.PhotoPath = fileName;
            }
            oVolunteer.LinkedIn = volunteer.LinkedIn;
            oVolunteer.IsProfileComplete = true;
            if (oVolunteer.Id > 0)
            {
                repository.Put(oVolunteer.Id, oVolunteer);
            }
            else
            {
                var userRole = new RoleRepository().Get().Where(x => x.Code == (int)EnumUserRole.Volunteer)
                        .FirstOrDefault();
                string password = Membership.GeneratePassword(8, 4);
                oVolunteer.user = new user()
                {
                    RowGuid = Guid.NewGuid(),
                    Email = oVolunteer.VolunteerEmail,
                    Username = oVolunteer.VolunteerEmail,
                    RegistrationDate = DateTime.Now,
                    FirstName = oVolunteer.VolunteerName,
                    RoleId = userRole.Id,
                    CreatedAt = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    FirstLogin = false,
                    IsMobileVerified = false,
                    IsEmailVerified = false,
                    CreatedBy = cu != null ? cu.OUser.Id : 0,
                    Password = EncryptionKeys.Encrypt(password)
                };
                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel emodel =
                    new EmailTemplateModel
                    {
                        Title = "Volunteer Registration",
                        RedirectUrl = url,
                        VolunteerName = oVolunteer.VolunteerName
                    };
                string body =
                    Util.RenderViewToString(bogusController.ControllerContext, "VolunteerRegister", emodel);
                EmailSender.SendSupportEmail(body, oVolunteer.VolunteerEmail);

                repository.Post(oVolunteer);

                var role = new RoleRepository().GetByCode((int)EnumUserRole.Approver1);
                var user = new AccountRepository().GetByRoleId(role.Id);
                url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
               bogusController = Util.CreateController<EmailTemplateController>();
               emodel =
                    new EmailTemplateModel
                    {
                        Title = "Volunteer Request Sent",
                        RedirectUrl = url,
                        VolunteerName = oVolunteer.VolunteerName
                    };
               body =
                    Util.RenderViewToString(bogusController.ControllerContext, "VolunteerRequest", emodel);
                EmailSender.SendSupportEmail(body, user.Email);


                cu = new ContextUser
                {
                    OUser = new user
                    {
                        Username = oVolunteer.VolunteerName,
                        Email = oVolunteer.VolunteerEmail,
                        Id = oVolunteer.UserId
                    },
                    EnumRole = EnumUserRole.Volunteer,
                    FullName = "",
                    ProfileUrl = ""
                };
                Session["user"] = cu;


            }
            if (Request["editprofile"] != null)
                return RedirectToAction("VolunteerProfile", new { editprofile = true });

            return RedirectToAction("VolunteerProfile");

        }
    }
}