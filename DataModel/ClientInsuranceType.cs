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
    
    public partial class ClientInsuranceType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientInsuranceType()
        {
            this.ClientVendorInsurances = new HashSet<ClientVendorInsurance>();
        }
    
        public System.Guid ClientInsuranceTypeId { get; set; }
        public System.Guid Client { get; set; }
        public string Description { get; set; }
        public string ActiveFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientVendorInsurance> ClientVendorInsurances { get; set; }
        public virtual Client Client1 { get; set; }
    }
}