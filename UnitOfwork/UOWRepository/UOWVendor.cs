using System;
using System.Linq;
using System.Collections.Generic;
using DataModel;
using UnitOfwork.Interfaces;
using BusinessEntities.BusinessEntityClasses;
using FacilitiesUserManagement.UserClasses;

namespace UnitOfwork.UOWRepository
{
    public class UOWVendor : IVendor
    {
        public bool IsVendorExists(string emailId)
        {
            bool exists = false;

            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    if (db.Vendors.Any(x => x.Email == emailId))
                    {
                        //var userDetail = db.Vendors.FirstOrDefault(x => x.Email == emailId);
                        exists = true;
                    }
                }

                catch (Exception ex)
                { }
            }
            return exists;
        }

        public Guid CreateVendorFromWO(VendorEntities vendor)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                UOWVendor vendorObj = new UOWRepository.UOWVendor();
                try
                {
                    var newVendor = new Vendor();
                    if (vendor.Email != null && (!vendorObj.IsVendorExists(vendor.Email)))
                    {
                        newVendor.VendorId = Guid.NewGuid();
                        newVendor.VendorName = vendor.VendorName;
                        newVendor.Address01 = vendor.Address01 != null ? vendor.Address01 : " ";
                        newVendor.Address02 = vendor.Address02 != null ? vendor.Address02 : " ";
                        newVendor.City = vendor.City != null ? vendor.City : " ";
                        newVendor.State = vendor.State != null ? vendor.State : " ";
                        newVendor.Zip01 = vendor.Zip01 != null ? vendor.Zip01 : " ";
                        newVendor.Zip02 = vendor.Zip02 != null ? vendor.Zip02 : " ";
                        newVendor.Email = vendor.Email != null ? vendor.Email : " ";
                        newVendor.TaxId = "1";
                        newVendor.TaxIdisEIN = "Y";
                        newVendor.Telephone = vendor.Telephone != null ? vendor.Telephone : " ";
                        newVendor.ActiveFlag = "Y";
                        newVendor.AddressVerifiedFlag = "Y";
                        db.Vendors.Add(newVendor);


                        var ClientID = db.ClientUsers.Where(a => a.User == vendor.LoggedInUserID).Select(a => a.Client).FirstOrDefault();
                        var clientVendor = new ClientVendor();
                        clientVendor.ClientVendorId = Guid.NewGuid();
                        clientVendor.Client = ClientID;
                        clientVendor.Vendor = newVendor.VendorId;
                        clientVendor.ActiveFlag = "Y";
                        clientVendor.TaxDocument = vendor.TaxDocument;
                        clientVendor.FileName = vendor.TaxDocumentName;
                        var cgFileTyle = db.CGSFileTypes.Where(p => p.Decription.ToString().Trim().ToLower() == vendor.TaxDocumentType.Trim().ToLower()).FirstOrDefault();
                        if (cgFileTyle != null)
                            clientVendor.FileType = cgFileTyle.CGSFileTypesId;

                        db.ClientVendors.Add(clientVendor);
                        db.SaveChanges();
                    }
                    return newVendor.VendorId;
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
        }

        public Guid CreateVendor(VendorEntities vendor)
        {
            Guid UserID = Guid.Empty;
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                UOWVendor vendorObj = new UOWRepository.UOWVendor();
                try
                {
                    vendor.UserEntity.Email = vendor.Email;
                    var newVendor = new Vendor();

                    if (vendor.Email != null && (!vendorObj.IsVendorExists(vendor.Email)))
                    {
                        if (vendor.UserEntity.FirstName != null && vendor.UserEntity.FirstName.Trim().Length > 0)
                        {
                            if (vendor.UserEntity.LastName == null)
                                vendor.UserEntity.LastName = "";

                            var U = VM_User.AddDataInUser<User>(vendor.UserEntity);
                            if (U != null)
                            {
                                newVendor.VendorId = Guid.NewGuid();
                                newVendor.VendorName = (vendor.VendorName != null && vendor.VendorName.Trim().Length > 0) ? vendor.VendorName : vendor.UserEntity.FirstName + " " + vendor.UserEntity.LastName;
                                newVendor.Address01 = vendor.Address01 != null ? vendor.Address01 : " ";
                                newVendor.Address02 = vendor.Address02 != null ? vendor.Address02 : " ";
                                newVendor.City = vendor.City != null ? vendor.City : " ";
                                newVendor.State = vendor.State != null ? vendor.State : " ";
                                newVendor.Zip01 = vendor.Zip01 != null ? vendor.Zip01 : " ";
                                newVendor.Zip02 = vendor.Zip02 != null ? vendor.Zip02 : " ";
                                newVendor.Email = vendor.Email != null ? vendor.Email : " ";
                                newVendor.TaxId = vendor.TaxId != null ? vendor.TaxId : " ";
                                newVendor.TaxIdisEIN = vendor.TaxIdisEIN != null ? vendor.TaxIdisEIN : " ";
                                newVendor.Telephone = vendor.Telephone != null ? vendor.Telephone : " ";
                                newVendor.ActiveFlag = vendor.Status;
                                newVendor.AddressVerifiedFlag = "Y";

                                //client vendors
                                var ClientID = db.ClientUsers.Where(a => a.User == vendor.LoggedInUserID).Select(a => a.Client).FirstOrDefault();
                                var clientVendor = new ClientVendor();
                                clientVendor.ClientVendorId = Guid.NewGuid();
                                clientVendor.Client = ClientID;
                                clientVendor.Vendor = newVendor.VendorId;
                                clientVendor.ActiveFlag = vendor.Status;
                                clientVendor.TaxDocument = vendor.TaxDocument;
                                clientVendor.FileName = vendor.TaxDocumentName;
                                var cgFileTyle = db.CGSFileTypes.Where(p => p.Decription.ToString().Trim().ToLower() == vendor.TaxDocumentType.Trim().ToLower()).FirstOrDefault();
                                if (cgFileTyle != null)
                                    clientVendor.FileType = cgFileTyle.CGSFileTypesId;

                                //Get Vendor Admin Id
                                var GetVendorAdminId = db.Roles.Where(a => a.RoleName == "Vendor Admin" && a.ActiveFlag == "Y").FirstOrDefault();
                                if (GetVendorAdminId != null)
                                {
                                    // Add data in VendorUser Table
                                    VendorUser CU = new VendorUser();
                                    CU.VendorUserId = Guid.NewGuid();
                                    CU.Vendor = newVendor.VendorId;
                                    CU.Role = GetVendorAdminId.RoleId;
                                    CU.User = U.UserId;
                                    CU.ActiveFlag = vendor.Status;
                                    db.VendorUsers.Add(CU);
                                }

                                foreach (Guid value in vendor.ClientProblemClassId)
                                {
                                    ClientVendorProblemClass CP = new ClientVendorProblemClass();
                                    CP.ClientVendorProblemClassesId = Guid.NewGuid();
                                    CP.ClientVendor = clientVendor.ClientVendorId;
                                    CP.ProblemClass = value;
                                    db.ClientVendorProblemClasses.Add(CP);
                                }
                                db.Vendors.Add(newVendor);
                                db.ClientVendors.Add(clientVendor);

                                if (db.SaveChanges() > 0)
                                {
                                    UserID = U.UserId;
                                }
                            }
                        }
                        else
                        {
                            //we must need the vendor name
                            if (vendor.VendorName != null && vendor.VendorName.Trim().Length > 0)
                            {
                                #region No information entered by user for Vendor
                                newVendor.VendorId = Guid.NewGuid();
                                newVendor.VendorName = vendor.VendorName != null ? vendor.VendorName : " ";
                                newVendor.Address01 = vendor.Address01 != null ? vendor.Address01 : " ";
                                newVendor.Address02 = vendor.Address02 != null ? vendor.Address02 : " ";
                                newVendor.City = vendor.City != null ? vendor.City : " ";
                                newVendor.State = vendor.State != null ? vendor.State : " ";
                                newVendor.Zip01 = vendor.Zip01 != null ? vendor.Zip01 : " ";
                                newVendor.Zip02 = vendor.Zip02 != null ? vendor.Zip02 : " ";
                                newVendor.Email = vendor.Email != null ? vendor.Email : " ";
                                newVendor.TaxId = vendor.TaxId != null ? vendor.TaxId : " ";
                                newVendor.TaxIdisEIN = vendor.TaxIdisEIN != null ? vendor.TaxIdisEIN : " ";
                                newVendor.Telephone = vendor.Telephone != null ? vendor.Telephone : " ";
                                newVendor.ActiveFlag = vendor.Status;
                                newVendor.AddressVerifiedFlag = "Y";

                                //client vendors
                                var ClientID = db.ClientUsers.Where(a => a.User == vendor.LoggedInUserID).Select(a => a.Client).FirstOrDefault();
                                var clientVendor = new ClientVendor();
                                clientVendor.ClientVendorId = Guid.NewGuid();
                                clientVendor.Client = ClientID;
                                clientVendor.Vendor = newVendor.VendorId;
                                clientVendor.ActiveFlag = vendor.Status;
                                clientVendor.TaxDocument = vendor.TaxDocument;
                                clientVendor.FileName = vendor.TaxDocumentName;
                                var cgFileTyle = db.CGSFileTypes.Where(p => p.Decription.ToString().Trim().ToLower() == vendor.TaxDocumentType.Trim().ToLower()).FirstOrDefault();
                                if (cgFileTyle != null)
                                    clientVendor.FileType = cgFileTyle.CGSFileTypesId;

                                foreach (Guid value in vendor.ClientProblemClassId)
                                {
                                    ClientVendorProblemClass CP = new ClientVendorProblemClass();
                                    CP.ClientVendorProblemClassesId = Guid.NewGuid();
                                    CP.ClientVendor = clientVendor.ClientVendorId;
                                    CP.ProblemClass = value;
                                    db.ClientVendorProblemClasses.Add(CP);
                                }

                                db.Vendors.Add(newVendor);
                                db.ClientVendors.Add(clientVendor);

                                if (db.SaveChanges() > 0)
                                {
                                    UserID = newVendor.VendorId;
                                }
                                #endregion
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return UserID;
        }

        public bool SaveVendorInsuranceData(ClientInsuranceDetails VendorInsuranceModel)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    ClientVendorInsurance CI = new ClientVendorInsurance();
                    if (VendorInsuranceModel.ClientVendorInsuranceId == Guid.Empty)
                    {
                        CI.ClientVendorInsuranceId = Guid.NewGuid();
                        CI.ClientVendor = VendorInsuranceModel.ClientVendorId;
                        CI.InsuranceType = VendorInsuranceModel.InsuranceTypeID;
                        CI.InsuranceCompanyName = VendorInsuranceModel.InsuranceCompName;
                        CI.InsuranceDocument = VendorInsuranceModel.InsuranceDocument;
                        CI.UploadedByUser = VendorInsuranceModel.LoggedInUserID;
                        CI.UploadedDate = DateTime.Now;
                        CI.FileName = VendorInsuranceModel.InsuranceDocumentName;
                        //dates set to default
                        if (VendorInsuranceModel.InsuranceBeginDate.Trim().Length > 0)
                            CI.CoverageBeginDate = DateTime.Parse(VendorInsuranceModel.InsuranceBeginDate);
                        if (VendorInsuranceModel.InsuranceEndDate.Trim().Length > 0)
                            CI.CoverageEndDate = DateTime.Parse(VendorInsuranceModel.InsuranceEndDate);
                        ////file type
                        var cgInsuranceFileType = db.CGSFileTypes.Where(p => p.Decription.ToString().Trim().ToLower() == VendorInsuranceModel.InsuranceDocumentType.Trim().ToLower()).FirstOrDefault();
                        if (cgInsuranceFileType != null)
                            CI.FileType = cgInsuranceFileType.CGSFileTypesId;

                        db.ClientVendorInsurances.Add(CI);
                        db.SaveChanges();
                    }
                    else
                    {
                        CI = db.ClientVendorInsurances.Where(a => a.ClientVendorInsuranceId == VendorInsuranceModel.ClientVendorInsuranceId).FirstOrDefault();
                        if (CI != null)
                        {
                            CI.ClientVendor = VendorInsuranceModel.ClientVendorId;
                            CI.InsuranceType = VendorInsuranceModel.InsuranceTypeID;
                            CI.InsuranceCompanyName = VendorInsuranceModel.InsuranceCompName;
                            CI.InsuranceDocument = VendorInsuranceModel.InsuranceDocument;
                            CI.UploadedByUser = VendorInsuranceModel.LoggedInUserID;
                            CI.UploadedDate = DateTime.Now;
                            CI.FileName = VendorInsuranceModel.InsuranceDocumentName;
                            //dates set to default
                            if (VendorInsuranceModel.InsuranceBeginDate.Trim().Length > 0)
                                CI.CoverageBeginDate = DateTime.Parse(VendorInsuranceModel.InsuranceBeginDate);
                            if (VendorInsuranceModel.InsuranceEndDate.Trim().Length > 0)
                                CI.CoverageEndDate = DateTime.Parse(VendorInsuranceModel.InsuranceEndDate);
                            ////file type
                            var cgInsuranceFileType = db.CGSFileTypes.Where(p => p.Decription.ToString().Trim().ToLower() == VendorInsuranceModel.InsuranceDocumentType.Trim().ToLower()).FirstOrDefault();
                            if (cgInsuranceFileType != null)
                                CI.FileType = cgInsuranceFileType.CGSFileTypesId;
                            db.SaveChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return true;
        }

        public int UpdateVendor(VendorEntities vendor)
        {
            int result = 0;
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var GetVendorDetails = db.Vendors.Where(p => p.Email.ToLower().Trim() == vendor.Email.Trim().ToLower()).FirstOrDefault();
                    if (GetVendorDetails != null)
                    {
                        GetVendorDetails.VendorName = vendor.VendorName;
                        GetVendorDetails.Address01 = vendor.Address01;
                        GetVendorDetails.Address02 = vendor.Address02;
                        GetVendorDetails.City = vendor.City;
                        GetVendorDetails.State = vendor.State;
                        GetVendorDetails.Zip01 = vendor.Zip01;
                        GetVendorDetails.Zip02 = vendor.Zip02;
                        GetVendorDetails.Email = vendor.Email;
                        GetVendorDetails.TaxId = vendor.TaxId;
                        GetVendorDetails.TaxIdisEIN = vendor.TaxIdisEIN;
                        GetVendorDetails.Telephone = vendor.Telephone;
                        GetVendorDetails.ActiveFlag = vendor.Status;

                        //get client vendor info                        
                        var getclientvendors = db.ClientVendors.Where(p => p.Vendor == GetVendorDetails.VendorId &&
                        p.Client.ToString().Trim().ToLower() == vendor.ClientID.ToString().Trim().ToLower()).FirstOrDefault();
                        if (getclientvendors != null)
                        {
                            getclientvendors.ActiveFlag = vendor.Status;
                            getclientvendors.TaxDocument = vendor.TaxDocument;
                            getclientvendors.FileName = vendor.TaxDocumentName;
                            var cgFileTyle = db.CGSFileTypes.Where(p => p.Decription.ToString().Trim().ToLower() == vendor.TaxDocumentType.Trim().ToLower()).FirstOrDefault();
                            if (cgFileTyle != null)
                                getclientvendors.FileType = cgFileTyle.CGSFileTypesId;

                            var VendorUser = db.VendorUsers.Where(x => x.Vendor == GetVendorDetails.VendorId).FirstOrDefault();
                            if (VendorUser != null)
                            {
                                VendorUser.ActiveFlag = vendor.Status;
                            }

                            // Delete all records in ClientVendorProblemClasses table
                            var allRec = db.ClientVendorProblemClasses.Where(a => a.ClientVendor == getclientvendors.ClientVendorId);
                            db.ClientVendorProblemClasses.RemoveRange(allRec);
                            db.SaveChanges();

                            // Insert new records in ClientVendorProblemClasses table
                            foreach (Guid value in vendor.ClientProblemClassId)
                            {
                                ClientVendorProblemClass CP = new ClientVendorProblemClass();
                                CP.ClientVendorProblemClassesId = Guid.NewGuid();
                                CP.ClientVendor = getclientvendors.ClientVendorId;
                                CP.ProblemClass = value;
                                db.ClientVendorProblemClasses.Add(CP);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }
        //changes
        public List<VendorEntities> GetClientVendors(Guid client)

        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var GetAllClientVendors = (from V in db.Vendors
                                           join VU in db.VendorUsers on V.VendorId equals VU.Vendor into temvu
                                           from VU in temvu.DefaultIfEmpty()
                                           join U in db.Users on VU.User equals U.UserId into temU
                                           from U in temU.DefaultIfEmpty()
                                           join CV in db.ClientVendors on V.VendorId equals CV.Vendor into tempVen
                                           from CV in tempVen.DefaultIfEmpty()
                                           where CV.Client == client
                                           select new
                                           {
                                               VendorId = V.VendorId,
                                               VendorName = V.VendorName,
                                               Address01 = V.Address01,
                                               Address02 = V.Address02,
                                               City = V.City,
                                               State = V.State,
                                               Zip01 = V.Zip01,
                                               Zip02 = V.Zip02,
                                               Email = V.Email,
                                               Telephone = V.Telephone,
                                               TaxId = V.TaxId,
                                               TaxIdisEIN = V.TaxIdisEIN,
                                               ClientVendorId = CV.ClientVendorId,
                                               _ActiveFlag = VU.Vendor == null ? CV.ActiveFlag : VU.ActiveFlag
                                           }).ToList().Select(W => new VendorEntities()
                                           {
                                               Address01 = W.Address01,
                                               Address02 = W.Address02,
                                               City = W.City,
                                               State = W.State,
                                               Zip01 = W.Zip01,
                                               Zip02 = W.Zip02,
                                               VendorName = W.VendorName,
                                               VendorId = W.VendorId,
                                               Email = W.Email,
                                               Telephone = W.Telephone,
                                               TaxId = W.TaxId,
                                               TaxIdisEIN = W.TaxIdisEIN,
                                               ClientVendorId = W.ClientVendorId,
                                               Status = UserStatus(W._ActiveFlag)
                                           }).ToList();
                return GetAllClientVendors;
            }
        }

        public string UserStatus(string ActiveFlag)
        {
            if (ActiveFlag == "Y")
            {
                return "Active";
            }
            else if (ActiveFlag == "R")
            {
                return "Registration Required";
            }
            else
            {
                return "InActive";
            }
        }

        public IList<UserEntity> GetUserDetails(Guid Vendorid)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var UserDetails = db.VendorUsers.Join(db.Users, VU => VU.User, U => U.UserId, (VU, U) => new { FirstName = U.FirstName, Vendor = VU.Vendor, LastName = U.LastName }).Where(VU => VU.Vendor == Vendorid).ToList().Select(W => new UserEntity()
                    {
                        FirstName = W.FirstName,
                        LastName = W.LastName,
                    }).ToList();
                    return UserDetails;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ClientVendorProblemClassEntity> GetClientVendorProblemClass(Guid ClientVendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var GetTaxandInsurance = (from CV in db.ClientVendorProblemClasses
                                          where CV.ClientVendor == ClientVendorId
                                          select new
                                          {
                                              _ClientVendorProblemClassesId = CV.ClientVendorProblemClassesId,
                                              _ProblemClass = CV.ProblemClass
                                          }).ToList().Select(W => new ClientVendorProblemClassEntity()
                                          {
                                              ClientVendorProblemClassesId = W._ClientVendorProblemClassesId,
                                              ProblemClass = W._ProblemClass
                                          }).ToList();
                return GetTaxandInsurance;
            }
        }

        public IEnumerable<ClientVendorDetails> GetInsuranceandTaxdetails(Guid ClientVendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var GetTaxandInsurance = (from CV in db.ClientVendors
                                          where CV.ClientVendorId == ClientVendorId && CV.ActiveFlag == "Y"
                                          select new
                                          {
                                              TaxDocument = CV.TaxDocument,
                                              FileType = CV.FileType,
                                              FileName = CV.FileName,
                                              ClientVendorId = CV.ClientVendorId
                                          }).ToList().Select(W => new ClientVendorDetails()
                                          {
                                              TaxDocument = W.TaxDocument,
                                              TaxDocfiletype = db.CGSFileTypes.Where(a => a.CGSFileTypesId == W.FileType).Select(a => a.Decription).FirstOrDefault(),
                                              FileName = W.FileName,
                                              ClientVendorId = W.ClientVendorId
                                          }).ToList();
                return GetTaxandInsurance;
            }
        }

        public IEnumerable<ClientInsuranceDetails> GetInsurancedetails(Guid ClientVendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var GetInsuranceDetails = (from CV in db.ClientVendorInsurances
                                           where CV.ClientVendorInsuranceId == ClientVendorId
                                           select new
                                           {
                                               ClientVendorInsuranceId = CV.ClientVendorInsuranceId,
                                               InsuranceCompanyName = CV.InsuranceCompanyName,
                                               CoverageBeginDate = CV.CoverageBeginDate,
                                               CoverageEndDate = CV.CoverageEndDate,
                                               InsuranceDocument = CV.InsuranceDocument,
                                               FileType = CV.FileType,
                                               FileName = CV.FileName
                                           }).ToList().Select(W => new ClientInsuranceDetails()
                                           {
                                               ClientVendorInsuranceId = W.ClientVendorInsuranceId,
                                               InsuranceCompName = W.InsuranceCompanyName,
                                               InsuranceBeginDate = W.CoverageBeginDate.ToString(),
                                               InsuranceEndDate = W.CoverageEndDate.ToString(),
                                               InsuranceDocument = W.InsuranceDocument,
                                               InsuranceDocumentType = db.CGSFileTypes.Where(a => a.CGSFileTypesId == W.FileType).Select(a => a.Decription).FirstOrDefault(),
                                               InsuranceDocumentName = W.FileName
                                           }).ToList();
                return GetInsuranceDetails;
            }
        }

        public IEnumerable<ClientInsuranceDetails> GetInsuranceGridDetails(Guid ClientVendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                var GetInsuranceDetails = (from CV in db.ClientVendorInsurances
                                           join IT in db.ClientInsuranceTypes on CV.InsuranceType equals IT.ClientInsuranceTypeId
                                           where CV.ClientVendor == ClientVendorId
                                           select new
                                           {
                                               ClientVendorInsuranceId = CV.ClientVendorInsuranceId,
                                               InsuranceCompanyName = CV.InsuranceCompanyName,
                                               CoverageBeginDate = CV.CoverageBeginDate,
                                               CoverageEndDate = CV.CoverageEndDate,
                                               _InsuranceTypeID = CV.InsuranceType,
                                               Description = IT.Description
                                           }).ToList().Select(W => new ClientInsuranceDetails()
                                           {
                                               ClientVendorInsuranceId = W.ClientVendorInsuranceId,
                                               InsuranceCompName = W.InsuranceCompanyName,
                                               InsuranceBeginDate = W.CoverageBeginDate.ToString(),
                                               InsuranceEndDate = W.CoverageEndDate.ToString(),
                                               InsuranceType = W.Description,
                                               InsuranceTypeID = W._InsuranceTypeID
                                           }).ToList();
                return GetInsuranceDetails;
            }
        }

        public IEnumerable<VendorEntities> GetVendorDetailById(Guid vendorId)
        {
            using (FacilitiesEntities db = new FacilitiesEntities())
            {
                try
                {
                    var vendorInfo = (from v in db.Vendors
                                      where v.VendorId == vendorId
                                      select new
                                      {
                                          VendorId = v.VendorId
                                      }).ToList().Select(v => new VendorEntities()
                                      {
                                          VendorId = v.VendorId
                                      }).ToList();
                    return vendorInfo;

                }
                catch (Exception ex)
                {
                    return null;
                }

            }


        }

        public IEnumerable<InsuranceType> GetInsuranceType(Guid ClientID)
        {
            try
            {
                using (FacilitiesEntities DB = new FacilitiesEntities())
                {
                    var InsuranceTypeList = (from C in DB.ClientInsuranceTypes
                                             where C.Client == ClientID && C.ActiveFlag == "Y"
                                             select new
                                             {
                                                 ClientInsuranceTypeId = C.ClientInsuranceTypeId,
                                                 Description = C.Description

                                             }).ToList().Select(x => new InsuranceType()
                                             {
                                                 ClientInsuranceTypeId = x.ClientInsuranceTypeId,
                                                 Description = x.Description

                                             }).ToList();
                    return InsuranceTypeList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
