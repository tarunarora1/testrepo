using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class VendorInvoiceEntity
    {
        public Guid ClassOfGoodId { get; set; }
        public System.Guid Client { get; set; }
        public string Name { get; set; }
        public string ActiveFlag { get; set; }
        public string TaxClassOfGoods { get; set; }
        public System.Guid VendorInvoiceHeaderId { get; set; }
        public System.Guid WorkOrder { get; set; }
        public string InvoiceOrQuote { get; set; }
        public string Description { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] Attachment { get; set; }
        public System.Guid Vendor { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public System.DateTime DateOfInvoice { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public System.Guid User { get; set; }
        public System.Guid VendorInvoiceDetailId { get; set; }
        public System.Guid VendorInvoiceHeader { get; set; }
        
        public string Notes { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public List<ClassOfGood> ClassOfGood { get; set; }

        public System.Guid ServiceRequestId { get; set; }

       public string Email { get; set; }
        public string Telephone { get; set; }

        public string VendorName { get; set; }
    }

    public class ClassOfGood
    {
        public Guid ClassOfGoodId { get; set; }
        public string Notes { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public string Name { get; set; }
        public decimal TotalAmountAfterTax { get; set; }
    }

    public class ServiceClassOfGood
    {
        public Guid ClassOfGoodId { get; set; }
        public Guid Client { get; set; }
        public DateTime DateOfInvoice { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal SAmount { get; set; }
        public string SNotes { get; set; }
        public decimal STax { get; set; }
        public decimal STotal { get; set; }
        public Guid ServiceRequestInvoiceDetailId { get; set; }
        public Guid ServiceRequestInvoiceHeaderId { get; set; }
        public string ServiceRequestInvoiceNumber { get; set; }
        public decimal WAmount { get; set; }
        public string WNotes { get; set; }
        public decimal WTax { get; set; }
        public decimal WTotal { get; set; }
        public string TaxClassOfGoods { get; set; }
    }

    public class SalesTaxClouddetails
    {
        public string URL { get; set; }
        public string APIKey { get; set; }
    }
    
}
