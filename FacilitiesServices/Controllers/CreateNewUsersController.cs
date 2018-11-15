using BusinessEntities.BusinessEntityClasses;
using System;
using System.Collections.Generic;
using System.Web.Http;
using UnitOfwork.Interfaces;
using UnitOfwork.UOWRepository;
using DataModel;
using BussinessEntities.BusinessEntityClasses;
using FacilitiesServices.Filters;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/CreateNewUsers")]
    public class CreateNewUsersController : ApiController
    {
        ICreateNewUsers repository = new UOWCreateNewUsers();

        [HttpPost]
        [Route("AddNewCustomerRequest")]
        public Guid AddNewCustomerRequest(CreateNewUsersEntity NewCustomer)
        {
            return repository.AddNewCustomerRequest(NewCustomer);
        }

        [HttpPost]
        [Route("AddNewClientRequest")]
        public Guid AddNewClientRequest(CreateNewUsersEntity NewClient)
        {
            return repository.AddNewClientRequest(NewClient);
        }        

        [HttpGet]
        [Route("GetUserEmailId")]
        public string GetUserEmailId(Guid UserId)
        {
            return repository.GetUserEmailId(UserId);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetLinkExpiyTime")]
        public Tuple<bool, string, string> GetLinkExpiyTime(string RandamString)
        {
            return repository.GetLinkExpiyTime(RandamString);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetLinkDetailData")]
        public List<ValueEntity> GetLinkDetailData(string RandamString)
        {
            return repository.GetLinkDetailData(RandamString);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ActivateRegistrationUsers")]
        public bool ActivateRegistrationUsers(User item)
        {
            return repository.ActivateRegistrationUsers(item);
        }
		[HttpGet]
        [Route("GetCustomerRoles")]
        public IEnumerable<RoleEntities> GetCustomerRoles()
        {
            return repository.GetCustomerRoles();
        }

        [HttpPost]
        [Route("SaveCustomerUsers")]
        public Guid SaveCustomerUsers(CustomerUsers item)
        {
            return repository.SaveCustomerUsers(item);
        }

        [HttpGet]
        [Route("GetCustomerUserGridData")]
        public IEnumerable<ClientCustomerUsersEntity> GetCustomerUserGridData(Guid CustomerId)
        {
            return repository.GetCustomerUserGridData(CustomerId);
        }

        [HttpGet]
        [Route("GetLocationWithAddress")]
        public List<CustomerLocationEntity> GetLocationWithAddress(Guid CustomerId)
        {
            return repository.GetLocationWithAddress(CustomerId);
        }

        [HttpGet]
        [Route("BindCustomerLocation")]
        public IEnumerable<BindLocationList> BindCustomerLocation(Guid Userid)
        {
            return repository.BindCustomerLocation(Userid);
        }

        [HttpGet]
        [Route("GetAssociateCustomerLocationusers")]
        public IEnumerable<GetCustomerLocationUsers_Result> GetAssociateCustomerLocationusers(Guid LocationId)
        {
            return repository.GetAssociateCustomerLocationusers(LocationId);
        }

        [HttpGet]
        [Route("GetAssociateCustomerLocationusers1")]
        public IEnumerable<GetCustomerEmailAddressForInvoiceQuote_Result> GetAssociateCustomerLocationusers1(Guid LocationId)
        {
            return repository.GetAssociateCustomerLocationusers1(LocationId);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RegisterUser")]
        public object RegisterUser(RegisterUserEntity item)
        {
            return repository.RegisterUser(item);
        }

        [HttpPost]
        [Route("RegisterUserData")]
        public bool RegisterUserData(RegisterDataLInkEntity item)
        {
            return repository.RegisterUserData(item);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("DeleteDataFromLinkTables")]
        public string DeleteDataFromLinkTables(Guid LinkHeaderID)
        {
            return repository.DeleteDataFromLinkTables(LinkHeaderID);
        }
        //[HttpGet]
        //[Route("ConvertLogoToByte")]
        //public bool ConvertLogoToByte(Guid LoggedInUser)
        //{
        //    return repository.ConvertLogoToByte(LoggedInUser);
        //}
    }
}
