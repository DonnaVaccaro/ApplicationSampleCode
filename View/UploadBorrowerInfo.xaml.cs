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
    /// Interaction logic for UploadBorrowerInfo.xaml
    /// </summary>
    public partial class UploadBorrowerInfo : Page
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UploadCaseList));
        private readonly BorrowerController borrowerController;
       
        private List<Borrower> borrowerList;

        private readonly LoanController loanController;
        private readonly LoanModificationController loanModificationController;

        private List<Loan> loanList;

        public UploadBorrowerInfo()
        {
            log.Info("upload borrower list constructor");
            InitializeComponent();
            borrowerController = new BorrowerController();
            borrowerList = new List<Borrower>();
            loanController = new LoanController();
            loanList = new List<Loan>();
            loanModificationController = new LoanModificationController();
        }

        public Page getUploadBorrowerInfoPage()
        {
            this.populateDataGrid();
            return this;
        }

        //Populates the data grid with loan database table and all its fields in asending order
        public void populateDataGrid()
        {
            log.Info("Populating loan data grid");
            LoanDataGrid.DataContext = null;
            List<Borrower> table = borrowerController.getAllBorrowers();
            if (table == null)
            {
                log.Error("When populating udate loan data grid, table came back null");
                return;
            }
            LoanDataGrid.DataContext = table;

        }
        private void clearValue()
        {

            ExistingCollateralDataGrid.DataContext = null;
           
        
           
            this.populateDataGrid();
        }
        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            clearValue();
        }


        private void ReadInBorrowerButtonClick(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog temp = new Microsoft.Win32.OpenFileDialog();
            temp.Filter = "Excel Worksheets(*.xlsx)|*.xlsx";
            bool result = (bool)temp.ShowDialog();
            if (result == false)
            {

                return;
            }
            string fileName = temp.FileName;

            ReadInExcelFile files = new ReadInExcelFile();

            bool res = files.ReadInBorrower(fileName);
            if (!res)
            {
                return;
            }
            List<Borrower> caseListValues = files.getBorrowerList();
            if (caseListValues == null)
            {
                log.Error("get caseListValues list from reading excell file came back null");
                return;
            }

            borrowerController.addAllBorrower(caseListValues);

        }

        private void ReadInLoanButtonClick(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog temp = new Microsoft.Win32.OpenFileDialog();
            temp.Filter = "Excel Worksheets(*.xlsx)|*.xlsx";
            bool result = (bool)temp.ShowDialog();
            if (result == false)
            {

                return;
            }
            string fileName = temp.FileName;

            ReadInExcelFile files = new ReadInExcelFile();

            bool res = files.ReadInLoan(fileName);
            if (!res)
            {
                return;
            }
            List<Loan> caseListValues = files.getLoanList();
            if (caseListValues == null)
            {
                log.Error("get caseListValues list from reading excell file came back null");
                return;
            }

            loanController.addAllLoan(caseListValues);
          

        }
    }
}
