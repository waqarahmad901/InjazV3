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
using TmsWebApp.Models;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;
using TranningWebApp.Repository.Lookup;

namespace TmsWebApp.Controllers
{
    [RoutePrefix("Partner")]
    [AuthorizeUser(AccessLevel = "SuperAdmin,Funder")]
    public class FunderController : BaseController
    {
        [Route("Index")]
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

            IEnumerable<funder_profile> funders;
            var repository = new FunderRepository();
            if (archive != null)
            {
                var funder = repository.GetByRowId(archive.Value);
                funder.IsActive = !funder.IsActive;
                funder.user.IsLocked = !funder.IsActive;
                  
                repository.Put(funder.Id, funder);
            }
            if (string.IsNullOrEmpty(filter))
            {
                funders = repository.Get();
            }
            else
            {
                string firstName = filter.Split(' ')[0];
                string fatherName = filter.Split(' ').Length > 1 ? filter.Split(' ')[1] : "";
                string familyName = filter.Split(' ').Length > 2?  filter.Split(' ')[2] : "";
                funders = repository.Get().Where(x => x.FunderName.ToLower().Contains(filter.ToLower()) ||
                (x.FatherName != null && fatherName != "" && x.FatherName.ToLower().Contains(fatherName.ToLower())) ||
                (x.FaimlyName != null && familyName != "" && x.FaimlyName.ToLower().Contains(familyName.ToLower()))
                );
            }  
            //Sorting order
            funders = funders.OrderByDescending(x => x.CreatedAt);
            ViewBag.Count = funders.Count();
             
            return View(funders.ToPagedList(page, pageSize));
        }
       [Route("Edit")]
        public ActionResult Edit(Guid? id)
        { 
            funder_profile funderModel = null;
            var countries = new CountryRepository().Get().Select(x =>
                        new SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();
            ViewBag.countries = countries;
            var cities = new CityRepository().Get().Distinct().Select(x =>
              new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
            ViewBag.citiesdd = cities;
            if (id == null)
            {
                funderModel = new funder_profile();
                funderModel.IsActive = true;
                funderModel.Country = 1192;
                funderModel.PhoneNumber = "+966";
                funderModel.FunderMobile = "+966";
            }
            else
            {
                var funderRepo = new FunderRepository();
                funderModel = funderRepo.GetByRowId(id.Value);
                funderModel.Password = EncryptionKeys.Decrypt( funderModel.user.Password);
            }
            return View(funderModel);
        }
        [Route("Edit")]
        [HttpPost]
        public ActionResult Edit(funder_profile profile)
        {
            var funderRepo = new FunderRepository();
            var accountRepo = new AccountRepository();
            funder_profile funder = null;
            var cu = Session["user"] as ContextUser;
            if (profile.Id == 0)
            {
                if (accountRepo.EmailExist(profile.FunderEmail))
                {
                    var countries = new CountryRepository().Get().Select(x =>
                       new SelectListItem { Text = x.Name, Value = x.Id + "" }).ToList();
                    ViewBag.countries = countries;
                    var cities = new CityRepository().Get().Distinct().Select(x =>
                       new SelectListItem { Text = x.City + " (" + x.City_ar + ")", Value = x.City + "", Selected = x.City == "Jeddah" }).ToList();
                    ViewBag.citiesdd = cities;
                    ViewBag.EmailExist = true;
                    return View(profile);
                }
                funder = new funder_profile();
                funder.RowGUID = Guid.NewGuid();
                funder.CreatedAt = DateTime.Now;
                funder.CreatedBy = cu.OUser.Id;
                funder.FunderEmail = profile.FunderEmail;

            }
            else
            {
                funder = funderRepo.Get(profile.Id);
                funder.UpdatedAt = DateTime.Now;
                funder.UpdatedBy =cu.OUser.Id;
                
            }
             
            var userRole = new RoleRepository().Get().Where(x=>x.Code == (int)EnumUserRole.Funder).FirstOrDefault();
            if (funder.FunderUserID == 0)
                funder.user = new user
                {
                    RowGuid = Guid.NewGuid(),
                    Email = profile.FunderEmail,
                    Username = profile.FunderEmail,
                    RegistrationDate = DateTime.Now,
                    FirstName = profile.FunderName,
                    RoleId = userRole.Id,
                    CreatedAt = DateTime.Now,
                    ValidFrom = DateTime.Now,
                    FirstLogin = false,
                    IsMobileVerified = false,
                    IsEmailVerified = false,
                    CreatedBy = cu.OUser.Id,

                    Password = EncryptionKeys.Encrypt(profile.Password)
        };   
            funder.FunderName = profile.FunderName;
            funder.FatherName = profile.FatherName;
            funder.FaimlyName = profile.FaimlyName;
            funder.FunderMobile = profile.FunderMobile; 
            funder.Country = profile.Country;
            funder.PhoneNumber = profile.PhoneNumber;
            funder.IsActive = profile.IsActive;
            funder.user.IsLocked = !funder.IsActive;
            funder.PartnerType = profile.PartnerType;
            funder.TypeOfFunding = profile.TypeOfFunding;
            funder.City = profile.City;
            funder.NationId = profile.NationId;
            funder.FunderName1 = profile.FunderName1;
            funder.FatherName1 = profile.FatherName1;
            funder.FaimlyName1 = profile.FaimlyName1;
            funder.PhoneNumber1 = profile.PhoneNumber1;
            funder.NationId1 = profile.NationId1;
            funder.City1 = profile.City1;
            funder.PartenerWebsite = profile.PartenerWebsite;
            funder.Email1 = profile.Email1;

            if (profile.Id == 0)
            {

                string url = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/Account/Login";
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel model = new EmailTemplateModel { Title = "Complete Profile", RedirectUrl = url, UserName = funder.FunderEmail, Password = EncryptionKeys.Decrypt(funder.user.Password), FunderName = funder.FunderName, User = funder.user.FirstName };
                string body = Util.RenderViewToString(bogusController.ControllerContext, "FunderProfile", model);
                EmailSender.SendSupportEmail(body, funder.FunderEmail);
                funderRepo.Post(funder);

            }
            else
                funderRepo.Put(funder.Id,funder); 
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var repository = new FunderRepository();
            repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}