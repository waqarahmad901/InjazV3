using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TranningWebApp.Repository.DataAccess
{
    public partial class school
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string SchoolName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string City { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string FacebookAddress { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string District { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string Region { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string Telephone { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        public string Fax { get; set; }
        
        public string Website { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression("^[0-9]*$",ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        public int SchoolID { get; set; }
        //[RegularExpression(@"^\-?\d+\.\d{0,6}$", ErrorMessageResourceType = typeof(Resource.Coordinator), ErrorMessageResourceName = "LatError")]
        //[Range(-180.999999, 180.999999, ErrorMessageResourceType = typeof(Resource.Coordinator), ErrorMessageResourceName = "LatError")]
        public Nullable<decimal> Lat { get; set; }
        //[RegularExpression(@"^\-?\d+\.\d{0,6}$", ErrorMessageResourceType = typeof(Resource.Coordinator), ErrorMessageResourceName = "LatError")]
        //[Range(-180.999999, 180.999999, ErrorMessageResourceType = typeof(Resource.Coordinator), ErrorMessageResourceName = "LatError")]
        public Nullable<decimal> Lng { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string SchoolManagerName { get; set; }
        public string SchoolManagerEmailAddress { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string SchoolManagerMobile { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string SchoolGuardName { get; set; }
        [StringLength(10, MinimumLength = 9, ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PhoneLengthValidation")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorNumaric")]
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string SchoolGuardMobile { get; set; }
        public string LinkedInAddress { get; set; }
        public string GoogleAddress { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string SchoolGeoLocation { get; set; }
    }
}