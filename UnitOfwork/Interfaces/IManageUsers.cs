using BusinessEntities.BusinessEntityClasses;
using System;
using System.Collections.Generic;

namespace UnitOfwork.Interfaces
{
    public interface IManageUsers
    {
        List<ClientUsersEntity> GetClientUsers(Guid LoggedInUser);

        List<ClientCustomerUsersEntity> GetClientCustomersUsers(Guid LoggedInUser);
        string GetUserOktaUserID(string Email);
        bool ChangePassword(ChangePasswordEntity item);
    }
}
