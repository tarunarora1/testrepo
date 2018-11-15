using DataModel;
using System;


namespace BusinessEntities.BusinessEntityClasses
{
    public class CreateNewUsersEntity
    {
        public User UserEntity { get; set; }
        public Customer CustomerEntity { get; set; }
        public ClientCustomer ClientCustomerEntity { get; set; }
        public CustomerUser CustomerUserEntity { get; set; }
        public ClientUser ClientUserEntity { get; set; }
        public Guid LoggedInUserID { get; set; }  
        public string ActiveFlag { get; set; }
    }   
}
