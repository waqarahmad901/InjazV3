using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranningWebApp.Models
{
    public class DashBoardClanderModel
    {
        public List<string> dates { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }

        public string fulldate { get { return startdate + " - " + enddate; } }
    }
}