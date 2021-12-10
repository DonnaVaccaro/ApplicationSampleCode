using DocumentFormat.OpenXml.Drawing;
using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.XPath;

namespace OTS.Controller
{
    public class MassTortController
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(MassTortController));

        private readonly SQLMassTortQueries sQLMassTortQueries;
        private readonly SQLCollaterialQueries sQLCollaterialQueries;
        private List<MassTort> uploadedMassTortNewMassTortList;
        private List<MassTort> uploadedMassTortUpdateMassTortList;
        private List<MassTort> uploadedMassTortCaseTypeNotValidList;
        private List<MassTort> uploadedMassTortCaseNotFoundList;
        private CollaterialController collaterialController;



        private List<MassTort> massTortListByLoanId;

        public MassTortController()
        {
            sQLMassTortQueries = new SQLMassTortQueries();
            collaterialController = new CollaterialController();
            uploadedMassTortNewMassTortList = new List<MassTort>();
            uploadedMassTortCaseTypeNotValidList = new List<MassTort>();
            uploadedMassTortUpdateMassTortList = new List<MassTort>();
            uploadedMassTortCaseNotFoundList = new List<MassTort>();
            sQLCollaterialQueries = new SQLCollaterialQueries();


        }


        public List<MassTort> saveUploadedAddMassTorts(List<MassTort> massTortList)
        {
            List<MassTort> failedList = new List<MassTort>();

            log.Info("save uploaded MassTort");
            if (massTortList == null)
            {
                return null;
            }


            for (int i = 0; i < massTortList.Count; i++)
            {

                MassTort m = massTortList[i];

                if (m == null)
                {
                    log.Error("Trying to add a new masstort but the case is null");
                    return null;
                }
                m.MassTortCreatedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                m.MassTortModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                Collaterial coll = collaterialController.searchCaseCaseID(m.CaseID);
                if (ComboBoxPopulationController.caseTypeMassTortList.Contains(coll.CaseType))
                {
                    m.CaseType = coll.CaseType;
                }
                else
                {
                    coll.CaseType = m.CaseType;
                }
                bool res = sQLMassTortQueries.addNewMassTort(m);
                if (!res)
                {
                    failedList.Add(m);

                }
                else
                {
                    string id = sQLMassTortQueries.selectsMostRecentInsertIdentity();


                    coll.MassTortID = id;
                    collaterialController.setUpdatedCase(coll);
                    collaterialController.updateCase();
                }
            }
            return failedList;
        }


        public List<MassTort> saveUploadedUpdatedMassTorts(List<MassTort> massTortList)
        {
            List<MassTort> failedList = new List<MassTort>();

            log.Info("save updated MassTort");
            if (massTortList == null)
            {
                return null;
            }


            for (int i = 0; i < massTortList.Count; i++)
            {
                MassTort m = massTortList[i];
                if (m == null)
                {
                    log.Error("Trying to add a updated MassTort but the case is null");
                    return null;
                }


                m.MassTortModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                List<MassTort> cc = sQLMassTortQueries.searchMassTortByMassTortID(m.MassTortID, m.CaseID);
                Collaterial coll = collaterialController.searchCaseCaseID(m.CaseID);

                if (cc == null)
                {
                    return null;
                }
                if (ComboBoxPopulationController.caseTypeMassTortList.Contains(coll.CaseType))
                {
                    m.CaseType = coll.CaseType;
                }
                else
                {
                    coll.CaseType = m.CaseType;
                }
                bool res = sQLMassTortQueries.udateMassTort(m);
                if (!res)
                {
                    failedList.Add(m);
                }
                else
                {
                    // string id = sQLMassTortQueries.selectsMostRecentInsertIdentity();


                    // coll.MassTortID = id;
                    //coll.CaseType = m.CaseType;
                    collaterialController.setUpdatedCase(coll);
                    collaterialController.updateCase();
                }

            }
            return failedList;
        }


        //Goes through all upload cases and determines if they are new or existing
        public Collaterial LookUpCase(MassTort masstort, Loan loan)
        {
            List<Collaterial> collaterialListByLoanId = new List<Collaterial>();
            Collaterial collFound = null;
            log.Info("uploaded cases is it new or existing");
            bool res = true;
            if (masstort == null)
            {
                log.Error("uploaded masstort list is null");
                return new Collaterial();
            }

            if (loan == null)
            {
                log.Error("loan is null");
                return null;
            }

            List<Collaterial> coll = sQLCollaterialQueries.searchCaseCaseID(masstort.CaseID);
            if(coll != null && coll.Count >= 1)
            {
                return coll[0];
            }
          
            collaterialListByLoanId = collaterialController.getActiveCases(loan);
            //grab all existing cases for this perticular loan id


            int index = 0;


            res = true;
            for (int j = 0; j < collaterialListByLoanId.Count; j++)
            {
                Collaterial c = collaterialListByLoanId[j];

                //check to see if borrower caes id is null
                if (c.BorrowerCaseID != null & masstort.BorrowerCaseID != null)
                {
                    //First case: if both borrower case id's are empty string , go to second check
                    if (c.BorrowerCaseID.Trim().Equals("") && masstort.BorrowerCaseID.Trim().Equals(""))
                    {
                        //test for nulls
                        if (masstort.Plaintiff != null && !masstort.Plaintiff.Trim().Equals(""))
                        {
                            if (c.Plaintiff.ToLower().Contains(masstort.Plaintiff.ToLower()))
                            {

                                collFound = c;


                                break;
                            }
                        }

                    }
                    //if both borrower case ids have real values, then check
                    else if (c.BorrowerCaseID.Trim().Equals(masstort.BorrowerCaseID.Trim()))
                    {
                        collFound = c;

                        break;
                    }

                }
                //else if both borrower case ids are null, go to second check
                if (res == true)
                {
                    //test for nulls
                    if (masstort.Plaintiff != null && !masstort.Plaintiff.Trim().Equals(""))
                    {
                        if (c.Plaintiff.ToLower().Contains(masstort.Plaintiff.ToLower()))
                        {


                            collFound = c;


                            break;
                        }
                    }
                }




            }
            return collFound;
        }


        //Goes through all upload masstort and determines if they are new or existing
        public List<MassTort> UploadMassTortIsItNewOrExisting(List<MassTort> uploadedMassTortList, Loan loan)
        {
            log.Info("uploaded MassTort is it new or existing");
            bool res = true;

            if (uploadedMassTortList == null)
            {
                log.Error("uploaded MassTort list is null");
                return new List<MassTort>();
            }

            if (loan == null)
            {
                log.Error("loan is null");
                return new List<MassTort>();
            }

            uploadedMassTortNewMassTortList = new List<MassTort>();
            uploadedMassTortUpdateMassTortList = new List<MassTort>();
            uploadedMassTortCaseTypeNotValidList = new List<MassTort>();
            uploadedMassTortCaseNotFoundList = new List<MassTort>();





            for (int i = 0; i < uploadedMassTortList.Count; i++)
            {
                bool notValidCaseType = false;
                MassTort u = uploadedMassTortList[i];

               // MassTort massTort = sQLMassTortQueries.searchMassTortByCaseID(u.CaseID);
               
                Collaterial coll = this.LookUpCase(u, loan);
              
                //If coll comes back with a value, check to see if its new or an update
                if (coll != null)
                {
                    u.BorrowerCaseID = coll.BorrowerCaseID;
                    u.Plaintiff = coll.Plaintiff;
                    //Look up to see if mass tort exists in the database
                    List<MassTort> t = sQLMassTortQueries.searchMassTortByMassTortID(coll.MassTortID, coll.CaseID);
                    //If it exists, check to see if it is a true update
                    if (t != null && t.Count == 1)
                    {
                        MassTort exist = t[0];
                        if (!ComboBoxPopulationController.caseTypeMassTortList.Contains(coll.CaseType) & !ComboBoxPopulationController.caseTypeMassTortList.Contains(u.CaseType))
                        {
                            uploadedMassTortCaseTypeNotValidList.Add(u);
                            notValidCaseType = true;
                        }



                        if (!notValidCaseType)
                        {
                            MassTort result = this.mergeUpdateMassTort(exist, u, coll);
                           
                            //if a true update, udate!  IF not do nothing
                            if (result != null)
                            {
                                result.BorrowerCaseID = coll.BorrowerCaseID;
                                result.Plaintiff = coll.Plaintiff;
                                uploadedMassTortUpdateMassTortList.Add(result);
                            }
                        }

                    }
                    //If it doesnt exists, add to database
                    else
                    {
                        u.CaseID = coll.CaseID;

                        if (!ComboBoxPopulationController.caseTypeMassTortList.Contains(coll.CaseType) & !ComboBoxPopulationController.caseTypeMassTortList.Contains(u.CaseType))
                        {
                            uploadedMassTortCaseTypeNotValidList.Add(u);
                            notValidCaseType = true;
                        }

                        if (!notValidCaseType)
                        {
                            if (!ComboBoxPopulationController.caseTypeMassTortList.Contains(u.CaseType))
                            {
                                u.CaseType = coll.CaseType;
                            }

                            bool exists = false;
                            for(int j = 0; j < uploadedMassTortNewMassTortList.Count; j++)
                            {
                                if (uploadedMassTortNewMassTortList[j].CaseID.Equals(u.CaseID))
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                uploadedMassTortNewMassTortList.Add(u);
                            }
                        }


                    }

                }
                //If coll comes back null, cannot find a match for loan's collaterial
                else
                {

                    uploadedMassTortCaseNotFoundList.Add(u);
                    //MessageBox.Show("case not found");
                }

            }
            return uploadedMassTortUpdateMassTortList;
        }

        public MassTort mergeUpdateMassTort(MassTort existingMassTort, MassTort updateMassTort, Collaterial coll)
        {
            if (existingMassTort == null)
            {
                return null;
            }
            if (updateMassTort == null)
            {
                return null;
            }
            if (coll == null)
            {
                return null;
            }
            bool unchanged = true;


            //CaseType
            //first case updated value is null
            if (updateMassTort.CaseType != null)
            {
                //second case esisting value is null
                if (existingMassTort.CaseType == null)
                {
                    existingMassTort.CaseType = updateMassTort.CaseType.Trim();
                    existingMassTort.CaseType = existingMassTort.CaseType + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.CaseType != null)
                {
                    if (!existingMassTort.CaseType.Trim().Equals(updateMassTort.CaseType.Trim()))
                    {
                        if (ComboBoxPopulationController.caseTypeMassTortList.Contains(coll.CaseType))
                        {
                            updateMassTort.CaseType = coll.CaseType.Trim();
                            existingMassTort.CaseType = existingMassTort.CaseType + "*";
                            unchanged = false;
                        }
                        else
                        {
                            existingMassTort.CaseType = updateMassTort.CaseType.Trim();
                            existingMassTort.CaseType = existingMassTort.CaseType + "*";
                            unchanged = false;
                        }
                    }
                }
            }



            //DateOfBirth
            //first case updated value is null
            if (updateMassTort.DateOfBirth != null)
            {
                //second case esisting value is null
                if (existingMassTort.DateOfBirth == null)
                {
                    existingMassTort.DateOfBirth = updateMassTort.DateOfBirth.Trim();
                    existingMassTort.DateOfBirth = existingMassTort.DateOfBirth + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.DateOfBirth != null)
                {
                    if (!existingMassTort.DateOfBirth.Trim().Equals(updateMassTort.DateOfBirth.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingMassTort.DateOfBirth, out existingDate)) & (DateTime.TryParse(updateMassTort.DateOfBirth, out updateDate)))
                        {
                            //if value < zero,arg 1 si earlier than arg 2
                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingMassTort.DateOfBirth = updateMassTort.DateOfBirth.Trim();
                                existingMassTort.DateOfBirth = existingMassTort.DateOfBirth + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //Gender
            //first case updated value is null
            if (updateMassTort.Gender != null)
            {
                //second case esisting value is null
                if (existingMassTort.Gender == null)
                {
                    existingMassTort.Gender = updateMassTort.Gender.Trim();
                    existingMassTort.Gender = existingMassTort.Gender + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.Gender != null)
                {
                    if (!existingMassTort.Gender.Trim().Equals(updateMassTort.Gender.Trim()))
                    {
                        if (!(existingMassTort.Gender.Trim().ToUpper().Equals("") & updateMassTort.Gender.Trim().ToUpper().Equals("NA")))
                        {
                            if (!(existingMassTort.Gender.Trim().Equals("NA") & updateMassTort.Gender.Trim().Equals("")))
                            {

                                existingMassTort.Gender = updateMassTort.Gender.Trim();
                                existingMassTort.Gender = existingMassTort.Gender + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //UsCitizen
            //first case updated value is null
            if (updateMassTort.UsCitizen != null)
            {
                //second case esisting value is null
                if (existingMassTort.UsCitizen == null)
                {
                    existingMassTort.UsCitizen = updateMassTort.UsCitizen.Trim();
                    existingMassTort.UsCitizen = existingMassTort.UsCitizen + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.UsCitizen != null)
                {
                    if (!existingMassTort.UsCitizen.Trim().ToUpper().Equals(updateMassTort.UsCitizen.Trim().ToUpper()))
                    {
                        if (!(existingMassTort.UsCitizen.Trim().Equals("") & updateMassTort.UsCitizen.Trim().Equals("NA")))
                        {
                            if (!(existingMassTort.UsCitizen.Trim().Equals("NA") & updateMassTort.UsCitizen.Trim().Equals("")))
                            {

                                existingMassTort.UsCitizen = updateMassTort.UsCitizen.Trim();
                                existingMassTort.UsCitizen = existingMassTort.UsCitizen + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //DrugProductName
            //first case updated value is null
            if (updateMassTort.DrugProductName != null)
            {
                //second case esisting value is null
                if (existingMassTort.DrugProductName == null)
                {
                    existingMassTort.DrugProductName = updateMassTort.DrugProductName.Trim();
                    existingMassTort.DrugProductName = existingMassTort.DrugProductName + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.DrugProductName != null)
                {
                    if (!existingMassTort.DrugProductName.Trim().Equals(updateMassTort.DrugProductName.Trim()))
                    {

                        existingMassTort.DrugProductName = updateMassTort.DrugProductName.Trim();
                        existingMassTort.DrugProductName = existingMassTort.DrugProductName + "*";
                        unchanged = false;
                    }
                }
            }

            //ProductID
            //first case updated value is null
            if (updateMassTort.ProductID != null)
            {
                //second case esisting value is null
                if (existingMassTort.ProductID == null)
                {
                    existingMassTort.ProductID = updateMassTort.ProductID.Trim();
                    existingMassTort.ProductID = existingMassTort.ProductID + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.ProductID != null)
                {
                    if (!existingMassTort.ProductID.Trim().Equals(updateMassTort.ProductID.Trim()))
                    {

                        existingMassTort.ProductID = updateMassTort.ProductID.Trim();
                        existingMassTort.ProductID = existingMassTort.ProductID + "*";
                        unchanged = false;
                    }
                }
            }

            //ProofOfUseProductIDExposure
            //first case updated value is null
            if (updateMassTort.ProofOfUseProductIDExposure != null)
            {
                //second case esisting value is null
                if (existingMassTort.ProofOfUseProductIDExposure == null)
                {
                    existingMassTort.ProofOfUseProductIDExposure = updateMassTort.ProofOfUseProductIDExposure.Trim();
                    existingMassTort.ProofOfUseProductIDExposure = existingMassTort.ProofOfUseProductIDExposure + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.ProofOfUseProductIDExposure != null)
                {
                    if (!existingMassTort.ProofOfUseProductIDExposure.Trim().ToUpper().Equals(updateMassTort.ProofOfUseProductIDExposure.Trim().ToUpper()))
                    {
                        if (!(existingMassTort.ProofOfUseProductIDExposure.Trim().Equals("") & updateMassTort.ProofOfUseProductIDExposure.Trim().Equals("NA")))
                        {
                            if (!(existingMassTort.ProofOfUseProductIDExposure.Trim().Equals("NA") & updateMassTort.ProofOfUseProductIDExposure.Trim().Equals("")))
                            {
                                existingMassTort.ProofOfUseProductIDExposure = updateMassTort.ProofOfUseProductIDExposure.Trim();
                                existingMassTort.ProofOfUseProductIDExposure = existingMassTort.ProofOfUseProductIDExposure + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //DateProofOfUseProductIDExposure
            //first case updated value is null
            if (updateMassTort.DateProofOfUseProductIDExposure != null)
            {
                //second case esisting value is null
                if (existingMassTort.DateProofOfUseProductIDExposure == null)
                {
                    existingMassTort.DateProofOfUseProductIDExposure = updateMassTort.DateProofOfUseProductIDExposure.Trim();
                    existingMassTort.DateProofOfUseProductIDExposure = existingMassTort.DateProofOfUseProductIDExposure + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.DateProofOfUseProductIDExposure != null)
                {
                    if (!existingMassTort.DateProofOfUseProductIDExposure.Trim().Equals(updateMassTort.DateProofOfUseProductIDExposure.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingMassTort.DateProofOfUseProductIDExposure, out existingDate)) & (DateTime.TryParse(updateMassTort.DateProofOfUseProductIDExposure, out updateDate)))
                        {
                            //if value < zero,arg 1 si earlier than arg 2
                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingMassTort.DateProofOfUseProductIDExposure = updateMassTort.DateProofOfUseProductIDExposure.Trim();
                                existingMassTort.DateProofOfUseProductIDExposure = existingMassTort.DateProofOfUseProductIDExposure + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //DurationUsageImplantExposureMonths
            //first case updated value is null
            if (updateMassTort.DurationUsageImplantExposureMonths != null)
            {
                //second case esisting value is null
                if (existingMassTort.DurationUsageImplantExposureMonths == null)
                {
                    existingMassTort.DurationUsageImplantExposureMonths = updateMassTort.DurationUsageImplantExposureMonths.Trim();
                    existingMassTort.DurationUsageImplantExposureMonths = existingMassTort.DurationUsageImplantExposureMonths + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.DurationUsageImplantExposureMonths != null)
                {
                    if (!existingMassTort.DurationUsageImplantExposureMonths.Trim().Equals(updateMassTort.DurationUsageImplantExposureMonths.Trim()))
                    {

                        existingMassTort.DurationUsageImplantExposureMonths = updateMassTort.DurationUsageImplantExposureMonths.Trim();
                        existingMassTort.DurationUsageImplantExposureMonths = existingMassTort.DurationUsageImplantExposureMonths + "*";
                        unchanged = false;
                    }
                }
            }



            //SolExpirationDate
            //first case updated value is null
            if (updateMassTort.SolExpirationDate != null)
            {
                //second case esisting value is null
                if (existingMassTort.SolExpirationDate == null)
                {
                    existingMassTort.SolExpirationDate = updateMassTort.SolExpirationDate.Trim();
                    existingMassTort.SolExpirationDate = existingMassTort.SolExpirationDate + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.SolExpirationDate != null)
                {
                    if (!existingMassTort.SolExpirationDate.Trim().Equals(updateMassTort.SolExpirationDate.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingMassTort.SolExpirationDate, out existingDate)) & (DateTime.TryParse(updateMassTort.SolExpirationDate, out updateDate)))
                        {

                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingMassTort.SolExpirationDate = updateMassTort.SolExpirationDate.Trim();
                                existingMassTort.SolExpirationDate = existingMassTort.SolExpirationDate + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //MedicalRecords
            //first case updated value is null
            if (updateMassTort.MedicalRecords != null)
            {
                //second case esisting value is null
                if (existingMassTort.MedicalRecords == null)
                {
                    existingMassTort.MedicalRecords = updateMassTort.MedicalRecords.Trim();
                    existingMassTort.MedicalRecords = existingMassTort.MedicalRecords + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.MedicalRecords != null)
                {
                    if (!existingMassTort.MedicalRecords.Trim().ToUpper().Equals(updateMassTort.MedicalRecords.Trim().ToUpper()))
                    {
                        if (!(existingMassTort.MedicalRecords.Trim().Equals("") & updateMassTort.MedicalRecords.Trim().Equals("NA")))
                        {
                            if (!(existingMassTort.MedicalRecords.Trim().Equals("NA") & updateMassTort.MedicalRecords.Trim().Equals("")))
                            {

                                existingMassTort.MedicalRecords = updateMassTort.MedicalRecords.Trim();
                                existingMassTort.MedicalRecords = existingMassTort.MedicalRecords + "*";
                                unchanged = false;

                            }
                        }
                    }
                }
            }

            //ImagingPhotographicEvidence
            //first case updated value is null
            if (updateMassTort.ImagingPhotographicEvidence != null)
            {
                //second case esisting value is null
                if (existingMassTort.ImagingPhotographicEvidence == null)
                {
                    existingMassTort.ImagingPhotographicEvidence = updateMassTort.ImagingPhotographicEvidence.Trim();
                    existingMassTort.ImagingPhotographicEvidence = existingMassTort.ImagingPhotographicEvidence + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.ImagingPhotographicEvidence != null)
                {
                    if (!existingMassTort.ImagingPhotographicEvidence.Trim().ToUpper().Equals(updateMassTort.ImagingPhotographicEvidence.Trim().ToUpper()))
                    {
                        if (!(existingMassTort.ImagingPhotographicEvidence.Trim().Equals("") & updateMassTort.ImagingPhotographicEvidence.Trim().Equals("NA")))
                        {
                            if (!(existingMassTort.ImagingPhotographicEvidence.Trim().Equals("NA") & updateMassTort.ImagingPhotographicEvidence.Trim().Equals("")))
                            {

                                existingMassTort.ImagingPhotographicEvidence = updateMassTort.ImagingPhotographicEvidence.Trim();
                                existingMassTort.ImagingPhotographicEvidence = existingMassTort.ImagingPhotographicEvidence + "*";
                                unchanged = false;

                            }
                        }
                    }
                }
            }


            //Diagnosis
            //first case updated value is null
            if (updateMassTort.Diagnosis != null)
            {
                //second case esisting value is null
                if (existingMassTort.Diagnosis == null)
                {
                    existingMassTort.Diagnosis = updateMassTort.Diagnosis.Trim();
                    existingMassTort.Diagnosis = existingMassTort.Diagnosis + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.Diagnosis != null)
                {
                    if (!existingMassTort.Diagnosis.Trim().Equals(updateMassTort.Diagnosis.Trim()))
                    {

                        existingMassTort.Diagnosis = updateMassTort.Diagnosis.Trim();
                        existingMassTort.Diagnosis = existingMassTort.Diagnosis + "*";
                        unchanged = false;

                    }
                }
            }


            //AgeAtDiagnosis
            //first case updated value is null
            if (updateMassTort.AgeAtDiagnosis != null)
            {
                //second case esisting value is null
                if (existingMassTort.AgeAtDiagnosis == null)
                {
                    existingMassTort.AgeAtDiagnosis = updateMassTort.AgeAtDiagnosis.Trim();
                    existingMassTort.AgeAtDiagnosis = existingMassTort.AgeAtDiagnosis + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.AgeAtDiagnosis != null)
                {
                    if (!existingMassTort.AgeAtDiagnosis.Trim().Equals(updateMassTort.AgeAtDiagnosis.Trim()))
                    {
                        if (!(existingMassTort.AgeAtDiagnosis.Trim().ToUpper().Equals("") & updateMassTort.AgeAtDiagnosis.Trim().ToUpper().Equals("0")))
                        {
                            if (!(existingMassTort.AgeAtDiagnosis.Trim().Equals("0") & updateMassTort.AgeAtDiagnosis.Trim().Equals("")))
                            {

                                existingMassTort.AgeAtDiagnosis = updateMassTort.AgeAtDiagnosis.Trim();
                                existingMassTort.AgeAtDiagnosis = existingMassTort.AgeAtDiagnosis + "*";
                                unchanged = false;

                            }
                        }
                    }
                }
            }

            //DaysHospitalized
            //first case updated value is null
            if (updateMassTort.DaysHospitalized != null)
            {
                //second case esisting value is null
                if (existingMassTort.DaysHospitalized == null)
                {
                    existingMassTort.DaysHospitalized = updateMassTort.DaysHospitalized.Trim();
                    existingMassTort.DaysHospitalized = existingMassTort.DaysHospitalized + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.DaysHospitalized != null)
                {
                    if (!existingMassTort.DaysHospitalized.Trim().Equals(updateMassTort.DaysHospitalized.Trim()))
                    {
                        if (!(existingMassTort.DaysHospitalized.Trim().ToUpper().Equals("") & updateMassTort.DaysHospitalized.Trim().ToUpper().Equals("0")))
                        {
                            if (!(existingMassTort.DaysHospitalized.Trim().Equals("0") & updateMassTort.DaysHospitalized.Trim().Equals("")))
                            {

                                existingMassTort.DaysHospitalized = updateMassTort.DaysHospitalized.Trim();
                                existingMassTort.DaysHospitalized = existingMassTort.DaysHospitalized + "*";
                                unchanged = false;

                            }
                        }
                    }
                }
            }

            //Treatment
            //first case updated value is null
            if (updateMassTort.Treatment != null)
            {
                //second case esisting value is null
                if (existingMassTort.Treatment == null)
                {
                    existingMassTort.Treatment = updateMassTort.Treatment.Trim();
                    existingMassTort.Treatment = existingMassTort.Treatment + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.Treatment != null)
                {
                    if (!existingMassTort.Treatment.Trim().Equals(updateMassTort.Treatment.Trim()))
                    {

                        existingMassTort.Treatment = updateMassTort.Treatment.Trim();
                        existingMassTort.Treatment = existingMassTort.Treatment + "*";
                        unchanged = false;

                    }
                }
            }


            //TotalNumberOfSurgeries
            //first case updated value is null
            if (updateMassTort.TotalNumberOfSurgeries != null)
            {
                //second case esisting value is null
                if (existingMassTort.TotalNumberOfSurgeries == null)
                {
                    existingMassTort.TotalNumberOfSurgeries = updateMassTort.TotalNumberOfSurgeries.Trim();
                    existingMassTort.TotalNumberOfSurgeries = existingMassTort.TotalNumberOfSurgeries + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.TotalNumberOfSurgeries != null)
                {
                    if (!existingMassTort.TotalNumberOfSurgeries.Trim().Equals(updateMassTort.TotalNumberOfSurgeries.Trim()))
                    {


                        existingMassTort.TotalNumberOfSurgeries = updateMassTort.TotalNumberOfSurgeries.Trim();
                        existingMassTort.TotalNumberOfSurgeries = existingMassTort.TotalNumberOfSurgeries + "*";
                        unchanged = false;

                    }
                }
            }


            //DateOfFirstSurgery
            //first case updated value is null
            if (updateMassTort.DateOfFirstSurgery != null)
            {
                //second case esisting value is null
                if (existingMassTort.DateOfFirstSurgery == null)
                {
                    existingMassTort.DateOfFirstSurgery = updateMassTort.DateOfFirstSurgery.Trim();
                    existingMassTort.DateOfFirstSurgery = existingMassTort.DateOfFirstSurgery + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.DateOfFirstSurgery != null)
                {
                    if (!existingMassTort.DateOfFirstSurgery.Trim().Equals(updateMassTort.DateOfFirstSurgery.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingMassTort.DateOfFirstSurgery, out existingDate)) & (DateTime.TryParse(updateMassTort.DateOfFirstSurgery, out updateDate)))
                        {

                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingMassTort.DateOfFirstSurgery = updateMassTort.DateOfFirstSurgery.Trim();
                                existingMassTort.DateOfFirstSurgery = existingMassTort.DateOfFirstSurgery + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //ExtraordinaryInjury
            //first case updated value is null
            if (updateMassTort.ExtraordinaryInjury != null)
            {
                //second case esisting value is null
                if (existingMassTort.ExtraordinaryInjury == null)
                {
                    existingMassTort.ExtraordinaryInjury = updateMassTort.ExtraordinaryInjury.Trim();
                    existingMassTort.ExtraordinaryInjury = existingMassTort.ExtraordinaryInjury + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.ExtraordinaryInjury != null)
                {
                    if (!existingMassTort.ExtraordinaryInjury.Trim().Equals(updateMassTort.ExtraordinaryInjury.Trim()))
                    {

                        existingMassTort.ExtraordinaryInjury = updateMassTort.ExtraordinaryInjury.Trim();
                        existingMassTort.ExtraordinaryInjury = existingMassTort.ExtraordinaryInjury + "*";
                        unchanged = false;


                    }
                }
            }

            //RiskFactorsAndOtherAdjustments
            //first case updated value is null
            if (updateMassTort.RiskFactorsAndOtherAdjustments != null)
            {
                //second case esisting value is null
                if (existingMassTort.RiskFactorsAndOtherAdjustments == null)
                {
                    existingMassTort.RiskFactorsAndOtherAdjustments = updateMassTort.RiskFactorsAndOtherAdjustments.Trim();
                    existingMassTort.RiskFactorsAndOtherAdjustments = existingMassTort.RiskFactorsAndOtherAdjustments + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.RiskFactorsAndOtherAdjustments != null)
                {
                    if (!existingMassTort.RiskFactorsAndOtherAdjustments.Trim().Equals(updateMassTort.RiskFactorsAndOtherAdjustments.Trim()))
                    {


                        existingMassTort.RiskFactorsAndOtherAdjustments = updateMassTort.RiskFactorsAndOtherAdjustments.Trim();
                        existingMassTort.RiskFactorsAndOtherAdjustments = existingMassTort.RiskFactorsAndOtherAdjustments + "*";
                        unchanged = false;

                    }
                }
            }

            //Defendant2
            //first case updated value is null
            if (updateMassTort.Defendant2 != null)
            {
                //second case esisting value is null
                if (existingMassTort.Defendant2 == null)
                {
                    existingMassTort.Defendant2 = updateMassTort.Defendant2.Trim();
                    existingMassTort.Defendant2 = existingMassTort.Defendant2 + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.Defendant2 != null)
                {
                    if (!existingMassTort.Defendant2.Trim().Equals(updateMassTort.Defendant2.Trim()))
                    {


                        existingMassTort.Defendant2 = updateMassTort.Defendant2.Trim();
                        existingMassTort.Defendant2 = existingMassTort.Defendant2 + "*";
                        unchanged = false;

                    }
                }
            }

            //Defendant3
            //first case updated value is null
            if (updateMassTort.Defendant3 != null)
            {
                //second case esisting value is null
                if (existingMassTort.Defendant3 == null)
                {
                    existingMassTort.Defendant3 = updateMassTort.Defendant3.Trim();
                    existingMassTort.Defendant3 = existingMassTort.Defendant3 + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.Defendant3 != null)
                {
                    if (!existingMassTort.Defendant3.Trim().Equals(updateMassTort.Defendant3.Trim()))
                    {

                        existingMassTort.Defendant3 = updateMassTort.Defendant3.Trim();
                        existingMassTort.Defendant3 = existingMassTort.Defendant3 + "*";
                        unchanged = false;

                    }
                }
            }


            //CbfFeePercent
            //first case updated value is null
            if (updateMassTort.CbfFeePercent != null)
            {
                //second case esisting value is null
                if (existingMassTort.CbfFeePercent == null)
                {
                    existingMassTort.CbfFeePercent = updateMassTort.CbfFeePercent.Trim();
                    existingMassTort.CbfFeePercent = existingMassTort.CbfFeePercent + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.CbfFeePercent != null)
                {
                    if (!existingMassTort.CbfFeePercent.Trim().Equals(updateMassTort.CbfFeePercent.Trim()))
                    {
                        if (!(existingMassTort.CbfFeePercent.Trim().Equals("") & updateMassTort.CbfFeePercent.Trim().Equals("0")))
                        {
                            if (!(existingMassTort.CbfFeePercent.Trim().Equals("0") & updateMassTort.CbfFeePercent.Trim().Equals("")))
                            {
                                existingMassTort.CbfFeePercent = updateMassTort.CbfFeePercent.Trim();
                                existingMassTort.CbfFeePercent = existingMassTort.CbfFeePercent + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //CbfFeeDollar
            //first case updated value is null
            if (updateMassTort.CbfFeeDollar != null)
            {
                //second case esisting value is null
                if (existingMassTort.CbfFeeDollar == null)
                {
                    existingMassTort.CbfFeeDollar = updateMassTort.CbfFeeDollar.Trim(); existingMassTort.CbfFeeDollar = existingMassTort.CbfFeeDollar + "*";

                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.CbfFeeDollar != null)
                {
                    if (!existingMassTort.CbfFeeDollar.Trim().Equals(updateMassTort.CbfFeeDollar.Trim()))
                    {
                        if (!(existingMassTort.CbfFeeDollar.Trim().Equals("") & updateMassTort.CbfFeeDollar.Trim().Equals("0")))
                        {
                            if (!(existingMassTort.CbfFeeDollar.Trim().Equals("0") & updateMassTort.CbfFeeDollar.Trim().Equals("")))
                            {

                                existingMassTort.CbfFeeDollar = updateMassTort.CbfFeeDollar.Trim();
                                existingMassTort.CbfFeeDollar = existingMassTort.CbfFeeDollar + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //AttorneyFeeForDistribution
            //first case updated value is null
            if (updateMassTort.AttorneyFeeForDistribution != null)
            {
                //second case esisting value is null
                if (existingMassTort.AttorneyFeeForDistribution == null)
                {
                    existingMassTort.AttorneyFeeForDistribution = updateMassTort.AttorneyFeeForDistribution.Trim();
                    existingMassTort.AttorneyFeeForDistribution = existingMassTort.AttorneyFeeForDistribution + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.AttorneyFeeForDistribution != null)
                {
                    if (!existingMassTort.AttorneyFeeForDistribution.Trim().Equals(updateMassTort.AttorneyFeeForDistribution.Trim()))
                    {

                        if (!(existingMassTort.AttorneyFeeForDistribution.Trim().Equals("") & updateMassTort.AttorneyFeeForDistribution.Trim().Equals("0")))
                        {
                            if (!(existingMassTort.AttorneyFeeForDistribution.Trim().Equals("0") & updateMassTort.AttorneyFeeForDistribution.Trim().Equals("")))
                            {


                                existingMassTort.AttorneyFeeForDistribution = updateMassTort.AttorneyFeeForDistribution.Trim();
                                existingMassTort.AttorneyFeeForDistribution = existingMassTort.AttorneyFeeForDistribution + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //Notes
            //first case updated value is null
            if (updateMassTort.Notes != null)
            {
                //second case esisting value is null
                if (existingMassTort.Notes == null)
                {
                    existingMassTort.Notes = updateMassTort.Notes.Trim();
                    existingMassTort.Notes = existingMassTort.Notes + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingMassTort.Notes != null)
                {
                    if (!existingMassTort.Notes.Trim().Equals(updateMassTort.Notes.Trim()))
                    {


                        existingMassTort.Notes = updateMassTort.Notes.Trim();
                        existingMassTort.Notes = existingMassTort.Notes + "*";
                        unchanged = false;

                    }
                }
            }

            //add new columns here
            if (unchanged == true)
            {
                return null;
            }
            return existingMassTort;
        }


        //gets a list of active cases
        public List<MassTort> getMassTortByLoanID(Loan loan)
        {
            log.Info("Get a list of all cases");
            List<MassTort> masstortList = sQLMassTortQueries.searchMassTortByLoanID(loan.LoanID);
            if (masstortList == null)
            {
                log.Error("list of all cases returned null");
                return new List<MassTort>();
            }
            for(int i = 0; i < masstortList.Count; i++)
            {
                MassTort mt = masstortList[i];
                string caseId = mt.CaseID;
                List<Collaterial> collList = sQLCollaterialQueries.searchCaseCaseID(caseId);
                if(collList.Count >= 1)
                {
                    masstortList[i].Plaintiff = collList[0].Plaintiff;
                    masstortList[i].BorrowerCaseID = collList[0].BorrowerCaseID;
                }

            }

            return masstortList;
        }



        public bool containsCaseId(List<MassTort> list, MassTort c)
        {
            var matches = list.Where(p => p.CaseID == c.CaseID);

            return true;
        }

        public List<MassTort> getUploadedCasesThatWillBeNewCases()
        {
            return this.uploadedMassTortNewMassTortList;

        }

        public List<MassTort> getUploadedMassTortCaseTypeNotValidList()
        {
            return this.uploadedMassTortCaseTypeNotValidList;

        }





        public List<MassTort> getUploadedCasesThatWillBeUpatedCases()
        {
            return this.uploadedMassTortUpdateMassTortList;

        }

        public List<MassTort> getUploadedCasesThatAreNotFound()
        {
            return this.uploadedMassTortCaseNotFoundList;

        }

        public List<MassTort> searchMassTortByMassTortIDLoanID(string massTortID, string loanID)
        {
            List<MassTort> masstortList = sQLMassTortQueries.searchMassTortByMassTortIDLoanID(massTortID, loanID);
            if (masstortList == null)
            {
                log.Error("list of all cases returned null");
                return new List<MassTort>();
            }

            return masstortList;
        }

    }
}
