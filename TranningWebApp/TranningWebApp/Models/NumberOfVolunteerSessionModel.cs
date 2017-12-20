using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranningWebApp.Models
{
    public class NumberOfVolunteerSessionModel
    {
        public string VolunteerName { get; set; }
        public int SessionCount { get; set; }
        public byte[] Graph { get; set; }
    }
}