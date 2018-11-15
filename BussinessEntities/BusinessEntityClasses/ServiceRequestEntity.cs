using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class ServiceRequestEntities
    {
        public System.Guid ServiceRequestId { get; set; }
        public System.Guid Client { get; set; }
        public string RequestNumber { get; set; }
        public System.Guid Customer { get; set; }
        public System.Guid CustomerLocation { get; set; }
        public System.Guid CreatedByUser { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid LastUpdatedByUser { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public System.DateTime DateWorkOrder { get; set; }
        public string CustomerRefNumber { get; set; }
        public decimal NTE { get; set; }
        public string Description { get; set; }
        public System.Guid ClientProblemClass { get; set; }
        public System.Guid ClientProblemCode { get; set; }
        public System.Guid ClientServiceRequestPriority { get; set; }
        public System.Guid ClientServiceRequestStatus { get; set; }
        public Guid? ClientServiceTypeId { get; set; }
        public string Notes { get; set; }
        public string DocsType { get; set; }
        public string DocsName { get; set; }
        public byte[] DocsContent { get; set; }
        public Guid WorkOrderAttachmentTypeId { get; set; }
    }

    public class ServiceUserLocation
    {
        public Guid CustomerLocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class ClientService
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
    }

    public class CustomerService
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }

    public class ServiceRequestModel
    {
        public Guid ServiceRequestId { get; set; }
        public string Clientname { get; set; }
        public Guid ClientId { get; set; }
        public Guid CustomerId { get; set; }
        public string RequestNumber { get; set; }
        public string Customername { get; set; }
        public string CustomerLocation { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime DateWorkOrder { get; set; }
        public string CustomerRefNumber { get; set; }
        public string Description { get; set; }
        public Guid? WorkOrderId { get; set; }
        public Guid CustomerLocationId { get; set; }
        public string WorkOrderExist { get; set; }
        public string ClientProblemClass { get; set; }
        public string ClientProblemCode { get; set; }
        public string ClientServiceRequestPriority { get; set; }
        public string ClientServiceRequestStatus { get; set; }
        public string ServiceType { get; set; }
        public ServiceRequestModel() { }

    }
}
