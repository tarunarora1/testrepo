using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.BusinessEntityClasses;
using DataModel;
using UnitOfWork.Interface;
using UnitOfwork.Interfaces;
using BussinessEntities.BusinessEntityClasses;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using UnitOfWork.UOWRepository;

namespace UnitOfwork.UOWRepository
{
    public class UOWPMModule : IPMModule
    {
        public DataTable SaveDataOnPmHeader(PMHeaderEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    ClientPMHeader CH = new ClientPMHeader();
                    #region ADD/Update the PM header entries
                    if (item.ClientPMHeaderId != Guid.Empty)
                        CH = db.ClientPMHeaders.Where(a => a.ClientPMHeaderId == item.ClientPMHeaderId).FirstOrDefault();
                    else
                        CH.ClientPMHeaderId = Guid.NewGuid();

                    CH.Client = item.Client;
                    CH.Customer = item.Customer;
                    CH.ProblemClass = item.ProblemClass;
                    CH.ProblemCode = item.ProblemCode;
                    CH.RequestPriority = item.RequestPriority;
                    CH.ServiceRequestType = item.ServiceRequestType;
                    CH.CustomerReference = item.CustomerReference;
                    CH.IssueDescription = item.IssueDescription;
                    CH.Frequency = item.Frequency;
                    CH.BeginDate = item.BeginDate;
                    CH.EndDate = item.EndDate;
                    CH.ArriveDateAndTime = item.ArriveDateAndTime;
                    CH.FinishDateAndTime = item.FinishDateAndTime;
                    CH.WOInAdvance = item.WOInAdvance;
                    CH.CreatedByUser = item.UserId;
                    CH.ActiveFlag = "N";

                    if (item.ClientPMHeaderId == Guid.Empty)
                        db.ClientPMHeaders.Add(CH);

                    db.SaveChanges();
                    #endregion


                    #region CALCULATE and insert the schedular entries and return
                    DataTable dt = GetWOConfirmationDate(CH.ClientPMHeaderId);
                    DataView view = dt.DefaultView;
                    view.Sort = "WorkOrderCreationDate ASC";
                    DataTable sortedDate = view.ToTable();
                    #endregion
                    return dt;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public DataTable GetWOConfirmationDate(Guid clientPMHeaderID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    DataTable dt = new DataTable();

                    //1 Get Original Header Data
                    //ClientPMHeader item = db.ClientPMHeaders.Where(a => a.ClientPMHeaderId == clientPMHeaderID).FirstOrDefault();
                    ClientPMHeader ClientPMHeader = db.ClientPMHeaders.Where(a => a.ClientPMHeaderId == clientPMHeaderID).FirstOrDefault();
                    if (ClientPMHeader != null)
                    {
                        dt.Columns.Add("WorkOrderCreationDate", typeof(string));
                        dt.Columns.Add("WorkOrderArrivalDate", typeof(string));
                        dt.Columns.Add("FinishDateAndTime", typeof(string));

                        //2) Get the latest Arrvedate-Advance days value (Need to have this calculated everytime)
                        DateTime WorkOrderInitialArrivalDate = ClientPMHeader.ArriveDateAndTime.AddDays(-ClientPMHeader.WOInAdvance);

                        //3) Delete all those existing records having work order creation date greater than equal to current date
                        List<PMWorkOrderCreationDate> pMWorkOrderExistingRecords = (from pm in db.PMWorkOrderCreationDates
                                                                                    where pm.ClientPMHeader == ClientPMHeader.ClientPMHeaderId
                                                                                    select pm).ToList();
                        if (pMWorkOrderExistingRecords != null)
                        {
                            pMWorkOrderExistingRecords = pMWorkOrderExistingRecords.Where(a => Convert.ToDateTime(a.WorkOrderCreationDate) > DateTime.Now).ToList();
                            if (pMWorkOrderExistingRecords.Count > 0)
                            {
                                db.PMWorkOrderCreationDates.RemoveRange(pMWorkOrderExistingRecords);
                                db.SaveChanges();
                            }
                        }


                        //4) based on frequency, calculate new dates
                        int countEntityAdded = 0;
                        bool ContinueAdding = true;
                        UOWServiceRequest uOWServiceRequest = new UOWServiceRequest();
                        CGSInterval cGSIntervals = uOWServiceRequest.GetCGSInterval().Where(a => a.IntervalId == ClientPMHeader.Frequency).FirstOrDefault();

                        DataRow dr = dt.NewRow();
                        //initial start row
                        dr["WorkOrderCreationDate"] = WorkOrderInitialArrivalDate;
                        dr["WorkOrderArrivalDate"] = ClientPMHeader.ArriveDateAndTime;
                        dr["FinishDateAndTime"] = ClientPMHeader.FinishDateAndTime;
                        dt.Rows.Add(dr);

                        do
                        {
                            //calculate next arrival date from initial arrival date
                            DataRow drNext = dt.NewRow();

                            //rest of the calculation of rows
                            switch (cGSIntervals.IntervalName)
                            {
                                case "Monthly 30":
                                    countEntityAdded += 1;
                                    if (!(WorkOrderInitialArrivalDate.AddMonths(countEntityAdded) <= ClientPMHeader.EndDate))
                                    {
                                        ContinueAdding = false;
                                        break;
                                    }

                                    drNext["WorkOrderCreationDate"] = WorkOrderInitialArrivalDate.AddMonths(countEntityAdded);
                                    drNext["WorkOrderArrivalDate"] = ClientPMHeader.ArriveDateAndTime.AddMonths(countEntityAdded);
                                    drNext["FinishDateAndTime"] = ClientPMHeader.FinishDateAndTime.AddMonths(countEntityAdded);
                                    dt.Rows.Add(drNext);
                                    break;
                                case "Bi-Monthly 60":
                                    countEntityAdded += 2;
                                    if (!(WorkOrderInitialArrivalDate.AddMonths(countEntityAdded) <= ClientPMHeader.EndDate))
                                    {
                                        ContinueAdding = false;
                                        break;
                                    }

                                    drNext["WorkOrderCreationDate"] = WorkOrderInitialArrivalDate.AddMonths(countEntityAdded);
                                    drNext["WorkOrderArrivalDate"] = ClientPMHeader.ArriveDateAndTime.AddMonths(countEntityAdded);
                                    drNext["FinishDateAndTime"] = ClientPMHeader.FinishDateAndTime.AddMonths(countEntityAdded);
                                    dt.Rows.Add(drNext);
                                    break;
                                case "Quarterly 90":
                                    countEntityAdded += 3;
                                    if (!(WorkOrderInitialArrivalDate.AddMonths(countEntityAdded) <= ClientPMHeader.EndDate))
                                    {
                                        ContinueAdding = false;
                                        break;
                                    }

                                    drNext["WorkOrderCreationDate"] = WorkOrderInitialArrivalDate.AddMonths(countEntityAdded);
                                    drNext["WorkOrderArrivalDate"] = ClientPMHeader.ArriveDateAndTime.AddMonths(countEntityAdded);
                                    drNext["FinishDateAndTime"] = ClientPMHeader.FinishDateAndTime.AddMonths(countEntityAdded);
                                    dt.Rows.Add(drNext);
                                    break;
                                case "Weekly 7":
                                    countEntityAdded += 7;
                                    if (!(WorkOrderInitialArrivalDate.AddDays(countEntityAdded) <= ClientPMHeader.EndDate))
                                    {
                                        ContinueAdding = false;
                                        break;
                                    }

                                    drNext["WorkOrderCreationDate"] = WorkOrderInitialArrivalDate.AddDays(countEntityAdded);
                                    drNext["WorkOrderArrivalDate"] = ClientPMHeader.ArriveDateAndTime.AddDays(countEntityAdded);
                                    drNext["FinishDateAndTime"] = ClientPMHeader.FinishDateAndTime.AddDays(countEntityAdded);
                                    dt.Rows.Add(drNext);
                                    break;
                                case "Bi-Weekly 14":
                                    countEntityAdded += 14;
                                    if (!(WorkOrderInitialArrivalDate.AddDays(countEntityAdded) <= ClientPMHeader.EndDate))
                                    {
                                        ContinueAdding = false;
                                        break;
                                    }

                                    drNext["WorkOrderCreationDate"] = WorkOrderInitialArrivalDate.AddDays(countEntityAdded);
                                    drNext["WorkOrderArrivalDate"] = ClientPMHeader.ArriveDateAndTime.AddDays(countEntityAdded);
                                    drNext["FinishDateAndTime"] = ClientPMHeader.FinishDateAndTime.AddDays(countEntityAdded);
                                    dt.Rows.Add(drNext);
                                    break;
                            }

                        } while (ContinueAdding);


                        ////3) Delete all those existing records having work order creation date greater than equal to current date
                        //List<PMWorkOrderCreationDate> pMWorkOrderExistingRecordsnew = (from pm in db.PMWorkOrderCreationDates
                        //                                                               where pm.ClientPMHeader == ClientPMHeader.ClientPMHeaderId
                        //                                                               select pm).ToList();
                        //if (pMWorkOrderExistingRecordsnew != null && pMWorkOrderExistingRecordsnew.Count > 0)
                        //{
                        //    db.PMWorkOrderCreationDates.RemoveRange(pMWorkOrderExistingRecordsnew);
                        //    db.SaveChanges();
                        //}


                        foreach (DataRow value in dt.Rows)
                        {
                            PMWorkOrderCreationDate PMWCD = new PMWorkOrderCreationDate();
                            if (Convert.ToDateTime(value["WorkOrderCreationDate"]) > DateTime.Now)
                            {
                               
                                PMWCD.PMWorkOrderCreationDateId = Guid.NewGuid();
                                PMWCD.ClientPMHeader = clientPMHeaderID;
                                PMWCD.WorkOrderCreationDate = Convert.ToDateTime(value["WorkOrderCreationDate"]);
                                PMWCD.WorkOrderArriveDate = Convert.ToDateTime(value["WorkOrderArrivalDate"]);
                                PMWCD.WorkOrderFinishDate = Convert.ToDateTime(value["FinishDateAndTime"]);
                                db.PMWorkOrderCreationDates.Add(PMWCD);
                                db.SaveChanges();
                            }
                        }
                    }

                    return dt;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public DataTable GetWOEditRecord(Guid clientPMHeaderID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                List<PMWorkOrderCreationDate> WOEditRecord = (from pm in db.PMWorkOrderCreationDates
                                                              where pm.ClientPMHeader == clientPMHeaderID
                                                              select pm).OrderBy(x => x.WorkOrderCreationDate).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("WorkOrderDate", typeof(string));
                dt.Columns.Add("ArriveDateAndTime", typeof(string));
                dt.Columns.Add("FinishDateAndTime", typeof(string));

                foreach (PMWorkOrderCreationDate pMWorkOrder in WOEditRecord)
                {
                    DataRow dr = dt.NewRow();
                    dr["WorkOrderDate"] = Convert.ToDateTime(pMWorkOrder.WorkOrderCreationDate).ToString("MM-dd-yyyy");
                    dr["ArriveDateAndTime"] = Convert.ToDateTime(pMWorkOrder.WorkOrderArriveDate).ToString("MM-dd-yyyy");
                    dr["FinishDateAndTime"] = Convert.ToDateTime(pMWorkOrder.WorkOrderFinishDate).ToString("MM-dd-yyyy");

                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }

        //public DataTable GetWOConfirmationDate(string PmHeaderId)
        //{
        //    SqlCommand objCmd;
        //    SqlDataAdapter da;
        //    using (FacilitiesEntities db = new FacilitiesEntities())
        //    {
        //        try
        //        {
        //            objCmd = new SqlCommand();
        //            //SqlConnection SLC = GetDBConnection();
        //            SqlConnection SLC = new SqlConnection(@"server = 173.248.153.60,1533; database = Facilities-dev; Integrated Security = false; connection timeout = 50; uid = FacilitiesUser; password = CP9p4Oietj3Ai");

        //            objCmd.Connection = SLC;
        //            SLC.Open();

        //            objCmd.CommandText = "prc_GetWOConfirmationDate";
        //            objCmd.CommandType = CommandType.StoredProcedure;
        //            objCmd.Parameters.AddWithValue("@PMHeaderId", PmHeaderId);
        //            // objCmd.Parameters.AddWithValue("@Frequency", Frequency);
        //            da = new SqlDataAdapter(objCmd);
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);

        //            //var data = db.prc_GetWOConfirmationDate(PmHeaderId).ToList();
        //            var HeaderID = new Guid(PmHeaderId);

        //            foreach (DataRow value in dt.Rows)
        //            {
        //                PMWorkOrderCreationDate PMWCD = new PMWorkOrderCreationDate();
        //                PMWCD.PMWorkOrderCreationDateId = Guid.NewGuid();
        //                PMWCD.ClientPMHeader = HeaderID;
        //                PMWCD.WorkOrderCreationDate = Convert.ToDateTime(value["WorkOrderDate"]);
        //                PMWCD.WorkOrderArriveDate = Convert.ToDateTime(value["ArriveDateAndTime"]);
        //                PMWCD.WorkOrderFinishDate = Convert.ToDateTime(value["FinishDateAndTime"]);
        //                db.PMWorkOrderCreationDates.Add(PMWCD);
        //                db.SaveChanges();
        //            }
        //            //return da;
        //            return dt;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw (ex);
        //        }
        //    }
        //}

        public bool UpdatePMConfirmation(Guid PMHeaderId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var data = db.ClientPMHeaders.Where(a => a.ClientPMHeaderId == PMHeaderId).FirstOrDefault();
                    data.ActiveFlag = "Y";
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool SaveDataOnPMVendorCustomerLocations(PmVendorCustomerLocationEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    foreach (var value in item.CustomerLocation)
                    {
                        PMVendorCustomerLocation PVCL = new PMVendorCustomerLocation();
                        PVCL.PMVendorCustomerLocationId = Guid.NewGuid();
                        PVCL.CustomerLocation = value;
                        PVCL.PMVendor = item.PMVendor;
                        db.PMVendorCustomerLocations.Add(PVCL);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return true;

        }

        public bool DeleteAssociatedCustomerLocationAndVendor(Guid VenderId, int AssociatedVenderLocationCount)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (AssociatedVenderLocationCount > 0)
                    {
                        var PMVendorCustomerLocations = db.PMVendorCustomerLocations.Where(x => x.PMVendor == VenderId);
                        db.PMVendorCustomerLocations.RemoveRange(PMVendorCustomerLocations);

                        var DeleteDataFromPmVendor = db.PMVendors.Where(a => a.PMVendorId == VenderId);
                        db.PMVendors.RemoveRange(DeleteDataFromPmVendor);
                    }
                    else
                    {
                        var DeleteDataFromPmVendor = db.PMVendors.Where(a => a.PMVendorId == VenderId);
                        db.PMVendors.RemoveRange(DeleteDataFromPmVendor);
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public bool SaveDataOnPMVendors(PMVendorsEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var vendorID = db.Vendors.Where(v => v.VendorName.Contains(item.Vendor)).Select(a => a.VendorId).FirstOrDefault();
                    PMVendor PV = new PMVendor();
                    PV.PMVendorId = Guid.NewGuid();
                    PV.PMHeader = item.PMHeader;
                    PV.Vendor = vendorID;
                    PV.WONTE = item.WONTE;
                    PV.Description = item.Description;
                    db.PMVendors.Add(PV);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return true;
        }

        public List<PMVendorsEntity> GetAssiocateVendor(Guid PMHeaderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var VendorList = (from PV in db.PMVendors
                                      join v in db.Vendors on PV.Vendor equals v.VendorId
                                      where PV.PMHeader == PMHeaderId
                                      orderby v.VendorName descending
                                      select new
                                      {
                                          _PMVendorId = PV.PMVendorId,
                                          _VendorName = v.VendorName,
                                          _WONTE = PV.WONTE,
                                          _Description = PV.Description
                                      }).ToList().Select(x => new PMVendorsEntity()
                                      {
                                          PMVendorId = x._PMVendorId,
                                          Vendor = x._VendorName,
                                          WONTE = x._WONTE,
                                          Description = x._Description
                                      }).ToList();
                    return VendorList;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public List<prc_GetPMConfirmationData_Result> GetPMConfirnationData(Guid PmHeaderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var data = db.prc_GetPMConfirmationData(PmHeaderId).ToList();
                    return data;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public static SqlConnection GetDBConnection()
        {
            string SqlConnStr = ConfigurationManager.AppSettings["SqlDSN"].ToString();
            SqlConnection objDbConn = new SqlConnection(SqlConnStr);
            return objDbConn;
        }

        public CustomerLocationEntity GetLatestCustomerInformation(Guid PMVendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserLocation = (from PL in db.PMVendorCustomerLocations
                                        join CL in db.CustomerLocations on PL.CustomerLocation equals CL.CustomerLocationId
                                        join C in db.Customers on CL.Customer equals C.CustomerId
                                        join PV in db.PMVendors on PL.PMVendor equals PV.PMVendorId
                                        where PL.PMVendor == PMVendorId
                                        select new
                                        {
                                            _PMVendorCustomerLocationId = PL.PMVendorCustomerLocationId,
                                            _CustomerName = C.CustomerName,
                                            _PMVendor = PL.PMVendor,
                                            _CustomerLocationId = CL.CustomerLocationId,
                                            _LocationName = CL.LocationName,
                                            _LocationCode = CL.LocationCode,
                                            _Address01 = CL.Address01,
                                            _City = CL.City,
                                            _State = CL.State,
                                            _Zip01 = CL.Zip01,
                                            _Telephone = CL.Telephone
                                        }).ToList().Select(m => new CustomerLocationEntity()
                                        {
                                            PMVendorCustomerLocationId = m._PMVendorCustomerLocationId,
                                            CustomerLocationId = m._CustomerLocationId,
                                            LocationName = m._LocationName,
                                            CustomerName = m._CustomerName,
                                            LocationCode = m._LocationCode,
                                            PMVendor = m._PMVendor,
                                            Address01 = m._Address01,
                                            City = m._City,
                                            State = m._State,
                                            Zip01 = m._Zip01,
                                            Telephone = m._Telephone
                                        }).FirstOrDefault();

                    return UserLocation;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public List<CustomerLocationEntity> GetDataFromPMVendorCustomerLocations(Guid PMVendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserLocation = (from PL in db.PMVendorCustomerLocations
                                        join CL in db.CustomerLocations on PL.CustomerLocation equals CL.CustomerLocationId
                                        join PV in db.PMVendors on PL.PMVendor equals PV.PMVendorId
                                        where PL.PMVendor == PMVendorId
                                        select new
                                        {
                                            _PMVendorCustomerLocationId = PL.PMVendorCustomerLocationId,
                                            _PMVendor = PL.PMVendor,
                                            _CustomerLocationId = CL.CustomerLocationId,
                                            _LocationName = CL.LocationName,
                                            _LocationCode = CL.LocationCode,
                                            _Address01 = CL.Address01,
                                            _City = CL.City,
                                            _State = CL.State,
                                            _Zip01 = CL.Zip01,
                                            _Telephone = CL.Telephone
                                        }).ToList().Select(m => new CustomerLocationEntity()
                                        {
                                            PMVendorCustomerLocationId = m._PMVendorCustomerLocationId,
                                            CustomerLocationId = m._CustomerLocationId,
                                            LocationName = m._LocationName,
                                            LocationCode = m._LocationCode,
                                            PMVendor = m._PMVendor,
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
                    throw (ex);
                }
            }

        }

        public bool DeleteSelectedPMVendorCustomerLocations(PMCustomerLOcations SelectedCustomerLocations)
        {

            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    foreach (var data in SelectedCustomerLocations.SelectedCustomerLocations)
                    {
                        var PMVendorCustomerLocations = db.PMVendorCustomerLocations.Where(a => a.PMVendorCustomerLocationId == data).ToList();
                        db.PMVendorCustomerLocations.RemoveRange(PMVendorCustomerLocations);
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                return true;
            }
        }

        public List<prc_GetPMModuleDetails_Result> GetClientpmheadersData(Guid ClientId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var data = db.prc_GetPMModuleDetails(ClientId).ToList();
                    return data;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public List<prc_GetPMWorkOrder_Result> GetWorkOrderData(Guid PMVendorId, Guid ClientId, Guid ClientPMHeaderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var data = db.prc_GetPMWorkOrder(PMVendorId, ClientId, ClientPMHeaderId).ToList();
                    return data;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public List<CustomerLocationEntity> GetCustomerLocation(Guid CustomerId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserLocation = (from cl in db.CustomerLocations
                                        where cl.Customer == CustomerId
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
                    throw (ex);
                }
            }

        }
    }
}
