using BusinessEntities.BusinessEntityClasses;
using System.Web.Http;
using UnitOfwork.Interfaces;
using UnitOfwork.UOWRepository;
using System.Collections.Generic;
using System;
using FacilitiesServices.Filters;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/vendor")]
    public class VendorController : ApiController
    {
        IVendor repository = new UOWVendor();

        [HttpGet]
        [Route("ifvendorexists")]
        public bool CheckIfVendorExists(string email)
        {
            return repository.IsVendorExists(email);
        }

        [HttpPost]
        [Route("CreateVendor")]
        public Guid CreateVendor(VendorEntities vendor)
        {
            return repository.CreateVendor(vendor);
        }


        [HttpPost]
        [Route("SaveVendorInsuranceData")]
        public bool SaveVendorInsuranceData(ClientInsuranceDetails VendorInsuranceModel)
        {
            return repository.SaveVendorInsuranceData(VendorInsuranceModel);
        }

        [HttpGet]
        [Route("GetClientVendors")]
        public List<VendorEntities> GetClientVendors(Guid client)
        {
            List<VendorEntities> clientVendorsList = null;

            clientVendorsList = repository.GetClientVendors(client);

            return clientVendorsList;
        }


        [HttpPost]
        [Route("UpdateVendor")]
        public int UpdateVendor(VendorEntities vendor)
        {
            if (vendor != null)
                return repository.UpdateVendor(vendor);
            else
                return 0;
        }

        [HttpGet]
        [Route("GetInsuranceandTaxdetails")]
        public IEnumerable<ClientVendorDetails> GetInsuranceandTaxdetails(Guid ClientVendorID)
        {
            return repository.GetInsuranceandTaxdetails(ClientVendorID);
        }

        [HttpGet]
        [Route("GetInsurancedetails")]
        public IEnumerable<ClientInsuranceDetails> GetInsurancedetails(Guid ClientVendorId)
        {
            return repository.GetInsurancedetails(ClientVendorId);
        }

        [HttpGet]
        [Route("GetUserDetails")]
        public IList<UserEntity> GetUserDetails(Guid Vendorid)
        {
            return repository.GetUserDetails(Vendorid);
        }

        [HttpGet]
        [Route("GetInsuranceType")]
        public IEnumerable<InsuranceType> GetInsuranceType(Guid ClientID)
        {
            return repository.GetInsuranceType(ClientID);
        }

        [HttpGet]
        [Route("GetInsuranceGridDetails")]
        public IEnumerable<ClientInsuranceDetails> GetInsuranceGridDetails(Guid ClientVendorId)
        {
            return repository.GetInsuranceGridDetails(ClientVendorId);
        }

        [HttpGet]
        [Route("GetClientVendorProblemClass")]
        public IEnumerable<ClientVendorProblemClassEntity> GetClientVendorProblemClass(Guid ClientVendorId)
        {
            return repository.GetClientVendorProblemClass(ClientVendorId);
        }

        [HttpPost]
        [Route("CreateVendorFromWO")]
        public Guid CreateVendorFromWO(VendorEntities vendor)
        {
            return repository.CreateVendorFromWO(vendor);
        }
    }
}
