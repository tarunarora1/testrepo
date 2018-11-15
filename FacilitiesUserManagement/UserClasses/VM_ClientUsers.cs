using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacilitiesUserManagement.UserClasses
{
    public static class VM_ClientUsers
    {
        public static void AddDataInClientUser(ClientUser item)
        {
            ClientUser CU = new ClientUser();
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    var GetClientRoleId = _dbContext.Roles.Where(a => a.RoleName == "Client User" && a.ActiveFlag == "Y").FirstOrDefault();
                    if (GetClientRoleId != null)
                    {
                        CU.ClientUserId = Guid.NewGuid();
                        CU.Client = item.Client;
                        CU.Role = GetClientRoleId.RoleId;
                        CU.User = item.User;
                        CU.ActiveFlag = "Y";
                        _dbContext.ClientUsers.Add(CU);
                        _dbContext.SaveChanges();
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void UpdateDataInClientUser(ClientUser item)
        {            
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    ClientUser CU = _dbContext.ClientUsers.First(p => p.User == item.User);
                    if (CU != null)
                    {
                        CU.ActiveFlag = item.ActiveFlag;  
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
