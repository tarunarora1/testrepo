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
    
    public partial class prc_GetServiceInvoiceServiceDetailsData_Result
    {
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Total { get; set; }
        public System.Guid ServiceRequestInvoiceHeaderId { get; set; }
        public System.Guid servicerequestid { get; set; }
        public string Description { get; set; }
        public System.DateTime DateOfInvoice { get; set; }
        public string ServiceRequestInvoiceNumber { get; set; }
        public string Statuses { get; set; }
    }
}
