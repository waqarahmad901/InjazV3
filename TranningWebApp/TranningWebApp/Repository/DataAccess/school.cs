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

    public partial class school
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public school()
        {
            this.coordinator_profile = new HashSet<coordinator_profile>();
            this.sessions = new HashSet<session>();
            this.participant_profile = new HashSet<participant_profile>();
            this.orientation_training = new HashSet<orientation_training>();
        }

        public int Id { get; set; }
        public System.Guid RowGuid { get; set; }
        public int CoordinatorUserId { get; set; }
        public string PhotoPath { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public bool Stage1Submitted { get; set; }
        public bool Stage2Submitted { get; set; }
        public bool Stage2Approved { get; set; }
        public Nullable<System.DateTime> Stage2ApprovedAt { get; set; }
        public Nullable<int> Stage2ApprovedByUserID { get; set; }
        public string DocumentPath { get; set; }
        public string Status { get; set; }
        public string StageOfSchool { get; set; }
        public string TypeOfSchool { get; set; } 
        public string FaceBook { get; set; }
        public string YouTube { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<coordinator_profile> coordinator_profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<session> sessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<participant_profile> participant_profile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orientation_training> orientation_training { get; set; }
        public virtual user user { get; set; }
    }
}
