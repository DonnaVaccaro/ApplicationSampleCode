using OTS.Controller;
using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Globalization;
using OTS.HelperClasses;
using System.Data;

namespace OTS.View
{
    /// <summary>
    /// Interaction logic for AddCase.xaml
    /// </summary>
    public partial class AddCase : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AddCase));
        private readonly CollaterialController collaterialController;
        private readonly LoanController loanController;
        private readonly CaseModificationController caseModificationController;
        //  private Dictionary<string, string> stateCourtLookup;


        public AddCase()
        {
            InitializeComponent();
            collaterialController = new CollaterialController();
            loanController = new LoanController();
            caseModificationController = new CaseModificationController();
            this.populateDropDownSelections();
            ActiveLoanRadioButton.IsChecked = true;
            // stateCourtLookup = new Dictionary<string, string>();
            CoCounselTextBox.IsEnabled = false;
            if (!ActiveUserList.isUserAllowed())
            {
                SaveButton.IsEnabled = false;
            }

        }

        public Page getAddCasePage()
        {
            this.populateDataGrid();
            populateDropDownSelections();
            populateDefaultValues();
            return this;
        }
        private void populateDefaultValues()
        {
            CaseTypeTextBox.SelectedItem = "NA";
            StateCircuitTextBox.SelectedItem = "NA";
            CourtVenueTextBox.SelectedItem = "NA";
            CaseStageTextBox.SelectedItem = "NA";
            InsuranceCarrierCombobox.SelectedItem = "NA";
            ExcessCarrierComboBox.SelectedItem = "NA";
            YearofSettlementTextBox.SelectedItem = "0";
            QtrSettlementTextBox.SelectedItem = "0";
            cfYearofSettlementTextBox.SelectedItem = "0";
            cfQtrSettlementTextBox.SelectedItem = "0";
            NumberOfCasesTextBox.Text = "1";
        }
        public void populateDropDownSelections()
        {
            CaseTypeTextBox.ItemsSource = MainWindow.caseTypeDisplayList;
            StateCircuitTextBox.ItemsSource = MainWindow.stateCircuitDisplayList;
            CourtVenueTextBox.ItemsSource = MainWindow.stateCircuitDisplayList;
            CaseStageTextBox.ItemsSource = MainWindow.caseStageDisplayList;
            populateQtrAndYear();
            InsuranceCarrierCombobox.ItemsSource = InsuranceCompanyController.insuranceCompanyListDisplay;

            ExcessCarrierComboBox.ItemsSource = InsuranceCompanyController.insuranceCompanyListDisplay;
            ExcessCarrierTextBox.IsEnabled = false;
        }
        public void populateQtrAndYear()
        {
            YearofSettlementTextBox.Items.Clear();
            QtrSettlementTextBox.Items.Clear();
            cfYearofSettlementTextBox.Items.Clear();
            cfQtrSettlementTextBox.Items.Clear();

            int maxDate = DateTime.UtcNow.Year + 20;
            YearofSettlementTextBox.Items.Add("0");
            cfYearofSettlementTextBox.Items.Add("0");
            for (int year = 2010; year <= maxDate; ++year)
            {
                YearofSettlementTextBox.Items.Add(year.ToString());
                cfYearofSettlementTextBox.Items.Add(year.ToString());
            }
            QtrSettlementTextBox.Items.Add("0");
            QtrSettlementTextBox.Items.Add("1");
            QtrSettlementTextBox.Items.Add("2");
            QtrSettlementTextBox.Items.Add("3");
            QtrSettlementTextBox.Items.Add("4");

            cfQtrSettlementTextBox.Items.Add("0");
            cfQtrSettlementTextBox.Items.Add("1");
            cfQtrSettlementTextBox.Items.Add("2");
            cfQtrSettlementTextBox.Items.Add("3");
            cfQtrSettlementTextBox.Items.Add("4");

        }

        //Saves a new case into the database
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {


            log.Info("Add case Save button");
            CaseModification caseModification = new CaseModification();
            Nullable<bool> isChecked = new Nullable<bool>();
            Nullable<bool> isAudited = new Nullable<bool>();
            Loan currentLoan = loanController.getCurrentLoan();
            if (currentLoan == null || currentLoan.LoanID.Trim().Equals(""))
            {
                MessageBox.Show("Please select a Loan");
                return;
            }
            Collaterial c = new Collaterial();

            c.BorrowerName = currentLoan.BorrowerName;
            c.BorrowerID = currentLoan.BorrowerID;
            c.LoanID = currentLoan.LoanID;
            c.Plaintiff = PlaintiffTextBox.Text;
            c.BorrowerCaseID = "";
            c.PrimaryDefendant = PrimaryDefendentTextBox.Text;
            c.CaseType = CaseTypeTextBox.Text;
            c.PrimaryInjury = PrimaryInjuryTextBox.Text;
            c.DateOfInjury = DateOfInjuryTextBox.Text;
            c.DateRetainerSigned = DateRetainerSignedTextBox.Text;
            c.DateFiled = DateFiledBox.Text;
            c.StateCircuit = StateCircuitTextBox.Text;
            c.CourtVenue = CourtVenueTextBox.Text;
            c.Jurisdiction = JurisdictionTextBox.Text;
            c.DocketNumber = CaseNumberTextBox.Text;
            c.Judge = JudgeTextBox.Text;
            c.CaseStage = CaseStageTextBox.Text;
            c.CaseStatus = "Open";
            c.CaseCost = this.stripComma(CaseCostDollarsTextBox.Text);



            c.InsuranceCarrier = InsuranceCarrierCombobox.SelectedValue.ToString();
            c.InsuranceCoverageDollars = this.stripComma(InsuranceCoverageDollarsTextBox.Text);

            c.ExcessCarrier = ExcessCarrierComboBox.SelectedValue.ToString();
            c.ExcessCoverageDollars = this.stripComma(ExcessCoverageDollars.Text);
            c.GrossSettlementDollars = this.stripComma(GrossSettlementDollarsTextBox.Text);
            c.AttorneysFeePercent = this.stripComma(AttorneysFeePercentTextBox.Text);
            c.AttorneysFeeDollars = this.stripComma(AttorneysFeetDollarsTextBox.Text);
            c.GrossCoAndReferringCounselFeePercentage = this.stripComma(GrossCoandReferringCounselFeePercentageTextBox.Text);
            c.GrossCoAndReferringCounselFeeDollars = this.stripComma(GrossCoandReferringCounselFeeDollarsTextBox.Text);
            c.FirmsNetFeesPercent = this.stripComma(FirmsNetFeesPercentageTextBox.Text);
            c.FirmsNetFeesDollars = this.stripComma(FirmsNetFeesDollarsTextBox.Text);
            c.ActualGrossSettlement = "0";
            c.ActualNetLegalFees = "0";
            c.ResolutionType = "Unknown";

            c.CfGrossSettlementDollars = this.stripComma(CFGrossSettlementDollarsTextBox.Text);

            c.CfFirmNetFeesDollars = this.stripComma(CFFirmNetFeesDollarsTextBox.Text);
            c.CfAttorneyFeeDollar = this.stripComma(CFAttorneyFeesDollarsTextBox.Text);
            c.QtrSettlement = QtrSettlementTextBox.Text;
            c.YearSettlement = YearofSettlementTextBox.Text;
            c.CfQuaterSettlement = cfQtrSettlementTextBox.Text;
            c.CfYearSettlement = cfYearofSettlementTextBox.Text;
            c.Notes = CaseNotesTextBox.Text;
            c.UserID = Environment.UserName;
            c.ReasonForUpdateSelection = "New Case";
            c.ReasonForUpdateOther = "";
            isChecked = IsTracked.IsChecked;
            c.IsTracked = isChecked.Value.ToString().ToLower();
            isAudited = IsAudited.IsChecked;
            c.IsAudited = isAudited.Value.ToString().ToLower();
            c.BorrowerCaseID = BorrowerCaseID.Text;
            c.IsDirectPay = DirectPayCheckBox.IsChecked.Value.ToString().ToLower();
            if (!DirectPayCheckBox.IsChecked.Value)
            {
                CoCounselTextBox.Text = "";
            }
            c.CoCounsel = CoCounselTextBox.Text;
            c.NumberOfCases = this.stripComma(NumberOfCasesTextBox.Text);
            c.CaseCreatedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            c.CaseModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            c.EffectiveDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            caseModification.CaseCreatedDate = DateTime.Now;
            caseModification.CaseModifiedDate = DateTime.Now;

            if (!this.validate())
            {

                return;
            }

            collaterialController.setCurrentCase(c);
            bool result = collaterialController.addNewCase();
            if (result == false)
            {
                System.Windows.MessageBox.Show("Case was Not successfully added to the database.");
                return;
            }
            else
            {
                System.Windows.MessageBox.Show("Case was successfully added to the database.");
                this.ClearFieldsOnly();
            }


            string id = collaterialController.selectsMostRecentInsertIdentity();
            if (id == null | id.Trim().Equals(""))
            {
                return;
            }
            c.CaseID = id;

            caseModificationController.addNewCaseModification(c, c, caseModification);

            scroll.ScrollToTop();

        }
        private string stripComma(string number)
        {
            return number.Replace(",", "");
        }

        private bool validate()
        {
            //Plaintiff is a required field.  Checking to make sure it has a value
            if (!collaterialController.checkRequiredText(PlaintiffLabel.Content.ToString(), PlaintiffTextBox.Text))
            {
                return false;
            }

            //if (!collaterialController.checkRequiredText(PrimaryDefendentLabel.Content.ToString(), PrimaryDefendentTextBox.Text))
            //{
            //    return false;
            //}

            //if (!collaterialController.checkRequiredText(CaseTypeLabel.Content.ToString(), CaseTypeTextBox.Text))
            //{
            //    return false;
            //}

            if (!collaterialController.checkDateTime(DateOfInjuryLabel.Content.ToString(), DateOfInjuryTextBox.Text))
            {
                return false;
            }

            if (!collaterialController.checkDateTime(DateRetainerSignedLabel.Content.ToString(), DateRetainerSignedTextBox.Text))
            {
                return false;
            }

            if (!collaterialController.checkDateTime(DateFiledLabel.Content.ToString(), DateFiledBox.Text))
            {
                return false;
            }





            return true;
        }

        private void UpdateCaseSearchButtonClick(object sender, RoutedEventArgs e)
        {
            int number = 0;
            if (LoanSearchTextBox.Text.Trim().Equals(""))
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
            List<Loan> table = loanController.searchBorrowerName(LoanSearchTextBox.Text, number);
            if (table == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }
            // LoanDataGrid.DataContext = null;
            LoanDataGrid.DataContext = table;
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {

            this.Clear();

        }
        private void Clear()
        {
            ClearLoanValues();

            this.populateDataGrid();

        }
        private void ClearLoanValues()
        {
            this.ClearFieldsOnly();
            BorrowerNameTextBox.Text = " ";
            LoanNameTextBox.Text = " ";
            CompanyTextBox.Text = " ";
            LoanSearchTextBox.Text = "";
            loanController.setCurrentLoan(null);

        }
        private void ClearFieldsOnly()
        {

            PlaintiffTextBox.Text = "";
            PrimaryDefendentTextBox.Text = "";
            CaseTypeTextBox.Text = "";
            PrimaryInjuryTextBox.Text = "";
            DateOfInjuryTextBox.Text = "";
            DateOfInjuryTextBox.Text = "Select a Date.";
            DateRetainerSignedTextBox.Text = "";
            DateRetainerSignedTextBox.Text = "Select a Date.";
            DateFiledBox.Text = "";
            DateFiledBox.Text = "Select a Date.";
            StateCircuitTextBox.Text = "";
            CourtVenueTextBox.Text = "";
            JurisdictionTextBox.Text = "";
            CaseNumberTextBox.Text = "";
            JudgeTextBox.Text = "";
            CaseStageTextBox.Text = "";
            CaseCostDollarsTextBox.Text = "";
            InsuranceCarrierCombobox.Text = "";

            InsuranceCoverageDollarsTextBox.Text = "";
            ExcessCarrierComboBox.Text = "";
            ExcessCarrierTextBox.Text = "";

            ExcessCoverageDollars.Text = "";
            GrossSettlementDollarsTextBox.Text = "";
            AttorneysFeePercentTextBox.Text = "";
            AttorneysFeetDollarsTextBox.Text = "";
            GrossCoandReferringCounselFeePercentageTextBox.Text = "";
            GrossCoandReferringCounselFeeDollarsTextBox.Text = "";
            FirmsNetFeesPercentageTextBox.Text = "";
            FirmsNetFeesDollarsTextBox.Text = "";
            CFGrossSettlementDollarsTextBox.Text = "";
            CFFirmNetFeesDollarsTextBox.Text = "";
            CFAttorneyFeesDollarsTextBox.Text = "";
            cfYearofSettlementTextBox.SelectedItem = "";
            cfQtrSettlementTextBox.SelectedItem = "";
            QtrSettlementTextBox.Text = "";
            YearofSettlementTextBox.Text = "";
            CaseNotesTextBox.Text = "";

            IsTracked.IsChecked = false;
            IsAudited.IsChecked = false;
            DirectPayCheckBox.IsChecked = false;
            CoCounselTextBox.Text = "";
            BorrowerCaseID.Text = "";
            NumberOfCasesTextBox.Text = "";
            InsuranceCarrierCombobox.ItemsSource = InsuranceCompanyController.insuranceCompanyListDisplay;

            ExcessCarrierComboBox.ItemsSource = InsuranceCompanyController.insuranceCompanyListDisplay;
            ExcessCarrierTextBox.IsEnabled = false;
            populateDefaultValues();


            collaterialController.setCurrentCase(null);
        }


        private void LoanDataGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LoanDataGrid.SelectedItem == null) return;
            Loan loan = LoanDataGrid.SelectedItem as Loan;

            if (loan == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }

            loanController.setCurrentLoan(loan);
            populateLoanTextFields(loan);

            // LoanDataGrid.SelectedItem = null;
        }
        //Populates all the text box fields for the existing borrrower and udate borrower
        private void populateLoanTextFields(Loan loan)
        {

            if (loan == null)
            {
                log.Warn("trying to populate update loan text fields on casebut loan is null");
                return;
            }
            log.Info("Populating update loan text fields on Case");
            BorrowerNameTextBox.Text = loan.BorrowerName;
            LoanNameTextBox.Text = loan.Nickname;
            CompanyTextBox.Text = loan.Company;


        }

        //Populates the data grid with loan database table and all its fields in asending order
        private void populateDataGrid()
        {
            log.Info("Populating loan data grid");

            List<Loan> table = new List<Loan>();
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                this.ClearLoanValues();
                table = loanController.getInActiveLoans();
            }
            else if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                this.ClearLoanValues();
                table = loanController.getActiveLoans();
            }
            else
            {
                this.ClearLoanValues();
                table = loanController.getAllLoans();
            }
            LoanDataGrid.DataContext = table;
        }

        //Populates the Borrower comboBox for user selection
        public void populateBorrowerName()
        {

            Dictionary<string, string> allBorrowerNameList = SQLBorrowerQueries.borrowerNameIdLookupTable;
            List<string> borrowerNames = new List<string>();
            foreach (KeyValuePair<string, string> author in allBorrowerNameList)
            {

                if (!borrowerNames.Contains(author.Value))
                {
                    string res = author.Value;
                    if (!res.Trim().Equals(""))
                    {

                        borrowerNames.Add(res);
                    }
                }

            }


        }


        //Filters the text box so users can only enter double values
        private void PreviewDoubleInput(object sender, TextCompositionEventArgs e)
        {
            bool res = IsTextAllowedForDouble(e.Text);
            if (res == false)
            {
                e.Handled = true;
            }

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
        //Regular Expression representing double values only
        private static readonly Regex dobuleRegex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        private static bool IsTextAllowedForDouble(string text)
        {
            bool res = dobuleRegex.IsMatch(text);
            return res;
        }





        //when user selects the state, the court gets populated based on that
        private void StateCircuitTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // stateCourtLookup = new Dictionary<string, string>();
            if (StateCircuitTextBox.SelectedItem == null)
            {
                return;
            }
            List<string> templist = new List<string>();
            List<StateCourtVenued> list = ComboBoxPopulationController.stateCourtList;


            foreach (StateCourtVenued doc in list)
            {
                string res = StateCircuitTextBox.SelectedItem.ToString();

                if (doc.StateCircuit.Trim().Equals(res.Trim()))
                {
                    templist.Add(doc.CourtVenued);
                    // stateCourtLookup.Add(doc.CourtVenued, doc.Id);
                }
            }

            templist.Sort();
            CourtVenueTextBox.ItemsSource = templist;
            CourtVenueTextBox.SelectedItem = "NA";

        }

        //Filters the text box so users can only enter integer values
        private void PreviewIntInput(object sender, TextCompositionEventArgs e)
        {
            bool res = IsTextAllowedForInteger(e.Text);
            if (res == false)
            {
                e.Handled = true;
            }

        }
        //Regular Expression representing integer values only
        private static readonly Regex integerRegex = new Regex(@"^[0-9]+$");
        private static bool IsTextAllowedForInteger(string text)
        {
            bool res = integerRegex.IsMatch(text);
            return res;
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



        private void FormatCurrency(object sender, TextChangedEventArgs e)
        {

            TextBox textBox1 = sender as TextBox;
            string strnumber = textBox1.Text;
            string finalstring = "";
            double value;
            bool result = double.TryParse(strnumber, out value);
            if (result)
            {
                char[] array = strnumber.ToCharArray();
                int incr = 0;

                foreach (char item in array)
                {
                    if (incr < 3)
                    {
                        finalstring = finalstring + item.ToString();
                    }
                    else
                    {
                        finalstring = finalstring + "," + item.ToString();
                        incr = 0;
                    }
                    incr++;
                    var thisVal = textBox1.Text;


                }
            }
            else
            {
                textBox1.Text = "";
            }
            textBox1.Text = finalstring;
        }


        //this one works
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            //Remove previous formatting, or the decimal check will fail including leading zeros
            string value = textBox1.Text.Replace(",", "")
                .Replace("$", "").TrimStart('0');
            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                // ul /= 100;
                //Unsub the event so we don't enter a loop
                // textBox1.TextChanged -= textBox1_TextChanged;
                //Format the text as currency
                //textBox1.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C2}", ul);
                // textBox1.Text = ul.ToString("#,000.00");
                textBox1.Text = string.Format("{0:n0}", ul);
                // ("#,0.##");
                // textBox1.TextChanged += textBox1_TextChanged;
                //  textBox1.Select(textBox1.Text.Length, 0);
            }
            //bool goodToGo = TextisValid(textBox1.Text);
            ////enterButton.Enabled = goodToGo;
            //if (!goodToGo)
            //{
            //    textBox1.Text = "$0.00";
            //    textBox1.Select(textBox1.Text.Length, 0);
            //}

        }

        private bool TextisValid(string text)
        {
            Regex money = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            return money.IsMatch(text);
        }


        //void tb_TextChanged(object sender, EventArgs e)
        //{
        //    //Remove previous formatting, or the decimal check will fail
        //    string value = CFGrossSettlementDollarsTextBox.Text.Replace(",", "").Replace("$", "");
        //    int ul;
        //    //Check we are indeed handling a number
        //    if (int.TryParse(value, out ul))
        //    {
        //        //Unsub the event so we don't enter a loop
        //        CFGrossSettlementDollarsTextBox.TextChanged -= tb_TextChanged;
        //        //Format the text as currency
        //        CFGrossSettlementDollarsTextBox.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:N0}", ul);
        //        CFGrossSettlementDollarsTextBox.TextChanged += tb_TextChanged;
        //    }
        //}

        private void textBoxTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                UpdateCaseSearchButtonClick(this, new RoutedEventArgs());
            }
        }

        private void InActiveRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.ClearLoanValues();
                list = loanController.getInActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                LoanDataGrid.DataContext = list;

            }
        }
        private void AllRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)AllLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();


                this.ClearLoanValues();
                list = loanController.getAllLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                LoanDataGrid.DataContext = list;

            }
        }
        private void ActiveRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                // log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.ClearLoanValues();
                list = loanController.getActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                LoanDataGrid.DataContext = list;

            }

        }

        private void ICDOnKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox1 = sender as TextBox;
            string number;
            string[] list = null;
            double value;
            string[] decimalend = textBox1.Text.Split(".");
            string finalstring = "";
            bool result = double.TryParse(textBox1.Text, out value);
            if (result)
            {
                number = value.ToString();
                string intPortion = "";
                list = number.Split(".");
                //get integer part of numbber
                if (list != null && list.Length >= 1)
                {
                    intPortion = list[0];
                    char[] array = intPortion.ToCharArray();


                    if (array.Length == 4)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(1, ",");


                    }
                    else if (array.Length == 5)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(2, ",");
                    }
                    else if (array.Length == 6)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(3, ",");
                    }
                    else if (array.Length == 7)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(1, ",");
                        finalstring = finalstring.Insert(5, ",");
                    }
                    else if (array.Length == 8)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(2, ",");
                        finalstring = finalstring.Insert(6, ",");
                    }
                    else if (array.Length == 9)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(3, ",");
                        finalstring = finalstring.Insert(7, ",");
                    }
                    else if (array.Length == 10)
                    {
                        finalstring = intPortion;
                        finalstring = finalstring.Insert(1, ",");
                        finalstring = finalstring.Insert(5, ",");
                        finalstring = finalstring.Insert(9, ",");
                    }
                    else
                    {
                        finalstring = intPortion;
                    }

                }

            }
            else
            {

                finalstring = "";

            }

            if (list != null && decimalend != null && decimalend.Length >= 1 && textBox1.Text.Contains("."))
            {
                finalstring = finalstring + "." + decimalend[1];
            }
            else if (list == null && decimalend != null && decimalend.Length >= 1 && textBox1.Text.Contains("."))
            {
                finalstring = "." + decimalend[1];
            }
           
            textBox1.Text = finalstring;
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void DirectPayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CoCounselTextBox.IsEnabled = true;
        }

        private void DirectPayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CoCounselTextBox.IsEnabled = false;
           // CoCounselTextBox.Text = "";
        }
    }
}
