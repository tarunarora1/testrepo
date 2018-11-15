using BusinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;

namespace UnitOfWork.Interface
{
    public interface IWorkOrder
    {
        List<EmailData> GetEmailData(Guid WorkOrderId);
        IEnumerable<ClientEntities> GetClient();
        IEnumerable<VendorEntities> GetVendor(Guid _ClientId);
        IEnumerable<StatusEntities> GetStatus();
        List<Tuple<Guid, string>> SaveWorkOrderDetails(WorkOrderEntities workorderitem);
        bool UpdatePDFData(WorkOrderEntities item);
        IEnumerable<WorkOrderEntities> GetWorkOrderRequest(Guid servicerequestId);
        IEnumerable<WorkOrderEntities> GetWorkOrderControlsData(Guid workorderID);
        byte[] GetPDFData(Guid workorderID);
        byte[] GetCustomerinformation(Guid servicerequestID, Guid WorkOrderId, Guid ClientId);
        IList<customerinformationforpdf> GetCustomerinformation2(Guid servicerequestID);
        IEnumerable<WorkOrderEntities> GetVendorAdminWorkOrderRequest(Guid UserID);
        string SendEmailtoVendor(Guid workorderid, string WorkOrderNumber, string Notes);
        byte[] GetPDFDataWith(Guid workorderID);
        IEnumerable<ClientWorkOrderNoteType> GetWorkOrderType(Guid ClientId);
        string SaveWorkOrderNotesDetails(WorkOrderNotesEntity item);
        IEnumerable<WorkOrderNotesEntity> GetWorkOrderNoteGridData(Guid WorkOrderId);
        IEnumerable<ClientAttachmentType> GetWorkOrderAttachmentTypes(Guid ClientId, string item);
        string SaveWorkOrderAttachments(WorkOrderAttachmentTypesEntity item);
        IEnumerable<WorkOrderAttachmentTypesEntity> GetWorkOrderAttachmentsGridData(Guid WorkOrderId);
        IList<prc_GetAttachment_Result> GetAttachmentFileData(Guid AttachmentId);
        IEnumerable<ClientClassOfGood> GetGoodsClasses(Guid Clientid);
        VendorInvoiceEntity GetVendorInvoiceGridData(Guid workorderID, Guid HeaderId);
        bool SaveVendorInvoiceDetails(VendorInvoiceEntity item1);
        bool SaveWorkOrderAction(WorkOrderActionEntity item);
        IList<WorkOrderCode> GetActionCode(Guid ClientId);
        IEnumerable<StatusEntities> GetStatusForActionTab(string item, Guid ClientId);
        List<Tuple<byte[], string>> GetAttachmentData(Guid value);
        IEnumerable<prc_GetAttachment_Result> GetAttachemntData(Guid AttachmentId);
        IEnumerable<prc_GetWorkOrderVendorInvoiceData_Result> GetWorkOrderInvoiceData(Guid workorderid);
        IEnumerable<Get_ServiceWorkOrderNoteGridData_Result> GetServiceWorkOrderNoteGridData(Guid WorkOrderId, Guid ServiceRequestId);
        int CheckInvoiceExitsCount(Guid workorderid);

        List<fn_getClientResourceData_Result> DataforPDfHeader(Guid ClientId);
        string CreateServiceandWorkOrderByPMDataAndSendIt();
        bool UpdatePMWOCreationDateColumn();
        bool InsertDummyData();
    }
}
