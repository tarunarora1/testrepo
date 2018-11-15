using System;
using DataModel;
using System.Linq;


namespace FacilitiesUserManagement.UserClasses
{
    public static class VM_CustomerUsers
    {
        public static void AddDataInCustomerUsers(CustomerUser item)
        {
            CustomerUser CU = new CustomerUser();
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    var RoleId = _dbContext.Roles.Where(a => a.RoleName == "Customer Admin" && a.ActiveFlag == "Y").FirstOrDefault();
                    
                    if (RoleId != null)
                    {
                        CU.CustomerUserId = Guid.NewGuid();
                        CU.Customer = item.Customer;
                        CU.Role = RoleId.RoleId;
                        CU.User = item.User;
                        CU.ActiveFlag = "Y";
                        _dbContext.CustomerUsers.Add(CU);
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void UpdateDataInCustomerUsers(CustomerUser item)
        {
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    CustomerUser CU = _dbContext.CustomerUsers.First(p => p.CustomerUserId == item.CustomerUserId);
                    if (CU != null)
                    {
                        CU.ActiveFlag = item.ActiveFlag;
                        CU.Role = item.Role;
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
