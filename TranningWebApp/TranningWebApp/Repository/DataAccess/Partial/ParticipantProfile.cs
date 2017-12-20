using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TmsWebApp.HelpingUtilities;

namespace TranningWebApp.Repository.DataAccess
{
    using System;
    using System.Collections.Generic;

    public partial class participant_profile
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d~!@#$%^&*()_+]{8,}$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PassordError")]

        public string Password { get; set; }
        public int SessionId{ get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "NationalIdValidation")]
         
        public string NationalID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string Mobile { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string Gender { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string ProgrammName1 { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string FatherMobile { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string City { get; set; }
        public string FacebookAddress { get; set; }
        public string TwitterAddress { get; set; }
        public string SnapChatAddress { get; set; }
        public string Stage { get; set; }
        public Nullable<int> SchoolId { get; set; }

        public string Instagram { get; set; }
        public bool IsSelected { get; set; }
        public string Class { get; set; }
        public string MobileNo { get; set; }
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