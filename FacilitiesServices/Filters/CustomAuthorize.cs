using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace FacilitiesServices.Filters
{
    public class CustomAuthorize : AuthorizeAttribute, System.Web.Http.Filters.IAuthorizationFilter
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization != null)
            {
                if (actionContext.Request.Headers.Authorization.Scheme == "Bearer00Lra_1eyaxnebOLb2PqukncUB9Tnq1-KQeIV9rxaa")
                {
                    return;
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                HttpContext.Current.Response.AddHeader("AuthenticationStatus", "NotAuthorized");
                return;
            }

        }
    }
}