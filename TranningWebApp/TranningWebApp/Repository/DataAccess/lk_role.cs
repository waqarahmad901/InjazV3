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
    
    public partial class lk_role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public lk_role()
        {
            this.admin_user = new HashSet<admin_user>();
            this.admin_user1 = new HashSet<admin_user>();
            this.users = new HashSet<user>();
        }
    
        public short Id { get; set; }
        public string Name { get; set; }
        public short Code { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
        public short DisplayOrder { get; set; }
        public string Description { get; set; }
        public System.Guid RowGuid { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public string FrindlyName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<admin_user> admin_user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<admin_user> admin_user1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users { get; set; }
    }
}
