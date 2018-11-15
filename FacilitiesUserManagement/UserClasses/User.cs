using DataModel;
using System;
using System.Linq;

namespace FacilitiesUserManagement.UserClasses
{
    public static class VM_User
    {        
        //static readonly FacilitiesEntities _dbContext;      
        public static User AddDataInUser<T>(User item)
        {
            User U = new User();
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    var checkUser = _dbContext.Users.Where(p => p.Email.ToLower().Trim() == item.Email.ToLower().Trim()).FirstOrDefault();
                    if (checkUser == null)
                    {
                        U.UserId = Guid.NewGuid();
                        U.FirstName = item.FirstName;
                        U.LastName = item.LastName;
                        U.Email = item.Email;
                        U.ActiveFlag = "R";
                        _dbContext.Users.Add(U);
                    }
                    else
                    {
                        U.UserId = checkUser.UserId;
                        U.ActiveFlag = checkUser.ActiveFlag;
                    }
                    _dbContext.SaveChanges();
                    return U;
                }                
            }
            catch(Exception ex)
            {
                throw (ex);                  
            }            
        }

        public static User UpdateDataInUser<T>(User item)
        {
            User U = new User();
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    U = _dbContext.Users.Where(a => a.UserId == item.UserId).FirstOrDefault();
                    if (U != null)
                    {
                        U.FirstName = item.FirstName;
                        U.LastName = item.LastName;
                        //U.ActiveFlag = item.ActiveFlag;
                        _dbContext.SaveChanges();
                    }
                    return U;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }    
}
