using OTS.Controller;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UploadMassTortList.xaml
    /// </summary>
    public partial class UploadMassTortList : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UploadMassTortList));
        private readonly CollaterialController collaterialController;
        private readonly LoanController loanController;
        private readonly MassTortController massTortController;

        private List<MassTort> massTortList;

        public UploadMassTortList()
        {
            InitializeComponent();
            log.Info("upload mass tort list constructor");
            collaterialController = new CollaterialController();
            massTortList = new List<MassTort>();

            loanController = new LoanController();
            massTortController = new MassTortController();
            loanController.setCurrentLoan(null);
            ActiveLoanRadioButton.IsChecked = true;


            if (!ActiveUserList.isUserAllowed())
            {
                CompareButton.IsEnabled = false;
                UploadButton.IsEnabled = false;
                
            }

            
        }

        public Page getUploadMassTortListPage()
        {
            this.populateDataGrid();
            ActiveLoanRadioButton.IsChecked = true;
            return this;
        }

        //Populates the data grid with loan database table and all its fields in asending order
        public void populateDataGrid()
        {
            log.Info("Populating loan data grid");
            LoanDataGrid.DataContext = null;
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
                log.Error("When populating udate loan data grid, table came back null");
                return;
            }
            LoanDataGrid.DataContext = table;

        }
        private void clearValue()
        {

            this.clearLoan();
            this.populateDataGrid();
        }

        private void clearLoan()
        {
            ExistingMassTortsDataGrid.DataContext = null;
            ExistingCasesDataGrid.DataContext = null;
            UploadedAddMassTortsDataGrid.DataContext = null;
            UploadedUpdatedMassTortsDataGrid.DataContext = null;
            UploadedUpdatedMassTortsCasesNotFoundDataGrid.DataContext = null;
            UploadedUpdatedMassTortsCaseTypeNotValidDataGrid.DataContext = null;
            LoanSearchTextBox.Text = "";
            loanController.setCurrentLoan(null);
        }

         private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            clearValue();
        }


        //Save upload cases
        private void UploadCaseListButtonClick(object sender, RoutedEventArgs e)
        {

            bool resulta = false;
            bool resultu = false;
          

            Loan loan = loanController.getCurrentLoan();
            if (loan == null)
            {
                System.Windows.MessageBox.Show("Please select a loan to upload to.");
                return;
            }
            Microsoft.Win32.OpenFileDialog temp = new Microsoft.Win32.OpenFileDialog();
            temp.Filter = "Excel Worksheets(*.xlsx)|*.xlsx";
            bool result = (bool)temp.ShowDialog();
            if (result == false)
            {

                return;
            }
            string fileName = temp.FileName;



            ReadInExcelFile files = new ReadInExcelFile();

            List<MassTort> existingMassTort = files.ReadInMassTortCompareFile(fileName, 0, loan);
            if (existingMassTort == null)
            {
                return;
            }
            List<MassTort> addMassTort = files.ReadInMassTortCompareFile(fileName, 1, loan);
            List<MassTort> updateMassTort = files.ReadInMassTortCompareFile(fileName, 2, loan);
          
            string fileTemp = System.IO.Path.ChangeExtension(fileName, null);
            WriteOutExcelFile wo = new WriteOutExcelFile();
            if (addMassTort != null && addMassTort.Count > 0)
            {

                System.Windows.MessageBox.Show("Adding masstorts to database .....");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                List<MassTort> addc = massTortController.saveUploadedAddMassTorts(addMassTort);
                System.Windows.MessageBox.Show("Complete");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                if (addc != null & addc.Count < 1)
                {
                    resulta = true;
                }
                else
                {
                    resulta = false;
                }

                if (!resulta)
                {
                    string fileNameadd = fileTemp + "addErrors.xlsx";
                     wo.WriteExcelToFileWithPathMassTorts(addc, fileNameadd);
                    System.Windows.MessageBox.Show("One or more masstorts was Not successfully added to the database.  Error file is here: " + fileNameadd);
                }

            }
            if (updateMassTort != null && updateMassTort.Count > 0)
            {
                System.Windows.MessageBox.Show("Updating masstorts in database .....");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                List<MassTort> updatec = massTortController.saveUploadedUpdatedMassTorts(updateMassTort);
                System.Windows.MessageBox.Show("Complete");
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                if (updatec != null & updatec.Count < 1)
                {
                    resultu = true;
                }
                else
                {
                    resultu = false;
                }

                if (!resultu)
                {
                    string fileNameupdate = fileTemp + "updateErrors.xlsx";
                    wo.WriteExcelToFileWithPathMassTorts(updatec, fileNameupdate);
                    System.Windows.MessageBox.Show("One or more Cases was Not successfully updated to the database.  Error file is here: " + fileNameupdate);
                }
            }


            if (resulta && resultu)
            {
                System.Windows.MessageBox.Show("Upload masstort List successfully added to the database.");
            }
            this.clearValue();
            scroll.ScrollToTop();

        }

        private void LoanDataGridMouseDoublClick(object sender, MouseButtonEventArgs e)
        {
            if (LoanDataGrid.SelectedItem == null) return;
            Loan loan = LoanDataGrid.SelectedItem as Loan;
            if (loan == null)
            {

                log.Error("loan selection was null");
                return;
            }

            loanController.setCurrentLoan(loan);
            populateDataCollateralGrid();
            LoanDataGrid.SelectedItem = null;


        }

        //Populates the data grid with case database table and all its fields 
        private void populateDataCollateralGrid()
        {
            log.Info("Populating cases data grid");

            Loan loan = loanController.getCurrentLoan();
            if (loan == null | loan.LoanID == null)
            {
                log.Error("loan or loan id is null when populating case data grid");
                return;
            }
            List<Collaterial> list = collaterialController.getActiveCases(loan);
            if (list == null)
            {
                log.Error("when searching cases by loan id, list came back null");
                return;
            }

           

            ExistingCasesDataGrid.DataContext = list;
            this.populateDataMassTortGrid();
        }

        //Populates the data grid with case database table and all its fields 
        private void populateDataMassTortGrid()
        {
            log.Info("Populating masstort data grid");

            Loan loan = loanController.getCurrentLoan();
            if (loan == null | loan.LoanID == null)
            {
                log.Error("loan or loan id is null when populating case data grid");
                return;
            }
            List<MassTort> list = massTortController.getMassTortByLoanID(loan);
            if (list == null)
            {
                log.Error("when searching masstort by loan id, list came back null");
                return;
            }



            ExistingMassTortsDataGrid.DataContext = list;
        }

        //search on loans
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
            LoanDataGrid.DataContext = null;
            LoanDataGrid.DataContext = table;
        }



        //reads in excel file from open file dialog
        private void CompareNewMassTortListButtonClick(object sender, RoutedEventArgs e)
        {
            Loan loan = loanController.getCurrentLoan();

            if (loan == null)
            {
                System.Windows.MessageBox.Show("Please select a loan to upload to.");
                return;
            }
            Microsoft.Win32.OpenFileDialog temp = new Microsoft.Win32.OpenFileDialog();
            temp.Filter = "Excel Worksheets(*.xlsx)|*.xlsx";
            bool result = (bool)temp.ShowDialog();
            if (result == false)
            {

                return;
            }
            string fileName = temp.FileName;

            ReadInExcelFile files = new ReadInExcelFile();

            bool res = files.UploadMassTortListReadInMassTortListTemplate(fileName, loan);
            if (!res)
            {
                return;
            }
            List<MassTort> massTortListValues = files.getMassTortList();
            if (massTortListValues == null)
            {
                log.Error("get massTortListValues list from reading excell file came back null");
                return;
            }
            massTortController.UploadMassTortIsItNewOrExisting(massTortListValues, loan);

            List<MassTort> existing = massTortController.getMassTortByLoanID(loan);
            if (existing != null)
            {
                ExistingMassTortsDataGrid.DataContext = existing;
            }
            List<MassTort> newMassTort = massTortController.getUploadedCasesThatWillBeNewCases();
            if (newMassTort != null)
            {
                UploadedAddMassTortsDataGrid.DataContext = newMassTort;
            }
            List<MassTort> updatedMassTort = massTortController.getUploadedCasesThatWillBeUpatedCases();
            if (updatedMassTort != null)
            {
                UploadedUpdatedMassTortsDataGrid.DataContext = updatedMassTort;
            }

            List<MassTort> updatedMassTortCasesTypeNotValid = massTortController.getUploadedMassTortCaseTypeNotValidList();
            if (updatedMassTortCasesTypeNotValid != null)
            {
                UploadedUpdatedMassTortsCaseTypeNotValidDataGrid.DataContext = updatedMassTortCasesTypeNotValid;
            }

            List<MassTort> updatedMassTortCasesNotFound = massTortController.getUploadedCasesThatAreNotFound();
            if (updatedMassTortCasesNotFound != null)
            {
                UploadedUpdatedMassTortsCasesNotFoundDataGrid.DataContext = updatedMassTortCasesNotFound;
            }
            WriteOutExcelFileWithMulitTabls w = new WriteOutExcelFileWithMulitTabls();
            w.WriteMassTortCompareToFile(existing, newMassTort, updatedMassTort, updatedMassTortCasesNotFound, updatedMassTortCasesTypeNotValid);


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

                this.clearLoan();
                list = loanController.getInActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                LoanDataGrid.DataContext = list;

            }
        }
        private void AllLoanRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)AllLoanRadioButton.IsChecked)
            {
                log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();


                this.clearLoan();
                list = loanController.getAllLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                LoanDataGrid.DataContext = list;

            }
        }
        private void ActiveLoanRadioButtonClicked(object sender, RoutedEventArgs e)
        {
            if ((bool)ActiveLoanRadioButton.IsChecked)
            {
                // log.Info("Populating update borrowers data grid");
                List<Loan> list = new List<Loan>();

                this.clearLoan();
                list = loanController.getActiveLoans();
                Comparison<Loan> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
                list.Sort(comparison);
                LoanDataGrid.DataContext = list;

            }

        }
    }
}
