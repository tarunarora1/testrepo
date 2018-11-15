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
    
    public partial class InvoiceStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvoiceStatus()
        {
            this.ClientServiceRequestActionStatuses = new HashSet<ClientServiceRequestActionStatus>();
            this.ServiceRequestInvoiceHeaders = new HashSet<ServiceRequestInvoiceHeader>();
        }
    
        public System.Guid InvoiceStatusId { get; set; }
        public System.Guid Client { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string ServiceRequestFlag { get; set; }
        public string WorkOrderFlag { get; set; }
        public string ActiveFlag { get; set; }
        public string DefaultFlag { get; set; }
        public string DisplayToCustomer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientServiceRequestActionStatus> ClientServiceRequestActionStatuses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequestInvoiceHeader> ServiceRequestInvoiceHeaders { get; set; }
        public virtual Client Client1 { get; set; }
    }
}
