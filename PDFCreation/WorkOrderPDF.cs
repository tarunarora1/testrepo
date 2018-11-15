using System;
using System.IO;
using iTextSharp.text.pdf;
using BusinessEntities.BusinessEntityClasses;
using System.Collections.Generic;
using iTextSharp.text;
using DataModel;

namespace PDFCreation
{
    public class WorkOrderPDF
    {        
        public byte[] ReadPDFTemplate(List<customerinformationforpdf> listData, List<fn_getClientResourceData_Result> pdfHeaderData, IList<LogoDetails> LogoData)
        {
            byte[] bytes2;
            
            byte[] imageBytes = Convert.FromBase64String(listData[0].Attachment);
   
            PdfReader reader = new PdfReader(imageBytes);
            bytes2 = BindDataToPDFTemplate(listData, reader, pdfHeaderData, LogoData);
            return bytes2;
        }

        public byte[] ReadQuotePDFTemplate(IList<customerinformationforpdf> listData, List<fn_getClientResourceData_Result> pdfHeaderData, List<AttachmentTypeData> WOInvoiceData, List<ServiceInvoice> item,string InvoiceNumber,DateTime DateOfInvoice, IList<LogoDetails> LogoData)
        {
            byte[] bytes2;            
            byte[] imageBytes = Convert.FromBase64String(WOInvoiceData[0].Attachment);
            PdfReader reader = new PdfReader(imageBytes);
            bytes2 = BindDataToQuotePDFTemplate(listData, reader, pdfHeaderData, WOInvoiceData,item, InvoiceNumber, DateOfInvoice, LogoData);
            return bytes2;
        }

        private byte[] BindDataToQuotePDFTemplate(IList<customerinformationforpdf> listdata, PdfReader readPath, List<fn_getClientResourceData_Result> pdfHeaderData, List<AttachmentTypeData> WOInvoiceData, List<ServiceInvoice> item2, string InvoiceNumber,DateTime DateOfInvoice, IList<LogoDetails> LogoData)
        {
            byte[] bytes;
            using (MemoryStream memStream = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(readPath, memStream, '\0', true))
                {
                    var form = stamper.AcroFields;
                    var fieldKeys = form.Fields.Keys;

                        foreach (string fieldKey in fieldKeys)
                        {
                            if (fieldKey == "address_1")
                                foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                                {
                                    if (item.Name == "WOHeaderLine01")
                                    {
                                        form.SetField(fieldKey, item.Value);
                                    }
                                }
                            if (fieldKey == "stateName_1")
                                foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                                {
                                    if (item.Name == "WOHeaderLine02")
                                    {
                                        form.SetField(fieldKey, item.Value);
                                    }
                                }

                            if (fieldKey == "telephone_1")
                                foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                                {
                                    if (item.Name == "WOHeaderLine03")
                                    {
                                        form.SetField(fieldKey, item.Value);
                                    }
                                }
                            if (fieldKey == "fax_1")
                                foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                                {
                                    if (item.Name == "WOHeaderLine04")
                                    {
                                        form.SetField(fieldKey, item.Value);
                                    }
                                }
                            if (fieldKey == "siteAddress_1")
                                foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                                {
                                    if (item.Name == "WOHeaderLine05")
                                    {
                                        form.SetField(fieldKey, item.Value);
                                    }
                                }
                        //Data for the Table in PDF Template

                        //Data to bind Activity Column Data
                        if (fieldKey == "Material_Activity")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "MATERIALS" && i.SNotes != null)
                                {
                                    form.SetField(fieldKey, i.SNotes + " " + i.Name);
                                }
                            }
                        if (fieldKey == "Trip_Activity")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "TRIP" && i.SNotes != null)
                                {
                                    form.SetField(fieldKey, i.SNotes + " " + i.Name);
                                }
                            }
                        if (fieldKey == "Labour_Activity")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "Labor" && i.SNotes != null)
                                {
                                    form.SetField(fieldKey, i.SNotes + " " + i.Name);
                                }
                            }

                        //Data to bind Amount Column
                        if (fieldKey == "Material_Amount")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "MATERIALS")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.SAmount)));
                                    }
                                }
                            }
                        if (fieldKey == "Trip_Amount")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "TRIP")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.SAmount)));
                                    }
                                }
                            }
                        if (fieldKey == "Labour_Amount")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "Labor")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.SAmount)));
                                    }
                                }
                            }

                        // Data to bind Estimated Tax Column
                        if (fieldKey == "Material_ESTax")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "MATERIALS")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.STax)));
                                    }
                                }
                            }
                        if (fieldKey == "Trip_ESTax")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "TRIP")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.STax)));
                                    }
                                }
                            }
                        if (fieldKey == "Labour_ESTax")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "Labor")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.STax)));
                                    }
                                }
                            }

                        // Data to bind Total column data
                        if (fieldKey == "Material_Total")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "MATERIALS")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.STotal)));
                                    }
                                }
                            }
                        if (fieldKey == "Trip_Total")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "TRIP")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.STotal)));
                                    }
                                }
                            }
                        if (fieldKey == "Labour_Total")
                            foreach (ServiceInvoice i in item2)
                            {
                                if (i.Name == "Labor")
                                {
                                    if (i.STotal != 0)
                                    {
                                        form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", i.STotal)));
                                    }
                                }
                            }
                        ////End of table data 


                        if (fieldKey == "description")
                            foreach (ServiceInvoice i in item2)
                            {
                                form.SetField(fieldKey, i.Description);
                            }
                        if (fieldKey == "HeaderForQuote&Invoice")
                            foreach (ServiceInvoice i in item2)
                            {
                                form.SetField(fieldKey, i.ModalStatus);
                                break;
                            }
                        if (fieldKey == "total_2")
                            form.SetField(fieldKey, "$" + Convert.ToString(string.Format("{0:0.00}", item2[0]._TotalAmountAfterTaxForAllTypes)));
                        if (fieldKey == "serviceCall_1")
                            form.SetField(fieldKey, InvoiceNumber);
                        if (fieldKey == "date_1")
                            form.SetField(fieldKey, DateOfInvoice.ToString("MM/dd/yyyy"));
                        if (fieldKey == "billTo")
                            form.SetField(fieldKey, listdata[0].CustomerName);
                        if (fieldKey == "poNumber_1")
                            form.SetField(fieldKey, listdata[0].CustomerRefNumber);
                        if (fieldKey == "address_2")
                            form.SetField(fieldKey, listdata[0].Address01 + listdata[0].Address02);
                        if (fieldKey == "address_3")
                            form.SetField(fieldKey, listdata[0].City + " " + listdata[0].State + " " + listdata[0].Zip01 + " " + listdata[0].Zip02);

                        PushbuttonField ad = form.GetNewPushbuttonFromField("submitButton_1");
                        ad.Layout = PushbuttonField.LAYOUT_ICON_ONLY;
                        ad.ProportionalIcon = true;
                       ad.Image = Image.GetInstance(LogoData[0].Logo);
                        form.ReplacePushbuttonField("submitButton_1", ad.Field);


                    }

                    // "Flatten" the form so it wont be editable/usable anymore
                    stamper.FormFlattening = false;
                    form.GenerateAppearances = false;

                    // You can also specify fields to be flattened, which
                    // leaves the rest of the form still be editable/usable
                    //stamper.PartialFormFlattening("field1");

                    stamper.Close();
                }
                //UOWWorkOrder wo = new UOWWorkOrder();
                //wo.ConvertByteToMemory(memStream, listdata);
                bytes = memStream.ToArray();
                return bytes;
            }
        }

        private byte[] BindDataToPDFTemplate(List<customerinformationforpdf> listdata, PdfReader readPath, List<fn_getClientResourceData_Result> pdfHeaderData, IList<LogoDetails> LogoData)
        {
            byte[] bytes;
            using (MemoryStream memStream = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(readPath, memStream, '\0', true))
                {
                    var form = stamper.AcroFields;
                    var fieldKeys = form.Fields.Keys;
                
                    //Page 1 of PDF
                    foreach (string fieldKey in fieldKeys)
                    {
                        if (fieldKey == "currentAddress_1")
                            foreach(fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if(item.Name == "WOHeaderLine01")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "currentAddress_2")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine02")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                      
                        if (fieldKey == "telephone_1")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine03")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "fax_1")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine04")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "websiteUrl_1")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine05")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "workOrderNumber_1")
                            form.SetField(fieldKey, listdata[0].WorkOrderNumber);
                        if (fieldKey == "currentDate_1")
                            form.SetField(fieldKey, Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")));
                        if (fieldKey == "csr_1")
                            form.SetField(fieldKey, listdata[0].FirstName+" "+ listdata[0].LastName);
                        if (fieldKey == "to")
                            form.SetField(fieldKey, listdata[0].VendorName);
                        if (fieldKey == "clientName")
                            form.SetField(fieldKey, listdata[0].ClientName);
                        if (fieldKey == "serviceCall_1")
                            form.SetField(fieldKey, listdata[0].WorkOrderNumber);
                        if (fieldKey == "workOrderNumber_2")
                            form.SetField(fieldKey, listdata[0].CustomerRefNumber);
                        if (fieldKey == "amount_1")
                            form.SetField(fieldKey, Convert.ToString("$" + listdata[0].NTE));
                        if (fieldKey == "store_name")
                            form.SetField(fieldKey, listdata[0].CustomerName);
                        if (fieldKey == "storeAddress_1")
                            form.SetField(fieldKey, listdata[0].Address01);
                        if (fieldKey == "storeAddress_2")
                            form.SetField(fieldKey, listdata[0].City + " " + listdata[0].State + " " + listdata[0].Zip01);
                        if (fieldKey == "siteinfo")
                            form.SetField(fieldKey, "siteinfo");
                        if (fieldKey == "telephone_2")
                            form.SetField(fieldKey, listdata[0].Telephone);
                        if (fieldKey == "contact")
                            form.SetField(fieldKey, "contact");                      
                        if (fieldKey == "currentDate_3")
                            form.SetField(fieldKey,listdata[0].DateArriveFrom.Value.ToString("MM/dd/yyyy"));
                        if (fieldKey == "currentTime_2")
                            form.SetField(fieldKey, listdata[0].TimeArriveFrom);
                        if (fieldKey == "currentDate_4")
                            form.SetField(fieldKey, listdata[0].DateArriveTo.Value.ToString("MM/dd/yyyy"));
                        if (fieldKey == "currentTime_3")
                            form.SetField(fieldKey, Convert.ToString(listdata[0].TimeArriveTo));
                        if (fieldKey == "description")
                            form.SetField(fieldKey, listdata[0].Description);
                        if (fieldKey == "IVRNumber_1")
                            form.SetField(fieldKey, listdata[0].IVRNumber);
                        if (fieldKey == "IVRNumber_2")
                            form.SetField(fieldKey, listdata[0].IVRNumber);
                        // Page 2 of PDF

                        if (fieldKey == "currentAddress_3")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine01")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "currentAddress_4")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine02")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }

                        if (fieldKey == "telephone_3")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine03")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "fax_2")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine04")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                        if (fieldKey == "websiteUrl_2")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine05")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                         if (fieldKey == "serviceCall_2")
                            form.SetField(fieldKey, listdata[0].WorkOrderNumber);
                            if (fieldKey == "clientName_2")
                            form.SetField(fieldKey, listdata[0].ClientName);
                        if (fieldKey == "storeName_2")
                            form.SetField(fieldKey, listdata[0].CustomerName);
                        if (fieldKey == "storeAddress_3")
                            form.SetField(fieldKey, listdata[0].Address01 + " " + listdata[0].City + " " + listdata[0].State + " " + listdata[0].Zip01);
                        if (fieldKey == "workOrderNumber_3")
                            form.SetField(fieldKey, listdata[0].CustomerRefNumber);
                        if (fieldKey == "telephone_4")
                            form.SetField(fieldKey, listdata[0].Telephone);
                        if (fieldKey == "clientName_4")
                            form.SetField(fieldKey, listdata[0].ClientName);
                        if (fieldKey == "clientName_3")
                            form.SetField(fieldKey, listdata[0].ClientName);
                        
                          if (fieldKey == "telephone_5")
                            foreach (fn_getClientResourceData_Result item in pdfHeaderData)
                            {
                                if (item.Name == "WOHeaderLine03")
                                {
                                    form.SetField(fieldKey, item.Value);
                                }
                            }
                    }

                    PushbuttonField ad = form.GetNewPushbuttonFromField("SubmitButton1");
                    ad.Layout = PushbuttonField.LAYOUT_ICON_ONLY;
                    ad.ProportionalIcon = true;
                   ad.Image = Image.GetInstance(LogoData[0].Logo);
                    form.ReplacePushbuttonField("SubmitButton1", ad.Field);

                    PushbuttonField ad2 = form.GetNewPushbuttonFromField("SubmitButton2");
                    ad2.Layout = PushbuttonField.LAYOUT_ICON_ONLY;
                    ad2.ProportionalIcon = true;
                    ad2.Image = Image.GetInstance(LogoData[0].Logo);
                    form.ReplacePushbuttonField("SubmitButton2", ad2.Field);

                    // "Flatten" the form so it wont be editable/usable anymore
                    stamper.FormFlattening = false;
                    form.GenerateAppearances = false;
                    // You can also specify fields to be flattened, which
                    // leaves the rest of the form still be editable/usable                    
                    stamper.Close();
                }
                
                bytes = memStream.ToArray();
                return bytes;
            }
        }
    }
}
