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
    
    public partial class session
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public session()
        {
            this.orientation_training = new HashSet<orientation_training>();
            this.session_evaluationform_photo = new HashSet<session_evaluationform_photo>();
            this.session_photo = new HashSet<session_photo>();
            this.session_participant = new HashSet<session_participant>();
            this.session_time = new HashSet<session_time>();
        }
    
        public int Id { get; set; }
        public System.Guid RowGUID { get; set; } 
        public System.DateTime ProposedDateTime { get; set; }
        public Nullable<System.DateTime> ActualDateTime { get; set; }
        public bool ApprovedByAdmin { get; set; }
        public Nullable<int> DurationInMinutes { get; set; }
        public string Status { get; set; } 
        public bool IsActive { get; set; }
        public bool SendKitByMailCourier { get; set; }
        public bool ConfirmKitReceivedBySchool { get; set; }
        public bool MarkedCompletedByVolunteer { get; set; }
        public bool MarkedCompletedByCoordinator { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<int> EmailSentToCoordinator1UserID { get; set; }
        public Nullable<int> EmailSentToCoordinator1UserID2 { get; set; }
        public Nullable<int> EmailSentToCoordinator1UserID3 { get; set; } 
        public bool VolunteerReceiveSessionDetailInEmail { get; set; }
        public bool VolunteerMarkedStudentAttendenceInSession { get; set; } 
        public string StudentEvaluationCatagory { get; set; }
        public bool IsVolunteerCertificateGenerated { get; set; }  
        public Nullable<int> Country { get; set; }
    
        public virtual school school { get; set; }
        public virtual volunteer_profile volunteer_profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orientation_training> orientation_training { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<session_evaluationform_photo> session_evaluationform_photo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<session_photo> session_photo { get; set; }
        public virtual certificate certificate { get; set; }
        public virtual certificate certificate1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<session_participant> session_participant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<session_time> session_time { get; set; }
        public virtual lk_country lk_country { get; set; }
    }
}
