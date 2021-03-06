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
    
    public partial class WorkOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkOrder()
        {
            this.WorkOrderActions = new HashSet<WorkOrderAction>();
            this.WorkOrderAttachments = new HashSet<WorkOrderAttachment>();
            this.WorkOrderNotes = new HashSet<WorkOrderNote>();
            this.WorkOrderVendorInvoiceHeaders = new HashSet<WorkOrderVendorInvoiceHeader>();
        }
    
        public System.Guid WorkOrderId { get; set; }
        public string WorkOrderNumber { get; set; }
        public System.Guid CreatedByUser { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid LastUpdatedByUser { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public System.Guid ServiceRequest { get; set; }
        public System.Guid Vendor { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> Status { get; set; }
        public Nullable<System.DateTime> DateArriveFrom { get; set; }
        public string TimeArriveFrom { get; set; }
        public Nullable<System.DateTime> DateArriveTo { get; set; }
        public string TimeArriveTo { get; set; }
        public decimal NTE { get; set; }
        public System.Guid Client { get; set; }
        public bool IsWorkOrderSent { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string WorkOrderNumberPrefix { get; set; }
        public Nullable<int> WorkOrderNumberSeqNumber { get; set; }
        public Nullable<System.Guid> WorkOrderAttachment { get; set; }
        public string WOCreateStatus { get; set; }
    
        public virtual ServiceRequest ServiceRequest1 { get; set; }
        public virtual Status Status1 { get; set; }
        public virtual Vendor Vendor1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderAction> WorkOrderActions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderAttachment> WorkOrderAttachments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderNote> WorkOrderNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderVendorInvoiceHeader> WorkOrderVendorInvoiceHeaders { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual Client Client1 { get; set; }
    }
}
