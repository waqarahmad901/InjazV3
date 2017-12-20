using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TmsWebApp.HelpingUtilities;

namespace TranningWebApp.Repository.DataAccess
{

    public partial class coordinator_profile
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d~!@#$%^&*()_+]{8,}$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PassordError")]

        public string Password { get; set; }
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")]
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string CoordinatorEmail { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string Gender { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string CoordinatorMobile { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "NationalIdValidation")]
        public string NationalID { get; set; }
        public string Class { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")] 
        public string VolunteerActivity1 { get; set; }
       
        public string VolunteerActivity2 { get; set; }
       
        public string VolunteerActivity3 { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")] 
        public string OtherCertificate1 { get; set; }
       
        public string OtherCertificate2 { get; set; }
       
        public string OtherCertificate3 { get; set; }
        public string IslamicDate
        {
            get
            {
                if (DateOfBirth != null && !string.IsNullOrEmpty(DateOfBirth.Value.ToShortDateString()))
                    return Util.DateConversion(DateOfBirth.Value.ToString("dd/MM/yyyy"), "Hijri", "en-us");
                return "";
            }

        }

    }
}