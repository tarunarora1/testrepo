using BusinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitOfwork.Interfaces;
using BussinessEntities.BusinessEntityClasses;
using RestSharp;
using FacilitiesUserManagement.UserClasses;

namespace UnitOfwork.UOWRepository
{
    public class UOWCreateNewUsers : ICreateNewUsers
    {       
        public object VM_GetOktaUrlAndKey { get; private set; }

        public bool IsCustomerExists(string Name)
        {
            bool exists = false;

            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (db.Customers.Any(x => x.CustomerName == Name))
                    {
                        exists = true;
                    }
                }

                catch (Exception ex)
                { }
            }
            return exists;
        }

        public Guid AddNewCustomerRequest(CreateNewUsersEntity item)
        {
            Guid UserID = Guid.Empty;
            using (FacilitiesEntities dB = new FacilitiesEntities())
            {
                try
                {
                    // Add data Customer Name In Customer Table
                    Customer C = new Customer();
                    if (item.UserEntity.UserId == Guid.Empty)
                    {
                        if (!IsCustomerExists(item.CustomerEntity.CustomerName))
                        {
                            // Add data User In User Table                        
                            var U = VM_User.AddDataInUser<User>(item.UserEntity);
                            //var UserId1 = dB.CustomerUsers.Where(x => x.User == U.UserId).FirstOrDefault();
                            if (U != null)
                            {
                                C.CustomerId = Guid.NewGuid();
                                C.CustomerName = item.CustomerEntity.CustomerName;
                                C.ActiveFlag = "Y";
                                dB.Customers.Add(C);

                                //Add data in clientcustomer Table
                                ClientCustomer CC = new ClientCustomer();
                                CC.ClientCustomerId = Guid.NewGuid();
                                CC.Client = item.ClientCustomerEntity.Client;
                                CC.Customer = C.CustomerId;
                                CC.ActiveFlag = "Y";
                                dB.ClientCustomers.Add(CC);

                                dB.SaveChanges();
                                item.CustomerUserEntity.User = U.UserId;
                                item.CustomerUserEntity.Customer = C.CustomerId;
                                VM_CustomerUsers.AddDataInCustomerUsers(item.CustomerUserEntity);
                                UserID = U.UserId;
                            }
                        }
                    }
                    else
                    {
                        var CUS = dB.Customers.Where(a => a.CustomerId == item.CustomerEntity.CustomerId).FirstOrDefault();
                        if (CUS != null)
                        {
                            var user =  VM_User.UpdateDataInUser<User>(item.UserEntity);
                            item.CustomerUserEntity.ActiveFlag = item.UserEntity.ActiveFlag;                            
                            VM_CustomerUsers.UpdateDataInCustomerUsers(item.CustomerUserEntity);
                            CUS.CustomerName = item.CustomerEntity.CustomerName;
                            dB.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return UserID;
        }

        public object RegisterUser(RegisterUserEntity item)
        {                        
            //User U = new User();
            var C = VM_OktaUrlAndKey.GetOktaUrlAndKey();
            var client = new RestClient(C.Item1 + "?activate=true");
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", C.Item2);
            request.AddParameter("application/json", "{\n  \"profile\": {\n    \"firstName\": \"" + item.FirstName + "\",\n    \"lastName\": \"" + item.LastName + "\",\n    \"email\": \"" + item.Email + "\",\n    \"login\": \"" + item.Email + "\"\n  },\n  \"credentials\": {\n    \"password\" : { \"value\": \"" + item.Password + "\" }\n  },\n \"groupIds\":[\"00ggb8306pWYH4ONr0h7\"]\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;
            //return U;
        }




    public bool RegisterUserData(RegisterDataLInkEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var LH = VM_RegistrationUserDataCGSLinkHeader.AddDataOnLinkHeader<LinkHeader>(item.LinkHeaderEntity);
                    if (LH != null)
                    {
                        item.LinkDetailEntity.CGSLinkHeader = LH.CGSLinkHeader;
                        item.LinkDetailEntity.LinkHeader = LH.LinkHeaderId;
                        VM_RegistrationUserDataCGSLinkHeader.AddDataOnLinkDetails(item.LinkDetailEntity);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return true;
        }

        public Tuple<bool, string, string> GetLinkExpiyTime(string RandamString)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var Action = "";
                var ActionUrl = "";
                try
                {
                    var data = db.LinkHeaders.Where(x => x.RandomString == RandamString).FirstOrDefault();
                    if (data != null)
                    {
                        var EndDate = data.EndDate;
                        var CurrentDate = DateTime.Now;
                        Action = data.Action;
                        ActionUrl = data.CGSLinkHeader1.ActionUrl;
                        //CurrentDate = CurrentDate.AddDays(2);
                        if (CurrentDate > EndDate)
                        {
                            return Tuple.Create(true, "Link has been expired please contact to admin.", "");
                        }
                        else
                        {
                            return Tuple.Create(false, Action, ActionUrl);
                        }
                    }
                    else
                    {
                        return Tuple.Create(true, "Link has been expired please contact to admin.", "");
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }
        }

        public List<ValueEntity> GetLinkDetailData(string RandamString)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var data = db.LinkHeaders.Where(x => x.RandomString == RandamString).FirstOrDefault();
                    List<ValueEntity> obj = new List<ValueEntity>();
                    foreach (var ss in data.CGSLinkHeader1.CGSLinkDetails)
                    {
                        foreach (var aa in data.LinkDetails)
                        {
                            if (ss.CGSLinkDetailId == aa.CGSLinkDetails)
                            {
                                ValueEntity obj1 = new ValueEntity();
                                obj1.Key = ss.ColumnHeader;
                                obj1.value = aa.Value;
                                obj1.LinkHeaderId = data.LinkHeaderId;
                                obj.Add(obj1);
                            }
                        }
                    }
                    return obj;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public Guid SaveCustomerUsers(CustomerUsers item)
        {
            Guid UserId = Guid.Empty;
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    CustomerUser CU = new CustomerUser();
                    if (item.UserEntity.UserId == Guid.Empty && item.CustomerUserEntity.CustomerUserId == Guid.Empty)
                    {
                        var U = VM_User.AddDataInUser<User>(item.UserEntity);
                        item.CustomerUserEntity.User = U.UserId;
                        UserId = U.UserId;
                        item.CustomerUserEntity.ActiveFlag = U.ActiveFlag;

                        CU.CustomerUserId = Guid.NewGuid();
                        CU.Customer = item.CustomerUserEntity.Customer;
                        CU.Role = item.CustomerUserEntity.Role;
                        CU.User = item.CustomerUserEntity.User;
                        CU.ActiveFlag = "Y";
                        db.CustomerUsers.Add(CU);
                        db.SaveChanges();
                        //VM_CustomerUsers.AddDataInCustomerUsers(item.CustomerUserEntity);

                        var CustomerRole = db.Roles.Where(a => a.RoleId == item.CustomerUserEntity.Role && a.ActiveFlag == "Y").Select(m => m.RoleName).FirstOrDefault();
                        if (CustomerRole == "Customer location User")
                        {
                            if (item.CustomerLocationID != Guid.Empty)
                            {
                                CustomerLocationUser CL = new CustomerLocationUser();
                                CL.CustomerLocationUser1 = Guid.NewGuid();
                                CL.CustomerLocation = item.CustomerLocationID;
                                CL.User = U.UserId;
                                CL.ActiveFlag = "Y";
                                db.CustomerLocationUsers.Add(CL);
                                db.SaveChanges();
                            }
                            else
                            {
                                foreach (var value in item.CustomerLocation)
                                {
                                    CustomerLocationUser CL = new CustomerLocationUser();
                                    CL.CustomerLocationUser1 = Guid.NewGuid();
                                    CL.CustomerLocation = value;
                                    CL.User = U.UserId;
                                    CL.ActiveFlag = "Y";
                                    db.CustomerLocationUsers.Add(CL);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        //Nitin31082018
                        var U = VM_User.UpdateDataInUser<User>(item.UserEntity);
                        var AfterActionUrl = db.CustomerUsers.Join(db.Roles, LH => LH.Role, CGL => CGL.RoleId, (LH, CGL) => new { RoleName = CGL.RoleName, User = LH.User }).Where(LH => LH.User == U.UserId).FirstOrDefault().RoleName;
                        if(AfterActionUrl == "Customer location User")
                        {
                            var allRec = db.CustomerLocationUsers.Where(a => a.User == item.UserEntity.UserId);
                            if (allRec != null)
                            {
                                db.CustomerLocationUsers.RemoveRange(allRec);
                                db.SaveChanges();
                            }
                        }
                        //item.CustomerUserEntity = db.CustomerUsers.Where(p => p.User == item.UserEntity.UserId).FirstOrDefault();
                        var Rolename = db.Roles.Where(x => x.RoleId == item.CustomerUserEntity.Role && x.ActiveFlag == "Y").Select(m=> m.RoleName).FirstOrDefault();
                        if(Rolename == "Customer location User")
                        {
                            if (item.CustomerLocationID != Guid.Empty)
                            {                                
                                var allRec = db.CustomerLocationUsers.Where(a => a.User == item.UserEntity.UserId);
                                if (allRec != null)
                                {
                                    db.CustomerLocationUsers.RemoveRange(allRec);
                                    db.SaveChanges();
                                }

                                CustomerLocationUser CL = new CustomerLocationUser();
                                CL.CustomerLocationUser1 = Guid.NewGuid();
                                CL.CustomerLocation = item.CustomerLocationID;
                                CL.User = U.UserId;
                                CL.ActiveFlag = "Y";
                                db.CustomerLocationUsers.Add(CL);
                                db.SaveChanges();
                            }
                            else
                            {
                                var allRec = db.CustomerLocationUsers.Where(a => a.User == item.UserEntity.UserId);
                                if (allRec != null)
                                {
                                    db.CustomerLocationUsers.RemoveRange(allRec);
                                    db.SaveChanges();
                                }
                                foreach (var value in item.CustomerLocation)
                                {
                                    CustomerLocationUser CL = new CustomerLocationUser();
                                    CL.CustomerLocationUser1 = Guid.NewGuid();
                                    CL.CustomerLocation = value;
                                    CL.User = U.UserId;
                                    CL.ActiveFlag = "Y";
                                    db.CustomerLocationUsers.Add(CL);
                                    db.SaveChanges();
                                }
                            }
                        }
                        var CustomerUsersdetails = db.CustomerUsers.Where(p => p.User == item.UserEntity.UserId).FirstOrDefault();
                        CustomerUsersdetails.Role = item.CustomerUserEntity.Role;
                        VM_CustomerUsers.UpdateDataInCustomerUsers(CustomerUsersdetails);
                    }
                    return UserId;
                }
                catch (Exception ex)
                {
                    return UserId;
                }
            }
        }

        public Guid AddNewClientRequest(CreateNewUsersEntity item)
        {
            Guid UserID = Guid.Empty;
            using (FacilitiesEntities dB = new FacilitiesEntities())
            {
                try
                {
                    if (item.UserEntity.UserId == Guid.Empty)
                    {
                        var U = VM_User.AddDataInUser<User>(item.UserEntity);
                        var UserId1 = dB.ClientUsers.Where(x => x.User == U.UserId).FirstOrDefault();
                        if (U != null && UserId1 == null)
                        {
                            item.ClientUserEntity.User = U.UserId;
                            VM_ClientUsers.AddDataInClientUser(item.ClientUserEntity);
                            UserID = U.UserId;
                        }
                    }
                    else
                    {
                        item.UserEntity.ActiveFlag = item.ClientUserEntity.ActiveFlag;
                        VM_ClientUsers.UpdateDataInClientUser(item.ClientUserEntity);
                        var U = VM_User.UpdateDataInUser<User>(item.UserEntity);
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return UserID;
        }

        public string GetUserEmailId(Guid UserId)
        {
            string EmailId = "";
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (UserId != null)
                    {
                        var UserEmailId = db.Users.Where(a => a.UserId == UserId && a.ActiveFlag == "R").FirstOrDefault();

                        if (UserEmailId != null)
                            EmailId = UserEmailId.Email;
                    }
                }
                catch (Exception ex)
                {

                }
                return EmailId;
            }
        }

        public bool ActivateRegistrationUsers(User item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                Guid UserID = Guid.Empty;
                var GetUsers = db.Users.Where(p => p.Email.ToLower() == item.Email.ToLower()).FirstOrDefault();

                if (GetUsers != null)
                {
                    //activate users table
                    GetUsers.ActiveFlag = "Y";
                    GetUsers.OktaUserID = item.OktaUserID;
                    GetUsers.FirstName = item.FirstName;
                    GetUsers.LastName = item.LastName;
                    GetUsers.Telephone = item.Telephone;
                    db.SaveChanges();
                }
            }
            return true;
        }        

        public IEnumerable<RoleEntities> GetCustomerRoles()
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var CustomerRoles = db.Roles.Where(s => s.RoleName == "Customer Admin" || s.RoleName == "Customer corporate User" || s.RoleName == "Customer location User" && s.ActiveFlag == "Y").Select(m => new RoleEntities
                    {
                        RoleId = m.RoleId,
                        RoleName = m.RoleName
                    }).ToList();

                    return CustomerRoles;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ClientCustomerUsersEntity> GetCustomerUserGridData(Guid CustomerId)
        {
            UOWManageUsers MU = new UOWManageUsers();
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var CustomerUsers = (from cu in db.CustomerUsers
                                         join c in db.Customers on cu.Customer equals c.CustomerId
                                         join u in db.Users on cu.User equals u.UserId
                                         join r in db.Roles on cu.Role equals r.RoleId
                                         where c.CustomerId == CustomerId
                                         select new
                                         {
                                             _CustomerID = c.CustomerId,
                                             _Customername = c.CustomerName,
                                             _RoleID = cu.Role,
                                             _RoleName = r.RoleName,
                                             _UserID = u.UserId,
                                             _Firstname = u.FirstName,
                                             _Lastname = u.LastName,
                                             _Email = u.Email,
                                             _ActiveFlag = u.ActiveFlag == "R" ? u.ActiveFlag : cu.ActiveFlag

                                         }).ToList().Select(W => new ClientCustomerUsersEntity()
                                         {
                                             CustomerID = W._CustomerID,
                                             Customername = W._Customername,
                                             RoleID = W._RoleID,
                                             RoleName = W._RoleName,
                                             UserID = W._UserID,
                                             Firstname = W._Firstname,
                                             Lastname = W._Lastname,
                                             Email = W._Email,
                                             Status = MU.UserStatus(W._ActiveFlag)
                                         }).ToList();

                    return CustomerUsers;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public List<CustomerLocationEntity> GetLocationWithAddress(Guid CustomerId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserLocation = (from cl in db.CustomerLocations
                                        where cl.ActiveFlag == "Y" && cl.Customer == CustomerId
                                        select new
                                        {
                                            _CustomerLocationId = cl.CustomerLocationId,
                                            _LocationName = cl.LocationName,
                                            _LocationCode = cl.LocationCode,
                                            _Address01 = cl.Address01,
                                            _City = cl.City,
                                            _State = cl.State,
                                            _Zip01 = cl.Zip01,
                                            _Telephone = cl.Telephone
                                        }).ToList().Select(m => new CustomerLocationEntity()
                                        {
                                            CustomerLocationId = m._CustomerLocationId,
                                            LocationName = m._LocationName,
                                            LocationCode = m._LocationCode,
                                            Address01 = m._Address01,
                                            City = m._City,
                                            State = m._State,
                                            Zip01 = m._Zip01,
                                            Telephone = m._Telephone
                                        }).ToList();
                    return UserLocation;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<BindLocationList> BindCustomerLocation(Guid Userid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var LocationList = (from clu in db.CustomerLocationUsers
                                        where clu.User == Userid
                                        select new
                                        {
                                            _CustomerLocation = clu.CustomerLocation
                                        }).ToList().Select(m => new BindLocationList()
                                        {
                                            CustomerLocation = m._CustomerLocation
                                        }).ToList();

                    return LocationList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<GetCustomerLocationUsers_Result> GetAssociateCustomerLocationusers(Guid LocationId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var CustomerLocationUsersList = db.GetCustomerLocationUsers(LocationId).ToList();
                    return CustomerLocationUsersList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<GetCustomerEmailAddressForInvoiceQuote_Result> GetAssociateCustomerLocationusers1(Guid LocationId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var CustomerLocationUsersList = db.GetCustomerEmailAddressForInvoiceQuote(LocationId).ToList();
                    return CustomerLocationUsersList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public string DeleteDataFromLinkTables(Guid LinkHeaderID)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var AfterActionUrl = db.LinkHeaders.Join(db.CGSLinkHeaders, LH => LH.CGSLinkHeader, CGL => CGL.CGSLinkHeaderId, (LH, CGL) => new { AfterActionUrl = CGL.AfterActionUrl, LinkHeaderId = LH.LinkHeaderId }).Where(LH => LH.LinkHeaderId == LinkHeaderID).FirstOrDefault().AfterActionUrl;

                    var deleteInLinkDetails = db.LinkDetails.Where(a => a.LinkHeader == LinkHeaderID);
                    db.LinkDetails.RemoveRange(deleteInLinkDetails);

                    var deleteInLinkheaders = db.LinkHeaders.Where(a => a.LinkHeaderId == LinkHeaderID);
                    db.LinkHeaders.RemoveRange(deleteInLinkheaders);
                    db.SaveChanges();
                    return AfterActionUrl;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
