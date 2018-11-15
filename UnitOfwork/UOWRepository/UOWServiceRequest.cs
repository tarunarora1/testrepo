using BusinessEntities.BusinessEntityClasses;
using DataModel;

using System;
using System.Collections.Generic;
using System.Linq;
using UnitOfWork.Interface;

namespace UnitOfWork.UOWRepository
{
    public class UOWServiceRequest : IServiceRequest
    {
        public List<ServiceRequestModel> GetServiceRequest(Guid Clientid)
        {
            List<ServiceRequestModel> Request = new List<ServiceRequestModel>();
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                string createdByUser = string.Empty;
                string LastUpdatedByUser = string.Empty;
                try
                {
                    var GetAllServiceRequests = db.GetServiceRequests(Clientid).OrderByDescending(p => p.DateCreated);
                    foreach (var item in GetAllServiceRequests)
                    {
                        Request.Add(new ServiceRequestModel
                        {
                            ServiceRequestId = item.ServiceRequestId,
                            RequestNumber = item.ServiceRequestNumber,
                            CustomerId = item.CustomerId,
                            Clientname = item.ClientName,
                            Customername = item.CustomerName,
                            CustomerLocation = item.LocationName,
                            DateCreated = item.DateCreated,
                            DateLastUpdated = item.DateLastUpdated,
                            DateWorkOrder = item.DateWorkOrder,
                            CustomerRefNumber = item.CustomerRefNumber,
                            Description = item.Description,
                            ClientProblemClass = item.ProblemClassName,
                            ClientProblemCode = item.ProblemCodeName,
                            ClientServiceRequestPriority = item.PriorityName,
                            ClientServiceRequestStatus = item.RequestStatus,
                            CreatedBy = createdByUser,
                            LastUpdatedBy = LastUpdatedByUser,
                            ServiceType = item.ServiceTypes,
                            CustomerLocationId = item.CustomerLocationId
                        });
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                return Request;
            }
        }

        public IEnumerable<CustomerService> GetCustomer(string ClientID)
        {
            //List<CustomerService> objCustomerService = new List<Models.CustomerService>();
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {

                    if (ClientID != null)
                    {
                        var clientid = new Guid(ClientID);
                        var CustomerList = (from C in db.Customers
                                            join CC in db.ClientCustomers on C.CustomerId equals CC.Customer
                                            where CC.Client == clientid && C.ActiveFlag=="Y" && CC.ActiveFlag =="Y"
                                            select new
                                            {
                                                CustomerId = C.CustomerId,
                                                CustomerName = C.CustomerName

                                            }).ToList().Select(x => new CustomerService()
                                            {
                                                CustomerId = x.CustomerId,
                                                CustomerName = x.CustomerName

                                            }).ToList();
                        return CustomerList;

                    }
                    else
                    {
                        var Data = db.Customers.Where(x => x.ActiveFlag == "Y").ToList().Select(x => new CustomerService()
                        {
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName

                        }).ToList();

                        return Data;

                    }




                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public List<CGSInterval> GetCGSInterval()
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var data = db.CGSIntervals.Where(a => a.ActiveFlag == "Y").ToList().Select(x => new CGSInterval()
                    {
                        IntervalId = x.IntervalId,
                        IntervalName = x.IntervalName + " " + x.IntervalNumber,
                        IntervalNumber = x.IntervalNumber
                    }).ToList(); ;
                    return data;
                }
                catch (Exception ex)
                {
                    return null;

                }
            }
        }

        public IEnumerable<ClientService> GetClientForCustomer(Guid _LoggedInUserID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var clientList = (from CU in db.CustomerUsers
                                      join cc in db.ClientCustomers on CU.Customer equals cc.Customer
                                      join c in db.Clients on cc.Client equals c.ClientId
                                      where CU.User == _LoggedInUserID && CU.ActiveFlag == "Y" && cc.ActiveFlag == "Y"
                                      select new
                                      {
                                          ClientId = c.ClientId,
                                          ClientName = c.ClientName

                                      }).ToList().Select(x => new ClientService()
                                      {
                                          ClientId = x.ClientId,
                                          ClientName = x.ClientName

                                      }).ToList();
                    return clientList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ProblemClassesEntity> GetProblemClasses(string ClientID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (ClientID != null)
                    {
                        var clientid = new Guid(ClientID);
                        var ClientProblemClassList = (from C in db.ClientProblemClasses
                                                      where C.ActiveFlag == "Y" && C.Client == clientid
                                                      orderby C.ProblemClassName ascending
                                                      select new
                                                      {
                                                          ClientProblemClassId = C.ClientProblemClassId,
                                                          ProblemClassName = C.ProblemClassName

                                                      }).ToList().Select(x => new ProblemClassesEntity()
                                                      {
                                                          ClientProblemClassId = x.ClientProblemClassId,
                                                          ProblemClassName = x.ProblemClassName

                                                      }).ToList();
                        return ClientProblemClassList;
                    }

                    else
                    {
                        var Data = db.ClientProblemClasses.Where(x => x.ActiveFlag == "Y").ToList().Select(x => new ProblemClassesEntity()
                        {
                            ClientProblemClassId = x.ClientProblemClassId,
                            ProblemClassName = x.ProblemClassName

                        }).ToList();


                        return Data;

                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ClientServiceType> GetServiceRequestType(string Clientid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (Clientid != null)
                    {
                        var clientid = new Guid(Clientid);
                        var ServiceRequestTypeList = (from C in db.ClientServiceTypes
                                                      where C.ActiveFlag == "Y" && C.Client == clientid && C.DefaultFlag == "Y"
                                                      select new
                                                      {
                                                          _ClientServiceTypeId = C.ClientServiceTypeId,
                                                          _ServiceType = C.ServiceType,
                                                          _DefaultFlag = C.DefaultFlag

                                                      }).ToList().Select(x => new ClientServiceType()
                                                      {
                                                          ClientServiceTypeId = x._ClientServiceTypeId,
                                                          ServiceType = x._ServiceType,
                                                          DefaultFlag = x._DefaultFlag

                                                      }).ToList();
                        return ServiceRequestTypeList;
                    }
                    else
                    {
                        var data = db.ClientServiceTypes.Where(a => a.ActiveFlag == "Y").ToList().Select(x => new ClientServiceType()
                        {
                            ClientServiceTypeId = x.ClientServiceTypeId,
                            ServiceType = x.ServiceType,
                            DefaultFlag = x.DefaultFlag

                        }).ToList();

                        return data;
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ClientServiceType> GetServiceRequestTypeForCustomer(Guid Clientid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var ServiceRequestTypeList = (from C in db.ClientServiceTypes
                                                  where C.ActiveFlag == "Y" && C.CuatomerFacingFlag == "Y" && C.Client == Clientid
                                                  select new
                                                  {
                                                      _ClientServiceTypeId = C.ClientServiceTypeId,
                                                      _ServiceType = C.ServiceType,
                                                      _DefaultFlag = C.DefaultFlag

                                                  }).ToList().Select(x => new ClientServiceType()
                                                  {
                                                      ClientServiceTypeId = x._ClientServiceTypeId,
                                                      ServiceType = x._ServiceType,
                                                      DefaultFlag = x._DefaultFlag

                                                  }).ToList();
                    return ServiceRequestTypeList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<RequestEntity> GetRequestPriorties(string ClientID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (ClientID != null)
                    {
                        var clientid = new Guid(ClientID);
                        var ClientProblemClassList = (from C in db.ClientServiceRequestPriorities
                                                      where C.ActiveFlag == "Y" && C.Client == clientid
                                                      orderby C.SortOrder ascending
                                                      select new
                                                      {
                                                          ClientServiceRequestPriorityId = C.ClientServiceRequestPriorityId,
                                                          PriorityName = C.PriorityName,
                                                          DefaultFlag = C.DefaultFlag

                                                      }).ToList().Select(x => new RequestEntity()
                                                      {
                                                          ClientServiceRequestPriorityId = x.ClientServiceRequestPriorityId,
                                                          PriorityName = x.PriorityName,
                                                          DefaultFlag = x.DefaultFlag

                                                      }).ToList();
                        return ClientProblemClassList;
                    }
                    else
                    {

                        var data = db.ClientServiceRequestPriorities.Where(a => a.ActiveFlag == "Y").ToList().Select(x => new RequestEntity()
                        {
                            ClientServiceRequestPriorityId = x.ClientServiceRequestPriorityId,
                            PriorityName = x.PriorityName,
                            DefaultFlag = x.DefaultFlag

                        }).ToList();
                        return data;


                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public IEnumerable<ProblemCodeEntity> GetProblemCodes(Guid ProblemClassID, Guid ClientID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var ClientProblemClassList = (from C in db.ClientProblemCodes
                                                  where C.ActiveFlag == "Y" && C.ClientProblemClass == ProblemClassID && C.Client == ClientID
                                                  orderby C.ProblemCodeName ascending                                                  
                                                  select new
                                                  {
                                                      ClientProblemCodeId = C.ClientProblemCodeId,
                                                      ProblemCodeName = C.ProblemCodeName

                                                  }).ToList().Select(x => new ProblemCodeEntity()
                                                  {
                                                      ClientProblemCodeId = x.ClientProblemCodeId,
                                                      ProblemCodeName = x.ProblemCodeName

                                                  }).ToList();
                    return ClientProblemClassList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ServiceUserLocation> GetLocationbyUser(Guid CustomerId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (CustomerId != null)
                    {
                        var CustomerLocation = (from CL in db.CustomerLocations
                                                where CL.ActiveFlag == "Y" && CL.Customer == CustomerId
                                                select new ServiceUserLocation
                                                {
                                                    CustomerLocationId = CL.CustomerLocationId,
                                                    LocationName = CL.LocationName + " " + CL.LocationCode
                                                }).ToList();
                        return CustomerLocation;
                    }
                }

                catch (Exception ex)
                {

                }
                return null;
            }
        }

        public IEnumerable<prc_GetCustomerLocationsForUser_Result> GetLocationbyUser(Guid UserID, string UserRole)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (UserID != null)
                    {
                        var CustomerLocation = db.prc_GetCustomerLocationsForUser(UserID, "Customers").ToList();

                        return CustomerLocation;
                    }

                }

                catch (Exception ex)
                {

                }
                return null;
            }
        }

        public Guid AddServiceRequest(ServiceRequestEntities ServiceRequest)
        {
         //   string OperationStatus = "Failure";

            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    Guid ClientID = Guid.Empty;
                    //var InvoiceStatus = db.InvoiceStatuses.Where(a => a.Description == "New").Select(x => x.InvoiceStatusId).FirstOrDefault();
                    if (ServiceRequest.Client == Guid.Empty)
                    {
                        ClientID = db.ClientUsers.Where(a => a.User == ServiceRequest.CreatedByUser).Select(a => a.Client).FirstOrDefault();
                    }
                    ServiceRequest SR = new ServiceRequest();
                    if (ServiceRequest != null)
                    {
                        if (ServiceRequest.ServiceRequestId == Guid.Empty)
                        {
                            var ServiceType = db.ClientServiceTypes.Where(a => a.ClientServiceTypeId == ServiceRequest.ClientServiceTypeId).Select(p => p.ServiceType).FirstOrDefault();                            
                            SR.ServiceRequestId = Guid.NewGuid();
                            if (ClientID != Guid.Empty)
                            {
                                SR.Client = ClientID;
                            }
                            else
                            {
                                SR.Client = ServiceRequest.Client;
                            }
                            if (ServiceRequest.Customer != Guid.Empty)
                            {
                                SR.Customer = ServiceRequest.Customer;
                            }
                            else
                            {
                                SR.Customer = db.CustomerUsers.Where(a => a.User == ServiceRequest.CreatedByUser).Select(x => x.Customer).FirstOrDefault();
                            }
                            SR.CustomerLocation = ServiceRequest.CustomerLocation;
                            SR.CreatedByUser = ServiceRequest.CreatedByUser;
                            SR.DateCreated = DateTime.Now;
                            SR.LastUpdatedByUser = ServiceRequest.CreatedByUser;
                            SR.DateLastUpdated = DateTime.Now;
                            SR.DateWorkOrder = DateTime.Now;
                            SR.CustomerRefNumber = ServiceRequest.CustomerRefNumber;
                            if (ServiceRequest.NTE == 0)
                            {
                                SR.NTE = 0;
                            }
                            else
                            {
                                SR.NTE = ServiceRequest.NTE;
                            }
                            if (ClientID != Guid.Empty)
                            {
                                var ServiceNumber = GetServiceRequestNumber("R", ClientID, ServiceType);
                                SR.ServiceRequestNumber = ServiceNumber[0].Number;
                                SR.ServiceRequestNumberPrefix = ServiceNumber[0].Prefix;
                                SR.ServiceRequestNumberSeq = ServiceNumber[0].SeqNbr;
                            }
                            else
                            {
                                var ServiceNumber = GetServiceRequestNumber("R", ServiceRequest.Client, ServiceType);
                                SR.ServiceRequestNumber = ServiceNumber[0].Number;
                                SR.ServiceRequestNumberPrefix = ServiceNumber[0].Prefix;
                                SR.ServiceRequestNumberSeq = ServiceNumber[0].SeqNbr;
                            }
                            SR.Description = ServiceRequest.Description;
                            SR.ClientProblemClass = ServiceRequest.ClientProblemClass;
                            SR.ClientProblemCode = ServiceRequest.ClientProblemCode;
                            SR.ServiceType = ServiceRequest.ClientServiceTypeId;
                            SR.ClientServiceRequestPriority = ServiceRequest.ClientServiceRequestPriority;
                            SR.ClientServiceRequestStatus = db.ClientServiceRequestStatuses.Where(a => a.DefaultFlag == "Y" && a.ActiveFlag == "Y").Select(x => x.ClientServiceRequestStatusId).FirstOrDefault();

                            db.ServiceRequests.Add(SR);

                            db.SaveChanges();
                            return SR.ServiceRequestId;
                            //OperationStatus = "Success";
                        }
                        else
                        {
                            SR = db.ServiceRequests.Where(a => a.ServiceRequestId == ServiceRequest.ServiceRequestId).FirstOrDefault();
                            if (SR != null)
                            {
                                if (ClientID != Guid.Empty)
                                {
                                    SR.Client = ClientID;
                                }
                                else
                                {
                                    SR.Client = ServiceRequest.Client;
                                }
                                SR.Customer = ServiceRequest.Customer;
                                SR.CustomerLocation = ServiceRequest.CustomerLocation;
                                SR.CreatedByUser = ServiceRequest.CreatedByUser;
                                SR.DateCreated = DateTime.Now;
                                SR.LastUpdatedByUser = ServiceRequest.CreatedByUser;
                                SR.DateLastUpdated = DateTime.Now;
                                SR.DateWorkOrder = DateTime.Now;
                                SR.CustomerRefNumber = ServiceRequest.CustomerRefNumber;
                                if (ServiceRequest.NTE == 0)
                                {
                                    SR.NTE = 0;
                                }
                                else
                                {
                                    SR.NTE = ServiceRequest.NTE;
                                }
                                SR.Description = ServiceRequest.Description;
                                SR.ClientProblemClass = ServiceRequest.ClientProblemClass;
                                SR.ClientProblemCode = ServiceRequest.ClientProblemCode;
                                SR.ClientServiceRequestPriority = ServiceRequest.ClientServiceRequestPriority;
                                SR.ServiceType = ServiceRequest.ClientServiceTypeId;
                                SR.ClientServiceRequestStatus = db.ClientServiceRequestStatuses.Where(a => a.DefaultFlag == "Y" && a.ActiveFlag == "Y").Select(x => x.ClientServiceRequestStatusId).FirstOrDefault();

                                db.SaveChanges();
                                return SR.ServiceRequestId;
                               // OperationStatus = "Success";
                            }
                        }
                    }
                    //}
                }
                catch (Exception ex)
                {
                    //OperationStatus += ex.Message;
                }
            }
            return ServiceRequest.ServiceRequestId;
        }

        public List<prc_createNumber_Result> GetServiceRequestNumber(string value, Guid ClientId, string Servicetype)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var RequestNumber = db.prc_createNumber(value, ClientId, Servicetype).ToList();
                    return RequestNumber;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ServiceRequestEntities> GetServiceRequestDatabyID(Guid servicerequestid)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var ClientList = (from W in DB.ServiceRequests
                                      where W.ServiceRequestId == servicerequestid
                                      select new
                                      {
                                          ServiceRequestId = W.ServiceRequestId,
                                          _ServiceRequestNumber = W.ServiceRequestNumber,
                                          Client = W.Client,
                                          Customer = W.Customer,
                                          CustomerLocation = W.CustomerLocation,
                                          CreatedByUser = W.CreatedByUser,
                                          DateCreated = W.DateCreated,
                                          LastUpdatedByUser = W.LastUpdatedByUser,
                                          DateLastUpdated = W.DateLastUpdated,
                                          DateWorkOrder = W.DateWorkOrder,
                                          CustomerRefNumber = W.CustomerRefNumber,
                                          NTE = W.NTE,
                                          Description = W.Description,
                                          ClientProblemClass = W.ClientProblemClass,
                                          ClientProblemCode = W.ClientProblemCode,
                                          ClientServiceRequestPriority = W.ClientServiceRequestPriority,
                                          ClientServiceRequestStatus = W.ClientServiceRequestStatus,
                                          ServiceType = W.ServiceType

                                      }).ToList().Select(W => new ServiceRequestEntities()
                                      {
                                          ServiceRequestId = W.ServiceRequestId,
                                          RequestNumber = W._ServiceRequestNumber,
                                          Client = W.Client,
                                          Customer = W.Customer,
                                          CustomerLocation = W.CustomerLocation,
                                          CreatedByUser = W.CreatedByUser,
                                          DateCreated = W.DateCreated,
                                          LastUpdatedByUser = W.LastUpdatedByUser,
                                          DateLastUpdated = W.DateLastUpdated,
                                          DateWorkOrder = W.DateWorkOrder,
                                          CustomerRefNumber = W.CustomerRefNumber,
                                          NTE = W.NTE,
                                          Description = W.Description,
                                          ClientProblemClass = W.ClientProblemClass,
                                          ClientProblemCode = W.ClientProblemCode,
                                          ClientServiceRequestPriority = W.ClientServiceRequestPriority,
                                          ClientServiceRequestStatus = W.ClientServiceRequestStatus,
                                          ClientServiceTypeId = W.ServiceType
                                      }).ToList();
                    return ClientList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ServiceRequestModel> GetServiceRequestforcustomer(string UserEmail, string UserRole)
        {
            List<ServiceRequestModel> Request = new List<ServiceRequestModel>();

            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    //Guid UserID = Guid.Empty;
                    //string UserGroup = string.Empty;

                    //string createdByUser = string.Empty;
                    //string LastUpdatedByUser = string.Empty;

                    //get users data
                    //var GetUserId = db.Users.Where(p => p.Email.ToLower() == UserEmail.ToLower().Trim()).FirstOrDefault();

                    //if (GetUserId != null)
                    //{
                    //    UserID = GetUserId.UserId;

                    //if (UserRole == "Customer location User")
                    //{
                    //get role id of the user from customerusers
                    //var GetRoleID = db.CustomerUsers.Where(p => p.User == UserID).FirstOrDefault();
                    //if (GetRoleID != null)
                    //{
                    //    //get rolegroupid from roles
                    //    var GetRoleGroup = db.Roles.Where(p => p.RoleId == GetRoleID.Role).FirstOrDefault();
                    //    if (GetRoleGroup != null)
                    //    {
                    //        //get rolegroup name from rolegroups
                    //        var GetRoleGroupDesc = db.RoleGroups.Where(p => p.RoleGroupId == GetRoleGroup.RoleGroup).FirstOrDefault();
                    //        if (GetRoleGroupDesc != null)
                    //        {
                    //            UserGroup = GetRoleGroupDesc.RoleGroupName;
                    //get only service requests for this user for only those locations that he has access to                    

                    //filter the service requests for only those requests that this user has created
                    //var GetforthisUserOnly = GetAllServiceRequestsForthisUser.Where(p => p.CreatedByUser == UserID);
                    var GetAllServiceRequestsForthisUser = db.GetServiceRequestsForLocationUser(UserEmail, "Customers").OrderByDescending(p => p.DateCreated).ToList();
                    foreach (var item in GetAllServiceRequestsForthisUser)
                    {
                        Request.Add(new ServiceRequestModel
                        {
                            ServiceRequestId = item.ServiceRequestId,
                            RequestNumber = item.ServiceRequestNumber,
                            Clientname = item.ClientName,
                            ClientId = item.ClientID,
                            Customername = item.CustomerName,
                            CustomerLocation = item.LocationName,
                            DateCreated = item.DateCreated,
                            DateLastUpdated = item.DateLastUpdated,
                            DateWorkOrder = item.DateWorkOrder,
                            CustomerRefNumber = item.CustomerRefNumber,
                            Description = item.Description,
                            ClientProblemClass = item.ProblemClassName,
                            ClientProblemCode = item.ProblemCodeName,
                            ClientServiceRequestPriority = item.PriorityName,
                            ClientServiceRequestStatus = item.RequestStatus,
                            CustomerLocationId = item.CustomerLocationId,
                            //CreatedBy = createdByUser,
                            //LastUpdatedBy = LastUpdatedByUser,
                            //WorkOrderId = item.WorkOrderId,
                            ServiceType = item.ServiceTypes,
                            //WorkOrderExist = item.WorkOrderExist
                        });
                    }
                }
                return Request;

            }


            //    }
            //}
            //}
            //}
            //else
            //{
            //    var GetAllServiceRequests = db.GetServiceRequests().OrderByDescending(p => p.DateCreated);
            //    foreach (var item in GetAllServiceRequests)
            //    {
            //        var IsWorkOrderExist = db.WorkOrders.Where(a => a.ServiceRequest == item.ServiceRequestId).FirstOrDefault();
            //        Request.Add(new ServiceRequestModel
            //        {
            //            ServiceRequestId = item.ServiceRequestId,
            //            RequestNumber = item.ServiceRequestNumber,
            //            Clientname = item.ClientName,
            //            Customername = item.CustomerName,
            //            CustomerLocation = item.LocationName,
            //            DateCreated = item.DateCreated,
            //            DateLastUpdated = item.DateLastUpdated,
            //            DateWorkOrder = item.DateWorkOrder,
            //            CustomerRefNumber = item.CustomerRefNumber,
            //            Description = item.Description,
            //            ClientProblemClass = item.ProblemClassName,
            //            ClientProblemCode = item.ProblemCodeName,
            //            ClientServiceRequestPriority = item.PriorityName,
            //            ClientServiceRequestStatus = item.RequestStatus,
            //            CreatedBy = createdByUser,
            //            LastUpdatedBy = LastUpdatedByUser,                                    
            //            ServiceType = item.ServiceTypes,
            //            WorkOrderId = IsWorkOrderExist != null ? IsWorkOrderExist.WorkOrderId : Guid.Empty,
            //            WorkOrderExist = (IsWorkOrderExist != null ? true.ToString() : false.ToString())
            //        });
            //    }
            //}


            catch (Exception ex)
            {
                return null;
            }
        }

        public int GetCountofWorkOrder(Guid servicerequestid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var WorkOrderCount = db.WorkOrders.Where(a => a.ServiceRequest == servicerequestid).Count();
                return WorkOrderCount;
            }
        }

        public List<Tuple<Guid, IList<CustomerLocation>>> AddNewLocation(CustomerLocation item)
        {
            List<Tuple<Guid, IList<CustomerLocation>>> OperationStatus = new List<Tuple<Guid, IList<CustomerLocation>>>();
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    //var LocationList = db.CustomerLocations.Where(a => a.Customer == item.Customer && a.ActiveFlag == "Y" && (a.LocationCode == item.LocationCode || (a.Address01 == item.Address01 && a.City == item.City && a.State == item.State && a.Zip01 == item.Zip01) || a.Telephone == item.Telephone)).ToList();
                    var LocationList = (from cl in db.CustomerLocations
                                            //where cl.Customer == item.Customer && cl.ActiveFlag == "Y" && cl.LocationCode == item.LocationCode && cl.Address01 == item.Address01 && cl.City == item.City && cl.State == item.State && cl.Zip01 == item.Zip01 && cl.Telephone == item.Telephone
                                        where cl.Customer == item.Customer && cl.ActiveFlag == "Y" && (cl.LocationCode == item.LocationCode || (cl.Address01 == item.Address01 && cl.City == item.City && cl.State == item.State && cl.Zip01 == item.Zip01) || cl.Telephone == item.Telephone)
                                        select new
                                        {
                                            _CustomerLocationId = cl.CustomerLocationId,
                                            _Address01 = cl.Address01,
                                            _Address02 = cl.Address02,
                                            _City = cl.City,
                                            _State = cl.State,
                                            _Zip01 = cl.Zip01,
                                            _Zip02 = cl.Zip02,
                                            _Telephone = cl.Telephone
                                        }).ToList().Select(c => new CustomerLocation()
                                        {
                                            CustomerLocationId = c._CustomerLocationId,
                                            Address01 = c._Address01,
                                            Address02 = c._Address02,
                                            City = c._City,
                                            State = c._State,
                                            Zip01 = c._Zip01,
                                            Zip02 = c._Zip02,
                                            Telephone = c._Telephone
                                        }).ToList();


                    if (LocationList.Count == 0)
                    {
                        if (item != null)
                        {
                            CustomerLocation cl = new CustomerLocation();
                            cl.CustomerLocationId = Guid.NewGuid();
                            cl.Customer = item.Customer;
                            if (item.LocationName != "")
                            {
                                cl.LocationName = item.LocationName;
                            }
                            else
                            {
                                cl.LocationName = item.Address01;
                            }
                            cl.LocationCode = item.LocationCode;
                            cl.Address01 = item.Address01;
                            cl.Address02 = item.Address02;
                            cl.City = item.City;
                            cl.State = item.State;
                            cl.Zip01 = item.Zip01;
                            cl.Zip02 = item.Zip02;
                            cl.Telephone = item.Telephone;
                            cl.ActiveFlag = "Y";

                            db.CustomerLocations.Add(cl);
                            db.SaveChanges();

                            //return LocationList;
                            //return cl.CustomerLocationId;
                            OperationStatus.Add(new Tuple<Guid, IList<CustomerLocation>>(cl.CustomerLocationId, LocationList));
                        }
                        else
                        {
                            OperationStatus.Add(new Tuple<Guid, IList<CustomerLocation>>(Guid.Empty, LocationList));
                            //return LocationList;
                        }
                    }
                    else
                    {
                        OperationStatus.Add(new Tuple<Guid, IList<CustomerLocation>>(Guid.Empty, LocationList));
                        //return LocationList;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
                //return null;
            }
            return OperationStatus;
        }

        public IEnumerable<ServiceRequestModel> GetExistsWorkOrderORNOT(Guid ClientId, Guid CustomerId, Guid LocationId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var CountServiceRecord = (from s in db.ServiceRequests
                                              join c in db.Clients on s.Client equals c.ClientId
                                              join cs in db.Customers on s.Customer equals cs.CustomerId
                                              join cl in db.CustomerLocations on s.CustomerLocation equals cl.CustomerLocationId
                                              join CRS in db.ClientServiceRequestStatuses on s.ClientServiceRequestStatus equals CRS.ClientServiceRequestStatusId
                                              where s.Client == ClientId && s.Customer == CustomerId && s.CustomerLocation == LocationId
                                              select new
                                              {
                                                  ServiceRequestId = s.ServiceRequestId,
                                                  RequestNumber = s.ServiceRequestNumber,
                                                  Clientname = c.ClientName,
                                                  Customername = cs.CustomerName,
                                                  CustomerLocation = cl.LocationName,
                                                  DateCreated = s.DateCreated,
                                                  DateLastUpdated = s.DateLastUpdated,
                                                  DateWorkOrder = s.DateWorkOrder,
                                                  CustomerRefNumber = s.CustomerRefNumber,
                                                  Description = s.Description,
                                                  //ClientProblemClass = s.ProblemClassName,
                                                  //ClientProblemCode = s.ProblemCodeName,
                                                  //ClientServiceRequestPriority = s.PriorityName,
                                                  ClientServiceRequestStatus = CRS.Description,
                                                  //ServiceType = s.ServiceTypes
                                              }).ToList().Select(s => new ServiceRequestModel()
                                              {
                                                  ServiceRequestId = s.ServiceRequestId,
                                                  RequestNumber = s.RequestNumber,
                                                  Clientname = s.Clientname,
                                                  Customername = s.Customername,
                                                  CustomerLocation = s.CustomerLocation,
                                                  DateCreated = s.DateCreated,
                                                  DateLastUpdated = s.DateLastUpdated,
                                                  DateWorkOrder = s.DateWorkOrder,
                                                  CustomerRefNumber = s.CustomerRefNumber,
                                                  Description = s.Description,
                                                  ClientServiceRequestStatus = s.ClientServiceRequestStatus
                                              }).ToList();
                    return CountServiceRecord;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<WorkOrderNotesEntity> GetWorkOrderNoteGridData(Guid servicerequestid)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var CustomerNoteGridData = (from WN in db.WorkOrderNotes
                                                join CWT in db.ClientWorkOrderNoteTypes on WN.WorkOrderNotesType equals CWT.WorkOrderNoteTypeId
                                                join W in db.WorkOrders on WN.WorkOrder equals W.WorkOrderId
                                                join s in db.ServiceRequests on W.ServiceRequest equals s.ServiceRequestId
                                                join C in db.Clients on WN.Client equals C.ClientId
                                                join U in db.Users on WN.User equals U.UserId
                                                where s.ServiceRequestId == servicerequestid
                                                orderby WN.UpdateDatetime descending
                                                select new
                                                {
                                                    _WorkOrderNotesId = WN.WorkOrderNotesId,
                                                    _WorkOrderNotesType = WN.WorkOrderNotesType,
                                                    _UpdateDatetime = WN.UpdateDatetime,
                                                    _Notes = WN.Notes,
                                                    _Description = CWT.Description,
                                                    _ClientName = C.ClientName,
                                                    _Email = U.Email,
                                                    _WorkOrder = WN.WorkOrder

                                                }).ToList().Select(x => new WorkOrderNotesEntity()
                                                {
                                                    WorkOrderNotesId = x._WorkOrderNotesId,
                                                    WorkOrderNotesType = x._WorkOrderNotesType,
                                                    UpdateDatetime = x._UpdateDatetime,
                                                    Notes = x._Notes,
                                                    TypeDescription = x._Description,
                                                    ClientName = x._ClientName,
                                                    UserName = x._Email,
                                                    WorkOrder = x._WorkOrder
                                                }).ToList();

                    return CustomerNoteGridData;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<Get_serviceandworkorderattachmentdata_Result> GetServiceRequestAttachmentData(Guid ServiceRequestId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var AttachmentData = db.Get_serviceandworkorderattachmentdata(ServiceRequestId).ToList();

                    return AttachmentData;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<GetServiceRequestIvoiceQuoteCount_Result> GetServiceRequestInvoiveQuoteCount(Guid ServiceRequestId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var AttachmentData = db.GetServiceRequestIvoiceQuoteCount(ServiceRequestId).ToList();

                    return AttachmentData;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<WorkOrderAttachmentTypesEntity> GetServiceAttachmentData(Guid Attachmentid, string AttachmentType)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (AttachmentType == "R")
                    {
                        var AttachmentData = (from wa in db.ServiceRequestAttachments
                                              join cg in db.CGSFileTypes on wa.FileType equals cg.CGSFileTypesId
                                              where wa.ServiceRequestAttachmentId == Attachmentid
                                              select new
                                              {
                                                  _Attachment = wa.Attachment,
                                                  _FileName = wa.FileName,
                                                  _Decription = cg.Decription
                                              }).ToList().Select(w => new WorkOrderAttachmentTypesEntity()
                                              {
                                                  Attachment = w._Attachment,
                                                  FileName = w._FileName,
                                                  FileType = w._Decription
                                              }).ToList();
                        //var AttachmentData = db.prc_GetAttachment(Attachmentid, "ServiceRequest").ToList();
                        return AttachmentData;
                    }
                    else
                    {
                        var AttachmentData = (from wa in db.WorkOrderAttachments
                                              join cg in db.CGSFileTypes on wa.FileType equals cg.CGSFileTypesId
                                              where wa.WorkOrderAttachmentId == Attachmentid
                                              select new
                                              {
                                                  _Attachment = wa.Attachment,
                                                  _FileName = wa.FileName,
                                                  _Decription = cg.Decription
                                              }).ToList().Select(w => new WorkOrderAttachmentTypesEntity()
                                              {
                                                  Attachment = w._Attachment,
                                                  FileName = w._FileName,
                                                  FileType = w._Decription
                                              }).ToList();
                        //var AttachmentData = db.prc_GetAttachment(Attachmentid, "workorder").ToList();
                        return AttachmentData;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool SaveServiceRequestAttachments(WorkOrderAttachmentTypesEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    ServiceRequestAttachment SA = new ServiceRequestAttachment();
                    if (item.ServiceRequestAttachmentId == Guid.Empty)
                    {
                        SA.ServiceRequestAttachmentId = Guid.NewGuid();
                        SA.ServiceRequest = item.ServiceRequest;
                        SA.Client = item.Client;
                        SA.WorkOrderAttachmentType = item.WorkOrderAttachmentType;
                        SA.UploadedDate = DateTime.Now;
                        SA.FileName = item.FileName;
                        SA.FileType = db.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                        SA.User = item.User;
                        SA.Notes = item.Notes;
                        SA.Attachment = item.Attachment;
                        db.ServiceRequestAttachments.Add(SA);
                        db.SaveChanges();
                    }
                    else
                    {
                        SA = db.ServiceRequestAttachments.Where(a => a.ServiceRequestAttachmentId == item.ServiceRequestAttachmentId).FirstOrDefault();
                        if (SA != null)
                        {
                            SA.WorkOrderAttachmentType = item.WorkOrderAttachmentType;
                            //SA.UploadedDate = DateTime.Now;
                            SA.Notes = item.Notes;
                            //SA.FileName = item.FileName;
                            //SA.FileType = db.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                            //SA.Attachment = item.Attachment;                            
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<prc_GetInvoiceServiceData_Result> GetVendorInvoiceServiceData(Guid ServiceId, string Value, Guid SIHeaderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var ServiceData = db.prc_GetInvoiceServiceData(ServiceId, Value, SIHeaderId).ToList();
                return ServiceData;
            }
        }

        public IEnumerable<prc_GetServiceInvoiceServiceDetailsData_Result> GetServiceInvoiceDeatilsData(Guid ServiceId, string Value, string ProjectStatus)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var SRInvoiveData = db.prc_GetServiceInvoiceServiceDetailsData(ServiceId, Value, ProjectStatus).ToList();
                return SRInvoiveData;
            }
        }

        public Guid SaveServiceInvoiceHeader(ServiceRequestInvoiceHeaderEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    ServiceRequestInvoiceHeader si = new ServiceRequestInvoiceHeader();
                    if (item.ServiceRequestInvoiceHeaderId == Guid.Empty)
                    {
                        var UserInRole = db.ClientServiceRequestActionStatuses.
                                            Join(db.ClientServiceRequestActions, ca => ca.CGSServiceRequestActions, cg => cg.CGSServiceRequestActionId,
                                            (ca, cg) => new { ca, cg }).
                                            Join(db.InvoiceStatuses, r => r.ca.InvoiceStatus, ro => ro.InvoiceStatusId, (r, ro) => new { r, ro })
                                            .Where(m => m.ro.ActiveFlag == "Y" && m.r.cg.ActionCode == "NewQI")
                                            .Select(m => new StatusEntities
                                            {
                                                StatusId = m.ro.InvoiceStatusId,
                                                DefaultServiceRequestStatus = m.r.cg.DefaultServiceRequestStatus,
                                                CGSServiceRequestActionId = m.r.cg.CGSServiceRequestActionId
                                            }).ToList();

                        si.ServiceRequestInvoiceHeaderId = Guid.NewGuid();
                        si.ServiceRequest = item.ServiceRequest;
                        si.Client = item.Client;
                        si.Customer = item.Customer;
                        si.DateOfInvoice = DateTime.Now;
                        si.ServiceRequestInvoiceNumber = "TBD";
                        si.DateUpdated = DateTime.Now;
                        si.User = item.User;
                        si.InvoiceOrQuote = item.InvoiceOrQuote;
                        si.InvoiceStatus = UserInRole[0].StatusId;
                        db.ServiceRequestInvoiceHeaders.Add(si);

                        if (item.WorkOrderVendorInvoiceHeader != null)
                        {
                            foreach (Guid value in item.WorkOrderVendorInvoiceHeader)
                            {
                                ServiceRequestInvoiceHeader2VendorInvoice svi = new ServiceRequestInvoiceHeader2VendorInvoice();
                                svi.ServiceRequestInvoiceHeader2VendorInvoiceId = Guid.NewGuid();
                                svi.ServiceRequestInvoiceHeader = si.ServiceRequestInvoiceHeaderId;
                                svi.VendorInvoiceHeader = value;
                                db.ServiceRequestInvoiceHeader2VendorInvoice.Add(svi);
                            }
                        }
                        db.SaveChanges();
                        var DataInsertInNote = db.prc_SaveServiceRequestActionDetails(item.ServiceRequest, UserInRole[0].DefaultServiceRequestStatus, item.User, item.Client, UserInRole[0].CGSServiceRequestActionId, si.ServiceRequestInvoiceHeaderId, "Test", "NewQI");
                        return si.ServiceRequestInvoiceHeaderId;
                    }
                    else
                    {
                        si = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == item.ServiceRequestInvoiceHeaderId).FirstOrDefault();
                        if (si != null)
                        {
                            si.DateUpdated = DateTime.Now;
                            // Delete all records in ServiceRequestInvoiceHeader2VendorInvoice table
                            var allRec = db.ServiceRequestInvoiceHeader2VendorInvoice.Where(a => a.ServiceRequestInvoiceHeader == item.ServiceRequestInvoiceHeaderId);
                            db.ServiceRequestInvoiceHeader2VendorInvoice.RemoveRange(allRec);
                            db.SaveChanges();
                        }

                        if (item.WorkOrderVendorInvoiceHeader != null)
                        {
                            foreach (Guid value in item.WorkOrderVendorInvoiceHeader)
                            {
                                ServiceRequestInvoiceHeader2VendorInvoice svi = new ServiceRequestInvoiceHeader2VendorInvoice();
                                svi.ServiceRequestInvoiceHeader2VendorInvoiceId = Guid.NewGuid();
                                svi.ServiceRequestInvoiceHeader = si.ServiceRequestInvoiceHeaderId;
                                svi.VendorInvoiceHeader = value;
                                db.ServiceRequestInvoiceHeader2VendorInvoice.Add(svi);
                                db.SaveChanges();
                            }
                        }
                        return si.ServiceRequestInvoiceHeaderId;
                    }
                }
                catch (Exception ex)
                {
                    return Guid.Empty;
                }
            }
        }

        public string GenerateQuoteAndInvoiceNumber(ServiceRequestInvoiceHeaderEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    ServiceRequestInvoiceHeader si = new ServiceRequestInvoiceHeader();
                    si = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == item.ServiceRequestInvoiceHeaderId).FirstOrDefault();
                    if (item.ServiceRequestInvoiceHeaderId != Guid.Empty)
                    {
                        if (si != null)
                        {
                            var QINumber = GetServiceRequestNumber(item.InvoiceOrQuote, item.Client, item.ServiceType);
                            si.ServiceRequestInvoiceNumber = QINumber[0].Number;
                            si.ServiceRequestInvoicePrefix = QINumber[0].Prefix;
                            si.ServiceRequestInvoiceSeqNumber = QINumber[0].SeqNbr;
                            db.SaveChanges();
                        }
                    }
                    return si.ServiceRequestInvoiceNumber;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool DeleteRecordfromheader(Guid ServiceHeaderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var allRec1 = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == ServiceHeaderId);
                    db.ServiceRequestInvoiceHeaders.RemoveRange(allRec1);
                    db.SaveChanges();

                    var allRec = db.ServiceRequestInvoiceHeader2VendorInvoice.Where(a => a.ServiceRequestInvoiceHeader == ServiceHeaderId);
                    db.ServiceRequestInvoiceHeader2VendorInvoice.RemoveRange(allRec);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public IEnumerable<prc_GetServiceInvoiceDetailsDataWithWorkOrder_Result> GetServiceInvoiceDetailsData(Guid ServiceHeaderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var servicedetails = db.prc_GetServiceInvoiceDetailsDataWithWorkOrder(ServiceHeaderId).ToList();
                    return servicedetails;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool UpdatePDFOnSubmitQuote(List<ServiceInvoice> item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    foreach (ServiceInvoice value in item)
                    {
                        ServiceRequestAttachment sa = new ServiceRequestAttachment();
                        ServiceRequestInvoiceHeader SRH = new ServiceRequestInvoiceHeader();
                        SRH = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == value.ServiceRequestInvoiceHeaderId).FirstOrDefault();
                        if (SRH.ServiceRequestAttachment != null)
                        {
                            sa = db.ServiceRequestAttachments.Where(a => a.ServiceRequestAttachmentId == SRH.ServiceRequestAttachment).FirstOrDefault();
                            sa.FileName = value.FileName;
                            sa.Attachment = value.Attachment;
                            db.SaveChanges();
                            break;
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }
        }
        public bool SaveServiceInvoiceDetails(List<ServiceInvoice> item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    //var QuotePDF = WO.GetCustomerinformation(item[0].ServiceRequest);
                    foreach (ServiceInvoice value in item)
                    {
                        ServiceRequestAttachment sa = new ServiceRequestAttachment();
                        ServiceRequestInvoiceHeader SRH = new ServiceRequestInvoiceHeader();


                        SRH = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == value.ServiceRequestInvoiceHeaderId).FirstOrDefault();
                        if (SRH.ServiceRequestAttachment != null)
                        {
                            UOWWorkOrder WO = new UOWWorkOrder();
                            byte[] customerlist = WO.GetCustomerinformation1(value.ServiceRequest, value.Client,item,SRH.ServiceRequestInvoiceNumber,SRH.DateOfInvoice);

                            var sra = db.ServiceRequestAttachments.Where(a => a.ServiceRequestAttachmentId == SRH.ServiceRequestAttachment);
                            db.ServiceRequestAttachments.RemoveRange(sra);

                            var allRec = db.ServiceRequestInvoiceDetails.Where(a => a.ServiceRequestInvoiceHeader == value.ServiceRequestInvoiceHeaderId);
                            db.ServiceRequestInvoiceDetails.RemoveRange(allRec);

                            sa.ServiceRequestAttachmentId = Guid.NewGuid();
                            sa.ServiceRequest = value.ServiceRequest;
                            sa.Client = value.Client;
                            sa.WorkOrderAttachmentType = db.ClientAttachmentTypes.Where(a => a.Name == "Service Quote/Invoice").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                            sa.UploadedDate = DateTime.Now;
                            sa.FileName = value.FileName;
                            sa.FileType = db.CGSFileTypes.Where(a => a.Decription == "application/pdf").Select(x => x.CGSFileTypesId).FirstOrDefault();
                            sa.User = value.User;
                            sa.Notes = value.Description;
                            sa.Attachment = customerlist;
                            db.ServiceRequestAttachments.Add(sa);

                            SRH.Description = value.Description;
                            SRH.DateUpdated = DateTime.Now;
                            SRH.ServiceRequestAttachment = sa.ServiceRequestAttachmentId;

                            db.SaveChanges();
                            break;
                        }
                        else
                        {
                            UOWWorkOrder WO = new UOWWorkOrder();
                            byte[] customerlist = WO.GetCustomerinformation1(value.ServiceRequest, value.Client, item, SRH.ServiceRequestInvoiceNumber, SRH.DateOfInvoice);

                            sa.ServiceRequestAttachmentId = Guid.NewGuid();
                            sa.ServiceRequest = value.ServiceRequest;
                            sa.Client = value.Client;
                            sa.WorkOrderAttachmentType = db.ClientAttachmentTypes.Where(a => a.Name == "Service Quote/Invoice").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                            sa.UploadedDate = DateTime.Now;
                            sa.FileName = value.FileName;
                            sa.FileType = db.CGSFileTypes.Where(a => a.Decription == "application/pdf").Select(x => x.CGSFileTypesId).FirstOrDefault();
                            sa.User = value.User;
                            sa.Notes = value.Description;
                            sa.Attachment = customerlist;
                            db.ServiceRequestAttachments.Add(sa);

                            SRH.Description = value.Description;
                            SRH.DateUpdated = DateTime.Now;
                            SRH.ServiceRequestAttachment = sa.ServiceRequestAttachmentId;
                            
                            db.SaveChanges();
                            break;



                            //sa.ServiceRequestAttachmentId = Guid.NewGuid();
                            //sa.ServiceRequest = value.ServiceRequest;
                            //sa.Client = value.Client;
                            //sa.WorkOrderAttachmentType = db.ClientAttachmentTypes.Where(a => a.Name == "Service Quote/Invoice").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                            //sa.UploadedDate = DateTime.Now;
                            //sa.FileName = value.FileName;
                            //sa.FileType = db.CGSFileTypes.Where(a => a.Decription == value.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                            //sa.User = value.User;
                            //sa.Notes = value.Description;
                            //sa.Attachment = value.Attachment;
                            //db.ServiceRequestAttachments.Add(sa);

                            //SRH.Description = value.Description;
                            //SRH.DateUpdated = DateTime.Now;
                            //SRH.ServiceRequestAttachment = sa.ServiceRequestAttachmentId;

                            //db.SaveChanges();
                            //break;
                        }

                        //ServiceRequestInvoiceHeader si = new ServiceRequestInvoiceHeader();
                        //si = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == value.ServiceRequestInvoiceHeaderId).FirstOrDefault();
                        //if (si != null)
                        //{
                        //    si.Description = value.Description;
                        //    si.DateUpdated = DateTime.Now;
                        //    si.ServiceRequestAttachment = sa.ServiceRequestAttachmentId;

                        //    var allRec = db.ServiceRequestInvoiceDetails.Where(a => a.ServiceRequestInvoiceHeader == value.ServiceRequestInvoiceHeaderId);
                        //    db.ServiceRequestInvoiceDetails.RemoveRange(allRec);
                        //    db.SaveChanges();
                        //    break;
                        //}
                    }

                    foreach (ServiceInvoice value in item)
                    {
                        if (value.STotal > 0)
                        {
                            ServiceRequestInvoiceDetail sri = new ServiceRequestInvoiceDetail();
                            sri.ServiceRequestInvoiceDetailId = Guid.NewGuid();
                            sri.ServiceRequestInvoiceHeader = value.ServiceRequestInvoiceHeaderId;
                            sri.ClassOfGoodId = value.ClassOfGoodId;
                            sri.Notes = value.SNotes;
                            sri.Amount = value.SAmount;
                            sri.Tax = value.STax;
                            sri.Total = value.STotal;
                            db.ServiceRequestInvoiceDetails.Add(sri);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public IList<StatusEntities> GetServiceInvoiceStatus(string item, Guid Clientid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var StatusList = (from ca in db.ClientServiceRequestActionStatuses
                                      join cg in db.ClientServiceRequestActions on ca.CGSServiceRequestActions equals cg.CGSServiceRequestActionId
                                      join s in db.InvoiceStatuses on ca.InvoiceStatus equals s.InvoiceStatusId
                                      where s.ActiveFlag == "Y" && cg.ActionCode == item && s.Client == Clientid
                                      orderby s.SortOrder ascending
                                      select new
                                      {
                                          StatusId = s.InvoiceStatusId,
                                          Description = s.Description,
                                          SortOrder = s.SortOrder

                                      }).Distinct().ToList().Select(x => new StatusEntities()
                                      {
                                          StatusId = x.StatusId,
                                          Description = x.Description,
                                          SortOrder = x.SortOrder

                                      }).ToList();
                    return StatusList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool SubmitQuoteWithEmail(ServiceRequestInvoiceAction1 item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserInRole = db.ClientServiceRequestActionStatuses.
                                            Join(db.ClientServiceRequestActions, ca => ca.CGSServiceRequestActions, cg => cg.CGSServiceRequestActionId,
                                            (ca, cg) => new { ca, cg }).
                                            Join(db.InvoiceStatuses, r => r.ca.InvoiceStatus, ro => ro.InvoiceStatusId, (r, ro) => new { r, ro })
                                            .Where(m => m.ro.ActiveFlag == "Y" && m.r.cg.ActionCode == item.ActionCode)
                                            .Select(m => new StatusEntities
                                            {
                                                StatusId = m.ro.InvoiceStatusId,
                                                DefaultServiceRequestStatus = m.r.cg.DefaultServiceRequestStatus,
                                                CGSServiceRequestActionId = m.r.cg.CGSServiceRequestActionId
                                            }).ToList();

                    ServiceRequestInvoiceHeader SRH = new ServiceRequestInvoiceHeader();
                    //ServiceRequest sr = new ServiceRequest();

                    //sr = db.ServiceRequests.Where(a => a.ServiceRequestId == item.ServiceRequest).FirstOrDefault();
                    SRH = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == item.ServiceRequestInvoiceHeaderId).FirstOrDefault();
                    if (SRH != null)
                    {
                        //    sr.ClientServiceRequestStatus = UserInRole[0].DefaultServiceRequestStatus;
                        //    sr.DateLastUpdated = DateTime.Now;
                        SRH.InvoiceStatus = UserInRole[0].StatusId;
                        db.SaveChanges();
                    }
                    var DataInsertInNote = db.prc_SaveServiceRequestActionDetails(item.ServiceRequest, UserInRole[0].DefaultServiceRequestStatus, item.Userid, item.ClientId, UserInRole[0].CGSServiceRequestActionId, item.ServiceRequestInvoiceHeaderId, item.Notes, item.ActionCode);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public byte[] GetServiceInvoicePDFData(Guid ServiceRequestInvoiceHeaderId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var PDFData = db.ServiceRequestInvoiceHeaders.Join(db.ServiceRequestAttachments, wo => wo.ServiceRequestAttachment, woa => woa.ServiceRequestAttachmentId, (wo, woa) => new { Attachment = woa.Attachment, wo.ServiceRequestInvoiceHeaderId }).Where(wo => wo.ServiceRequestInvoiceHeaderId == ServiceRequestInvoiceHeaderId).FirstOrDefault().Attachment;

                    return PDFData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Tuple<byte[], string>> GetServiceQuoteAttachmentData(Guid value)
        {
            List<Tuple<byte[], string>> Values = new List<Tuple<byte[], string>>();

            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var DocData = (from wa in db.ServiceRequestInvoiceHeaders
                                   join sra in db.ServiceRequestAttachments on wa.ServiceRequestAttachment equals sra.ServiceRequestAttachmentId
                                   where wa.ServiceRequestInvoiceHeaderId == value
                                   select new
                                   {
                                       _Attachment = sra.Attachment,
                                       _FileName = sra.FileName
                                   }).ToList().Select(w => new WorkOrderAttachment()
                                   {
                                       Attachment = w._Attachment,
                                       FileName = w._FileName
                                   }).ToList();
                    if (DocData != null)
                    {
                        Values.Add(new Tuple<byte[], string>(DocData[0].Attachment, DocData[0].FileName));
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
                return Values;
            }
        }

        public IList<ServiceClassOfGood> GetServiceInvoiceWthoutWorkorder(Guid ServiceHeaderId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var ServiceHeaderData = db.ServiceRequestInvoiceHeaders.Where(a => a.ServiceRequestInvoiceHeaderId == ServiceHeaderId).FirstOrDefault();

                    var Data = db.ClientClassOfGoods.Select(a => new ServiceClassOfGood
                    {
                        ClassOfGoodId = a.ClassOfGoodId,
                        Name = a.Name,
                        TaxClassOfGoods = a.TaxClassOfGoods,
                        DateOfInvoice = ServiceHeaderData.DateOfInvoice,
                        ServiceRequestInvoiceHeaderId = ServiceHeaderData.ServiceRequestInvoiceHeaderId,
                        Client = ServiceHeaderData.Client,
                        ServiceRequestInvoiceNumber = ServiceHeaderData.ServiceRequestInvoiceNumber
                    }).ToList();

                    return Data;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<fn_getClientResourceData_Result> GetResourceSalesTaxDetails(Guid ClientId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var SalesTaxDetails = db.fn_getClientResourceData(ClientId, "Tax Cloud Sales Tax").ToList();
                    return SalesTaxDetails;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }
        public List<SalesTaxClouddetails> GetTaxCloudAPIDetails()
        {
            List<SalesTaxClouddetails> details = new List<SalesTaxClouddetails>();
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var AddressURLDetails = (from rth in db.ResourceTypeHeaders
                                             join rtd in db.ResourceTypeDetails on rth.ResourceTypeHeadersId equals rtd.ResourceTypeHeader
                                             join crd in db.ClientResourceDetails on rtd.ResourceTypeDetailsId equals crd.ResourceTypeDetail
                                             where rth.Name == "Tax Cloud Sales Tax" && rtd.Description == "The Base URL for the call"
                                             select new
                                             {
                                                 Value = crd.Value
                                             }).FirstOrDefault();

                    var AddressAPIIDDetails = (from rth in db.ResourceTypeHeaders
                                               join rtd in db.ResourceTypeDetails on rth.ResourceTypeHeadersId equals rtd.ResourceTypeHeader
                                               join crd in db.ClientResourceDetails on rtd.ResourceTypeDetailsId equals crd.ResourceTypeDetail
                                               where rth.Name == "Tax Cloud Sales Tax" && rtd.Description == "The key for this resource can be different in each environment"
                                               select new
                                               {
                                                   Value = crd.Value
                                               }).FirstOrDefault();
                    SalesTaxClouddetails stc = new SalesTaxClouddetails();
                    stc.URL = AddressURLDetails.Value;
                    stc.APIKey = AddressAPIIDDetails.Value;
                    details.Add(stc);

                    return details;

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public string GetClientEmailAddress(Guid ClientId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var EmailAddress = db.ClientUsers.Join(db.Users, VU => VU.User, U => U.UserId, (VU, U) => new { VU, U.Email, VU.ActiveFlag }).Join(db.Clients, r => r.VU.Client, ro => ro.ClientId, (r, ro) => new { r, ro, r.ActiveFlag, r.Email }).Where(m => m.ro.ActiveFlag == "Y" && m.r.ActiveFlag == "Y" && m.ro.ClientId == ClientId).FirstOrDefault();
                    return EmailAddress.Email;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
        public IEnumerable<Get_ServiceQuoteUserStatusGridData_Result> GetServiceQuoteUserStatusGridData(Guid ServiceHeaderId, string CallStatus)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserList = db.Get_ServiceQuoteUserStatusGridData(ServiceHeaderId, CallStatus).ToList();
                    return UserList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public bool SaveAuthorizeAndCaptureAPIResponse(ServiceRequestInvoiceHeaderApiResponseEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    ServiceRequestInvoiceHeaderApiResponse sar = new ServiceRequestInvoiceHeaderApiResponse();
                    sar.ServiceRequestInvoiceHeaderApiResponseId = Guid.NewGuid();
                    sar.CartID = item.CartID;
                    sar.ResponseType = item.ResponseType;
                    sar.ResponseMessage = item.ResponseMessage;
                    sar.ResponseDate = DateTime.Now;
                    sar.ServiceRequestInvoiceHeader = item.ServiceRequestInvoiceHeader;
                    db.ServiceRequestInvoiceHeaderApiResponses.Add(sar);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
