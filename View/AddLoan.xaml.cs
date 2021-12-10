using OTS.Controller;
using OTS.DataBase;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace OTS.View
{
    /// <summary>
    /// Interaction logic for AddLoan.xaml
    /// </summary>
    public partial class AddLoan : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AddLoan));
        private BorrowerController borrowerController;
        private LoanController loanController;
        private readonly LoanModificationController loanModificationController;
        private string defaultStatus;
        //private AccessDatabaseLoanLPSController accessDatabaseLoanLPSController;

        public AddLoan()
        {
            log.Info("Constructor Add Loan page");
            InitializeComponent();
            borrowerController = new BorrowerController();
            loanController = new LoanController();
            loanModificationController = new LoanModificationController();
            //accessDatabaseLoanLPSController = new AccessDatabaseLoanLPSController();
            ComboBoxPopulationController.statusList.TryGetValue("1", out defaultStatus);
            this.populateBorrowerName();
            this.populateCompanyName();
            this.populateDefaultValues();
            populateStatusComboBox();
            AllLoanRadioButton.IsChecked = true;
            if (!ActiveUserList.isUserAllowed())
            {
                SaveButton.IsEnabled = false;
            }
        }
        //Populates the status combobox.  
        private void populateStatusComboBox()
        {

            log.Info("populating status combobox");
            StatusTextBox.ItemsSource = MainWindow.statusDisplayList;

            StatusTextBox.SelectedItem = defaultStatus;

        }
        public void populateDefaultValues()
        {
            AmortizationLengthTextBox.Text = "24";
            InterestReservedDaysTextBox.Text = "60";
            CompanyNameTextBox.SelectedItem = "NA";
        }
        public Page getAddLoanPage()
        {
            this.populateDataGridAsend();
            this.populateDefaultValues();
            populateBorrowerName();
            populateStatusComboBox();
            return this;
        }

        //Populates the Company comboBox for user selection
        public void populateBorrowerName()
        {
            // List<Borrower> allBorrowerList =  borrowerController.getAllBorrowers();
            Dictionary<string, string> allBorrowerNameList = SQLBorrowerQueries.borrowerNameIdLookupTable;
            List<string> borrowerNames = new List<string>();
            foreach (KeyValuePair<string, string> author in allBorrowerNameList)
            {
                // Console.WriteLine("Key: {0}, Value: {1}", author.Key, author.Value);
                if (!borrowerNames.Contains(author.Value))
                {
                    string res = author.Value;
                    if (!res.Trim().Equals(""))
                    {

                        borrowerNames.Add(res);
                    }
                }

            }

            BorrowerNameTextBox.ItemsSource = borrowerNames.OrderBy(s => s);
        }

        //Populates the Company comboBox for user selection
        private void populateCompanyName()
        {

            CompanyNameTextBox.ItemsSource = MainWindow.companyDisplayList;
        }

        private Loan getLoanValues()
        {
            //Loan loan = loanController.getCurrentLoan();
            Loan loan = new Loan();


            //Borrower ID lookup, not displayed on page but needed for loan database entry. Required field in database
            List<Borrower> list = borrowerController.searchBorrowerName(BorrowerNameTextBox.Text.Trim());
            if (list == null || list.Count == 0)
            {
                //Need error log
                System.Windows.MessageBox.Show("Select a loan or enter requred values.");
                return null;
            }
            if (loan.BorrowerID == "")
            {
                loan.BorrowerID = borrowerController.LookupBorrowerName(BorrowerNameTextBox.SelectedItem.ToString());
            }
            else
            {
                loan.BorrowerID = loan.BorrowerID;
            }


            loan.BorrowerName = BorrowerNameTextBox.Text;
            loan.Company = CompanyNameTextBox.Text;

            //Customer Original Date 
           // loan.OriginalLoanDate = list[0].CustomersOrigDate;

            //Maturity Date
            loan.MaturityDate = MaturityDatePicker.Text;

            //Amortization Date 
            loan.AmortStartDate = AmortizationDatePicker.Text;

            //Nickmame not required
            loan.Nickname = LoanNicknameTextBox.Text;

            //Company Name


            //Main Only 
            // loan.MailOnly = MailOnlyCheckBox.IsChecked.ToString();
            if ((bool)MailOnlyCheckBox.IsChecked)
            {
                loan.MailOnly = "true";
            }
            else
            {
                loan.MailOnly = "false";
            }


            //Is Hybrid 
            //    loan.IsHybrid = HybridCheckBox.IsChecked.ToString();
            if ((bool)HybridCheckBox.IsChecked)
            {
                loan.IsHybrid = "true";
            }
            else
            {
                loan.IsHybrid = "false";
            }

            //Tier 1 Limit 
            loan.Tier1Max = Tier1LimitTextBox.Text;

            //Tier 1 Rate Accrued
            loan.Tier1Rate = Tier1RateAccruedTextBox.Text;

            //Tier 1 Rate Deferred 
            if (Tier1RateDeferredTextBox.Text.Trim().Equals(""))
            {
                loan.Tier1DeferredRate = "0.0";
            }
            else
            {
                loan.Tier1DeferredRate = Tier1RateDeferredTextBox.Text;
            }

            //Tier 1 LIBOR Floor Accrued
            loan.Tier1Floor = Tier1FloorTextBoxAccrued.Text;

            //Tier 1 LIBOR Floor Deferred
            if (Tier1FloorTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier1DeferredFloor = "0.0";
            }
            else
            {
                loan.Tier1DeferredFloor = Tier1FloorTextBoxDeferred.Text;
            }

            //Tier 1 Ceiling Accrued
            loan.Tier1Ceiling = Tier1CeilingTextBoxAccrued.Text;

            //Tier 1 Ceiling Deferred
            if (Tier1CeilingTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier1DeferredCeiling = "0.0";
            }
            else
            {
                loan.Tier1DeferredCeiling = Tier1CeilingTextBoxDeferred.Text;
            }

            //Tier 2 Limit 
            if (Tier2LimitTextBox.Text.Trim().Equals(""))
            {
                loan.Tier2Max = "0.0";
            }
            else
            {
                loan.Tier2Max = Tier2LimitTextBox.Text;
            }

            //Tier 2 Rate Accrued 
            if (Tier2RateTextBoxAccrued.Text.Trim().Equals(""))
            {
                loan.Tier2Rate = "0.0";
            }
            else
            {
                loan.Tier2Rate = Tier2RateTextBoxAccrued.Text;
            }

            //Tier 2 Rate Deferred
            if (Tier2RateTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier2DeferredRate = "0.0";
            }
            else
            {
                loan.Tier2DeferredRate = Tier2RateTextBoxDeferred.Text;
            }

            //Tier 2 LIBOR Floor Accrued 
            if (Tier2FloorTextBoxAccrued.Text.Trim().Equals(""))
            {
                loan.Tier2Floor = "0.0";
            }
            else
            {
                loan.Tier2Floor = Tier2FloorTextBoxAccrued.Text;
            }

            //Tier 2 LIBOR Floor Deferred
            if (Tier2FloorTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier2DeferredFloor = "0.0";
            }
            else
            {
                loan.Tier2DeferredFloor = Tier2FloorTextBoxDeferred.Text;
            }

            //Tier 2 Ceiling Accrued
            if (Tier2CeilingTextBoxAccrued.Text.Trim().Equals(""))
            {
                loan.Tier2Ceiling = "0.0";
            }
            else
            {
                loan.Tier2Ceiling = Tier2CeilingTextBoxAccrued.Text;
            }

            //Tier 2 Ceiling Deferred
            if (Tier2CeilingTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier2DeferredCeiling = "0.0";
            }
            else
            {
                loan.Tier2DeferredCeiling = Tier2CeilingTextBoxDeferred.Text;
            }

            //Tier 3 Limit 
            if (Tier3LimitTextBox.Text.Trim().Equals(""))
            {
                loan.Tier3Max = "0.0";
            }
            else
            {
                loan.Tier3Max = Tier3LimitTextBox.Text;
            }

            //Tier 3 Rate Accrued 
            if (Tier3RateTextBoxAccrued.Text.Trim().Equals(""))
            {
                loan.Tier3Rate = "0.0";
            }
            else
            {
                loan.Tier3Rate = Tier3RateTextBoxAccrued.Text;
            }


            // Tier 3 Rate Deferred

            if (Tier3RateTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier3DeferredRate = "0.0";
            }
            else
            {
                loan.Tier3DeferredRate = Tier3RateTextBoxDeferred.Text;
            }


            //Tier 3 LIBOR Floor Accrued 
            if (Tier3FloorTextBoxAccrued.Text.Trim().Equals(""))
            {
                loan.Tier3Floor = "0.0";
            }
            else
            {
                loan.Tier3Floor = Tier3FloorTextBoxAccrued.Text;
            }

            //Tier 3 LIBOR Floor Deferred
            if (Tier3FloorTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier3DeferredFloor = "0.0";
            }
            else
            {
                loan.Tier3DeferredFloor = Tier3FloorTextBoxDeferred.Text;
            }

            //Tier 3 Ceiling Accrued
            if (Tier3CeilingTextBoxAccrued.Text.Trim().Equals(""))
            {
                loan.Tier3Ceiling = "0.0";
            }
            else
            {
                loan.Tier3Ceiling = Tier3CeilingTextBoxAccrued.Text;
            }

            //Tier 3 Ceiling Deferred
            if (Tier3CeilingTextBoxDeferred.Text.Trim().Equals(""))
            {
                loan.Tier3DeferredCeiling = "0.0";
            }
            else
            {
                loan.Tier3DeferredCeiling = Tier3CeilingTextBoxDeferred.Text;
            }


            //Is Variable not required
            // loan.IsVariable = IsVariableCheckBox.IsChecked.ToString();
            if ((bool)IsVariableCheckBox.IsChecked)
            {
                loan.IsVariable = "true";
            }
            else
            {
                loan.IsVariable = "false";
            }

            //Amortization Length
            loan.AmortizationLength = AmortizationLengthTextBox.Text;

            //Interest reserve days 
            loan.InterestReserve = InterestReservedDaysTextBox.Text;

            //Interest Reserved Dollars 
            if (InterestReservedDollarTextBox.Text.Trim().Equals(""))
            {
                loan.InterestReserveMax = "0";
            }
            else
            {
                loan.InterestReserveMax = InterestReservedDollarTextBox.Text;
            }
            return loan;
        }
        //Saving a Loan into the database
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            log.Info("Add Loan Save button");
            LoanModification loanModification = new LoanModification();

            Loan loan = getLoanValues();
            if (loan == null)
            {
                return;
            }
            //Do not Mail is not required
            if ((bool)DoNotMailCheckBox.IsChecked)
            {
                loan.DoNotMail = "true";
            }
            else
            {
                loan.DoNotMail = "false";
            }
            loan.Status = StatusTextBox.Text;
            loan.RestrictedAmount = RestrictedAmountDollarTextBox.Text;
            loan.Notes = NotesTextBox.Text;
            loan.ReasonForUpdate = "Added a new Loan";
            loan.UserID = Environment.UserName;
            loan.DocumentType = "Other";
            loan.CreatedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            loan.ModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            loanModification.ModificationDate = DateTime.Now;
            loanModification.IsNewLoan = true;
            if (!this.ValidateUserInput())
            {
                return;
            }

            
            //Find out LPS and CM loan id max
           
            string maxCMID = loanController.getMaxCMLoanIndex();
            string max = loanController.add1MaxID(maxCMID);
            //string maxLPSID = accessDatabaseLoanLPSController.getMaxLoanID();
            //string max = loanController.findMaxID(maxLPSID, maxCMID);
            if (max.Equals(""))
            {
                System.Windows.MessageBox.Show("Finding loan max id came back empty");
                return;
            }
            loan.LoanID = max;
            loanController.setCurrentLoan(loan);
            //bool lpsloan = accessDatabaseLoanLPSController.addLoanToLPS(loan);
            //if (lpsloan == false)
            //{
            //    System.Windows.MessageBox.Show("Loan was Not successfully added to the lps database.");
            //    return;
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Loan was successfully added to the lps database.");
            //    this.clear();
            //}
            bool result = loanController.addLoanCMWithID(loan);
            if (result == false)
            {
                System.Windows.MessageBox.Show("Loan was Not successfully added to the CM database.");
                return;
            }
            else
            {
                System.Windows.MessageBox.Show("Loan was successfully added to the CM database.");
                this.clear();
            }
           
          
            this.populateDataGridDecs();
            scroll.ScrollToTop();

        }

        //Validating all values entered in from user
        private bool ValidateUserInput()
        {
            //Borrower name is a required field.  Checking to make sure it has a value
            if (!loanController.checkRequiredText(BorrowerNameAddLoanLabel.Content.ToString(), BorrowerNameTextBox.Text))
            {
                return false;
            }


          


            //Maturity Date is a required field.
            if (!loanController.checkDateTime(MaturityDateAddLoanLabel.Content.ToString(), MaturityDatePicker.Text))
            {
                MaturityDatePicker.Text = "";
                return false;
            }



            //Amortization Date is a required field.
            if (!loanController.checkDateTime(AmortizationDateAddLoanLabel.Content.ToString(), AmortizationDatePicker.Text))
            {
                AmortizationDatePicker.Text = "";
                return false;
            }


            //Company Name required
            if (!loanController.checkRequiredText(CompanyNameAddLoanLabel.Content.ToString(), CompanyNameTextBox.Text))
            {
                return false;
            }

            
            //Tier 1 Limit retquired.  This is also called Tier 1 Max in the database
            if (!loanController.checkRequiredDouble(Tier1LimitAddLoanLabel.Content.ToString(), Tier1LimitTextBox.Text))
            {
                return false;
            }



            //Tier 1 Rate Accrued is  required
            if (!loanController.checkRequiredDouble(Tier1RateAddLoanLabel.Content.ToString(), Tier1RateAccruedTextBox.Text))
            {
                return false;
            }



            //Tier 1 Rate Deferred not a required field
            if (!loanController.checkNonRequiredDouble(Tier1RateAddLoanLabel.Content.ToString(), Tier1RateDeferredTextBox.Text))
            {
                return false;
            }



            //Tier 1 LIBOR Floor Accrued is a required field
            if (!loanController.checkRequiredDouble(Tier1FloorAddLoanLabel.Content.ToString(), Tier1FloorTextBoxAccrued.Text))
            {
                return false;
            }



            //Tier 1 LIBOR Floor Deferred is not a required field
            if (!loanController.checkNonRequiredDouble(Tier1FloorAddLoanLabel.Content.ToString(), Tier1FloorTextBoxDeferred.Text))
            {
                return false;
            }



            //Tier 1 Ceiling Accrued is a required field
            if (!loanController.checkRequiredDouble(Tier1CeilingAddLoanLabel.Content.ToString(), Tier1CeilingTextBoxAccrued.Text))
            {
                return false;
            }




            //Tier 1 Ceiling Deferred is not required
            if (!loanController.checkNonRequiredDouble(Tier1CeilingAddLoanLabel.Content.ToString(), Tier1CeilingTextBoxDeferred.Text))
            {
                return false;
            }



            //Tier 2 Limit not required.  Also know as Tier 2 Max in the database
            if (!loanController.checkNonRequiredDouble(Tier2LimitAddLoanLabel.Content.ToString(), Tier2LimitTextBox.Text))
            {
                return false;
            }



            //Tier 2 Rate Accrued is not required
            if (!loanController.checkNonRequiredDouble(Tier2RateAddLoanLabel.Content.ToString(), Tier2RateTextBoxAccrued.Text))
            {
                return false;
            }



            //Tier 2 Rate Deferred not reqired
            if (!loanController.checkNonRequiredDouble(Tier2RateAddLoanLabel.Content.ToString(), Tier2RateTextBoxDeferred.Text))
            {
                return false;
            }



            //Tier 2 LIBOR Floor Accrued not required
            if (!loanController.checkNonRequiredDouble(Tier2FloorAddLoanLabel.Content.ToString(), Tier2FloorTextBoxAccrued.Text))
            {
                return false;
            }



            //Tier 2 LIBOR Floor Deferred not required
            if (!loanController.checkNonRequiredDouble(Tier2FloorAddLoanLabel.Content.ToString(), Tier2FloorTextBoxDeferred.Text))
            {
                return false;
            }



            //Tier 2 Ceiling Accrued not required
            if (!loanController.checkNonRequiredDouble(Tier2CeilingAddLoanLabel.Content.ToString(), Tier2CeilingTextBoxAccrued.Text))
            {
                return false;
            }



            //Tier 2 Ceiling Deferred not required
            if (!loanController.checkNonRequiredDouble(Tier2CeilingAddLoanLabel.Content.ToString(), Tier2CeilingTextBoxDeferred.Text))
            {
                return false;
            }




            //Tier 3 Limit not required
            if (!loanController.checkNonRequiredDouble(Tier3LimitAddLoanLabel.Content.ToString(), Tier3LimitTextBox.Text))
            {
                return false;
            }



            //Tier 3 Rate Accrued not required
            if (!loanController.checkNonRequiredDouble(Tier3RateAddLoanLabel.Content.ToString(), Tier3RateTextBoxAccrued.Text))
            {
                return false;
            }




            //Tier 3 Rate Deferred not required
            if (!loanController.checkNonRequiredDouble(Tier3RateAddLoanLabel.Content.ToString(), Tier3RateTextBoxDeferred.Text))
            {
                return false;
            }





            //Tier 3 LIBOR Floor Accrued not required
            if (!loanController.checkNonRequiredDouble(Tier3FloorAddLoanLabel.Content.ToString(), Tier3FloorTextBoxAccrued.Text))
            {
                return false;
            }



            //Tier 3 LIBOR Floor Deferred not required
            if (!loanController.checkNonRequiredDouble(Tier3FloorAddLoanLabel.Content.ToString(), Tier3FloorTextBoxDeferred.Text))
            {
                return false;
            }




            //Tier 3 Ceiling Accrued not required
            if (!loanController.checkNonRequiredDouble(Tier3CeilingAddLoanLabel.Content.ToString(), Tier3CeilingTextBoxAccrued.Text))
            {
                return false;
            }


            //Tier 3 Ceiling Deferred not required
            if (!loanController.checkNonRequiredDouble(Tier3CeilingAddLoanLabel.Content.ToString(), Tier3CeilingTextBoxDeferred.Text))
            {
                return false;
            }


            //Amortization Length is required
            if (!loanController.checkRequiredDouble(AmortizationLengthAddLoanLabel.Content.ToString(), AmortizationLengthTextBox.Text))
            {
                return false;
            }



            //Interest reserve days is required
            if (!loanController.checkRequiredDouble(InterestReserveDaysLoanLabel.Content.ToString(), InterestReservedDaysTextBox.Text))
            {
                return false;
            }



            //Interest Reserved Dollars is not required
            if (!loanController.checkNonRequiredDouble(InterestReserveDollarsLoanLabel.Content.ToString(), InterestReservedDollarTextBox.Text))
            {
                return false;
            }


            return true;

        }
        private void clear()
        {
            BorrowerNameTextBox.Text = "";
           
            MaturityDatePicker.Text = "";
            MaturityDatePicker.Text = "Select a Date.";
            AmortizationDatePicker.Text = "";
            AmortizationDatePicker.Text = "Select a Date.";
            LoanNicknameTextBox.Text = "";
            CompanyNameTextBox.Text = "";
            StatusTextBox.SelectedItem = defaultStatus;
            RestrictedAmountDollarTextBox.Text = "";
            MailOnlyCheckBox.IsChecked = false;
            HybridCheckBox.IsChecked = false;
            Tier1LimitTextBox.Text = "";
            Tier1RateAccruedTextBox.Text = "";
            Tier1RateDeferredTextBox.Text = "";
            Tier1FloorTextBoxAccrued.Text = "";
            Tier1FloorTextBoxDeferred.Text = "";
            Tier1CeilingTextBoxAccrued.Text = "";
            Tier1CeilingTextBoxDeferred.Text = "";
            Tier2LimitTextBox.Text = "";
            Tier2RateTextBoxAccrued.Text = "";
            Tier2RateTextBoxDeferred.Text = "";
            Tier2FloorTextBoxAccrued.Text = "";
            Tier2FloorTextBoxDeferred.Text = "";
            Tier2CeilingTextBoxAccrued.Text = "";
            Tier2CeilingTextBoxDeferred.Text = "";
            Tier3LimitTextBox.Text = "";
            Tier3RateTextBoxAccrued.Text = "";
            Tier3RateTextBoxDeferred.Text = "";
            Tier3FloorTextBoxAccrued.Text = "";
            Tier3FloorTextBoxDeferred.Text = "";
            Tier3CeilingTextBoxAccrued.Text = "";
            Tier3CeilingTextBoxDeferred.Text = "";
            IsVariableCheckBox.IsChecked = false;
            AmortizationLengthTextBox.Text = "";
            InterestReservedDaysTextBox.Text = "";
            InterestReservedDollarTextBox.Text = "";
            DoNotMailCheckBox.IsChecked = false;
            NotesTextBox.Text = "";
            UpdateLoanSearchTextBox.Text = "";
            loanController.setCurrentLoan(null);
            this.populateDataGridAsend();
        }
        //Clears all user entered data in page
        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            this.clear();
        }

        //This method handles user copying and pasting non doubles into text boxes that can only take doubles
        private void CopyPasteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox addressBox = sender as TextBox;
            if (addressBox.Text.Trim().Equals(""))
            {
                return;
            }
            bool res = IsTextAllowedForDouble(addressBox.Text);
            if (res == false)
            {
                System.Windows.MessageBox.Show("In valid paste content: " + addressBox.Text);
                e.Handled = true;
                addressBox.Text = "";
            }
        }

        //This method handles user copying and pasting non integers into text boxes that can only take integers
        private void CopyPasteTextBox_TextChangedInts(object sender, TextChangedEventArgs e)
        {
            TextBox addressBox = sender as TextBox;
            if (addressBox.Text.Trim().Equals(""))
            {
                return;
            }
            bool res = IsTextAllowedForInteger(addressBox.Text);
            if (res == false)
            {
                System.Windows.MessageBox.Show("In valid paste content: " + addressBox.Text);
                e.Handled = true;
                addressBox.Text = "";
            }
            addressBox.SelectionStart = addressBox.Text.Length;
        }

        //Filters the text box so users can only enter double values
        private void PreviewDoubleInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            bool res = IsTextAllowedForDouble(e.Text);
            if (res == false)
            {
                e.Handled = true;
            }
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        //Filters the text box so users can only enter integer values
        private void PreviewIntInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            bool res = IsTextAllowedForInteger(e.Text);
            if (res == false)
            {
                e.Handled = true;
            }
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        //Regular Expression representing double values only
        private static readonly Regex dobuleRegex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        private static bool IsTextAllowedForDouble(string text)
        {
            bool res = dobuleRegex.IsMatch(text);
            return res;
        }

        //Regular Expression representing integer values only
        private static readonly Regex integerRegex = new Regex(@"^[0-9]+$");
        private static bool IsTextAllowedForInteger(string text)
        {
            bool res = integerRegex.IsMatch(text);
            return res;
        }


        //Populates the data grid with loan database table and all its fields in desending order
        //When a user addes a new loan into the database, the list needs to be desending so the
        // the user can easily see the newly added loan
        private void populateDataGridDecs()
        {
            log.Info("populating data grid desending");
            UpdateLoanDataGrid.DataContext = null;
            List<Loan> table = new List<Loan>();
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                this.clear();
                table = loanController.getInActiveLoans();
            }
            else if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                this.clear();
                table = loanController.getActiveLoans();
            }
            else
            {
                this.clear();
                table = loanController.getAllLoans();
            }

            if (table == null)
            {
                log.Error("when populating data grid table came back null");
                return;
            }
            table.OrderBy(x => Int32.Parse(x.LoanID));
            // Comparison <Loan> comparison = (x, y) => String.Compare(x.LoanID.Trim(), y.LoanID.Trim());
            //  table.Sort(comparison);
            table.Reverse();
            UpdateLoanDataGrid.DataContext = table.OrderBy(x => Int32.Parse(x.LoanID));
        }

        //Populates the data grid with loan database table and all its fields in asending order
        public void populateDataGridAsend()
        {
            log.Info("populating data grid asending");
            UpdateLoanDataGrid.DataContext = null;

            List<Loan> table = new List<Loan>();
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                table = loanController.getInActiveLoans();
            }
            else if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                table = loanController.getActiveLoans();
            }
            else
            {
                table = loanController.getAllLoans();
            }


            if (table == null)
            {
                log.Error("when populating data grid table came back null");
                return;
            }

            Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            table.Sort(comparison);
            UpdateLoanDataGrid.DataContext = table;
        }
        private void UpdateLoanSearchButtonClick(object sender, RoutedEventArgs e)
        {
           // List<Loan> table = new List<Loan>();
            int number = 0;
            if (UpdateLoanSearchTextBox.Text.Trim().Equals(""))
            {
                return;
            }
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                number = 1;
            }
            else if ((bool)ActiveLoanRadioButton.IsChecked)
            {

                number = 2;
            }
            else
            {

                number = 3;
            }
            List<Loan> table = loanController.searchBorrowerName(UpdateLoanSearchTextBox.Text, number);
            if (table == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }
            UpdateLoanDataGrid.DataContext = null;
            UpdateLoanDataGrid.DataContext = table;
        }

        private void LoanDataGridMouseDoublClick(object sender, MouseButtonEventArgs e)
        {


            if (UpdateLoanDataGrid.SelectedItem == null) return;
            Loan loan = UpdateLoanDataGrid.SelectedItem as Loan;
            if (loan == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }
            //string borrowerNumber = borrowerController.LookupBorrowerName(loan.BorrowerName);
            Borrower b = borrowerController.getBorrowerById(loan.BorrowerID);
            //loan.OriginalLoanDate = b.CustomersOrigDate;
            loanController.setCurrentLoan(loan);
            populateTextFields(loan);
            UpdateLoanDataGrid.SelectedItem = null;



        }

        private void populateTextFields(Loan loan)
        {
            loan = loanController.getCurrentLoan();
            BorrowerNameTextBox.SelectedItem = loan.BorrowerName;
            CompanyNameTextBox.Text = loan.Company;
            
            loanController.setCurrentLoan(loan);

        }

        private void DatePicker_DateValidationError(object sender,
                DatePickerDateValidationErrorEventArgs e)
        {
            DateTime newDate;
            DatePicker datePickerObj = sender as DatePicker;

            if (DateTime.TryParse(e.Text, out newDate))
            {
                if (datePickerObj.BlackoutDates.Contains(newDate))
                {
                    System.Windows.MessageBox.Show("Incorrect date");
                }
            }
            else
            {
                //System.Windows.MessageBox.Show("Incorrect date");

            }
        }

        private void CalculateInterestReserveButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.getLoanValues() == null)
            {
                System.Windows.MessageBox.Show("Please select a loan and add values for this calculation");
                return;
            }
            double result = loanController.InterestReserveCalculation(this.getLoanValues());
            if (result == -1)
            {
                return;
            }
            InterestReservedDollarTextBox.Text = result.ToString();
        }

        private void CalculateAmortiationLengthButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.getLoanValues() == null)
            {
                System.Windows.MessageBox.Show("Please select a loan and add values for this calculation");
                return;
            }
            int result = loanController.calculateAmortizationLength(this.getLoanValues());
            AmortizationLengthTextBox.Text = result.ToString();
        }


        private void textBoxTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                UpdateLoanSearchButtonClick(this, new RoutedEventArgs());
            }
        }

        private void InActiveRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.clear();
                list = loanController.getInActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                UpdateLoanDataGrid.DataContext = list;

            }
        }
        private void AllRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)AllLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.clear();
                list = loanController.getAllLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                UpdateLoanDataGrid.DataContext = list;

            }
        }
        private void ActiveRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                // log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.clear();
                list = loanController.getActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                UpdateLoanDataGrid.DataContext = list;

            }

        }
    }
}
