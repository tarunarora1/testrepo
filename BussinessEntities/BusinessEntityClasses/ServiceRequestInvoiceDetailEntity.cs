using System;
using System.Collections.Generic;

namespace BusinessEntities.BusinessEntityClasses
{
    public class ServiceRequestInvoiceDetailEntity
    {
        //public List<ServiceInvoice> VendorInvoice { get; set; }
    }

    public class ServiceInvoice
    {
        public Guid ServiceRequestInvoiceDetailId { get; set; }
        public Guid ServiceRequestInvoiceHeaderId { get; set; }
        public Guid ClassOfGoodId { get; set; }
        public Guid ServiceRequest { get; set; }
        public Guid Client { get; set; }
        public Guid User { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }        
        public string SNotes { get; set; }
        public string Description { get; set; }
        public decimal SAmount { get; set; }
        public decimal STax { get; set; }
        public decimal STotal { get; set; }        
        public byte[] Attachment { get; set; }
        public decimal _TotalAmountAfterTaxForAllTypes { get; set; }
        public string ModalStatus { get; set; }
        public string Name { get; set; }
    }
}
