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
    
    public partial class CustomerLocationUser
    {
        public System.Guid CustomerLocationUser1 { get; set; }
        public System.Guid CustomerLocation { get; set; }
        public System.Guid User { get; set; }
        public string ActiveFlag { get; set; }
    
        public virtual CustomerLocation CustomerLocation1 { get; set; }
        public virtual User User1 { get; set; }
    }
}
