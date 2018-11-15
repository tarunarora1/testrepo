using BusinessEntities.BusinessEntityClasses;
using DataModel;
using FacilitiesServices.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using UnitOfWork.Interface;
using UnitOfWork.UOWRepository;
using System.Linq;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/ServiceRequest")]
    public class ServiceRequestController : ApiController
    {
        IServiceRequest repository = new UOWServiceRequest();

        [HttpGet]
        [Route("GetServiceRequest")]
        public List<ServiceRequestModel> GetServiceRequest(Guid Clientid)
        {
            return repository.GetServiceRequest(Clientid);
        }

        [HttpGet]
        [Route("GetLocationbyUser")]
        public IEnumerable<ServiceUserLocation> GetLocationbyUser(Guid CustomerId)
        {
            return repository.GetLocationbyUser(CustomerId);
        }

        [HttpGet]
        [Route("GetLocationbyUser")]
        public IEnumerable<prc_GetCustomerLocationsForUser_Result> GetLocationbyUser(Guid UserID, string item)
        {
            return repository.GetLocationbyUser(UserID, item);
        }

        [HttpGet]
        [Route("GetClientForCustomer")]
        public IEnumerable<ClientService> GetClientForCustomer(Guid _LoggedInUserID)
        {
            return repository.GetClientForCustomer(_LoggedInUserID);
        }

        [HttpGet]
        [Route("GetProblemClasses")]
        public IEnumerable<ProblemClassesEntity> GetProblemClasses(string ClientID)
        {
            return repository.GetProblemClasses(ClientID);
        }

        [HttpGet]
        [Route("GetProblemCodes")]
        public IEnumerable<ProblemCodeEntity> GetProblemCodes(Guid ProblemClassID, Guid ClientID)
        {
            return repository.GetProblemCodes(ProblemClassID, ClientID);
        }

        [HttpGet]
        [Route("GetRequestPriorties")]
        public IEnumerable<RequestEntity> GetRequestPriorties(string ClientID)
        {
            return repository.GetRequestPriorties(ClientID);
        }

        [HttpGet]
        [Route("GetCustomer")]
        public IEnumerable<CustomerService> GetCustomer(string ClientID)
        {
            return repository.GetCustomer(ClientID);
        }

        [HttpPost]
        [Route("AddServiceRequest")]
        public Guid AddServiceRequest(ServiceRequestEntities ServiceRequest)
        {
            return repository.AddServiceRequest(ServiceRequest);
        }

        [HttpGet]
        [Route("GetServiceRequestDatabyID")]
        public IEnumerable<ServiceRequestEntities> GetServiceRequestDatabyID(Guid servicerequestid)
        {
            return repository.GetServiceRequestDatabyID(servicerequestid);
        }

        [HttpGet]
        [Route("GetServiceRequestforcustomer")]
        public List<ServiceRequestModel> GetServiceRequestforcustomer(string UserEmail, string UserRole)
        {
            return repository.GetServiceRequestforcustomer(UserEmail, UserRole);
        }
        [HttpGet]
        [Route("GetCountofWorkOrder")]
        public int GetCountofWorkOrder(Guid servicerequestid)
        {
            return repository.GetCountofWorkOrder(servicerequestid);
        }
        [HttpGet]
        [Route("GetServiceRequestType")]
        public IEnumerable<ClientServiceType> GetServiceRequestType(string Clientid)
        {
            return repository.GetServiceRequestType(Clientid);
        }

        [HttpGet]
        [Route("GetCGSInterval")]
        public List<CGSInterval> GetCGSInterval()
        {
            return repository.GetCGSInterval();
        }

        [HttpGet]
        [Route("GetServiceRequestTypeForCustomer")]
        public IEnumerable<ClientServiceType> GetServiceRequestTypeForCustomer(Guid Clientid)
        {
            return repository.GetServiceRequestTypeForCustomer(Clientid);
        }

        [HttpPost]
        [Route("AddNewLocation")]
        public List<Tuple<Guid, IList<CustomerLocation>>> AddNewLocation(CustomerLocation item)
        {
            return repository.AddNewLocation(item);
        }

        [HttpGet]
        [Route("GetExistsWorkOrderORNOT")]
        public IEnumerable<ServiceRequestModel> GetExistsWorkOrderORNOT(Guid ClientId, Guid CustomerId, Guid LocationId)
        {
            return repository.GetExistsWorkOrderORNOT(ClientId, CustomerId, LocationId);
        }

        [HttpGet]
        [Route("GetWorkOrderNoteGridData")]
        public IEnumerable<WorkOrderNotesEntity> GetWorkOrderNoteGridData(Guid servicerequestid)
        {
            return repository.GetWorkOrderNoteGridData(servicerequestid);
        }

        [HttpGet]
        [Route("GetServiceRequestAttachmentData")]
        public IEnumerable<Get_serviceandworkorderattachmentdata_Result> GetServiceRequestAttachmentData(Guid ServiceRequestId)
        {
            return repository.GetServiceRequestAttachmentData(ServiceRequestId);
        }

        [HttpGet]
        [Route("GetServiceRequestInvoiveQuoteCount")]
        public IEnumerable<GetServiceRequestIvoiceQuoteCount_Result> GetServiceRequestInvoiveQuoteCount(Guid ServiceRequestId)
        {
            return repository.GetServiceRequestInvoiveQuoteCount(ServiceRequestId);
        }

        [HttpGet]
        [Route("GetServiceAttachmentData")]
        public IEnumerable<WorkOrderAttachmentTypesEntity> GetServiceAttachmentData(Guid Attachmentid, string AttachmentType)
        {
            return repository.GetServiceAttachmentData(Attachmentid, AttachmentType);
        }

        [HttpPost]
        [Route("SaveServiceRequestAttachments")]
        public bool SaveServiceRequestAttachments(WorkOrderAttachmentTypesEntity item)
        {
            return repository.SaveServiceRequestAttachments(item);
        }

        [HttpGet]
        [Route("GetVendorInvoiceServiceData")]
        public IEnumerable<prc_GetInvoiceServiceData_Result> GetVendorInvoiceServiceData(Guid ServiceId, string Value, Guid SIHeaderId)
        {
            return repository.GetVendorInvoiceServiceData(ServiceId, Value, SIHeaderId);
        }

        [HttpGet]
        [Route("GetServiceInvoiceDeatilsData")]
        public IEnumerable<prc_GetServiceInvoiceServiceDetailsData_Result> GetServiceInvoiceDeatilsData(Guid ServiceId, string Value, string ProjectStatus)
        {
            return repository.GetServiceInvoiceDeatilsData(ServiceId, Value, ProjectStatus);
        }

        [HttpPost]
        [Route("SaveServiceInvoiceHeader")]
        public Guid SaveServiceInvoiceHeader(ServiceRequestInvoiceHeaderEntity item)
        {
            return repository.SaveServiceInvoiceHeader(item);
        }

        [HttpGet]
        [Route("GetServiceInvoiceDetailsData")]
        public IEnumerable<prc_GetServiceInvoiceDetailsDataWithWorkOrder_Result> GetServiceInvoiceDetailsData(Guid ServiceHeaderId)
        {
            return repository.GetServiceInvoiceDetailsData(ServiceHeaderId);
        }

        [HttpPost]
        [Route("SaveServiceInvoiceDetails")]
        public bool SaveServiceInvoiceDetails(List<ServiceInvoice> item)
        {
            return repository.SaveServiceInvoiceDetails(item);
        }

        [HttpGet]
        [Route("GetServiceInvoiceStatus")]
        public IList<StatusEntities> GetServiceInvoiceStatus(string item, Guid Clientid)
        {
            return repository.GetServiceInvoiceStatus(item, Clientid);
        }

        //[HttpPost]
        //[Route("SubmitQuoteWithEmail")]
        //public bool SubmitQuoteWithEmail(ServiceRequestInvoiceAction1 item)
        //{
        //    string FileName = null;
        //    string temp_inBase64 = null;
        //    List<Tuple<string>> DocsFile = new List<Tuple<string>>();
        //    var UpdatedValue = repository.SubmitQuoteWithEmail(item);
        //    byte[] EmailPDFData = repository.GetServiceInvoicePDFData(item.ServiceRequestInvoiceHeaderId);
        //    temp_inBase64 = Convert.ToBase64String(EmailPDFData);
        //    FileName = "Quote" + ".pdf";           
        //    DocsFile.Add(new Tuple<string>(temp_inBase64));
        //    if (UpdatedValue == true)
        //    {
        //        new EmailController().SendEmailToCustomer(item.Toemail, item.Ccemail, item.Message, FileName, DocsFile, item.ClientId);
        //    }

        //    return UpdatedValue;
        //}

        [HttpPost]
        [Route("SubmitQuoteWithEmail")]
        public bool SubmitQuoteWithEmail(ServiceRequestInvoiceAction1 item)
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
                    ccFields.EmailAddress = "%%DeliveryAddress%%";
                    ccFields.FriendlyName = "";
                    ccFieldsList.Add(ccFields);

                    emailAddresses.Add(ccItem);
                }
                messagesFields.Cc = ccFieldsList;

                string FileName = null;
                string temp_inBase64 = null;
                List<AttachmentsDocuments> DocsFile = new List<AttachmentsDocuments>();

                var UpdatedValue = repository.SubmitQuoteWithEmail(item);


                byte[] EmailPDFData = repository.GetServiceInvoicePDFData(item.ServiceRequestInvoiceHeaderId);
                temp_inBase64 = Convert.ToBase64String(EmailPDFData);
                FileName = "Quote" + ".pdf";
                AttachmentsDocuments docs = new AttachmentsDocuments();
                docs.Name = FileName;
                docs.Content = temp_inBase64;
                DocsFile.Add(docs);

                // Getting data for dynamic values on Email Template
                List<EmailDataForQuoteInvoice> EmailData = GetEmailData(item.ServiceRequestInvoiceHeaderId);
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
                    perMessageVendorName.Value = EmailData[0].CustomerName;

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
                messagesFields.Subject = item.ActionCode;

                // To add everything in the Message Fields  
                List<MessagesFields> messagesFieldsList = new List<MessagesFields>();
                messagesFieldsList.Add(messagesFields);
                actionTabEmail.Messages = messagesFieldsList;

                if (UpdatedValue == true)
                {
                    new EmailController().SendEmailToCustomer(actionTabEmail, item.ClientId);
                }

                return UpdatedValue;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private List<EmailDataForQuoteInvoice> GetEmailData(Guid ServiceRequestInvoiceHeaderId)
        {
            try
            {
                using (FacilitiesEntities db = new FacilitiesEntities())
                {
                    var EmailData = (from cl in db.Clients
                                     join cc in db.ClientCustomers on cl.ClientId equals cc.Client
                                     join cs in db.Customers on cc.Customer equals cs.CustomerId
                                     join sr in db.ServiceRequests on cs.CustomerId equals sr.Customer
                                     join sri in db.ServiceRequestInvoiceHeaders on sr.ServiceRequestId equals sri.ServiceRequest
                                     join us in db.Users on sri.User equals us.UserId
                                     where sri.ServiceRequestInvoiceHeaderId == ServiceRequestInvoiceHeaderId
                                     select new
                                     {
                                         FirstName = us.FirstName,
                                         LastName = us.LastName,
                                         CustomerName = cs.CustomerName,
                                         ClientName = cl.ClientName
                                     }).ToList().Select(W => new EmailDataForQuoteInvoice()
                                     {
                                         FirstName = W.FirstName,
                                         LastName = W.LastName,
                                         CustomerName = W.CustomerName,
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

        [HttpGet]
        [Route("GetServiceInvoiceWthoutWorkorder")]
        public IList<ServiceClassOfGood> GetServiceInvoiceWthoutWorkorder(Guid ServiceHeaderId)
        {
            return repository.GetServiceInvoiceWthoutWorkorder(ServiceHeaderId);
        }

        [HttpGet]
        [Route("GetTaxCloudAPIDetails")]
        public List<SalesTaxClouddetails> GetTaxCloudAPIDetails()
        {
            return repository.GetTaxCloudAPIDetails();
        }

        [HttpGet]
        [Route("GetResourceSalesTaxDetails")]
        public List<fn_getClientResourceData_Result> GetResourceSalesTaxDetails(Guid ClientId)
        {
            return repository.GetResourceSalesTaxDetails(ClientId);
        }

        [HttpDelete]
        [Route("DeleteRecordfromheader")]
        public bool DeleteRecordfromheader(Guid ServiceHeaderId)
        {
            return repository.DeleteRecordfromheader(ServiceHeaderId);
        }

        [HttpGet]
        [Route("GetClientEmailAddress")]
        public string GetClientEmailAddress(Guid ClientId)
        {
            return repository.GetClientEmailAddress(ClientId);
        }

        [HttpGet]
        [Route("GetServiceQuoteAttachmentData")]
        public List<Tuple<byte[], string>> GetServiceQuoteAttachmentData(Guid item)
        {
            return repository.GetServiceQuoteAttachmentData(item);
        }

        [HttpGet]
        [Route("GetServiceQuoteUserStatusGridData")]
        public IEnumerable<Get_ServiceQuoteUserStatusGridData_Result> GetServiceQuoteUserStatusGridData(Guid item, string CallStatus)
        {
            return repository.GetServiceQuoteUserStatusGridData(item, CallStatus);
        }

        [HttpPut]
        [Route("GenerateQuoteAndInvoiceNumber")]
        public string GenerateQuoteAndInvoiceNumber(ServiceRequestInvoiceHeaderEntity item)
        {
            return repository.GenerateQuoteAndInvoiceNumber(item);
        }

        [HttpPost]
        [Route("UpdatePDFOnSubmitQuote")]
        public bool UpdatePDFOnSubmitQuote(List<ServiceInvoice> item)
        {
            return repository.UpdatePDFOnSubmitQuote(item);
        }

        [HttpPost]
        [Route("SaveAuthorizeAndCaptureAPIResponse")]
        public bool SaveAuthorizeAndCaptureAPIResponse(ServiceRequestInvoiceHeaderApiResponseEntity item)
        {
            return repository.SaveAuthorizeAndCaptureAPIResponse(item);
        }
    }
}
