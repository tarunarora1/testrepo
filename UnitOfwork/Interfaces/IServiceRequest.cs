using BusinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;

namespace UnitOfWork.Interface
{
    public interface IServiceRequest
    {        
        IEnumerable<ServiceUserLocation> GetLocationbyUser(Guid CustomerId);
        IEnumerable<prc_GetCustomerLocationsForUser_Result> GetLocationbyUser(Guid UserID, string item);
        IEnumerable<CustomerService> GetCustomer(string ClientID);
        Guid AddServiceRequest(ServiceRequestEntities ServiceRequest);
        List<ServiceRequestModel> GetServiceRequest(Guid Clientid);
        IEnumerable<ServiceRequestEntities> GetServiceRequestDatabyID(Guid servicerequestid);
        IEnumerable<ClientService> GetClientForCustomer(Guid _LoggedInUserID);
        List<ServiceRequestModel> GetServiceRequestforcustomer(string UserEmail, string UserRole);
        int GetCountofWorkOrder(Guid servicerequestid);
        IEnumerable<ProblemClassesEntity> GetProblemClasses(string ClientID);
        IEnumerable<RequestEntity> GetRequestPriorties(string ClientID);
        IEnumerable<ProblemCodeEntity> GetProblemCodes(Guid ProblemClassID,Guid ClientID);
        IEnumerable<ClientServiceType> GetServiceRequestType(string Clientid);
        IEnumerable<ClientServiceType> GetServiceRequestTypeForCustomer(Guid Clientid);
        List<CGSInterval> GetCGSInterval();
        List<Tuple<Guid, IList<CustomerLocation>>> AddNewLocation(CustomerLocation item);
        IEnumerable<ServiceRequestModel> GetExistsWorkOrderORNOT(Guid ClientId, Guid CustomerId, Guid LocationId);
        IEnumerable<WorkOrderNotesEntity> GetWorkOrderNoteGridData(Guid servicerequestid);
        IEnumerable<Get_serviceandworkorderattachmentdata_Result> GetServiceRequestAttachmentData(Guid ServiceRequestId);
        IEnumerable<WorkOrderAttachmentTypesEntity> GetServiceAttachmentData(Guid Attachmentid,string AttachmentType);
        bool SaveServiceRequestAttachments(WorkOrderAttachmentTypesEntity item);
        IEnumerable<prc_GetInvoiceServiceData_Result> GetVendorInvoiceServiceData(Guid ServiceId,string Value,Guid SIHeaderId);
        IEnumerable<prc_GetServiceInvoiceServiceDetailsData_Result> GetServiceInvoiceDeatilsData(Guid ServiceId, string Value,string ProjectStatus);
        Guid SaveServiceInvoiceHeader(ServiceRequestInvoiceHeaderEntity item);
        IEnumerable<prc_GetServiceInvoiceDetailsDataWithWorkOrder_Result> GetServiceInvoiceDetailsData(Guid ServiceHeaderId);
        bool SaveServiceInvoiceDetails(List<ServiceInvoice> VendorInvoice);
        IList<StatusEntities> GetServiceInvoiceStatus(string item,Guid ClientId);
        bool SubmitQuoteWithEmail(ServiceRequestInvoiceAction1 item);
        byte[] GetServiceInvoicePDFData(Guid ServiceRequestInvoiceHeaderId);
        IList<ServiceClassOfGood> GetServiceInvoiceWthoutWorkorder(Guid ServiceHeaderId);
        List<SalesTaxClouddetails> GetTaxCloudAPIDetails();
        IEnumerable<GetServiceRequestIvoiceQuoteCount_Result> GetServiceRequestInvoiveQuoteCount(Guid ServiceRequestId);
        bool DeleteRecordfromheader(Guid ServiceHeaderId);
        string GetClientEmailAddress(Guid ClientId);
        List<Tuple<byte[], string>> GetServiceQuoteAttachmentData(Guid value);
        IEnumerable<Get_ServiceQuoteUserStatusGridData_Result> GetServiceQuoteUserStatusGridData(Guid ServiceHeaderId,string CallStatus);
        string GenerateQuoteAndInvoiceNumber(ServiceRequestInvoiceHeaderEntity item);
        bool UpdatePDFOnSubmitQuote(List<ServiceInvoice> item);
        bool SaveAuthorizeAndCaptureAPIResponse(ServiceRequestInvoiceHeaderApiResponseEntity item);
        List<fn_getClientResourceData_Result> GetResourceSalesTaxDetails(Guid ClientId);
    }
}
