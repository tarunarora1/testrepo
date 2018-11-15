using System;

namespace BusinessEntities.BusinessEntityClasses
{
    public class EmailEntity
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Guid ClientId { get;set;}
		public System.Guid UserId { get; set; }
        public System.Guid LoggedInUserId { get; set; }
        public string IsClientVendorOrCustomer { get; set; }
    }
}
