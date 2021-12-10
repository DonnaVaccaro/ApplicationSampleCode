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
    /// Interaction logic for PortfolioDiversificationReport.xaml
    /// </summary>
    public partial class PortfolioDiversificationReport : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PortfolioDiversificationReport));
        private LoanController loanController;
        private ReportController reportController;
        private DataTable summaryLoanID;
        private DataTable detailedLoanID;
        private DataTable summaryFirm;
        private DataTable detailedFirm;

        public PortfolioDiversificationReport()
        {
            InitializeComponent();
            loanController = new LoanController();
            reportController = new ReportController();
            summaryLoanID = new DataTable();
            detailedLoanID = new DataTable();
            summaryFirm = new DataTable();
            detailedFirm = new DataTable();
            loanController.setCurrentLoan(null);
            ActiveLoanRadioButton.IsChecked = true;
        }
        public Page getPortfolioDiversificationReportPage()
        {
            this.populateDataGridAsend();
            RealitazionDataGrid.Visibility = Visibility.Hidden;
            loanController.setCurrentLoan(null);
            populateCompanyComboBox();
            companyCombobox.SelectedItem = "NA";
            summaryLoanID = new DataTable();
            detailedLoanID = new DataTable();
            summaryFirm = new DataTable();
            detailedFirm = new DataTable();
            loanController.setCurrentLoan(null);
            return this;
        }
        private void populateCompanyComboBox()
        {
            companyCombobox.Items.Clear();
            companyCombobox.Items.Add("NA");
            companyCombobox.Items.Add("All CFII");
            companyCombobox.Items.Add("CFII");
            companyCombobox.Items.Add("CALII");
            companyCombobox.Items.Add("CFH");
        }
        //Populates the data grid with loan database table and all its fields in asending order
        public void populateDataGridAsend()
        {
            log.Info("populating data grid asending");
            UpdateLoanDataGrid.DataContext = null;
            List<Loan> table = new List<Loan>();
            if ((bool)DroppedLoanRadioButton.IsChecked)
            {
                this.clearLoan();
                table = loanController.getInActiveLoans();
            }
            else if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                this.clearLoan();
                table = loanController.getActiveLoans();
            }
            else
            {
                this.clearLoan();
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
            loanController.setCurrentLoan(null);
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

            loanController.setCurrentLoan(loan);


            detailedLoanID = reportController.detailedPartPDByLoanID(loan.Company, loan.LoanID);
            if (detailedLoanID == null || detailedLoanID.Rows.Count == 0)
            {
                MessageBox.Show("No data available.");
                RealitazionDataGrid.Visibility = Visibility.Hidden;
                UpdateLoanDataGrid.SelectedItem = null;
                return;
            }
            summaryLoanID = reportController.summaryReportPDByLoanID(loan.Company, loan.LoanID);

            if (summaryLoanID == null || summaryLoanID.Rows.Count == 0)
            {
                MessageBox.Show("No data available.");
                RealitazionDataGrid.Visibility = Visibility.Hidden;
                UpdateLoanDataGrid.SelectedItem = null;
                return;
            }
           // detailedLoanID = DataTableConversion.convertPDDetials(detailedLoanID);
           // summaryLoanID = DataTableConversion.convertPDSummary(summaryLoanID);
            RealitazionDataGrid.DataContext = summaryLoanID;
            RealitazionDataGrid.Visibility = Visibility.Visible;

            companyCombobox.SelectedItem = "NA";


        }

        private void UpdateLoanSearchButtonClick(object sender, RoutedEventArgs e)
        {

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

        private void ViewDetailsButtonClick(object sender, RoutedEventArgs e)
        {
            Loan loan = loanController.getCurrentLoan();
            if (loan == null)
            {
                if (detailedFirm == null)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                if (detailedFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                ReportDisplay report = new ReportDisplay();
                report.setDataGrid(detailedFirm);
                report.Show();
            }
            else
            {



                if (detailedLoanID == null)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                if (detailedLoanID.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                ReportDisplay report = new ReportDisplay();
                report.setDataGrid(detailedLoanID);
                report.Show();
            }


        }

        private void ExportButtonClick(object sender, RoutedEventArgs e)
        {
            Loan loan = loanController.getCurrentLoan();
            string company = companyCombobox.SelectedItem.ToString();
            if (loan == null)
            {
                if (detailedFirm == null || detailedFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }

                if (summaryFirm == null || summaryFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
              
                string fileName = company + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".xlsx";
                WriteOutExcelFileWithMulitTabls w = new WriteOutExcelFileWithMulitTabls();
                bool res = w.ExportReportsOneFile(detailedFirm, summaryFirm, fileName);
                if (res)
                {
                    MessageBox.Show("Cases were successfully written to a file.");
                }
                else
                {
                    MessageBox.Show("Cases were NOT successfully written to a file.");
                }
            }
            else
            {



                if (detailedLoanID == null || detailedLoanID.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }

                if (summaryLoanID == null || summaryLoanID.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
               
                string fileName = loan.BorrowerName + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".xlsx";
                WriteOutExcelFileWithMulitTabls w = new WriteOutExcelFileWithMulitTabls();
                bool res = w.ExportReportsOneFile(detailedLoanID, summaryLoanID, fileName);
                if (res)
                {
                    MessageBox.Show("Cases were successfully written to a file.");
                }
                else
                {
                    MessageBox.Show("Cases were NOT successfully written to a file.");
                }
            }
        }

        private void Clear()
        {
            this.clearLoan();
            this.populateDataGridAsend();
           
        }

        private void clearLoan()
        {
            RealitazionDataGrid.Visibility = Visibility.Hidden;
            UpdateLoanSearchTextBox.Text = "";
            loanController.setCurrentLoan(null);
            UpdateLoanDataGrid.SelectedItem = null;
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {

            this.Clear();
        }

        private void CompaniesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loanController.setCurrentLoan(null);
            if (companyCombobox.SelectedItem == null || companyCombobox.SelectedItem.ToString().Equals("NA"))
            {
                RealitazionDataGrid.Visibility = Visibility.Hidden;
                UpdateLoanDataGrid.SelectedItem = null;
                return;
            }
            string company = companyCombobox.SelectedItem.ToString();
            if (company.ToString().Equals("All CFII"))
            {
                detailedFirm = reportController.detailedPartPDAllCFIIByCompany();
                if (detailedFirm == null || detailedFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                summaryFirm = reportController.summaryReportPDAllCFIIByCompany();

                if (summaryFirm == null || summaryFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
              
            }
            else if (company.ToString().Equals("CFII"))
            {
                detailedFirm = reportController.detailedPartPDCFIIByCompany();
                if (detailedFirm == null || detailedFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                summaryFirm = reportController.summaryReportPDCFIIByCompany();

                if (summaryFirm == null || summaryFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
            }
            else if (company.ToString().Equals("CALII"))
            {
                detailedFirm = reportController.detailedPartPDCALIIByCompany();
                if (detailedFirm == null || detailedFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                summaryFirm = reportController.summaryReportPDCALIIByCompany();

                if (summaryFirm == null || summaryFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
            }
            else if (company.ToString().Equals("CFH"))
            {
                detailedFirm = reportController.detailedPartPDCFHByCompany();
                if (detailedFirm == null || detailedFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
                summaryFirm = reportController.summaryReportPDCFHByCompany();

                if (summaryFirm == null || summaryFirm.Rows.Count == 0)
                {
                    MessageBox.Show("No data available.");
                    RealitazionDataGrid.Visibility = Visibility.Hidden;
                    UpdateLoanDataGrid.SelectedItem = null;
                    return;
                }
            }
           // detailedFirm = DataTableConversion.convertPDDetials(detailedFirm);
           // summaryFirm = DataTableConversion.convertPDSummary(summaryFirm);
            RealitazionDataGrid.DataContext = summaryFirm;
            RealitazionDataGrid.Visibility = Visibility.Visible;
        }

        private void UnselectLoanButtonClick(object sender, RoutedEventArgs e)
        {
            loanController.setCurrentLoan(null);
            RealitazionDataGrid.Visibility = Visibility.Hidden;
            UpdateLoanDataGrid.SelectedItem = null;
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

                this.clearLoan();
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


                this.clearLoan();
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

                this.clearLoan();
                list = loanController.getActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                UpdateLoanDataGrid.DataContext = list;

            }

        }

        private void EnhancedDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (col != null && e.PropertyType == typeof(double) || e.PropertyType == typeof(int))
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
