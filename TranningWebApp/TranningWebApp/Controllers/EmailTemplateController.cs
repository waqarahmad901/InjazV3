using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TmsWebApp.Models;
using TranningWebApp.Controllers;

namespace TmsWebApp.Controllers
{
    public class EmailTemplateController : BaseController
    {
        // GET: EmailTemplate
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CoordinatorRegistration(EmailTemplateModel model)
        {
            return View(model);
        }
        public ActionResult CoordinatorProfile(EmailTemplateModel model)
        {
            return View(model);
        }
    }

}