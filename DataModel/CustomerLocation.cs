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
    
    public partial class CustomerLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerLocation()
        {
            this.CustomerLocationUsers = new HashSet<CustomerLocationUser>();
            this.ServiceRequests = new HashSet<ServiceRequest>();
            this.PMVendorCustomerLocations = new HashSet<PMVendorCustomerLocation>();
        }
    
        public System.Guid CustomerLocationId { get; set; }
        public System.Guid Customer { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string Address01 { get; set; }
        public string Address02 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip01 { get; set; }
        public string Zip02 { get; set; }
        public string Telephone { get; set; }
        public string ActiveFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerLocationUser> CustomerLocationUsers { get; set; }
        public virtual Customer Customer1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMVendorCustomerLocation> PMVendorCustomerLocations { get; set; }
    }
}