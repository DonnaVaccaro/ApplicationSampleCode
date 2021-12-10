using log4net;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class CaseModification
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Collaterial));
        private string previousBorrowerCaseID;
        private string newBorrowerCaseID;
        private string borrowerName;
        private int borrowerID;
        private int caseID;
        private int massTortID;
        private int loanID;

        //Previous values
        private string previousPlaintiff;
        private string previousPrimaryDefendant;
        private int previousCaseType;
        private string previousPrimaryInjury;
        private DateTime previousDateOfInjury;
        private DateTime previousDateRetainerSigned;
        private DateTime previousDateFiled;
        private int previousStateCircuit;
        private int previousCourtVenue;
        private string previousDocketNumber;
        private string previousJudge;
        private int previousCaseStage;
        private int previousInsuranceCarrier;
        private double previousInsuranceCoverageDollars;
        private int previousExcessCarrier;
        private double previousExcessCoverageDollars;
        private double previousGrossSettlementDollars;
        private double previousAttorneysFeePercent;
        private double previousAttorneysFeeDollars;
        private double previousGrossCoAndReferringCouncelFeePercentage;
        private double previousGrossCoAndReferringCouncelFeeDollars;
        private double previousFirmsNetFeesPercent;
        private double previousFirmsNetFeesDollars;
        private int previousQtrSettlement;
        private int previousYearSettlement;
        private string previousNotes;
        private int previousCaseStatus;
        private double previousActualGrossSettlement;
        private double previousActualNetLegalFees;
        private int previousResolutionType;
        private int previousReasonForUpdateSelection;
        private string previousReasonForUpdateOther;
        private string previousUserID;
        private bool previousIsTracked;
        private DateTime previousEffectiveDate;
        private DateTime? previousResolvedDate;
        private int previousNumberOfCases;

        //New values
        private string newPlaintiff;
        private string newPrimaryDefendant;
        private int newCaseType;
        private string newPrimaryInjury;
        private DateTime newDateOfInjury;
        private DateTime newDateRetainerSigned;
        private DateTime newDateFiled;
        private int newStateCircuit;
        private int newCourtVenue;
        private string newDocketNumber;
        private string newJudge;
        private int newCaseStage;
        private int newInsuranceCarrier;
        private double newInsuranceCoverageDollars;
        private int newExcessCarrier;
        private double newExcessCoverageDollars;
        private double newGrossSettlementDollars;
        private double newAttorneysFeePercent;
        private double newAttorneysFeeDollars;
        private double newGrossCoAndReferringCouncelFeePercentage;
        private double newGrossCoAndReferringCouncelFeeDollars;
        private double newFirmsNetFeesPercent;
        private double newFirmsNetFeesDollars;
        private int newQtrSettlement;
        private int newYearSettlement;
        private string newNotes;
        private int newCaseStatus;
        private double newActualGrossSettlement;
        private double newActualNetLegalFees;
        private int newResolutionType;
        private int newReasonForUpdateSelection;
        private string newReasonForUpdateOther;
        private string newUserID;
        private bool newIsTracked;
        private DateTime newEffectiveDate;
        private DateTime? newResolvedDate;
        private DateTime caseCreatedDate;
        private DateTime caseModifiedDate;
        private int newNumberOfCases;

        private double newCFGrossSettlementDollars;
        private double newCFFirmNetFeesDollars;
        private int newCFQuaterSettlement;
        private int newCFYearSettlement;

        private double previousCFGrossSettlementDollars;
        private double previousCFFirmNetFeesDollars;
        private int previousCFQuaterSettlement;
        private int previousCFYearSettlement;
        private double previousCaseCost;
        private double newCaseCost;

        private double previousCFAttorneyFeeDollar;
        private double newCFAttorneyFeeDollar;
        private string previousJurisdiction;
        private string newJurisdiction;
        private bool previousIsAudited;
        private bool newIsAudited;
        private DateTime terminationDate;
        private bool previousIsDirectPay;
        private bool newIsDirectPay;
        private string previousCoCounsel;
        private string newCoCounsel;

        public CaseModification()
        {

        }

        public static ILog Log => log;

        public string PreviousBorrowerCaseID { get => previousBorrowerCaseID; set => previousBorrowerCaseID = value; }
        public string NewBorrowerCaseID { get => newBorrowerCaseID; set => newBorrowerCaseID = value; }
        public string BorrowerName { get => borrowerName; set => borrowerName = value; }
        public int BorrowerID { get => borrowerID; set => borrowerID = value; }
        public int CaseID { get => caseID; set => caseID = value; }
        public int MassTortID { get => massTortID; set => massTortID = value; }
        public int LoanID { get => loanID; set => loanID = value; }
        public string PreviousPlaintiff { get => previousPlaintiff; set => previousPlaintiff = value; }
        public string PreviousPrimaryDefendant { get => previousPrimaryDefendant; set => previousPrimaryDefendant = value; }
        public int PreviousCaseType { get => previousCaseType; set => previousCaseType = value; }
        public string PreviousPrimaryInjury { get => previousPrimaryInjury; set => previousPrimaryInjury = value; }
        public DateTime PreviousDateOfInjury { get => previousDateOfInjury; set => previousDateOfInjury = value; }
        public DateTime PreviousDateRetainerSigned { get => previousDateRetainerSigned; set => previousDateRetainerSigned = value; }
        public DateTime PreviousDateFiled { get => previousDateFiled; set => previousDateFiled = value; }
        public int PreviousStateCircuit { get => previousStateCircuit; set => previousStateCircuit = value; }
        public int PreviousCourtVenue { get => previousCourtVenue; set => previousCourtVenue = value; }
        public string PreviousDocketNumber { get => previousDocketNumber; set => previousDocketNumber = value; }
        public string PreviousJudge { get => previousJudge; set => previousJudge = value; }
        public int PreviousCaseStage { get => previousCaseStage; set => previousCaseStage = value; }
        public int PreviousInsuranceCarrier { get => previousInsuranceCarrier; set => previousInsuranceCarrier = value; }
        public double PreviousInsuranceCoverageDollars { get => previousInsuranceCoverageDollars; set => previousInsuranceCoverageDollars = value; }
        public int PreviousExcessCarrier { get => previousExcessCarrier; set => previousExcessCarrier = value; }
        public double PreviousExcessCoverageDollars { get => previousExcessCoverageDollars; set => previousExcessCoverageDollars = value; }
        public double PreviousGrossSettlementDollars { get => previousGrossSettlementDollars; set => previousGrossSettlementDollars = value; }
        public double PreviousAttorneysFeePercent { get => previousAttorneysFeePercent; set => previousAttorneysFeePercent = value; }
        public double PreviousAttorneysFeeDollars { get => previousAttorneysFeeDollars; set => previousAttorneysFeeDollars = value; }
        public double PreviousGrossCoAndReferringCouncelFeePercentage { get => previousGrossCoAndReferringCouncelFeePercentage; set => previousGrossCoAndReferringCouncelFeePercentage = value; }
        public double PreviousGrossCoAndReferringCouncelFeeDollars { get => previousGrossCoAndReferringCouncelFeeDollars; set => previousGrossCoAndReferringCouncelFeeDollars = value; }
        public double PreviousFirmsNetFeesPercent { get => previousFirmsNetFeesPercent; set => previousFirmsNetFeesPercent = value; }
        public double PreviousFirmsNetFeesDollars { get => previousFirmsNetFeesDollars; set => previousFirmsNetFeesDollars = value; }
        public int PreviousQtrSettlement { get => previousQtrSettlement; set => previousQtrSettlement = value; }
        public int PreviousYearSettlement { get => previousYearSettlement; set => previousYearSettlement = value; }
        public string PreviousNotes { get => previousNotes; set => previousNotes = value; }
        public int PreviousCaseStatus { get => previousCaseStatus; set => previousCaseStatus = value; }
        public double PreviousActualGrossSettlement { get => previousActualGrossSettlement; set => previousActualGrossSettlement = value; }
        public double PreviousActualNetLegalFees { get => previousActualNetLegalFees; set => previousActualNetLegalFees = value; }
        public int PreviousResolutionType { get => previousResolutionType; set => previousResolutionType = value; }
        public int PreviousReasonForUpdateSelection { get => previousReasonForUpdateSelection; set => previousReasonForUpdateSelection = value; }
        public string PreviousReasonForUpdateOther { get => previousReasonForUpdateOther; set => previousReasonForUpdateOther = value; }
        public string PreviousUserID { get => previousUserID; set => previousUserID = value; }
        public bool PreviousIsTracked { get => previousIsTracked; set => previousIsTracked = value; }
        public DateTime PreviousEffectiveDate { get => previousEffectiveDate; set => previousEffectiveDate = value; }
       
        public int PreviousNumberOfCases { get => previousNumberOfCases; set => previousNumberOfCases = value; }
        public string NewPlaintiff { get => newPlaintiff; set => newPlaintiff = value; }
        public string NewPrimaryDefendant { get => newPrimaryDefendant; set => newPrimaryDefendant = value; }
        public int NewCaseType { get => newCaseType; set => newCaseType = value; }
        public string NewPrimaryInjury { get => newPrimaryInjury; set => newPrimaryInjury = value; }
        public DateTime NewDateOfInjury { get => newDateOfInjury; set => newDateOfInjury = value; }
        public DateTime NewDateRetainerSigned { get => newDateRetainerSigned; set => newDateRetainerSigned = value; }
        public DateTime NewDateFiled { get => newDateFiled; set => newDateFiled = value; }
        public int NewStateCircuit { get => newStateCircuit; set => newStateCircuit = value; }
        public int NewCourtVenue { get => newCourtVenue; set => newCourtVenue = value; }
        public string NewDocketNumber { get => newDocketNumber; set => newDocketNumber = value; }
        public string NewJudge { get => newJudge; set => newJudge = value; }
        public int NewCaseStage { get => newCaseStage; set => newCaseStage = value; }
        public int NewInsuranceCarrier { get => newInsuranceCarrier; set => newInsuranceCarrier = value; }
        public double NewInsuranceCoverageDollars { get => newInsuranceCoverageDollars; set => newInsuranceCoverageDollars = value; }
        public int NewExcessCarrier { get => newExcessCarrier; set => newExcessCarrier = value; }
        public double NewExcessCoverageDollars { get => newExcessCoverageDollars; set => newExcessCoverageDollars = value; }
        public double NewGrossSettlementDollars { get => newGrossSettlementDollars; set => newGrossSettlementDollars = value; }
        public double NewAttorneysFeePercent { get => newAttorneysFeePercent; set => newAttorneysFeePercent = value; }
        public double NewAttorneysFeeDollars { get => newAttorneysFeeDollars; set => newAttorneysFeeDollars = value; }
        public double NewGrossCoAndReferringCouncelFeePercentage { get => newGrossCoAndReferringCouncelFeePercentage; set => newGrossCoAndReferringCouncelFeePercentage = value; }
        public double NewGrossCoAndReferringCouncelFeeDollars { get => newGrossCoAndReferringCouncelFeeDollars; set => newGrossCoAndReferringCouncelFeeDollars = value; }
        public double NewFirmsNetFeesPercent { get => newFirmsNetFeesPercent; set => newFirmsNetFeesPercent = value; }
        public double NewFirmsNetFeesDollars { get => newFirmsNetFeesDollars; set => newFirmsNetFeesDollars = value; }
        public int NewQtrSettlement { get => newQtrSettlement; set => newQtrSettlement = value; }
        public int NewYearSettlement { get => newYearSettlement; set => newYearSettlement = value; }
        public string NewNotes { get => newNotes; set => newNotes = value; }
        public int NewCaseStatus { get => newCaseStatus; set => newCaseStatus = value; }
        public double NewActualGrossSettlement { get => newActualGrossSettlement; set => newActualGrossSettlement = value; }
        public double NewActualNetLegalFees { get => newActualNetLegalFees; set => newActualNetLegalFees = value; }
        public int NewResolutionType { get => newResolutionType; set => newResolutionType = value; }
        public int NewReasonForUpdateSelection { get => newReasonForUpdateSelection; set => newReasonForUpdateSelection = value; }
        public string NewReasonForUpdateOther { get => newReasonForUpdateOther; set => newReasonForUpdateOther = value; }
        public string NewUserID { get => newUserID; set => newUserID = value; }
        public bool NewIsTracked { get => newIsTracked; set => newIsTracked = value; }
        public DateTime NewEffectiveDate { get => newEffectiveDate; set => newEffectiveDate = value; }
       
        public DateTime CaseCreatedDate { get => caseCreatedDate; set => caseCreatedDate = value; }
        public DateTime CaseModifiedDate { get => caseModifiedDate; set => caseModifiedDate = value; }
        public int NewNumberOfCases { get => newNumberOfCases; set => newNumberOfCases = value; }
        public DateTime? PreviousResolvedDate { get => previousResolvedDate; set => previousResolvedDate = value; }
        public DateTime? NewResolvedDate { get => newResolvedDate; set => newResolvedDate = value; }
      
        public int NewCFQuaterSettlement { get => newCFQuaterSettlement; set => newCFQuaterSettlement = value; }
        public int NewCFYearSettlement { get => newCFYearSettlement; set => newCFYearSettlement = value; }
       
        public int PreviousCFQuaterSettlement { get => previousCFQuaterSettlement; set => previousCFQuaterSettlement = value; }
        public int PreviousCFYearSettlement { get => previousCFYearSettlement; set => previousCFYearSettlement = value; }
        public double NewCFGrossSettlementDollars { get => newCFGrossSettlementDollars; set => newCFGrossSettlementDollars = value; }
        public double NewCFFirmNetFeesDollars { get => newCFFirmNetFeesDollars; set => newCFFirmNetFeesDollars = value; }
        public double PreviousCFGrossSettlementDollars { get => previousCFGrossSettlementDollars; set => previousCFGrossSettlementDollars = value; }
        public double PreviousCFFirmNetFeesDollars { get => previousCFFirmNetFeesDollars; set => previousCFFirmNetFeesDollars = value; }
        public double PreviousCaseCost { get => previousCaseCost; set => previousCaseCost = value; }
        public double NewCaseCost { get => newCaseCost; set => newCaseCost = value; }
        public double PreviousCFAttorneyFeeDollar { get => previousCFAttorneyFeeDollar; set => previousCFAttorneyFeeDollar = value; }
        public double NewCFAttorneyFeeDollar { get => newCFAttorneyFeeDollar; set => newCFAttorneyFeeDollar = value; }
        public string PreviousJurisdiction { get => previousJurisdiction; set => previousJurisdiction = value; }
        public string NewJurisdiction { get => newJurisdiction; set => newJurisdiction = value; }
        public bool PreviousIsAudited { get => previousIsAudited; set => previousIsAudited = value; }
        public bool NewIsAudited { get => newIsAudited; set => newIsAudited = value; }
        public DateTime TerminationDate { get => terminationDate; set => terminationDate = value; }
        public bool PreviousIsDirectPay { get => previousIsDirectPay; set => previousIsDirectPay = value; }
        public bool NewIsDirectPay { get => newIsDirectPay; set => newIsDirectPay = value; }
        public string PreviousCoCounsel { get => previousCoCounsel; set => previousCoCounsel = value; }
        public string NewCoCounsel { get => newCoCounsel; set => newCoCounsel = value; }








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

    }
}
