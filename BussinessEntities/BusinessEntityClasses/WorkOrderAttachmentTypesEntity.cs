using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class WorkOrderAttachmentTypesEntity
    {
        public System.Guid WorkOrderAttachmentId { get; set; }
        public System.Guid ServiceRequestAttachmentId { get; set; }
        public System.Guid WorkOrder { get; set; }
        public System.Guid ServiceRequest { get; set; }
        public System.Guid Client { get; set; }
        public System.Guid WorkOrderAttachmentType { get; set; }
        public System.DateTime UploadedDate { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public System.Guid User { get; set; }
        public string Notes { get; set; }
        public byte[] Attachment { get; set; }
        public string TypeDescription { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public string DisplaytoCustomer { get; set; }
    }    
}
