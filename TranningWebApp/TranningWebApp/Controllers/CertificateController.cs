using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TranningWebApp.Common;
using TranningWebApp.Controllers;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TmsWebApp.Controllers
{
    [AuthorizeUser(AccessLevel = "SuperAdmin")]
    public class CertificateController : BaseController
    {
        // GET: Certificate
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

            IEnumerable<certificate> certificates;
            var repository = new CertificateRepository();
           
            if (string.IsNullOrEmpty(filter))
            {
                certificates = repository.Get();
            }
            else
            {
                certificates = repository.Get().Where(x => x.Name.ToLower().Contains(filter.ToLower()));
            }
            //Sorting order
            certificates = certificates.OrderBy(x => x.Name);
            ViewBag.Count = certificates.Count();

            return View(certificates.ToPagedList(page, pageSize));
        }

        public ActionResult Edit(Guid? id)
        {
            certificate certificateModel = null; 
            ViewBag.cctypes = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Value = "CPCertificateStudent2016", Text =  "CP Certificate Student 2016"},
                new SelectListItem { Selected = false, Value = "CPCertificateVolunteer2016", Text= "CP Certificate Volunteer 2016"},
                new SelectListItem { Selected = false, Value = "MrshidyCertificateStudent", Text= "Mrshidy Certificate Student"},
                new SelectListItem { Selected = false, Value = "MrshidyCertificateVolunteer", Text= "Mrshidy Certificate Volunteer"},
                new SelectListItem { Selected = false, Value = "PFCertificate_Student", Text= "PF Certificate Student"},
                new SelectListItem { Selected = false, Value = "PFCertificate_Volunteer", Text= "PF Certificate Volunteer"},
                new SelectListItem { Selected = false, Value = "SafeerCertificate_Student", Text= "Safeer Certificate Student"},
                new SelectListItem { Selected = false, Value = "SafeerCertificate_Volunteer", Text= "Safeer Certificate Volunteer"},
                new SelectListItem { Selected = false, Value = "SYCCertificates-Student", Text= "SYC Certificate Student"},
                new SelectListItem { Selected = false, Value = "SYCCertificates-Volunteer", Text= "SYC Certificate Volunteer"}
            };
            if (id == null)
            {
                certificateModel = new certificate(); 
            }
            else
            {
                var certificateRepo = new CertificateRepository();
                certificateModel = certificateRepo.GetByRowId(id.Value); 
            }
            return View(certificateModel);
        }
        [HttpPost]
        public ActionResult Edit(certificate cer, HttpPostedFileBase file)
        {
            var certificateRepo = new CertificateRepository();
            certificate cerModel = null;
            if (cer.Id == 0)
            {

                cerModel = new certificate();
                cerModel.RowGUID = Guid.NewGuid();
                if (file == null)
                {
                    ViewBag.cctypes = new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Value = "CPCertificateStudent2016", Text =  "CP Certificate Student 2016"},
                new SelectListItem { Selected = false, Value = "CPCertificateVolunteer2016", Text= "CP Certificate Volunteer 2016"},
                new SelectListItem { Selected = false, Value = "MrshidyCertificateStudent", Text= "Mrshidy Certificate Student"},
                new SelectListItem { Selected = false, Value = "MrshidyCertificateVolunteer", Text= "Mrshidy Certificate Volunteer"},
                new SelectListItem { Selected = false, Value = "PFCertificate_Student", Text= "PF Certificate Student"},
                new SelectListItem { Selected = false, Value = "PFCertificate_Volunteer", Text= "PF Certificate Volunteer"},
                new SelectListItem { Selected = false, Value = "SafeerCertificate_Student", Text= "Safeer Certificate Student"},
                new SelectListItem { Selected = false, Value = "SafeerCertificate_Volunteer", Text= "Safeer Certificate Volunteer"},
                new SelectListItem { Selected = false, Value = "SYCCertificates-Student", Text= "SYC Certificate Student"},
                new SelectListItem { Selected = false, Value = "SYCCertificates-Volunteer", Text= "SYC Certificate Volunteer"}
            };

                    ViewBag.NeedFile = true;
                    return View(cer);
                }
               
            }
            else
                cerModel = certificateRepo.Get(cer.Id);
            cerModel.Name = cer.Name;
            cerModel.Type = cer.Type;
            cerModel.TypeCertificate = cer.TypeCertificate;
            if (file != null)
            {
                string fileName = "~/Uploads/CertificateLibrary/" + System.IO.Path.GetFileName(file.FileName);
                string filePath = Server.MapPath(fileName);
                file.SaveAs(filePath);
                cerModel.UploadFilePath = fileName;
            }
            if (cer.Id == 0)
                certificateRepo.Post(cerModel);
            else
                certificateRepo.Put(cer.Id,cerModel);

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