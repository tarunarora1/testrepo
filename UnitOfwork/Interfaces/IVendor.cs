using BusinessEntities.BusinessEntityClasses;
using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfwork.Interfaces
{
    public interface IVendor
    {
        bool IsVendorExists(string email);
        Guid CreateVendor(VendorEntities vendor);
        int UpdateVendor(VendorEntities vendor);
        List<VendorEntities> GetClientVendors(Guid client);
        IEnumerable<VendorEntities> GetVendorDetailById(Guid vendorId);
        IEnumerable<ClientVendorDetails> GetInsuranceandTaxdetails(Guid ClientVendorID);
        IEnumerable<ClientInsuranceDetails> GetInsurancedetails(Guid ClientVendorId);
        IList<UserEntity> GetUserDetails(Guid Vendorid);
        IEnumerable<InsuranceType> GetInsuranceType(Guid ClientID);
        IEnumerable<ClientInsuranceDetails> GetInsuranceGridDetails(Guid ClientVendorId);
        bool SaveVendorInsuranceData(ClientInsuranceDetails VendorInsuranceModel);
        IEnumerable<ClientVendorProblemClassEntity> GetClientVendorProblemClass(Guid ClientVendorId);
        Guid CreateVendorFromWO(VendorEntities vendor);
    }
}
