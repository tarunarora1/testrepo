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
    
    public partial class ClientVendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientVendor()
        {
            this.ClientVendorInsurances = new HashSet<ClientVendorInsurance>();
            this.ClientVendorProblemClasses = new HashSet<ClientVendorProblemClass>();
        }
    
        public System.Guid ClientVendorId { get; set; }
        public System.Guid Client { get; set; }
        public System.Guid Vendor { get; set; }
        public string ActiveFlag { get; set; }
        public byte[] TaxDocument { get; set; }
        public Nullable<System.Guid> FileType { get; set; }
        public string FileName { get; set; }
    
        public virtual CGSFileType CGSFileType { get; set; }
        public virtual Vendor Vendor1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientVendorInsurance> ClientVendorInsurances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientVendorProblemClass> ClientVendorProblemClasses { get; set; }
        public virtual Client Client1 { get; set; }
    }
}
