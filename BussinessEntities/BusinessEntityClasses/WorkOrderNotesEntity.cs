

namespace BusinessEntities.BusinessEntityClasses
{
    public class WorkOrderNotesEntity
    {
        public System.Guid WorkOrderNotesId { get; set; }
        public System.Guid WorkOrderNotesType { get; set; }
        public System.Guid ClientId { get; set; }
        public System.Guid WorkOrder { get; set; }
        public System.DateTime CreateDateTime { get; set; }
        public System.DateTime UpdateDatetime { get; set; }
        public System.Guid User { get; set; }
        public string Notes { get; set; }
        public string DeleteFlag { get; set; }
        public string TypeDescription { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        
    }
}
