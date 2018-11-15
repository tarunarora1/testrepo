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
    
    public partial class ResourceTypeDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ResourceTypeDetail()
        {
            this.ClientResourceDetails = new HashSet<ClientResourceDetail>();
            this.CustomerResourceDetails = new HashSet<CustomerResourceDetail>();
        }
    
        public System.Guid ResourceTypeDetailsId { get; set; }
        public System.Guid ResourceTypeHeader { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string ActiveFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientResourceDetail> ClientResourceDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerResourceDetail> CustomerResourceDetails { get; set; }
        public virtual ResourceTypeHeader ResourceTypeHeader1 { get; set; }
    }
}
