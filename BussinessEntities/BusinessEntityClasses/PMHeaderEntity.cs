using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessEntities.BusinessEntityClasses
{
   public class PMHeaderEntity
    {
        public System.Guid ClientPMHeaderId { get; set; }
        public System.Guid Customer { get; set; }
        public System.Guid ProblemClass { get; set; }
        public System.Guid ProblemCode { get; set; }
        public System.Guid RequestPriority { get; set; }
        public System.Guid ServiceRequestType { get; set; }
        public string CustomerReference { get; set; }
        public string IssueDescription { get; set; }
        public System.Guid Frequency { get; set; }
        public System.DateTime BeginDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime ArriveDateAndTime { get; set; }
        public System.DateTime FinishDateAndTime { get; set; }
        public int WOInAdvance { get; set; }
        public string ActiveFlag { get; set; }
        public System.Guid Client { get; set; }
        public Guid UserId { get; set; }
    }

    public class PMVendorsEntity
    {
        public System.Guid PMVendorId { get; set; }
        public System.Guid PMHeader { get; set; }
        public string Vendor { get; set; }
        public decimal WONTE { get; set; }
        public string Description { get; set; }
    }

    public class PMCustomerLOcations
    {
        public Guid[] SelectedCustomerLocations { get; set; }
    }
}
