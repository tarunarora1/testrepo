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
    
    public partial class ServiceRequestAction
    {
        public System.Guid ServiceRequestActionID { get; set; }
        public System.Guid User { get; set; }
        public System.Guid ServiceRequestAction1 { get; set; }
        public System.Guid ServiceRequest { get; set; }
        public System.DateTime CurrentDate { get; set; }
        public string ActiveFlag { get; set; }
    
        public virtual ClientServiceRequestAction ClientServiceRequestAction { get; set; }
        public virtual ServiceRequest ServiceRequest1 { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}
