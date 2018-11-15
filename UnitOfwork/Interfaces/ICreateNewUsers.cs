using BusinessEntities.BusinessEntityClasses;
using BussinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;

namespace UnitOfwork.Interfaces
{
    public interface ICreateNewUsers
    {
        Guid AddNewCustomerRequest(CreateNewUsersEntity NewCustomer);
        string GetUserEmailId(Guid UserId);
        bool ActivateRegistrationUsers(User item);
        Guid AddNewClientRequest(CreateNewUsersEntity NewClient);                
        //bool ActivateClientAdminUser(string Email, string OKTAUserId);
        //bool ActivateCustomerUser(string Email, string OKTAUserId);
        //bool ActivateVendorAdminUser(string Email, string OKTAUserId);        
        IEnumerable<RoleEntities> GetCustomerRoles();
        Guid SaveCustomerUsers(CustomerUsers item);
        IEnumerable<ClientCustomerUsersEntity> GetCustomerUserGridData(Guid CustomerId);
        List<CustomerLocationEntity> GetLocationWithAddress(Guid CustomerId);
        IEnumerable<BindLocationList> BindCustomerLocation(Guid Userid);
        IEnumerable<GetCustomerLocationUsers_Result> GetAssociateCustomerLocationusers(Guid LocationId);
        IEnumerable<GetCustomerEmailAddressForInvoiceQuote_Result> GetAssociateCustomerLocationusers1(Guid LocationId);
        object RegisterUser(RegisterUserEntity item);
        bool RegisterUserData(RegisterDataLInkEntity item);
        Tuple<bool, string, string> GetLinkExpiyTime(string RandamString);
        List<ValueEntity> GetLinkDetailData(string RandamString);
        string DeleteDataFromLinkTables(Guid LinkHeaderID);
        //bool ConvertLogoToByte(Guid LoggedInUser);
    }
}
