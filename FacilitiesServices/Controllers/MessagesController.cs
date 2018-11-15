using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/Messages")]
    public class MessagesController : ApiController
    {
        [HttpGet]
        [Route("get")]
        public IEnumerable<dynamic> Get()
        {
            var principal = RequestContext.Principal.Identity as ClaimsIdentity;

            var login = principal.Claims
                .SingleOrDefault(c => c.Type == System.IdentityModel.Claims.ClaimTypes.NameIdentifier)
                ?.Value;

            return new dynamic[]
            {
            new { Date = DateTime.Now, Text = "I am a Robot." },
            new { Date = DateTime.Now, Text = "Hello, world!" },
            };
        }
    }
}