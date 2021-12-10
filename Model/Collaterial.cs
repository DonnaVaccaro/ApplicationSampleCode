using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class Collaterial
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Collaterial));
        private string borrowerCaseID;
        private string borrowerName;
        private string borrowerID;
        private string caseID;
        private string massTortID;
        private string loanID;
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
        private string grossSettlementDollars;
        private string attorneysFeePercent;
        private string attorneysFeeDollars;
        private string grossCoAndReferringCounselFeePercentage;
        private string grossCoAndReferringCounselFeeDollars;
        private string firmsNetFeesPercent;
        private string firmsNetFeesDollars;
        private string qtrSettlement;
        private string yearSettlement;
        private string notes;
        private string caseStatus;
        private string actualGrossSettlement;
        private string actualNetLegalFees;
        private string resolutionType;
        private string reasonForUpdateSelection;
        private string reasonForUpdateOther;
        private string userID;
        private string effectiveDate;
        private string resolvedDate;
        private string caseCreatedDate;
        private string caseModifiedDate;
        private string isTracked;
        private string numberOfCases;
        private string cfGrossSettlementDollars;
        private string cfFirmNetFeesDollars;
        private string cfQuaterSettlement;
        private string cfYearSettlement;
        private string caseCost;
        private string cfAttorneyFeeDollar;
        private string jurisdiction;
        private string isAudited;
        private string isDirectPay;
        private string coCounsel;



        public Collaterial()
        {
            log.Info("collaterial constructor");
            this.massTortID = "0";
            this.loanID = "2";
            this.borrowerID = "3";
            this.borrowerCaseID = "";
            this.reasonForUpdateSelection = "New Case Added";
            this.reasonForUpdateOther = "";
            this.caseStatus = "Open";
            this.caseType = OTS.Controller.ComboBoxPopulationController.defaultNA;
            this.notes = "";
            this.caseCreatedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            this.caseModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            this.effectiveDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            this.isTracked = "false";
            this.numberOfCases = "1";
            this.jurisdiction = "";
            this.isAudited = "false";
            this.isDirectPay = "false";
            this.coCounsel = "";
        }


        public string CaseID { get => caseID; set => caseID = value; }
        public string MassTortID { get => massTortID; set => massTortID = value; }
        public string LoanID { get => loanID; set => loanID = value; }
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
        public string GrossSettlementDollars { get => grossSettlementDollars; set => grossSettlementDollars = value; }
        public string AttorneysFeePercent { get => attorneysFeePercent; set => attorneysFeePercent = value; }
        public string AttorneysFeeDollars { get => attorneysFeeDollars; set => attorneysFeeDollars = value; }
        public string GrossCoAndReferringCounselFeePercentage { get => grossCoAndReferringCounselFeePercentage; set => grossCoAndReferringCounselFeePercentage = value; }
        public string GrossCoAndReferringCounselFeeDollars { get => grossCoAndReferringCounselFeeDollars; set => grossCoAndReferringCounselFeeDollars = value; }
        public string FirmsNetFeesPercent { get => firmsNetFeesPercent; set => firmsNetFeesPercent = value; }
        public string FirmsNetFeesDollars { get => firmsNetFeesDollars; set => firmsNetFeesDollars = value; }
        
        public string Notes { get => notes; set => notes = value; }
        public string BorrowerName { get => borrowerName; set => borrowerName = value; }
        public string CaseStatus { get => caseStatus; set => caseStatus = value; }
        public string ActualGrossSettlement { get => actualGrossSettlement; set => actualGrossSettlement = value; }
        public string ActualNetLegalFees { get => actualNetLegalFees; set => actualNetLegalFees = value; }
        public string ResolutionType { get => resolutionType; set => resolutionType = value; }
        public string ReasonForUpdateSelection { get => reasonForUpdateSelection; set => reasonForUpdateSelection = value; }
        public string UserID { get => userID; set => userID = value; }
        public string BorrowerID { get => borrowerID; set => borrowerID = value; }
       
        public string BorrowerCaseID { get => borrowerCaseID; set => borrowerCaseID = value; }
        public string ReasonForUpdateOther { get => reasonForUpdateOther; set => reasonForUpdateOther = value; }
        public string IsTracked { get => isTracked; set => isTracked = value; }
        public string QtrSettlement { get => qtrSettlement; set => qtrSettlement = value; }
        public string YearSettlement { get => yearSettlement; set => yearSettlement = value; }
        public string EffectiveDate { get => effectiveDate; set => effectiveDate = value; }
        public string ResolvedDate { get => resolvedDate; set => resolvedDate = value; }
        public string CaseCreatedDate { get => caseCreatedDate; set => caseCreatedDate = value; }
        public string CaseModifiedDate { get => caseModifiedDate; set => caseModifiedDate = value; }
        public string NumberOfCases { get => numberOfCases; set => numberOfCases = value; }
        public string CfGrossSettlementDollars { get => cfGrossSettlementDollars; set => cfGrossSettlementDollars = value; }
        public string CfFirmNetFeesDollars { get => cfFirmNetFeesDollars; set => cfFirmNetFeesDollars = value; }
        public string CfQuaterSettlement { get => cfQuaterSettlement; set => cfQuaterSettlement = value; }
        public string CfYearSettlement { get => cfYearSettlement; set => cfYearSettlement = value; }
        public string CaseCost { get => caseCost; set => caseCost = value; }
        public string CfAttorneyFeeDollar { get => cfAttorneyFeeDollar; set => cfAttorneyFeeDollar = value; }
        public string Jurisdiction { get => jurisdiction; set => jurisdiction = value; }
        public string IsAudited { get => isAudited; set => isAudited = value; }
        public string IsDirectPay { get => isDirectPay; set => isDirectPay = value; }
        public string CoCounsel { get => coCounsel; set => coCounsel = value; }




        //Numbers need to be changed from a percentage in the user input screen to a decimal in the database
        //Input: percentage from user input on add loan page
        //Output: conversion from a percentage to a decimal only to to store in database
        public double convertPercentageToDecimal(string percent)
        {
            log.Info("convertPercentageToDecimal");
            if (percent == null)
            {
                return 0;
            }
            double temp;
            double.TryParse(percent, out temp);
            double result = temp / 100;
            result = Math.Round(result, 7);
            return result;
        }

        //Numbers need to be changes from database decimal to user input page percentage
        //Input: Decimal from database
        //Oupput: converted decimal from database to percentage to be displayed on the loan page for user input
        public string convertDecimalToPercentage(string decimalInput)
        {

            log.Info("convertDecimalToPercentage");
            if (decimalInput == null)
            {
                return "0";
            }
            double temp;
            double.TryParse(decimalInput, out temp);
            double result = temp * 100;
            result = Math.Round(result, 7);
            return result.ToString();

        }


    }





}
