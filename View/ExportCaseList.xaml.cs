using CaseManager.HelperClasses;
using Newtonsoft.Json;
using OTS.Controller;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace OTS.View
{
    /// <summary>
    /// Interaction logic for ExportCaseList.xaml
    /// </summary>
    public partial class ExportCaseList : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ExportCaseList));
        private readonly LoanController loanController;
        private readonly CollaterialController collaterialController;
        private List<Collaterial> collaterialList;
        private DataTable collaterialDisplayList;
        

        public ExportCaseList()
        {
            InitializeComponent();
            loanController = new LoanController();
            collaterialController = new CollaterialController();
            collaterialList = new List<Collaterial>();
            collaterialDisplayList = new DataTable();
            ActiveLoanRadioButton.IsChecked = true;
        }

        private void ExportButtonClick(object sender, RoutedEventArgs e)
        {

            Loan loan = loanController.getCurrentLoan();
            if(loan == null)
            {
                return;
            }
            if (collaterialList == null)
            {
                return;
            }
            if(collaterialList.Count == 0)
            {
                MessageBox.Show("Loan does not have any cases to export");
                return;
            }
            WriteOutExcelFile writeOutExcelFile = new WriteOutExcelFile();

            bool res = writeOutExcelFile.ExportToFile(collaterialList);
            if (res)
            {
                MessageBox.Show("Cases were successfully written to a file.");
            }
            else
            {
                MessageBox.Show("Cases were NOT successfully written to a file.");
            }
        }
        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            loanController.setCurrentLoan(null);
            this.getExportCaseListPage();
            UpdateCaseSearchTextBox.Text = "";
        }
        private void clearloan()
        {
            ExportCaseDataGrid.ItemsSource = null;
            UpdateCaseSearchTextBox.Text = "";
            loanController.setCurrentLoan(null);
        }

        private void UpdateCaseSearchButtonClick(object sender, RoutedEventArgs e)
        {
            int number = 0;
            if (UpdateCaseSearchTextBox.Text.Trim().Equals(""))
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
            List<Loan> table = loanController.searchBorrowerName(UpdateCaseSearchTextBox.Text,number);
            if (table == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }

            ExportCaseLoanDataGrid.DataContext = table;
        }

        public Page getExportCaseListPage()
        {
            this.populateDataGrid();
            this.populateDataCollateralGrid();
            AllCaseRadioButton.IsChecked = true;
            ActiveLoanRadioButton.IsChecked = true;
            return this;
        }

        //Populates the data grid with borrower database table and all its fields in asending order
        private void populateDataCollateralGrid()
        {
            log.Info("Populating update borrowers data grid");
            List<Collaterial> list = new List<Collaterial>();

            Loan loan = loanController.getCurrentLoan();
            if (loan == null)
            {

                list.Clear();
                ExportCaseDataGrid.ItemsSource = list;
                return;
            }
            
            if ((bool)DroppedCaseRadioButton.IsChecked)
            {
                list = collaterialController.getDroppedCases(loan);
            }
            else if ((bool)ActiveCaseRadioButton.IsChecked)
            {
                list = collaterialController.getActiveCases(loan);
            }
            else
            {
                list = collaterialController.searchCaseLoanID(loan.LoanID);
            }

            collaterialList = list;
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(collaterialList), (typeof(DataTable)));
            collaterialDisplayList = DataTableConversion.convertExportCaseListToDisplay(table);
            ExportCaseDataGrid.ItemsSource = collaterialDisplayList.DefaultView;


        }

        //Populates the data grid with loan database table and all its fields in asending order
        private void populateDataGrid()
        {
            log.Info("Populating loan data grid");
            ExportCaseLoanDataGrid.DataContext = null;
            List<Loan> table = new List<Loan>();
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                this.clearloan();
                table = loanController.getInActiveLoans();
            }
            else if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                this.clearloan();
                table = loanController.getActiveLoans();
            }
            else
            {
                this.clearloan();
                table = loanController.getAllLoans();
            }
            ExportCaseLoanDataGrid.DataContext = table;
        }

        private void LoanDataGridMouseDoublClick(object sender, MouseButtonEventArgs e)
        {
            if (ExportCaseLoanDataGrid.SelectedItem == null) return;
            Loan loan = ExportCaseLoanDataGrid.SelectedItem as Loan;
            if (loan == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }

            loanController.setCurrentLoan(loan);

            populateDataCollateralGrid();
            ExportCaseLoanDataGrid.SelectedItem = null;

        }

        private void DroppedRadioButtonClicked(object sender, RoutedEventArgs e)
        {

            log.Info("Populating update borrowers data grid");
            List<Collaterial> list = new List<Collaterial>();
            Loan loan = loanController.getCurrentLoan();
            if (loan != null)
            {
                //clearValues();
                list = collaterialController.getDroppedCases(loan);
                collaterialList = list;
                ExportCaseDataGrid.DataContext = collaterialDisplayList;
            }
        }
        private void AllRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            log.Info("Populating update borrowers data grid");
            List<Collaterial> list = new List<Collaterial>();
            Loan loan = loanController.getCurrentLoan();
            if (loan != null)
            {
                //clearValues();
                list = collaterialController.searchCaseLoanID(loan.LoanID);
                collaterialList = list;
                ExportCaseDataGrid.DataContext = collaterialDisplayList;
            }
        }
        private void ActiveRadioButtonClicked(object sender, RoutedEventArgs e)
        {

            // log.Info("Populating update borrowers data grid");
            List<Collaterial> list = new List<Collaterial>();
            Loan loan = loanController.getCurrentLoan();
            if (loan != null)
            {
               // clearValues();
                list = collaterialController.getActiveCases(loan);
                collaterialList = list;
                ExportCaseDataGrid.DataContext = collaterialDisplayList;
            }

        }

        private void textBoxTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                UpdateCaseSearchButtonClick(this, new RoutedEventArgs());
            }
        }

        private void InActiveLoanRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.clearloan();
                list = loanController.getInActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                ExportCaseLoanDataGrid.DataContext = list;

            }
        }
        private void AllLoanRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)AllLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();


                this.clearloan();
                list = loanController.getAllLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                ExportCaseLoanDataGrid.DataContext = list;

            }
        }
        private void ActiveLoanRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                // log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.clearloan();
                list = loanController.getActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                ExportCaseLoanDataGrid.DataContext = list;

            }

        }

        
        

    }
}
