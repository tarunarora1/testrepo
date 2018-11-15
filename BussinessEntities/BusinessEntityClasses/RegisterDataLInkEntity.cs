using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessEntities.BusinessEntityClasses
{
    public class RegisterDataLInkEntity
    {
        public LinkDetailEntity LinkDetailEntity { get; set; }

        public LinkHeader LinkHeaderEntity { get; set; }
    }

    public class LinkDetailEntity
    {
        public Guid LinkDetailId { get; set; }
        public Guid CGSLinkHeader { get; set; }
        public Guid CGSLinkDetails { get; set; }
        public Guid LinkHeader { get; set; }
        public List<ValueEntity> Value { get; set; }
    }

    public class ValueEntity
    {
        public string Key { get; set; }
        public string value { get; set; }
        public Guid LinkHeaderId { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
    }
}
