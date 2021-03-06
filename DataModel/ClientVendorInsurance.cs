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
    
    public partial class ClientVendorInsurance
    {
        public System.Guid ClientVendorInsuranceId { get; set; }
        public System.Guid ClientVendor { get; set; }
        public System.Guid InsuranceType { get; set; }
        public string InsuranceCompanyName { get; set; }
        public System.DateTime CoverageBeginDate { get; set; }
        public System.DateTime CoverageEndDate { get; set; }
        public byte[] InsuranceDocument { get; set; }
        public System.DateTime UploadedDate { get; set; }
        public System.Guid UploadedByUser { get; set; }
        public string FileName { get; set; }
        public System.Guid FileType { get; set; }
    
        public virtual ClientInsuranceType ClientInsuranceType { get; set; }
        public virtual ClientVendor ClientVendor1 { get; set; }
        public virtual User User { get; set; }
    }
}
