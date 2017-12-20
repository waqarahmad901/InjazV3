using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranningWebApp.Models
{
    public class SessionFeedBack
    {
        public int SessionId { get; set; }
        public int ParticipantId { get; set; }
        public string FeedBack { get; set; }
    }
}