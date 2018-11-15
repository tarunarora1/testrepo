using BussinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacilitiesUserManagement.UserClasses
{
    public static class VM_RegistrationUserDataCGSLinkHeader
    {
        public static LinkHeader AddDataOnLinkHeader<T>(LinkHeader item)
        {
            LinkHeader LH = new LinkHeader();
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    var data = _dbContext.CGSLinkHeaders.Where(x => x.Action == item.Action).FirstOrDefault();
                    if(data != null)
                    {
                        LH.LinkHeaderId = Guid.NewGuid();
                        LH.CGSLinkHeader = data.CGSLinkHeaderId;
                        LH.Action = data.Action;
                        LH.RandomString = item.RandomString;
                        LH.BeginDate = DateTime.Now;
                        var TimeFrame = data.Timeframe;
                        var endDate = DateTime.Now;
                        endDate = endDate.AddDays(TimeFrame);
                        LH.EndDate = endDate;
                        _dbContext.LinkHeaders.Add(LH);
                        _dbContext.SaveChanges();
                    }


                }
                return LH;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void AddDataOnLinkDetails(LinkDetailEntity item)
        {            
            try
            {
                using (FacilitiesEntities _dbContext = new FacilitiesEntities())
                {
                    var data = _dbContext.CGSLinkDetails.Where(x => x.CGSLinkHeader == item.CGSLinkHeader).ToList();
                    if(data != null)
                    {
                        foreach (var ss in item.Value)
                        {
                            var dd=data.FirstOrDefault(r => r.ColumnHeader == ss.Key);
                            LinkDetail LD = new LinkDetail();
                            LD.LinkDetailId = Guid.NewGuid();
                            LD.CGSLinkHeader = item.CGSLinkHeader;
                            LD.CGSLinkDetails = dd.CGSLinkDetailId;
                            LD.LinkHeader = item.LinkHeader;
                            LD.Value = ss.value;
                            _dbContext.LinkDetails.Add(LD);
                            _dbContext.SaveChanges();

                        }                       
                    }
                }                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
