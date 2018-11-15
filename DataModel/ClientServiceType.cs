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
    
    public partial class ClientServiceType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientServiceType()
        {
            this.ServiceRequests = new HashSet<ServiceRequest>();
        }
    
        public System.Guid ClientServiceTypeId { get; set; }
        public System.Guid Client { get; set; }
        public string ServiceType { get; set; }
        public string ActiveFlag { get; set; }
        public string CuatomerFacingFlag { get; set; }
        public string DefaultFlag { get; set; }
        public string ServiceTypeAbbrv { get; set; }
        public Nullable<int> ServiceTypeSeed { get; set; }
        public Nullable<int> WorkOrderSeed { get; set; }
        public Nullable<int> QuoteSeed { get; set; }
        public Nullable<int> InvoiceSeed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
        public virtual Client Client1 { get; set; }
    }
}
