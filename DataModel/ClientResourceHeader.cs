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
    
    public partial class ClientResourceHeader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientResourceHeader()
        {
            this.ClientResourceDetails = new HashSet<ClientResourceDetail>();
        }
    
        public System.Guid ClientResourceHeadersId { get; set; }
        public System.Guid ResourceTypeHeader { get; set; }
        public System.Guid Client { get; set; }
        public string ActiveFlag { get; set; }
        public string Alias { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientResourceDetail> ClientResourceDetails { get; set; }
        public virtual ResourceTypeHeader ResourceTypeHeader1 { get; set; }
        public virtual Client Client1 { get; set; }
    }
}
