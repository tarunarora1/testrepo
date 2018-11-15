using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;
using UnitOfWork.Interface;
using BusinessEntities.BusinessEntityClasses;
using System.IO;
using PDFCreation;

namespace UnitOfWork.UOWRepository
{
    public class UOWWorkOrder : IWorkOrder
    {
        public object WorkOrderPDF { get; private set; }

        public IEnumerable<ClientEntities> GetClient()
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var ClientList = (from C in DB.Clients
                                      where C.ActiveFlag == "Y"
                                      select new
                                      {
                                          ClientId = C.ClientId,
                                          ClientName = C.ClientName

                                      }).ToList().Select(x => new ClientEntities()
                                      {
                                          ClientId = x.ClientId,
                                          ClientName = x.ClientName

                                      }).ToList();
                    return ClientList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<VendorEntities> GetVendor(Guid _ClientId)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var VendorList = (from C in DB.Vendors
                                      join Cl in DB.ClientVendors on C.VendorId equals Cl.Vendor
                                      join cli in DB.Clients on Cl.Client equals cli.ClientId
                                      where cli.ClientId == _ClientId && C.ActiveFlag == "Y" && Cl.ActiveFlag == "Y"
                                      select new
                                      {
                                          VendorId = C.VendorId,
                                          VendorName = C.VendorName

                                      }).ToList().Select(x => new VendorEntities()
                                      {
                                          VendorId = x.VendorId,
                                          VendorName = x.VendorName

                                      }).ToList();
                    return VendorList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<StatusEntities> GetStatus()
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var StatusList = (from ca in DB.ClientActionStatuses
                                      join cg in DB.CGSActions on ca.CGSAction equals cg.CGSActionId
                                      join s in DB.Statuses on ca.Status equals s.StatusId
                                      where s.ActiveFlag == "Y" && cg.ActionCode == "CreateWO"
                                      orderby s.SortOrder ascending
                                      select new
                                      {
                                          StatusId = s.StatusId,
                                          Description = s.Description,
                                          SortOrder = s.SortOrder

                                      }).ToList().Select(x => new StatusEntities()
                                      {
                                          StatusId = x.StatusId,
                                          Description = x.Description,
                                          SortOrder = x.SortOrder

                                      }).ToList();
                    return StatusList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<StatusEntities> GetStatusForActionTab(string item, Guid ClientId)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var StatusList = (from ca in DB.ClientActionStatuses
                                      join cg in DB.CGSActions on ca.CGSAction equals cg.CGSActionId
                                      join s in DB.Statuses on ca.Status equals s.StatusId
                                      where s.ActiveFlag == "Y" && cg.ActionCode == item && ca.Client == ClientId
                                      orderby s.SortOrder ascending
                                      select new
                                      {
                                          StatusId = s.StatusId,
                                          Description = s.Description,
                                          SortOrder = s.SortOrder

                                      }).ToList().Select(x => new StatusEntities()
                                      {
                                          StatusId = x.StatusId,
                                          Description = x.Description,
                                          SortOrder = x.SortOrder

                                      }).ToList();
                    return StatusList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ClientWorkOrderNoteType> GetWorkOrderType(Guid ClientId)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var WorkOrderTypeList = (from C in DB.ClientWorkOrderNoteTypes
                                             where C.ActiveFlag == "Y" && C.Client == ClientId && C.Description != "System Generated"
                                             select new
                                             {
                                                 _WorkOrderNoteTypeId = C.WorkOrderNoteTypeId,
                                                 _Description = C.Description,
                                                 _Name = C.Name

                                             }).ToList().Select(x => new ClientWorkOrderNoteType()
                                             {
                                                 WorkOrderNoteTypeId = x._WorkOrderNoteTypeId,
                                                 Description = x._Description,
                                                 Name = x._Name

                                             }).ToList();
                    return WorkOrderTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ClientAttachmentType> GetWorkOrderAttachmentTypes(Guid ClientId, string item)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var WorkOrderAttachmentTypeList = (from C in DB.ClientAttachmentTypes
                                                       where C.ActiveFlag == "Y" && C.Client == ClientId && C.ServiceRequestOrWorkOrder == item
                                                       select new
                                                       {
                                                           _WorkOrderAttachmentTypeId = C.WorkOrderAttachmentTypeId,
                                                           _Name = C.Name,


                                                       }).ToList().Select(x => new ClientAttachmentType()
                                                       {
                                                           WorkOrderAttachmentTypeId = x._WorkOrderAttachmentTypeId,
                                                           Name = x._Name
                                                       }).ToList();
                    return WorkOrderAttachmentTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<ClientClassOfGood> GetGoodsClasses(Guid Clientid)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var VendorInvoiceTypeList = (from C in DB.ClientClassOfGoods
                                                 where C.ActiveFlag == "Y" && C.Client == Clientid
                                                 select new
                                                 {
                                                     _ClassOfGoodId = C.ClassOfGoodId,
                                                     _Name = C.Name,


                                                 }).ToList().Select(x => new ClientClassOfGood()
                                                 {
                                                     ClassOfGoodId = x._ClassOfGoodId,
                                                     Name = x._Name
                                                 }).ToList();
                    return VendorInvoiceTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Tuple<Guid, string>> SaveWorkOrderDetails(WorkOrderEntities workorderitem)
        {
            List<Tuple<Guid, string>> OperationStatus = new List<Tuple<Guid, string>>();
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    if (workorderitem == null)
                        throw new ArgumentNullException("item");

                    if (workorderitem.Client == Guid.Empty)
                        workorderitem.Client = DB.ClientUsers.Where(a => a.User == workorderitem.CreatedByUser).Select(a => a.Client).FirstOrDefault();

                    if (workorderitem.Vendor == Guid.Empty)
                        if (workorderitem.VendorName != null)
                            workorderitem.Vendor = DB.Vendors.Where(v => v.VendorName.Contains(workorderitem.VendorName)).Select(a => a.VendorId).FirstOrDefault();

                    if (workorderitem.Client != null)
                    {
                        WorkOrder WO = new WorkOrder();
                        if (workorderitem.WorkOrderId == Guid.Empty)
                        {
                            var ExistsVendor = DB.WorkOrders.Where(v => v.ServiceRequest == workorderitem.ServiceRequest && v.Vendor == workorderitem.Vendor).Count();
                            //this vendor should not be allowed to create another work order for the same service request...
                            if (ExistsVendor <= 0)
                            {
                                var WorkOrderNumber = GetWorkOrderNumber(workorderitem.Client, workorderitem.Servicetype);
                                if (WorkOrderNumber != null)
                                {
                                    var StatusId = DB.Statuses.Where(v => v.Description.Contains(workorderitem.Statusdescription)).Select(a => a.StatusId).FirstOrDefault();

                                    if (workorderitem.WorkOrderId == Guid.Empty)
                                        WO.WorkOrderId = Guid.NewGuid();

                                    WO.WorkOrderNumber = WorkOrderNumber[0].Number;
                                    WO.CreatedByUser = workorderitem.CreatedByUser;
                                    WO.DateCreated = DateTime.Now;
                                    WO.LastUpdatedByUser = workorderitem.LastUpdatedByUser;
                                    WO.DateLastUpdated = DateTime.Now;
                                    WO.ServiceRequest = workorderitem.ServiceRequest;
                                    WO.Vendor = workorderitem.Vendor;
                                    WO.Description = workorderitem.Description;
                                    WO.Status = StatusId;
                                    WO.DateArriveFrom = workorderitem.DateArriveFrom;
                                    WO.TimeArriveFrom = workorderitem.TimeArriveFrom;
                                    WO.DateArriveTo = workorderitem.DateArriveTo;
                                    WO.TimeArriveTo = workorderitem.TimeArriveTo;
                                    WO.NTE = workorderitem.NTE;
                                    WO.Client = workorderitem.Client;
                                    WO.WOCreateStatus = "N";
                                    WO.IsWorkOrderSent = false;
                                    WO.WorkOrderNumberPrefix = WorkOrderNumber[0].Prefix;
                                    WO.WorkOrderNumberSeqNumber = WorkOrderNumber[0].SeqNbr;
                                    DB.WorkOrders.Add(WO);
                                    DB.SaveChanges();
                                    var AddDataforAction = DB.prc_SaveWorkOrderActionDetails(WO.WorkOrderId, workorderitem.Status, workorderitem.CreatedByUser, workorderitem.Client, DateTime.Now, workorderitem.Description, "CreateWO");
                                    OperationStatus.Add(new Tuple<Guid, string>(WO.WorkOrderId, WO.WorkOrderNumber));
                                }
                            }
                            else
                            {
                                OperationStatus.Add(new Tuple<Guid, string>(Guid.Empty, "Please select another vendor"));
                            }
                        }
                        else
                        {
                            WO = DB.WorkOrders.Where(a => a.WorkOrderId == workorderitem.WorkOrderId).FirstOrDefault();
                            if (WO != null)
                            {
                                WO.WorkOrderNumber = workorderitem.WorkOrderNumber;
                                WO.CreatedByUser = workorderitem.CreatedByUser;
                                WO.DateCreated = DateTime.Now;
                                WO.LastUpdatedByUser = workorderitem.LastUpdatedByUser;
                                WO.DateLastUpdated = DateTime.Now;
                                WO.ServiceRequest = workorderitem.ServiceRequest;
                                WO.Vendor = workorderitem.Vendor;
                                WO.Description = workorderitem.Description;
                                WO.DateArriveFrom = workorderitem.DateArriveFrom;
                                WO.TimeArriveFrom = workorderitem.TimeArriveFrom;
                                WO.DateArriveTo = workorderitem.DateArriveTo;
                                WO.TimeArriveTo = workorderitem.TimeArriveTo;
                                WO.NTE = workorderitem.NTE;
                                WO.Client = workorderitem.Client;
                                WO.WOCreateStatus = "N";
                                DB.SaveChanges();
                                OperationStatus.Add(new Tuple<Guid, string>(WO.WorkOrderId, WO.WorkOrderNumber));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
            }
            return OperationStatus;
        }

        public bool UpdatePDFData(WorkOrderEntities item)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    WorkOrderAttachment woa = new WorkOrderAttachment();
                    if (item.WorkOrderAttachment == Guid.Empty || item.WorkOrderAttachment == null)
                    {
                        woa.WorkOrderAttachmentId = Guid.NewGuid();
                        woa.WorkOrder = item.WorkOrderId;
                        woa.Client = item.Client;
                        woa.WorkOrderAttachmentType = db.ClientAttachmentTypes.Where(a => a.Name == "WorkOrder").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                        woa.UploadedDate = DateTime.Now;
                        woa.FileName = item.WorkOrderNumber + ".pdf";
                        woa.FileType = db.CGSFileTypes.Where(a => a.Decription == "application/pdf").Select(x => x.CGSFileTypesId).FirstOrDefault();
                        woa.User = item.CreatedByUser;
                        woa.Notes = item.Description;
                        woa.Attachment = item.PDF;
                        woa.DisplaytoCustomer = "N";
                        db.WorkOrderAttachments.Add(woa);
                        db.SaveChanges();

                        var WO = db.WorkOrders.Where(a => a.WorkOrderId == item.WorkOrderId).FirstOrDefault();
                        if (WO != null)
                        {
                            WO.WorkOrderAttachment = woa.WorkOrderAttachmentId;
                            WO.DateCreated = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        woa = db.WorkOrderAttachments.Where(a => a.WorkOrderAttachmentId == item.WorkOrderAttachment).FirstOrDefault();
                        if (woa != null)
                        {
                            woa.Attachment = item.PDF;
                            woa.UploadedDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<prc_createNumber_Result> GetWorkOrderNumber(Guid ClientId, string Servicetype)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var WorOrderNumber = db.prc_createNumber("W", ClientId, Servicetype).ToList();
                    return WorOrderNumber;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveWorkOrderNotesDetails(WorkOrderNotesEntity item)
        {
            string OperationStatus = "Failure";
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    if (item == null)
                    {
                        throw new ArgumentNullException("item");
                    }
                    WorkOrderNote WO = new WorkOrderNote();
                    if (item.WorkOrderNotesId == Guid.Empty)
                    {
                        WO.WorkOrderNotesId = Guid.NewGuid();
                        WO.WorkOrderNotesType = item.WorkOrderNotesType;
                        WO.Client = item.ClientId;
                        WO.WorkOrder = item.WorkOrder;
                        WO.CreateDateTime = DateTime.Now;
                        WO.UpdateDatetime = DateTime.Now;
                        WO.User = item.User;
                        WO.Notes = item.Notes;
                        WO.DeleteFlag = "N";
                        DB.WorkOrderNotes.Add(WO);
                        DB.SaveChanges();
                        OperationStatus = "Success";
                    }
                    else
                    {
                        WO = DB.WorkOrderNotes.Where(a => a.WorkOrderNotesId == item.WorkOrderNotesId).FirstOrDefault();
                        if (WO != null)
                        {
                            WO.WorkOrderNotesType = item.WorkOrderNotesType;
                            WO.UpdateDatetime = DateTime.Now;
                            WO.Notes = item.Notes;
                            DB.SaveChanges();
                            OperationStatus = "Update Success";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
            }
            return OperationStatus;
        }

        public string SaveWorkOrderAttachments(WorkOrderAttachmentTypesEntity item)
        {
            string OperationStatus = "Failure";
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    if (item == null)
                    {
                        throw new ArgumentNullException("item");
                    }
                    WorkOrderAttachment WOA = new WorkOrderAttachment();
                    if (item.WorkOrderAttachmentId == Guid.Empty)
                    {
                        WOA.WorkOrderAttachmentId = Guid.NewGuid();
                        WOA.WorkOrder = item.WorkOrder;
                        WOA.Client = item.Client;
                        WOA.WorkOrderAttachmentType = item.WorkOrderAttachmentType;
                        WOA.UploadedDate = DateTime.Now;
                        WOA.FileName = item.FileName;
                        WOA.FileType = DB.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                        WOA.User = item.User;
                        WOA.Notes = item.Notes;
                        WOA.DisplaytoCustomer = item.DisplaytoCustomer;
                        WOA.Attachment = item.Attachment;
                        DB.WorkOrderAttachments.Add(WOA);
                        DB.SaveChanges();
                        OperationStatus = "Success";
                    }
                    else
                    {
                        WOA = DB.WorkOrderAttachments.Where(a => a.WorkOrderAttachmentId == item.WorkOrderAttachmentId).FirstOrDefault();
                        if (WOA != null)
                        {
                            WOA.WorkOrderAttachmentType = item.WorkOrderAttachmentType;
                            WOA.UploadedDate = DateTime.Now;
                            WOA.Notes = item.Notes;
                            WOA.FileName = item.FileName;
                            WOA.FileType = DB.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                            WOA.Attachment = item.Attachment;
                            WOA.DisplaytoCustomer = item.DisplaytoCustomer;
                            DB.SaveChanges();
                            OperationStatus = "Update Success";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
            }
            return OperationStatus;
        }

        public byte[] GetPDFData(Guid workorderid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    //var PDF = db.WorkOrders.Where(a => a.WorkOrderId == workorderid).Select(x => x.PDF).FirstOrDefault();
                    var PDF = db.WorkOrders.Join(db.WorkOrderAttachments, wo => wo.WorkOrderAttachment, woa => woa.WorkOrderAttachmentId, (wo, woa) => new { Attachment = woa.Attachment, wo.WorkOrderId }).Where(wo => wo.WorkOrderId == workorderid).FirstOrDefault().Attachment;
                    return PDF;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<WorkOrderEntities> GetWorkOrderRequest(Guid servicerequestId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var workorderlist = (from W in db.WorkOrders
                                         join S in db.ServiceRequests on W.ServiceRequest equals S.ServiceRequestId
                                         join V in db.Vendors on W.Vendor equals V.VendorId
                                         join Sa in db.Statuses on W.Status equals Sa.StatusId
                                         where W.ServiceRequest == servicerequestId
                                         orderby W.WorkOrderNumber descending
                                         select new
                                         {
                                             WorkOrderId = W.WorkOrderId,
                                             WorkOrderNumber = W.WorkOrderNumber,
                                             ServiceRequest = W.ServiceRequest,
                                             CustomerRefNumber = S.CustomerRefNumber,
                                             Vendor = W.Vendor,
                                             VendorName = V.VendorName,
                                             Email = V.Email,
                                             Status = W.Status,
                                             StatusDescription = Sa.Description,
                                             DateArriveFrom = W.DateArriveFrom,
                                             TimeArriveFrom = W.TimeArriveFrom,
                                             DateArriveTo = W.DateArriveTo,
                                             TimeArriveTo = W.TimeArriveTo,
                                             NTE = W.NTE,
                                             Description = W.Description,
                                             Client = W.Client,
                                             CreatedByUser = W.CreatedByUser,
                                             LastUpdatedByUser = W.LastUpdatedByUser,
                                             IsWorkOrderSent = W.IsWorkOrderSent,
                                             CompletionDate = W.CompletionDate,
                                             _WorkOrderNumberPrefix = W.WorkOrderNumberPrefix,
                                             _WorkOrderNumberSeqNumber = W.WorkOrderNumberSeqNumber,
                                             _WorkOrderAttachment = W.WorkOrderAttachment
                                             //WorkOrderId = W.WorkOrderId,
                                             //WorkOrderNumber = W.WorkOrderNumber,
                                             //CustomerRefNumber = S.CustomerRefNumber,
                                             //VendorName = V.VendorName,
                                             //Description = Sa.Description,
                                             //WorkDescription = W.Description,
                                             //NTE = W.NTE,                                                 
                                             //IsWorkOrderSent = W.IsWorkOrderSent,
                                             //CompletionDate = W.CompletionDate
                                         }).ToList().Select(W => new WorkOrderEntities()
                                         {
                                             WorkOrderId = W.WorkOrderId,
                                             WorkOrderNumber = W.WorkOrderNumber,
                                             ServiceRequest = W.ServiceRequest,
                                             Vendor = W.Vendor,
                                             VendorName = W.VendorName,
                                             Status = W.Status,
                                             DateArriveFrom = W.DateArriveFrom,
                                             TimeArriveFrom = W.TimeArriveFrom,
                                             DateArriveTo = W.DateArriveTo,
                                             TimeArriveTo = W.TimeArriveTo,
                                             Statusdescription = W.StatusDescription,
                                             NTE = W.NTE,
                                             Description = W.Description,
                                             Client = W.Client,
                                             CreatedByUser = W.CreatedByUser,
                                             LastUpdatedByUser = W.LastUpdatedByUser,
                                             IsWorkOrderSent = W.IsWorkOrderSent,
                                             Email = W.Email,
                                             CompletionDate = W.CompletionDate,
                                             Custref = W.CustomerRefNumber,
                                             Workorderprefix = W._WorkOrderNumberPrefix,
                                             Workorderseqnumber = W._WorkOrderNumberSeqNumber,
                                             WorkOrderAttachment = W._WorkOrderAttachment
                                             //WorkOrderId = x.WorkOrderId,
                                             //WorkOrderNumber = x.WorkOrderNumber,
                                             //NTE = x.NTE,
                                             //Statusdescription = x.Description,
                                             //VendorName = x.VendorName,
                                             //Description = x.WorkDescription,
                                             //Custref = x.CustomerRefNumber,
                                             //IsWorkOrderSent = x.IsWorkOrderSent,
                                             //CompletionDate = x.CompletionDate
                                         }).ToList();
                    return workorderlist;
                }

                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<WorkOrderEntities> GetWorkOrderControlsData(Guid workorderid)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var ClientList = (from W in DB.WorkOrders
                                      join S in DB.ServiceRequests on W.ServiceRequest equals S.ServiceRequestId
                                      join V in DB.Vendors on W.Vendor equals V.VendorId
                                      join Sa in DB.Statuses on W.Status equals Sa.StatusId
                                      join WA in DB.WorkOrderAttachments on W.WorkOrderId equals WA.WorkOrder
                                      where W.WorkOrderId == workorderid
                                      select new
                                      {
                                          WorkOrderId = W.WorkOrderId,
                                          WorkOrderNumber = W.WorkOrderNumber,
                                          ServiceRequest = W.ServiceRequest,
                                          Vendor = W.Vendor,
                                          VendorName = V.VendorName,
                                          Email = V.Email,
                                          Status = W.Status,
                                          StatusDescription = Sa.Description,
                                          DateArriveFrom = W.DateArriveFrom,
                                          TimeArriveFrom = W.TimeArriveFrom,
                                          DateArriveTo = W.DateArriveTo,
                                          TimeArriveTo = W.TimeArriveTo,
                                          NTE = W.NTE,
                                          Description = W.Description,
                                          Client = W.Client,
                                          CreatedByUser = W.CreatedByUser,
                                          LastUpdatedByUser = W.LastUpdatedByUser,
                                          IsWorkOrderSent = W.IsWorkOrderSent,
                                          PDF = WA.Attachment,
                                          FileType = DB.CGSFileTypes.Where(a => a.CGSFileTypesId == WA.FileType).Select(x => x.Decription).FirstOrDefault(),
                                          Workorderprefix = W.WorkOrderNumberPrefix,
                                          Workorderseqnumber = W.WorkOrderNumberSeqNumber,
                                          CompletionDate = W.CompletionDate,
                                          _WorkOrderAttachment = W.WorkOrderAttachment

                                      }).ToList().Select(W => new WorkOrderEntities()
                                      {
                                          WorkOrderId = W.WorkOrderId,
                                          WorkOrderNumber = W.WorkOrderNumber,
                                          ServiceRequest = W.ServiceRequest,
                                          Vendor = W.Vendor,
                                          VendorName = W.VendorName,
                                          Status = W.Status,
                                          DateArriveFrom = W.DateArriveFrom,
                                          TimeArriveFrom = W.TimeArriveFrom,
                                          DateArriveTo = W.DateArriveTo,
                                          TimeArriveTo = W.TimeArriveTo,
                                          Statusdescription = W.StatusDescription,
                                          NTE = W.NTE,
                                          Description = W.Description,
                                          Client = W.Client,
                                          CreatedByUser = W.CreatedByUser,
                                          LastUpdatedByUser = W.LastUpdatedByUser,
                                          IsWorkOrderSent = W.IsWorkOrderSent,
                                          PDF = W.PDF,
                                          FileType = W.FileType,
                                          Email = W.Email,
                                          Workorderprefix = W.Workorderprefix,
                                          Workorderseqnumber = W.Workorderseqnumber,
                                          CompletionDate = W.CompletionDate,
                                          WorkOrderAttachment = W._WorkOrderAttachment
                                      }).ToList();
                    return ClientList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public byte[] GetPDFDataWith(Guid workorderID)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    //var PDFData = db.WorkOrders.Where(a => a.WorkOrderId == workorderID).Select(a => a.PDF).FirstOrDefault();
                    var PDFData = db.WorkOrders.Join(db.WorkOrderAttachments, wo => wo.WorkOrderAttachment, woa => woa.WorkOrderAttachmentId, (wo, woa) => new { Attachment = woa.Attachment, wo.WorkOrderId }).Where(wo => wo.WorkOrderId == workorderID).FirstOrDefault().Attachment;

                    return PDFData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //------------------------------------------------------------Code Add By Neha for pdf Template functionality--------------------------------------------

        public IList<LogoDetails> ReadLogoAndConvertToByte(Guid ClientId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var LogoData = (from cl in db.Clients
                                    join cgtl in db.CGSThemeTemplateLogoes on cl.Logo equals cgtl.LogoID
                                    where cl.ClientId == ClientId
                                    select new
                                    {
                                        ClientId = cl.ClientId,
                                        LogoID = cgtl.LogoID,
                                        Logo = cgtl.Logo
                                    }).ToList().Select(W => new LogoDetails()
                                    {
                                        ClientId = W.ClientId,
                                        LogoID = W.LogoID,
                                        Logo = W.Logo
                                    }).ToList();
                    return LogoData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<fn_getClientResourceData_Result> DataforPDfHeader(Guid ClientId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var pdfHeaderData = db.fn_getClientResourceData(ClientId, "WorkorderHeader").ToList();

                    return pdfHeaderData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<TemplateTypeforPDF> GetTemplateType(Guid ClientId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var TemplateType = (from cl in db.Clients
                                        join cgs in db.CGSThemes on cl.CGSTheme equals cgs.CGSThemesId
                                        join CGST in db.CGSThemeTemplates on cl.CGSTheme equals CGST.CGSTheme
                                        join cgstt in db.CGSThemeTemplateTypes on CGST.TemplateType equals cgstt.TemplateTypeID
                                        where cl.ClientId == ClientId
                                        select new
                                        {
                                            ClientId = cl.ClientId,
                                            NameOfTheme = cgs.NameOfTheme,
                                            Attachment = CGST.Attachment,
                                            TemplateTypeID = cgstt.TemplateTypeID,
                                            TemplateName = cgstt.TemplateName
                                        }).ToList().Select(W => new TemplateTypeforPDF()
                                        {
                                            ClientId = W.ClientId,
                                            NameOfTheme = W.NameOfTheme,
                                            Attachment = W.Attachment,
                                            TemplateTypeID = W.TemplateTypeID,
                                            TemplateName = W.TemplateName
                                        }).ToList();
                    return TemplateType;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public byte[] GetCustomerinformation(Guid servicerequestID, Guid WorkOrderId, Guid ClientId)
        {
            byte[] byte3 = null;
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var CustomerInfo = (from wo in db.WorkOrders
                                        join sr in db.ServiceRequests on wo.ServiceRequest equals sr.ServiceRequestId
                                        join cl in db.CustomerLocations on sr.CustomerLocation equals cl.CustomerLocationId
                                        join cu in db.Customers on sr.Customer equals cu.CustomerId
                                        join v in db.Vendors on wo.Vendor equals v.VendorId
                                        join c in db.Clients on sr.Client equals c.ClientId
                                        join cgs in db.CGSThemes on c.CGSTheme equals cgs.CGSThemesId
                                        join CGST in db.CGSThemeTemplates on c.CGSTheme equals CGST.CGSTheme
                                        join Clc in db.ClientCustomers on sr.Customer equals Clc.Customer
                                        join u in db.Users on wo.CreatedByUser equals u.UserId
                                        join cgstt in db.CGSThemeTemplateTypes on CGST.TemplateType equals cgstt.TemplateTypeID
                                        where sr.ServiceRequestId == servicerequestID && wo.WorkOrderId == WorkOrderId && c.ClientId == ClientId
                                        select new
                                        {
                                            CustomerRefNumber = sr.CustomerRefNumber,
                                            WorkOrderId = wo.WorkOrderId,
                                            WorkOrderNumber = wo.WorkOrderNumber,
                                            Description = wo.Description,
                                            DateArriveFrom = wo.DateArriveFrom,
                                            TimeArriveFrom = wo.TimeArriveFrom,
                                            DateArriveTo = wo.DateArriveTo,
                                            TimeArriveTo = wo.TimeArriveTo,
                                            NTE = wo.NTE,
                                            CustomerName = cu.CustomerName,
                                            VendorName = v.VendorName,
                                            Email = v.Email,
                                            Address01 = cl.Address01,
                                            Address02 = cl.Address02,
                                            City = cl.City,
                                            State = cl.State,
                                            Zip01 = cl.Zip01,
                                            Telephone = cl.Telephone,
                                            NameOfTheme = cgs.NameOfTheme,
                                            ClientName = c.ClientName,
                                            Attachment = CGST.Attachment,
                                            IVRNumber = Clc.IVRNumber,
                                            FirstName = u.FirstName,
                                            LastName = u.LastName,
                                            TemplateTypeID = cgstt.TemplateTypeID,
                                            TemplateName = cgstt.TemplateName

                                        }).ToList().Select(W => new customerinformationforpdf()
                                        {
                                            CustomerRefNumber = W.CustomerRefNumber,
                                            WorkOrderId = W.WorkOrderId,
                                            WorkOrderNumber = W.WorkOrderNumber,
                                            Description = W.Description,
                                            DateArriveFrom = W.DateArriveFrom,
                                            TimeArriveFrom = W.TimeArriveFrom,
                                            DateArriveTo = W.DateArriveTo,
                                            TimeArriveTo = W.TimeArriveTo,
                                            NTE = W.NTE,
                                            CustomerName = W.CustomerName,
                                            VendorName = W.VendorName,
                                            Email = W.Email,
                                            Address01 = W.Address01,
                                            Address02 = W.Address02,
                                            City = W.City,
                                            State = W.State,
                                            Zip01 = W.Zip01,
                                            Telephone = W.Telephone,
                                            NameOfTheme = W.NameOfTheme,
                                            ClientName = W.ClientName,
                                            Attachment = W.Attachment,
                                            IVRNumber = W.IVRNumber,
                                            FirstName = W.FirstName,
                                            LastName = W.LastName,
                                            TemplateTypeID = W.TemplateTypeID,
                                            TemplateName = W.TemplateName
                                        }).ToList();

                    if (CustomerInfo.Count > 0)
                    {
                        var pdfHeaderData = DataforPDfHeader(ClientId);
                        var LogoData = ReadLogoAndConvertToByte(ClientId);
                        WorkOrderPDF WOPF = new WorkOrderPDF();
                        byte3 = WOPF.ReadPDFTemplate(CustomerInfo, pdfHeaderData, LogoData);
                        return byte3;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<AttachmentTypeData> ReadPDFTemplateForWOQuoteInvoice(Guid ClientID, string ModalStatus)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var AttachmentType = (from cl in db.Clients
                                      join cgt in db.CGSThemes on cl.CGSTheme equals cgt.CGSThemesId
                                      join ctt in db.CGSThemeTemplates on cgt.CGSThemesId equals ctt.CGSTheme
                                      join cttt in db.CGSThemeTemplateTypes on ctt.TemplateType equals cttt.TemplateTypeID
                                      where cl.ClientId == ClientID && cttt.TemplateName == ModalStatus
                                      select new
                                      {
                                          ClientId = cl.ClientId,
                                          TemplateName = cttt.TemplateName,
                                          TemplateTypeID = cttt.TemplateTypeID,
                                          Attachment = ctt.Attachment,
                                      }).ToList().Select(W => new AttachmentTypeData()
                                      {
                                          ClientId = W.ClientId,
                                          TemplateName = W.TemplateName,
                                          TemplateTypeID = W.TemplateTypeID,
                                          Attachment = W.Attachment
                                      }).ToList();
                return AttachmentType;

            }

        }

        public byte[] GetCustomerinformation1(Guid servicerequestID, Guid ClientId, List<ServiceInvoice> item, string InvoiceNumber, DateTime DateOfInvoice)
        {
            byte[] byte3 = null;
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var CustomerInfo = (from sr in db.ServiceRequests
                                        join cl in db.CustomerLocations on sr.CustomerLocation equals cl.CustomerLocationId
                                        join cu in db.Customers on sr.Customer equals cu.CustomerId
                                        join c in db.Clients on sr.Client equals c.ClientId
                                        where sr.ServiceRequestId == servicerequestID
                                        select new
                                        {
                                            CustomerRefNumber = sr.CustomerRefNumber,
                                            CustomerName = cu.CustomerName,
                                            Address01 = cl.Address01,
                                            Address02 = cl.Address02,
                                            City = cl.City,
                                            State = cl.State,
                                            Zip01 = cl.Zip01,
                                            Telephone = cl.Telephone
                                        }).ToList().Select(W => new customerinformationforpdf()
                                        {
                                            CustomerRefNumber = W.CustomerRefNumber,
                                            CustomerName = W.CustomerName,
                                            Address01 = W.Address01,
                                            Address02 = W.Address02,
                                            City = W.City,
                                            State = W.State,
                                            Zip01 = W.Zip01,
                                            Telephone = W.Telephone
                                        }).ToList();


                    if (CustomerInfo.Count > 0)
                    {
                        List<AttachmentTypeData> atd = ReadPDFTemplateForWOQuoteInvoice(ClientId, "Quote/Invoice");
                        var pdfHeaderData = DataforPDfHeader(ClientId);
                        var LogoData = ReadLogoAndConvertToByte(ClientId);
                        WorkOrderPDF WOPF = new WorkOrderPDF();
                        byte3 = WOPF.ReadQuotePDFTemplate(CustomerInfo, pdfHeaderData, atd, item, InvoiceNumber, DateOfInvoice, LogoData);
                        return byte3;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<customerinformationforpdf> GetCustomerinformation2(Guid servicerequestID)
        {

            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var CustomerInfo = (from sr in db.ServiceRequests
                                        join cl in db.CustomerLocations on sr.CustomerLocation equals cl.CustomerLocationId
                                        join cu in db.Customers on sr.Customer equals cu.CustomerId
                                        //join v in db.Vendors on wo.Vendor equals v.VendorId
                                        join c in db.Clients on sr.Client equals c.ClientId
                                        //join cgs in db.CGSThemes on c.CGSTheme equals cgs.CGSThemesId
                                        //join CGST in db.CGSThemeTemplates on c.CGSTheme equals CGST.CGSTheme
                                        where sr.ServiceRequestId == servicerequestID
                                        select new
                                        {
                                            CustomerRefNumber = sr.CustomerRefNumber,
                                            CustomerName = cu.CustomerName,
                                            //VendorName = v.VendorName,
                                            //Email = v.Email,
                                            Address01 = cl.Address01,
                                            Address02 = cl.Address02,
                                            City = cl.City,
                                            State = cl.State,
                                            Zip01 = cl.Zip01,
                                            Telephone = cl.Telephone,
                                            //NameOfTheme = cgs.NameOfTheme,
                                            ClientName = c.ClientName
                                            //Attachment = CGST.Attachment
                                        }).ToList().Select(W => new customerinformationforpdf()
                                        {
                                            CustomerRefNumber = W.CustomerRefNumber,
                                            CustomerName = W.CustomerName,
                                            //VendorName = W.VendorName,
                                            //Email = W.Email,
                                            Address01 = W.Address01,
                                            Address02 = W.Address02,
                                            City = W.City,
                                            State = W.State,
                                            Zip01 = W.Zip01,
                                            Telephone = W.Telephone,
                                            //NameOfTheme = W.NameOfTheme,
                                            ClientName = W.ClientName
                                            //Attachment = W.Attachment
                                        }).ToList();


                    return CustomerInfo;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //private void ReadPDFTemplate(List<customerinformationforpdf> listData)
        //{
        //   // WorkOrderPDF wopf = new WorkOrderPDF();
        //    string formFile = @"D:\Client Projects\PDF Templates\" + listData[0].NameOfTheme.Trim() + ".pdf";

        //    PdfReader reader = new PdfReader(formFile);
        //    BindDataToPDFTemplate(listData, reader);

        //}

        //private MemoryStream BindDataToPDFTemplate(List<customerinformationforpdf> listdata, PdfReader readPath)
        //{
        //    using (MemoryStream memStream = new MemoryStream())
        //    {
        //        using (PdfStamper stamper = new PdfStamper(readPath, memStream, '\0', true))
        //        {
        //            var form = stamper.AcroFields;
        //            var fieldKeys = form.Fields.Keys;

        //            foreach (string fieldKey in fieldKeys)
        //            {
        //                if (fieldKey == "workOrderNumber_1")
        //                    form.SetField(fieldKey, listdata[0].WorkOrderNumber);
        //                if (fieldKey == "currentDate_1")
        //                    form.SetField(fieldKey, Convert.ToString(DateTime.Now.ToString("dd/MM/yyy")));
        //                if (fieldKey == "currentDate_2")
        //                    form.SetField(fieldKey, Convert.ToString(DateTime.Now.ToString("dd/MM/yyy")));
        //                if (fieldKey == "clientName")
        //                    form.SetField(fieldKey, listdata[0].ClientName);
        //                if (fieldKey == "workOrderNumber_2")
        //                    form.SetField(fieldKey, listdata[0].WorkOrderNumber);
        //                if (fieldKey == "netAmount")
        //                    form.SetField(fieldKey, Convert.ToString("$" + listdata[0].NTE));
        //                if (fieldKey == "customerRefrenceNumber_1")
        //                    form.SetField(fieldKey, listdata[0].CustomerRefNumber);
        //                if (fieldKey == "customerName_1")
        //                    form.SetField(fieldKey, listdata[0].CustomerName);
        //                if (fieldKey == "telephone_1")
        //                    form.SetField(fieldKey, listdata[0].Telephone);
        //                if (fieldKey == "address_1")
        //                    form.SetField(fieldKey, listdata[0].Address01);
        //                if (fieldKey == "address_3")
        //                    form.SetField(fieldKey, listdata[0].City + " " + listdata[0].State + " " + listdata[0].Zip01);
        //                //if (fieldKey == "contact")
        //                //    form.SetField(fieldKey, "contact");
        //                if (fieldKey == "siteinfo")
        //                    form.SetField(fieldKey, "siteinfo");
        //                if (fieldKey == "dateArriveFrom")
        //                    form.SetField(fieldKey, Convert.ToString(listdata[0].DateArriveFrom));
        //                if (fieldKey == "timeArriveFrom")
        //                    form.SetField(fieldKey, listdata[0].TimeArriveFrom);
        //                if (fieldKey == "dateArriveTo")
        //                    form.SetField(fieldKey, Convert.ToString(listdata[0].DateArriveTo));
        //                if (fieldKey == "timeArriveTo")
        //                    form.SetField(fieldKey, listdata[0].TimeArriveTo);
        //                if (fieldKey == "description")
        //                    form.SetField(fieldKey, listdata[0].Description);
        //                if (fieldKey == "workOrderNumber_3")
        //                    form.SetField(fieldKey, listdata[0].WorkOrderNumber);
        //                if (fieldKey == "currentDate_3")
        //                    form.SetField(fieldKey, Convert.ToString(DateTime.Now.ToString("dd/MM/yyy")));
        //                if (fieldKey == "workOrderNumber_4")
        //                    form.SetField(fieldKey, listdata[0].WorkOrderNumber);
        //                if (fieldKey == "customerName_2")
        //                    form.SetField(fieldKey, listdata[0].CustomerName);
        //                if (fieldKey == "telephone_2")
        //                    form.SetField(fieldKey, listdata[0].Telephone);
        //                if (fieldKey == "address_2")
        //                    form.SetField(fieldKey, listdata[0].Address01 + " " + listdata[0].City + " " + listdata[0].State + " " + listdata[0].Zip01);
        //                if (fieldKey == "customerRefrenceNumber_2")
        //                    form.SetField(fieldKey, listdata[0].CustomerRefNumber);
        //            }

        //            // "Flatten" the form so it wont be editable/usable anymore
        //            stamper.FormFlattening = false;
        //            form.GenerateAppearances = false;

        //            // You can also specify fields to be flattened, which
        //            // leaves the rest of the form still be editable/usable
        //            //stamper.PartialFormFlattening("field1");

        //            stamper.Close();
        //        }

        //        ConvertByteToMemory(memStream, listdata);
        //        return memStream;
        //    }
        //}

        //public void ConvertByteToMemory(MemoryStream mem, List<customerinformationforpdf> data)
        //{
        //    byte[] bytes = mem.ToArray();
        //    try
        //    {
        //        using (FacilitiesEntities db = new FacilitiesEntities())
        //        {
        //            var workorder = db.WorkOrderAttachments.ToList();//first row to be updated
        //            workorder[0].Attachment = bytes;
        //            workorder[0].WorkOrder = data[0].WorkOrderId;
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //}

        //------------------------------------------------------------Code End By Neha for pdf Template functionality--------------------------------------------
        public IEnumerable<WorkOrderEntities> GetVendorAdminWorkOrderRequest(Guid UserID)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var VendorId = db.VendorUsers.Where(a => a.User == UserID).Select(a => a.Vendor).FirstOrDefault();
                    if (VendorId != Guid.Empty)
                    {
                        var workorderlist = (from W in db.WorkOrders
                                             join S in db.ServiceRequests on W.ServiceRequest equals S.ServiceRequestId
                                             join C in db.Clients on W.Client equals C.ClientId
                                             join CR in db.Customers on S.Customer equals CR.CustomerId
                                             join CL in db.CustomerLocations on S.CustomerLocation equals CL.CustomerLocationId
                                             join V in db.Vendors on W.Vendor equals V.VendorId
                                             join Sa in db.Statuses on W.Status equals Sa.StatusId
                                             where W.Vendor == VendorId && W.IsWorkOrderSent == true
                                             select new
                                             {
                                                 WorkOrderId = W.WorkOrderId,
                                                 WorkOrderNumber = W.WorkOrderNumber,
                                                 CustomerRefNumber = S.CustomerRefNumber,
                                                 VendorName = V.VendorName,
                                                 Description = Sa.Description,
                                                 ClientName = C.ClientName,
                                                 CustomerName = CR.CustomerName,
                                                 Address01 = CL.Address01,
                                                 City = CL.City,
                                                 State = CL.State,
                                                 Zip01 = CL.Zip01,
                                                 Telephone = CL.Telephone,
                                                 WorkDescription = W.Description,
                                                 NTE = W.NTE,
                                                 CompletionDate = W.CompletionDate,
                                                 //PDF = W.PDF,
                                                 //FileName = W.FileName,
                                                 IsWorkOrderSent = W.IsWorkOrderSent

                                             }).ToList().Select(x => new WorkOrderEntities()
                                             {
                                                 WorkOrderId = x.WorkOrderId,
                                                 WorkOrderNumber = x.WorkOrderNumber,
                                                 NTE = x.NTE,
                                                 Statusdescription = x.Description,
                                                 VendorName = x.VendorName,
                                                 Description = x.WorkDescription,
                                                 Custref = x.CustomerRefNumber,
                                                 IsWorkOrderSent = x.IsWorkOrderSent,
                                                 ClientName = x.ClientName,
                                                 CustomerName = x.CustomerName,
                                                 CustomerAddress = x.Address01 + " " + x.State + " " + x.City + " " + x.Zip01,
                                                 CustomerPhone = x.Telephone,
                                                 CompletionDate = x.CompletionDate
                                                 //PDF = x.PDF,                                                 
                                             }).ToList();
                        return workorderlist;
                    }
                    return null;
                }

                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<WorkOrderNotesEntity> GetWorkOrderNoteGridData(Guid WorkOrderId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var workordernotelist = (from WN in db.WorkOrderNotes
                                             join CWT in db.ClientWorkOrderNoteTypes on WN.WorkOrderNotesType equals CWT.WorkOrderNoteTypeId
                                             //join W in db.WorkOrders on WN.WorkOrder equals W.WorkOrderId
                                             join C in db.Clients on WN.Client equals C.ClientId
                                             join U in db.Users on WN.User equals U.UserId
                                             where WN.WorkOrder == WorkOrderId
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
                    return workordernotelist;
                }

                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<Get_ServiceWorkOrderNoteGridData_Result> GetServiceWorkOrderNoteGridData(Guid WorkOrderId, Guid ServiceRequestId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var ServiceWorkOrderNoteData = db.Get_ServiceWorkOrderNoteGridData(WorkOrderId, ServiceRequestId).ToList();
                    return ServiceWorkOrderNoteData;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<WorkOrderAttachmentTypesEntity> GetWorkOrderAttachmentsGridData(Guid WorkOrdeId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var workordernotelist = (from WN in db.WorkOrderAttachments
                                             join CWT in db.ClientAttachmentTypes on WN.WorkOrderAttachmentType equals CWT.WorkOrderAttachmentTypeId
                                             join C in db.Clients on WN.Client equals C.ClientId
                                             join U in db.Users on WN.User equals U.UserId
                                             where WN.WorkOrder == WorkOrdeId
                                             orderby WN.UploadedDate descending
                                             select new
                                             {
                                                 _WorkOrderAttachmentId = WN.WorkOrderAttachmentId,
                                                 _WorkOrderAttachmentType = WN.WorkOrderAttachmentType,
                                                 _UploadedDate = WN.UploadedDate,
                                                 _Notes = WN.Notes,
                                                 _Description = CWT.Name,
                                                 _ClientName = C.ClientName,
                                                 _Email = U.Email,
                                                 _WorkOrder = WN.WorkOrder,
                                                 _DisplaytoCustomer = WN.DisplaytoCustomer,
                                                 _FileName = WN.FileName

                                             }).ToList().Select(x => new WorkOrderAttachmentTypesEntity()
                                             {
                                                 WorkOrderAttachmentId = x._WorkOrderAttachmentId,
                                                 WorkOrderAttachmentType = x._WorkOrderAttachmentType,
                                                 UploadedDate = x._UploadedDate,
                                                 Notes = x._Notes,
                                                 TypeDescription = x._Description,
                                                 ClientName = x._ClientName,
                                                 UserName = x._Email,
                                                 WorkOrder = x._WorkOrder,
                                                 DisplaytoCustomer = x._DisplaytoCustomer,
                                                 FileName = x._FileName
                                             }).ToList();
                    return workordernotelist;
                }

                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IList<prc_GetAttachment_Result> GetAttachmentFileData(Guid AttachmentId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var GetAttachment = db.prc_GetAttachment(AttachmentId, "workorder").ToList();
                    return GetAttachment;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VendorInvoiceEntity GetVendorInvoiceGridData(Guid workorderID, Guid HeaderId)
        {
            //List<VendorInvoiceEntity> Vendor = new List<VendorInvoiceEntity>();
            VendorInvoiceEntity Vendor = new VendorInvoiceEntity();
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var VendorHeader = (from wo in db.WorkOrders
                                        join woh in db.WorkOrderVendorInvoiceHeaders on wo.WorkOrderId equals woh.WorkOrder
                                        where wo.WorkOrderId == workorderID && woh.VendorInvoiceHeaderId == HeaderId
                                        select new
                                        {
                                            _VendorInvoiceHeaderId = woh.VendorInvoiceHeaderId,
                                            _DateOfInvoice = woh.DateOfInvoice,
                                            _VendorInvoiceNumber = woh.VendorInvoiceNumber,
                                            _InvoiceOrQuote = woh.InvoiceOrQuote,
                                            _Description = woh.Description

                                        }).ToList().Select(w => new VendorInvoiceEntity()
                                        {
                                            VendorInvoiceHeaderId = w._VendorInvoiceHeaderId,
                                            DateOfInvoice = w._DateOfInvoice,
                                            VendorInvoiceNumber = w._VendorInvoiceNumber,
                                            Description = w._Description,
                                            InvoiceOrQuote = w._InvoiceOrQuote
                                        }).ToList();

                    Vendor.VendorInvoiceHeaderId = VendorHeader[0].VendorInvoiceHeaderId;
                    Vendor.DateOfInvoice = VendorHeader[0].DateOfInvoice;
                    Vendor.VendorInvoiceNumber = VendorHeader[0].VendorInvoiceNumber;
                    Vendor.Description = VendorHeader[0].Description;
                    Vendor.InvoiceOrQuote = VendorHeader[0].InvoiceOrQuote;

                    var GetVendorInvoice = (from wod in db.WorkOrderVendorInvoiceDetails
                                            join cg in db.ClientClassOfGoods on wod.ClassOfGoodId equals cg.ClassOfGoodId
                                            where wod.VendorInvoiceHeader == Vendor.VendorInvoiceHeaderId && cg.ActiveFlag == "Y"
                                            select new
                                            {
                                                _ClassOfGoodId = cg.ClassOfGoodId,
                                                _Name = cg.Name,
                                                _Amount = wod.Amount,
                                                _Notes = wod.Notes,
                                                _Tax = wod.Tax,
                                                _Total = wod.Total
                                            }).ToList().Select(x => new ClassOfGood()
                                            {
                                                ClassOfGoodId = x._ClassOfGoodId,
                                                Name = x._Name,
                                                Amount = x._Amount,
                                                Notes = x._Notes,
                                                Tax = x._Tax,
                                                TotalAmountAfterTax = x._Total,
                                            }).ToList();

                    Vendor.ClassOfGood = GetVendorInvoice;

                    return Vendor;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public int CheckInvoiceExitsCount(Guid workorderid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var InvoiceCount = db.WorkOrderVendorInvoiceHeaders.Where(a => a.WorkOrder == workorderid && a.InvoiceOrQuote == "I").ToList();

                return InvoiceCount.Count;
            }
        }

        public bool SaveVendorInvoiceDetails(VendorInvoiceEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    WorkOrderVendorInvoiceHeader woh = new WorkOrderVendorInvoiceHeader();
                    var vendorId = db.WorkOrders.Where(a => a.WorkOrderId == item.WorkOrder).Select(x => x.Vendor).FirstOrDefault();

                    if (item.VendorInvoiceHeaderId == Guid.Empty)
                    {
                        if (item.InvoiceOrQuote == "Q")
                        {
                            if (item.Attachment.Length > 0)
                            {
                                var AttachmentId = db.ClientAttachmentTypes.Where(a => a.Name == "Vendor Quote").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                                WorkOrderAttachment WOA = new WorkOrderAttachment();
                                WOA.WorkOrderAttachmentId = Guid.NewGuid();
                                WOA.WorkOrder = item.WorkOrder;
                                WOA.Client = item.Client;
                                WOA.WorkOrderAttachmentType = AttachmentId;
                                WOA.UploadedDate = DateTime.Now;
                                WOA.FileName = item.FileName;
                                WOA.FileType = db.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                                WOA.User = item.User;
                                WOA.Notes = item.Description;
                                WOA.DisplaytoCustomer = "N";
                                WOA.Attachment = item.Attachment;
                                db.WorkOrderAttachments.Add(WOA);
                                //db.SaveChanges();

                                woh.VendorInvoiceHeaderId = Guid.NewGuid();
                                woh.WorkOrder = item.WorkOrder;
                                woh.Client = item.Client;
                                woh.Vendor = vendorId;
                                woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                woh.DateOfInvoice = item.DateOfInvoice;
                                woh.DateUpdated = DateTime.Now;
                                woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                woh.User = item.User;
                                woh.Description = item.Description;
                                woh.WorkOrderAttachment = WOA.WorkOrderAttachmentId;
                                db.WorkOrderVendorInvoiceHeaders.Add(woh);

                                db.SaveChanges();
                            }
                            else
                            {
                                woh.VendorInvoiceHeaderId = Guid.NewGuid();
                                woh.WorkOrder = item.WorkOrder;
                                woh.Client = item.Client;
                                woh.Vendor = vendorId;
                                woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                woh.DateOfInvoice = item.DateOfInvoice;
                                woh.DateUpdated = DateTime.Now;
                                woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                woh.User = item.User;
                                woh.Description = item.Description;
                                db.WorkOrderVendorInvoiceHeaders.Add(woh);
                                db.SaveChanges();
                            }

                            foreach (ClassOfGood value in item.ClassOfGood)
                            {
                                //if (value.TotalAmountAfterTax > 0)
                                //{
                                WorkOrderVendorInvoiceDetail wod = new WorkOrderVendorInvoiceDetail();
                                wod.VendorInvoiceDetailId = Guid.NewGuid();
                                wod.VendorInvoiceHeader = woh.VendorInvoiceHeaderId;
                                wod.ClassOfGoodId = value.ClassOfGoodId;
                                wod.Notes = value.Notes;
                                wod.Amount = value.Amount;
                                wod.Tax = value.Tax;
                                wod.Total = value.TotalAmountAfterTax;
                                db.WorkOrderVendorInvoiceDetails.Add(wod);
                                db.SaveChanges();
                                //}
                            }
                            var StatusData = db.prc_SaveWorkOrderActionDetails(item.WorkOrder, Guid.Empty, item.User, item.Client, DateTime.Now, item.Description, "ReceivedVQ");
                        }
                        else
                        {
                            if (item.Attachment.Length > 0)
                            {
                                var AttachmentId = db.ClientAttachmentTypes.Where(a => a.Name == "Vendor Invoice").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                                WorkOrderAttachment WOA = new WorkOrderAttachment();
                                WOA.WorkOrderAttachmentId = Guid.NewGuid();
                                WOA.WorkOrder = item.WorkOrder;
                                WOA.Client = item.Client;
                                WOA.WorkOrderAttachmentType = AttachmentId;
                                WOA.UploadedDate = DateTime.Now;
                                WOA.FileName = item.FileName;
                                WOA.FileType = db.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                                WOA.User = item.User;
                                WOA.Notes = item.Description;
                                WOA.DisplaytoCustomer = "N";
                                WOA.Attachment = item.Attachment;
                                db.WorkOrderAttachments.Add(WOA);
                                //db.SaveChanges();

                                woh.VendorInvoiceHeaderId = Guid.NewGuid();
                                woh.WorkOrder = item.WorkOrder;
                                woh.Client = item.Client;
                                woh.Vendor = vendorId;
                                woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                woh.DateOfInvoice = item.DateOfInvoice;
                                woh.DateUpdated = DateTime.Now;
                                woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                woh.User = item.User;
                                woh.Description = item.Description;
                                woh.WorkOrderAttachment = WOA.WorkOrderAttachmentId;
                                db.WorkOrderVendorInvoiceHeaders.Add(woh);

                                db.SaveChanges();
                            }
                            else
                            {
                                woh.VendorInvoiceHeaderId = Guid.NewGuid();
                                woh.WorkOrder = item.WorkOrder;
                                woh.Client = item.Client;
                                woh.Vendor = vendorId;
                                woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                woh.DateOfInvoice = item.DateOfInvoice;
                                woh.DateUpdated = DateTime.Now;
                                woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                woh.User = item.User;
                                woh.Description = item.Description;
                                db.WorkOrderVendorInvoiceHeaders.Add(woh);
                                db.SaveChanges();
                            }

                            foreach (ClassOfGood value in item.ClassOfGood)
                            {
                                WorkOrderVendorInvoiceDetail wod = new WorkOrderVendorInvoiceDetail();
                                wod.VendorInvoiceDetailId = Guid.NewGuid();
                                wod.VendorInvoiceHeader = woh.VendorInvoiceHeaderId;
                                wod.ClassOfGoodId = value.ClassOfGoodId;
                                wod.Notes = value.Notes;
                                wod.Amount = value.Amount;
                                wod.Tax = value.Tax;
                                wod.Total = value.TotalAmountAfterTax;
                                db.WorkOrderVendorInvoiceDetails.Add(wod);
                                db.SaveChanges();
                            }

                            var StatusData = db.prc_SaveWorkOrderActionDetails(item.WorkOrder, Guid.Empty, item.User, item.Client, DateTime.Now, item.Description, "ReceivedVInv");
                        }
                    }
                    else
                    {
                        if (item.InvoiceOrQuote == "Q")
                        {
                            if (item.Attachment.Length > 0)
                            {
                                woh = db.WorkOrderVendorInvoiceHeaders.Where(a => a.VendorInvoiceHeaderId == item.VendorInvoiceHeaderId).FirstOrDefault();
                                if (woh.WorkOrderAttachment != null)
                                {
                                    // Delete records in WorkOrderVendorInvoiceDetail table
                                    var allRec = db.WorkOrderAttachments.Where(a => a.WorkOrderAttachmentId == woh.WorkOrderAttachment);
                                    db.WorkOrderAttachments.RemoveRange(allRec);
                                    db.SaveChanges();
                                }

                                var AttachmentId = db.ClientAttachmentTypes.Where(a => a.Name == "Vendor Quote").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                                WorkOrderAttachment WOA = new WorkOrderAttachment();
                                WOA.WorkOrderAttachmentId = Guid.NewGuid();
                                WOA.WorkOrder = item.WorkOrder;
                                WOA.Client = item.Client;
                                WOA.WorkOrderAttachmentType = AttachmentId;
                                WOA.UploadedDate = DateTime.Now;
                                WOA.FileName = item.FileName;
                                WOA.FileType = db.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                                WOA.User = item.User;
                                WOA.Notes = item.Description;
                                WOA.DisplaytoCustomer = "N";
                                WOA.Attachment = item.Attachment;
                                db.WorkOrderAttachments.Add(WOA);

                                woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                woh.DateOfInvoice = item.DateOfInvoice;
                                woh.DateUpdated = item.DateUpdated;
                                woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                woh.Description = item.Description;
                                woh.WorkOrderAttachment = WOA.WorkOrderAttachmentId;

                                // Delete all records in WorkOrderVendorInvoiceDetail table
                                var allRec1 = db.WorkOrderVendorInvoiceDetails.Where(a => a.VendorInvoiceHeader == item.VendorInvoiceHeaderId);
                                db.WorkOrderVendorInvoiceDetails.RemoveRange(allRec1);
                                db.SaveChanges();


                            }
                            else
                            {
                                woh = db.WorkOrderVendorInvoiceHeaders.Where(a => a.VendorInvoiceHeaderId == item.VendorInvoiceHeaderId).FirstOrDefault();
                                if (woh != null)
                                {
                                    woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                    woh.DateOfInvoice = item.DateOfInvoice;
                                    woh.DateUpdated = item.DateUpdated;
                                    woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                    woh.Description = item.Description;
                                    // Delete all records in WorkOrderVendorInvoiceDetail table
                                    var allRec = db.WorkOrderVendorInvoiceDetails.Where(a => a.VendorInvoiceHeader == item.VendorInvoiceHeaderId);
                                    db.WorkOrderVendorInvoiceDetails.RemoveRange(allRec);
                                    db.SaveChanges();
                                }
                            }
                            foreach (ClassOfGood value in item.ClassOfGood)
                            {
                                WorkOrderVendorInvoiceDetail wod = new WorkOrderVendorInvoiceDetail();
                                wod.VendorInvoiceDetailId = Guid.NewGuid();
                                wod.VendorInvoiceHeader = woh.VendorInvoiceHeaderId;
                                wod.ClassOfGoodId = value.ClassOfGoodId;
                                wod.Notes = value.Notes;
                                wod.Amount = value.Amount;
                                wod.Tax = value.Tax;
                                wod.Total = value.TotalAmountAfterTax;
                                db.WorkOrderVendorInvoiceDetails.Add(wod);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            if (item.Attachment.Length > 0)
                            {
                                woh = db.WorkOrderVendorInvoiceHeaders.Where(a => a.VendorInvoiceHeaderId == item.VendorInvoiceHeaderId).FirstOrDefault();
                                if (woh.WorkOrderAttachment != null)
                                {
                                    // Delete records in WorkOrderVendorInvoiceDetail table
                                    var allRec = db.WorkOrderAttachments.Where(a => a.WorkOrderAttachmentId == woh.WorkOrderAttachment);
                                    db.WorkOrderAttachments.RemoveRange(allRec);
                                    db.SaveChanges();
                                }

                                var AttachmentId = db.ClientAttachmentTypes.Where(a => a.Name == "Vendor Invoice").Select(x => x.WorkOrderAttachmentTypeId).FirstOrDefault();
                                WorkOrderAttachment WOA = new WorkOrderAttachment();
                                WOA.WorkOrderAttachmentId = Guid.NewGuid();
                                WOA.WorkOrder = item.WorkOrder;
                                WOA.Client = item.Client;
                                WOA.WorkOrderAttachmentType = AttachmentId;
                                WOA.UploadedDate = DateTime.Now;
                                WOA.FileName = item.FileName;
                                WOA.FileType = db.CGSFileTypes.Where(a => a.Decription == item.FileType).Select(x => x.CGSFileTypesId).FirstOrDefault();
                                WOA.User = item.User;
                                WOA.Notes = item.Description;
                                WOA.DisplaytoCustomer = "N";
                                WOA.Attachment = item.Attachment;
                                db.WorkOrderAttachments.Add(WOA);

                                woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                woh.DateOfInvoice = item.DateOfInvoice;
                                woh.DateUpdated = item.DateUpdated;
                                woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                woh.Description = item.Description;
                                woh.WorkOrderAttachment = WOA.WorkOrderAttachmentId;

                                // Delete all records in WorkOrderVendorInvoiceDetail table
                                var allRec1 = db.WorkOrderVendorInvoiceDetails.Where(a => a.VendorInvoiceHeader == item.VendorInvoiceHeaderId);
                                db.WorkOrderVendorInvoiceDetails.RemoveRange(allRec1);
                                db.SaveChanges();


                            }
                            else
                            {
                                woh = db.WorkOrderVendorInvoiceHeaders.Where(a => a.VendorInvoiceHeaderId == item.VendorInvoiceHeaderId).FirstOrDefault();
                                if (woh != null)
                                {
                                    woh.VendorInvoiceNumber = item.VendorInvoiceNumber;
                                    woh.DateOfInvoice = item.DateOfInvoice;
                                    woh.DateUpdated = item.DateUpdated;
                                    woh.InvoiceOrQuote = item.InvoiceOrQuote;
                                    woh.Description = item.Description;
                                    // Delete all records in WorkOrderVendorInvoiceDetail table
                                    var allRec = db.WorkOrderVendorInvoiceDetails.Where(a => a.VendorInvoiceHeader == item.VendorInvoiceHeaderId);
                                    db.WorkOrderVendorInvoiceDetails.RemoveRange(allRec);
                                    db.SaveChanges();
                                }
                            }
                            foreach (ClassOfGood value in item.ClassOfGood)
                            {
                                WorkOrderVendorInvoiceDetail wod = new WorkOrderVendorInvoiceDetail();
                                wod.VendorInvoiceDetailId = Guid.NewGuid();
                                wod.VendorInvoiceHeader = woh.VendorInvoiceHeaderId;
                                wod.ClassOfGoodId = value.ClassOfGoodId;
                                wod.Notes = value.Notes;
                                wod.Amount = value.Amount;
                                wod.Tax = value.Tax;
                                wod.Total = value.TotalAmountAfterTax;
                                db.WorkOrderVendorInvoiceDetails.Add(wod);
                                db.SaveChanges();
                            }
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

        public bool SaveWorkOrderAction(WorkOrderActionEntity item)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var WorkOrder = db.prc_SaveWorkOrderActionDetails(item.WorkOrderId, item.Status, item.Userid, item.ClientId, item.CompletionDate, item.Notes, item.ActionCode);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public IList<WorkOrderCode> GetActionCode(Guid ClientId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var ActionCode = (from ca in db.ClientWorkOrderActions
                                      join cg in db.CGSActions on ca.CGSAction equals cg.CGSActionId
                                      where ca.DisplayInActionFlag == "Y" && ca.ActiveFlag == "Y" && cg.ActiveFlag == "Y" && ca.Client == ClientId
                                      select new
                                      {
                                          _ActionID = ca.ActionID,
                                          _ActionName = ca.ActionName,
                                          _Action = ca.Action,
                                          _DefaultNotesType = ca.DefaultNotesType,
                                          _ActionCode = cg.ActionCode
                                      }).ToList().Select(w => new WorkOrderCode()
                                      {
                                          ActionName = w._ActionName,
                                          Action = w._Action,
                                          NotesTypeId = w._DefaultNotesType,
                                          ActionCode = w._ActionCode
                                      }).ToList();
                    return ActionCode;
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<Tuple<byte[], string>> GetAttachmentData(Guid value)
        {
            List<Tuple<byte[], string>> Values = new List<Tuple<byte[], string>>();

            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var DocData = (from wa in db.WorkOrderAttachments
                                   where wa.WorkOrderAttachmentId == value
                                   select new
                                   {
                                       _Attachment = wa.Attachment,
                                       _FileName = wa.FileName
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

        public IEnumerable<prc_GetAttachment_Result> GetAttachemntData(Guid AttachmentId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var AttachmentData = db.prc_GetAttachment(AttachmentId, "workorder").ToList();
                    return AttachmentData;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<prc_GetWorkOrderVendorInvoiceData_Result> GetWorkOrderInvoiceData(Guid workorderid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var InvoiceList = db.prc_GetWorkOrderVendorInvoiceData(workorderid).ToList();
                    return InvoiceList;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public string SendEmailtoVendor(Guid workorderid, string FileName, string Notes)
        {
            string OperationStatus = "Failure";
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    if (workorderid == null)
                    {
                        throw new ArgumentNullException("item");
                    }
                    WorkOrder WO = new WorkOrder();
                    WO = db.WorkOrders.Where(a => a.WorkOrderId == workorderid).FirstOrDefault();
                    if (WO != null)
                    {
                        var Vendoremail = db.Vendors.Where(v => v.VendorId == WO.Vendor).FirstOrDefault();
                        if (Vendoremail != null)
                        {
                            WO.IsWorkOrderSent = true;
                            db.SaveChanges();
                            var WorkOrder = db.prc_SaveWorkOrderActionDetails(WO.WorkOrderId, Guid.Empty, WO.CreatedByUser, WO.Client, DateTime.Now, Notes, "EmailWO");
                            OperationStatus = Vendoremail.Email;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
            }
            return OperationStatus;
        }

        public List<EmailData> GetEmailData(Guid WorkOrderId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var EmailData = (from wo in db.WorkOrders
                                     join v in db.Vendors on wo.Vendor equals v.VendorId
                                     join u in db.Users on wo.CreatedByUser equals u.UserId
                                     join sr in db.ServiceRequests on wo.ServiceRequest equals sr.ServiceRequestId
                                     join c in db.Clients on sr.Client equals c.ClientId
                                     where wo.WorkOrderId == WorkOrderId
                                     select new
                                     {
                                         WorkOrderId = wo.WorkOrderId,
                                         VendorName = v.VendorName,
                                         FirstName = u.FirstName,
                                         LastName = u.LastName,
                                         ClientName = c.ClientName
                                     }).ToList().Select(W => new EmailData()
                                     {
                                         WorkOrderId = W.WorkOrderId,
                                         VendorName = W.VendorName,
                                         FirstName = W.FirstName,
                                         LastName = W.LastName,
                                         ClientName = W.ClientName
                                     }).ToList();
                    return EmailData;
                }
            }
            catch
            {
                return null;
            }
        }

        public string SendEmailtoVendor(ActionTabEmailOld item)
        {
            try
            {
                //repository = new UOWWorkOrder();

                ActionTabEmail actionTabEmail = new ActionTabEmail();

                //1. ) Adding "to" Fields in Message Fields.
                ToFields toFields = new ToFields();
                toFields.EmailAddress = "%%DeliveryAddress%%";
                toFields.FriendlyName = "";

                List<ToFields> toFieldsList = new List<ToFields>();
                toFieldsList.Add(toFields);
                MessagesFields messagesFields = new MessagesFields();
                messagesFields.To = toFieldsList;


                List<string> emailAddresses = new List<string>();
                emailAddresses.Add(item.Toemail);

                //2.) Adding CC Fields in Message Fields.
                List<CcFields> ccFieldsList = new List<CcFields>();
                if (item.Ccemail != null)
                {
                    foreach (string ccItem in item.Ccemail)
                    {
                        CcFields ccFields = new CcFields();
                        // ccFields.EmailAddress = "%%CcEmail1%%";
                        ccFields.EmailAddress = "%%DeliveryAddress%%";
                        ccFields.FriendlyName = "";
                        ccFieldsList.Add(ccFields);

                        emailAddresses.Add(ccItem);
                    }
                    messagesFields.Cc = ccFieldsList;
                }

                // Adding Work Order Attachment for sending email.
                string FileName = null;
                string temp_inBase64 = null;
                List<AttachmentsDocuments> DocsFile = new List<AttachmentsDocuments>();
                if (item.ChekValue == true)
                {
                    byte[] EmailPDFData = GetPDFDataWith(item.WorkOrderId);
                    temp_inBase64 = Convert.ToBase64String(EmailPDFData);
                    FileName = "WO_" + item.WorkOrderNumber + ".pdf";

                    AttachmentsDocuments docs = new AttachmentsDocuments();
                    docs.Name = FileName;
                    docs.Content = temp_inBase64;
                    DocsFile.Add(docs);
                }

                // Adding Other Document for sending email.
                if (item.OtherDocument != null)
                {
                    foreach (var value in item.OtherDocument)
                    {
                        List<Tuple<byte[], string>> DocsData = GetAttachmentData(value);
                        FileName = DocsData[0].Item2;
                        temp_inBase64 = Convert.ToBase64String(DocsData[0].Item1);

                        AttachmentsDocuments docs = new AttachmentsDocuments();
                        docs.Name = FileName;
                        docs.Content = temp_inBase64;
                        DocsFile.Add(docs);
                    }
                }

                // Getting data for dynamic values on Email Template
                List<EmailData> EmailData = GetEmailData(item.WorkOrderId);
                List<fn_getClientResourceData_Result> HeaderData = DataforPDfHeader(item.ClientId);
                List<List<PerMessage>> perMessagesArrayList = new List<List<PerMessage>>();

                MergedData mergedData = new MergedData();
                foreach (string e in emailAddresses)
                {
                    List<PerMessage> perMessagesList = new List<PerMessage>();

                    PerMessage perMessageDeliveryAddress = new PerMessage();
                    perMessageDeliveryAddress.Field = "DeliveryAddress";
                    perMessageDeliveryAddress.Value = e;
                    PerMessage perMessageVendorName = new PerMessage();
                    perMessageVendorName.Field = "VendorName";
                    perMessageVendorName.Value = EmailData[0].VendorName;

                    PerMessage perMessageCSRName = new PerMessage();
                    perMessageCSRName.Field = "CSRName";
                    perMessageCSRName.Value = EmailData[0].FirstName + " " + EmailData[0].LastName;

                    PerMessage perMessageCompanyName = new PerMessage();
                    perMessageCompanyName.Field = "CompanyName";
                    perMessageCompanyName.Value = EmailData[0].ClientName;

                    PerMessage perMessageCompanyName2 = new PerMessage();
                    perMessageCompanyName2.Field = "CompanyName2";
                    perMessageCompanyName2.Value = EmailData[0].ClientName;

                    PerMessage perMessageAddress01 = new PerMessage(); ;
                    PerMessage perMessageAddress02 = new PerMessage();
                    PerMessage perMessageTelephone = new PerMessage();
                    PerMessage perMessageFax = new PerMessage();
                    PerMessage perMessageURL = new PerMessage();

                    foreach (fn_getClientResourceData_Result headerData in HeaderData)
                    {
                        if (headerData.Name == "WOHeaderLine01")
                        {
                            perMessageAddress01.Field = "Address01";
                            perMessageAddress01.Value = headerData.Value;
                        }
                        if (headerData.Name == "WOHeaderLine02")
                        {

                            perMessageAddress02.Field = "Address02";
                            perMessageAddress02.Value = headerData.Value;
                        }
                        if (headerData.Name == "WOHeaderLine03")
                        {

                            perMessageTelephone.Field = "Telephone";
                            perMessageTelephone.Value = headerData.Value;
                        }
                        if (headerData.Name == "WOHeaderLine04")
                        {

                            perMessageFax.Field = "Fax";
                            perMessageFax.Value = headerData.Value;
                        }
                        if (headerData.Name == "WOHeaderLine05")
                        {

                            perMessageURL.Field = "URL";
                            perMessageURL.Value = headerData.Value;
                        }
                    }
                    PerMessage perMessageMessage = new PerMessage();
                    perMessageMessage.Field = "Message";
                    perMessageMessage.Value = item.Message;



                    perMessagesList.Add(perMessageDeliveryAddress);
                    perMessagesList.Add(perMessageVendorName);
                    perMessagesList.Add(perMessageCSRName);
                    perMessagesList.Add(perMessageCompanyName);
                    perMessagesList.Add(perMessageCompanyName2);
                    perMessagesList.Add(perMessageAddress01);
                    perMessagesList.Add(perMessageAddress02);
                    perMessagesList.Add(perMessageTelephone);
                    perMessagesList.Add(perMessageFax);
                    perMessagesList.Add(perMessageURL);
                    perMessagesList.Add(perMessageMessage);

                    perMessagesArrayList.Add(perMessagesList);

                    mergedData.PerMessage = perMessagesArrayList;
                }

                messagesFields.MergeData = mergedData;
                messagesFields.Attachments = DocsFile;
                messagesFields.Subject = item.WorkOrderNumber;

                // To add everything in the Message Fields  
                List<MessagesFields> messagesFieldsList = new List<MessagesFields>();
                messagesFieldsList.Add(messagesFields);
                actionTabEmail.Messages = messagesFieldsList;

                string Updatevalue = SendEmailtoVendor(item.WorkOrderId, FileName, item.Notes);
                new UnitOfwork.UOWRepository.UOWEmailModule().SendEmail(actionTabEmail, item.ClientId);
                return Updatevalue;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
                return ex.Message;
            }
        }

        public string CreateServiceandWorkOrderByPMDataAndSendIt()
        {
            System.Text.StringBuilder message = new System.Text.StringBuilder("");
            WorkOrderEntities entities = new WorkOrderEntities();

            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                   // 1 find list of all the sceduled work orders for today WORKORDERCREATIONDATE table
                    var GetWOScheduledtoday = db.PMWorkOrderCreationDates.ToList();

                    GetWOScheduledtoday = GetWOScheduledtoday.Where(a => Convert.ToDateTime(a.WorkOrderCreationDate) > DateTime.Now).ToList();

                    //loop in above list and repeat #2-#6
                    foreach (var itemWOForToday in GetWOScheduledtoday)
                    {
                        message.Append("WODate:" + itemWOForToday.PMWorkOrderCreationDateId + "; ");

                        //2 go to PMHeader Table and get client and customer info
                        var GetPMHeaderDetails = db.ClientPMHeaders.Where(p => p.ClientPMHeaderId == itemWOForToday.ClientPMHeader).FirstOrDefault();
                        if (GetPMHeaderDetails != null)
                        {
                            message.Append("PMHeader:" + GetPMHeaderDetails.ClientPMHeaderId + "; ");
                            //3 go to PMheader to get the vendor for pm header
                            var GetVendorForHeader = db.PMVendors.Where(p => p.PMHeader == itemWOForToday.ClientPMHeader).FirstOrDefault();
                            if (GetVendorForHeader != null)
                            {
                                message.Append("Vendor:" + GetVendorForHeader.Vendor + "; ");
                                //4 go to vendor to find the email against the above vendor
                                var GetVendorDetails = db.Vendors.Where(p => p.VendorId == GetVendorForHeader.Vendor).FirstOrDefault();
                                if (GetVendorDetails != null)
                                {

                                    //5 go to service request and get service requests for client+customer combination (picked from #2) ...ONLY with status='New'
                                    var GetServiceRequestDetail = db.ServiceRequests.Where(p => p.Client == GetPMHeaderDetails.Client && p.Customer == GetPMHeaderDetails.Customer).FirstOrDefault();
                                    if (GetServiceRequestDetail != null)
                                    {
                                        message.Append("SR:" + GetServiceRequestDetail.ServiceRequestId + "; ");
                                       // WORK ORDER CREATE AND SAVE
                                       // 6 Pick all the details from above var and create the work order and save it... which are not sent and have status ='P'

                                        entities.WorkOrderId = Guid.Empty;
                                        entities.Vendor = GetVendorDetails.VendorId;//Vendor ID given
                                        entities.Client = GetPMHeaderDetails.Client;//client ID given
                                        entities.Servicetype = "Service Request"; //service type given
                                        entities.Statusdescription = "New";// status description given
                                        entities.ServiceRequest = GetServiceRequestDetail.ServiceRequestId;//service request ID given
                                        entities.CreatedByUser = GetPMHeaderDetails.CreatedByUser;//createdby given
                                        entities.LastUpdatedByUser = GetPMHeaderDetails.CreatedByUser;//last update by given
                                        entities.DateArriveFrom = itemWOForToday.WorkOrderCreationDate;//WO arrive date given
                                        entities.DateArriveTo = itemWOForToday.WorkOrderFinishDate;//WO finish date given
                                        entities.TimeArriveFrom = string.Empty;
                                        entities.TimeArriveTo = string.Empty;
                                        entities.Description = "Schedular Created WO";
                                        entities.NTE = GetVendorForHeader.WONTE;//NTE given

                                        List<Tuple<Guid, string>> SaveWorkOrder = SaveWorkOrderDetails(entities);//This will return the WOID and Number
                                        message.Append("WO Saved:" + entities.WorkOrderId + "; ");
                                        entities.WorkOrderId = SaveWorkOrder[0].Item1;
                                        entities.WorkOrderNumber = SaveWorkOrder[0].Item2;
                                        if (SaveWorkOrder[0].Item1 != Guid.Empty)
                                        {
                                            //7 Create the PDF and return it as byte[]
                                            entities.PDF = GetCustomerinformation(entities.ServiceRequest, entities.WorkOrderId, entities.Client);
                                            message.Append("PDF Created ; ");

                                            //8 Save the PDF
                                            bool IsPDFUpdated = UpdatePDFData(entities);
                                            if (IsPDFUpdated)
                                            {
                                                message.Append("PDF Data Filled ; ");
                                                //9 Send the email 
                                                ActionTabEmailOld itemEmail = new ActionTabEmailOld();
                                                itemEmail.Toemail = GetVendorDetails.Email;
                                                itemEmail.Ccemail = null;
                                                itemEmail.WorkOrderId = entities.WorkOrderId;
                                                itemEmail.WorkOrderNumber = entities.WorkOrderNumber;
                                                itemEmail.Message = "PM Module Emails";
                                                itemEmail.ChekValue = true;
                                                itemEmail.ClientId = entities.Client;
                                                itemEmail.OtherDocument = null;
                                                string opstatus = SendEmailtoVendor(itemEmail);
                                                if (opstatus.Trim().ToLower() != "Failure".Trim().ToLower())
                                                {
                                                    message.Append("Email Sent ; ");
                                                    //update the sent date in the database 
                                                    var GettheCurrentRecord = db.PMWorkOrderCreationDates.Where(p => p.PMWorkOrderCreationDateId == itemWOForToday.PMWorkOrderCreationDateId).FirstOrDefault();

                                                    var GetWorkOrderSentStatus = db.WorkOrders.Where(p => p.WorkOrderId == entities.WorkOrderId).FirstOrDefault();

                                                    if (GettheCurrentRecord != null && GetWorkOrderSentStatus != null)
                                                    {
                                                        GettheCurrentRecord.WorkOrderSentDate = DateTime.Now;
                                                        GetWorkOrderSentStatus.IsWorkOrderSent = true;
                                                        db.SaveChanges();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return message.Append(";;;;" + e.Message).ToString();
            }


            return message.ToString();
        }

        public bool UpdatePMWOCreationDateColumn()
        {

            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var data = db.PMWorkOrderCreationDates;

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool InsertDummyData()
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                //CGSFileType cgs = new CGSFileType();

                //DataModel.CGSFileType tt = new CGSFileType();
                //Guid newid =new Guid("7F379DD2-7C92-4995-A899-8B04E742F783");

                //var getcurrent = db.CGSFileTypes.Where(p => p.CGSFileTypesId == newid).FirstOrDefault();
                //if (getcurrent != null)
                //{
                //    getcurrent.Decription = "test"+System.DateTime.Now.ToString();
                //    cgs.Extention = "Test ext";
                //    db.SaveChanges();
                //}

                //add in dummy

                PMTestTable pmtest = new PMTestTable();
                Guid newid = new Guid("4F650625-7CAC-4691-8DCE-ACCD5DA25005");

                var getcurrent = db.PMTestTables.Where(p => p.ID == newid).FirstOrDefault();
                if (getcurrent != null)
                {
                    getcurrent.Description = "test" + System.DateTime.Now.ToString();
                    db.SaveChanges();
                }
                return true;
            }
        }
    }
}

