using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
using TmsWebApp.Common;
using TmsWebApp.HelpingUtilities;

namespace TranningWebApp.Repository.DataAccess
{
    public partial class session
    {
        public SessionStatus EnumSessionStatus { get; set; }
        public string SubmitButton { get; set; }
        public bool ChangeDateVisible { get; set; }
        public bool EvaluationFormFilled { get; set; }
        public bool IsPreFilledByStudent { get; set; }
        public bool IsPostFilledByStudent { get; set; }
        public bool IsFilledVolunteer { get; set; }
        public bool IsFilledCoordinator { get; set; }


        public bool IsAttendenseMarked { get; set; }
        public bool IsSelected { get; set; }

        public bool SessionOccured { get { return true; } }

  

        public string SessionImageLink { get; set; }
        public string EvaluationImageLink { get; set; }

        public IPagedList<participant_profile> PagedParticipants { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string ProgramName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string ProgramPurpose { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public Nullable<int> SchoolID { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string TargetGroup { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public Nullable<int> VolunteerId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public Nullable<int> StudentCertificate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public Nullable<int> VolunteerCertificate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string PropesedDateString { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string ActualDateString { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]


        public string PropesedDateStringIslamic
        {
            get
            {
                if (!string.IsNullOrEmpty(PropesedDateString))
                    return Util.DateConversion(PropesedDateString, "Hijri", "en-us");
                return "";
            }

        }
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public string ActualDateStringStringIslamic
        {
          get
            {
                if (!string.IsNullOrEmpty(ActualDateString))
                    return Util.DateConversion(ActualDateString, "Hijri", "en-us");
                return "";
            }
        }
        public bool RequestThisSession => VolunteerId != null;
        [Required(ErrorMessageResourceType = typeof(Resource.General), ErrorMessageResourceName = "ValidatorRequired")]

        public int NumberOfStudents { get; set; }
        public int NumberOfActualStudents { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public Nullable<System.DateTime> ProposedEndDateTime { get; set; }

        public string PropesedEndDateStringIslamic
        {
            get
            {
                if (ProposedEndDateTime!=null)
                    return Util.DateConversion(ProposedEndDateTime.Value.ToString("dd/MM/yyyy"), "Hijri", "en-us");
                return "";
            }

        }

        public string ActualEndDateStringIslamic
        {
            get
            {
                if (ActualEndDateTime != null)
                    return Util.DateConversion(ActualEndDateTime.Value.ToString("dd/MM/yyyy"), "Hijri", "en-us");
                return "";
            }

        }

        public Nullable<System.DateTime> ActualEndDateTime { get; set; }
    }
}