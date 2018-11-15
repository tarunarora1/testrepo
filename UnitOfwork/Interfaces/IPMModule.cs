using BusinessEntities.BusinessEntityClasses;
using BussinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace UnitOfwork.Interfaces
{
    public interface IPMModule
    {
        DataTable SaveDataOnPmHeader(PMHeaderEntity item);
        List<prc_GetPMModuleDetails_Result> GetClientpmheadersData(Guid ClientId);
        bool SaveDataOnPMVendors(PMVendorsEntity item);
        List<PMVendorsEntity> GetAssiocateVendor(Guid PMHeaderId);
        List<CustomerLocationEntity> GetCustomerLocation(Guid CustomerId);
        bool SaveDataOnPMVendorCustomerLocations(PmVendorCustomerLocationEntity item);
        List<CustomerLocationEntity> GetDataFromPMVendorCustomerLocations(Guid PMVendorId);
        bool DeleteAssociatedCustomerLocationAndVendor(Guid VenderId, int AssociatedVenderLocationCount);
        bool DeleteSelectedPMVendorCustomerLocations(PMCustomerLOcations SelectedCustomerLocations);
        List<prc_GetPMConfirmationData_Result> GetPMConfirnationData(Guid PmHeaderId);
        DataTable GetWOConfirmationDate(Guid clientPMHeaderID);
        DataTable GetWOEditRecord(Guid clientPMHeaderID);
        bool UpdatePMConfirmation(Guid PMHeaderId);
        List<prc_GetPMWorkOrder_Result> GetWorkOrderData(Guid PMVendorId, Guid ClientId, Guid ClientPMHeaderId);
        CustomerLocationEntity GetLatestCustomerInformation(Guid PMVendorId);

    }
        
        


        
}
