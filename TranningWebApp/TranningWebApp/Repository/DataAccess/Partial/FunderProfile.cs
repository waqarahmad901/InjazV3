using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranningWebApp.Repository.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class funder_profile
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string FunderName { get; set; } 
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string PhoneNumber { get; set; } 
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string FunderMobile { get; set; } 
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")]
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string FunderEmail { get; set; }
        //[Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d~!@#$%^&*()_+]{8,}$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PassordError")]
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string Password { get; set; }
         
        public string City { get; set; } 
        [StringLength(10, MinimumLength = 10, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "NationalIdValidation")]
        public string NationId { get; set; }
        public string FunderName1 { get; set; }
        public string FatherName1 { get; set; }
        public string FaimlyName1 { get; set; }
        public string NationId1 { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string PhoneNumber1 { get; set; }
        public string City1 { get; set; }
        public string PartenerWebsite { get; set; }
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")] 
        public string Email1 { get; set; }
    }
}