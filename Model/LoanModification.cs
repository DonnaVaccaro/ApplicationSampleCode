using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class LoanModification
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoanModification));
        private int loanModificationID;
        private int loanID;
        private DateTime modificationDate;
        private int documentType;
        private bool isNewLoan;
        private bool isPaymentTermChange;
        //reserve change
        private bool isReserveChange;
        //Renewal change
        private bool isRenewal;
        //Check for increase
        private bool isIncease;
        //Check for decrease
        private bool isDecrease;
        //Assignment change
        private bool isAssignmentChange;
        //interest rate change
        private bool isInterestRateChange;
        //Max change
        private bool isMaxChange;
        //maturity change
        private bool isMaturityChange;
        //amortiz date change
        private bool isAmortizationDateChange;
        //Amortization Length change
        private bool isAmortizationLengthChange;
        //guarantor change
        private bool isGuarantorChange;
        //borrower change
        private bool isBorrowerChange;
        //Other Change
        private bool isOtherChange;
        private bool isCorrection;
        private string notes;
        private string userID;
        //Capture maturity date change
        private DateTime previousMaturityDate;
        private DateTime newMaturityDate;

        //Capture Amorit length change
        private int previousAmortizationLength;
        private int newAmortizationLength;

        //interest reserve change
        private double previousInterestReserve;
        private double newInterestReserve;

        private double previousTier1Max;
        private double previousTier1Rate;
        private double previousTier1Floor;
        private double previousTier1Ceiling;
        private double previousTier2Max;
        private double previousTier2Rate;
        private double previousTier2Floor;
        private double previousTier2Ceiling;
        private double previousTier3Max;
        private double previousTier3Rate;
        private double previousTier3Floor;
        private double previousTier3Ceiling;
        private double previousTier1DeferredRate;
        private double previousTier1DeferredFloor;
        private double previousTier1DeferredCeiling;
        private double previousTier2DeferredRate;
        private double previousTier2DeferredFloor;
        private double previousTier2DeferredCeiling;
        private double previousTier3DeferredRate;
        private double previousTier3DeferredFloor;
        private double previousTier3DeferredCeiling;


        private double newTier1Max;
        private double newTier1Rate;
        private double newTier1Floor;
        private double newTier1Ceiling;
        private double newTier2Max;
        private double newTier2Rate;
        private double newTier2Floor;
        private double newTier2Ceiling;
        private double newTier3Max;
        private double newTier3Rate;
        private double newTier3Floor;
        private double newTier3Ceiling;
        private double newTier1DeferredRate;
        private double newTier1DeferredFloor;
        private double newTier1DeferredCeiling;
        private double newTier2DeferredRate;
        private double newTier2DeferredFloor;
        private double newTier2DeferredCeiling;
        private double newTier3DeferredRate;
        private double newTier3DeferredFloor;
        private double newTier3DeferredCeiling;

        //Capture amort start date
        private DateTime previousAmortStateDate;
        private DateTime newAmortStateDate;
        //capturing company change
        private int previousCompany;
        private int newCompany;


        private string previousNickname;
        private string newNickname;

        private string previousPaymentTerms;
        private string newPaymentTerms;


        //Need to finish
        private double newLoanAmount;
        //Capture Renewal amount
        private double renewalAmount;
        //capture Increase or decrease amount
        private double increaseDecreaseAmount;
        private string mandatoryRepayments;
        //Capture interest reserve change
        private double previousInterestReserveMax;
        private double newInterestReserveMax;
        //capture assignment amount
        private double assignmentAmount;
        private bool paymentTermChange;
        private bool isClosed;
        private string reasonForUpdate;


        private DateTime createdDate;
        private DateTime modifiedDate;

        private int newStatus;
        private double newRestrictedAmount;

        private int previousStatus;
        private double previousRestrictedAmount;

        private DateTime previousOriginalLoanDate;
        private DateTime newOriginalLoanDate;

        private DateTime previousOriginationDate;
        private DateTime newOriginationDate;



        public LoanModification()
        {
            log.Info("loan modification constructor");
        }

        public int LoanModificationID { get => loanModificationID; set => loanModificationID = value; }
        public int LoanID { get => loanID; set => loanID = value; }
        public DateTime ModificationDate { get => modificationDate; set => modificationDate = value; }
        public int DocumentType { get => documentType; set => documentType = value; }
        public bool IsNewLoan { get => isNewLoan; set => isNewLoan = value; }
        public bool IsPaymentTermChange { get => isPaymentTermChange; set => isPaymentTermChange = value; }
        public bool IsReserveChange { get => isReserveChange; set => isReserveChange = value; }
        public bool IsRenewal { get => isRenewal; set => isRenewal = value; }
        public bool IsIncease { get => isIncease; set => isIncease = value; }
        public bool IsDecrease { get => isDecrease; set => isDecrease = value; }
        public bool IsAssignmentChange { get => isAssignmentChange; set => isAssignmentChange = value; }
        public bool IsInterestRateChange { get => isInterestRateChange; set => isInterestRateChange = value; }
        public bool IsMaxChange { get => isMaxChange; set => isMaxChange = value; }
        public bool IsMaturityChange { get => isMaturityChange; set => isMaturityChange = value; }
        public bool IsAmortizationDateChange { get => isAmortizationDateChange; set => isAmortizationDateChange = value; }
        public bool IsAmortizationLengthChange { get => isAmortizationLengthChange; set => isAmortizationLengthChange = value; }
        public bool IsGuarantorChange { get => isGuarantorChange; set => isGuarantorChange = value; }
        public bool IsBorrowerChange { get => isBorrowerChange; set => isBorrowerChange = value; }
        public bool IsOtherChange { get => isOtherChange; set => isOtherChange = value; }
        public bool IsCorrection { get => isCorrection; set => isCorrection = value; }
        public string Notes { get => notes; set => notes = value; }
        public string UserID { get => userID; set => userID = value; }
        public DateTime PreviousMaturityDate { get => previousMaturityDate; set => previousMaturityDate = value; }
        public DateTime NewMaturityDate { get => newMaturityDate; set => newMaturityDate = value; }
        public int PreviousAmortizationLength { get => previousAmortizationLength; set => previousAmortizationLength = value; }
        public int NewAmortizationLength { get => newAmortizationLength; set => newAmortizationLength = value; }
        public double PreviousInterestReserve { get => previousInterestReserve; set => previousInterestReserve = value; }
        public double NewInterestReserve { get => newInterestReserve; set => newInterestReserve = value; }
        public double PreviousTier1Max { get => previousTier1Max; set => previousTier1Max = value; }
        public double PreviousTier1Rate { get => previousTier1Rate; set => previousTier1Rate = value; }
        public double PreviousTier1Floor { get => previousTier1Floor; set => previousTier1Floor = value; }
        public double PreviousTier1Ceiling { get => previousTier1Ceiling; set => previousTier1Ceiling = value; }
        public double PreviousTier2Max { get => previousTier2Max; set => previousTier2Max = value; }
        public double PreviousTier2Rate { get => previousTier2Rate; set => previousTier2Rate = value; }
        public double PreviousTier2Floor { get => previousTier2Floor; set => previousTier2Floor = value; }
        public double PreviousTier2Ceiling { get => previousTier2Ceiling; set => previousTier2Ceiling = value; }
        public double PreviousTier3Max { get => previousTier3Max; set => previousTier3Max = value; }
        public double PreviousTier3Rate { get => previousTier3Rate; set => previousTier3Rate = value; }
        public double PreviousTier3Floor { get => previousTier3Floor; set => previousTier3Floor = value; }
        public double PreviousTier3Ceiling { get => previousTier3Ceiling; set => previousTier3Ceiling = value; }
        public double PreviousTier1DeferredRate { get => previousTier1DeferredRate; set => previousTier1DeferredRate = value; }
        public double PreviousTier1DeferredFloor { get => previousTier1DeferredFloor; set => previousTier1DeferredFloor = value; }
        public double PreviousTier1DeferredCeiling { get => previousTier1DeferredCeiling; set => previousTier1DeferredCeiling = value; }
        public double PreviousTier2DeferredRate { get => previousTier2DeferredRate; set => previousTier2DeferredRate = value; }
        public double PreviousTier2DeferredFloor { get => previousTier2DeferredFloor; set => previousTier2DeferredFloor = value; }
        public double PreviousTier2DeferredCeiling { get => previousTier2DeferredCeiling; set => previousTier2DeferredCeiling = value; }
        public double PreviousTier3DeferredRate { get => previousTier3DeferredRate; set => previousTier3DeferredRate = value; }
        public double PreviousTier3DeferredFloor { get => previousTier3DeferredFloor; set => previousTier3DeferredFloor = value; }
        public double PreviousTier3DeferredCeiling { get => previousTier3DeferredCeiling; set => previousTier3DeferredCeiling = value; }
        public double NewTier1Max { get => newTier1Max; set => newTier1Max = value; }
        public double NewTier1Rate { get => newTier1Rate; set => newTier1Rate = value; }
        public double NewTier1Floor { get => newTier1Floor; set => newTier1Floor = value; }
        public double NewTier1Ceiling { get => newTier1Ceiling; set => newTier1Ceiling = value; }
        public double NewTier2Max { get => newTier2Max; set => newTier2Max = value; }
        public double NewTier2Rate { get => newTier2Rate; set => newTier2Rate = value; }
        public double NewTier2Floor { get => newTier2Floor; set => newTier2Floor = value; }
        public double NewTier2Ceiling { get => newTier2Ceiling; set => newTier2Ceiling = value; }
        public double NewTier3Max { get => newTier3Max; set => newTier3Max = value; }
        public double NewTier3Rate { get => newTier3Rate; set => newTier3Rate = value; }
        public double NewTier3Floor { get => newTier3Floor; set => newTier3Floor = value; }
        public double NewTier3Ceiling { get => newTier3Ceiling; set => newTier3Ceiling = value; }
        public double NewTier1DeferredRate { get => newTier1DeferredRate; set => newTier1DeferredRate = value; }
        public double NewTier1DeferredFloor { get => newTier1DeferredFloor; set => newTier1DeferredFloor = value; }
        public double NewTier1DeferredCeiling { get => newTier1DeferredCeiling; set => newTier1DeferredCeiling = value; }
        public double NewTier2DeferredRate { get => newTier2DeferredRate; set => newTier2DeferredRate = value; }
        public double NewTier2DeferredFloor { get => newTier2DeferredFloor; set => newTier2DeferredFloor = value; }
        public double NewTier2DeferredCeiling { get => newTier2DeferredCeiling; set => newTier2DeferredCeiling = value; }
        public double NewTier3DeferredRate { get => newTier3DeferredRate; set => newTier3DeferredRate = value; }
        public double NewTier3DeferredFloor { get => newTier3DeferredFloor; set => newTier3DeferredFloor = value; }
        public double NewTier3DeferredCeiling { get => newTier3DeferredCeiling; set => newTier3DeferredCeiling = value; }
        public DateTime PreviousAmortStateDate { get => previousAmortStateDate; set => previousAmortStateDate = value; }
        public DateTime NewAmortStateDate { get => newAmortStateDate; set => newAmortStateDate = value; }
        public int PreviousCompany { get => previousCompany; set => previousCompany = value; }
        public int NewCompany { get => newCompany; set => newCompany = value; }
        public string PreviousNickname { get => previousNickname; set => previousNickname = value; }
        public string NewNickname { get => newNickname; set => newNickname = value; }
        public string PreviousPaymentTerms { get => previousPaymentTerms; set => previousPaymentTerms = value; }
        public string NewPaymentTerms { get => newPaymentTerms; set => newPaymentTerms = value; }
        public double NewLoanAmount { get => newLoanAmount; set => newLoanAmount = value; }
        public double RenewalAmount { get => renewalAmount; set => renewalAmount = value; }
        public double IncreaseDecreaseAmount { get => increaseDecreaseAmount; set => increaseDecreaseAmount = value; }
        public string MandatoryRepayments { get => mandatoryRepayments; set => mandatoryRepayments = value; }
        public double PreviousInterestReserveMax { get => previousInterestReserveMax; set => previousInterestReserveMax = value; }
        public double NewInterestReserveMax { get => newInterestReserveMax; set => newInterestReserveMax = value; }
        public double AssignmentAmount { get => assignmentAmount; set => assignmentAmount = value; }
        public bool PaymentTermChange { get => paymentTermChange; set => paymentTermChange = value; }
        public bool IsClosed { get => isClosed; set => isClosed = value; }
        public string ReasonForUpdate { get => reasonForUpdate; set => reasonForUpdate = value; }
        public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
        public DateTime ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
        public int NewStatus { get => newStatus; set => newStatus = value; }
        public double NewRestrictedAmount { get => newRestrictedAmount; set => newRestrictedAmount = value; }
        public int PreviousStatus { get => previousStatus; set => previousStatus = value; }
       
        public double PreviousRestrictedAmount { get => previousRestrictedAmount; set => previousRestrictedAmount = value; }
        public DateTime PreviousOriginalLoanDate { get => previousOriginalLoanDate; set => previousOriginalLoanDate = value; }
        public DateTime NewOriginalLoanDate { get => newOriginalLoanDate; set => newOriginalLoanDate = value; }
        public DateTime PreviousOriginationDate { get => previousOriginationDate; set => previousOriginationDate = value; }
        public DateTime NewOriginationDate { get => newOriginationDate; set => newOriginationDate = value; }





        //Storing rate field into database floor when saving to database
        //Per Nick: Database Floor = User input Rate
        public double ConvertInputRateFieldToDatabaseFloor(string inputRateField)
        {
            log.Info("ConvertInputRateFieldToDatabaseFloor");
            if (inputRateField == null)
            {
                return 0;
            }
            double temp;
            double.TryParse(inputRateField, out temp);
            double result = temp / 100;
            result = Math.Round(result, 7);
            return result;
        }

        //Storing database floor value into UI rate field to display for users
        //Per Nick: Database Floor = User input Rate
        public double ConvertDatabaseFloorToOutputRateField(string databaseFloor)
        {
            log.Info("ConvertDatabaseFloorToOutputRateField");
            if (databaseFloor == null)
            {
                return 0;
            }
            double temp;
            double.TryParse(databaseFloor, out temp);
            double result = temp * 100;
            result = Math.Round(result, 7);
            return result;
        }

        //Converting input rate and LIBOR floor into the floor database field
        public double ConvertInputRateFieldAndLIBORFloorFieldToDatabaseRate(string inputRateField, string inputLIBORFloorField)
        {
            log.Info("ConvertInputRateFieldAndLIBORFloorFieldToDatabaseRate");
            if (inputRateField == null)
            {
                return 0;
            }
            if (inputLIBORFloorField == null)
            {
                return 0;
            }
            double rate;
            double floor;

            double.TryParse(inputRateField, out rate);
            double.TryParse(inputLIBORFloorField, out floor);
            double result = rate + floor;
            result = result / 100;
            result = Math.Round(result, 7);
            return result;
        }

        //Convert rate database value into LIBOR floor UI value
        public double ConvertDatabaseRateToInputRateFieldAndLIBORFloorField(string databaseRateValue, string databaseLIBORFloorValue)
        {
            log.Info("ConvertDatabaseRateToInputRateFieldAndLIBORFloorField");
            if (databaseRateValue == null)
            {
                return 0;
            }
            if (databaseLIBORFloorValue == null)
            {
                return 0;
            }
            double rate;
            double floor;

            double.TryParse(databaseRateValue, out rate);
            double.TryParse(databaseLIBORFloorValue, out floor);
            double result = rate - floor;
            result = result * 100;
            result = Math.Round(result, 7);
            return result;
        }


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
        public double convertDecimalToPercentage(string decimalInput)
        {
            log.Info("convertDecimalToPercentage");
            if (decimalInput == null)
            {
                return 0;
            }
            double temp;
            double.TryParse(decimalInput, out temp);
            double result = temp * 100;
            result = Math.Round(result, 7);
            return result;

        }

        //Rounds to 4 decimal place
        //DMV: Not sure where to use this yet
        public double roundToFourDecimals(string decimalInput)
        {
            log.Info("roundToFourDecimals");
            if (decimalInput == null)
            {
                return 0;
            }
            double temp;
            double.TryParse(decimalInput, out temp);
            double result = Math.Round(temp, 7);
            return result;

        }
    }


}
