using System;

namespace BussinessEntities.BusinessEntityClasses
{
   public  class PmVendorCustomerLocationEntity
    {
        public Guid PMVendorCustomerLocationId { get; set; }
        public Guid[] CustomerLocation { get; set; }
        public Guid PMVendor { get; set; }
    }
}
