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
    
    public partial class ServiceRequestInvoiceHeader2VendorInvoice
    {
        public System.Guid ServiceRequestInvoiceHeader2VendorInvoiceId { get; set; }
        public System.Guid ServiceRequestInvoiceHeader { get; set; }
        public System.Guid VendorInvoiceHeader { get; set; }
    
        public virtual WorkOrderVendorInvoiceHeader WorkOrderVendorInvoiceHeader { get; set; }
        public virtual ServiceRequestInvoiceHeader ServiceRequestInvoiceHeader1 { get; set; }
    }
}