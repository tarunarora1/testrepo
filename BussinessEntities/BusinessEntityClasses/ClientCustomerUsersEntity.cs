using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class ClientCustomerUsersEntity
    {
        public System.Guid ClientID { get; set; }
        public System.Guid CustomerID { get; set; }
        public string Customername { get; set; }
        public System.Guid ClientCustomerID { get; set; }
        public System.Guid RoleID { get; set; }
        public System.Guid UserID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
        public Guid CustomerUserId { get; set; }
    }

    public class RoleEntities
    {
        public System.Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class CustomerUsers
    {
        public CustomerUser CustomerUserEntity { get; set; }
        public User UserEntity { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //public Guid Customer { get; set; }
        //public string Active { get; set; }
        //public Guid Client { get; set; }
        //public Guid Role { get; set; }
        public Guid CustomerLocationID { get; set; }
        public Guid[] CustomerLocation { get; set; }
        //public Guid UserId { get; set; }
        //public Guid CustomerUserId {get; set;}
    } 
    
    public class BindLocationList
    {
        public Guid CustomerLocation { get; set; }
    }  
}

