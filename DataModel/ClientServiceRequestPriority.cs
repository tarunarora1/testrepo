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
    
    public partial class ClientServiceRequestPriority
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientServiceRequestPriority()
        {
            this.ServiceRequests = new HashSet<ServiceRequest>();
            this.ClientPMHeaders = new HashSet<ClientPMHeader>();
        }
    
        public System.Guid ClientServiceRequestPriorityId { get; set; }
        public System.Guid Client { get; set; }
        public string PriorityName { get; set; }
        public string ActiveFlag { get; set; }
        public Nullable<decimal> SortOrder { get; set; }
        public string DefaultFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientPMHeader> ClientPMHeaders { get; set; }
        public virtual Client Client1 { get; set; }
    }
}
