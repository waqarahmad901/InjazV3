//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TranningWebApp.Repository.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class volunteer_profile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public volunteer_profile()
        {
            this.sessions = new HashSet<session>();
        }
    
        public int Id { get; set; }
        public System.Guid RowGuid { get; set; } 
        public string GoogleSigninId { get; set; }   
        public string VolunteerExperince2 { get; set; }
        public string VolunteerExperince3 { get; set; }
        public Nullable<bool> PerformVolunteerActivity { get; set; }
        public bool HasTOTCertificate { get; set; }
        public string OtherCertificate1 { get; set; }
        public string OtherCertificate2 { get; set; }
        public string OtherCertificate3 { get; set; }
        public string OtherCertificate4 { get; set; }
        public Nullable<System.DateTime> OTDateTime { get; set; } 
        public string PhotoPath { get; set; } 
        public int CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<bool> IsEmailSent { get; set; }
        public Nullable<bool> IsApprovedAtLevel1 { get; set; }
        public Nullable<bool> IsApprovedAtLevel2 { get; set; }
        public Nullable<bool> IsApprovedAtLevel3 { get; set; }
        public Nullable<bool> IsProfileComplete { get; set; }
        public Nullable<bool> FirstLogin { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public string ApprovedAtLevel1Comments { get; set; }
        public string ApprovedAtLevel2Comments { get; set; }
        public string ApprovedAtLevel3Comments { get; set; }
        public Nullable<int> OTId { get; set; }
        public bool OTAcceptedByVolunteer { get; set; }
        public bool OTRejectedByVolunteer { get; set; }
        public bool OTAttendenceForVolunteer { get; set; }
        public Nullable<int> OTIdAssigner { get; set; }
        public string LinkedInSignInId { get; set; }
        public string AcademicQualification1 { get; set; }
        public string AcademicQualification2 { get; set; }
        public string VolunteerActivity1 { get; set; }
        public string VolunteerActivity2 { get; set; }
        public string VolunteerActivity3 { get; set; }
        public Nullable<bool> HasTOTCertificate1 { get; set; }
        public int UserId { get; set; }
        public string RejectedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<session> sessions { get; set; }
        public virtual orientation_training orientation_training { get; set; }
        public virtual user user { get; set; }
    }
}
