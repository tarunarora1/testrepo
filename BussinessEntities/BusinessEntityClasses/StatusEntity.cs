using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class StatusEntities
    {
        public Guid StatusId { get; set; }
        public Guid Client { get; set; }
        public Guid DefaultServiceRequestStatus { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public string ActiveFlag { get; set; }
        public Guid CGSServiceRequestActionId {get; set;}
    }
}
