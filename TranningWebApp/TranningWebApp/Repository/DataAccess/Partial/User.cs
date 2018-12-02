using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TranningWebApp.Repository.DataAccess
{
    public partial class user
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")]
        [Remote("EmailAlreadyExistUser", "Home", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "EmailAlreadyExist")]

        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [Remote("UserAlreadyExist", "Home", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "UserAlreadyExist")]

        public string Username { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d~!@#$%^&*()_+]{8,}$", ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "PassordError")]
        public string Password { get; set; }

        public short RoleId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string MiddleName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string LastName { get; set; }
    }
}