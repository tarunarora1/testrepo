//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class CGSFileType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CGSFileType()
        {
            this.ClientVendors = new HashSet<ClientVendor>();
            this.WorkOrderAttachments = new HashSet<WorkOrderAttachment>();
            this.WorkOrderAttachments1 = new HashSet<WorkOrderAttachment>();
            this.ServiceRequestAttachments = new HashSet<ServiceRequestAttachment>();
            this.CGSThemeTemplates = new HashSet<CGSThemeTemplate>();
        }
    
        public System.Guid CGSFileTypesId { get; set; }
        public string Decription { get; set; }
        public string Extention { get; set; }
        public string ActiveFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientVendor> ClientVendors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderAttachment> WorkOrderAttachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderAttachment> WorkOrderAttachments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequestAttachment> ServiceRequestAttachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CGSThemeTemplate> CGSThemeTemplates { get; set; }
    }
}
