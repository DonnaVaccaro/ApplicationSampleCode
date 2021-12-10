using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class Loan
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Loan));
        private string loanID;
        private string borrowerName;
        private string nickname;
        private string borrowerID;
        private string company;
        private string maturityDate;
        private string originalLoanDate;
        private string amortStartDate;
        private string active;
        private string termsEffectiveDate;
        private string isHybrid;
        private string tier1Max;
        private string tier1Rate;
        private string tier1Floor;
        private string tier1Ceiling;
        private string tier2Max;
        private string tier2Rate;
        private string tier2Floor;
        private string tier2Ceiling;
        private string tier3Max;
        private string tier3Rate;
        private string tier3Floor;
        private string tier3Ceiling;
        private string tier1DeferredRate;
        private string tier1DeferredFloor;
        private string tier1DeferredCeiling;
        private string tier2DeferredRate;
        private string tier2DeferredFloor;
        private string tier2DeferredCeiling;
        private string tier3DeferredRate;
        private string tier3DeferredFloor;
        private string tier3DeferredCeiling;
        private string isVariable;
        private string amortizationLength;
        private string interestReserve;
        private string interestReserveMax;
        private string payoffDate;
        private string excludeFromReports;
        private string mailOnly;
        private string doNotMail;
        private string notes;
        private string reasonForUpdate;
        private string userID;
        private string documentType;
        private string createdDate;
        private string modifiedDate;
        private string status;
        private string restrictedAmount;
        private string originationDate;
        private string isNewLoan;
        private string isClosed;



        public Loan()
        {
            log.Info("loan constructor");
            string date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            string defaultDate = new DateTime(1900, 01, 01).ToString();
            this.loanID = "";
            this.borrowerName = "";
            this.nickname = " ";
            this.borrowerID = "";
            this.company = "CFII";
           
            this.active = "false";
          
            this.isHybrid = "0";
            this.tier1Max = "0.0";
            this.tier1Rate = "0.0";
            this.tier1Floor = "0.0";
            this.tier1Ceiling = "0.0";
            this.tier2Max = "0.0";
            this.tier2Rate = "0.0";
            this.tier2Floor = "0.0";
            this.tier2Ceiling = "0.0";
            this.tier3Max = "0.0";
            this.tier3Rate = "0.0";
            this.tier3Floor = "0.0";
            this.tier3Ceiling = "0.0";
            this.tier1DeferredRate = "0.0";
            this.tier1DeferredFloor = "0.0";
            this.tier1DeferredCeiling = "0.0";
            this.tier2DeferredRate = "0.0";
            this.tier2DeferredFloor = "0.0";
            this.tier2DeferredCeiling = "0.0";
            this.tier3DeferredRate = "0.0";
            this.tier3DeferredFloor = "0.0";
            this.tier3DeferredCeiling = "0.0";
            this.isVariable = "true";
            this.amortizationLength = "24";
            this.interestReserve = "60";
            this.interestReserveMax = "0.0";
            this.payoffDate = "";
            this.excludeFromReports = "true";
            this.mailOnly = "false";
            this.doNotMail = "false";
            this.notes = "";
            this.reasonForUpdate = "Added a new Loan";
            this.createdDate = date;
            this.modifiedDate = date;
            this.originalLoanDate = defaultDate;
            this.originationDate = defaultDate;
            this.userID = Environment.UserName;
            this.IsNewLoan = "false";



        }


        public string BorrowerName { get => borrowerName; set => borrowerName = value; }
        public string LoanID { get => loanID; set => loanID = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string BorrowerID { get => borrowerID; set => borrowerID = value; }
        public string Company { get => company; set => company = value; }
        public string MaturityDate { get => maturityDate; set => maturityDate = value; }
        public string OriginalLoanDate { get => originalLoanDate; set => originalLoanDate = value; }
        public string AmortStartDate { get => amortStartDate; set => amortStartDate = value; }
        public string Active { get => active; set => active = value; }
        public string TermsEffectiveDate { get => termsEffectiveDate; set => termsEffectiveDate = value; }
        public string IsHybrid { get => isHybrid; set => isHybrid = value; }
        public string Tier1Max { get => tier1Max; set => tier1Max = value; }
        public string Tier1Rate { get => tier1Rate; set => tier1Rate = value; }
        public string Tier1Floor { get => tier1Floor; set => tier1Floor = value; }
        public string Tier1Ceiling { get => tier1Ceiling; set => tier1Ceiling = value; }
        public string Tier2Max { get => tier2Max; set => tier2Max = value; }
        public string Tier2Rate { get => tier2Rate; set => tier2Rate = value; }
        public string Tier2Floor { get => tier2Floor; set => tier2Floor = value; }
        public string Tier2Ceiling { get => tier2Ceiling; set => tier2Ceiling = value; }
        public string Tier3Max { get => tier3Max; set => tier3Max = value; }
        public string Tier3Rate { get => tier3Rate; set => tier3Rate = value; }
        public string Tier3Floor { get => tier3Floor; set => tier3Floor = value; }
        public string Tier3Ceiling { get => tier3Ceiling; set => tier3Ceiling = value; }
        public string Tier1DeferredRate { get => tier1DeferredRate; set => tier1DeferredRate = value; }
        public string Tier1DeferredFloor { get => tier1DeferredFloor; set => tier1DeferredFloor = value; }
        public string Tier1DeferredCeiling { get => tier1DeferredCeiling; set => tier1DeferredCeiling = value; }
        public string Tier2DeferredRate { get => tier2DeferredRate; set => tier2DeferredRate = value; }
        public string Tier2DeferredFloor { get => tier2DeferredFloor; set => tier2DeferredFloor = value; }
        public string Tier2DeferredCeiling { get => tier2DeferredCeiling; set => tier2DeferredCeiling = value; }
        public string Tier3DeferredRate { get => tier3DeferredRate; set => tier3DeferredRate = value; }
        public string Tier3DeferredFloor { get => tier3DeferredFloor; set => tier3DeferredFloor = value; }
        public string Tier3DeferredCeiling { get => tier3DeferredCeiling; set => tier3DeferredCeiling = value; }
        public string IsVariable { get => isVariable; set => isVariable = value; }
        public string AmortizationLength { get => amortizationLength; set => amortizationLength = value; }
        public string InterestReserve { get => interestReserve; set => interestReserve = value; }
        public string InterestReserveMax { get => interestReserveMax; set => interestReserveMax = value; }
        public string PayoffDate { get => payoffDate; set => payoffDate = value; }
        public string ExcludeFromReports { get => excludeFromReports; set => excludeFromReports = value; }
        public string MailOnly { get => mailOnly; set => mailOnly = value; }
        public string DoNotMail { get => doNotMail; set => doNotMail = value; }
        public string Notes { get => notes; set => notes = value; }
        public string ReasonForUpdate { get => reasonForUpdate; set => reasonForUpdate = value; }
        public string UserID { get => userID; set => userID = value; }
        public string DocumentType { get => documentType; set => documentType = value; }
        public string CreatedDate { get => createdDate; set => createdDate = value; }
        public string ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
        public string Status { get => status; set => status = value; }
        public string RestrictedAmount { get => restrictedAmount; set => restrictedAmount = value; }
        public string OriginationDate { get => originationDate; set => originationDate = value; }
        public string IsNewLoan { get => isNewLoan; set => isNewLoan = value; }
        public string IsClosed { get => isClosed; set => isClosed = value; }


        //Storing rate field into database rate when saving to database
        //Per Nick: Database rate = User input Rate
        //input: UI Tier X Rate = 15%
        //output: Database Tier_X_Rate = .15
        public double ConvertInputRateFieldToDatabaseRate(string inputRateField)
        {
            log.Info("ConvertInputRateFieldToDatabaserate");
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

        //Converting input rate and LIBOR floor into the floor database field
        //Input UI Tier X Rate = 15%,  UI Tier X LIBOR floor = 1%
        //Output Database  Tier_X_Floor = .16
        public double ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(string inputRateField, string inputLIBORFloorField)
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



        //Storing database floor value into UI rate field to display for users
        //Per Nick: Database Rate = User input Rate
        public string ConvertDatabaseRateToOutputRateField(string databaseRate)
        {
            log.Info("ConvertDatabaseFloorToOutputRateField");
            if (databaseRate == null)
            {
                return "0";
            }
            double temp;
            double.TryParse(databaseRate, out temp);
            double result = temp * 100;
            result = Math.Round(result, 7);
            return result.ToString();
        }

       

        //Convert rate database value into LIBOR floor UI value
        public string ConvertDatabaseRateAndFloorToLIBORFloorField(string databaseRateValue, string databaseFloorValue)
        {
            log.Info("ConvertDatabaseRateAndFloorToLIBORFloorField");
            if (databaseRateValue == null)
            {
                return "0";
            }
            if (databaseFloorValue == null)
            {
                return "0";
            }
            double rate;
            double floor;

            double.TryParse(databaseRateValue, out rate);
            double.TryParse(databaseFloorValue, out floor);
            double result = rate - floor;
            result = result * 100;
            // result = Math.Abs(result);
            result = Math.Round(result, 7);
            return result.ToString();
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

        //Rounds to 4 decimal place
        //DMV: Not sure where to use this yet
        public string roundToFourDecimals(string decimalInput)
        {
            log.Info("roundToFourDecimals");
            if (decimalInput == null)
            {
                return "0";
            }
            double temp;
            double.TryParse(decimalInput, out temp);
            double result = Math.Round(temp, 7);
            return result.ToString();

        }
    }
}
