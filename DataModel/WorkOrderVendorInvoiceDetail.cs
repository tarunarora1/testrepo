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
    
    public partial class WorkOrderVendorInvoiceDetail
    {
        public System.Guid VendorInvoiceDetailId { get; set; }
        public System.Guid VendorInvoiceHeader { get; set; }
        public System.Guid ClassOfGoodId { get; set; }
        public string Notes { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    
        public virtual WorkOrderVendorInvoiceHeader WorkOrderVendorInvoiceHeader { get; set; }
    }
}
