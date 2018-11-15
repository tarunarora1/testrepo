using UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Web.Http;
using UnitOfWork.UOWRepository;
using System.IO;
using DataModel;
using BusinessEntities.BusinessEntityClasses;
using FacilitiesServices.Filters;
using System.Linq;
using System.IO;
using context = System.Web.HttpContext;
namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/WorkOrder")]
    public class WorkOrderController : ApiController
    {
        IWorkOrder repository = new UOWWorkOrder();

        [HttpGet]
        [Route("GetClient")]
        public IEnumerable<ClientEntities> GetClient()
        {
            return repository.GetClient();
        }

        [HttpGet]
        [Route("GetVendor")]
        public IEnumerable<VendorEntities> GetVendor(Guid _ClientId)
        {
            return repository.GetVendor(_ClientId);
        }

        [HttpGet]
        [Route("GetStatus")]
        public IEnumerable<StatusEntities> GetStatus()
        {
            return repository.GetStatus();
        }

        [HttpGet]
        [Route("GetStatusForActionTab")]
        public IEnumerable<StatusEntities> GetStatusForActionTab(string ActionCode, Guid ClientId)
        {
            return repository.GetStatusForActionTab(ActionCode, ClientId);
        }

        [HttpPost]
        [Route("SaveWorkOrderDetails")]
        public List<Tuple<Guid, string>> SaveWorkOrderDetails(WorkOrderEntities WorkOrderEntities)
        {
            return repository.SaveWorkOrderDetails(WorkOrderEntities);
        }

        [HttpPost]
        [Route("UpdatePDFData")]
        public bool UpdatePDFData(WorkOrderEntities item)
        {
            return repository.UpdatePDFData(item);
        }

        [HttpPost]
        [Route("SaveWorkOrderNotesDetails")]
        public string SaveWorkOrderNotesDetails(WorkOrderNotesEntity item)
        {
            return repository.SaveWorkOrderNotesDetails(item);
        }

        [HttpPost]
        [Route("SaveWorkOrderAttachments")]
        public string SaveWorkOrderAttachments(WorkOrderAttachmentTypesEntity item)
        {
            return repository.SaveWorkOrderAttachments(item);
        }

        private List<EmailData> GetEmailData(Guid WorkOrderId)
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

        [HttpPost]
        [Route("SendEmailForAction")]
        public string SendEmailForAction(ActionTabEmailOld item)
        {
            try
            {
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

                string FileName = null;
                string temp_inBase64 = null;
                List<AttachmentsDocuments> DocsFile = new List<AttachmentsDocuments>();

                if (item.ChekValue == true)
                {
                    byte[] EmailPDFData = repository.GetPDFDataWith(item.WorkOrderId);
                    temp_inBase64 = Convert.ToBase64String(EmailPDFData);
                    FileName = "WO_" + item.WorkOrderNumber + ".pdf";
                    AttachmentsDocuments docs = new AttachmentsDocuments();
                    docs.Name = FileName;
                    docs.Content = temp_inBase64;
                    DocsFile.Add(docs);

                }
                if (item.OtherDocument != null)
                {
                    foreach (var value in item.OtherDocument)
                    {
                        List<Tuple<byte[], string>> DocsData = repository.GetAttachmentData(value);
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
                UOWWorkOrder uOWWorkOrder = new UOWWorkOrder();
                List<fn_getClientResourceData_Result> HeaderData = uOWWorkOrder.DataforPDfHeader(item.ClientId);
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

                new EmailController().SendEmailForAction(actionTabEmail, item.ClientId);
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
                return ex.Message;
            }

        }

        [HttpGet]
        [Route("GetWorkOrderRequest")]
        public IEnumerable<WorkOrderEntities> GetWorkOrderRequest(Guid servicerequestId)
        {
            return repository.GetWorkOrderRequest(servicerequestId);
        }

        [HttpGet]
        [Route("GetWorkOrderControlsData")]
        public IEnumerable<WorkOrderEntities> GetWorkOrderControlsData(Guid workorderID)
        {
            return repository.GetWorkOrderControlsData(workorderID);
        }

        [HttpGet]
        [Route("GetPDFData")]
        public byte[] GetPDFData(Guid workorderID)
        {
            return repository.GetPDFData(workorderID);
        }

        [HttpGet]
        [Route("GetCustomerinformation")]
        public byte[] GetCustomerinformation(Guid servicerequestID, Guid WorkOrderId, Guid ClientId)
        {
            return repository.GetCustomerinformation(servicerequestID, WorkOrderId, ClientId);
        }

        [HttpGet]
        [Route("GetCustomerinformation2")]
        public IList<customerinformationforpdf> GetCustomerinformation2(Guid servicerequestID)
        {
            return repository.GetCustomerinformation2(servicerequestID);
        }

        [HttpGet]
        [Route("GetVendorAdminWorkOrderRequest")]
        public IEnumerable<WorkOrderEntities> GetVendorAdminWorkOrderRequest(Guid UserID)
        {
            return repository.GetVendorAdminWorkOrderRequest(UserID);
        }

        //[HttpGet]
        //[Route("GetWorkOrderNumber")]
        //public List<prc_createWorkOrderNumber_Result> GetWorkOrderNumber(Guid ClientId, string Servicetype)
        //{
        //    return repository.GetWorkOrderNumber(ClientId, Servicetype);
        //}

        [HttpGet]
        [Route("GetWorkOrderType")]
        public IEnumerable<ClientWorkOrderNoteType> GetWorkOrderType(Guid ClientId)
        {
            return repository.GetWorkOrderType(ClientId);
        }

        [HttpGet]
        [Route("GetWorkOrderNoteGridData")]
        public IEnumerable<WorkOrderNotesEntity> GetWorkOrderNoteGridData(Guid WorkOrderId)
        {
            return repository.GetWorkOrderNoteGridData(WorkOrderId);
        }

        [HttpGet]
        [Route("GetWorkOrderAttachmentTypes")]
        public IEnumerable<ClientAttachmentType> GetWorkOrderAttachmentTypes(Guid ClientId, string item)
        {
            return repository.GetWorkOrderAttachmentTypes(ClientId, item);
        }

        [HttpGet]
        [Route("GetWorkOrderAttachmentsGridData")]
        public IEnumerable<WorkOrderAttachmentTypesEntity> GetWorkOrderAttachmentsGridData(Guid WorkOrderId)
        {
            return repository.GetWorkOrderAttachmentsGridData(WorkOrderId);
        }

        [HttpGet]
        [Route("GetAttachmentFileData")]
        public IList<prc_GetAttachment_Result> GetAttachmentFileData(Guid AttachmentId)
        {
            return repository.GetAttachmentFileData(AttachmentId);
        }

        [HttpGet]
        [Route("GetGoodsClasses")]
        public IEnumerable<ClientClassOfGood> GetGoodsClasses(Guid Clientid)
        {
            return repository.GetGoodsClasses(Clientid);
        }
        [HttpGet]
        [Route("GetVendorInvoiceGridData")]
        public VendorInvoiceEntity GetVendorInvoiceGridData(Guid workorderID, Guid HeaderId)
        {
            return repository.GetVendorInvoiceGridData(workorderID, HeaderId);
        }

        [HttpPost]
        [Route("SaveVendorInvoiceDetails")]
        public bool SaveVendorInvoiceDetails(VendorInvoiceEntity item1)
        {
            return repository.SaveVendorInvoiceDetails(item1);
        }

        [HttpPost]
        [Route("SaveWorkOrderAction")]
        public bool SaveWorkOrderAction(WorkOrderActionEntity item)
        {
            return repository.SaveWorkOrderAction(item);
        }

        [HttpGet]
        [Route("GetActionCode")]
        public IList<WorkOrderCode> GetActionCode(Guid ClientId)
        {
            return repository.GetActionCode(ClientId);
        }

        [HttpGet]
        [Route("GetAttachemntData")]
        public IEnumerable<prc_GetAttachment_Result> GetAttachemntData(Guid AttachmentId)
        {
            return repository.GetAttachemntData(AttachmentId);
        }

        [HttpGet]
        [Route("GetWorkOrderInvoiceData")]
        public IEnumerable<prc_GetWorkOrderVendorInvoiceData_Result> GetWorkOrderInvoiceData(Guid workorderid)
        {
            return repository.GetWorkOrderInvoiceData(workorderid);
        }

        [HttpGet]
        [Route("GetServiceWorkOrderNoteGridData")]
        public IEnumerable<Get_ServiceWorkOrderNoteGridData_Result> GetServiceWorkOrderNoteGridData(Guid WorkOrderId, Guid ServiceRequestId)
        {
            return repository.GetServiceWorkOrderNoteGridData(WorkOrderId, ServiceRequestId);
        }

        [HttpGet]
        [Route("CheckInvoiceExitsCount")]
        public int CheckInvoiceExitsCount(Guid workorderid)
        {
            return repository.CheckInvoiceExitsCount(workorderid);
        }

        public bool UpdatePMWOCreationDateColumn()
        {
            return repository.UpdatePMWOCreationDateColumn();
        }


        [HttpPost]
        [Route("SendEmailtoVendor")]
        public string SendEmailtoVendor(ActionTabEmailOld item)
        {
            try
            {
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
                    byte[] EmailPDFData = repository.GetPDFDataWith(item.WorkOrderId);
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
                        List<Tuple<byte[], string>> DocsData = repository.GetAttachmentData(value);
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
                List<fn_getClientResourceData_Result> HeaderData = repository.DataforPDfHeader(item.ClientId);
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

                string Updatevalue = repository.SendEmailtoVendor(item.WorkOrderId, FileName, item.Notes);
                new EmailController().SendEmail(actionTabEmail, item.ClientId);
                return Updatevalue;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message: " + ex.Message);
                return ex.Message;
            }
        }


        [HttpGet]
        [Route("CreateServiceandWorkOrderByPMDataAndSendIt")]
        public string CreateServiceandWorkOrderByPMDataAndSendIt()
        {
            string operationTillnow = repository.CreateServiceandWorkOrderByPMDataAndSendIt();
            return repository.CreateServiceandWorkOrderByPMDataAndSendIt();
        }

        [HttpGet]
        [Route("TestSchedularCalling")]
        public bool TestSchedularCalling()
        {
            return repository.InsertDummyData();
        }
    }
}
