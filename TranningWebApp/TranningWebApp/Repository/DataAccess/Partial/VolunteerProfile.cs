using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TmsWebApp.HelpingUtilities;
using TranningWebApp.Models.CustomValidator;

namespace TranningWebApp.Repository.DataAccess
{
    public partial class volunteer_profile
    { 
        public string SubmitButton { get; set; }
        public string OTDateString { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string VolunteerName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")]
        public string VolunteerEmail { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string VolunteerMobile { get; set; }


        public string Gender { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "NationalIdValidation")]
        public string NationalID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string AcademicQualification { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public System.DateTime DateOfBirth { get; set; }
        public string CompanyName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string VolunteerExperince1 { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string Telephone { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string Region { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string City { get; set; }

        public string LinkedIn { get; set; }

        public string[] SelectedExp { get; set; }
        public SelectList List { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string VolExp { get; set; }

        public string IslamicDate
        {
            get
            {
                if (!string.IsNullOrEmpty(DateOfBirth.ToShortDateString()))
                    return Util.DateConversion(DateOfBirth.ToString("dd/MM/yyyy"), "Hijri", "en-us");
                return "";
            }

        }
    }
}