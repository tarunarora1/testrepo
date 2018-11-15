using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class WorkOrderEntities
    {
        public System.Guid WorkOrderId { get; set; }
        public string WorkOrderNumber { get; set; }
        public System.Guid CreatedByUser { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid LastUpdatedByUser { get; set; }
        public System.DateTime DateLastUpdated { get; set; }
        public System.Guid ServiceRequest { get; set; }
        public System.Guid Vendor { get; set; }
        public Guid? WorkOrderAttachment { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> Status { get; set; }
        public Nullable<System.DateTime> DateArriveFrom { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public string TimeArriveFrom { get; set; }
        public Nullable<System.DateTime> DateArriveTo { get; set; }
        public string TimeArriveTo { get; set; }
        public decimal NTE { get; set; }
        public System.Guid Client { get; set; }
       
        public string VendorName { get; set; }
        public string Statusdescription { get; set; }
        public string Custref { get; set; }
        public bool? IsWorkOrderSent { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string Email { get; set; }

        public string ClientName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string Workorderprefix { get; set; }
        public int? Workorderseqnumber { get; set; }
        public string Servicetype { get; set; }
        public byte[] PDF { get; set; }
    }

    public class customerinformationforpdf
    {
        //------------------------------------------------------------Code Add By Neha for pdf Template functionality--------------------------------------------
      
        public string CustomerRefNumber { get; set; }
        public System.Guid WorkOrderId { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> Status { get; set; }
        public DateTime? DateArriveFrom { get; set; }
        public string TimeArriveFrom { get; set; }
        public Nullable<System.DateTime> DateArriveTo { get; set; }
        public string TimeArriveTo { get; set; }
        public decimal NTE { get; set; }
        public string WorkOrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string Address01 { get; set; }
        public string Address02 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip01 { get; set; }
        public string Zip02 { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string VendorName { get; set; }
        public string Attachment { get; set; }
        public string NameOfTheme { get; set; }
        public string ClientName { get; set; }
        public byte[] Logo { get; set; }
        public string IVRNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.Guid TemplateTypeID { get; set; }
        public string TemplateName { get; set; }
        //------------------------------------------------------------Code Add By Neha for pdf Template functionality--------------------------------------------
    }


    public class pdfHeaderData
    {
        public System.Guid ClientId { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }


    }
    public class AttachmentTypeData
    {
        public System.Guid ClientId { get; set; }
        public string Attachment { get; set; }
        public string TemplateName { get; set; }
        public System.Guid TemplateTypeID { get; set; }
    }

    public class LogoDetails
    { 
    public System.Guid ClientId { get; set; }
        public System.Guid LogoID { get; set; }
        public string LogoName { get; set; }
        public byte[] Logo { get; set; }
    }

    public class TemplateTypeforPDF
    {
        public System.Guid ClientId { get; set; }
        public string Attachment { get; set; }
        public string NameOfTheme { get; set; }
        public System.Guid TemplateTypeID { get; set; }
        public string TemplateName { get; set; }
    }
}
