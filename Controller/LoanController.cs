using DocumentFormat.OpenXml.Packaging;
using OTS.DataBase;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace OTS.Controller
{
    public class LoanController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LoanController));
        private readonly SQLLoanQueries sQLLoanQueries;
        private readonly SQLBorrowerQueries sQLBorrowerQueries;
        private readonly LoanModificationController loanModificationController;
       // private AccessDatabaseLoanLPSController accessDatabaseLoanLPSController;
        private Loan currentLoan;
        private Loan updatedLoan;

        public LoanController()
        {
            log.Info("Constructor Loan Controller");
            currentLoan = new Loan();
            updatedLoan = new Loan();
            sQLLoanQueries = new SQLLoanQueries();
            sQLBorrowerQueries = new SQLBorrowerQueries();
            loanModificationController = new LoanModificationController();
            //accessDatabaseLoanLPSController = new AccessDatabaseLoanLPSController();
        }


        public string add1MaxID(string CMID)
        {
            
            //loanID = CMLoanID + 1
            int cm;
            string maxID = "";

            
            int.TryParse(CMID, out cm);
            
            maxID = (cm + 1).ToString();

            return maxID;

        }


        public string findMaxID(string LPSID, string CMID)
        {
            //If LPSLoanID >= CMLoanID then loanID = LPSLoanID + 1
            //ELSE loanID = CMLoanID + 1
            int lps;
            int cm;
            string maxID = "";

            int.TryParse(LPSID, out lps);
            int.TryParse(CMID, out cm);
            if (lps >= cm)
            {
                maxID = (lps + 1).ToString();
            }
            else
            {
                maxID = (cm + 1).ToString();
            }

            return maxID;

        }

        public bool addExistingLoan(Loan newLoan)
        {
            bool res = sQLLoanQueries.addExistingLoan(newLoan);
            return res;
        }

        public string getMaxCMLoanIndex()
        {
            string tem = sQLLoanQueries.getMaxCMLoanIndex();
            return tem;

        }

        public bool addLoanCMWithID(Loan loan)
        {
            log.Info("add default loah");
          
            bool ress = sQLLoanQueries.IdentityOn();

            ress = sQLLoanQueries.addExistingLoan(loan);

            if (!ress)
            {
                MessageBox.Show("Loan Mode failed it insert" + "loan id " + loan.LoanID);
            }

            bool resss = sQLLoanQueries.IdentityOff();
            return ress;
        }


        public bool addLoanWithID(Loan loan)
        {
            log.Info("add new Borrowers");
            bool res = true;

            bool ress = sQLLoanQueries.IdentityOn();


            LoanModification loanModification = new LoanModification();
            loanModification.ModificationDate = DateTime.Now;
            loanModification.IsNewLoan = true;
            res = sQLLoanQueries.addExistingLoan(loan);


            bool re = loanModificationController.addExistingLoanModificationLoan(loan, loan, loanModification);
            if (!re)
            {
                MessageBox.Show("Loan Mode failed it insert" + "loan id " + loan.LoanID);
            }

            bool resss = sQLLoanQueries.IdentityOff();
            return res;
        }



        public List<Loan> addAllLoan(List<Loan> list)
        {
            log.Info("add new Borrowers");
            bool res = true;
            List<Loan> failInsert = new List<Loan>();
            List<LoanModification> failInsertMod = new List<LoanModification>();
            bool ress = sQLLoanQueries.IdentityOn();
            //if (!ress)
            //{
            //    MessageBox.Show("Identity On returned false");
            //    return null;
            //}
            for (int i = 0; i < list.Count; i++)
            {
                LoanModification loanModification = new LoanModification();
                loanModification.ModificationDate = DateTime.Now;
                loanModification.IsNewLoan = false;
                res = sQLLoanQueries.addExistingLoan(list[i]);
                if (!res)
                {
                    failInsert.Add(list[i]);
                }

                bool re = loanModificationController.addExistingLoanModificationLoan(list[i], list[i], loanModification);
                if (!re)
                {
                    MessageBox.Show("Loan Mode failed it insert" + "loan id " + list[i].LoanID);
                }
            }
            bool resss = sQLLoanQueries.IdentityOff();
            return failInsert;
        }

        public List<Loan> getActiveLoans()
        {
            List<Loan> allLoansList = sQLLoanQueries.getActiveLoans();
            if (allLoansList == null)
            {
                log.Error("Quering all loans and returned null");
                return new List<Loan>();
            }
            for (int i = 0; i < allLoansList.Count; i++)
            {
                string name;
                SQLBorrowerQueries.borrowerNameIdLookupTable.TryGetValue(allLoansList[i].BorrowerID, out name);
                allLoansList[i].BorrowerName = name;

            }
            Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            allLoansList.Sort(comparison);
            return allLoansList;

        }

        public List<Loan> getInActiveLoans()
        {
            List<Loan> allLoansList = sQLLoanQueries.getInActiveLoans();
            if (allLoansList == null)
            {
                log.Error("Quering all loans and returned null");
                return new List<Loan>();
            }
            for (int i = 0; i < allLoansList.Count; i++)
            {
                string name;
                SQLBorrowerQueries.borrowerNameIdLookupTable.TryGetValue(allLoansList[i].BorrowerID, out name);
                allLoansList[i].BorrowerName = name;

            }
            Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            allLoansList.Sort(comparison);
            return allLoansList;

        }

        public void setCurrentLoan(Loan loan)
        {
            currentLoan = loan;
        }

        public Loan getCurrentLoan()
        {
            return currentLoan;
        }

        public void setUpdatedLoan(Loan updated)
        {
            updatedLoan = updated;
        }

        public Loan getUpdatedLoan()
        {
            return updatedLoan;
        }

        public List<Loan> getAllLoans()
        {

            log.Info("Get a list of all loans");
            List<Loan> allLoansList = sQLLoanQueries.getAllLoans();
            if (allLoansList == null)
            {
                log.Error("Quering all loans and returned null");
                return new List<Loan>();
            }
            for (int i = 0; i < allLoansList.Count; i++)
            {
                string name;
                SQLBorrowerQueries.borrowerNameIdLookupTable.TryGetValue(allLoansList[i].BorrowerID, out name);
                allLoansList[i].BorrowerName = name;

            }
            Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            allLoansList.Sort(comparison);
            return allLoansList;
        }

        public List<Loan> searchBorrowerName(string name, int number)
        {
            log.Info("Search Borrower by Name");
            List<Loan> searchLoan = new List<Loan>();
            List<Borrower> borrowerSearchList = sQLBorrowerQueries.searchBorrowerName(name);
            //searching the borrower data base for borrower names simular to the search criteria
            if (number == 3)
            {

            }
            else if (number == 2)
            {


            }
            else
            {

            }
            if (borrowerSearchList != null)
            {

                for (int i = 0; i < borrowerSearchList.Count; i++)
                {
                    //For each found grab borrowers iD
                    string id = borrowerSearchList[i].BorrowerId;
                    //For each borrower ID, look the id up in the loan table and get the loan data
                    List<Loan> result = new List<Loan>();
                    if (number == 3)
                    {
                        result = sQLLoanQueries.searchBorrowerID(id);
                    }
                    else if (number == 2)
                    {
                        result = sQLLoanQueries.searchBorrowerIDActiveLoan(id);

                    }
                    else
                    {
                        result = sQLLoanQueries.searchBorrowerIDNotActiveLoan(id);
                    }

                    for (int j = 0; j < result.Count; j++)
                    {

                        //Adding borrowers name because its not in the loan database
                        result[j].BorrowerName = borrowerSearchList[i].BorrowerName;
                        //Add the loan data to the list for the grid data display
                        searchLoan.Add(result[j]);

                    }
                }

                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                searchLoan.Sort(comparison);
            }
            else
            {
                log.Error("Borrower list came back null");
                return searchLoan;
            }
            return searchLoan;
        }


        public bool checkDateTime(string title, string value)
        {
            log.Info("check date time");
            if (value == null)
            {
                log.Error("value is null");
                System.Windows.MessageBox.Show("Please enter " + title);
                return false;
            }

            if (value.Equals(""))
            {
                System.Windows.MessageBox.Show("Please enter " + title);
                return false;
            }
            bool res = Utilities.checkDateRange(title, value);
            if (!res)
            {
                return false;
            }

            return true;
        }




        //Checking required text boxes to make sure they have values in them for strings
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

        //Checking required text boxes to make sure they have values in them for doubles
        public bool checkRequiredDouble(string title, string value)
        {

            log.Info("Checking to make sure requried double values are filled in for adding a loan");
            double doubleValue;
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

            result = double.TryParse(value, out doubleValue);

            return result;
        }

        //Checking non required text boxes to make sure they have values in them for doubles
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

            result = double.TryParse(value, out doubleValue);

            return result;
        }

        //DMV: ***********************Need to investagate this more
        //when a new borrower gets created, a default loan will get create too;
        public bool addDefaultNewLoanForCMAndLPS(Borrower borrower)
        {
            string defaultCompany;
            bool returnValue;

            if (borrower == null)
            {
                log.Error("when creating a new borrower and creating a default loan, borrower is null");
                return false;
            }
           
            ///DMV Need to test on tuesday***********************
            Loan loan = new Loan();
            loan.BorrowerID = borrower.BorrowerId;
            loan.BorrowerName = borrower.BorrowerName;

            loan.Company = "CFII";
            loan.UserID = Environment.UserName;
           
            loan.DocumentType = "New Loan Document";
            

            string maxCMID = this.getMaxCMLoanIndex();
            string max = this.add1MaxID(maxCMID);
            //string maxLPSID = accessDatabaseLoanLPSController.getMaxLoanID();
            //string max = this.findMaxID(maxLPSID, maxCMID);
            if (max.Equals(""))
            {
                System.Windows.MessageBox.Show("Finding default loan max id came back empty");
                return false;
            }
            loan.LoanID = max;
            this.setCurrentLoan(loan);
            //bool res = accessDatabaseLoanLPSController.addLoanToLPS(loan);
            //if (res)
            //{
            //    System.Windows.MessageBox.Show("Default Loan was successfully added to the  LPS database.");
            //    returnValue = true;
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Default Loan was Not successfully added to the LPS database.");

            //    return false;
            //}

            bool result = this.addLoanCMWithID(loan);
            if (result)
            {
               // System.Windows.MessageBox.Show("Default Loan was successfully added to the  database.");
                returnValue = true;
            }
            else
            {
               // System.Windows.MessageBox.Show("Default Loan was Not successfully added to the  database.");
               
                return false;
            }
           

            return result;

        }


        //Adding a new loan
        public bool addNewLoan()
        {
            log.Info("adding new loan");
            bool addNewLoanResult = true;
            string messageBox;
            Loan loan = this.getCurrentLoan();
            if (loan == null)
            {
                log.Error("Trying to add a new loan but the loan is null");
                return false;
            }

            List<Loan> searchBorrowerByNameList = this.searchBorrowerName(loan.BorrowerName, 3);

            //Searchs to see if there is an existing loan
            if (searchBorrowerByNameList != null && searchBorrowerByNameList.Count >= 1)
            {
                string title = "Found a possible match in the database.";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result;
                messageBox = "";

                for (int i = 0; i < searchBorrowerByNameList.Count; i++)
                {
                    if (i <= 20)
                    {
                        string name = searchBorrowerByNameList[i].BorrowerName;
                        string nickName = searchBorrowerByNameList[i].Nickname;
                        messageBox = messageBox + Environment.NewLine + "Borrower Name: " + name + "      Loan NickName: " + nickName;
                    }
                }

                messageBox = messageBox + (Environment.NewLine + Environment.NewLine + "Do you want to proceed with adding a new loan?");
                result = MessageBox.Show(messageBox, title, buttons);
                if (result == MessageBoxResult.Yes)
                {
                    //do nothing

                }
                else
                {
                    return false;
                }
            }

            sQLLoanQueries.addNewLoan(loan);
            return addNewLoanResult;
        }

        public bool updateLoan(Loan loan)
        {
            log.Info("Update loan");
            if (loan == null)
            {
                log.Error("trying to upate loan, loan is null");
                return false;
            }
            bool result = sQLLoanQueries.udateLoan(loan);
            return result;

        }

        //Checks to see if the Amortization length has changed
        //if amort lenght changes return true
        public bool hasAmortizationLengthChanged()
        {
            log.Info("hasAmortizationLengthChanged Check");
            currentLoan = this.getCurrentLoan();
            updatedLoan = this.getUpdatedLoan();
            if (currentLoan == null)
            {
                return false;
            }
            if (updatedLoan == null)
            {
                return false;
            }
            bool result = false;

            if (currentLoan.AmortizationLength != updatedLoan.AmortizationLength)
            {
                result = true;
            }
            return result;
        }

        //Checks to see if the Amortization date has changed
        //returns true if changed
        public bool hasAmortizationDateChanged()
        {
            log.Info("hasAmortizationDateChanged Check");
            Loan currentLoan = this.getCurrentLoan();
            Loan updatedLoan = this.getUpdatedLoan();
            if (currentLoan == null)
            {
                return false;
            }
            if (updatedLoan == null)
            {
                return false;
            }
            bool result = true;
            DateTime currentAmorDate;
            DateTime updatedAmorDate;

            if ((DateTime.TryParse(currentLoan.AmortStartDate, out currentAmorDate)) & (DateTime.TryParse(updatedLoan.AmortStartDate, out updatedAmorDate)))
            {

                if (DateTime.Equals(currentAmorDate.Date, updatedAmorDate.Date))
                {
                    result = false;
                }

            }

            return result;
        }

        //Renewal Change: (existing) Maturity < (new) maturity
        //DateTime.Compare c# method
        //Less than zero : If t1 is earlier than t2.
        //Zero : If t1 is the same as t2.
        //Greater than zero : If t1 is later than t2.
        //returns true if renewal changes
        public bool hasRenewalChanged()
        {
            log.Info("hasRenewalChanged Check");
            Loan currentLoan = this.getCurrentLoan();
            Loan updatedLoan = this.getUpdatedLoan();
            if (currentLoan == null)
            {
                return false;
            }
            if (updatedLoan == null)
            {
                return false;
            }
            bool result = false;
            DateTime currentMaturityDate;
            DateTime updatedMaturityrDate;

            if ((DateTime.TryParse(currentLoan.MaturityDate, out currentMaturityDate)) & (DateTime.TryParse(updatedLoan.MaturityDate, out updatedMaturityrDate)))
            {
                //if value < zero,arg 1 si earlier than arg 2
                int value = DateTime.Compare(currentMaturityDate.Date, updatedMaturityrDate.Date);
                if (value < 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        //Maturity Date Change: 
        //DateTime.Compare c# method
        //Less than zero : If t1 is earlier than t2.
        //Zero : If t1 is the same as t2.
        //Greater than zero : If t1 is later than t2.
        public bool hasMaturityDateChanged()
        {
            log.Info("hasMaturityDateChanged Check");
            Loan currentLoan = this.getCurrentLoan();
            Loan updatedLoan = this.getUpdatedLoan();
            if (currentLoan == null)
            {
                return false;
            }
            if (updatedLoan == null)
            {
                return false;
            }
            bool result = false;
            DateTime currentMaturityDate;
            DateTime updatedMaturityrDate;

            if ((DateTime.TryParse(currentLoan.MaturityDate, out currentMaturityDate)) & (DateTime.TryParse(updatedLoan.MaturityDate, out updatedMaturityrDate)))
            {
                //Greater than zero : If t1 is later than t2.
                int value = DateTime.Compare(currentMaturityDate.Date, updatedMaturityrDate.Date);
                if (value != 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            return result;
        }

        //Adds up all the existing Limit values
        private double getExistingLimits()
        {
            log.Info("getExistingLimits");
            Loan currentLoan = this.getCurrentLoan();
            Loan updatedLoan = this.getUpdatedLoan();
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

        //Adds up all the new Limit values
        private double getNewLimits()
        {
            log.Info("getNewLimits");
            Loan currentLoan = this.getCurrentLoan();
            Loan updatedLoan = this.getUpdatedLoan();
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

        //Increase: (existing) Tier_1_limit +  Tier_2_Limit + Tier_3_Limit < (new) Tier_1_limit +  Tier_2_Limit + Tier_3_Limit
        public bool hasLimitIncreasedChanged()
        {
            log.Info("hasLimitIncreasedChanged Check");
            bool result = false;

            if (this.getExistingLimits() < this.getNewLimits())
            {
                result = true;
            }
            return result;
        }

        //Decrease: (existing) Tier_1_limit +  Tier_2_Limit + Tier_3_Limit > (new) Tier_1_limit +  Tier_2_Limit + Tier_3_Limit
        public bool hasLimitDecreasedChanged()
        {
            log.Info("hasLimitDecreasedChanged Check");
            bool result = false;

            if (this.getExistingLimits() > this.getNewLimits())
            {
                result = true;
            }
            return result;
        }

        //if hasRateIncreasedChanged or hasRateDecreasedChanged is true
        public bool hasMaxChanged()
        {
            log.Info("hasMaxChanged Check");
            bool result = false;

            if (this.hasInterestRateChanged())
            {
                result = true;
            }
            return result;
        }

        //Interest Rate Change: any of the rate fields (deferred or not) different from new value (one to one comparison on same rate field no combining)
        public bool hasInterestRateChanged()
        {
            log.Info("hasInterestRateChanged Check");
            currentLoan = this.getCurrentLoan();
            updatedLoan = this.getUpdatedLoan();
            if (currentLoan == null)
            {
                return false;
            }
            if (updatedLoan == null)
            {
                return false;
            }
            bool result = false;
            //Rate
            //tier 1 Rate current
            double tier1RateAccruedCurrent;
            double tier2RateAccruedCurrent;
            double tier3RateAccruedCurrent;
            Double.TryParse(currentLoan.Tier1Rate, out tier1RateAccruedCurrent);
            Double.TryParse(currentLoan.Tier2Rate, out tier2RateAccruedCurrent);
            Double.TryParse(currentLoan.Tier3Rate, out tier3RateAccruedCurrent);

            //tier 1 Rate deffered current
            double tier1RateDiferredCurrent;
            double tier2RateDiferredCurrent;
            double tier3RateDiferredCurrent;
            Double.TryParse(currentLoan.Tier1DeferredRate, out tier1RateDiferredCurrent);
            Double.TryParse(currentLoan.Tier2DeferredRate, out tier2RateDiferredCurrent);
            Double.TryParse(currentLoan.Tier3DeferredRate, out tier3RateDiferredCurrent);

            //tier 1 Rate updated
            double tier1RateAccruedUpdated;
            double tier2RateAccruedUpdated;
            double tier3RateAccruedUpdated;
            Double.TryParse(updatedLoan.Tier1Rate, out tier1RateAccruedUpdated);
            Double.TryParse(updatedLoan.Tier2Rate, out tier2RateAccruedUpdated);
            Double.TryParse(updatedLoan.Tier3Rate, out tier3RateAccruedUpdated);

            //teir 1 Deffered updated
            double tier1RateDiferredUpdated;
            double tier2RateDiferredUpdated;
            double tier3RateDiferredUpdated;
            Double.TryParse(updatedLoan.Tier1DeferredRate, out tier1RateDiferredUpdated);
            Double.TryParse(updatedLoan.Tier2DeferredRate, out tier2RateDiferredUpdated);
            Double.TryParse(updatedLoan.Tier3DeferredRate, out tier3RateDiferredUpdated);

            if (tier1RateAccruedCurrent != tier1RateAccruedUpdated)
            {
                result = true;
            }
            if (tier2RateAccruedCurrent != tier2RateAccruedUpdated)
            {
                result = true;
            }
            if (tier3RateAccruedCurrent != tier3RateAccruedUpdated)
            {
                result = true;
            }
            if (tier1RateDiferredCurrent != tier1RateDiferredUpdated)
            {
                result = true;
            }
            if (tier2RateDiferredCurrent != tier2RateDiferredUpdated)
            {
                result = true;
            }
            if (tier3RateDiferredCurrent != tier3RateDiferredUpdated)
            {
                result = true;
            }




            //tier 1 Floor current
            double tier1FloorAccruedCurrent;
            double tier2FloorAccruedCurrent;
            double tier3FloorAccruedCurrent;
            Double.TryParse(currentLoan.Tier1Floor, out tier1FloorAccruedCurrent);
            Double.TryParse(currentLoan.Tier2Floor, out tier2FloorAccruedCurrent);
            Double.TryParse(currentLoan.Tier3Floor, out tier3FloorAccruedCurrent);

            //tier 1 Floor deffered current
            double tier1FloorDiferredCurrent;
            double tier2FloorDiferredCurrent;
            double tier3FloorDiferredCurrent;
            Double.TryParse(currentLoan.Tier1DeferredFloor, out tier1FloorDiferredCurrent);
            Double.TryParse(currentLoan.Tier2DeferredFloor, out tier2FloorDiferredCurrent);
            Double.TryParse(currentLoan.Tier3DeferredFloor, out tier3FloorDiferredCurrent);

            //tier 1 Floor updated
            double tier1FloorAccruedUpdated;
            double tier2FloorAccruedUpdated;
            double tier3FloorAccruedUpdated;
            Double.TryParse(updatedLoan.Tier1Floor, out tier1FloorAccruedUpdated);
            Double.TryParse(updatedLoan.Tier2Floor, out tier2FloorAccruedUpdated);
            Double.TryParse(updatedLoan.Tier3Floor, out tier3FloorAccruedUpdated);

            //teir 1 Deffered updated
            double tier1FloorDiferredUpdated;
            double tier2FloorDiferredUpdated;
            double tier3FloorDiferredUpdated;
            Double.TryParse(updatedLoan.Tier1DeferredFloor, out tier1FloorDiferredUpdated);
            Double.TryParse(updatedLoan.Tier2DeferredFloor, out tier2FloorDiferredUpdated);
            Double.TryParse(updatedLoan.Tier3DeferredFloor, out tier3FloorDiferredUpdated);

            if (tier1FloorAccruedCurrent != tier1FloorAccruedUpdated)
            {
                result = true;
            }
            if (tier2FloorAccruedCurrent != tier2FloorAccruedUpdated)
            {
                result = true;
            }
            if (tier3FloorAccruedCurrent != tier3FloorAccruedUpdated)
            {
                result = true;
            }
            if (tier1FloorDiferredCurrent != tier1FloorDiferredUpdated)
            {
                result = true;
            }
            if (tier2FloorDiferredCurrent != tier2FloorDiferredUpdated)
            {
                result = true;
            }
            if (tier3FloorDiferredCurrent != tier3FloorDiferredUpdated)
            {
                result = true;
            }



            //tier 1 Ceiling current
            double tier1CeilingAccruedCurrent;
            double tier2CeilingAccruedCurrent;
            double tier3CeilingAccruedCurrent;
            Double.TryParse(currentLoan.Tier1Ceiling, out tier1CeilingAccruedCurrent);
            Double.TryParse(currentLoan.Tier2Ceiling, out tier2CeilingAccruedCurrent);
            Double.TryParse(currentLoan.Tier3Ceiling, out tier3CeilingAccruedCurrent);

            //tier 1 Ceiling deffered current
            double tier1CeilingDiferredCurrent;
            double tier2CeilingDiferredCurrent;
            double tier3CeilingDiferredCurrent;
            Double.TryParse(currentLoan.Tier1DeferredCeiling, out tier1CeilingDiferredCurrent);
            Double.TryParse(currentLoan.Tier2DeferredCeiling, out tier2CeilingDiferredCurrent);
            Double.TryParse(currentLoan.Tier3DeferredCeiling, out tier3CeilingDiferredCurrent);

            //tier 1 Ceiling updated
            double tier1CeilingAccruedUpdated;
            double tier2CeilingAccruedUpdated;
            double tier3CeilingAccruedUpdated;
            Double.TryParse(updatedLoan.Tier1Ceiling, out tier1CeilingAccruedUpdated);
            Double.TryParse(updatedLoan.Tier2Ceiling, out tier2CeilingAccruedUpdated);
            Double.TryParse(updatedLoan.Tier3Ceiling, out tier3CeilingAccruedUpdated);

            //teir 1 Deffered updated
            double tier1CeilingDiferredUpdated;
            double tier2CeilingDiferredUpdated;
            double tier3CeilingDiferredUpdated;
            Double.TryParse(updatedLoan.Tier1DeferredCeiling, out tier1CeilingDiferredUpdated);
            Double.TryParse(updatedLoan.Tier2DeferredCeiling, out tier2CeilingDiferredUpdated);
            Double.TryParse(updatedLoan.Tier3DeferredCeiling, out tier3CeilingDiferredUpdated);

            if (tier1CeilingAccruedCurrent != tier1CeilingAccruedUpdated)
            {
                result = true;
            }
            if (tier2CeilingAccruedCurrent != tier2CeilingAccruedUpdated)
            {
                result = true;
            }
            if (tier3CeilingAccruedCurrent != tier3CeilingAccruedUpdated)
            {
                result = true;
            }
            if (tier1CeilingDiferredCurrent != tier1CeilingDiferredUpdated)
            {
                result = true;
            }
            if (tier2CeilingDiferredCurrent != tier2CeilingDiferredUpdated)
            {
                result = true;
            }
            if (tier3CeilingDiferredCurrent != tier3CeilingDiferredUpdated)
            {
                result = true;
            }




            return result;
        }


        //Reserve Change: Either the dollar value is changed or the days of reserve are changed
        public bool hasInterestReserveChanged()
        {
            log.Info("hasInterestReserveChanged Check");
            Loan currentLoan = this.getCurrentLoan();
            Loan updatedLoan = this.getUpdatedLoan();
            if (currentLoan == null)
            {
                return false;
            }
            if (updatedLoan == null)
            {
                return false;
            }
            bool result = false;
            double interestReserveDays;
            double interestReserveDollars;

            double interestReserveDaysUpdated;
            double interestReserveDollarUpdateds;

            Double.TryParse(currentLoan.InterestReserve, out interestReserveDays);
            Double.TryParse(currentLoan.InterestReserveMax, out interestReserveDollars);

            Double.TryParse(updatedLoan.InterestReserve, out interestReserveDaysUpdated);
            Double.TryParse(updatedLoan.InterestReserveMax, out interestReserveDollarUpdateds);

            if (interestReserveDays != interestReserveDaysUpdated)
            {

                result = true;
            }
            if (interestReserveDollars != interestReserveDollarUpdateds)
            {

                result = true;
            }
            return result;
        }

        public string selectsMostRecentInsertIdentity()
        {
            string id = sQLLoanQueries.selectsMostRecentInsertIdentity();
            return id;
        }

        public double InterestReserveCalculation(Loan loan)
        {
            int days;
            double result;
            double limitNumber;
            double liborFloorNumber;


            double limitNumber2;
            double liborFloorNumber2;

            double limitNumber3;
            double liborFloorNumber3;

            double tier1Result = 0;
            double tier2Result = 0;
            double tier3Result = 0;



            bool res = int.TryParse(loan.InterestReserve, out days);
            if (res == false || days == 0)
            {
                MessageBox.Show("Please fill in Interest Reserve Days");
                return -1;
            }


            //Tier 1 calculation
            bool resMax = double.TryParse(loan.Tier1Max, out limitNumber);
            if (resMax && limitNumber >= .01)
            {
                bool resFloor = double.TryParse(loan.Tier1Floor, out liborFloorNumber);
                if (resFloor && liborFloorNumber >= 1)
                {
                    tier1Result = MathCalculations.InterestReserveCalculation(limitNumber, liborFloorNumber, days);
                }
                else
                {
                    MessageBox.Show("Please provide Tier 1 Floor for calculation of Interest Reserve.");
                    return -1;
                }
            }

            //Tier 2 calculation
            bool resMax2 = double.TryParse(loan.Tier2Max, out limitNumber2);
            if (resMax2 && limitNumber2 >= .01)
            {
                bool resFloor2 = double.TryParse(loan.Tier2Floor, out liborFloorNumber2);
                if (resFloor2 && liborFloorNumber2 >= 1)
                {
                    tier2Result = MathCalculations.InterestReserveCalculation(limitNumber2, liborFloorNumber2, days);
                }
                else
                {
                    MessageBox.Show("Please provide Tier 2 Floor for calculation of Interest Reserve.");
                    return -1;
                }
            }


            //Tier 2 calculation
            bool resMax3 = double.TryParse(loan.Tier3Max, out limitNumber3);
            if (resMax3 && limitNumber3 >= .01)
            {
                bool resFloor3 = double.TryParse(loan.Tier3Floor, out liborFloorNumber3);
                if (resFloor3 && liborFloorNumber3 >= 1)
                {
                    tier3Result = MathCalculations.InterestReserveCalculation(limitNumber3, liborFloorNumber3, days);
                }
                else
                {
                    MessageBox.Show("Please provide Tier 3 Floor for calculation of Interest Reserve.");
                    return -1;
                }
            }


            result = tier1Result + tier2Result + tier3Result;
            result = Math.Round(result, 2);
            return result;
        }

        //amortization date - Maturity date == Amortization length in months
        public int calculateAmortizationLength(Loan loan)
        {
            DateTime amort;
            DateTime matur;

            bool resA = DateTime.TryParse(loan.AmortStartDate, out amort);
            if (!resA)
            {
                MessageBox.Show("Please select an Amortization Date");
                return 0;
            }
            bool resM = DateTime.TryParse(loan.MaturityDate, out matur);
            if (!resM)
            {
                MessageBox.Show("Please select an Maturity Date");
                return 0;
            }
            int res = MathCalculations.calculateAmortizationLength(amort, matur);

            return res;
        }


    }
}
