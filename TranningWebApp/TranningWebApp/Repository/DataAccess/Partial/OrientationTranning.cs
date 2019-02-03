using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TmsWebApp.HelpingUtilities;

namespace TranningWebApp.Repository.DataAccess
{
    public partial class orientation_training
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> OTDateTime { get; set; }
        [Url(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorUrlFormat")]
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string Location { get; set; }
        public string Subject { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string ContactPersonName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string ContactPersonPhone { get; set; }
        public string OtDateIslamic
        {
           get
            {
                if (!string.IsNullOrEmpty(OTDateTime?.ToShortDateString()))
                    return Util.DateConversion(OTDateTime.Value.ToString("dd/MM/yyyy"), "Hijri", "en-us");
                return "";
            }

        }
        public string OtDateIslamicEnd
        {
            get
            {
                if (!string.IsNullOrEmpty(OTDateEnd?.ToShortDateString()))
                    return Util.DateConversion(OTDateEnd.Value.ToString("dd/MM/yyyy"), "Hijri", "en-us");
                return "";
            }

        }

        public List<SelectListItem> Volunteers { get; set; }
        public List<SelectListItem> Schools { get; set; }

        public int NumberOfAttendene {
            get { return string.IsNullOrEmpty(VolunteersIds) ? 0 : VolunteersIds.Split(',').Count(); }
        }

        public bool IsOTOccured
        {
            get;
            set;
        }
    }
}