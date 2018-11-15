using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities.BusinessEntityClasses
{
    public class VendorEntities
    {
        public User UserEntity { get; set; }
        public System.Guid VendorId { get; set; }
        public string VendorName { get; set; }
       
        public string Address01 { get; set; }
        public string Address02 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip01 { get; set; }
        public string Zip02 { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string TaxId { get; set; }
        public string TaxIdisEIN { get; set; }
        public byte[] TaxDocument { get; set; }
        public string ActiveFlag { get; set; }
        public string TaxDocumentType { get; set; }
        public string TaxDocumentName { get; set; }
        public Guid ClientVendorId { get; set; }

        public byte[] InsuranceDocument { get; set; }
        public string InsuranceDocumentName { get; set; }
        public string InsuranceDocumentType { get; set; }
        public string InsuranceCompName { get; set; }
        public string InsuranceBeginDate { get; set; }
        public string InsuranceEndDate { get; set; }
        public Guid LoggedInUserID { get; set; }
        public string ClientID { get; set; }
        public string TaxDocfiletype { get; set; }
        public Guid[] ClientProblemClassId { get; set; }
        public string Status { get; set; }
                
    }

    public class UserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ClientInsuranceDetails
    {
        public Guid ClientVendorInsuranceId { get; set; }
        public byte[] InsuranceDocument { get; set; }
        public string InsuranceDocumentName { get; set; }
        public string InsuranceDocumentType { get; set; }
        public string InsuranceCompName { get; set; }
        public string InsuranceBeginDate { get; set; }
        public string InsuranceEndDate { get; set; }
        public Guid LoggedInUserID { get; set; }
        public string ClientID { get; set; }
        public string TaxDocfiletype { get; set; }
        public string InsuranceType { get; set; }
        public Guid ClientVendorId { get; set; }
        public Guid InsuranceTypeID { get; set; }
    }

    public class ClientVendorDetails
    {
        public System.Guid ClientVendorId { get; set; }
        public byte[] TaxDocument { get; set; }
        public string TaxDocfiletype { get; set; }
        public string FileName { get; set; }
    }

    public class InsuranceType
    {
        public System.Guid ClientInsuranceTypeId { get; set; }
        public System.Guid Client { get; set; }
        public string Description { get; set; }
        public string ActiveFlag { get; set; }
    }    
}
