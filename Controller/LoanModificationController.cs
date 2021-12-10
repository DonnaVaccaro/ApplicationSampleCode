using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTS.Controller
{
    public class LoanModificationController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoanModificationController));
        private readonly SQLLoanModificationQueries sQLLoanModificationQueries;
        private  LoanModification existingLoanMod;
        private  LoanModification newLoanMod;
       

        public LoanModificationController()
        {
            log.Info("Constructor Loan Modification Controller");
            sQLLoanModificationQueries = new SQLLoanModificationQueries();
            existingLoanMod = new LoanModification();
            newLoanMod = new LoanModification();


        }



        public bool addExistingLoanModificationLoan(Loan loan, Loan updateLoan, LoanModification loanModification)
        {
            log.Info("Adding new modification");
            if (loan == null)
            {
                log.Error("Trying to add a loan modification and the loan is null");
                return false;
            }
            if (updateLoan == null)
            {
                log.Error("Trying to add a loan modification and the updateLoan is null");
                return false;
            }
            if (loanModification == null)
            {
                log.Error("Trying to add a loan modification and the loanModification is null");
                return false;
            }


            bool addNewLoanResult = true;
            int id = 0;
            string res = "1";
            int intValue = 0;
            DateTime dateTimeValue;
            double doubleValue = 0.0;


            int.TryParse(loan.LoanID, out id);
            loanModification.LoanID = id;
            //Doing a lookup for document type
            if (ComboBoxPopulationController.docuemntTypeList.ContainsValue(updateLoan.DocumentType))
            {

                res = ComboBoxPopulationController.docuemntTypeList.FirstOrDefault(x => x.Value == updateLoan.DocumentType).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultDocuemntTypeIndex;


            }
            loanModification.DocumentType = intValue;
            loanModification.Notes = updateLoan.Notes;
            loanModification.UserID = updateLoan.UserID;
            //Maturity Date
            if (!DateTime.TryParse(loan.MaturityDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.PreviousMaturityDate = dateTimeValue;

            if (!DateTime.TryParse(updateLoan.MaturityDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.NewMaturityDate = dateTimeValue;
            //amortizaton lenght
            int.TryParse(loan.AmortizationLength, out id);
            loanModification.PreviousAmortizationLength = id;

            int.TryParse(updateLoan.AmortizationLength, out id);
            loanModification.NewAmortizationLength = id;
            //Interest reserve
            double.TryParse(loan.InterestReserve, out doubleValue);
            loanModification.PreviousInterestReserve = doubleValue;

            double.TryParse(updateLoan.InterestReserve, out doubleValue);
            loanModification.NewInterestReserve = doubleValue;
            //tier 1 max
            double.TryParse(loan.Tier1Max, out doubleValue);
            loanModification.PreviousTier1Max = doubleValue;

            double.TryParse(updateLoan.Tier1Max, out doubleValue);
            loanModification.NewTier1Max = doubleValue;

            //tier 1 rate
            loanModification.PreviousTier1Rate = loan.convertPercentageToDecimal(loan.Tier1Rate);

            loanModification.NewTier1Rate = loan.convertPercentageToDecimal(updateLoan.Tier1Rate);

            //tier 1 floor
            loanModification.PreviousTier1Floor = loan.convertPercentageToDecimal(loan.Tier1Floor);

            loanModification.NewTier1Floor = loan.convertPercentageToDecimal(updateLoan.Tier1Floor);

            //tier 1 ceiling
            loanModification.PreviousTier1Ceiling = loan.convertPercentageToDecimal(loan.Tier1Ceiling);

            loanModification.NewTier1Ceiling = loan.convertPercentageToDecimal(updateLoan.Tier1Ceiling);


            //tier 2 max
            double.TryParse(loan.Tier2Max, out doubleValue);
            loanModification.PreviousTier2Max = doubleValue;

            double.TryParse(updateLoan.Tier2Max, out doubleValue);
            loanModification.NewTier2Max = doubleValue;

            //tier 2 rate
            loanModification.PreviousTier2Rate = loan.convertPercentageToDecimal(loan.Tier2Rate);

            loanModification.NewTier2Rate = loan.convertPercentageToDecimal(updateLoan.Tier2Rate);

            //tier 2 floor
            loanModification.PreviousTier2Floor = loan.convertPercentageToDecimal(loan.Tier2Floor);

            loanModification.NewTier2Floor = loan.convertPercentageToDecimal(updateLoan.Tier2Floor);

            //tier 2 ceiling
            loanModification.PreviousTier2Ceiling = loan.convertPercentageToDecimal(loan.Tier2Ceiling);

            loanModification.NewTier2Ceiling = loan.convertPercentageToDecimal(updateLoan.Tier2Ceiling);

            //tier 3 max
            double.TryParse(loan.Tier3Max, out doubleValue);
            loanModification.PreviousTier3Max = doubleValue;

            double.TryParse(updateLoan.Tier3Max, out doubleValue);
            loanModification.NewTier3Max = doubleValue;

            //tier 3 rate
            loanModification.PreviousTier3Rate = loan.convertPercentageToDecimal(loan.Tier3Rate);

            loanModification.NewTier3Rate = loan.convertPercentageToDecimal(updateLoan.Tier3Rate);

            //tier 3 floor
            loanModification.PreviousTier3Floor = loan.convertPercentageToDecimal(loan.Tier3Floor);

            loanModification.NewTier3Floor = loan.convertPercentageToDecimal(updateLoan.Tier3Floor);

            //tier 3 ceiling
            loanModification.PreviousTier3Ceiling = loan.convertPercentageToDecimal(loan.Tier3Ceiling);

            loanModification.NewTier3Ceiling = loan.convertPercentageToDecimal(updateLoan.Tier3Ceiling);

            //tier 1 rate deferred
            loanModification.PreviousTier1DeferredRate = loan.convertPercentageToDecimal(loan.Tier1DeferredRate);

            loanModification.NewTier1DeferredRate = loan.convertPercentageToDecimal(updateLoan.Tier1DeferredRate);

            //tier 1 Floor deferred
            loanModification.PreviousTier1DeferredFloor = loan.convertPercentageToDecimal(loan.Tier1DeferredFloor);

            loanModification.NewTier1DeferredFloor = loan.convertPercentageToDecimal(updateLoan.Tier1DeferredFloor);

            //tier 1 ceiling deferred
            loanModification.PreviousTier1DeferredCeiling = loan.convertPercentageToDecimal(loan.Tier1DeferredCeiling);

            loanModification.NewTier1DeferredCeiling = loan.convertPercentageToDecimal(updateLoan.Tier1DeferredCeiling);

            //tier 2 rate deferred
            loanModification.PreviousTier2DeferredRate = loan.convertPercentageToDecimal(loan.Tier2DeferredRate);

            loanModification.NewTier2DeferredRate = loan.convertPercentageToDecimal(updateLoan.Tier2DeferredRate);

            //tier 2 Floor deferred
            loanModification.PreviousTier2DeferredFloor = loan.convertPercentageToDecimal(loan.Tier2DeferredFloor);

            loanModification.NewTier2DeferredFloor = loan.convertPercentageToDecimal(updateLoan.Tier2DeferredFloor);

            //tier 2 ceiling deferred
            loanModification.PreviousTier2DeferredCeiling = loan.convertPercentageToDecimal(loan.Tier2DeferredCeiling);

            loanModification.NewTier2DeferredCeiling = loan.convertPercentageToDecimal(updateLoan.Tier2DeferredCeiling);

            //tier 3 rate deferred
            loanModification.PreviousTier3DeferredRate = loan.convertPercentageToDecimal(loan.Tier3DeferredRate);

            loanModification.NewTier3DeferredRate = loan.convertPercentageToDecimal(updateLoan.Tier3DeferredRate);

            //tier 3 Floor deferred
            loanModification.PreviousTier3DeferredFloor = loan.convertPercentageToDecimal(loan.Tier3DeferredFloor);

            loanModification.NewTier3DeferredFloor = loan.convertPercentageToDecimal( updateLoan.Tier3DeferredFloor);

            //tier 3 ceiling deferred
            loanModification.PreviousTier3DeferredCeiling = loan.convertPercentageToDecimal(loan.Tier3DeferredCeiling);

            loanModification.NewTier3DeferredCeiling = loan.convertPercentageToDecimal(updateLoan.Tier3DeferredCeiling);


            //amort start Date
            if (!DateTime.TryParse(loan.AmortStartDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.PreviousAmortStateDate = dateTimeValue;

            if (!DateTime.TryParse(updateLoan.AmortStartDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.NewAmortStateDate = dateTimeValue;

            double.TryParse(loan.RestrictedAmount, out doubleValue);
            loanModification.PreviousRestrictedAmount = doubleValue;
            double.TryParse(updateLoan.RestrictedAmount, out doubleValue);
            loanModification.NewRestrictedAmount = doubleValue;

            intValue = 1;
            //looking up company id from list
            if (ComboBoxPopulationController.companyList.ContainsValue(loan.Company))
            {

                res = ComboBoxPopulationController.companyList.FirstOrDefault(x => x.Value == loan.Company).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCompanyIndex;
            }
            loanModification.PreviousCompany = intValue;

            intValue = 1;
            //looking up status id from list
            if (ComboBoxPopulationController.statusList.ContainsValue(loan.Status))
            {

                res = ComboBoxPopulationController.statusList.FirstOrDefault(x => x.Value == loan.Status).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultStatusIndex;
            }
            loanModification.PreviousStatus = intValue;


            intValue = 1;
            //looking up company id from list
            if (ComboBoxPopulationController.companyList.ContainsValue(updateLoan.Company))
            {

                res = ComboBoxPopulationController.companyList.FirstOrDefault(x => x.Value == updateLoan.Company).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCompanyIndex;
            }
            loanModification.NewCompany = intValue;

            intValue = 1;
            //looking up status id from list
            if (ComboBoxPopulationController.statusList.ContainsValue(updateLoan.Status))
            {

                res = ComboBoxPopulationController.statusList.FirstOrDefault(x => x.Value == updateLoan.Status).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultStatusIndex;
            }
            loanModification.NewStatus = intValue;


            loanModification.PreviousNickname = loan.Nickname;
            loanModification.NewNickname = updateLoan.Nickname;

            loanModification.PreviousPaymentTerms = " ";
            loanModification.NewPaymentTerms = " ";

            loanModification.RenewalAmount = 0.0;
            loanModification.NewLoanAmount = 0.0;
            loanModification.IncreaseDecreaseAmount = this.getNewLimits(updateLoan);
            //Not sure what this is
            loanModification.MandatoryRepayments = " ";

            double.TryParse(loan.InterestReserveMax, out doubleValue);
            loanModification.PreviousInterestReserveMax = doubleValue;

            double.TryParse(updateLoan.InterestReserveMax, out doubleValue);
            loanModification.NewInterestReserveMax = doubleValue;


            loanModification.AssignmentAmount = 0.0;

            loanModification.PaymentTermChange = false;
            //DMV
            //loanModification.IsClosed = updateLoan.IsClosed;

            loanModification.ReasonForUpdate = updateLoan.ReasonForUpdate;


            //Loan created Date
            if (!DateTime.TryParse(loan.CreatedDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.CreatedDate = dateTimeValue;

            //Loan modified Date
            if (!DateTime.TryParse(loan.ModifiedDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.ModifiedDate = dateTimeValue;



            sQLLoanModificationQueries.addNewModificationLoan(loanModification);
            existingLoanMod = loanModification;
            return addNewLoanResult;
        }




        public bool addNewModificationLoan(Loan loan, Loan updateLoan, LoanModification loanModification)
        {
            log.Info("Adding new modification");
            if (loan == null)
            {
                log.Error("Trying to add a loan modification and the loan is null");
                return false;
            }
            if (updateLoan == null)
            {
                log.Error("Trying to add a loan modification and the updateLoan is null");
                return false;
            }
            if (loanModification == null)
            {
                log.Error("Trying to add a loan modification and the loanModification is null");
                return false;
            }


            bool addNewLoanResult = true;
            int id = 0;
            string res = "1";
            int intValue = 0;
            DateTime dateTimeValue;
            double doubleValue = 0.0;


            int.TryParse(loan.LoanID, out id);
            loanModification.LoanID = id;
            //Doing a lookup for document type
            if (ComboBoxPopulationController.docuemntTypeList.ContainsValue(updateLoan.DocumentType))
            {

                res = ComboBoxPopulationController.docuemntTypeList.FirstOrDefault(x => x.Value == updateLoan.DocumentType).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultDocuemntTypeIndex;


            }
            loanModification.DocumentType = intValue;
            loanModification.Notes = updateLoan.Notes;
            loanModification.UserID = updateLoan.UserID;

            bool boolValue;
            bool.TryParse(updateLoan.IsNewLoan, out boolValue);
            loanModification.IsNewLoan = boolValue;

            //Maturity Date
            if (!DateTime.TryParse(loan.MaturityDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.PreviousMaturityDate = dateTimeValue;

            if (!DateTime.TryParse(updateLoan.MaturityDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.NewMaturityDate = dateTimeValue;
            //amortizaton lenght
            int.TryParse(loan.AmortizationLength, out id);
            loanModification.PreviousAmortizationLength = id;

            int.TryParse(updateLoan.AmortizationLength, out id);
            loanModification.NewAmortizationLength = id;
            //Interest reserve
            double.TryParse(loan.InterestReserve, out doubleValue);
            loanModification.PreviousInterestReserve = doubleValue;

            double.TryParse(updateLoan.InterestReserve, out doubleValue);
            loanModification.NewInterestReserve = doubleValue;
            //tier 1 max
            double.TryParse(loan.Tier1Max, out doubleValue);
            loanModification.PreviousTier1Max = doubleValue;

            double.TryParse(updateLoan.Tier1Max, out doubleValue);
            loanModification.NewTier1Max = doubleValue;

            //tier 1 rate
            loanModification.PreviousTier1Rate = loan.ConvertInputRateFieldToDatabaseRate(loan.Tier1Rate);

            loanModification.NewTier1Rate = loan.ConvertInputRateFieldToDatabaseRate(updateLoan.Tier1Rate);

            //tier 1 floor
            loanModification.PreviousTier1Floor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(loan.Tier1Rate,loan.Tier1Floor);

            loanModification.NewTier1Floor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(updateLoan.Tier1Rate,updateLoan.Tier1Floor);

            //tier 1 ceiling
            loanModification.PreviousTier1Ceiling = loanModification.convertPercentageToDecimal(loan.Tier1Ceiling);

            loanModification.NewTier1Ceiling = loanModification.convertPercentageToDecimal(updateLoan.Tier1Ceiling);


            //tier 2 max
            double.TryParse(loan.Tier2Max, out doubleValue);
            loanModification.PreviousTier2Max = doubleValue;

            double.TryParse(updateLoan.Tier2Max, out doubleValue);
            loanModification.NewTier2Max = doubleValue;

            //tier 2 rate
            loanModification.PreviousTier2Rate = loan.ConvertInputRateFieldToDatabaseRate(loan.Tier2Rate);

            loanModification.NewTier2Rate = loan.ConvertInputRateFieldToDatabaseRate(updateLoan.Tier2Rate);

            //tier 2 floor
            loanModification.PreviousTier2Floor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(loan.Tier2Rate,loan.Tier2Floor);

            loanModification.NewTier2Floor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(updateLoan.Tier2Rate,updateLoan.Tier2Floor);

            //tier 2 ceiling
            loanModification.PreviousTier2Ceiling = loanModification.convertPercentageToDecimal(loan.Tier2Ceiling);

            loanModification.NewTier2Ceiling = loanModification.convertPercentageToDecimal(updateLoan.Tier2Ceiling);

            //tier 3 max
            double.TryParse(loan.Tier3Max, out doubleValue);
            loanModification.PreviousTier3Max = doubleValue;

            double.TryParse(updateLoan.Tier3Max, out doubleValue);
            loanModification.NewTier3Max = doubleValue;

            //tier 3 rate
            loanModification.PreviousTier3Rate = loan.ConvertInputRateFieldToDatabaseRate(loan.Tier3Rate);

            loanModification.NewTier3Rate = loan.ConvertInputRateFieldToDatabaseRate(updateLoan.Tier3Rate);

            //tier 3 floor
            loanModification.PreviousTier3Floor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(loan.Tier3Rate,loan.Tier3Floor);

            loanModification.NewTier3Floor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(updateLoan.Tier3Rate,updateLoan.Tier3Floor);

            //tier 3 ceiling
            loanModification.PreviousTier3Ceiling = loanModification.convertPercentageToDecimal(loan.Tier3Ceiling);

            loanModification.NewTier3Ceiling = loanModification.convertPercentageToDecimal(updateLoan.Tier3Ceiling);

            //tier 1 rate deferred
            loanModification.PreviousTier1DeferredRate = loan.ConvertInputRateFieldToDatabaseRate(loan.Tier1DeferredRate);

            loanModification.NewTier1DeferredRate = loan.ConvertInputRateFieldToDatabaseRate(updateLoan.Tier1DeferredRate);

            //tier 1 Floor deferred
            loanModification.PreviousTier1DeferredFloor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(loan.Tier1DeferredRate,loan.Tier1DeferredFloor);

            loanModification.NewTier1DeferredFloor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(updateLoan.Tier1DeferredRate,updateLoan.Tier1DeferredFloor);

            //tier 1 ceiling deferred
            loanModification.PreviousTier1DeferredCeiling = loanModification.convertPercentageToDecimal(loan.Tier1DeferredCeiling);

            loanModification.NewTier1DeferredCeiling = loanModification.convertPercentageToDecimal(updateLoan.Tier1DeferredCeiling);

            //tier 2 rate deferred
            loanModification.PreviousTier2DeferredRate = loan.ConvertInputRateFieldToDatabaseRate(loan.Tier2DeferredRate);

            loanModification.NewTier2DeferredRate = loan.ConvertInputRateFieldToDatabaseRate(updateLoan.Tier2DeferredRate);

            //tier 2 Floor deferred
            loanModification.PreviousTier2DeferredFloor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(loan.Tier2DeferredRate,loan.Tier2DeferredFloor);

            loanModification.NewTier2DeferredFloor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(updateLoan.Tier2DeferredRate,updateLoan.Tier2DeferredFloor);

            //tier 2 ceiling deferred
            loanModification.PreviousTier2DeferredCeiling = loanModification.convertPercentageToDecimal(loan.Tier2DeferredCeiling);

            loanModification.NewTier2DeferredCeiling = loanModification.convertPercentageToDecimal(updateLoan.Tier2DeferredCeiling);

            //tier 3 rate deferred
            loanModification.PreviousTier3DeferredRate = loan.ConvertInputRateFieldToDatabaseRate(loan.Tier3DeferredRate);

            loanModification.NewTier3DeferredRate = loan.ConvertInputRateFieldToDatabaseRate(updateLoan.Tier3DeferredRate);

            //tier 3 Floor deferred
            loanModification.PreviousTier3DeferredFloor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(loan.Tier3DeferredRate,loan.Tier3DeferredFloor);

            loanModification.NewTier3DeferredFloor = loan.ConvertInputRateFieldAndLIBORFloorFieldToDatabaseFloor(updateLoan.Tier3DeferredRate, updateLoan.Tier3DeferredFloor);

            //tier 3 ceiling deferred
            loanModification.PreviousTier3DeferredCeiling = loanModification.convertPercentageToDecimal(loan.Tier3DeferredCeiling);

            loanModification.NewTier3DeferredCeiling = loanModification.convertPercentageToDecimal(updateLoan.Tier3DeferredCeiling);


            //amort start Date
            if (!DateTime.TryParse(loan.AmortStartDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.PreviousAmortStateDate = dateTimeValue;

            if (!DateTime.TryParse(updateLoan.AmortStartDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.NewAmortStateDate = dateTimeValue;

            double.TryParse(loan.RestrictedAmount, out doubleValue);
            loanModification.PreviousRestrictedAmount = doubleValue;
            double.TryParse(updateLoan.RestrictedAmount, out doubleValue);
            loanModification.NewRestrictedAmount = doubleValue;

            intValue = 1;
            //looking up company id from list
            if (ComboBoxPopulationController.companyList.ContainsValue(loan.Company))
            {

                res = ComboBoxPopulationController.companyList.FirstOrDefault(x => x.Value == loan.Company).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCompanyIndex;
            }
            loanModification.PreviousCompany = intValue;

            intValue = 1;
            //looking up status id from list
            if (ComboBoxPopulationController.statusList.ContainsValue(loan.Status))
            {

                res = ComboBoxPopulationController.statusList.FirstOrDefault(x => x.Value == loan.Status).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultStatusIndex;
            }
            loanModification.PreviousStatus = intValue;


            intValue = 1;
            //looking up company id from list
            if (ComboBoxPopulationController.companyList.ContainsValue(updateLoan.Company))
            {

                res = ComboBoxPopulationController.companyList.FirstOrDefault(x => x.Value == updateLoan.Company).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultCompanyIndex;
            }
            loanModification.NewCompany = intValue;

            intValue = 1;
            //looking up status id from list
            if (ComboBoxPopulationController.statusList.ContainsValue(updateLoan.Status))
            {

                res = ComboBoxPopulationController.statusList.FirstOrDefault(x => x.Value == updateLoan.Status).Key;
                int.TryParse(res, out intValue);

            }
            else
            {
                intValue = ComboBoxPopulationController.defaultStatusIndex;
            }
            loanModification.NewStatus = intValue;


            loanModification.PreviousNickname = loan.Nickname;
            loanModification.NewNickname = updateLoan.Nickname;

            loanModification.PreviousPaymentTerms = " ";
            loanModification.NewPaymentTerms = " ";

            loanModification.RenewalAmount = 0.0;
            loanModification.NewLoanAmount = 0.0;
            loanModification.IncreaseDecreaseAmount = this.getNewLimits(updateLoan);
            //Not sure what this is
            loanModification.MandatoryRepayments = " ";

            double.TryParse(loan.InterestReserveMax, out doubleValue);
            loanModification.PreviousInterestReserveMax = doubleValue;

            double.TryParse(updateLoan.InterestReserveMax, out doubleValue);
            loanModification.NewInterestReserveMax = doubleValue;


            loanModification.AssignmentAmount = 0.0;

            loanModification.PaymentTermChange = false;

            bool boolValue1;
            bool.TryParse(updateLoan.IsClosed, out boolValue1);
            loanModification.IsClosed = boolValue1;

            loanModification.ReasonForUpdate = updateLoan.ReasonForUpdate;


            //Loan created Date
            if (!DateTime.TryParse(loan.CreatedDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.CreatedDate = dateTimeValue;

            //Loan modified Date
            if (!DateTime.TryParse(loan.ModifiedDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.ModifiedDate = dateTimeValue;


            //previous original loan date
            if (!DateTime.TryParse(loan.OriginalLoanDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.PreviousOriginalLoanDate = dateTimeValue;


            //new original loan date
            if (!DateTime.TryParse(updateLoan.OriginalLoanDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.NewOriginalLoanDate = dateTimeValue;


            //previous Origination loan date
            if (!DateTime.TryParse(loan.OriginationDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.PreviousOriginationDate = dateTimeValue;


            //new Origination loan date
            if (!DateTime.TryParse(updateLoan.OriginationDate, out dateTimeValue))
            {
                dateTimeValue = ComboBoxPopulationController.defaultDate;
            }
            loanModification.NewOriginationDate = dateTimeValue;

            

            sQLLoanModificationQueries.addNewModificationLoan(loanModification);
            newLoanMod = loanModification;
            return addNewLoanResult;
        }

        public LoanModification getExistingLoanMod()
        {
            return existingLoanMod;
        }

        public LoanModification getNewLoanMod()
        {
            return newLoanMod;
        }



        private double getExistingLimits(Loan currentLoan)
        {
            if (currentLoan == null)
            {
                return 0;
            }

            double result = 0;
            double tier1Limit;
            double tier2Limit;
            double tier3Limit;

            Double.TryParse(currentLoan.Tier1Max, out tier1Limit);
            Double.TryParse(currentLoan.Tier2Max, out tier2Limit);
            Double.TryParse(currentLoan.Tier3Max, out tier3Limit);

            result = tier1Limit + tier2Limit + tier3Limit;

            return result;
        }

        private double getNewLimits(Loan updatedLoan)
        {

            if (updatedLoan == null)
            {
                return 0;
            }
            double result = 0;
            double tier1Limit;
            double tier2Limit;
            double tier3Limit;

            Double.TryParse(updatedLoan.Tier1Max, out tier1Limit);
            Double.TryParse(updatedLoan.Tier2Max, out tier2Limit);
            Double.TryParse(updatedLoan.Tier3Max, out tier3Limit);

            result = tier1Limit + tier2Limit + tier3Limit;

            return result;
        }
    }
}
