using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities.BusinessEntityClasses;
using UnitOfWork.UOWRepository;
using RestSharp;
using Newtonsoft.Json;
using DataModel;

namespace UnitOfwork.UOWRepository
{
    public class UOWEmailModule
    {
     
        public bool SendEmail(ActionTabEmail item, Guid ClientId)
        {
            try
            {
                UserLoginRole UL = new UserLoginRole();
                string BaseUrl = "";
                List<fn_getClientResourceData_Result> EmailDetails = UL.GetResourceEmailDetails(ClientId, "Email Configuration");
                foreach (fn_getClientResourceData_Result Email in EmailDetails)
                {
                    if (Email.Name == "SocketLabsServerId")
                    {
                        item.ServerId = Email.Value;
                    }

                    if (Email.Name == "SocketLabsUrl")
                    {
                        BaseUrl = Email.Value;
                    }

                    if (Email.Name == "SocketLabsApiKey")
                    {
                        item.ApiKey = Email.Value;
                    }

                    if (Email.Name == "APITemplateKey")
                    {
                        item.Messages[0].ApiTemplate = Email.Value;
                    }

                    if (Email.Name == "EmailDefaultFrom")
                    {
                        item.Messages[0].From = new FromFields();
                        item.Messages[0].From.EmailAddress = Email.Value;
                        item.Messages[0].From.FriendlyName = "";
                    }
                }

                string EmailData = JsonConvert.SerializeObject(item);
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(Method.POST);
                request.AddParameter("application/json", EmailData, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("accept", "application/json");

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailForAction(ActionTabEmail item, Guid ClientId)
        {
            try
            {
                //UserLoginRole UL = new UserLoginRole();
                //string BaseUrl = "";
                //List<fn_getClientResourceData_Result> EmailDetails = UL.GetResourceEmailDetails(ClientId, "Email Configuration");
                //foreach (fn_getClientResourceData_Result Email in EmailDetails)
                //{
                //    if (Email.Name == "SocketLabsServerId")
                //    {
                //        item.ServerId = Email.Value;
                //    }

                //    if (Email.Name == "SocketLabsUrl")
                //    {
                //        BaseUrl = Email.Value;
                //    }

                //    if (Email.Name == "SocketLabsApiKey")
                //    {
                //        item.ApiKey = Email.Value;
                //    }

                //    if (Email.Name == "APITemplateKey")
                //    {
                //        item.Messages[0].ApiTemplate = Email.Value;
                //    }

                //    if (Email.Name == "EmailDefaultFrom")
                //    {
                //        item.Messages[0].From = new FromFields();
                //        item.Messages[0].From.EmailAddress = Email.Value;
                //        item.Messages[0].From.FriendlyName = "";
                //    }
                //}


                //string EmailData = JsonConvert.SerializeObject(item);
                //var client = new RestClient(BaseUrl);
                //var request = new RestRequest(Method.POST);
                //request.AddParameter("application/json", EmailData, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailToCustomer(ActionTabEmail item, Guid ClientId)
        {
            try
            {
                //UserLoginRole UL = new UserLoginRole();
                //string BaseUrl = "";
                //List<fn_getClientResourceData_Result> EmailDetails = UL.GetResourceEmailDetails(ClientId, "Email Configuration");
                //foreach (fn_getClientResourceData_Result Email in EmailDetails)
                //{
                //    if (Email.Name == "SocketLabsServerId")
                //    {
                //        item.ServerId = Email.Value;
                //    }

                //    if (Email.Name == "SocketLabsUrl")
                //    {
                //        BaseUrl = Email.Value;
                //    }

                //    if (Email.Name == "SocketLabsApiKey")
                //    {
                //        item.ApiKey = Email.Value;
                //    }

                //    if (Email.Name == "APITemplateKey")
                //    {
                //        item.Messages[0].ApiTemplate = Email.Value;
                //    }

                //    if (Email.Name == "EmailDefaultFrom")
                //    {
                //        item.Messages[0].From = new FromFields();
                //        item.Messages[0].From.EmailAddress = Email.Value;
                //        item.Messages[0].From.FriendlyName = "";
                //    }
                //}


                //string EmailData = JsonConvert.SerializeObject(item);
                //var client = new RestClient(BaseUrl);
                //var request = new RestRequest(Method.POST);
                //request.AddParameter("application/json", EmailData, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendEmailForNewClientUsers(EmailEntity Email)
        {
            try
            {
                //ActionTabEmail actionTabEmail = new ActionTabEmail();

                ////1. ) Adding "to" Fields in Message Fields.
                //ToFields toFields = new ToFields();
                //toFields.EmailAddress = "%%DeliveryAddress%%";
                //toFields.FriendlyName = "";

                //List<ToFields> toFieldsList = new List<ToFields>();
                //toFieldsList.Add(toFields);
                //MessagesFields messagesFields = new MessagesFields();
                //messagesFields.To = toFieldsList;


                //List<string> emailAddresses = new List<string>();
                //emailAddresses.Add(Email.ToAddress);                
                //List<EmailDataForQuoteInvoice> EmailDataforUser = GetEmailDataforUser(Email.UserId, Email.IsClientVendorOrCustomer);              

                //List<EmailDataForClientDetails> EmailDataforClients = GetEmailDataforClientDetails(Email.LoggedInUserId);
                //UOWWorkOrder uOWWorkOrder = new UOWWorkOrder();
                //List<fn_getClientResourceData_Result> HeaderData = uOWWorkOrder.DataforPDfHeader(Email.ClientId);
                //List<List<PerMessage>> perMessagesArrayList = new List<List<PerMessage>>();

                //MergedData mergedData = new MergedData();
                //foreach (string e in emailAddresses)
                //{
                //    List<PerMessage> perMessagesList = new List<PerMessage>();

                //    PerMessage perMessageDeliveryAddress = new PerMessage();
                //    perMessageDeliveryAddress.Field = "DeliveryAddress";
                //    perMessageDeliveryAddress.Value = e;                  

                //    PerMessage perMessageVendorName = new PerMessage();
                //    perMessageVendorName.Field = "VendorName";
                //    perMessageVendorName.Value = EmailDataforUser[0].FirstName;

                //    PerMessage perMessageCSRName = new PerMessage();
                //    perMessageCSRName.Field = "CSRName";
                //    perMessageCSRName.Value = EmailDataforClients[0].FirstName + " " + EmailDataforClients[0].LastName;

                //    PerMessage perMessageCompanyName = new PerMessage();
                //    perMessageCompanyName.Field = "CompanyName";
                //    perMessageCompanyName.Value = EmailDataforClients[0].ClientName;

                //    PerMessage perMessageCompanyName2 = new PerMessage();
                //    perMessageCompanyName2.Field = "CompanyName2";
                //    perMessageCompanyName2.Value = EmailDataforClients[0].ClientName;

                //    PerMessage perMessageAddress01 = new PerMessage(); ;
                //    PerMessage perMessageAddress02 = new PerMessage();
                //    PerMessage perMessageTelephone = new PerMessage();
                //    PerMessage perMessageFax = new PerMessage();
                //    PerMessage perMessageURL = new PerMessage();

                //    foreach (fn_getClientResourceData_Result headerData in HeaderData)
                //    {
                //        if (headerData.Name == "WOHeaderLine01")
                //        {
                //            perMessageAddress01.Field = "Address01";
                //            perMessageAddress01.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine02")
                //        {

                //            perMessageAddress02.Field = "Address02";
                //            perMessageAddress02.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine03")
                //        {

                //            perMessageTelephone.Field = "Telephone";
                //            perMessageTelephone.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine04")
                //        {

                //            perMessageFax.Field = "Fax";
                //            perMessageFax.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine05")
                //        {

                //            perMessageURL.Field = "URL";
                //            perMessageURL.Value = headerData.Value;
                //        }
                //    }
                //    PerMessage perMessageMessage = new PerMessage();
                //    perMessageMessage.Field = "Message";
                //    perMessageMessage.Value = Email.Body;



                //    perMessagesList.Add(perMessageDeliveryAddress);
                //    perMessagesList.Add(perMessageVendorName);
                //    perMessagesList.Add(perMessageCSRName);
                //    perMessagesList.Add(perMessageCompanyName);
                //    perMessagesList.Add(perMessageCompanyName2);
                //    perMessagesList.Add(perMessageAddress01);
                //    perMessagesList.Add(perMessageAddress02);
                //    perMessagesList.Add(perMessageTelephone);
                //    perMessagesList.Add(perMessageFax);
                //    perMessagesList.Add(perMessageURL);
                //    perMessagesList.Add(perMessageMessage);

                //    perMessagesArrayList.Add(perMessagesList);
                //    mergedData.PerMessage = perMessagesArrayList;
                //}

                //messagesFields.MergeData = mergedData;
                //// messagesFields.Attachments = DocsFile;
                //messagesFields.Subject = Email.Subject;

                //List<MessagesFields> messagesFieldsList = new List<MessagesFields>();
                //messagesFieldsList.Add(messagesFields);
                //actionTabEmail.Messages = messagesFieldsList;

                //UserLoginRole UL = new UserLoginRole();
                //string BaseUrl = "";
                //List<fn_getClientResourceData_Result> EmailDetails = UL.GetResourceEmailDetails(Email.ClientId, "Email Configuration");
                //foreach (fn_getClientResourceData_Result Email1 in EmailDetails)
                //{
                //    if (Email1.Name == "SocketLabsUrl")
                //    {
                //        BaseUrl = Email1.Value;
                //    }
                //    else if (Email1.Name == "SocketLabsServerId")
                //    {
                //        actionTabEmail.ServerId = Email1.Value;
                //    }
                //    else if (Email1.Name == "EmailDefaultFrom")
                //    {
                //        actionTabEmail.Messages[0].From = new FromFields();
                //        actionTabEmail.Messages[0].From.EmailAddress = Email1.Value;
                //        actionTabEmail.Messages[0].From.FriendlyName = "";
                //    }
                //    else if (Email1.Name == "SocketLabsApiKey")
                //    {
                //        actionTabEmail.ApiKey = Email1.Value;
                //    }
                //    else if (Email1.Name == "APITemplateKey")
                //    {
                //        actionTabEmail.Messages[0].ApiTemplate = Email1.Value;
                //    }

                //}

                //string EmailDataFinal = JsonConvert.SerializeObject(actionTabEmail);
                //var client = new RestClient(BaseUrl);
                //var request = new RestRequest(Method.POST);
                //request.AddParameter("application/json", EmailDataFinal, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<EmailDataForQuoteInvoice> GetEmailDataforUser(Guid UserId, string IsClientVendorOrCustomer)
        {
            try
            {
                //List<EmailDataForQuoteInvoice> EmailData1 = new List<EmailDataForQuoteInvoice>();
                //using (FacilitiesEntities db = new FacilitiesEntities())
                //{

                //    if (IsClientVendorOrCustomer == "Customer")
                //    {

                //        EmailData1 = (from cu in db.CustomerUsers
                //                      join u in db.Users on cu.User equals u.UserId
                //                      where u.UserId == UserId
                //                      select new
                //                      {
                //                          FirstName = u.FirstName,
                //                          LastName = u.LastName,
                //                      }).ToList().Select(W => new EmailDataForQuoteInvoice()
                //                      {
                //                          FirstName = W.FirstName,
                //                          LastName = W.LastName,
                //                      }).ToList();

                //    }


                //    else if (IsClientVendorOrCustomer == "Client")
                //    {
                //        EmailData1 = (from cu in db.ClientUsers
                //                      join cl in db.Clients on cu.Client equals cl.ClientId
                //                      join u in db.Users on cu.User equals u.UserId
                //                      where u.UserId == UserId
                //                      select new
                //                      {
                //                          FirstName = u.FirstName,
                //                          LastName = u.LastName,
                //                          ClientName = cl.ClientName
                //                      }).ToList().Select(W => new EmailDataForQuoteInvoice()
                //                      {
                //                          FirstName = W.FirstName,
                //                          LastName = W.LastName,
                //                          ClientName = W.ClientName
                //                      }).ToList();

                //    }

                //    else if (IsClientVendorOrCustomer == "Vendor")
                //    {
                //        EmailData1 = (from cu in db.VendorUsers
                //                      join v in db.Vendors on cu.Vendor equals v.VendorId
                //                      join u in db.Users on cu.User equals u.UserId
                //                      where u.UserId == UserId
                //                      select new
                //                      {
                //                          FirstName = u.FirstName,
                //                          LastName = u.LastName,
                //                      }).ToList().Select(W => new EmailDataForQuoteInvoice()
                //                      {
                //                          FirstName = W.FirstName,
                //                          LastName = W.LastName,
                //                      }).ToList();


                //    }
                //    return EmailData1;
                //}
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool SendEmailForNewCustomerUsers(EmailEntity Email)
        {
            try
            {
                //ActionTabEmail actionTabEmail = new ActionTabEmail();
                ////1. ) Adding "to" Fields in Message Fields.
                //ToFields toFields = new ToFields();
                //toFields.EmailAddress = "%%DeliveryAddress%%";
                //toFields.FriendlyName = "";

                //List<ToFields> toFieldsList = new List<ToFields>();
                //toFieldsList.Add(toFields);
                //MessagesFields messagesFields = new MessagesFields();
                //messagesFields.To = toFieldsList;


                //List<string> emailAddresses = new List<string>();
                //emailAddresses.Add(Email.ToAddress);

                //List<EmailDataForQuoteInvoice> EmailData = GetEmailDataforUser(Email.UserId, Email.IsClientVendorOrCustomer);
                //List<EmailDataForClientDetails> EmailDataforClients = GetEmailDataforClientDetails(Email.LoggedInUserId);

                //UOWWorkOrder uOWWorkOrder = new UOWWorkOrder();
                //List<fn_getClientResourceData_Result> HeaderData = uOWWorkOrder.DataforPDfHeader(Email.ClientId);
                //List<List<PerMessage>> perMessagesArrayList = new List<List<PerMessage>>();

                //MergedData mergedData = new MergedData();
                //foreach (string e in emailAddresses)
                //{
                //    List<PerMessage> perMessagesList = new List<PerMessage>();

                //    PerMessage perMessageDeliveryAddress = new PerMessage();
                //    perMessageDeliveryAddress.Field = "DeliveryAddress";
                //    perMessageDeliveryAddress.Value = e;

                //    PerMessage perMessageVendorName = new PerMessage();
                //    perMessageVendorName.Field = "VendorName";
                //    perMessageVendorName.Value = EmailData[0].FirstName;

                //    PerMessage perMessageCSRName = new PerMessage();
                //    perMessageCSRName.Field = "CSRName";
                //    perMessageCSRName.Value = EmailDataforClients[0].FirstName + " " + EmailDataforClients[0].LastName;

                //    PerMessage perMessageCompanyName = new PerMessage();
                //    perMessageCompanyName.Field = "CompanyName";
                //    perMessageCompanyName.Value = EmailDataforClients[0].ClientName;

                //    PerMessage perMessageCompanyName2 = new PerMessage();
                //    perMessageCompanyName2.Field = "CompanyName2";
                //    perMessageCompanyName2.Value = EmailDataforClients[0].ClientName;

                //    PerMessage perMessageAddress01 = new PerMessage(); ;
                //    PerMessage perMessageAddress02 = new PerMessage();
                //    PerMessage perMessageTelephone = new PerMessage();
                //    PerMessage perMessageFax = new PerMessage();
                //    PerMessage perMessageURL = new PerMessage();

                //    foreach (fn_getClientResourceData_Result headerData in HeaderData)
                //    {
                //        if (headerData.Name == "WOHeaderLine01")
                //        {
                //            perMessageAddress01.Field = "Address01";
                //            perMessageAddress01.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine02")
                //        {

                //            perMessageAddress02.Field = "Address02";
                //            perMessageAddress02.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine03")
                //        {

                //            perMessageTelephone.Field = "Telephone";
                //            perMessageTelephone.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine04")
                //        {

                //            perMessageFax.Field = "Fax";
                //            perMessageFax.Value = headerData.Value;
                //        }
                //        if (headerData.Name == "WOHeaderLine05")
                //        {

                //            perMessageURL.Field = "URL";
                //            perMessageURL.Value = headerData.Value;
                //        }
                //    }
                //    PerMessage perMessageMessage = new PerMessage();
                //    perMessageMessage.Field = "Message";
                //    perMessageMessage.Value = Email.Body;



                //    perMessagesList.Add(perMessageDeliveryAddress);
                //    perMessagesList.Add(perMessageVendorName);
                //    perMessagesList.Add(perMessageCSRName);
                //    perMessagesList.Add(perMessageCompanyName);
                //    perMessagesList.Add(perMessageCompanyName2);
                //    perMessagesList.Add(perMessageAddress01);
                //    perMessagesList.Add(perMessageAddress02);
                //    perMessagesList.Add(perMessageTelephone);
                //    perMessagesList.Add(perMessageFax);
                //    perMessagesList.Add(perMessageURL);
                //    perMessagesList.Add(perMessageMessage);

                //    perMessagesArrayList.Add(perMessagesList);
                //    mergedData.PerMessage = perMessagesArrayList;
                //}

                //messagesFields.MergeData = mergedData;
                //// messagesFields.Attachments = DocsFile;
                //messagesFields.Subject = Email.Subject;

                //List<MessagesFields> messagesFieldsList = new List<MessagesFields>();
                //messagesFieldsList.Add(messagesFields);
                //actionTabEmail.Messages = messagesFieldsList;

                //UserLoginRole UL = new UserLoginRole();
                //string BaseUrl = "";
                //List<fn_getClientResourceData_Result> EmailDetails = UL.GetResourceEmailDetails(Email.ClientId, "Email Configuration");
                //foreach (fn_getClientResourceData_Result Email1 in EmailDetails)
                //{
                //    if (Email1.Name == "SocketLabsUrl")
                //    {
                //        BaseUrl = Email1.Value;
                //    }
                //    else if (Email1.Name == "SocketLabsServerId")
                //    {
                //        actionTabEmail.ServerId = Email1.Value;
                //    }
                //    else if (Email1.Name == "EmailDefaultFrom")
                //    {
                //        actionTabEmail.Messages[0].From = new FromFields();
                //        actionTabEmail.Messages[0].From.EmailAddress = Email1.Value;
                //        actionTabEmail.Messages[0].From.FriendlyName = "";
                //    }
                //    else if (Email1.Name == "SocketLabsApiKey")
                //    {
                //        actionTabEmail.ApiKey = Email1.Value;
                //    }
                //    else if (Email1.Name == "APITemplateKey")
                //    {
                //        actionTabEmail.Messages[0].ApiTemplate = Email1.Value;
                //    }

                //}

                //string EmailDataFinal = JsonConvert.SerializeObject(actionTabEmail);
                //var client = new RestClient(BaseUrl);
                //var request = new RestRequest(Method.POST);
                //request.AddParameter("application/json", EmailDataFinal, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);

                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<EmailDataForClientDetails> GetEmailDataforClientDetails(Guid LoggedInUserId)
        {
            try
            {
                //using (FacilitiesEntities db = new FacilitiesEntities())
                //{
                //    var EmailDataforClients = (from cu in db.ClientUsers
                //                               join cl in db.Clients on cu.Client equals cl.ClientId
                //                               join u in db.Users on cu.User equals u.UserId
                //                               where u.UserId == LoggedInUserId
                //                               select new
                //                               {
                //                                   FirstName = u.FirstName,
                //                                   LastName = u.LastName,
                //                                   ClientName = cl.ClientName
                //                               }).ToList().Select(W => new EmailDataForClientDetails()
                //                               {
                //                                   FirstName = W.FirstName,
                //                                   LastName = W.LastName,
                //                                   ClientName = W.ClientName
                //                               }).ToList();
                //    return EmailDataforClients;
                //}
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool ForgotpasswordSendEmail(EmailEntity Email)
        {
            bool IsEmailSent = false;
            try
            {
                //UserLoginRole UL = new UserLoginRole();
                //List<fn_getClientResourceData_Result> EmailDetails = UL.GetResourceEmailDetails(Email.ClientId, "Email Configuration");
                //foreach (fn_getClientResourceData_Result Email1 in EmailDetails)
                //{
                //    if (Email1.Name == "SocketLabsUrl")
                //    {
                //        SocketLabsUrl = Email1.Value;
                //    }
                //    else if (Email1.Name == "SocketLabsServerId")
                //    {
                //        SocketLabsServerId = Email1.Value;
                //    }
                //    else if (Email1.Name == "EmailDefaultFrom")
                //    {
                //        EmailDefaultFrom = Email1.Value;
                //    }
                //    else if (Email1.Name == "SocketLabsApiKey")
                //    {
                //        SocketLabsApiKey = Email1.Value;
                //    }
                //}
                //var client = new RestClient(SocketLabsUrl);
                //var request = new RestRequest(Method.POST);
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");
                //string EmailData = "{\r\n    \"ServerId\": \"" + SocketLabsServerId + "\",\r\n    \"ApiKey\": \"" + SocketLabsApiKey + "\",\r\n    \"Messages\": [{\r\n        \"To\": [{\r\n            \"EmailAddress\": \"" + Email.ToAddress + "\"}],\r\n        \"From\": {\r\n            \"EmailAddress\": \"" + EmailDefaultFrom + "\",\r\n            \"FriendlyName\": \"" + EmailDefaultFrom + "\"\r\n        },\r\n        \"Subject\": \"" + Email.Subject + "\",\r\n        \"TextBody\": \"null\",\r\n        \"HtmlBody\": \"" + Email.Body + "\",\r\n}";
                //request.AddParameter("application/json", EmailData, ParameterType.RequestBody);
                //IRestResponse response = client.Execute(request);
                IsEmailSent = true; ;
            }
            catch (Exception ex)
            {
                IsEmailSent = false;
            }
            return IsEmailSent;
        }
    }
}
