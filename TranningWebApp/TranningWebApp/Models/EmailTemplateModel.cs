using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TmsWebApp.Models
{
    public class EmailTemplateModel
    {
        public string Title { get; set; }
        public string RedirectUrl { get; set; }
        public string User { get; set; }
        public string Email{ get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VolunteerName { get; set; }
        public string CoordinatorName { get; set; }
        public string SessionTitle { get; set; }
        public string OTSubject { get; set; }
        public string ParticipantName { get; set; }
        public string FunderName { get; set; }
        public string Comment { get; set; }

    }
}