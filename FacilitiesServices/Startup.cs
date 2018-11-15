using Microsoft.Owin;
using Okta.AspNet;
using Owin;
using System.Configuration;

[assembly: OwinStartup(typeof(FacilitiesServices.Startup))]

namespace FacilitiesServices
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseOktaWebApi(new OktaWebApiOptions()
            {
                OktaDomain = ConfigurationManager.AppSettings["okta:OktaDomain"],
                ClientId = ConfigurationManager.AppSettings["okta:ClientId"]
            });
        }
    }
}
