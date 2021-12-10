using CaseManager.Model;
using Microsoft.Win32;
using OTS.Controller;
using OTS.HelperClasses;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace OTS.View
{
    /// <summary>
    /// Interaction logic for UploadCaseList.xaml
    /// </summary>
    public partial class UploadCaseList : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UploadCaseList));
        private readonly CollaterialController collaterialController;
        private readonly LoanController loanController;
        private readonly LoanModificationController loanModificationController;
        private List<Collaterial> collaterialList;
       

        public UploadCaseList()
        {
           
            InitializeComponent();
            log.Info("upload case list constructor");
            collaterialController = new CollaterialController();
            collaterialList = new List<Collaterial>();

            loanController = new LoanController();
            loanModificationController = new LoanModificationController();
            loanController.setCurrentLoan(null);
            ActiveLoanRadioButton.IsChecked = true;
            OnlyResolvedCases.IsEnabled = false;
            OnlyResolvedCases.IsChecked = false;
            if (!ActiveUserList.isUserAllowed())
            {
                CompareButton.IsEnabled = false;
                UploadButton.IsEnabled = false;
            }

            if (Environment.UserName.Equals("emil"))
            {
                OnlyResolvedCases.IsEnabled = true;
            }
        }
      

        public Page getUploadCaseListPage()
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
            LoanDataGrid.DataContext = table;
           
        }
        private void clearValue() 
        {
            this.clearLoan();
            
            this.populateDataGrid();
        }

        private void clearLoan()
        {
            ExistingCollateralDataGrid.DataContext = null;
            UploadedNewCasesDataGrid.DataContext = null;
            UploadedUpdatedCasesDataGrid.DataContext = null;
            UploadedRemovedCasesDataGrid.DataContext = null;
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
            bool resultd = false;
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
            if (OnlyResolvedCases.IsChecked.Value)
            {
                List<Collaterial> existingCases = files.ReadInCaseListCompareFileForResolvedOnly(fileName, 0, loan);
                if (existingCases == null)
                {
                    return;
                }
                List<Collaterial> addCases = files.ReadInCaseListCompareFileForResolvedOnly(fileName, 1, loan);

                string fileTemp = System.IO.Path.ChangeExtension(fileName, null);
                WriteOutExcelFile wo = new WriteOutExcelFile();
                if (addCases != null && addCases.Count != 0)
                {

                    System.Windows.MessageBox.Show("Adding cases to database .....");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    List<Collaterial> addc = collaterialController.saveUploadedAddCasesResolvedOnly(addCases);
                    System.Windows.MessageBox.Show("Complete");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    if (addc != null & addc.Count <= 1)
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
                        wo.WriteExcelToFileWithPath(addc, fileNameadd);
                        System.Windows.MessageBox.Show("One or more Cases was Not successfully added to the database.  Error file is here: " + fileNameadd);
                    }

                }
                this.clearValue();
                scroll.ScrollToTop();
            }
            else
            {
                List<Collaterial> existingCases = files.ReadInCaseListCompareFile(fileName, 0, loan);
                if (existingCases == null)
                {
                    return;
                }
                List<Collaterial> addCases = files.ReadInCaseListCompareFile(fileName, 1, loan);
                List<Collaterial> updateCases = files.ReadInCaseListCompareFile(fileName, 2, loan);
                List<Collaterial> deleteCases = files.ReadInCaseListCompareFile(fileName, 3, loan);
                string fileTemp = System.IO.Path.ChangeExtension(fileName, null);
                WriteOutExcelFile wo = new WriteOutExcelFile();
                if (addCases != null && addCases.Count != 0)
                {

                    System.Windows.MessageBox.Show("Adding cases to database .....");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    List<Collaterial> addc = collaterialController.saveUploadedAddCases(addCases);
                    System.Windows.MessageBox.Show("Complete");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    if (addc != null & addc.Count <= 1)
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
                        wo.WriteExcelToFileWithPath(addc, fileNameadd);
                        System.Windows.MessageBox.Show("One or more Cases was Not successfully added to the database.  Error file is here: " + fileNameadd);
                    }

                }
                if (updateCases != null && updateCases.Count != 0)
                {
                    System.Windows.MessageBox.Show("Updating cases in database .....");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    List<Collaterial> updatec = collaterialController.saveUploadedUpdatedCases(updateCases);
                    System.Windows.MessageBox.Show("Complete");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    if (updatec != null & updatec.Count <= 1)
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
                        wo.WriteExcelToFileWithPath(updatec, fileNameupdate);
                        System.Windows.MessageBox.Show("One or more Cases was Not successfully updated to the database.  Error file is here: " + fileNameupdate);
                    }
                }
                if (deleteCases != null && deleteCases.Count != 0)
                {
                    System.Windows.MessageBox.Show("Dropping cases in database .....");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    List<Collaterial> dropc = collaterialController.saveUploadedDroppedCases(deleteCases);
                    System.Windows.MessageBox.Show("Complete");
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                    if (dropc != null & dropc.Count < 1)
                    {
                        resultd = true;
                    }
                    else
                    {
                        resultd = false;
                    }
                    if (!resultd)
                    {
                        string fileNamedelete = fileTemp + "deleteErrors.xlsx";
                        wo.WriteExcelToFileWithPath(dropc, fileNamedelete);
                        System.Windows.MessageBox.Show("One or more Cases was Not successfully dropped to the database.  Error file is here: " + fileNamedelete);
                    }
                }

                if (resulta && resultu && resultd)
                {
                    System.Windows.MessageBox.Show("Upload Case List successfully added to the database.");
                }
                this.clearValue();
                scroll.ScrollToTop();
            }
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
           // LoanDataGrid.SelectedItem = null;


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
            List<Collaterial> list = new List<Collaterial>();
            if (OnlyResolvedCases.IsChecked.Value)
            {
                list = collaterialController.getDroppedCases(loan);
                if (list == null)
                {
                    log.Error("when searching cases by loan id, list came back null");
                    return;
                }
            }
            else
            {

           
           list = collaterialController.searchCaseLoanID(loan.LoanID);
            if (list == null)
            {
                log.Error("when searching cases by loan id, list came back null");
                return;
            }
            }


            ExistingCollateralDataGrid.DataContext = list;
        }


        private void ExportTemplateButtonButtonClick(object sender, RoutedEventArgs e)
        {
            Loan loan = loanController.getCurrentLoan();
            if (loan == null)
            {
                System.Windows.MessageBox.Show("Please select Loan");
                return;
            }
            List<CaseListTemplate> table = new List<CaseListTemplate>();
            table = collaterialController.searchCaseLoanIDIntoCaseListTemplate(loan.LoanID);
            if (table == null)
            {
                System.Windows.MessageBox.Show("Loan does not have any cases to export");
                return;
            }
            if (table.Count == 0)
            {
                System.Windows.MessageBox.Show("Loan does not have any cases to export");
                return;
            }

            string fileName = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Worksheets(*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;

            }
            else
            {
                log.Info("User cancelled save file dialog");
                return;
            }
            if (fileName == null | fileName.Trim().Equals(""))
            {
                log.Info("filename is null or empty");
                return;
            }
          
            WriteOutExcelFile writeOutExcelFile = new WriteOutExcelFile();


            bool res = writeOutExcelFile.CreateExcelFileForUpload(table, fileName);
            if (res)
            {
                MessageBox.Show("Cases were successfully written to a file.");
            }
            else
            {
                MessageBox.Show("Cases were NOT successfully written to a file.");
            }
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
            List<Loan> table = loanController.searchBorrowerName(LoanSearchTextBox.Text,number);
            if (table == null)
            {
                log.Error("When populating udate loan data grid from search function, table came back null");
                return;
            }
            LoanDataGrid.DataContext = null;
            LoanDataGrid.DataContext = table;
        }

       

        //reads in excel file from open file dialog
        private void CompareNewCaseListButtonClick(object sender, RoutedEventArgs e)
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
            //List<Collaterial> caseListValues = files.ReadInCaseListTemplate(fileName, 0, loan);
            bool res = files.UploadCaseListReadInCaseListTemplate(fileName, loan);
            if (!res)
            {
                return;
            }

            List<Collaterial> caseListValues = files.getCollaterialList();
            if (caseListValues == null)
            {
                log.Error("get caseListValues list from reading excell file came back null");
                return;
            }


            if (OnlyResolvedCases.IsChecked.Value)
            {
                List<Collaterial> existing = collaterialController.getDroppedCases(loan);
                if (existing != null)
                {
                    ExistingCollateralDataGrid.DataContext = existing;
                }
                List<Collaterial> newcases = caseListValues;
                if (newcases != null)
                {
                    UploadedNewCasesDataGrid.DataContext = newcases;
                }
                WriteOutExcelFileWithMulitTabls w = new WriteOutExcelFileWithMulitTabls();
                w.WriteCaseListCompareToFileForOnlyResolvedCases(existing, newcases);
            }
            else
            {
               

                collaterialController.UploadCasesIsItNewExistingOrSettled(caseListValues, loan);

               // List<Collaterial> existing = collaterialController.getActiveCases(loan);
                List<Collaterial> existing = collaterialController.searchCaseLoanID(loan.LoanID);
                if (existing != null)
                {
                    ExistingCollateralDataGrid.DataContext = existing;
                }
                List<Collaterial> newcases = collaterialController.getUploadedCasesThatWillBeNewCases();
                if (newcases != null)
                {
                    UploadedNewCasesDataGrid.DataContext = newcases;
                }
                List<Collaterial> updated = collaterialController.getUploadedCasesThatWillBeUpatedCases();
                if (updated != null)
                {
                    UploadedUpdatedCasesDataGrid.DataContext = updated;
                }
                List<Collaterial> dropped = collaterialController.getExistingCasesByLoanIDSelectedFromUploadCases();
                if (dropped != null)
                {
                    UploadedRemovedCasesDataGrid.DataContext = dropped;
                }
                WriteOutExcelFileWithMulitTabls w = new WriteOutExcelFileWithMulitTabls();
                w.WriteCaseListCompareToFile(existing, newcases, updated, dropped);
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
