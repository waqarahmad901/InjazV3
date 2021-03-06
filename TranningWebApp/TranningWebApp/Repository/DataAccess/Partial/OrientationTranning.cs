﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TmsWebApp.HelpingUtilities;

namespace TranningWebApp.Repository.DataAccess
{
    public partial class orientation_training
    {
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> OTDateTime { get; set; }
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
    }
}