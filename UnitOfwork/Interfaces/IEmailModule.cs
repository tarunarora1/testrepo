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

namespace UnitOfwork.Interfaces
{
    public interface IEmailModule
    {
        bool SendEmail(ActionTabEmail item, Guid ClientId);

        bool SendEmailForAction(ActionTabEmail item, Guid ClientId);

        bool SendEmailToCustomer(ActionTabEmail item, Guid ClientId);

        bool SendEmailForNewClientUsers(EmailEntity Email);

        List<EmailDataForQuoteInvoice> GetEmailDataforUser(Guid UserId, string IsClientVendorOrCustomer);

        bool SendEmailForNewCustomerUsers(EmailEntity Email);

        List<EmailDataForClientDetails> GetEmailDataforClientDetails(Guid LoggedInUserId);

        bool ForgotpasswordSendEmail(EmailEntity Email);
    }
}
