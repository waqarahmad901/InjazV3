using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranningWebApp.Models
{
    public class PdfDataModel
    {
        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
        public string city { get; set; }
        public string graphString { get; set; }

    }
}