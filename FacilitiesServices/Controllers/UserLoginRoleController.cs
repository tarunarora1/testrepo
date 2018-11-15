using System;
using System.Collections.Generic;
using System.Web.Http;
using UnitOfWork.Interface;
using UnitOfWork.UOWRepository;
using FacilitiesServices.Filters;
using BusinessEntities.BusinessEntityClasses;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/LoginUserRole")]
    public class UserLoginRoleController : ApiController
    {
        IUserLogin repository = new UserLoginRole();

        [HttpGet]
        [Route("GetLoginUserRole")]
        public List<KeyValuePair<Guid, string>> GetLoginUserRole(string UserEmailId, string application = "")
        {
            return repository.GetLoginUserRole(UserEmailId, application);
        }

        [HttpPost]
        [Route("LoginUser")]
        public bool LoginUser(LoginUserEntity item)
        {
            return repository.LoginUser(item);
        }

    }
}
