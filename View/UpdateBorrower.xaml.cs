using Microsoft.AspNetCore.Mvc.ModelBinding;
using OTS.Controller;
using OTS.HelperClasses;
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
using Xceed.Wpf.Toolkit;

namespace OTS.View
{
    /// <summary>
    /// Interaction logic for UpdateBorrower.xaml
    /// </summary>
    public partial class UpdateBorrower : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UpdateBorrower));
        private readonly BorrowerController borrowerController;
        //private readonly AccessDatabaseBorrowerLPSController accessDatabaseBorrowerLPSController;
       // private string defaultStatus;
        private string defaultAutoPay;


        public UpdateBorrower()
        {
            log.Info("Constructor Update Borrower page");
            InitializeComponent();
            borrowerController = new BorrowerController();
            //accessDatabaseBorrowerLPSController = new AccessDatabaseBorrowerLPSController();

           // this.populateStatusComboBox();
            populateStateComboBox();
            populateAutoPayComboBox();
           // ComboBoxPopulationController.statusList.TryGetValue("1", out defaultStatus);

            ComboBoxPopulationController.autopayList.TryGetValue("4", out defaultAutoPay);

            if (!ActiveUserList.isUserAllowed())
            {
                SaveButton.IsEnabled = false;
            }
           
        }
        //Populates the autopay combobox.  
        private void populateAutoPayComboBox()
        {
            log.Info("populating autopay combobox");
            UpdateBorrowerAutopayTextBox.ItemsSource = MainWindow.autopayDisplayList;
            UpdateBorrowerAutopayTextBoxChange.ItemsSource = MainWindow.autopayDisplayList;
            UpdateBorrowerAutopayTextBoxChange.SelectedItem = defaultAutoPay;


        }


        //Populates the status combobox.  
        //private void populateStatusComboBox()
        //{
        //    log.Info("populating Status ComboBox");
        //    UpdateBorrowerStatusComboBox.ItemsSource = MainWindow.statusDisplayList;
        //    UpdateBorrowerStatusComboBoxChange.ItemsSource = MainWindow.statusDisplayList;
        //    UpdateBorrowerStatusComboBoxChange.SelectedItem = defaultStatus;

        //}


        //Populates the state combobox.  
        //This method needs to be pulling the states list from the database
        private void populateStateComboBox()
        {
            log.Info("populating state combobox");
            UpdateBorrowerStateComboBox.ItemsSource = MainWindow.statesDisplayList;
            UpdateBorrowerStateComboBoxChange.ItemsSource = MainWindow.statesDisplayList;


        }

        public Page getUpdateBorrowerPage()
        {
            this.populateDataGrid();
            //populateStatusComboBox();
            populateAutoPayComboBox();
            return this;
        }

        //Saves an updated borrower into the database
        private void UpdateBorrowerSaveButtonClick(object sender, RoutedEventArgs e)
        {
            log.Info("Update Borrower Save Button");
            if (borrowerController.getBorrower() == null || borrowerController.getBorrower().BorrowerId == null)
            {
                System.Windows.MessageBox.Show("Please select a borrower to update from the table");
                log.Warn("User did not select a Borrower to update, application prompted the user to select one");
                return;
            }
            Borrower b = borrowerController.getBorrower();
            if (!this.validateUserInput(b))
            {
                return;
            }
            b.BorrowerName = UpdateBorrowerBorrowerNameTextBoxChange.Text;
            b.BorrowerNameLine2 = UpdateBorrowerBorrowerNameLine2TextBoxChange.Text;
            b.Address1 = UpdateBorrowerAddress1TextBoxChange.Text;
            b.Address2 = UpdateBorrowerAddress2TextBoxChange.Text;
            b.Address3 = UpdateBorrowerAddress3TextBoxChange.Text;
            b.City = UpdateBorrowerCityTextBoxChange.Text;
            b.State = UpdateBorrowerStateComboBoxChange.Text;
            b.Zip = borrowerController.stripZip(UpdateBorrowerZipTextBoxChange.Text);
            b.PhoneNumber = borrowerController.stripPhone(UpdateBorrowerPhoneNumberNameTextBoxChange.Text);
            b.Primarycontact = UpdateBorrowerPrimaryContactTextBoxChange.Text;
            b.Email1 = UpdateBorrowerEmail1TextBoxChange.Text;
            b.Email2 = UpdateBorrowerEmail2TextBoxChange.Text;
            b.Email3 = UpdateBorrowerEmail3TextBoxChange.Text;
            b.Email4 = UpdateBorrowerEmail4TextBoxChange.Text;
            b.Autopay = UpdateBorrowerAutopayTextBoxChange.Text;
            if (UpdateBorrowerCustomersOrigDateTextBoxChange.SelectedDate != null && !UpdateBorrowerCustomersOrigDateTextBoxChange.SelectedDate.Equals(""))
            {
                b.CustomersOrigDate = UpdateBorrowerCustomersOrigDateTextBoxChange.SelectedDate.Value.ToString("MM/dd/yyyy hh:mm tt");
            }
            else
            {
                b.CustomersOrigDate = "01/01/1900";
            }
           
           // b.Status = UpdateBorrowerStatusComboBoxChange.Text;
            b.Notes = UpdateBorrowerNotesTextBoxChange.Text;
            b.ReasonForUpdate = UpdateReasonForChangeTextBoxChange.Text;

            b.UserId = Environment.UserName;
            b.ModifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            borrowerController.setBorrower(b);
            bool res = borrowerController.updateBorrower(b);

            if (res)
            {
                System.Windows.MessageBox.Show("Borrower was successfully updated to the database.");
                this.clear();
            }
            else
            {
                System.Windows.MessageBox.Show("Borrower was Not successfully updated to the database.");
            }

            //bool res2 = accessDatabaseBorrowerLPSController.updateBorrowerToLPS(b);
            //if (res2)
            //{
            //    System.Windows.MessageBox.Show("Borrower was successfully updated to the lps database.");
            //    this.clear();
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Borrower was Not successfully updated to the lps database.");
            //}


            this.populateDataGrid();
            scroll.ScrollToTop();

        }

        //Validates all the user input fields on the page
        private bool validateUserInput(Borrower b)
        {
            log.Info("Validate User Input");

            if (b == null)
            {
                log.Error("validate user input borrower is null");
                return false;
            }
            //Borrower name is a required field.  Checking to make sure it has a value
            if (!borrowerController.checkRequiredValue(UpdateBorrowerNameLabel.Content.ToString(), UpdateBorrowerBorrowerNameTextBoxChange.Text))
            {
                return false;
            }
            //if (!borrowerController.checkRequiredValue(UpdateBorrowerAddress1Label.Content.ToString(), UpdateBorrowerAddress1TextBoxChange.Text))
            //{
            //    return false;
            //}
            //if (!borrowerController.checkRequiredValue(UpdateBorrowerCityLabel.Content.ToString(), UpdateBorrowerCityTextBoxChange.Text))
            //{
            //    return false;
            //}
            if (!borrowerController.checkRequiredValue(UpdateBorrowerStateLabel.Content.ToString(), UpdateBorrowerStateComboBoxChange.Text))
            {
                return false;
            }

            //if (!borrowerController.checkRequiredValue(UpdateBorrowerZipLabel.Content.ToString(), UpdateBorrowerZipTextBoxChange.Text))
            //{

            //    return false;
            //}

            //checking the zip code length
            if (!borrowerController.checkZipCodeLength(UpdateBorrowerZipLabel.Content.ToString(), UpdateBorrowerZipTextBoxChange.Text))
            {
                return false;
            }

            // checking the phone number length
            if (!borrowerController.checkPhoneLength(PhoneNumberLabel.Content.ToString(), UpdateBorrowerPhoneNumberNameTextBoxChange.Text))
            {
                return false;
            }


            //if (!borrowerController.checkRequiredValue(UpdateBorrowerAutopayLabel.Content.ToString(), UpdateBorrowerAutopayTextBoxChange.Text))
            //{
            //    return false;
            //}

            //if (UpdateBorrowerStatusComboBoxChange.Text.Equals("Funded"))
            //{
            //    if (!borrowerController.checkDateTime(UpdateBorrowerCustomersOrigDateLabel.Content.ToString(), UpdateBorrowerCustomersOrigDateTextBoxChange.Text))
            //    {
            //        UpdateBorrowerCustomersOrigDateTextBoxChange.Text = "";
            //        return false;
            //    }
            //}
                if (!borrowerController.checkRequiredValue(ReasonForUpdate.Content.ToString(), UpdateReasonForChangeTextBoxChange.Text))
            {
                return false;
            }
            return true;
        }



        //Populates all the text box fields for the existing borrrower and udate borrower
        private void populateTextFields(Borrower borrower)
        {
            if (borrower == null)
            {
                log.Warn("trying to populate update borrowers text fields but borrower is null");
                return;
            }
            log.Info("Populating update borrowers text fields");

            UpdateBorrowerBorrowerNameTextBox.Text = borrower.BorrowerName;
            UpdateBorrowerBorrowerNameLine2TextBox.Text = borrower.BorrowerNameLine2;
            UpdateBorrowerAddress1TextBox.Text = borrower.Address1;
            UpdateBorrowerAddress2TextBox.Text = borrower.Address2;
            UpdateBorrowerAddress3TextBox.Text = borrower.Address3;
            UpdateBorrowerCityTextBox.Text = borrower.City;

            UpdateBorrowerStateComboBox.Text = borrower.State;
            UpdateBorrowerZipTextBox.Text = borrower.Zip;
            UpdateBorrowerPhoneNumberNameTextBox.Text = borrower.PhoneNumber;
            UpdateBorrowerPrimaryContactTextBox.Text = borrower.Primarycontact;
            UpdateBorrowerEmail1TextBox.Text = borrower.Email1;
            UpdateBorrowerEmail2TextBox.Text = borrower.Email2;
            UpdateBorrowerEmail3TextBox.Text = borrower.Email3;
            UpdateBorrowerEmail4TextBox.Text = borrower.Email4;
            UpdateBorrowerAutopayTextBox.Text = borrower.Autopay;
            UpdateBorrowerCustomersOrigDateTextBox.Text = borrower.CustomersOrigDate;
           // UpdateBorrowerStatusComboBox.SelectedItem = borrower.Status;
            UpdateBorrowerNotesTextBox.Text = borrower.Notes;
            UpdateReasonForUpdateTextBox.Text = borrower.ReasonForUpdate;

            UpdateBorrowerBorrowerNameTextBoxChange.Text = borrower.BorrowerName;
            UpdateBorrowerBorrowerNameLine2TextBoxChange.Text = borrower.BorrowerNameLine2;
            UpdateBorrowerAddress1TextBoxChange.Text = borrower.Address1;
            UpdateBorrowerAddress2TextBoxChange.Text = borrower.Address2;
            UpdateBorrowerAddress3TextBoxChange.Text = borrower.Address3;
            UpdateBorrowerCityTextBoxChange.Text = borrower.City;

            UpdateBorrowerStateComboBoxChange.Text = borrower.State;
            UpdateBorrowerZipTextBoxChange.Text = borrower.Zip;
            UpdateBorrowerPhoneNumberNameTextBoxChange.Text = borrower.PhoneNumber;
            UpdateBorrowerPrimaryContactTextBoxChange.Text = borrower.Primarycontact;
            UpdateBorrowerEmail1TextBoxChange.Text = borrower.Email1;
            UpdateBorrowerEmail2TextBoxChange.Text = borrower.Email2;
            UpdateBorrowerEmail3TextBoxChange.Text = borrower.Email3;
            UpdateBorrowerEmail4TextBoxChange.Text = borrower.Email4;
            UpdateBorrowerAutopayTextBoxChange.Text = borrower.Autopay;
            UpdateBorrowerCustomersOrigDateTextBoxChange.Text = borrower.CustomersOrigDate;
           // UpdateBorrowerStatusComboBoxChange.SelectedItem = borrower.Status;
            UpdateBorrowerNotesTextBoxChange.Text = borrower.Notes;

            
        }

        //Clears out all the user input fields
        private void UpdateBorrowerClearButtonClick(object sender, RoutedEventArgs e)
        {
            this.clear();

        }

        private void clear()
        {
            log.Info("clearing update borrowers fields");

            UpdateBorrowerBorrowerNameTextBox.Clear();
            UpdateBorrowerBorrowerNameLine2TextBox.Clear();
            UpdateBorrowerAddress1TextBox.Clear();
            UpdateBorrowerAddress2TextBox.Clear();
            UpdateBorrowerAddress3TextBox.Clear();
            UpdateBorrowerCityTextBox.Clear();
            UpdateBorrowerStateComboBox.SelectedValue = "";
            UpdateBorrowerZipTextBox.Clear();
            UpdateBorrowerPhoneNumberNameTextBox.Clear();
            UpdateBorrowerPrimaryContactTextBox.Clear();
            UpdateBorrowerEmail1TextBox.Clear();
            UpdateBorrowerEmail2TextBox.Clear();
            UpdateBorrowerEmail3TextBox.Clear();
            UpdateBorrowerEmail4TextBox.Clear();
            UpdateBorrowerAutopayTextBox.SelectedValue = "";
            UpdateBorrowerCustomersOrigDateTextBox.Text = "";
            UpdateBorrowerCustomersOrigDateTextBox.Text = "Select a Date.";
           // UpdateBorrowerStatusComboBox.SelectedValue = "";
            UpdateBorrowerNotesTextBox.Text = "";
            UpdateReasonForUpdateTextBox.Text = "";

            UpdateBorrowerBorrowerNameTextBoxChange.Clear();
            UpdateBorrowerBorrowerNameLine2TextBoxChange.Clear();
            UpdateBorrowerAddress1TextBoxChange.Clear();
            UpdateBorrowerAddress2TextBoxChange.Clear();
            UpdateBorrowerAddress3TextBoxChange.Clear();
            UpdateBorrowerCityTextBoxChange.Clear();
            UpdateBorrowerStateComboBoxChange.SelectedValue = "";
            UpdateBorrowerZipTextBoxChange.Clear();
            UpdateBorrowerPhoneNumberNameTextBoxChange.Clear();
            UpdateBorrowerPrimaryContactTextBoxChange.Clear();
            UpdateBorrowerEmail1TextBoxChange.Clear();
            UpdateBorrowerEmail2TextBoxChange.Clear();
            UpdateBorrowerEmail3TextBoxChange.Clear();
            UpdateBorrowerEmail4TextBoxChange.Clear();
            UpdateBorrowerAutopayTextBoxChange.SelectedItem = defaultAutoPay;
            UpdateBorrowerCustomersOrigDateTextBoxChange.Text = "";
            UpdateBorrowerCustomersOrigDateTextBoxChange.Text = "Select a Date.";
           // UpdateBorrowerStatusComboBoxChange.SelectedItem = defaultStatus;
            UpdateBorrowerNotesTextBoxChange.Clear();
            UpdateReasonForChangeTextBoxChange.Clear();
            UpdateBorrowerSearchTextBox.Clear();
            UpdateBorrowerSearchTextBox.Text = "";
            this.populateDataGrid();
            borrowerController.setBorrower(null);
        }

        //Populates the data grid with borrower database table and all its fields in asending order
        private void populateDataGrid()
        {
            log.Info("Populating update borrowers data grid");
            UpdateBorrowerDataGrid.DataContext = null;
            List<Borrower> table = borrowerController.getAllBorrowers();
            if (table == null)
            {
                log.Error("When populating udate borrowers data grid, table came back null");
                return;
            }
            Comparison<Borrower> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            table.Sort(comparison);
            UpdateBorrowerDataGrid.DataContext = table;
        }

        //Takes user input and searches on borrower name in the database for like matches
        private void UpdateBorrowerSearchButtonClick(object sender, RoutedEventArgs e)
        {

            if (UpdateBorrowerSearchTextBox.Text.Trim().Equals(""))
            {
                return;
            }
            List<Borrower> table = borrowerController.searchBorrowerName(UpdateBorrowerSearchTextBox.Text);
            if (table == null)
            {
                log.Error("When populating udate borrowers data grid from search function, table came back null");
                return;
            }
            UpdateBorrowerDataGrid.DataContext = null;
            UpdateBorrowerDataGrid.DataContext = table;
        }

        //User double clicks on a row in the database the the fields get populated with the data
        private void DataGridMouseDoublClick(object sender, MouseButtonEventArgs e)
        {
            if (UpdateBorrowerDataGrid.SelectedItem == null) return;
            Borrower borrower = UpdateBorrowerDataGrid.SelectedItem as Borrower;
            if (borrower == null)
            {
                log.Error("When populating udate borrowers data grid from search function, table came back null");
                return;
            }
            this.populateTextFields(borrower);
            borrowerController.setBorrower(borrower);
            UpdateBorrowerDataGrid.SelectedItem = null;

        }

        private void DatePicker_PreviewLostKeyboardFocus(object sender, RoutedEventArgs e)
        {

            //DatePicker datePicker = sender as DatePicker;
            //if (datePicker != null)
            //{
            //    bool res = Utilities.checkDateRange(datePicker.Text);
            //    if (!res)
            //    {
            //        return;
            //    }
            //}

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

        private void textBoxTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                UpdateBorrowerSearchButtonClick(this, new RoutedEventArgs());
            }
        }
    }
}
