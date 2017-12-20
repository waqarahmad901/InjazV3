using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TmsWebApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        //[Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(TranningWebApp.Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(TranningWebApp.Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(TranningWebApp.Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d~!@#$%^&*()_+]{8,}$", ErrorMessageResourceType = typeof(TranningWebApp.Resource.General), ErrorMessageResourceName = "PassordError")]

        public string Password { get; set; }

        [DataType(DataType.Password)] 
        [Compare("Password", ErrorMessageResourceType = typeof(TranningWebApp.Resource.General), ErrorMessageResourceName = "passwordmatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [EmailAddress(ErrorMessageResourceType = typeof(TranningWebApp.Resource.Funder), ErrorMessageResourceName = "EmailNotCorrect")]
        [Required(ErrorMessageResourceType = typeof(TranningWebApp.Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        public string Email { get; set; }
    }
}
