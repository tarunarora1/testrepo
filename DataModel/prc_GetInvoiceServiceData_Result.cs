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
    
    public partial class prc_GetInvoiceServiceData_Result
    {
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Total { get; set; }
        public System.Guid VendorInvoiceHeaderid { get; set; }
        public System.Guid workorderid { get; set; }
        public System.Guid servicerequestid { get; set; }
        public System.Guid vendor { get; set; }
        public string Description { get; set; }
        public System.DateTime DateOfInvoice { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string VendorName { get; set; }
        public Nullable<System.Guid> SVendorId { get; set; }
    }
}