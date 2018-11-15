using BusinessEntities.BusinessEntityClasses;
using DataModel;
using FacilitiesUserManagement.UserClasses;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitOfWork.Interface;

namespace UnitOfWork.UOWRepository
{
    public class UserLoginRole : IUserLogin
    {
        public bool LoginUser(LoginUserEntity data)
        {
            var C = VM_OktaUrlAndKey.GetOktaUrlAndKey();
            string str = C.Item1.Replace("users", "authn"); // to replace the specific text with blank
            var client = new RestClient(str);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("accept", "application/json");
            string oktaLogin = " {\n  \"username\": \"" + data.Username + "\",\n  \"password\": \"" + data.Password + "\",\n  \"options\": {\n    \"multiOptionalFactorEnroll\": true,\n    \"warnBeforePasswordExpired\": true\n  } \n}";
            request.AddParameter("application/json", oktaLogin, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusDescription == "Unauthorized")
            {
                return false;
            }
            else
            {
                return true;
            }
            //return true;
        }

        public List<KeyValuePair<Guid, string>> GetLoginUserRole(string UserEmailId, string application = "")
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    List<KeyValuePair<Guid, string>> Authenticateuser = new List<KeyValuePair<Guid, string>>();
                    Guid CustomerID = Guid.Empty;
                    Guid UserID = Guid.Empty;
                    String UserRole = false.ToString();
                    Guid UserRoleID = Guid.Empty;

                    //Only for the client login
                    string TaxCloudAPIURL = string.Empty;
                    string TaxCloudAPICloudKey = string.Empty;
                    string ClientID = string.Empty;

                    var GetUserId = db.Users.Where(p => p.Email.ToLower() == UserEmailId.ToLower().Trim()).FirstOrDefault();

                    if (GetUserId != null)
                    {
                        UserID = GetUserId.UserId;

                        var GetClientUser = db.ClientUsers.Where(p => p.User == UserID && p.ActiveFlag == "Y").FirstOrDefault();
                        var GetCustomerUser = db.CustomerUsers.Where(p => p.User == UserID && p.ActiveFlag == "Y").FirstOrDefault();
                        var GetVendorUser = db.VendorUsers.Where(p => p.User == UserID && p.ActiveFlag == "Y").FirstOrDefault();

                        if (application.ToLower().Trim().Length == 0)
                        {
                            if (GetClientUser != null)
                                application = "client";

                            if (GetCustomerUser != null)
                                application = "customer";

                            if (GetVendorUser != null)
                                application = "vendor";
                        }

                        switch (application.Trim().ToLower())
                        {
                            case "client":
                                if (GetClientUser != null)
                                {
                                    //client user
                                    UserRoleID = GetClientUser.Role;
                                    var GetUserRole = db.Roles.Where(p => p.RoleId == UserRoleID).FirstOrDefault();

                                    if (GetUserRole != null)
                                        UserRole = GetUserRole.RoleName;

                                    //Get the clientID for the logged in user
                                    var GetClientInfo = db.ClientUsers.Where(p => p.User == UserID && p.ActiveFlag == "Y").FirstOrDefault();
                                    if (GetClientInfo != null)
                                    {
                                        ClientID = GetClientInfo.Client.ToString();
                                        //get the client resource headers mapping
                                        var AddressURLDetails = (from rth in db.ResourceTypeHeaders
                                                                 join rtd in db.ResourceTypeDetails on rth.ResourceTypeHeadersId equals rtd.ResourceTypeHeader
                                                                 join crd in db.ClientResourceDetails on rtd.ResourceTypeDetailsId equals crd.ResourceTypeDetail
                                                                 where rth.Name == "Tax Cloud Address Verification" && rtd.Description == "The Base URL for the call"
                                                                 select new
                                                                 {
                                                                     Value = crd.Value
                                                                 }).FirstOrDefault();

                                        TaxCloudAPIURL = AddressURLDetails.Value;

                                        var AddressAPIIDDetails = (from rth in db.ResourceTypeHeaders
                                                                   join rtd in db.ResourceTypeDetails on rth.ResourceTypeHeadersId equals rtd.ResourceTypeHeader
                                                                   join crd in db.ClientResourceDetails on rtd.ResourceTypeDetailsId equals crd.ResourceTypeDetail
                                                                   where rth.Name == "Tax Cloud Address Verification" && rtd.Description == "The key for this resource can be different in each environment"
                                                                   select new
                                                                   {
                                                                       Value = crd.Value
                                                                   }).FirstOrDefault();

                                        TaxCloudAPICloudKey = AddressAPIIDDetails.Value;

                                        //var getClientResourceheaders = db.ClientResourceHeaders.Where(p => p.Client == GetClientInfo.Client ).FirstOrDefault();
                                        //if (getClientResourceheaders != null)
                                        //{
                                        //    //get header details: will get the actual values of the header: URL and the Key
                                        //    var GetClientResourceDetails = db.ClientResourceDetails.Where(p => p.ClientResourceHeader == getClientResourceheaders.ClientResourceHeadersId).ToList();
                                        //    if (GetClientResourceDetails.Count > 0)
                                        //    {
                                        //        Guid GetClientResourceDetails0 = GetClientResourceDetails[0].ResourceTypeDetail;
                                        //        Guid GetClientResourceDetails1 = GetClientResourceDetails[1].ResourceTypeDetail;

                                        //        var GetClientTaxCloudAPIKeyDetails = db.ResourceTypeDetails
                                        //            .Where(p => p.ResourceTypeDetailsId == GetClientResourceDetails0).FirstOrDefault();

                                        //        var GetClientTaxCloudAPIBaseURlDetails = db.ResourceTypeDetails
                                        //            .Where(p => p.ResourceTypeDetailsId == GetClientResourceDetails1).FirstOrDefault();


                                        //        foreach (var itemDetails in GetClientResourceDetails)
                                        //        {
                                        //            if (itemDetails.ResourceTypeDetail == GetClientTaxCloudAPIKeyDetails.ResourceTypeDetailsId &&
                                        //                GetClientTaxCloudAPIKeyDetails.Name.Trim().ToLower() == "APIKey".Trim().ToLower())
                                        //                TaxCloudAPICloudKey = itemDetails.Value;

                                        //            if (itemDetails.ResourceTypeDetail == GetClientTaxCloudAPIBaseURlDetails.ResourceTypeDetailsId &&
                                        //                GetClientTaxCloudAPIBaseURlDetails.Name.Trim().ToLower() == "URL".Trim().ToLower())
                                        //                TaxCloudAPIURL = itemDetails.Value;
                                        //        }
                                        //    }
                                        //}
                                    }

                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, UserRole));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, TaxCloudAPIURL));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, TaxCloudAPICloudKey));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, ClientID));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, GetUserId.FirstName));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, GetUserId.LastName));
                                }
                                break;
                            case "customer":

                                if (GetCustomerUser != null)
                                {
                                    CustomerID = GetCustomerUser.Customer;
                                    UserRoleID = GetCustomerUser.Role;
                                    var GetUserRole = db.Roles.Where(p => p.RoleId == UserRoleID).FirstOrDefault();

                                    if (GetUserRole != null)
                                        UserRole = GetUserRole.RoleName;

                                    //Get the clientID for the logged in user
                                    var GetClientInfo = db.CustomerUsers.Join(db.ClientCustomers, x => x.Customer, p => p.Customer, (x, p) => new { p.Client, x.User }).Where(z => z.User == UserID).FirstOrDefault();
                                    if (GetClientInfo != null)
                                    {
                                        ClientID = GetClientInfo.Client.ToString();
                                        var AddressURLDetails = (from rth in db.ResourceTypeHeaders
                                                                 join rtd in db.ResourceTypeDetails on rth.ResourceTypeHeadersId equals rtd.ResourceTypeHeader
                                                                 join crd in db.ClientResourceDetails on rtd.ResourceTypeDetailsId equals crd.ResourceTypeDetail
                                                                 where rth.Name == "Tax Cloud Address Verification" && rtd.Description == "The Base URL for the call"
                                                                 select new
                                                                 {
                                                                     Value = crd.Value
                                                                 }).FirstOrDefault();

                                        TaxCloudAPIURL = AddressURLDetails.Value;

                                        var AddressAPIIDDetails = (from rth in db.ResourceTypeHeaders
                                                                   join rtd in db.ResourceTypeDetails on rth.ResourceTypeHeadersId equals rtd.ResourceTypeHeader
                                                                   join crd in db.ClientResourceDetails on rtd.ResourceTypeDetailsId equals crd.ResourceTypeDetail
                                                                   where rth.Name == "Tax Cloud Address Verification" && rtd.Description == "The key for this resource can be different in each environment"
                                                                   select new
                                                                   {
                                                                       Value = crd.Value
                                                                   }).FirstOrDefault();

                                        TaxCloudAPICloudKey = AddressAPIIDDetails.Value;
                                        //get the client resource headers mapping
                                        //var getClientResourceheaders = db.ClientResourceHeaders.Where(p => p.Client == GetClientInfo.Client).FirstOrDefault();
                                        //if (getClientResourceheaders != null)
                                        //{
                                        //    //get header details: will get the actual values of the header: URL and the Key
                                        //    var GetClientResourceDetails = db.ClientResourceDetails.Where(p => p.ClientResourceHeader == getClientResourceheaders.ClientResourceHeadersId).ToList();
                                        //    if (GetClientResourceDetails.Count > 0)
                                        //    {
                                        //        Guid GetClientResourceDetails0 = GetClientResourceDetails[0].ResourceTypeDetail;
                                        //        Guid GetClientResourceDetails1 = GetClientResourceDetails[1].ResourceTypeDetail;

                                        //        var GetClientTaxCloudAPIKeyDetails = db.ResourceTypeDetails
                                        //            .Where(p => p.ResourceTypeDetailsId == GetClientResourceDetails0).FirstOrDefault();

                                        //        var GetClientTaxCloudAPIBaseURlDetails = db.ResourceTypeDetails
                                        //            .Where(p => p.ResourceTypeDetailsId == GetClientResourceDetails1).FirstOrDefault();


                                        //        foreach (var itemDetails in GetClientResourceDetails)
                                        //        {
                                        //            if (itemDetails.ResourceTypeDetail == GetClientTaxCloudAPIKeyDetails.ResourceTypeDetailsId &&
                                        //                GetClientTaxCloudAPIKeyDetails.Name.Trim().ToLower() == "APIKey".Trim().ToLower())
                                        //                TaxCloudAPICloudKey = itemDetails.Value;

                                        //            if (itemDetails.ResourceTypeDetail == GetClientTaxCloudAPIBaseURlDetails.ResourceTypeDetailsId &&
                                        //                GetClientTaxCloudAPIBaseURlDetails.Name.Trim().ToLower() == "URL".Trim().ToLower())
                                        //                TaxCloudAPIURL = itemDetails.Value;
                                        //        }
                                        //    }
                                        //}
                                    }

                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, UserRole));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, TaxCloudAPIURL));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(CustomerID, TaxCloudAPICloudKey));
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, ClientID));
                                }

                                break;
                            case "vendor":
                                if (GetVendorUser != null)
                                {
                                    UserRoleID = GetVendorUser.Role;
                                    var GetUserRole = db.Roles.Where(p => p.RoleId == UserRoleID).FirstOrDefault();

                                    if (GetUserRole != null)
                                        UserRole = GetUserRole.RoleName;
                                    Authenticateuser.Add(new KeyValuePair<Guid, string>(UserID, UserRole));
                                }
                                break;
                        }
                    }
                    return Authenticateuser;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<fn_getClientResourceData_Result> GetResourceEmailDetails(Guid ClientId, string item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var SalesTaxDetails = db.fn_getClientResourceData(ClientId, item).ToList();
                    return SalesTaxDetails;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        //public IList<EmailDetails> GetEmailDetailsByClient(Guid ClientId)
        //{
        //    try
        //    {
        //        using (FacilitiesEntities db = new FacilitiesEntities())
        //        {
        //            var EmailDetail = (from RH in db.ResourceTypeHeaders
        //                               join CH in db.ClientResourceHeaders on RH.ResourceTypeHeadersId equals CH.ResourceTypeHeader
        //                               join CD in db.ClientResourceDetails on CH.ClientResourceHeadersId equals CD.ClientResourceHeader
        //                               where RH.Name == "neha Testing" && CH.Client == ClientId && CH.Alias == "Testing Email"
        //                               select new
        //                               {
        //                                   _Value = CD.Value

        //                               }).ToList().Select(x => new EmailDetails()
        //                               {
        //                                   Value = x._Value

        //                               }).ToList();
        //            return EmailDetail;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        //public IList<EmailDetails> GetEmailPasswordByClient(Guid ClientId)
        //{
        //    try
        //    {
        //        using (FacilitiesEntities db = new FacilitiesEntities())
        //        {
        //            var EmailDetail = (from RH in db.ResourceTypeHeaders
        //                               join CH in db.ClientResourceHeaders on RH.ResourceTypeHeadersId equals CH.ResourceTypeHeader
        //                               join CD in db.ClientResourceDetails on CH.ClientResourceHeadersId equals CD.ClientResourceHeader
        //                               where RH.Name == "neha Testing" && CH.Client == ClientId && CH.Alias == "Testing Password"
        //                               select new
        //                               {
        //                                   _Value = CD.Value

        //                               }).ToList().Select(x => new EmailDetails()
        //                               {
        //                                   Value = x._Value                                         
        //                               }).ToList();
        //            return EmailDetail;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}
    }
}
