using OTS.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OTS.Model;
using CaseManager.Model;

namespace OTS.Controller
{
    public class ComboBoxPopulationController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ComboBoxPopulationController));
        private readonly SQLComboBoxPopulationQueries sQLComboBoxPopulationQueries;
        public static Dictionary<string, string> stateList = new Dictionary<string, string>();
        public static Dictionary<string, string> companyList = new Dictionary<string, string>();
        public static Dictionary<string, string> docuemntTypeList = new Dictionary<string, string>();
        public static Dictionary<string, string> autopayList = new Dictionary<string, string>();
        public static Dictionary<string, string> statusList = new Dictionary<string, string>();
        public static Dictionary<string, string> caseStatusList = new Dictionary<string, string>();
        public static Dictionary<string, string> resolutionTypeList = new Dictionary<string, string>();
        public static Dictionary<string, string> caseStageList = new Dictionary<string, string>();
        public static List<StateCourtVenued> stateCourtList = new List<StateCourtVenued>();
        public static Dictionary<string, string> stateCircuitLookupList = new Dictionary<string, string>();
        public static Dictionary<string, string> courtVenueLookupList = new Dictionary<string, string>();
        public static Dictionary<string, string> caseTypeList = new Dictionary<string, string>();
        public static List<string> caseTypeMassTortList = new List<string>();
        public static Dictionary<string, string> reasonForUpdateCaseList = new Dictionary<string, string>();
        public static List<CaseSubTypesList> caseSubTypeList;

        public static int defaultStateVenueIndex;
        public static int defaultStateIndex;
        public static int defaultCourtVenueIndex;
        public static int defaultCompanyIndex;
        public static int defaultDocuemntTypeIndex;
        public static int defaultAutopayIndex;
        public static int defaultStatusIndex;
        public static int defaultCaseStatusIndex;
        public static int defaultResolutionTypeIndex;
        public static int defaultCaseStageIndex;
        public static int defaultCaseTypeIndex;
        public static int defaultReasonForUpdateIndex;
        public static string defaultNA;
        public static string defaultResolutionTypeUnknown;
        public static string defaultReasonForUpdateOther;
        public static DateTime defaultDate;
        public static string versionNumber;
        
        




        public ComboBoxPopulationController()
        {
            log.Info("Constructor combox population Controller");
            sQLComboBoxPopulationQueries = new SQLComboBoxPopulationQueries();
           

        }
        public void initialize()
        {
            stateList = this.getStates();
            companyList = this.getCompanies();
            docuemntTypeList = this.getDocumentTypes();
            autopayList = this.getAutopay();
            statusList = this.getStatus();
            caseStatusList = this.getCaseStatus();
            resolutionTypeList = this.getResolutionType();
            caseStageList = this.getCaseStage();
            stateCourtList = this.getStateCircuit();
            caseTypeList = this.getCaseType();
            caseTypeMassTortList = this.getCaseTypeMassTort();
            stateCircuitLookupList = this.getStateCircuitLookup();
            courtVenueLookupList = this.getCourtVenueLookup();
            reasonForUpdateCaseList = this.getReasonForUpdateCase();
            defaultNA = "NA";
            defaultResolutionTypeUnknown = "Unknown";
            defaultReasonForUpdateOther = "Other";
            defaultCaseStageIndex = this.getCaseStageDefaultIndex();
            defaultStateVenueIndex = this.getStateVenueDefaultIndex();
            defaultCourtVenueIndex = this.getCourtVenueDefaultIndex();
            defaultCompanyIndex = this.getCompanyDefaultIndex();
            defaultDocuemntTypeIndex = this.getDocuemntTypeDefaultIndex();
            defaultAutopayIndex = this.getAutopayDefaultIndex();
            defaultStatusIndex = this.getStatusDefaultIndex();
            defaultCaseStatusIndex = this.getCaseStatusDefaultIndex();
            defaultResolutionTypeIndex = this.getCaseResolutionTypeDefaultIndex();
            defaultCaseStageIndex = this.getCaseStageDefaultIndex();
            defaultCaseTypeIndex = this.getCaseTypeDefaultIndex();
            defaultReasonForUpdateIndex = this.getReasonForUpdateDefaultIndex();
            defaultStateIndex = this.getStateDefaultIndex();
            defaultDate = new DateTime(1900, 01, 01);
            versionNumber = this.getCMVersionNumber();
            caseSubTypeList = this.createCaseSubTypesList();
        }

        public int getStateDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.stateList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }


        public int getReasonForUpdateDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.reasonForUpdateCaseList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getCaseTypeDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.caseTypeList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getCaseResolutionTypeDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.resolutionTypeList.FirstOrDefault(x => x.Value == "Unknown").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getCaseStatusDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.caseStatusList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getStatusDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.statusList.FirstOrDefault(x => x.Value == "None").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }


        public int getAutopayDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.autopayList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getDocuemntTypeDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.docuemntTypeList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getCompanyDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.companyList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        public int getCourtVenueDefaultIndex()
        {
            string res;
            int intValue = 0;
            foreach (StateCourtVenued doc in ComboBoxPopulationController.stateCourtList)
            {

                if (doc.CourtVenued.Equals("NA"))
                {
                    res = doc.Id.Trim();
                    int.TryParse(res, out intValue);
                    break;
                }
            }
            return intValue;
        }

        public int getStateVenueDefaultIndex()
        {
            string res;
            int intValue = 0;
            foreach (StateCourtVenued doc in ComboBoxPopulationController.stateCourtList)
            {

                if (doc.StateCircuit.Equals("NA"))
                {
                    res = doc.Id.Trim();
                    int.TryParse(res, out intValue);
                    break;
                }
            }
            return intValue;
        }

        public int getCaseStageDefaultIndex()
        {
            string res;
            int intValue;
            res = ComboBoxPopulationController.caseStageList.FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getStates()
        {
            stateList = sQLComboBoxPopulationQueries.createStateList();

            return stateList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getCompanies()
        {

            companyList = sQLComboBoxPopulationQueries.createCompanyList();

            return companyList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getDocumentTypes()
        {
            docuemntTypeList = sQLComboBoxPopulationQueries.createDocumentTypeList();

            return docuemntTypeList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getAutopay()
        {
            autopayList = sQLComboBoxPopulationQueries.createAutopayList();

            return autopayList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getStatus()
        {
            statusList = sQLComboBoxPopulationQueries.createStatusList();

            return statusList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getCaseType()
        {
            caseTypeList = sQLComboBoxPopulationQueries.createCaseTypeList();

            return caseTypeList;
        }

        //Creates static list to populate ui drop down values
        public List<string> getCaseTypeMassTort()
        {

            caseTypeMassTortList = sQLComboBoxPopulationQueries.createCaseTypeMassTortList();

            return caseTypeMassTortList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getResolutionType()
        {
            resolutionTypeList = sQLComboBoxPopulationQueries.createResolutionTypeList();

            return resolutionTypeList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getCaseStage()
        {
            caseStageList = sQLComboBoxPopulationQueries.createCaseStageList();

            return caseStageList;
        }

        //Creates static list to populate ui drop down values
        public List<StateCourtVenued> getStateCircuit()
        {
            stateCourtList = sQLComboBoxPopulationQueries.createStateCourtList();

            return stateCourtList;
        }

        public Dictionary<string, string> getStateCircuitLookup()
        {
            stateCourtList = sQLComboBoxPopulationQueries.createStateCourtList();

            for (int i = 0; i < stateCourtList.Count; i++)
            {
                stateCircuitLookupList.Add(stateCourtList[i].Id, stateCourtList[i].StateCircuit);
            }

            return stateCircuitLookupList;
        }

        public Dictionary<string, string> getCourtVenueLookup()
        {
            stateCourtList = sQLComboBoxPopulationQueries.createStateCourtList();

            for (int i = 0; i < stateCourtList.Count; i++)
            {
                courtVenueLookupList.Add(stateCourtList[i].Id, stateCourtList[i].CourtVenued);
            }

            return courtVenueLookupList;
        }


        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getCaseStatus()
        {
            caseStatusList = sQLComboBoxPopulationQueries.createCaseStatusList();

            return caseStatusList;
        }

        //Creates static list to populate ui drop down values
        public Dictionary<string, string> getReasonForUpdateCase()
        {
            reasonForUpdateCaseList = sQLComboBoxPopulationQueries.createReasonForUpdateCaseList();

            return reasonForUpdateCaseList;
        }

        public string getCMVersionNumber()
        {
            return sQLComboBoxPopulationQueries.getCMVersionNumber();
        }



        public static List<EnhancedPortfolioDiversificationSelection> ClearCaseType()
        {
            List<string> caseType = new List<string>();
            //for (int j = 0; j < enh.Count; j++)
            //{
            //    if (!caseType.Contains(enh[j].CaseType.Trim()))
            //    {
            //        caseType.Add(enh[j].CaseType.Trim());
            //    }
            //}
            //caseType.Sort();
            List<EnhancedPortfolioDiversificationSelection> caseTypeList = new List<EnhancedPortfolioDiversificationSelection>();
            List<CaseSubTypesList> list = ComboBoxPopulationController.caseSubTypeList;
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            selectAll.CheckStatus = false;
            caseTypeList.Add(selectAll);

            //for (int i = 0; i < caseType.Count; i++)
            //{
            //    for (int u = 0; u < list.Count; u++)
            //    {
            //        if (caseType[i].Equals(list[u].CaseType.Trim()))
            //        {
            //            EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
            //            int val;
            //            int.TryParse(list.ElementAt(u).CaseSubtypesId, out val);
            //            obj.ID = val;
            //            obj.CheckStatus = false;
            //            obj.Name = list.ElementAt(u).CaseType;

            //            caseTypeList.Add(obj);
            //            break;
            //        }
            //    }

            //}
            return caseTypeList;
        }


        public static List<EnhancedPortfolioDiversificationSelection> AddCaseTypeInList(List<EnhancedPortfolioDiversification> enh)
        {
            List<string> caseType = new List<string>();
            for (int j = 0; j < enh.Count; j++)
            {
                if (!caseType.Contains(enh[j].CaseType.Trim()))
                {
                    caseType.Add(enh[j].CaseType.Trim());
                }
            }
            caseType.Sort();
            List<EnhancedPortfolioDiversificationSelection> caseTypeList = new List<EnhancedPortfolioDiversificationSelection>();
            List<CaseSubTypesList> list = ComboBoxPopulationController.caseSubTypeList;
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            //  selectAll.CheckStatus = true;
            caseTypeList.Add(selectAll);

            for (int i = 0; i < caseType.Count; i++)
            {
                for (int u = 0; u < list.Count; u++)
                {
                    if (caseType[i].Equals(list[u].CaseType.Trim()))
                    {
                        EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
                        int val;
                        int.TryParse(list.ElementAt(u).CaseSubtypesId, out val);
                        obj.ID = val;
                        //      obj.CheckStatus = true;
                        obj.Name = list.ElementAt(u).CaseType;

                        caseTypeList.Add(obj);
                        break;
                    }
                }

            }
            return caseTypeList;
        }

        public static List<EnhancedPortfolioDiversificationSelection> ClearFundedEntity()
        {

            List<EnhancedPortfolioDiversificationSelection> pdList = new List<EnhancedPortfolioDiversificationSelection>();
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            selectAll.CheckStatus = false;
            pdList.Add(selectAll);

            Dictionary<string, string> list = ComboBoxPopulationController.companyList;
            list = (from c in list orderby c.Value ascending select c).ToDictionary(c => c.Key, c => c.Value);

            for (int i = 0; i < list.Count; i++)
            {
                EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
                int val;
                int.TryParse(list.ElementAt(i).Key, out val);
                obj.ID = val;
                 obj.CheckStatus = false;
                obj.Name = list.ElementAt(i).Value;
                pdList.Add(obj);
            }
            return pdList;

        }
        public static List<EnhancedPortfolioDiversificationSelection> AddFundedEntityInList()
        {
            List<EnhancedPortfolioDiversificationSelection> pdList = new List<EnhancedPortfolioDiversificationSelection>();
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            // selectAll.CheckStatus = true;
            pdList.Add(selectAll);

            Dictionary<string, string> list = ComboBoxPopulationController.companyList;
            list = (from c in list orderby c.Value ascending select c).ToDictionary(c => c.Key, c => c.Value);

            for (int i = 0; i < list.Count; i++)
            {
                EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
                int val;
                int.TryParse(list.ElementAt(i).Key, out val);
                obj.ID = val;
                // obj.CheckStatus = true;
                obj.Name = list.ElementAt(i).Value;
                pdList.Add(obj);
            }
            return pdList;
        }


        public static List<EnhancedPortfolioDiversificationSelection> ClearBorrower()
        {
            List<EnhancedPortfolioDiversificationSelection> borrower = new List<EnhancedPortfolioDiversificationSelection>();
            //List<string> nature = new List<string>();
            //enh = enh.OrderBy(order => order.BorrowerName).ToList();
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            selectAll.CheckStatus = false;
            borrower.Add(selectAll);
            //for (int j = 0; j < enh.Count; j++)
            //{
            //    if (!nature.Contains(enh[j].BorrowerName.Trim()))
            //    {
            //        nature.Add(enh[j].BorrowerName.Trim());
            //        EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();

            //        obj.ID = enh[j].BorrowerID;
            //        obj.CheckStatus = false;
            //        obj.Name = enh[j].BorrowerName;

            //        borrower.Add(obj);

            //    }
            //}

            return borrower;
        }



        public static List<EnhancedPortfolioDiversificationSelection> AddBorrowerList(List<EnhancedPortfolioDiversification> enh)
        {
            List<EnhancedPortfolioDiversificationSelection> borrower = new List<EnhancedPortfolioDiversificationSelection>();
            List<string> nature = new List<string>();
            enh = enh.OrderBy(order => order.BorrowerName).ToList();
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            // selectAll.CheckStatus = true;
            borrower.Add(selectAll);
            for (int j = 0; j < enh.Count; j++)
            {
                if (!nature.Contains(enh[j].BorrowerName.Trim()))
                {
                    nature.Add(enh[j].BorrowerName.Trim());
                    EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();

                    obj.ID = enh[j].BorrowerID;
                    //   obj.CheckStatus = true;
                    obj.Name = enh[j].BorrowerName;

                    borrower.Add(obj);
                  //  .Add(enh[j].BorrowerID);borrowersIdList
                }
            }

            return borrower;
        }

        //public static Dictionary<int, double> addBorrowerDenomators()
        //{

        //    for (int i = 0; i < borrowersIdList.Count; i++)
        //    {
        //        SQLReportQueries.
        //        //enhancedDenominatorPercentOfTotalByCompanyFirm
        //    }
        //}

        public static List<EnhancedPortfolioDiversificationSelection> ClearTypeOfAction()
        {

            List<string> nature = new List<string>();
          
            List<EnhancedPortfolioDiversificationSelection> typeOfAction = new List<EnhancedPortfolioDiversificationSelection>();
            List<CaseSubTypesList> list = ComboBoxPopulationController.caseSubTypeList;
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            selectAll.CheckStatus = false;
            typeOfAction.Add(selectAll);

            //for (int i = 0; i < nature.Count; i++)
            //{
            //    for (int u = 0; u < list.Count; u++)
            //    {
            //        if (nature[i].Equals(list[u].TypeOfAction.Trim()))
            //        {
            //            EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
            //            int val;
            //            int.TryParse(list.ElementAt(u).CaseSubtypesId, out val);
            //            obj.ID = val;
            //            obj.CheckStatus = false;
            //            obj.Name = list.ElementAt(u).TypeOfAction;

            //            typeOfAction.Add(obj);
            //            break;
            //        }
            //    }

            //}
            return typeOfAction;
        }

        public static List<EnhancedPortfolioDiversificationSelection> AddTypeOfActionInList(List<EnhancedPortfolioDiversification> enh)
        {

            List<string> nature = new List<string>();
            for (int j = 0; j < enh.Count; j++)
            {
                if (!nature.Contains(enh[j].TypeOfAction.Trim()))
                {
                    nature.Add(enh[j].TypeOfAction.Trim());
                }
            }
            nature.Sort();
            List<EnhancedPortfolioDiversificationSelection> typeOfAction = new List<EnhancedPortfolioDiversificationSelection>();
            List<CaseSubTypesList> list = ComboBoxPopulationController.caseSubTypeList;
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            //   selectAll.CheckStatus = true;
            typeOfAction.Add(selectAll);

            for (int i = 0; i < nature.Count; i++)
            {
                for (int u = 0; u < list.Count; u++)
                {
                    if (nature[i].Equals(list[u].TypeOfAction.Trim()))
                    {
                        EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
                        int val;
                        int.TryParse(list.ElementAt(u).CaseSubtypesId, out val);
                        obj.ID = val;
                        //      obj.CheckStatus = true;
                        obj.Name = list.ElementAt(u).TypeOfAction;

                        typeOfAction.Add(obj);
                        break;
                    }
                }

            }
            return typeOfAction;
        }


        public static List<EnhancedPortfolioDiversificationSelection> ClearNatureOfClaim()
        {
            List<string> nature = new List<string>();
            //for (int j = 0; j < enh.Count; j++)
            //{
            //    if (!nature.Contains(enh[j].NatureOfClaim.Trim()))
            //    {
            //        nature.Add(enh[j].NatureOfClaim.Trim());
            //    }
            //}
            //nature.Sort();
            List<EnhancedPortfolioDiversificationSelection> natureOfClaimList = new List<EnhancedPortfolioDiversificationSelection>();
            List<CaseSubTypesList> list = ComboBoxPopulationController.caseSubTypeList;
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            selectAll.CheckStatus = false;
            natureOfClaimList.Add(selectAll);

            //for (int i = 0; i < nature.Count; i++)
            //{
            //    for (int u = 0; u < list.Count; u++)
            //    {
            //        if (nature[i].Equals(list[u].Nature_Of_Claim.Trim()))
            //        {
            //            EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
            //            int val;
            //            int.TryParse(list.ElementAt(u).CaseSubtypesId, out val);
            //            obj.ID = val;
            //            obj.CheckStatus = false;
            //            obj.Name = list.ElementAt(u).Nature_Of_Claim;

            //            natureOfClaimList.Add(obj);
            //            break;
            //        }
            //    }

            //}
            return natureOfClaimList;
        }


        public static List<EnhancedPortfolioDiversificationSelection> AddNatureOfClaimInList(List<EnhancedPortfolioDiversification> enh)
        {
            List<string> nature = new List<string>();
            for (int j = 0; j < enh.Count; j++)
            {
                if (!nature.Contains(enh[j].NatureOfClaim.Trim()))
                {
                    nature.Add(enh[j].NatureOfClaim.Trim());
                }
            }
            nature.Sort();
            List<EnhancedPortfolioDiversificationSelection> natureOfClaimList = new List<EnhancedPortfolioDiversificationSelection>();
            List<CaseSubTypesList> list = ComboBoxPopulationController.caseSubTypeList;
            EnhancedPortfolioDiversificationSelection selectAll = new EnhancedPortfolioDiversificationSelection();

            selectAll.ID = 0;
            selectAll.Name = "Select All";
            //   selectAll.CheckStatus = true;
            natureOfClaimList.Add(selectAll);

            for (int i = 0; i < nature.Count; i++)
            {
                for (int u = 0; u < list.Count; u++)
                {
                    if (nature[i].Equals(list[u].Nature_Of_Claim.Trim()))
                    {
                        EnhancedPortfolioDiversificationSelection obj = new EnhancedPortfolioDiversificationSelection();
                        int val;
                        int.TryParse(list.ElementAt(u).CaseSubtypesId, out val);
                        obj.ID = val;
                        //   obj.CheckStatus = true;
                        obj.Name = list.ElementAt(u).Nature_Of_Claim;

                        natureOfClaimList.Add(obj);
                        break;
                    }
                }

            }
            return natureOfClaimList;
        }

        public List<CaseSubTypesList> createCaseSubTypesList()
        {
            return sQLComboBoxPopulationQueries.createCaseSubTypesList();
        }

    }
}
