using System;
using System.Collections.Generic;
using System.Text;

namespace CaseManager.Model
{
    public class CaseListTemplate
    {
        private string borrowerCaseID;
        private string plaintiff;
        private string primaryDefendant;
        private string caseType;
        private string primaryInjury;
        private string dateOfInjury;
        private string dateRetainerSigned;
        private string dateFiled;
        private string stateCircuit;
        private string courtVenue;
        private string docketNumber;
        private string judge;
        private string caseStage;
        private string insuranceCarrier;
        private string insuranceCoverageDollars;
        private string excessCarrier;
        private string excessCoverageDollars;
        private string caseCost;
        private string grossSettlementDollars;
        private string attorneysFeePercent;
        private string attorneysFeeDollars;
        private string grossCoAndReferringCouncelFeePercentage;
        private string grossCoAndReferringCouncelFeeDollars;
        private string firmsNetFeesPercent;
        private string firmsNetFeesDollars;
        private string qtrSettlement;
        private string yearSettlement;
        private string cfGrossSettlementDollars;
        private string cfAttorneyFeeDollar;
        private string cfFirmNetFeesDollars;
        private string cfQuaterSettlement;
        private string cfYearSettlement;
        private string numberOfCases;
        private string notes;
        private string isTracked;
        private string isAudited;
        private string caseStatus;
        private string actualGrossSettlement;
        private string actualNetLegalFees;
        private string jurisdiction;


        public CaseListTemplate()
        {

        }

        public string BorrowerCaseID { get => borrowerCaseID; set => borrowerCaseID = value; }
        public string Plaintiff { get => plaintiff; set => plaintiff = value; }
        public string PrimaryDefendant { get => primaryDefendant; set => primaryDefendant = value; }
        public string CaseType { get => caseType; set => caseType = value; }
        public string PrimaryInjury { get => primaryInjury; set => primaryInjury = value; }
        public string DateOfInjury { get => dateOfInjury; set => dateOfInjury = value; }
        public string DateRetainerSigned { get => dateRetainerSigned; set => dateRetainerSigned = value; }
        public string DateFiled { get => dateFiled; set => dateFiled = value; }
        public string StateCircuit { get => stateCircuit; set => stateCircuit = value; }
        public string CourtVenue { get => courtVenue; set => courtVenue = value; }
        public string DocketNumber { get => docketNumber; set => docketNumber = value; }
        public string Judge { get => judge; set => judge = value; }
        public string CaseStage { get => caseStage; set => caseStage = value; }
        public string InsuranceCarrier { get => insuranceCarrier; set => insuranceCarrier = value; }
        public string InsuranceCoverageDollars { get => insuranceCoverageDollars; set => insuranceCoverageDollars = value; }
        public string ExcessCarrier { get => excessCarrier; set => excessCarrier = value; }
        public string ExcessCoverageDollars { get => excessCoverageDollars; set => excessCoverageDollars = value; }
        public string CaseCost { get => caseCost; set => caseCost = value; }
        public string GrossSettlementDollars { get => grossSettlementDollars; set => grossSettlementDollars = value; }
        public string AttorneysFeePercent { get => attorneysFeePercent; set => attorneysFeePercent = value; }
        public string AttorneysFeeDollars { get => attorneysFeeDollars; set => attorneysFeeDollars = value; }
        public string GrossCoAndReferringCouncelFeePercentage { get => grossCoAndReferringCouncelFeePercentage; set => grossCoAndReferringCouncelFeePercentage = value; }
        public string GrossCoAndReferringCouncelFeeDollars { get => grossCoAndReferringCouncelFeeDollars; set => grossCoAndReferringCouncelFeeDollars = value; }
        public string FirmsNetFeesPercent { get => firmsNetFeesPercent; set => firmsNetFeesPercent = value; }
        public string FirmsNetFeesDollars { get => firmsNetFeesDollars; set => firmsNetFeesDollars = value; }
        public string QtrSettlement { get => qtrSettlement; set => qtrSettlement = value; }
        public string YearSettlement { get => yearSettlement; set => yearSettlement = value; }
        public string CfGrossSettlementDollars { get => cfGrossSettlementDollars; set => cfGrossSettlementDollars = value; }
        public string CfAttorneyFeeDollar { get => cfAttorneyFeeDollar; set => cfAttorneyFeeDollar = value; }
        public string CfFirmNetFeesDollars { get => cfFirmNetFeesDollars; set => cfFirmNetFeesDollars = value; }
        public string CfQuaterSettlement { get => cfQuaterSettlement; set => cfQuaterSettlement = value; }
        public string CfYearSettlement { get => cfYearSettlement; set => cfYearSettlement = value; }
        public string NumberOfCases { get => numberOfCases; set => numberOfCases = value; }
        public string Notes { get => notes; set => notes = value; }
        public string IsTracked { get => isTracked; set => isTracked = value; }
        public string IsAudited { get => isAudited; set => isAudited = value; }
        public string CaseStatus { get => caseStatus; set => caseStatus = value; }
        public string ActualGrossSettlement { get => actualGrossSettlement; set => actualGrossSettlement = value; }
        public string ActualNetLegalFees { get => actualNetLegalFees; set => actualNetLegalFees = value; }
        public string Jurisdiction { get => jurisdiction; set => jurisdiction = value; }
    }
}
