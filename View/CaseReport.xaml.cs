using CaseManager.HelperClasses;
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
    /// Interaction logic for CaseReport.xaml
    /// </summary>
    public partial class CaseReport : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CaseReport));
        private readonly LoanController loanController;
        private readonly ReportController reportController;
        private DataTable collaterialList;

        public CaseReport()
        {
            InitializeComponent();
            loanController = new LoanController();
            reportController = new ReportController();
            collaterialList = new DataTable();
        }

        private void ExportButtonClick(object sender, RoutedEventArgs e)
        {

            Loan loan = loanController.getCurrentLoan();
            if (loan == null)
            {
                return;
            }
            if (collaterialList == null)
            {
                return;
            }
            WriteOutExcelFile writeOutExcelFile = new WriteOutExcelFile();

            bool res = writeOutExcelFile.ExportToReport(collaterialList);
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
            this.getCaseReportPage();
            UpdateCaseSearchTextBox.Text = "";
        }

        private void UpdateCaseSearchButtonClick(object sender, RoutedEventArgs e)
        {
            int number = 0;
            if (UpdateCaseSearchTextBox.Text.Trim().Equals(""))
            {
                return;
            }
            List<Loan> table = loanController.searchBorrowerName(UpdateCaseSearchTextBox.Text,2);
            if (table == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }

            ExportCaseLoanDataGrid.DataContext = table;
        }

        public Page getCaseReportPage()
        {
            List<Collaterial> list = new List<Collaterial>();
            list.Clear();
            ExportCaseDataGrid.DataContext = list;
            this.populateDataGrid();
           // this.populateDataCollateralGrid();
           
            return this;
        }

        //Populates the data grid with borrower database table and all its fields in asending order
        //DMV: this needs to be replaced with data from case database table.  this is in just for testing.
        private void populateDataCollateralGrid()
        {
            log.Info("Populating update borrowers data grid");
            List<Collaterial> list = new List<Collaterial>();

            Loan loan = loanController.getCurrentLoan();
            if (loan == null)
            {

                list.Clear();
                ExportCaseDataGrid.DataContext = list;
                return;
            }

             collaterialList = reportController.selectCaseSummary(loan.LoanID);
            ExportCaseDataGrid.DataContext = collaterialList;


        }

        //Populates the data grid with loan database table and all its fields in asending order
        private void populateDataGrid()
        {
            log.Info("Populating loan data grid");
            ExportCaseLoanDataGrid.DataContext = null;
            List<Loan> table = loanController.getActiveLoans();
            if (table == null)
            {
                log.Error("When populating udate loan data grid, table came back null");
                return;
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

       



        private void textBoxTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                UpdateCaseSearchButtonClick(this, new RoutedEventArgs());
            }
        }

        private void EnhancedDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (col != null && e.PropertyType == typeof(double))
            {
                // if (col.Header.ToString().Equals("CfNetFees"))
                if (FieldTypesInExcel.dollarList.Contains(col.Header.ToString()))
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "C2" };
                else //if (col.Header.ToString().Equals("NumberOfCases"))
                    if (FieldTypesInExcel.wholeNumberList.Contains(col.Header.ToString()))
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "N0" };
                else //if (col.Header.ToString().Equals("PercentageOfFirmCollateral"))
                    if (FieldTypesInExcel.percentList.Contains(col.Header.ToString()))
                    col.Binding = new Binding(e.PropertyName) { StringFormat = "P3" };

            }
        }
    }
}
