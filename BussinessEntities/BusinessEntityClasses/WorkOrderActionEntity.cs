
using System;
using System.Collections.Generic;

namespace BusinessEntities.BusinessEntityClasses
{
    public class WorkOrderActionEntity
    {
        public System.Guid WorkOrderId { get; set; }
        public System.Guid Status { get; set; }
        public System.DateTime CompletionDate { get; set; }
        public System.Guid Userid { get; set; }
        public System.Guid ClientId { get; set; }
        public string ActionCode { get; set; }
        public string Notes { get; set; }        
    }

    public class WorkOrderCode
    {
        public string ActionName { get; set; }
        public string Action { get; set; }
        public string ActionCode { get; set; }
        public System.Guid NotesTypeId { get; set; }
    }

    public class ActionTabEmail
    {
        public List<MessagesFields> Messages { get; set; }
        public string ServerId { get; set; }
        public string ApiKey { get; set; }

    }

    public class MessagesFields
    { 
        public List<ToFields> To { get; set; }
        public FromFields From { get; set; }
        public string Subject { get; set; }
        public string ApiTemplate { get; set; }
        public List<AttachmentsDocuments> Attachments { get; set; }
        public MergedData MergeData { get; set; }
        public List<CcFields> Cc { get; set; }
    }

    public class ToFields
    {
        public string EmailAddress { get; set; }
        public string FriendlyName { get; set; }
    }

    public class FromFields
    {
        public string EmailAddress { get; set; }
        public string FriendlyName { get; set; }
    }

    public class MergedData
    {
        public List<List<PerMessage>> PerMessage { get; set; }
    }

    public class PerMessage
    {
        public string Field { get; set; }
        public string Value { get; set; }
    }

    public class CcFields
    {
        public string EmailAddress { get; set; }
        public string FriendlyName { get; set; }
    }

    public class AttachmentsDocuments
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    public class ActionTabEmailOld
    {
        public System.Guid WorkOrderId { get; set; }
        public string WorkOrderNumber { get; set; }
        public string Toemail { get; set; }
        public List<string> Ccemail { get; set; }
        public string Message { get; set; }
        public System.Guid[] OtherDocument { get; set; }
        public bool ChekValue { get; set; }
        public string Notes { get; set; }
        public string ActionName { get; set; }
        public Guid ClientId { get; set; }
        public string ServerId { get; set; }
        public string ApiKey { get; set; }
        public string SocketLabsUrl { get; set; }
    }

    public class EmailData
    {
        public System.Guid WorkOrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VendorName { get; set; }
        public System.Guid ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
