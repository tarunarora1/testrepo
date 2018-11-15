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
    
    public partial class ClientServiceRequestNoteType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClientServiceRequestNoteType()
        {
            this.ServiceRequestNotes = new HashSet<ServiceRequestNote>();
            this.ClientServiceRequestActions = new HashSet<ClientServiceRequestAction>();
        }
    
        public System.Guid ServiceRequestNoteTypeId { get; set; }
        public System.Guid Client { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActiveFlag { get; set; }
        public string VisibleToCustomer { get; set; }
        public string VisibleToVendor { get; set; }
        public string SystemGeneratedFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceRequestNote> ServiceRequestNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientServiceRequestAction> ClientServiceRequestActions { get; set; }
        public virtual Client Client1 { get; set; }
    }
}