using BusinessEntities.BusinessEntityClasses;
using DataModel;
using FacilitiesUserManagement.UserClasses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitOfwork.Interfaces;

namespace UnitOfwork.UOWRepository
{
    public class UOWManageUsers : IManageUsers
    {
        public List<ClientUsersEntity> GetClientUsers(Guid LoggedInUser)
        {
            List<ClientUsersEntity> GetClientUsers = new List<ClientUsersEntity>();

            using (FacilitiesEntities dB = new FacilitiesEntities())
            {
                var GetClient = dB.ClientUsers.Where(p => p.User == LoggedInUser).FirstOrDefault();

                if (GetClient != null)
                {
                    GetClientUsers = (from CU in dB.ClientUsers
                                      join U in dB.Users on CU.User equals U.UserId
                                      join R in dB.Roles on CU.Role equals R.RoleId
                                      where CU.Client == GetClient.Client
                                      select new
                                      {
                                          _Clientuserid = CU.ClientUserId,
                                          _Clientid = CU.Client,
                                          _RoleID = R.RoleId,
                                          _Userid = CU.User,
                                          _RoleName = R.RoleName,
                                          _ActiveFlag = U.ActiveFlag == "R"?U.ActiveFlag:CU.ActiveFlag,
                                          _Firstname = U.FirstName,
                                          _Lastname = U.LastName,
                                          _Email = U.Email
                                      }).ToList().Select(W => new ClientUsersEntity()
                                      {
                                          ClientID = W._Clientid,
                                          Email = W._Email,
                                          ClientUserID = W._Clientuserid,
                                          Firstname = W._Firstname,
                                          Lastname = W._Lastname,
                                          RoleID = W._RoleID,
                                          UserID = W._Userid,
                                          RoleName = W._RoleName,
                                          Status = UserStatus(W._ActiveFlag)
                                      }).ToList();
                }
            }
            return GetClientUsers;
        }
        public string UserStatus(string ActiveFlag)
        {
            if (ActiveFlag == "Y")
            {
                return "Active";
            }
            else if(ActiveFlag == "R")
            {
                return "Registration Required";
            }
            else
            {
                return "InActive";
            }
        }

        public List<ClientCustomerUsersEntity> GetClientCustomersUsers(Guid LoggedInUser)
        {
            List<ClientCustomerUsersEntity> ObjClientCustomersUsers = new List<ClientCustomerUsersEntity>();
            using (FacilitiesEntities dB = new FacilitiesEntities())
            {
                var GetClient = dB.ClientUsers.Where(p => p.User == LoggedInUser).FirstOrDefault();
                if (GetClient != null)
                {
                    var GetCustomerAdminRole = dB.Roles.Where(p => p.RoleName.Trim() == "Customer Admin".Trim()).FirstOrDefault();
                    if (GetCustomerAdminRole != null)
                    {


                        ObjClientCustomersUsers = (from ClCust in dB.ClientCustomers
                                                   join CU in dB.CustomerUsers on ClCust.Customer equals CU.Customer
                                                   join U in dB.Users on CU.User equals U.UserId
                                                   join C in dB.Customers on ClCust.Customer equals C.CustomerId
                                                   where ClCust.Client == GetClient.Client && CU.Role == GetCustomerAdminRole.RoleId && C.ActiveFlag == "Y"
                                                   select new
                                                   {
                                                       _ClientID = ClCust.Client,
                                                       _CustomerID = ClCust.Customer,
                                                       _Customername = C.CustomerName,
                                                       _ClientCustomerID = ClCust.ClientCustomerId,
                                                       _RoleID = CU.Role,
                                                       _UserID = U.UserId,
                                                       _CustomerUserId = CU.CustomerUserId,
                                                       _ActiveFlag = U.ActiveFlag == "R" ? U.ActiveFlag : CU.ActiveFlag,
                                                       _Firstname = U.FirstName,
                                                       _Lastname = U.LastName,
                                                       _Email = U.Email
                                                   }
                                                   ).ToList().Select(W => new ClientCustomerUsersEntity()
                                                   {
                                                       ClientCustomerID = W._ClientCustomerID,
                                                       ClientID = W._ClientID,
                                                       CustomerID = W._CustomerID,
                                                       Customername = W._Customername,
                                                       Email = W._Email,
                                                       Firstname = W._Firstname,
                                                       Lastname = W._Lastname,
                                                       RoleID = W._RoleID,
                                                       UserID = W._UserID,
                                                      CustomerUserId =W._CustomerUserId,
                                                       Status = UserStatus(W._ActiveFlag)
                                                   }).ToList();
                    }
                }
            }
            return ObjClientCustomersUsers;
        }

        public string GetUserOktaUserID(string Email)
        {
            string OktaUserID = string.Empty;
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var GetUsers = db.Users.Where(p => p.Email.ToLower() == Email.ToLower()).FirstOrDefault();
                if (GetUsers != null)
                    OktaUserID = GetUsers.OktaUserID;
                var C = VM_OktaUrlAndKey.GetOktaUrlAndKey();
                var client = new RestClient(C.Item1 + "/" + OktaUserID + "/credentials/forgot_password?sendEmail=true");
                var request = new RestRequest(Method.POST);
                request.AddHeader("authorization", C.Item2);
                IRestResponse response = client.Execute(request);
            }
            return OktaUserID;
        }

        public bool ChangePassword(ChangePasswordEntity data)
        {
            var C = VM_OktaUrlAndKey.GetOktaUrlAndKey();
            var client = new RestClient(C.Item1 + "/" + data._loggedInOktaUserID + "/credentials/change_password");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", C.Item2);
            string ss = "{\n  \"oldPassword\": { \"value\": \"" + data._OldPass + "\" },\n  \"newPassword\": { \"value\": \"" + data._NewPass + "\" }\n}";
            request.AddParameter("application/json", ss, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return true;
        }
    }
}
