using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TranningWebApp.Resource;

namespace TranningWebApp.Models.CustomValidator
{
    public class AgeValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dob = (DateTime)value;
            DateTime now = DateTime.Today;
            int age = now.Year - dob.Year;
            if (dob > now.AddYears(-age))
                age--;
            if (age > 18)
                return ValidationResult.Success;
            return new ValidationResult(General.AgeValidation);

        }
    }
}