using BusinessEntities.BusinessEntityClasses;
using FacilitiesServices.Filters;
using System;
using System.Collections.Generic;
using System.Web.Http;
using UnitOfwork.Interfaces;
using UnitOfwork.UOWRepository;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/ManageUsers")]
    public class ManageUsersController : ApiController
    {
        IManageUsers repository = new UOWManageUsers();

        [HttpGet]
        [Route("GetClientUsers")]
        public List<ClientUsersEntity> GetClientUsers(Guid LoggedInUser)
        {
            return repository.GetClientUsers(LoggedInUser);
        }

        [HttpGet]
        [Route("GetClientCustomersUsers")]
        public List<ClientCustomerUsersEntity> GetClientCustomersUsers(Guid LoggedInUser)
        {
            return repository.GetClientCustomersUsers(LoggedInUser);
        }

        [HttpGet]
        [Route("GetUserOktaUserID")]
        public string GetUserOktaUserID(string Email)
        {
            return repository.GetUserOktaUserID(Email);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public bool ChangePassword(ChangePasswordEntity item)
        {
            return repository.ChangePassword(item);
        }
    }
}