using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TranningWebApp.Models
{
    public class DashBoardModel
    {
        public int SchoolPendingCount { get; set; }
        public int SchoolApprovedCount { get; set; }
        public int ParticipantEnrolledCount { get; set; }
        public int ParticipantCertificateCount { get; set; }
        public int volunteerPendingCount { get; set; }
        public int volunteerRejectedCount { get; set; }
        public int volunteerAvailableCount { get; set; }
        public int SessionPendingCount { get; set; }
        public int SessionActiveCount { get; set; }
        public int SessionOccuredCount { get; set; }
        public int SchoolInitialCount { get; set; }
        public int ParticipantInProgressCount { get; set; }

        public List<SessionClander> Sessions { get; set; }

        public string sessionsJson
        {
            get
            {
                return JsonConvert.SerializeObject(Sessions);
            }
        }

    }
    public class SessionClander
    {
        public string title { get; set; }
        public string start { get; set; }
        public string color { get; set; }
        public string url { get; set; }
    }
}