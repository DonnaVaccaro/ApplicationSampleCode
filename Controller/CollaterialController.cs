using CaseManager.Model;
using OTS.DataBase;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;

namespace OTS.Controller
{
    public class CollaterialController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CollaterialController));
        private readonly SQLLoanQueries sQLLoanQueries;
        private readonly SQLBorrowerQueries sQLBorrowerQueries;
        private readonly SQLCollaterialQueries sQLCollaterialQueries;
        private readonly CaseModificationController caseModificationController;
        private Collaterial currentCase;
        private Collaterial updatedCase;
        private List<Collaterial> uploadedCaseNewCaseList;
        private List<Collaterial> uploadedCaseUpdateCaseList;
        private List<Collaterial> deletedCaseList;
        private List<Collaterial> collaterialListByLoanId;
       // private AccessDatabaseLoanLPSController accessDatabaseCollaterialLPSController;


        public CollaterialController()
        {
            log.Info("Constructor Case Controller");
            currentCase = new Collaterial();
            updatedCase = new Collaterial();
            sQLLoanQueries = new SQLLoanQueries();
            sQLBorrowerQueries = new SQLBorrowerQueries();
            sQLCollaterialQueries = new SQLCollaterialQueries();
            uploadedCaseNewCaseList = new List<Collaterial>();
            uploadedCaseUpdateCaseList = new List<Collaterial>();
            deletedCaseList = new List<Collaterial>();
            caseModificationController = new CaseModificationController();
           



        }

        public void setCurrentCase(Collaterial collaterial)
        {
            this.currentCase = collaterial;
        }

        public Collaterial getCurrentCase()
        {
            return this.currentCase;
        }

        public void setUpdatedCase(Collaterial updatedcollaterial)
        {
            this.updatedCase = updatedcollaterial;
        }

        public Collaterial getUpdatedCase()
        {
            return this.updatedCase;
        }


        //gets a list of all cases
        public List<Collaterial> getAllCases()
        {
            log.Info("Get a list of all cases");
            List<Collaterial> allCasesList = sQLCollaterialQueries.getAllCases();
            if (allCasesList == null)
            {
                log.Error("list of all cases returned null");
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            allCasesList.Sort(comparison);
            return allCasesList;
        }

        public List<CaseListTemplate> searchCaseLoanIDIntoCaseListTemplate(string loanID)
        {
            List<CaseListTemplate> table = sQLCollaterialQueries.searchCaseLoanIDIntoCaseListTemplate(loanID);
            for(int i = 0; i < table.Count; i++)
            {
                string jur = table[i].Jurisdiction;
                string value = StateCourtLookup.getJurdictionValue(jur);
                string st = table[i].CourtVenue;
                if (value != null)
                {
                    if (st.Equals("NA"))
                    {
                        table[i].CourtVenue = value;
                    }
                }

               
            }
          
          
            return table;

        }
            //gets a list of active cases
            public List<Collaterial> getActiveCases(Loan loan)
        {
            log.Info("Get a list of all cases");
            List<Collaterial> activeCasesList = sQLCollaterialQueries.getActiveCases(loan);
            if (activeCasesList == null)
            {
                log.Error("list of all cases returned null");
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            activeCasesList.Sort(comparison);
            return activeCasesList;
        }

        //gets a list of active cases
        public List<Collaterial> getDroppedCases(Loan loan)
        {
            log.Info("Get a list of dropped cases");
            List<Collaterial> droppedCasesList = sQLCollaterialQueries.getDroppedCases(loan);
            if (droppedCasesList == null)
            {
                log.Error("list of all cases returned null");
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            droppedCasesList.Sort(comparison);
            return droppedCasesList;
        }


        //Goes through all upload cases and determines if they are new or existing
        public List<Collaterial> UploadCasesIsItNewExistingOrSettledBackup(List<Collaterial> uploadedCollaterialList, Loan loan)
        {
            log.Info("uploaded cases is it new or existing");
            bool res = true;
            if (uploadedCollaterialList == null)
            {
                log.Error("uploaded collateral list is null");
                return new List<Collaterial>();
            }

            if (loan == null)
            {
                log.Error("loan is null");
                return new List<Collaterial>();
            }

            uploadedCaseNewCaseList = new List<Collaterial>();
            uploadedCaseUpdateCaseList = new List<Collaterial>();
            deletedCaseList = new List<Collaterial>();
            collaterialListByLoanId = new List<Collaterial>();
            //grab all existing cases for this perticular loan id
            collaterialListByLoanId = this.searchCaseLoanID(loan.LoanID);
            //this list we will remove each case that we match as an update so at the end
            //all thats left is the deleted cases
            deletedCaseList = this.getActiveCases(loan);
            if (collaterialListByLoanId == null)
            {
                log.Error("list of cases by loan id returned null");
                return new List<Collaterial>();
            }
            int index = 0;
            //for each existing case, see if there could be a match, which will mean its a update
            //not a new case
            for (int i = 0; i < uploadedCollaterialList.Count; i++)
            //foreach (Collaterial u in uploadedCollaterialList)
            {
                
                Collaterial u = uploadedCollaterialList[i];
               
                res = true;
                for (int j = 0; j < collaterialListByLoanId.Count; j++)
                {
                    Collaterial c = collaterialListByLoanId[j];
                   
                    //check to see if borrower caes id is null
                    if (c.BorrowerCaseID != null && u.BorrowerCaseID != null )
                    {
                        //First case: if both borrower case id's are empty string , go to second check
                        if (c.BorrowerCaseID.Trim().Equals("") && u.BorrowerCaseID.Trim().Equals(""))
                        {
                            //test for nulls
                            if (u.Plaintiff != null && !u.Plaintiff.Trim().Equals("") && u.PrimaryDefendant != null && !u.PrimaryDefendant.Trim().Equals(""))
                            {
                                if ((c.Plaintiff.ToLower().Trim().Equals(u.Plaintiff.ToLower().Trim()) && c.PrimaryDefendant.ToLower().Trim().Equals(u.PrimaryDefendant.ToLower().Trim()))  || (c.Plaintiff.ToLower().Trim().Equals(u.Plaintiff.ToLower().Trim()) && c.DocketNumber.ToLower().Trim().Equals(u.DocketNumber.ToLower().Trim())))
                                {


                                    //this case will be an update
                                    Collaterial mergedCase = this.mergeUpdateCase(c, u);
                                    if (mergedCase != null)
                                    {

                                        uploadedCaseUpdateCaseList.Add(mergedCase);

                                        collaterialListByLoanId.RemoveAt(j);
                                        res = false;
                                        break;
                                    }
                                    collaterialListByLoanId.RemoveAt(j);
                                    res = false;
                                    break;
                                }
                            }

                        }
                        //if both borrower case ids have real values, then check
                        else if (c.BorrowerCaseID.Trim().Equals(u.BorrowerCaseID.Trim()))
                        {


                            //this case will be an update
                            Collaterial mergedCase = this.mergeUpdateCase(c, u);
                            if (mergedCase != null)
                            {
                                if (containsCaseId(uploadedCaseUpdateCaseList, mergedCase) == true)
                                {
                                    uploadedCaseUpdateCaseList.Add(mergedCase);
                                }
                                collaterialListByLoanId.RemoveAt(j);
                                res = false;
                                break;
                            }
                            collaterialListByLoanId.RemoveAt(j);
                            res = false;
                            break;
                        }

                    }
                    //else if both borrower case ids are null, go to second check
                    if (res == true)
                    {
                        //test for nulls
                        if (u.Plaintiff != null && !u.Plaintiff.Trim().Equals(""))
                        {
                            if ((c.Plaintiff.ToLower().Trim().Equals(u.Plaintiff.ToLower().Trim()) && c.PrimaryDefendant.ToLower().Trim().Equals(u.PrimaryDefendant.ToLower().Trim())) || (c.Plaintiff.ToLower().Trim().Equals(u.Plaintiff.ToLower().Trim()) && c.DocketNumber.ToLower().Trim().Equals(u.DocketNumber.ToLower().Trim())))
                            {


                                //this case will be an update
                                Collaterial mergedCase = this.mergeUpdateCase(c, u);
                                if (mergedCase != null)
                                {
                                    if (containsCaseId(uploadedCaseUpdateCaseList, mergedCase) == true)
                                    {
                                        uploadedCaseUpdateCaseList.Add(mergedCase);
                                    }
                                    collaterialListByLoanId.RemoveAt(j);
                                    res = false;
                                    break;
                                }
                                collaterialListByLoanId.RemoveAt(j);
                                res = false;
                                break;
                            }
                        }
                    }
                }

                //this if statement needs to be out of the existing case loop becasue
                //if the case id is null there is no match and if the borrower case id is not a match
                //this means its a new case
                if (res)
                {
                    uploadedCaseNewCaseList.Add(u);
                }
            }
            return uploadedCaseUpdateCaseList;
        }


        //Goes through all upload cases and determines if they are new or existing
        public List<Collaterial> UploadCasesIsItNewExistingOrSettled(List<Collaterial> uploadedCollaterialList, Loan loan)
        {
            log.Info("uploaded cases is it new or existing");
            bool res = true;
            if (uploadedCollaterialList == null)
            {
                log.Error("uploaded collateral list is null");
                return new List<Collaterial>();
            }

            if (loan == null)
            {
                log.Error("loan is null");
                return new List<Collaterial>();
            }

            uploadedCaseNewCaseList = new List<Collaterial>();
            uploadedCaseUpdateCaseList = new List<Collaterial>();
            deletedCaseList = new List<Collaterial>();
            collaterialListByLoanId = new List<Collaterial>();
            //grab all existing cases for this perticular loan id
            collaterialListByLoanId = this.searchCaseLoanID(loan.LoanID);
            //this list we will remove each case that we match as an update so at the end
            //all thats left is the deleted cases
            deletedCaseList = this.getActiveCases(loan);
            if (collaterialListByLoanId == null)
            {
                log.Error("list of cases by loan id returned null");
                return new List<Collaterial>();
            }
            int index = 0;
            //for each existing case, see if there could be a match, which will mean its a update
            //not a new case
            for (int i = 0; i < uploadedCollaterialList.Count; i++)
            //foreach (Collaterial u in uploadedCollaterialList)
            {

                Collaterial u = uploadedCollaterialList[i];
               

                res = true;
                for (int j = 0; j < collaterialListByLoanId.Count; j++)
                {
                    Collaterial c = collaterialListByLoanId[j];
                   
                    //First check borrower case id and plaintiff
                    if (u.BorrowerCaseID != null && c.BorrowerCaseID != null  && 
                        u.Plaintiff != null && c.Plaintiff != null  && 
                        !c.BorrowerCaseID.Trim().Equals("") && !u.BorrowerCaseID.Trim().Equals("")  && 
                        !c.Plaintiff.Trim().Equals("") && !u.Plaintiff.Trim().Equals("") && u.BorrowerCaseID.Trim().Equals(c.BorrowerCaseID.Trim()) && 
                        u.Plaintiff.Trim().Equals(c.Plaintiff.Trim()))
                    {
                        
                            //case match!

                            //will this case be an update
                            Collaterial mergedCase = this.mergeUpdateCase(c, u);
                            //if null its not an update
                            if (mergedCase != null)
                            {
                                // mergedCase is not null so its an update
                                //Look up case by case id, if case is found add to update case list
                                if (containsCaseId(uploadedCaseUpdateCaseList, mergedCase) == true)
                                {
                                    uploadedCaseUpdateCaseList.Add(mergedCase);
                                }
                                //remove from existing case list
                                collaterialListByLoanId.RemoveAt(j);
                                //set false so we know its not an add case
                                res = false;
                                break;
                            }
                            //case found but not a update case, nothing to do
                            //Remove from existing case list
                            collaterialListByLoanId.RemoveAt(j);
                            //set false so we know its not an add case
                            res = false;
                            break;
                        
                    }
                    else if(u.PrimaryDefendant != null && c.PrimaryDefendant != null && 
                        u.Plaintiff != null && c.Plaintiff != null &&
                         !c.PrimaryDefendant.Trim().Equals("") && !u.PrimaryDefendant.Trim().Equals("") &&
                        !c.Plaintiff.Trim().Equals("") && !u.Plaintiff.Trim().Equals("") &&
                        u.PrimaryDefendant.Trim().Equals(c.PrimaryDefendant.Trim()) && 
                        u.Plaintiff.Trim().Equals(c.Plaintiff.Trim()))
                    {
                       
                            //case match!

                            //will this case be an update
                            Collaterial mergedCase = this.mergeUpdateCase(c, u);
                            //if null its not an update
                            if (mergedCase != null)
                            {
                                // mergedCase is not null so its an update
                                //Look up case by case id, if case is found add to update case list
                                if (containsCaseId(uploadedCaseUpdateCaseList, mergedCase) == true)
                                {
                                    uploadedCaseUpdateCaseList.Add(mergedCase);
                                }
                                //remove from existing case list
                                collaterialListByLoanId.RemoveAt(j);
                                //set false so we know its not an add case
                                res = false;
                                break;
                            }
                            //case found but not a update case, nothing to do
                            //Remove from existing case list
                            collaterialListByLoanId.RemoveAt(j);
                            //set false so we know its not an add case
                            res = false;
                            break;
                       
                    }
                    else if (u.DocketNumber != null && c.DocketNumber != null && 
                        u.Plaintiff != null && c.Plaintiff != null &&
                        !c.DocketNumber.Trim().Equals("") && !u.DocketNumber.Trim().Equals("") &&
                        !c.Plaintiff.Trim().Equals("") && !u.Plaintiff.Trim().Equals("") &&
                        u.DocketNumber.Trim().Equals(c.DocketNumber.Trim()) && 
                        u.Plaintiff.Trim().Equals(c.Plaintiff.Trim()))
                    {
                       
                            //case match!

                            //will this case be an update
                            Collaterial mergedCase = this.mergeUpdateCase(c, u);
                            //if null its not an update
                            if (mergedCase != null)
                            {
                                // mergedCase is not null so its an update
                                //Look up case by case id, if case is found add to update case list
                                if (containsCaseId(uploadedCaseUpdateCaseList, mergedCase) == true)
                                {
                                    uploadedCaseUpdateCaseList.Add(mergedCase);
                                }
                                //remove from existing case list
                                collaterialListByLoanId.RemoveAt(j);
                                //set false so we know its not an add case
                                res = false;
                                break;
                            }
                            //case found but not a update case, nothing to do
                            //Remove from existing case list
                            collaterialListByLoanId.RemoveAt(j);
                            //set false so we know its not an add case
                            res = false;
                            break;
                        }

                    //else if (u.Plaintiff != null && c.Plaintiff != null && u.Plaintiff.Trim().Equals(c.Plaintiff.Trim()))
                    //{
                    //    //case match!

                    //    //will this case be an update
                    //    Collaterial mergedCase = this.mergeUpdateCase(c, u);
                    //    //if null its not an update
                    //    if (mergedCase != null)
                    //    {
                    //        // mergedCase is not null so its an update
                    //        //Look up case by case id, if case is found add to update case list
                    //        if (containsCaseId(uploadedCaseUpdateCaseList, mergedCase) == true)
                    //        {
                    //            uploadedCaseUpdateCaseList.Add(mergedCase);
                    //        }
                    //        //remove from existing case list
                    //        collaterialListByLoanId.RemoveAt(j);
                    //        //set false so we know its not an add case
                    //        res = false;
                    //        break;
                    //    }
                    //    //case found but not a update case, nothing to do
                    //    //Remove from existing case list
                    //    collaterialListByLoanId.RemoveAt(j);
                    //    //set false so we know its not an add case
                    //    res = false;
                    //    break;
                    //}

                }
                

                //this if statement needs to be out of the existing case loop 
                //if none of the matching conditions exist, then its a new a case.   Add to new case list
                if (res)
                {
                    uploadedCaseNewCaseList.Add(u);
                }
            }
            return uploadedCaseUpdateCaseList;
        }



        public bool containsCaseId(List<Collaterial> list, Collaterial c)
        {
            var matches = list.Where(p => p.CaseID == c.CaseID);

            return true;
        }

        public bool removeCase(List<Collaterial> list, Collaterial c)
        {
            IEnumerable<Collaterial> matches = list.Where(p => p.CaseID == c.CaseID);
            if (matches != null)
            {
                for (int i = 0; i < matches.Count(); i++)
                {
                    list.RemoveAt(i);
                }
            }

            return true;
        }
        public Collaterial mergeUpdateCase(Collaterial existingCase, Collaterial updateCase)
        {
            if (existingCase == null)
            {
                return null;
            }
            if (updateCase == null)
            {
                return null;
            }
            bool unchanged = true;

            //Borrower Case ID
            //first case updated value is null
            if (updateCase.BorrowerCaseID != null)
            {
                //second case esisting value is null
                if (existingCase.BorrowerCaseID == null)
                {
                    existingCase.BorrowerCaseID = updateCase.BorrowerCaseID.Trim();
                    existingCase.BorrowerCaseID = existingCase.BorrowerCaseID + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.BorrowerCaseID != null)
                {
                    if (!existingCase.BorrowerCaseID.Trim().Equals(updateCase.BorrowerCaseID.Trim()))
                    {

                        existingCase.BorrowerCaseID = updateCase.BorrowerCaseID.Trim();
                        existingCase.BorrowerCaseID = existingCase.BorrowerCaseID + "*";
                        unchanged = false;
                    }
                }
            }

            //Plaintiff
            //first case updated value is null
            if (updateCase.Plaintiff != null)
            {
                //second case esisting value is null
                if (existingCase.Plaintiff == null)
                {
                    existingCase.Plaintiff = updateCase.Plaintiff.Trim();
                    existingCase.Plaintiff = existingCase.Plaintiff + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.Plaintiff != null)
                {
                    if (!existingCase.Plaintiff.Trim().Equals(updateCase.Plaintiff.Trim()))
                    {

                        existingCase.Plaintiff = updateCase.Plaintiff.Trim();
                        existingCase.Plaintiff = existingCase.Plaintiff + "*";
                        unchanged = false;
                    }
                }
            }

            //PrimaryDefendant
            //first case updated value is null
            if (updateCase.PrimaryDefendant != null)
            {
                //second case esisting value is null
                if (existingCase.PrimaryDefendant == null)
                {
                    existingCase.PrimaryDefendant = updateCase.PrimaryDefendant.Trim();
                    existingCase.PrimaryDefendant = existingCase.PrimaryDefendant + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.PrimaryDefendant != null)
                {
                    if (!existingCase.PrimaryDefendant.Trim().Equals(updateCase.PrimaryDefendant.Trim()))
                    {

                        existingCase.PrimaryDefendant = updateCase.PrimaryDefendant.Trim();
                        existingCase.PrimaryDefendant = existingCase.PrimaryDefendant + "*";
                        unchanged = false;
                    }
                }
            }


            //CaseType
            //first case updated value is null
            if (updateCase.CaseType != null)
            {
                //second case esisting value is null
                if (existingCase.CaseType == null)
                {
                    existingCase.CaseType = updateCase.CaseType.Trim();
                    existingCase.CaseType = existingCase.CaseType + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CaseType != null)
                {
                    if (!existingCase.CaseType.Trim().Equals(updateCase.CaseType.Trim()))
                    {

                        if (!(existingCase.CaseType.Trim().Equals("") && updateCase.CaseType.Trim().Equals("NA")))
                        {
                            if (!(existingCase.CaseType.Trim().Equals("NA") && updateCase.CaseType.Trim().Equals("")))
                            {


                                existingCase.CaseType = updateCase.CaseType.Trim();
                                existingCase.CaseType = existingCase.CaseType + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //PrimaryInjury
            //first case updated value is null
            if (updateCase.PrimaryInjury != null)
            {
                //second case esisting value is null
                if (existingCase.PrimaryInjury == null)
                {
                    existingCase.PrimaryInjury = updateCase.PrimaryInjury.Trim();
                    existingCase.PrimaryInjury = existingCase.PrimaryInjury + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.PrimaryInjury != null)
                {
                    if (!existingCase.PrimaryInjury.Trim().Equals(updateCase.PrimaryInjury.Trim()))
                    {

                        existingCase.PrimaryInjury = updateCase.PrimaryInjury.Trim();
                        existingCase.PrimaryInjury = existingCase.PrimaryInjury + "*";
                        unchanged = false;
                    }
                }
            }

            //DateOfInjury
            //first case updated value is null
            if (updateCase.DateOfInjury != null)
            {
                //second case esisting value is null
                if (existingCase.DateOfInjury == null)
                {
                    existingCase.DateOfInjury = updateCase.DateOfInjury.Trim();
                    existingCase.DateOfInjury = existingCase.DateOfInjury + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.DateOfInjury != null)
                {
                    if (!existingCase.DateOfInjury.Trim().Equals(updateCase.DateOfInjury.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingCase.DateOfInjury, out existingDate)) & (DateTime.TryParse(updateCase.DateOfInjury, out updateDate)))
                        {
                            //if value < zero,arg 1 si earlier than arg 2
                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingCase.DateOfInjury = updateCase.DateOfInjury.Trim();
                                existingCase.DateOfInjury = existingCase.DateOfInjury + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //DateRetainerSigned
            //first case updated value is null
            if (updateCase.DateRetainerSigned != null)
            {
                //second case esisting value is null
                if (existingCase.DateRetainerSigned == null)
                {
                    existingCase.DateRetainerSigned = updateCase.DateRetainerSigned.Trim();
                    existingCase.DateRetainerSigned = existingCase.DateRetainerSigned + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.DateRetainerSigned != null)
                {
                    if (!existingCase.DateRetainerSigned.Trim().Equals(updateCase.DateRetainerSigned.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingCase.DateRetainerSigned, out existingDate)) & (DateTime.TryParse(updateCase.DateRetainerSigned, out updateDate)))
                        {
                            //if value < zero,arg 1 si earlier than arg 2
                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingCase.DateRetainerSigned = updateCase.DateRetainerSigned.Trim();
                                existingCase.DateRetainerSigned = existingCase.DateRetainerSigned + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //DateFiled
            //first case updated value is null
            if (updateCase.DateFiled != null)
            {
                //second case esisting value is null
                if (existingCase.DateFiled == null)
                {
                    existingCase.DateFiled = updateCase.DateFiled.Trim();
                    existingCase.DateFiled = existingCase.DateFiled + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.DateFiled != null)
                {
                    if (!existingCase.DateFiled.Trim().Equals(updateCase.DateFiled.Trim()))
                    {
                        DateTime existingDate;
                        DateTime updateDate;

                        if ((DateTime.TryParse(existingCase.DateFiled, out existingDate)) & (DateTime.TryParse(updateCase.DateFiled, out updateDate)))
                        {

                            int value = DateTime.Compare(existingDate.Date, updateDate.Date);
                            if (value != 0)
                            {

                                existingCase.DateFiled = updateCase.DateFiled.Trim();
                                existingCase.DateFiled = existingCase.DateFiled + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //StateCircuit
            //first case updated value is null
            if (updateCase.StateCircuit != null)
            {
                //second case esisting value is null
                if (existingCase.StateCircuit == null)
                {
                    existingCase.StateCircuit = updateCase.StateCircuit.Trim();
                    existingCase.StateCircuit = existingCase.StateCircuit + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.StateCircuit != null)
                {
                    if (!existingCase.StateCircuit.Trim().Equals(updateCase.StateCircuit.Trim()))
                    {
                        if (!(existingCase.StateCircuit.Trim().Equals("") && updateCase.StateCircuit.Trim().Equals("NA")))
                        {
                            if (!(existingCase.StateCircuit.Trim().Equals("NA") && updateCase.StateCircuit.Trim().Equals("")))
                            {
                                existingCase.StateCircuit = updateCase.StateCircuit.Trim();
                                existingCase.StateCircuit = existingCase.StateCircuit + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //CourtVenue
            //first case updated value is null
            if (updateCase.CourtVenue != null)
            {
                //second case esisting value is null
                if (existingCase.CourtVenue == null)
                {
                    existingCase.CourtVenue = updateCase.CourtVenue.Trim();
                    existingCase.CourtVenue = existingCase.CourtVenue + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CourtVenue != null)
                {
                    if (!existingCase.CourtVenue.Trim().Equals(updateCase.CourtVenue.Trim()))
                    {
                        if (!(existingCase.CourtVenue.Trim().Equals("") && updateCase.CourtVenue.Trim().Equals("NA")))
                        {
                            if (!(existingCase.CourtVenue.Trim().Equals("NA") && updateCase.CourtVenue.Trim().Equals("")))
                            {
                                existingCase.CourtVenue = updateCase.CourtVenue.Trim();
                                existingCase.CourtVenue = existingCase.CourtVenue + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //Jurisdiction
            //first case updated value is null
            if (updateCase.Jurisdiction != null)
            {
                //second case esisting value is null
                if (existingCase.Jurisdiction == null)
                {
                    existingCase.Jurisdiction = updateCase.Jurisdiction.Trim();
                    existingCase.Jurisdiction = existingCase.Jurisdiction + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.Jurisdiction != null)
                {
                    if (!existingCase.Jurisdiction.Trim().Equals(updateCase.Jurisdiction.Trim()))
                    {
                        if (!(existingCase.Jurisdiction.Trim().Equals("") && updateCase.Jurisdiction.Trim().Equals("NA")))
                        {
                            if (!(existingCase.Jurisdiction.Trim().Equals("NA") && updateCase.Jurisdiction.Trim().Equals("")))
                            {
                                existingCase.Jurisdiction = updateCase.Jurisdiction.Trim();
                                existingCase.Jurisdiction = existingCase.Jurisdiction + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //CaseNumber
            //first case updated value is null
            if (updateCase.DocketNumber != null)
            {
                //second case esisting value is null
                if (existingCase.DocketNumber == null)
                {
                    existingCase.DocketNumber = updateCase.DocketNumber.Trim();
                    existingCase.DocketNumber = existingCase.DocketNumber + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.DocketNumber != null)
                {
                    if (!existingCase.DocketNumber.Trim().Equals(updateCase.DocketNumber.Trim()))
                    {

                        existingCase.DocketNumber = updateCase.DocketNumber.Trim();
                        existingCase.DocketNumber = existingCase.DocketNumber + "*";
                        unchanged = false;
                    }
                }
            }

            //Judge
            //first case updated value is null
            if (updateCase.Judge != null)
            {
                //second case esisting value is null
                if (existingCase.Judge == null)
                {
                    existingCase.Judge = updateCase.Judge.Trim();
                    existingCase.Judge = existingCase.Judge + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.Judge != null)
                {
                    if (!existingCase.Judge.Trim().Equals(updateCase.Judge.Trim()))
                    {

                        existingCase.Judge = updateCase.Judge.Trim();
                        existingCase.Judge = existingCase.Judge + "*";
                        unchanged = false;
                    }
                }
            }

            //CaseStage
            //first case updated value is null
            if (updateCase.CaseStage != null)
            {
                //second case esisting value is null
                if (existingCase.CaseStage == null)
                {
                    existingCase.CaseStage = updateCase.CaseStage.Trim();
                    existingCase.CaseStage = existingCase.CaseStage + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CaseStage != null)
                {
                    if (!existingCase.CaseStage.Trim().Equals(updateCase.CaseStage.Trim()))
                    {
                        if (!(existingCase.CaseStage.Trim().Equals("") && updateCase.CaseStage.Trim().Equals("NA")))
                        {
                            if (!(existingCase.CaseStage.Trim().Equals("NA") && updateCase.CaseStage.Trim().Equals("")))
                            {
                                existingCase.CaseStage = updateCase.CaseStage.Trim();
                                existingCase.CaseStage = existingCase.CaseStage + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //InsuranceCarrier
            //first case updated value is null
            if (updateCase.InsuranceCarrier != null)
            {
                //second case esisting value is null
                if (existingCase.InsuranceCarrier == null)
                {
                    existingCase.InsuranceCarrier = updateCase.InsuranceCarrier.Trim();
                    existingCase.InsuranceCarrier = existingCase.InsuranceCarrier + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.InsuranceCarrier != null)
                {
                    if (!existingCase.InsuranceCarrier.Trim().Equals(updateCase.InsuranceCarrier.Trim()))
                    {
                        if (!(existingCase.InsuranceCarrier.Trim().Equals("") && updateCase.InsuranceCarrier.Trim().Equals("NA")))
                        {
                            if (!(existingCase.InsuranceCarrier.Trim().Equals("NA") && updateCase.InsuranceCarrier.Trim().Equals("")))
                            {
                               
                                existingCase.InsuranceCarrier = updateCase.InsuranceCarrier.Trim();
                                existingCase.InsuranceCarrier = existingCase.InsuranceCarrier + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //InsuranceCoverageDollars
            //first case updated value is null
            if (updateCase.InsuranceCoverageDollars != null)
            {
                //second case esisting value is null
                if (existingCase.InsuranceCoverageDollars == null)
                {
                    existingCase.InsuranceCoverageDollars = updateCase.InsuranceCoverageDollars.Trim();
                    existingCase.InsuranceCoverageDollars = existingCase.InsuranceCoverageDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.InsuranceCoverageDollars != null)
                {
                    if (!existingCase.InsuranceCoverageDollars.Trim().Equals(updateCase.InsuranceCoverageDollars.Trim()))
                    {
                        if (!(existingCase.InsuranceCoverageDollars.Trim().Equals("") && updateCase.InsuranceCoverageDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.InsuranceCoverageDollars.Trim().Equals("0") && updateCase.InsuranceCoverageDollars.Trim().Equals("")))
                            {
                                existingCase.InsuranceCoverageDollars = updateCase.InsuranceCoverageDollars.Trim();
                                existingCase.InsuranceCoverageDollars = existingCase.InsuranceCoverageDollars + "*";
                                unchanged = false;
                            }

                        }

                    }
                }
            }

            //ExcessCarrier
            //first case updated value is null
            if (updateCase.ExcessCarrier != null)
            {
                //second case esisting value is null
                if (existingCase.ExcessCarrier == null)
                {
                    existingCase.ExcessCarrier = updateCase.ExcessCarrier.Trim();
                    existingCase.ExcessCarrier = existingCase.ExcessCarrier + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.ExcessCarrier != null)
                {
                    if (!existingCase.ExcessCarrier.Trim().Equals(updateCase.ExcessCarrier.Trim()))
                    {
                        if (!(existingCase.ExcessCarrier.Trim().Equals("") && updateCase.ExcessCarrier.Trim().Equals("NA")))
                        {
                            if (!(existingCase.ExcessCarrier.Trim().Equals("NA") && updateCase.ExcessCarrier.Trim().Equals("")))
                            {

                                existingCase.ExcessCarrier = updateCase.ExcessCarrier.Trim();
                                existingCase.ExcessCarrier = existingCase.ExcessCarrier + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //ExcessCoverageDollars
            //first case updated value is null
            if (updateCase.ExcessCoverageDollars != null)
            {
                //second case esisting value is null
                if (existingCase.ExcessCoverageDollars == null)
                {
                    existingCase.ExcessCoverageDollars = updateCase.ExcessCoverageDollars.Trim();
                    existingCase.ExcessCoverageDollars = existingCase.ExcessCoverageDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.ExcessCoverageDollars != null)
                {
                    if (!existingCase.ExcessCoverageDollars.Trim().Equals(updateCase.ExcessCoverageDollars.Trim()))
                    {
                        if (!(existingCase.ExcessCoverageDollars.Trim().Equals("") && updateCase.ExcessCoverageDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.ExcessCoverageDollars.Trim().Equals("0") && updateCase.ExcessCoverageDollars.Trim().Equals("")))
                            {

                                existingCase.ExcessCoverageDollars = updateCase.ExcessCoverageDollars.Trim();
                                existingCase.ExcessCoverageDollars = existingCase.ExcessCoverageDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //GrossSettlementDollars
            //first case updated value is null
            if (updateCase.GrossSettlementDollars != null)
            {
                //second case esisting value is null
                if (existingCase.GrossSettlementDollars == null)
                {
                    existingCase.GrossSettlementDollars = updateCase.GrossSettlementDollars.Trim();
                    existingCase.GrossSettlementDollars = existingCase.GrossSettlementDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.GrossSettlementDollars != null)
                {
                    if (!existingCase.GrossSettlementDollars.Trim().Equals(updateCase.GrossSettlementDollars.Trim()))
                    {
                        if (!(existingCase.GrossSettlementDollars.Trim().Equals("") && updateCase.GrossSettlementDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.GrossSettlementDollars.Trim().Equals("0") && updateCase.GrossSettlementDollars.Trim().Equals("")))
                            {
                                existingCase.GrossSettlementDollars = updateCase.GrossSettlementDollars.Trim();
                                existingCase.GrossSettlementDollars = existingCase.GrossSettlementDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //AttorneysFeePercent
            //first case updated value is null
            if (updateCase.AttorneysFeePercent != null)
            {
                //second case esisting value is null
                if (existingCase.AttorneysFeePercent == null)
                {
                    existingCase.AttorneysFeePercent = updateCase.AttorneysFeePercent.Trim();
                    existingCase.AttorneysFeePercent = existingCase.AttorneysFeePercent + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.AttorneysFeePercent != null)
                {
                    if (!existingCase.AttorneysFeePercent.Trim().Equals(updateCase.AttorneysFeePercent.Trim()))
                    {
                        if (!(existingCase.AttorneysFeePercent.Trim().Equals("") && updateCase.AttorneysFeePercent.Trim().Equals("0")))
                        {
                            if (!(existingCase.AttorneysFeePercent.Trim().Equals("0") && updateCase.AttorneysFeePercent.Trim().Equals("")))
                            {
                                existingCase.AttorneysFeePercent = updateCase.AttorneysFeePercent.Trim();
                                existingCase.AttorneysFeePercent = existingCase.AttorneysFeePercent + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //AttorneysFeeDollars
            //first case updated value is null
            if (updateCase.AttorneysFeeDollars != null)
            {
                //second case esisting value is null
                if (existingCase.AttorneysFeeDollars == null)
                {
                    existingCase.AttorneysFeeDollars = updateCase.AttorneysFeeDollars.Trim(); existingCase.AttorneysFeeDollars = existingCase.AttorneysFeeDollars + "*";

                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.AttorneysFeeDollars != null)
                {
                    if (!existingCase.AttorneysFeeDollars.Trim().Equals(updateCase.AttorneysFeeDollars.Trim()))
                    {
                        if (!(existingCase.AttorneysFeeDollars.Trim().Equals("") && updateCase.AttorneysFeeDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.AttorneysFeeDollars.Trim().Equals("0") && updateCase.AttorneysFeeDollars.Trim().Equals("")))
                            {

                                existingCase.AttorneysFeeDollars = updateCase.AttorneysFeeDollars.Trim();
                                existingCase.AttorneysFeeDollars = existingCase.AttorneysFeeDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //GrossCoAndReferringCounselFeePercentage
            //first case updated value is null
            if (updateCase.GrossCoAndReferringCounselFeePercentage != null)
            {
                //second case esisting value is null
                if (existingCase.GrossCoAndReferringCounselFeePercentage == null)
                {
                    existingCase.GrossCoAndReferringCounselFeePercentage = updateCase.GrossCoAndReferringCounselFeePercentage.Trim();
                    existingCase.GrossCoAndReferringCounselFeePercentage = existingCase.GrossCoAndReferringCounselFeePercentage + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.GrossCoAndReferringCounselFeePercentage != null)
                {
                    if (!existingCase.GrossCoAndReferringCounselFeePercentage.Trim().Equals(updateCase.GrossCoAndReferringCounselFeePercentage.Trim()))
                    {

                        if (!(existingCase.GrossCoAndReferringCounselFeePercentage.Trim().Equals("") && updateCase.GrossCoAndReferringCounselFeePercentage.Trim().Equals("0")))
                        {
                            if (!(existingCase.GrossCoAndReferringCounselFeePercentage.Trim().Equals("0") && updateCase.GrossCoAndReferringCounselFeePercentage.Trim().Equals("")))
                            {


                                existingCase.GrossCoAndReferringCounselFeePercentage = updateCase.GrossCoAndReferringCounselFeePercentage.Trim();
                                existingCase.GrossCoAndReferringCounselFeePercentage = existingCase.GrossCoAndReferringCounselFeePercentage + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //GrossCoAndReferringCounselFeeDollars
            //first case updated value is null
            if (updateCase.GrossCoAndReferringCounselFeeDollars != null)
            {
                //second case esisting value is null
                if (existingCase.GrossCoAndReferringCounselFeeDollars == null)
                {
                    existingCase.GrossCoAndReferringCounselFeeDollars = updateCase.GrossCoAndReferringCounselFeeDollars.Trim();
                    existingCase.GrossCoAndReferringCounselFeeDollars = existingCase.GrossCoAndReferringCounselFeeDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.GrossCoAndReferringCounselFeeDollars != null)
                {
                    if (!existingCase.GrossCoAndReferringCounselFeeDollars.Trim().Equals(updateCase.GrossCoAndReferringCounselFeeDollars.Trim()))
                    {

                        if (!(existingCase.GrossCoAndReferringCounselFeeDollars.Trim().Equals("") && updateCase.GrossCoAndReferringCounselFeeDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.GrossCoAndReferringCounselFeeDollars.Trim().Equals("0") && updateCase.GrossCoAndReferringCounselFeeDollars.Trim().Equals("")))
                            {
                                existingCase.GrossCoAndReferringCounselFeeDollars = updateCase.GrossCoAndReferringCounselFeeDollars.Trim();
                                existingCase.GrossCoAndReferringCounselFeeDollars = existingCase.GrossCoAndReferringCounselFeeDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //FirmsNetFeesPercent
            //first case updated value is null
            if (updateCase.FirmsNetFeesPercent != null)
            {
                //second case esisting value is null
                if (existingCase.FirmsNetFeesPercent == null)
                {
                    existingCase.FirmsNetFeesPercent = updateCase.FirmsNetFeesPercent.Trim();
                    existingCase.FirmsNetFeesPercent = existingCase.FirmsNetFeesPercent + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.FirmsNetFeesPercent != null)
                {
                    if (!existingCase.FirmsNetFeesPercent.Trim().Equals(updateCase.FirmsNetFeesPercent.Trim()))
                    {
                        if (!(existingCase.FirmsNetFeesPercent.Trim().Equals("") && updateCase.FirmsNetFeesPercent.Trim().Equals("0")))
                        {
                            if (!(existingCase.FirmsNetFeesPercent.Trim().Equals("0") && updateCase.FirmsNetFeesPercent.Trim().Equals("")))
                            {

                                existingCase.FirmsNetFeesPercent = updateCase.FirmsNetFeesPercent.Trim();
                                existingCase.FirmsNetFeesPercent = existingCase.FirmsNetFeesPercent + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //FirmsNetFeesDollars
            //first case updated value is null
            if (updateCase.FirmsNetFeesDollars != null)
            {
                //second case esisting value is null
                if (existingCase.FirmsNetFeesDollars == null)
                {
                    existingCase.FirmsNetFeesDollars = updateCase.FirmsNetFeesDollars.Trim();
                    existingCase.FirmsNetFeesDollars = existingCase.FirmsNetFeesDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.FirmsNetFeesDollars != null)
                {
                    if (!existingCase.FirmsNetFeesDollars.Trim().Equals(updateCase.FirmsNetFeesDollars.Trim()))
                    {
                        if (!(existingCase.FirmsNetFeesDollars.Trim().Equals("") && updateCase.FirmsNetFeesDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.FirmsNetFeesDollars.Trim().Equals("0") && updateCase.FirmsNetFeesDollars.Trim().Equals("")))
                            {

                                existingCase.FirmsNetFeesDollars = updateCase.FirmsNetFeesDollars.Trim();
                                existingCase.FirmsNetFeesDollars = existingCase.FirmsNetFeesDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }



            //case cost
            //first case updated value is null
            if (updateCase.CaseCost != null)
            {
                //second case esisting value is null
                if (existingCase.CaseCost == null)
                {
                    existingCase.CaseCost = updateCase.CaseCost.Trim();
                    existingCase.CaseCost = existingCase.CaseCost + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CaseCost != null)
                {
                    if (!existingCase.CaseCost.Trim().Equals(updateCase.CaseCost.Trim()))
                    {
                        if (!(existingCase.CaseCost.Trim().Equals("") && updateCase.CaseCost.Trim().Equals("0")))
                        {
                            if (!(existingCase.CaseCost.Trim().Equals("0") && updateCase.CaseCost.Trim().Equals("")))
                            {
                                existingCase.CaseCost = updateCase.CaseCost.Trim();
                                existingCase.CaseCost = existingCase.CaseCost + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }



            //QtrSettlement
            //first case updated value is null
            if (updateCase.QtrSettlement != null)
            {
                //second case esisting value is null
                if (existingCase.QtrSettlement == null)
                {
                    existingCase.QtrSettlement = updateCase.QtrSettlement.Trim();
                    existingCase.QtrSettlement = existingCase.QtrSettlement + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.QtrSettlement != null)
                {
                    if (!existingCase.QtrSettlement.Trim().Equals(updateCase.QtrSettlement.Trim()))
                    {
                        if (!(existingCase.QtrSettlement.Trim().Equals("") && updateCase.QtrSettlement.Trim().Equals("0")))
                        {
                            if (!(existingCase.QtrSettlement.Trim().Equals("0") && updateCase.QtrSettlement.Trim().Equals("")))
                            {
                                existingCase.QtrSettlement = updateCase.QtrSettlement.Trim();
                                existingCase.QtrSettlement = existingCase.QtrSettlement + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //Year Settlement
            //first case updated value is null
            if (updateCase.YearSettlement != null)
            {
                //second case esisting value is null
                if (existingCase.YearSettlement == null)
                {
                    existingCase.YearSettlement = updateCase.YearSettlement.Trim();
                    existingCase.YearSettlement = existingCase.YearSettlement + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.YearSettlement != null)
                {
                    if (!existingCase.YearSettlement.Trim().Equals(updateCase.YearSettlement.Trim()))
                    {
                        if (!(existingCase.YearSettlement.Trim().Equals("") && updateCase.YearSettlement.Trim().Equals("0")))
                        {
                            if (!(existingCase.YearSettlement.Trim().Equals("0") && updateCase.YearSettlement.Trim().Equals("")))
                            {
                                existingCase.YearSettlement = updateCase.YearSettlement.Trim();
                                existingCase.YearSettlement = existingCase.YearSettlement + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //CF Gross Settlement ($)
            //first case updated value is null
            if (updateCase.CfGrossSettlementDollars != null)
            {
                //second case esisting value is null
                if (existingCase.CfGrossSettlementDollars == null)
                {
                    existingCase.CfGrossSettlementDollars = updateCase.CfGrossSettlementDollars.Trim();
                    existingCase.CfGrossSettlementDollars = existingCase.CfGrossSettlementDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CfGrossSettlementDollars != null)
                {
                    if (!existingCase.CfGrossSettlementDollars.Trim().Equals(updateCase.CfGrossSettlementDollars.Trim()))
                    {
                        if (!(existingCase.CfGrossSettlementDollars.Trim().Equals("") && updateCase.CfGrossSettlementDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.CfGrossSettlementDollars.Trim().Equals("0") && updateCase.CfGrossSettlementDollars.Trim().Equals("")))
                            {
                                existingCase.CfGrossSettlementDollars = updateCase.CfGrossSettlementDollars.Trim();
                                existingCase.CfGrossSettlementDollars = existingCase.CfGrossSettlementDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //CF Attorney's Fee ($)
            //first case updated value is null
            if (updateCase.CfAttorneyFeeDollar != null)
            {
                //second case esisting value is null
                if (existingCase.CfAttorneyFeeDollar == null)
                {
                    existingCase.CfAttorneyFeeDollar = updateCase.CfAttorneyFeeDollar.Trim();
                    existingCase.CfAttorneyFeeDollar = existingCase.CfAttorneyFeeDollar + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CfAttorneyFeeDollar != null)
                {
                    if (!existingCase.CfAttorneyFeeDollar.Trim().Equals(updateCase.CfAttorneyFeeDollar.Trim()))
                    {
                        if (!(existingCase.CfAttorneyFeeDollar.Trim().Equals("") && updateCase.CfAttorneyFeeDollar.Trim().Equals("0")))
                        {
                            if (!(existingCase.CfAttorneyFeeDollar.Trim().Equals("0") && updateCase.CfAttorneyFeeDollar.Trim().Equals("")))
                            {
                                existingCase.CfAttorneyFeeDollar = updateCase.CfAttorneyFeeDollar.Trim();
                                existingCase.CfAttorneyFeeDollar = existingCase.CfAttorneyFeeDollar + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }

            //CF Firm's Net Fees ($)
            //first case updated value is null
            if (updateCase.CfFirmNetFeesDollars != null)
            {
                //second case esisting value is null
                if (existingCase.CfFirmNetFeesDollars == null)
                {
                    existingCase.CfFirmNetFeesDollars = updateCase.CfFirmNetFeesDollars.Trim();
                    existingCase.CfFirmNetFeesDollars = existingCase.CfFirmNetFeesDollars + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CfFirmNetFeesDollars != null)
                {
                    if (!existingCase.CfFirmNetFeesDollars.Trim().Equals(updateCase.CfFirmNetFeesDollars.Trim()))
                    {
                        if (!(existingCase.CfFirmNetFeesDollars.Trim().Equals("") && updateCase.CfFirmNetFeesDollars.Trim().Equals("0")))
                        {
                            if (!(existingCase.CfFirmNetFeesDollars.Trim().Equals("0") && updateCase.CfFirmNetFeesDollars.Trim().Equals("")))
                            {
                                existingCase.CfFirmNetFeesDollars = updateCase.CfFirmNetFeesDollars.Trim();
                                existingCase.CfFirmNetFeesDollars = existingCase.CfFirmNetFeesDollars + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //CF Qtr of Settlement
            //first case updated value is null
            if (updateCase.CfQuaterSettlement != null)
            {
                //second case esisting value is null
                if (existingCase.CfQuaterSettlement == null)
                {
                    existingCase.CfQuaterSettlement = updateCase.CfQuaterSettlement.Trim();
                    existingCase.CfQuaterSettlement = existingCase.CfQuaterSettlement + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CfQuaterSettlement != null)
                {
                    if (!existingCase.CfQuaterSettlement.Trim().Equals(updateCase.CfQuaterSettlement.Trim()))
                    {
                        if (!(existingCase.CfQuaterSettlement.Trim().Equals("") && updateCase.CfQuaterSettlement.Trim().Equals("0")))
                        {
                            if (!(existingCase.CfQuaterSettlement.Trim().Equals("0") && updateCase.CfQuaterSettlement.Trim().Equals("")))
                            {
                                existingCase.CfQuaterSettlement = updateCase.CfQuaterSettlement.Trim();
                                existingCase.CfQuaterSettlement = existingCase.CfQuaterSettlement + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //CF Year of Settlement
            //first case updated value is null
            if (updateCase.CfYearSettlement != null)
            {
                //second case esisting value is null
                if (existingCase.CfYearSettlement == null)
                {
                    existingCase.CfYearSettlement = updateCase.CfYearSettlement.Trim();
                    existingCase.CfYearSettlement = existingCase.CfYearSettlement + "*";
                    unchanged = false;
                    //Last case both have values so figure out is there really is an udate
                }
                else if (existingCase.CfYearSettlement != null)
                {
                    if (!existingCase.CfYearSettlement.Trim().Equals(updateCase.CfYearSettlement.Trim()))
                    {
                        if (!(existingCase.CfYearSettlement.Trim().Equals("") && updateCase.CfYearSettlement.Trim().Equals("0")))
                        {
                            if (!(existingCase.CfYearSettlement.Trim().Equals("0") && updateCase.CfYearSettlement.Trim().Equals("")))
                            {
                                existingCase.CfYearSettlement = updateCase.CfYearSettlement.Trim();
                                existingCase.CfYearSettlement = existingCase.CfYearSettlement + "*";
                                unchanged = false;
                            }
                        }
                    }
                }
            }


            //add new columns here
            if (unchanged == true)
            {
                return null;
            }
            return existingCase;
        }
        public List<Collaterial> getExistingCasesByLoanIDSelectedFromUploadCases()
        {
            return this.collaterialListByLoanId;

        }
        public List<Collaterial> getUploadedCasesThatWillBeNewCases()
        {
            return this.uploadedCaseNewCaseList;

        }
        public List<Collaterial> getUploadedCasesThatWillBeUpatedCases()
        {
            return this.uploadedCaseUpdateCaseList;

        }
        public List<Collaterial> getUploadedCasesThatWillBeRemovedCases()
        {
            return this.deletedCaseList;

        }

        public void uploadCaseFile()
        {

        }

        public bool addNewCase()
        {
            log.Info("Add new case");
            bool addNewCaseResult = true;
            string messageBox;
            Collaterial c = this.getCurrentCase();
            if (c == null)
            {
                log.Error("Trying to add a new case but the case is null");
                return false;
            }

            sQLCollaterialQueries.addNewCase(c);

            return true;
        }


        public Collaterial searchCaseCaseID(string caseID)
        {

            List<Collaterial> list = sQLCollaterialQueries.searchCaseCaseID(caseID);
            if(list != null && list.Count >= 1)
            {
                return list[0];
            }
            return null;
        }


        public List<Collaterial> saveUploadedAddCasesResolvedOnly(List<Collaterial> caseList)
        {
            List<Collaterial> failedList = new List<Collaterial>();
           
            log.Info("save uploaded cases");
            if (caseList == null)
            {
                return null;
            }


            for (int i = 0; i < caseList.Count; i++)
            {
               
                Collaterial c = caseList[i];
                if (c == null)
                {
                    log.Error("Trying to add a new case but the case is null");
                    return null;
                }
                c.ReasonForUpdateSelection = "Existing Resolved Cases";
                bool res = sQLCollaterialQueries.addNewCase(c);
                if (!res)
                {
                    failedList.Add(c);

                }
                
            }
            return failedList;
        }



        public List<Collaterial> saveUploadedAddCases(List<Collaterial> caseList)
        {
            List<Collaterial> failedList = new List<Collaterial>();
            CaseModification caseModification = new CaseModification();
            caseModification.CaseCreatedDate = DateTime.Now;
            caseModification.CaseModifiedDate = DateTime.Now;
            log.Info("save uploaded cases");
            if (caseList == null)
            {
                return null;
            }


            for (int i = 0; i < caseList.Count; i++)
            {
                caseModification = new CaseModification();
                Collaterial c = caseList[i];
                if (c == null)
                {
                    log.Error("Trying to add a new case but the case is null");
                    return null;
                }
                c.ReasonForUpdateSelection = "New Case";
                c.CaseModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                bool res = sQLCollaterialQueries.addNewCase(c);
                if (!res)
                {
                    failedList.Add(c);

                }
                else
                {


                    string id = selectsMostRecentInsertIdentity();
                    if (id == null | id.Trim().Equals(""))
                    {
                        return null;
                    }
                    c.CaseID = id;
                    caseModificationController.addNewCaseModification(c, c, caseModification);
                }
            }
            return failedList;
        }

        public List<Collaterial> saveUploadedDroppedCases(List<Collaterial> caseList)
        {
            List<Collaterial> failedList = new List<Collaterial>();
            CaseModification caseModification = new CaseModification();
            caseModification.CaseCreatedDate = DateTime.Now;
            caseModification.CaseModifiedDate = DateTime.Now;
            log.Info("save dropped cases");
            if (caseList == null)
            {
                return null;
            }


            for (int i = 0; i < caseList.Count; i++)
            {
                Collaterial c = caseList[i];
                if (c == null)
                {
                    log.Error("Trying to add a new case but the case is null");
                    return null;
                }
                if (!c.CaseStatus.Equals("Resolved") || c.ResolvedDate.ToString().StartsWith("1900-01-01"))
                {
                    c.CaseStatus = "Resolved";
                    if (!c.ResolvedDate.ToString().StartsWith("1900-01-01"))
                    {
                        c.ResolvedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    }
                    c.EffectiveDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    c.CaseModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                    List<Collaterial> cc = sQLCollaterialQueries.searchCaseCaseID(c.CaseID);
                    bool res = sQLCollaterialQueries.udateCase(c);
                    if (!res)
                    {
                        failedList.Add(c);

                    }
                    else
                    {

                        if (cc == null)
                        {
                            return null;
                        }
                        if (cc.Count() >= 1)
                        {
                            bool res2 = caseModificationController.updatedTerminationDate(c.CaseID, c.EffectiveDate);
                            caseModificationController.addNewCaseModification(cc[0], c, caseModification);
                        }
                    }

                }
            }
            return failedList;
        }

        public List<Collaterial> saveUploadedUpdatedCases(List<Collaterial> caseList)
        {
            List<Collaterial> failedList = new List<Collaterial>();
            CaseModification caseModification = new CaseModification();
            caseModification.CaseCreatedDate = DateTime.Now;
            caseModification.CaseModifiedDate = DateTime.Now;
            log.Info("save updated cases");
            if (caseList == null)
            {
                return null;
            }


            for (int i = 0; i < caseList.Count; i++)
            {
                Collaterial c = caseList[i];
                
                if (c == null)
                {
                    log.Error("Trying to add a updated case but the case is null");
                    return null;
                }


                List<Collaterial> cc = sQLCollaterialQueries.searchCaseCaseID(c.CaseID);
                if (cc == null)
                {
                    return null;
                }
                if (c.CaseStatus.Equals("Resolved"))
                {
                    c.CaseStatus = "Resolved";
                    c.ResolvedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                }
                c.CaseModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                c.EffectiveDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                bool res = sQLCollaterialQueries.udateCase(c);
                if (!res)
                {
                    failedList.Add(c);
                }
                else
                {


                    if (cc.Count() >= 1)
                    {
                        bool res2 = caseModificationController.updatedTerminationDate(c.CaseID, c.EffectiveDate);
                        caseModificationController.addNewCaseModification(cc[0], c, caseModification);
                    }
                }
            }
            return failedList;
        }



        public bool updateCase()
        {
            log.Info("Update collaterial");
            Collaterial updateCollaterial = this.getUpdatedCase();
            if (updateCollaterial == null)
            {
                log.Error("updating case but its null");
                return false;
            }
            return sQLCollaterialQueries.udateCase(updateCollaterial);

        }

        public List<Collaterial> searchCaseLoanID(String loanID)
        {
            log.Info("search case by loan id");
            if (loanID == null)
            {
                log.Error("searching case by loan id and id is null");
                return new List<Collaterial>();
            }
            List<Collaterial> list = sQLCollaterialQueries.searchCaseLoanID(loanID);
            if (list == null)
            {
                log.Error("searching case by loan id and list is null");
                return new List<Collaterial>();
            }
            for (int i = 0; i < list.Count; i++)
            {
                string name = "";
                SQLBorrowerQueries.borrowerNameIdLookupTable.TryGetValue(list[i].BorrowerID, out name);
                list[i].BorrowerName = name;
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            list.Sort(comparison);
            return list;
        }

        //This will be used when reading in Case list ie. Upload Case list.  Look up for existing
        //case using the borrower case if
        public List<Collaterial> searchCasesFromBorrowerCaseID(string borrowerCaseId)
        {
            log.Info("search case by borrower case id");
            if (borrowerCaseId == null)
            {
                return new List<Collaterial>();
            }

            List<Collaterial> list = sQLCollaterialQueries.searchCaseLoanID(borrowerCaseId);
            if (list == null)
            {
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            list.Sort(comparison);
            return list;
        }

        //this will be used when search for a case in the update case ui
        public List<Collaterial> lookupCasesFromPlaintiffDefendentCaseNumber(string loanID, string plaintiff, string defendent, string caseNumber)
        {
            List<Collaterial> list = sQLCollaterialQueries.lookupCasesFromPlaintiffDefendentCaseNumber(loanID, plaintiff, defendent, caseNumber);
            if (list == null)
            {
                log.Error("search case by plaintiff, defendent and case number came back null");
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            list.Sort(comparison);
            return list;
        }

        //this will be used when search for a case in the update case ui
        public List<Collaterial> lookupCasesFromPlaintiffDefendentCaseNumberActiveOnlyCases(string loanID, string plaintiff, string defendent, string caseNumber)
        {
            List<Collaterial> list = sQLCollaterialQueries.lookupCasesFromPlaintiffDefendentCaseNumberActiveCases(loanID, plaintiff, defendent, caseNumber);
            if (list == null)
            {
                log.Error("search case by plaintiff, defendent and case number came back null");
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            list.Sort(comparison);
            return list;
        }

        //this will be used when search for a case in the update case ui
        public List<Collaterial> lookupCasesFromPlaintiffDefendentCaseNumberResolveOnlyCases(string loanID, string plaintiff, string defendent, string caseNumber)
        {
            List<Collaterial> list = sQLCollaterialQueries.lookupCasesFromPlaintiffDefendentCaseNumberResolveOnlyCases(loanID, plaintiff, defendent, caseNumber);
            if (list == null)
            {
                log.Error("search case by plaintiff, defendent and case number came back null");
                return new List<Collaterial>();
            }
            Comparison<Collaterial> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            list.Sort(comparison);
            return list;
        }

        //Checking required text boxes to make sure they have values in them for strings
        public bool checkRequiredCFText(string title, string value)
        {
            log.Info("Checking to make sure requried text values are filled in for adding a loan");
            bool result = true;

            if (value == null || value.Trim().Equals("")) //|| value.Trim().Equals("0")) Not sure if this needs to be added
            {
                System.Windows.MessageBox.Show("Please enter " + title);
                result = false;
            }
            return result;
        }

        public bool checkRequiredText(string title, string value)
        {
            log.Info("Checking to make sure requried text values are filled in for adding a loan");
            bool result = true;

            if (value == null || value.Trim().Equals(""))
            {
                System.Windows.MessageBox.Show("Please enter " + title);
                result = false;
            }
            return result;
        }

        public bool checkRequiredDouble(string title, string value)
        {
            double doubleValue;

            log.Info("Checking to make sure requried double values are filled in for adding a loan");
            bool result = true;


            int count = value.Split('.').Length - 1;
            if (count > 1)
            {
                MessageBox.Show("Please remove the extra decimal points in the number");
                return false;
            }
            string temp = value.Replace('.', ' ');
            if (temp.Trim().Equals(""))
            {
                MessageBox.Show("Please enter a number in the field: " + title);
                return false;
            }
            try
            {

                //   double doubleValue = Convert.ToDouble(value);
                result = double.TryParse(value, out doubleValue);
                //DMV Not sure why i put in math round
                //result = Math.Round(doubleValue, 2);
            }
            catch (FormatException fe)
            {
                log.Error("Format Exception trying to convert string into double.  Field: " + title + " ", fe);
                return false;
            }


            return result;
        }

        public bool checkNonRequiredDouble(string title, string value)
        {
            log.Info("Checking to make sure non requried double values are filled correctly in for adding a loan");
            bool result = true;
            double doubleValue;
            if (value.Trim().Equals(""))
            {
                return true;
            }
            int count = value.Split('.').Length - 1;
            if (count > 1)
            {
                MessageBox.Show("Please remove the extra decimal points in the number");
                return false;
            }
            string temp = value.Replace('.', ' ');
            if (temp.Trim().Equals(""))
            {
                MessageBox.Show("Please enter a number in the field: " + title);
                return false;
            }
            try
            {

                result = double.TryParse(value, out doubleValue);
                // doubleValue = Math.Round(doubleValue, 2);
            }
            catch (FormatException fe)
            {
                log.Error("Format Exception trying to convert string into double.  Field: " + title + " ", fe);
                return false;
            }


            return result;
        }

        public string selectsMostRecentInsertIdentity()
        {
            string id = sQLCollaterialQueries.selectsMostRecentInsertIdentity();
            return id;
        }

        //public List<InsuranceCompany> getInsuranceCompany()
        //{
        //    insuranceCompanyList = InsuranceCompanyController.getInsuranceCompany();

        //    return insuranceCompanyList;
        //}

        //public Dictionary<string, string> getInsuranceCompanyLookUp()
        //{
        //    insuranceCompanyListLookUp = new Dictionary<string, string>();
        //    insuranceCompanyList = getInsuranceCompany();
        //    foreach (InsuranceCompany ins in insuranceCompanyList)
        //    {
        //        insuranceCompanyListLookUp.Add(ins.Id, ins.Description);
        //    }


        //    return insuranceCompanyListLookUp;
        //}

        //public List<string> getInsuranceCompanyDisplay()
        //{

        //    insuranceCompanyListDisplay = new List<string>();
        //    insuranceCompanyList = getInsuranceCompany();
        //    foreach (InsuranceCompany ins in insuranceCompanyList)
        //    {
        //        insuranceCompanyListDisplay.Add(ins.Description);
        //    }


        //    return insuranceCompanyListDisplay;
        //}

        

        public bool checkDateTime(string title, string value)
        {
            log.Info("check date time");
            if (value == null)
            {
                log.Error("value is null");

                return false;
            }

            if (value.Equals(""))
            {

                return true;
            }
            bool res = Utilities.checkDateRange(title, value);
            if (!res)
            {
                return false;
            }

            return true;
        }
    }
}
