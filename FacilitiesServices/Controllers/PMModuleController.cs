using System.Web.Http;
using UnitOfwork.Interfaces;
using UnitOfwork.UOWRepository;
using DataModel;
using System.Collections.Generic;
using System;
using BussinessEntities.BusinessEntityClasses;
using BusinessEntities.BusinessEntityClasses;
using FacilitiesServices.Filters;
using System.Data;

namespace FacilitiesServices.Controllers
{
    [Authorize]
    [RoutePrefix("API/PMMOdule")]
    public class PMModuleController : ApiController
    {
        IPMModule repository = new UOWPMModule();

        [HttpPost]
        [Route("SaveDataOnPmHeader")]
        public DataTable SaveDataOnPmHeader(PMHeaderEntity item)
        {
            return repository.SaveDataOnPmHeader(item);
        }

        [HttpGet]
        [Route("GetClientpmheadersData")]
        public List<prc_GetPMModuleDetails_Result> GetClientpmheadersData(Guid ClientId)
        {
            return repository.GetClientpmheadersData(ClientId);
        }

        [HttpGet]
        [Route("GetWorkOrderData")]
        public List<prc_GetPMWorkOrder_Result> GetWorkOrderData(Guid PMVendorId, Guid ClientId, Guid ClientPMHeaderId)
        {
            return repository.GetWorkOrderData(PMVendorId, ClientId,ClientPMHeaderId);
        }

        [HttpGet]
        [Route("GetLatestCustomerInformation")]
        public CustomerLocationEntity GetLatestCustomerInformation(Guid PMVendorId)
        {
            return repository.GetLatestCustomerInformation(PMVendorId);
        }
        

        [HttpPost]
        [Route("SaveDataOnPMVendors")]
        public bool SaveDataOnPMVendors(PMVendorsEntity item)
        {
            return repository.SaveDataOnPMVendors(item);
        }

        [HttpPost]
        [Route("SaveDataOnPMVendorCustomerLocations")]
        public bool SaveDataOnPMVendorCustomerLocations(PmVendorCustomerLocationEntity item)
        {
            return repository.SaveDataOnPMVendorCustomerLocations(item);
        }

        [HttpGet]
        [Route("GetAssiocateVendor")]
        public List<PMVendorsEntity> GetAssiocateVendor(Guid PMHeaderId)
        {
            return repository.GetAssiocateVendor(PMHeaderId);
        }

        [HttpGet]
        [Route("GetPMConfirnationData")]
        public List<prc_GetPMConfirmationData_Result> GetPMConfirnationData(Guid PmHeaderId)
        {
            return repository.GetPMConfirnationData(PmHeaderId);
        }

        [HttpGet]
        [Route("GetWOConfirmationDate")]
        public DataTable GetWOConfirmationDate(Guid clientPMHeaderID)
        {
            return repository.GetWOConfirmationDate(clientPMHeaderID);
        }

        [HttpGet]
        [Route("GetWOEditRecord")]
        public DataTable GetWOEditRecord(Guid clientPMHeaderID)
        {
            return repository.GetWOEditRecord(clientPMHeaderID);
        }
        [HttpGet]
        [Route("GetCustomerLocation")]
        public List<CustomerLocationEntity> GetCustomerLocation(Guid CustomerId)
         {
            return repository.GetCustomerLocation(CustomerId);
        }

        [HttpGet]
        [Route("GetDataFromPMVendorCustomerLocations")]
        public List<CustomerLocationEntity> GetDataFromPMVendorCustomerLocations(Guid PMVendorId)
        {
            return repository.GetDataFromPMVendorCustomerLocations(PMVendorId);
        }

        [HttpDelete]
        [Route("DeleteAssociatedCustomerLocationAndVendor")]
        public bool DeleteAssociatedCustomerLocationAndVendor(Guid VenderId, int AssociatedVenderLocationCount)
        {
            return repository.DeleteAssociatedCustomerLocationAndVendor(VenderId, AssociatedVenderLocationCount);
        }

        [HttpPost]
        [Route("DeleteSelectedPMVendorCustomerLocations")]
        public bool DeleteSelectedPMVendorCustomerLocations(PMCustomerLOcations SelectedCustomerLocations)
        {
            return repository.DeleteSelectedPMVendorCustomerLocations(SelectedCustomerLocations);
        }

        [HttpGet]
        [Route("UpdatePMConfirmation")]
        public bool UpdatePMConfirmation(Guid PMHeaderId)
        {
            return repository.UpdatePMConfirmation(PMHeaderId);
        }
    }

}
