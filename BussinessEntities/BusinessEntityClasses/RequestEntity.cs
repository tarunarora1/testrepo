using System;

namespace BusinessEntities.BusinessEntityClasses
{
    public class RequestEntity
    {
        public System.Guid ClientServiceRequestPriorityId { get; set; }
        public System.Guid Client { get; set; }
        public string PriorityName { get; set; }
        public string ActiveFlag { get; set; }
        public Nullable<decimal> SortOrder { get; set; }
        public string DefaultFlag { get; set; }
    }
}
