using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacilitiesUserManagement.UserClasses
{   
    public static class VM_OktaUrlAndKey
    {   
        public static Tuple<string, string> GetOktaUrlAndKey()
        {
            try
            {
                var OKtaKey = "";
                var OktaUrl = "";
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    var HeaderId = _dbContext.ResourceTypeHeaders.Where(h => h.Name == "Okta Base Url").FirstOrDefault();
                    var CustomerResourceHeaders = HeaderId.ClientResourceHeaders;
                    foreach (var item in CustomerResourceHeaders)
                    {
                        var data = item.ClientResourceDetails;
                        OktaUrl = data.FirstOrDefault(x => x.Value.Contains("http")).Value;
                        OKtaKey = data.FirstOrDefault(x => !x.Value.Contains("http")).Value;
                    }
                }
                return Tuple.Create(OktaUrl, OKtaKey);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

