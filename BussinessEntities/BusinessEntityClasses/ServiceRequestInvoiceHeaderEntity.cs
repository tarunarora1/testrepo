
using System;
using System.Collections.Generic;

namespace BusinessEntities.BusinessEntityClasses
{
    public class ServiceRequestInvoiceHeaderEntity
    {
        public System.Guid ServiceRequestInvoiceHeaderId { get; set; }
        public System.Guid ServiceRequest { get; set; }
        public System.Guid Client { get; set; }
        public System.Guid Customer { get; set; }
        public string ServiceRequestInvoiceNumber { get; set; }
        public System.DateTime DateOfInvoice { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public System.Guid User { get; set; }
        public string InvoiceOrQuote { get; set; }
        public string ServiceType { get; set; }
        public Guid[] WorkOrderVendorInvoiceHeader { get; set; }
    }

    public class ServiceRequestInvoiceAction1
    {
        public Guid ServiceRequest {get;set;}
        public Guid ServiceRequestInvoiceHeaderId { get; set; }
        public Guid ClientId { get; set; }
        public string ActionCode { get; set; }
        public Guid Status { get; set; }
        public string Notes { get; set; }
        public Guid Userid { get; set; }
        public string Toemail { get; set; }
        public List<string> Ccemail { get; set; }
        public string Message { get; set; }
    }

    public class ServiceRequestInvoiceHeaderApiResponseEntity
    {
        public Guid ServiceRequestInvoiceHeaderApiResponseId { get; set; }
        public Guid CartID { get; set; }
        public int ResponseType { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime ResponseDate { get; set; }
        public Guid ServiceRequestInvoiceHeader { get; set; }
    }


    public class EmailDataForQuoteInvoice
    {
        public Guid ServiceRequestInvoiceHeaderId { get; set; }
        public Guid ClientId { get; set; }
        public System.Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ClientName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class EmailDataForClientDetails
    {
        public string ClientName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


}
