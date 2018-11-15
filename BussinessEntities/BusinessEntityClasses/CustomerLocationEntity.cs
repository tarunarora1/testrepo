namespace BusinessEntities.BusinessEntityClasses
{
    public class CustomerLocationEntity
    {
        public System.Guid CustomerLocationId { get; set; }
        public System.Guid Customer { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string Address01 { get; set; }
        public string Address02 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip01 { get; set; }
        public string Zip02 { get; set; }
        public string Telephone { get; set; }
        public string ActiveFlag { get; set; }
        public System.Guid PMVendor { get; set; }
        public string CustomerName { get; set; }

        public System.Guid PMVendorCustomerLocationId { get; set; }
    }
}
