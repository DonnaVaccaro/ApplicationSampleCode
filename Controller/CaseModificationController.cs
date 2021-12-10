using OTS.DataBase;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTS.Controller
{
    public class CaseModificationController
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CaseModificationController));
        private readonly SQLCaseModificationQueries sQLCaseModificationQueries;
       
        

        public CaseModificationController()
        {
            log.Info("Constructor case Modification Controller");
            sQLCaseModificationQueries = new SQLCaseModificationQueries();
          
          
        }

        public bool addNewCaseModification(Collaterial collaterial, Collaterial updateCollaterial, CaseModification caseModifiction)
        {
            log.Info("Add new case modification");
            if (collaterial == null)
            {
                log.Error("Trying to add a case modification and the loan is null");
                return false;
            }

            if (updateCollaterial == null)
            {
                log.Error("Trying to add a case modification and the loan is null");
                return false;
            }

            if (caseModifiction == null)
            {
                log.Error("Trying to add a case modification and the case modification is null");
                return false;
            }
            DateTime dateTimeValue;

            double doubleValue;
            int intValue = 1;
            bool boolValue = false;
            string res = "1";
            string res1 = "1";
            bool addNewCaseResult = true;
            string messageBox;
            Collaterial c = collaterial;
            int.TryParse(collaterial.CaseID, out intValue);
            caseModifiction.CaseID = intValue;
            caseModifiction.PreviousBorrowerCaseID = collaterial.BorrowerCaseID;
            caseModifiction.NewBorrowerCaseID = updateCollaterial.BorrowerCaseID;
            int.TryParse(collaterial.BorrowerID, out intValue);
            caseModifiction.BorrowerID = intValue;

            int.TryParse(collaterial.LoanID, out intValue);
            caseModifiction.LoanID = intValue;

            int.TryParse(collaterial.MassTortID, out intValue);
            caseModifiction.MassTortID = intValue;


            //Starting Previous values**************************************
            caseModifiction.PreviousPlaintiff = collaterial.Plaintiff;

            caseModifiction.PreviousPrimaryDefendant = collaterial.PrimaryDefendant;

            //Converting user selection to id
            if (ComboBoxPopulationController.caseTypeList.ContainsValue(collaterial.CaseType))
            {

                res = ComboBoxPopulationController.caseTypeList.FirstOrDefault(x => x.Value == collaterial.CaseType).Key.Trim();
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCaseTypeIndex;
            }
            caseModifiction.PreviousCaseType = intValue;

            //Converting user selection to id
            if (ComboBoxPopulationController.caseStatusList.ContainsValue(collaterial.CaseStatus))
            {

                res = ComboBoxPopulationController.caseStatusList.FirstOrDefault(x => x.Value == collaterial.CaseStatus).Key.Trim();
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCaseStatusIndex;
            }
            caseModifiction.PreviousCaseStatus = intValue;
            caseModifiction.PreviousPrimaryInjury = collaterial.PrimaryInjury;


            //Date of injury
            if (!DateTime.TryParse(collaterial.DateOfInjury, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            
            caseModifiction.PreviousDateOfInjury = dateTimeValue;

            // Date Retainser Signed
            if (!DateTime.TryParse(collaterial.DateRetainerSigned, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.PreviousDateRetainerSigned = dateTimeValue;

            // Date filed
            if (!DateTime.TryParse(collaterial.DateFiled, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.PreviousDateFiled = dateTimeValue;

            StateCourtLookup.setStateCourtIDs(collaterial.StateCircuit, collaterial.CourtVenue);
            caseModifiction.PreviousStateCircuit = StateCourtLookup.getStateId();
            caseModifiction.PreviousCourtVenue = StateCourtLookup.getCourtId();

            caseModifiction.PreviousJurisdiction = collaterial.Jurisdiction;

            caseModifiction.PreviousDocketNumber = collaterial.DocketNumber;

            caseModifiction.PreviousJudge = collaterial.Judge;

            //Converting user selection to id
            if (ComboBoxPopulationController.caseStageList.ContainsValue(collaterial.CaseStage))
            {

                res = ComboBoxPopulationController.caseStageList.FirstOrDefault(x => x.Value == collaterial.CaseStage).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCaseStageIndex;
            }
            if (intValue == 34)
            {
                intValue = ComboBoxPopulationController.defaultCaseStageIndex;
            }
            caseModifiction.PreviousCaseStage = intValue;

            if (InsuranceCompanyController.insuranceCompanyListLookUp.ContainsValue(collaterial.InsuranceCarrier))
            {

                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == collaterial.InsuranceCarrier).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == "NA").Key;
                int.TryParse(res, out intValue);
            }
            caseModifiction.PreviousInsuranceCarrier = intValue;

            double.TryParse(collaterial.InsuranceCoverageDollars, out doubleValue);
            caseModifiction.PreviousInsuranceCoverageDollars = doubleValue;

            double.TryParse(collaterial.CaseCost, out doubleValue);
            caseModifiction.PreviousCaseCost = doubleValue;

            if (InsuranceCompanyController.insuranceCompanyListLookUp.ContainsValue(collaterial.ExcessCarrier))
            {

                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == collaterial.ExcessCarrier).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == "NA").Key;
                int.TryParse(res, out intValue);
            }
            caseModifiction.PreviousExcessCarrier = intValue;

            double.TryParse(collaterial.ExcessCoverageDollars, out doubleValue);
            caseModifiction.PreviousExcessCoverageDollars = doubleValue;


            double.TryParse(collaterial.GrossSettlementDollars, out doubleValue);
            caseModifiction.PreviousGrossSettlementDollars = doubleValue;

            caseModifiction.PreviousAttorneysFeePercent = collaterial.convertPercentageToDecimal(collaterial.AttorneysFeePercent);

            double.TryParse(collaterial.AttorneysFeeDollars, out doubleValue);
            caseModifiction.PreviousAttorneysFeeDollars = doubleValue;

            caseModifiction.PreviousGrossCoAndReferringCouncelFeePercentage = collaterial.convertPercentageToDecimal(collaterial.GrossCoAndReferringCounselFeePercentage);

            double.TryParse(collaterial.GrossCoAndReferringCounselFeeDollars, out doubleValue);
            caseModifiction.PreviousGrossCoAndReferringCouncelFeeDollars = doubleValue;

            caseModifiction.PreviousFirmsNetFeesPercent = collaterial.convertPercentageToDecimal(collaterial.FirmsNetFeesPercent);

            double.TryParse(collaterial.FirmsNetFeesDollars, out doubleValue);
            caseModifiction.PreviousFirmsNetFeesDollars = doubleValue;


            //collaterial does not separate qtr and year and they are not ints
            int.TryParse(collaterial.QtrSettlement, out intValue);
            caseModifiction.PreviousQtrSettlement = intValue;
            //collaterial does not separate qtr and year and they are not ints
            int.TryParse(collaterial.YearSettlement, out intValue);
            caseModifiction.PreviousYearSettlement = intValue;
            caseModifiction.PreviousNotes = collaterial.Notes;
            caseModifiction.PreviousUserID = collaterial.UserID;

            double.TryParse(collaterial.ActualGrossSettlement, out doubleValue);
            caseModifiction.PreviousActualGrossSettlement = doubleValue;
            double.TryParse(collaterial.ActualNetLegalFees, out doubleValue);
            caseModifiction.PreviousActualNetLegalFees = doubleValue;

            //Converting user selection to id
            if (ComboBoxPopulationController.resolutionTypeList.ContainsValue(collaterial.ResolutionType))
            {

                res = ComboBoxPopulationController.resolutionTypeList.FirstOrDefault(x => x.Value == collaterial.ResolutionType).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultResolutionTypeIndex;
            }
            caseModifiction.PreviousResolutionType = intValue;

            if (ComboBoxPopulationController.reasonForUpdateCaseList.ContainsValue(collaterial.ReasonForUpdateSelection))
            {

                res = ComboBoxPopulationController.reasonForUpdateCaseList.FirstOrDefault(x => x.Value == collaterial.ReasonForUpdateSelection).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultReasonForUpdateIndex;
            }
            caseModifiction.PreviousReasonForUpdateSelection = intValue;

            caseModifiction.PreviousReasonForUpdateOther = collaterial.ReasonForUpdateOther;

            if (!bool.TryParse(collaterial.IsTracked, out boolValue))
            {
                boolValue = false;
            }
         
            caseModifiction.PreviousIsTracked = boolValue;

            if (!bool.TryParse(collaterial.IsAudited, out boolValue))
            {
                boolValue = false;
            }
           
            caseModifiction.PreviousIsAudited = boolValue;

            if (!bool.TryParse(collaterial.IsDirectPay, out boolValue))
            {
                boolValue = false;
            }

            caseModifiction.PreviousIsDirectPay = boolValue;

            caseModifiction.PreviousCoCounsel = collaterial.CoCounsel;

            // effective date
            if (!DateTime.TryParse(collaterial.EffectiveDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.PreviousEffectiveDate = dateTimeValue;


            // resolution date
           
            if (!DateTime.TryParse(collaterial.ResolvedDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            
                caseModifiction.PreviousResolvedDate = dateTimeValue;
            
            

            int.TryParse(collaterial.NumberOfCases, out intValue);
            caseModifiction.PreviousNumberOfCases = intValue;


            double.TryParse(collaterial.CfGrossSettlementDollars, out doubleValue);
            caseModifiction.PreviousCFGrossSettlementDollars = doubleValue;

            double.TryParse(collaterial.CfFirmNetFeesDollars, out doubleValue);
            caseModifiction.PreviousCFFirmNetFeesDollars = doubleValue;

            int.TryParse(collaterial.CfQuaterSettlement, out intValue);
            caseModifiction.PreviousCFQuaterSettlement = intValue;

            int.TryParse(collaterial.CfYearSettlement, out intValue);
            caseModifiction.PreviousCFYearSettlement = intValue;

            double.TryParse(collaterial.CfAttorneyFeeDollar, out doubleValue);
            caseModifiction.PreviousCFAttorneyFeeDollar = doubleValue;
            //DMV********************still need to add New values


            double.TryParse(updateCollaterial.CaseCost, out doubleValue);
            caseModifiction.NewCaseCost = doubleValue;

            caseModifiction.NewPlaintiff = updateCollaterial.Plaintiff;

            caseModifiction.NewPrimaryDefendant = updateCollaterial.PrimaryDefendant;

            //Converting user selection to id
            if (ComboBoxPopulationController.caseTypeList.ContainsValue(updateCollaterial.CaseType))
            {

                res = ComboBoxPopulationController.caseTypeList.FirstOrDefault(x => x.Value == updateCollaterial.CaseType).Key.Trim();
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCaseTypeIndex;
            }
            caseModifiction.NewCaseType = intValue;

            //Converting user selection to id
            if (ComboBoxPopulationController.caseStatusList.ContainsValue(updateCollaterial.CaseStatus))
            {

                res = ComboBoxPopulationController.caseStatusList.FirstOrDefault(x => x.Value == updateCollaterial.CaseStatus).Key.Trim();
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCaseStatusIndex;
            }
            caseModifiction.NewCaseStatus = intValue;
            caseModifiction.NewPrimaryInjury = updateCollaterial.PrimaryInjury;


            //Date of injury
            if (!DateTime.TryParse(updateCollaterial.DateOfInjury, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.NewDateOfInjury = dateTimeValue;

            // Date Retainser Signed
            if (!DateTime.TryParse(updateCollaterial.DateRetainerSigned, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.NewDateRetainerSigned = dateTimeValue;

            // Date filed
            if (!DateTime.TryParse(updateCollaterial.DateFiled, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.NewDateFiled = dateTimeValue;

            StateCourtLookup.setStateCourtIDs(updateCollaterial.StateCircuit, updateCollaterial.CourtVenue);
            caseModifiction.NewStateCircuit = StateCourtLookup.getStateId();
            caseModifiction.NewCourtVenue = StateCourtLookup.getCourtId();

            caseModifiction.NewJurisdiction = updateCollaterial.Jurisdiction;

            caseModifiction.NewDocketNumber = updateCollaterial.DocketNumber;

            caseModifiction.NewJudge = updateCollaterial.Judge;

            //Converting user selection to id
            if (ComboBoxPopulationController.caseStageList.ContainsValue(updateCollaterial.CaseStage))
            {

                res = ComboBoxPopulationController.caseStageList.FirstOrDefault(x => x.Value == updateCollaterial.CaseStage).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCaseStageIndex;
            }
            caseModifiction.NewCaseStage = intValue;

            if (InsuranceCompanyController.insuranceCompanyListLookUp.ContainsValue(updateCollaterial.InsuranceCarrier))
            {

                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == updateCollaterial.InsuranceCarrier).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == "NA").Key;
                int.TryParse(res, out intValue);
            }
            caseModifiction.NewInsuranceCarrier = intValue;

            double.TryParse(updateCollaterial.InsuranceCoverageDollars, out doubleValue);
            caseModifiction.NewInsuranceCoverageDollars = doubleValue;

            if (InsuranceCompanyController.insuranceCompanyListLookUp.ContainsValue(updateCollaterial.ExcessCarrier))
            {

                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == updateCollaterial.ExcessCarrier).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                res = InsuranceCompanyController.insuranceCompanyListLookUp.FirstOrDefault(x => x.Value == "NA").Key;
                int.TryParse(res, out intValue);
            }
            caseModifiction.NewExcessCarrier = intValue;

            double.TryParse(updateCollaterial.ExcessCoverageDollars, out doubleValue);
            caseModifiction.NewExcessCoverageDollars = doubleValue;


            double.TryParse(updateCollaterial.GrossSettlementDollars, out doubleValue);
            caseModifiction.NewGrossSettlementDollars = doubleValue;

            caseModifiction.NewAttorneysFeePercent = updateCollaterial.convertPercentageToDecimal(updateCollaterial.AttorneysFeePercent);

            double.TryParse(updateCollaterial.AttorneysFeeDollars, out doubleValue);
            caseModifiction.NewAttorneysFeeDollars = doubleValue;

            caseModifiction.NewGrossCoAndReferringCouncelFeePercentage = updateCollaterial.convertPercentageToDecimal(updateCollaterial.GrossCoAndReferringCounselFeePercentage);

            double.TryParse(updateCollaterial.GrossCoAndReferringCounselFeeDollars, out doubleValue);
            caseModifiction.NewGrossCoAndReferringCouncelFeeDollars = doubleValue;

            caseModifiction.NewFirmsNetFeesPercent = updateCollaterial.convertPercentageToDecimal(updateCollaterial.FirmsNetFeesPercent);

            double.TryParse(updateCollaterial.FirmsNetFeesDollars, out doubleValue);
            caseModifiction.NewFirmsNetFeesDollars = doubleValue;


            //updateCollaterial does not separate qtr and year and they are not ints
            int.TryParse(updateCollaterial.QtrSettlement, out intValue);
            caseModifiction.NewQtrSettlement = intValue;
            //updateCollaterial does not separate qtr and year and they are not ints
            int.TryParse(updateCollaterial.YearSettlement, out intValue);
            caseModifiction.NewYearSettlement = intValue;
            caseModifiction.NewNotes = updateCollaterial.Notes;
            caseModifiction.NewUserID = updateCollaterial.UserID;

            double.TryParse(updateCollaterial.ActualGrossSettlement, out doubleValue);
            caseModifiction.NewActualGrossSettlement = doubleValue;
            double.TryParse(updateCollaterial.ActualNetLegalFees, out doubleValue);
            caseModifiction.NewActualNetLegalFees = doubleValue;

            //Converting user selection to id
            if (ComboBoxPopulationController.resolutionTypeList.ContainsValue(updateCollaterial.ResolutionType))
            {

                res = ComboBoxPopulationController.resolutionTypeList.FirstOrDefault(x => x.Value == updateCollaterial.ResolutionType).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultResolutionTypeIndex;
            }
            caseModifiction.NewResolutionType = intValue;

            if (ComboBoxPopulationController.reasonForUpdateCaseList.ContainsValue(updateCollaterial.ReasonForUpdateSelection))
            {

                res = ComboBoxPopulationController.reasonForUpdateCaseList.FirstOrDefault(x => x.Value == updateCollaterial.ReasonForUpdateSelection).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultReasonForUpdateIndex;
            }
            caseModifiction.NewReasonForUpdateSelection = intValue;

            caseModifiction.NewReasonForUpdateOther = updateCollaterial.ReasonForUpdateOther;

            if (!bool.TryParse(updateCollaterial.IsTracked, out boolValue))
            {
                boolValue = false;
            }
            
            caseModifiction.NewIsTracked = boolValue;

            if (!bool.TryParse(updateCollaterial.IsAudited, out boolValue))
            {
                boolValue = false;
            }
            
            caseModifiction.NewIsAudited = boolValue;

            if (!bool.TryParse(updateCollaterial.IsDirectPay, out boolValue))
            {
                boolValue = false;
            }

            caseModifiction.NewIsDirectPay = boolValue;

            caseModifiction.NewCoCounsel = updateCollaterial.CoCounsel;



            // effective date
            if (!DateTime.TryParse(updateCollaterial.EffectiveDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            caseModifiction.NewEffectiveDate = dateTimeValue;


            // resolution date
            if (!DateTime.TryParse(updateCollaterial.ResolvedDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            
                caseModifiction.NewResolvedDate = dateTimeValue;
           
           

            if (!DateTime.TryParse(collaterial.CaseCreatedDate, out dateTimeValue))
            {
                dateTimeValue = DateTime.Now;
            }
            caseModifiction.CaseCreatedDate = dateTimeValue;

            if (!DateTime.TryParse(updateCollaterial.CaseModifiedDate, out dateTimeValue))
            {
                dateTimeValue = DateTime.Now;
            }
            caseModifiction.CaseModifiedDate = dateTimeValue;

            int.TryParse(updateCollaterial.NumberOfCases, out intValue);
            caseModifiction.NewNumberOfCases = intValue;

            double.TryParse(updateCollaterial.CfGrossSettlementDollars, out doubleValue);
            caseModifiction.NewCFGrossSettlementDollars = doubleValue;

            double.TryParse(updateCollaterial.CfFirmNetFeesDollars, out doubleValue);
            caseModifiction.NewCFFirmNetFeesDollars = doubleValue;

            int.TryParse(updateCollaterial.CfQuaterSettlement, out intValue);
            caseModifiction.NewCFQuaterSettlement = intValue;

            int.TryParse(updateCollaterial.CfYearSettlement, out intValue);
            caseModifiction.NewCFYearSettlement = intValue;

            double.TryParse(updateCollaterial.CfAttorneyFeeDollar, out doubleValue);
            caseModifiction.NewCFAttorneyFeeDollar = doubleValue;

            sQLCaseModificationQueries.addNewCaseModification(caseModifiction);

            return true;
        }

        public bool updatedTerminationDate(string caseid, string effectiveDate)
        {
            int value;
            DateTime dateTime;
            int.TryParse(caseid, out value);
            DateTime.TryParse(effectiveDate, out dateTime);
            
            bool result = sQLCaseModificationQueries.updatedTerminationDate(value, dateTime);

            return result;
        }


        }
}
