using Microsoft.AspNetCore.Mvc.ModelBinding;
using OTS.Controller;
using OTS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using Xceed.Wpf;
using System.Windows.Forms;
using OTS.HelperClasses;

namespace OTS.View
{
    /// <summary>
    /// Interaction logic for AddBorrower.xaml
    /// </summary>
    public partial class AddBorrower : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(AddBorrower));
        private readonly BorrowerController borrowerController;
        private readonly LoanController loanController;
        private readonly LoanModificationController loanModificationController;
       // private AccessDatabaseBorrowerLPSController accessDatabaseBorrowerLPSController;
        //private AccessDatabaseLoanLPSController accessDatabaseLoanLPSController;
        private string defaultAutoPay;


        public AddBorrower()
        {
            log.Info("Constructor Add Borrower page");
            InitializeComponent();
            borrowerController = new BorrowerController();
            loanController = new LoanController();
            loanModificationController = new LoanModificationController();
            //accessDatabaseBorrowerLPSController = new AccessDatabaseBorrowerLPSController();
            //accessDatabaseLoanLPSController = new AccessDatabaseLoanLPSController();
            //   ComboBoxPopulationController.statusList.TryGetValue("1", out defaultStatus);

            ComboBoxPopulationController.autopayList.TryGetValue("4", out defaultAutoPay);
           // populateStatusComboBox();
            populateStateComboBox();
            populateAutoPayComboBox();
            if (!ActiveUserList.isUserAllowed())
            {
                SaveButton.IsEnabled = false;
            }


        }

        //Validates all the user input fields on the page
        private bool validateUserInput(Borrower b)
        {
            log.Info("Validate User Input");

            if (b == null)
            {
                log.Error("Save botton click and borrower is null");
                return false;
            }
            //Borrower name is a required field.  Checking to make sure it has a value
            if (!borrowerController.checkRequiredValue(BorrowerNameLabel.Content.ToString(), BorrowerNameTextBox.Text))
            {
                return false;
            }
            //if (!borrowerController.checkRequiredValue(Address1Label.Content.ToString(), Address1TextBox.Text))
            //{
            //    return false;
            //}
            //if (!borrowerController.checkRequiredValue(CityLabel.Content.ToString(), CityTextBox.Text))
            //{
            //    return false;
            //}
            if (!borrowerController.checkRequiredValue(StateLabel.Content.ToString(), StateComboBox.Text))
            {
                return false;
            }

            //if (!borrowerController.checkRequiredValue(ZipLabel.Content.ToString(), ZipTextBox.Text))
            //{

            //    return false;
            //}

            //checking the zip code length
            if (!borrowerController.checkZipCodeLength(ZipLabel.Content.ToString(), ZipTextBox.Text))
            {
                return false;
            }

            // checking the phone number length
            if (!borrowerController.checkPhoneLength(PhoneNumberLabel.Content.ToString(), PhoneNumberNameTextBox.Text))
            {
                return false;
            }



            if (!borrowerController.checkRequiredValue(AutopayLabel.Content.ToString(), AutopayTextBox.Text))
            {
                return false;
            }
            //if (StatusComboBox.Text.Equals("Funded"))
            //{
            //    if (!borrowerController.checkDateTime(CustomersOrigDateLabel.Content.ToString(), CustomersOrigDatePicker.Text))
            //    {
            //        CustomersOrigDatePicker.Text = "";
            //        return false;
            //    }
            //}
            return true;
        }



        //Saves a new borrower into the database
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {

            log.Info("Add Borrower Save button");

            Borrower b = borrowerController.getBorrower();
            if (!this.validateUserInput(b))
            {
                return;
            }

            b.BorrowerName = BorrowerNameTextBox.Text;
            b.BorrowerNameLine2 = BorrowerNameLine2TextBox.Text;
            b.Address1 = Address1TextBox.Text;
            b.Address2 = Address2TextBox.Text;
            b.Address3 = Address3TextBox.Text;
            b.City = CityTextBox.Text;
            b.State = StateComboBox.Text;

            b.Zip = borrowerController.stripZip(ZipTextBox.Text);
            b.PhoneNumber = borrowerController.stripPhone(PhoneNumberNameTextBox.Text);

            b.Primarycontact = PrimaryContactTextBox.Text;
            b.Email1 = Email1TextBox.Text;
            b.Email2 = Email2TextBox.Text;
            b.Email3 = Email3TextBox.Text;
            b.Email4 = Email4TextBox.Text;
            b.Autopay = AutopayTextBox.Text;
            if (CustomersOrigDatePicker.Text != null && !CustomersOrigDatePicker.Text.Equals(""))
            {
                b.CustomersOrigDate = CustomersOrigDatePicker.Text.ToString();
            }
            else
            {
                b.CustomersOrigDate = "01/01/1900";
            }
          //  b.Status = StatusComboBox.Text;
            b.Notes = NotesTextBox.Text;
            b.ReasonForUpdate = "Added a New Borrower";
            b.UserId = Environment.UserName;
            b.CreatedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            b.ModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            //Find out LPS and CM borrower id max
            //Add new borrower to LPS and CM
            string maxCMID = borrowerController.getMaxCMBorrowerIndex();
            string max = loanController.add1MaxID(maxCMID);
            // string maxLPSID = accessDatabaseBorrowerLPSController.getMaxBorrowerID();
            // string max = loanController.findMaxID(maxLPSID, maxCMID);
            if (max.Equals(""))
            {
                System.Windows.MessageBox.Show("Finding borrower max id came back empty");
                return;
            }
            b.BorrowerId = max;
            borrowerController.setBorrower(b);
            //bool rest = accessDatabaseBorrowerLPSController.addBorrowerToLPS(b);
            //if (rest == false)
            //{
            //    System.Windows.MessageBox.Show("Borrower was Not successfully added to the LPS database.");
            //    return;
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Borrower was successfully added to the LPS database.");
            //    this.Clear();
            //}

            bool result = borrowerController.addBorrowerWithID(b);
            if(result == false)
            {
                System.Windows.MessageBox.Show("Borrower was Not successfully added to the database.");
                return;
            }
            else
            {
                System.Windows.MessageBox.Show("Borrower was successfully added to the database.");
                this.Clear();
            }

          
            //Add default loan to CM and LPS
            bool result1 = loanController.addDefaultNewLoanForCMAndLPS(b);
            if (result1 == false)
            {
               
                return;
            }
            else
            {
               
              
            }
            this.populateDataGridDecs();
            borrowerController.getAllBorrowers();
            scroll.ScrollToTop();
        }

        private void Clear()
        {
            log.Info("Clear add borrower text boxes");
            BorrowerNameTextBox.Clear();
            BorrowerNameLine2TextBox.Clear();
            Address1TextBox.Clear();
            Address2TextBox.Clear();
            Address3TextBox.Clear();
            CityTextBox.Clear();
            StateComboBox.SelectedValue = "";
            ZipTextBox.Clear();
            PhoneNumberNameTextBox.Clear();
            PrimaryContactTextBox.Clear();
            Email1TextBox.Clear();
            Email2TextBox.Clear();
            Email3TextBox.Clear();
            Email4TextBox.Clear();
            AutopayTextBox.SelectedItem = defaultAutoPay;
            CustomersOrigDatePicker.Text = "";
            CustomersOrigDatePicker.Text = "Select a Date.";
           // StatusComboBox.SelectedItem = defaultStatus;
            NotesTextBox.Text = "";

        }

        //Clears out all the user input fields
        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {

            this.Clear();
        }

        public Page getAddBorrowerPage()
        {
            populateDataGridAsend();
            return this;
        }

        //Populates the status combobox.  
        //private void populateStatusComboBox()
        //{
           
        //    log.Info("populating status combobox");
        //    StatusComboBox.ItemsSource = MainWindow.statusDisplayList;
           
        //    StatusComboBox.SelectedItem = defaultStatus;

        //}

        //Populates the status combobox.  
        private void populateAutoPayComboBox()
        {
            log.Info("populating autopay combobox");
            AutopayTextBox.ItemsSource = MainWindow.autopayDisplayList;
            AutopayTextBox.SelectedItem = defaultAutoPay;


        }

        //Populates the state combobox.  
        //This method needs to be pulling the states list from the database
        private void populateStateComboBox()
        {
            log.Info("populating state combobox");

            StateComboBox.ItemsSource = MainWindow.statesDisplayList;

        }


        //Populates the data grid with borrower database table and all its fields in desending order
        //When a user addes a new borrower into the database, the list needs to be desending so the
        // the user can easily see the newly added borrower
        private void populateDataGridDecs()
        {
            log.Info("populating data grid desending");
            BorrowerDataGrid.DataContext = null;
            List<Borrower> table = borrowerController.getAllBorrowers();
            if (table == null)
            {
                log.Error("when populating data grid table came back null");
                return;
            }

            BorrowerDataGrid.DataContext = table.OrderBy(x => Int32.Parse(x.BorrowerId)).Reverse();
        }


        //Populates the data grid with borrower database table and all its fields in asending order
        public void populateDataGridAsend()
        {
            log.Info("populating data grid asending");
            BorrowerDataGrid.DataContext = null;
            List<Borrower> table = borrowerController.getAllBorrowers();
            if (table == null)
            {
                log.Error("when populating data grid table came back null");
                return;
            }
            Comparison<Borrower> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            table.Sort(comparison);

            BorrowerDataGrid.DataContext = table;
        }

        //private void datepickerIputText(object sender, TextCompositionEventArgs e)
        //{
        //    if (CustomersOrigDatePicker.SelectedDate != null)
        //    {
        //        string d = CustomersOrigDatePicker.SelectedDate.Value.ToString();
        //        DateTime date = CustomersOrigDatePicker.SelectedDate.Value;
        //    }
        //}

        //private void datapickermouseleave(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    if (CustomersOrigDatePicker.SelectedDate != null) {
        //    string d = CustomersOrigDatePicker.SelectedDate.Value.ToString();
        //    DateTime date = CustomersOrigDatePicker.SelectedDate.Value;
        //    }
        //}

        private void DatePicker_PreviewLostKeyboardFocus(object sender, RoutedEventArgs e)
        {

        //    DatePicker datePicker = sender as DatePicker;
        //    if(datePicker != null)
        //    {
        //        bool res = Utilities.checkDateRange(datePicker);
        //        if (!res)
        //        {
        //            return;
        //        }
        //    }

           


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

    }
}
