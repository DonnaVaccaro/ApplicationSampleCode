using CaseManager.HelperClasses;
using CaseManager.Model;
using OTS;
using OTS.Controller;
using OTS.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaseManager.View
{
    /// <summary>
    /// Interaction logic for EnhancedPortfolioDiversificationReport.xaml
    /// </summary>
    public partial class EnhancedPortfolioDiversificationReport : Page
    {
        public List<EnhancedPortfolioDiversificationSelection> caseTypeList;
        public List<EnhancedPortfolioDiversificationSelection> fundedEntityList;
        public List<EnhancedPortfolioDiversificationSelection> typeOfActionList;
        public List<EnhancedPortfolioDiversificationSelection> natureOfClaimList;
        public List<EnhancedPortfolioDiversificationSelection> firmClaimList;
        public ReportController reportController;
        public List<int> listFunded;
        public List<string> listAction;
        public List<string> listCaseType;
        public List<string> listNatureOfClaim;
        public List<int> listFirm;
        public DateTime dateSelected;
        public bool dateTimeChecked;

        List<EnhancedPortfolioDiversification> enh;


        public EnhancedPortfolioDiversificationReport()
        {

          
            InitializeComponent();
            DatePicker.IsEnabled = false;
           
            DatePicker.SelectedDate = DateTime.Today;
            fundedEntityList = ComboBoxPopulationController.AddFundedEntityInList();

            reportController = new ReportController();
           

            BindDropDown();
            listFunded = new List<int>();
            listAction = new List<string>();
            listCaseType = new List<string>();
            listNatureOfClaim = new List<string>();
            listFirm = new List<int>();
            enh = new List<EnhancedPortfolioDiversification>();

        }


        public Page getEnhancedPortfolioDiversificationReportPage()
        {
            RealitazionDataGrid.Visibility = Visibility.Hidden;
            MessageBoxResult result = MessageBox.Show("Loading Data will take about 30 seconds..........", "Confirmation", MessageBoxButton.OKCancel);
            //  MessageBox.Show("Loading Data will take about 15 seconds..........");
            if (result != MessageBoxResult.OK)
            {
                
                return null;
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            reportController.createBorrowerLookDenomiator();
            MessageBox.Show("Finished Loading");
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            return this;
        }

        private void BindDropDown()
        {

            FundedEntity.ItemsSource = fundedEntityList;
        }


        //Funded Entity
        private void FundedEntitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FundedEntityTextChanged(object sender, TextChangedEventArgs e)
        {
            FundedEntity.ItemsSource = fundedEntityList.Where(x => x.Name.StartsWith(FundedEntity.Text.Trim()));
        }

        private void FundedEntityCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            FundedEntityBindListBOX(sender);
        }

        private void FundedEntityBindListBOX(object sender)
        {
            
            CheckBox clickedBox = (CheckBox)sender;
            if(clickedBox.Content == null)
            {
                return;
            }
            listFunded = new List<int>();

            if ((clickedBox.Content.Equals("Select All")) && (clickedBox.IsChecked.Value))

            {

                foreach (var obj2 in fundedEntityList)

                {
                    if (!listFunded.Contains(obj2.ID) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = true;

                        listFunded.Add(obj2.ID);
                    }

                }
                if (listFunded.Count != 0)
                {
                    FundedEntity.ItemsSource = null;
                    FundedEntity.ItemsSource = fundedEntityList;
                    FundedText.Foreground = Brushes.Green;
                    FundedText.Content = listFunded.Count + " items selected";
                    this.updateFundedEntityBindListBOX(listFunded);
                }
               
            }else
            if ((clickedBox.Content != null && clickedBox.Content.Equals("Select All")) && !(clickedBox.IsChecked.Value))
            {

                foreach (var obj2 in fundedEntityList)

                {
                    if (!listFunded.Contains(obj2.ID) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = false;

                    }

                }

                FundedEntity.ItemsSource = null;
                FundedEntity.ItemsSource = fundedEntityList;
                FundedText.Foreground = Brushes.Red;
                FundedText.Content = "0 items selected";
                listFunded = new List<int>();
                firmClaimList = ComboBoxPopulationController.ClearBorrower();
                FirmText.Content = "0 items selected";
                FirmText.Foreground = Brushes.Red;
                Firm.ItemsSource = firmClaimList;
                listFirm = new List<int>();
                caseTypeList = ComboBoxPopulationController.ClearCaseType();
                CaseTypeText.Content = "0 items selected";
                CaseTypeText.Foreground = Brushes.Red;
                CaseType.ItemsSource = caseTypeList;
                listCaseType = new List<string>();
                listAction = new List<string>();
                typeOfActionList = ComboBoxPopulationController.ClearTypeOfAction();
                ActionText.Content = "0 items selected";
                ActionText.Foreground = Brushes.Red;
                TypeOfAction.ItemsSource = typeOfActionList;
               
                natureOfClaimList = ComboBoxPopulationController.ClearNatureOfClaim();
                NatureText.Content = "0 items selected";
                NatureText.Foreground = Brushes.Red;
                NatureofClaim.ItemsSource = natureOfClaimList;
                listNatureOfClaim = new List<string>();

                EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();
              



            }
            else
            {
                foreach (var obj2 in fundedEntityList)
                {
                    if (!listFunded.Contains(obj2.ID) && !obj2.Name.Equals("Select All"))
                    {
                       
                        if (obj2.CheckStatus)
                        {
                            listFunded.Add(obj2.ID);
                        }
                    }
                }
                if (listFunded.Count != 0)
                {
                    FundedEntity.ItemsSource = null;
                    FundedEntity.ItemsSource = fundedEntityList;
                    FundedText.Foreground = Brushes.Green;
                    FundedText.Content = listFunded.Count + " items selected";
                    this.updateFundedEntityBindListBOX(listFunded);
                }
            }
           FundedEntity.SelectedItem = null;
          
            RealitazionDataGrid.Visibility = Visibility.Hidden;
        }

        public void updateFundedEntityBindListBOX(List<int> list)
        {
            enh = reportController.getEnhancedPortfolioDiversificationByCompany(listFunded, dateSelected, dateTimeChecked);

            EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();
          

            firmClaimList = ComboBoxPopulationController.AddBorrowerList(enh);
            Firm.ItemsSource = firmClaimList;
            FirmText.Foreground = Brushes.Red;
            FirmText.Content = "0 items selected";


            typeOfActionList = ComboBoxPopulationController.AddTypeOfActionInList(enh);
            TypeOfAction.ItemsSource = typeOfActionList;
            ActionText.Foreground = Brushes.Red;
            ActionText.Content = "0 items selected";

            caseTypeList = ComboBoxPopulationController.AddCaseTypeInList(enh);
            CaseType.ItemsSource = caseTypeList;
            CaseTypeText.Foreground = Brushes.Red;
            CaseTypeText.Content = "0 items selected";

            natureOfClaimList = ComboBoxPopulationController.AddNatureOfClaimInList(enh);
            NatureofClaim.ItemsSource = natureOfClaimList;
            NatureText.Foreground = Brushes.Red;
            NatureText.Content = "0 items selected";
        }



        //Firm
        private void FirmSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FirmTextChanged(object sender, TextChangedEventArgs e)
        {
            Firm.ItemsSource = firmClaimList.Where(x => x.Name.StartsWith(Firm.Text.Trim()));
        }

        private void FirmCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            FirmBindListBOX(sender);
        }

        private void FirmBindListBOX(object sender)
        {
            CheckBox clickedBox = (CheckBox)sender;
            if (clickedBox.Content == null)
            {
                return;
            }
            listFirm = new List<int>();

            if ((clickedBox.Content.Equals("Select All")) && (clickedBox.IsChecked.Value))

            {

                foreach (var obj2 in firmClaimList)

                {
                    if (!listFirm.Contains(obj2.ID) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = true;
                        listFirm.Add(obj2.ID);
                    }

                }
                if (listFirm.Count != 0)
                {
                    Firm.ItemsSource = null;
                    Firm.ItemsSource = firmClaimList;
                    FirmText.Foreground = Brushes.Green;
                    FirmText.Content = listFirm.Count + " items selected";
                    this.updateFirmBindListBOX(listFunded);
                }
            }
            else
            if (clickedBox.Content != null && (clickedBox.Content.Equals("Select All")) && !(clickedBox.IsChecked.Value))
            {

                foreach (var obj2 in firmClaimList)

                {
                    if (!listFirm.Contains(obj2.ID) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = false;

                    }

                }

                Firm.ItemsSource = null;
                listFirm = new List<int>();
                FirmText.Foreground = Brushes.Red;
                FirmText.Content = "0 items selected";
                Firm.ItemsSource = firmClaimList;

                caseTypeList = ComboBoxPopulationController.ClearCaseType();
                CaseTypeText.Content = "0 items selected";
                CaseTypeText.Foreground = Brushes.Red;
                CaseType.ItemsSource = caseTypeList;
                listCaseType = new List<string>();

                listAction = new List<string>();
                typeOfActionList = ComboBoxPopulationController.ClearTypeOfAction();
                ActionText.Content = "0 items selected";
                ActionText.Foreground = Brushes.Red;
                TypeOfAction.ItemsSource = typeOfActionList;
              
                natureOfClaimList = ComboBoxPopulationController.ClearNatureOfClaim();
                NatureText.Content = "0 items selected";
                NatureText.Foreground = Brushes.Red;
                NatureofClaim.ItemsSource = natureOfClaimList;
                listNatureOfClaim = new List<string>();

                EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();
            }
            else
            {
                foreach (var obj2 in firmClaimList)

                {
                    if (!listFirm.Contains(obj2.ID) && !obj2.Name.Equals("Select All"))
                    {
                        if (obj2.CheckStatus)
                        {
                            listFirm.Add(obj2.ID);
                        }
                    }

                }
                if (listFirm.Count != 0 && listFunded.Count != 0)
                {
                    Firm.ItemsSource = null;
                    Firm.ItemsSource = firmClaimList;
                    FirmText.Foreground = Brushes.Green;
                    FirmText.Content = listFirm.Count + " items selected";
                    this.updateFirmBindListBOX(listFunded);
                }
            }
            RealitazionDataGrid.Visibility = Visibility.Hidden;
        }

        public void updateFirmBindListBOX(List<int> list)
        {
            enh = reportController.getEnhancedPortfolioDiversificationFirm(listFunded, listFirm,dateSelected, dateTimeChecked);
             EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();

            typeOfActionList = ComboBoxPopulationController.AddTypeOfActionInList(enh);
            TypeOfAction.ItemsSource = typeOfActionList;
            ActionText.Foreground = Brushes.Red;
            ActionText.Content = "0 items selected";

            caseTypeList = ComboBoxPopulationController.AddCaseTypeInList(enh);
            CaseType.ItemsSource = caseTypeList;
            CaseTypeText.Foreground = Brushes.Red;
            CaseTypeText.Content = "0 items selected";

            natureOfClaimList = ComboBoxPopulationController.AddNatureOfClaimInList(enh);
            NatureofClaim.ItemsSource = natureOfClaimList;
            NatureText.Foreground = Brushes.Red;
            NatureText.Content = "0 items selected";
        }



        //Type of Action
        private void TypeOfActionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TypeOfActionTextChanged(object sender, TextChangedEventArgs e)
        {
            TypeOfAction.ItemsSource = typeOfActionList.Where(x => x.Name.StartsWith(TypeOfAction.Text.Trim()));
        }

        private void TypeOfActionCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            TypeOfActionBindListBOX(sender);
        }

        private void TypeOfActionBindListBOX(object sender)
        {
            CheckBox clickedBox = (CheckBox)sender;
            if (clickedBox.Content == null)
            {
                return;
            }
            listAction = new List<string>();

            if ((clickedBox.Content.Equals("Select All")) && (clickedBox.IsChecked.Value))

            {

                foreach (var obj2 in typeOfActionList)

                {
                    if (!listAction.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {

                        obj2.CheckStatus = true;
                        listAction.Add(obj2.Name);
                    }

                }
                if (listAction.Count != 0)
                {
                    TypeOfAction.ItemsSource = null;
                    TypeOfAction.ItemsSource = typeOfActionList;
                    ActionText.Foreground = Brushes.Green;
                    ActionText.Content = listAction.Count + " items selected";
                    this.updateTypeOfActionBindListBOX(listFunded);
                }
            }
            else
            if (clickedBox.Content != null && (clickedBox.Content.Equals("Select All")) && !(clickedBox.IsChecked.Value))
            {

                foreach (var obj2 in typeOfActionList)

                {
                    if (!listAction.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = false;


                    }

                }


               
                listAction = new List<string>();
                TypeOfAction.ItemsSource = null;
                ActionText.Content = "0 items selected";
                ActionText.Foreground = Brushes.Red;
                TypeOfAction.ItemsSource = typeOfActionList;

                caseTypeList = ComboBoxPopulationController.ClearCaseType();
                CaseType.ItemsSource = null;
                CaseTypeText.Foreground = Brushes.Red;
                CaseTypeText.Content = "0 items selected";
                CaseType.ItemsSource = caseTypeList;


                natureOfClaimList = ComboBoxPopulationController.ClearNatureOfClaim();
                NatureText.Content = "0 items selected";
                NatureText.Foreground = Brushes.Red;
                NatureofClaim.ItemsSource = natureOfClaimList;
                listNatureOfClaim = new List<string>();


                EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();


            }
            else
            {
                foreach (var obj2 in typeOfActionList)

                {
                    if (!listAction.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {

                        if (obj2.CheckStatus)
                        {
                            listAction.Add(obj2.Name);
                        }
                    }

                }
                if (listAction.Count != 0 && listFirm.Count != 0 && listFunded.Count != 0)
                {
                    TypeOfAction.ItemsSource = null;
                    TypeOfAction.ItemsSource = typeOfActionList;
                    ActionText.Foreground = Brushes.Green;
                    ActionText.Content = listAction.Count + " items selected";
                    this.updateTypeOfActionBindListBOX(listFunded);
                }
            }
            RealitazionDataGrid.Visibility = Visibility.Hidden;
        }

        public void updateTypeOfActionBindListBOX(List<int> list)
        {
            enh = reportController.getEnhancedPortfolioDiversificationTypeOfAction(listFunded, listFirm, listAction,dateSelected,dateTimeChecked);
              EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();

            caseTypeList = ComboBoxPopulationController.AddCaseTypeInList(enh);
            CaseType.ItemsSource = caseTypeList;
            CaseTypeText.Foreground = Brushes.Red;
            CaseTypeText.Content = "0 items selected";

            natureOfClaimList = ComboBoxPopulationController.AddNatureOfClaimInList(enh);
            NatureofClaim.ItemsSource = natureOfClaimList;
            NatureText.Foreground = Brushes.Red;
            NatureText.Content = "0 items selected";
        }



        //Case Type
        private void CaseTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CaseTypeTextChanged(object sender, TextChangedEventArgs e)
        {
            CaseType.ItemsSource = caseTypeList.Where(x => x.Name.StartsWith(CaseType.Text.Trim()));
        }

        private void CaseTypeCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            CaseTypeBindListBOX(sender);
        }

        private void CaseTypeBindListBOX(object sender)
        {
            CheckBox clickedBox = (CheckBox)sender;
            if (clickedBox.Content == null)
            {
                return;
            }
            listCaseType = new List<string>();

            if ((clickedBox.Content.Equals("Select All")) && (clickedBox.IsChecked.Value))

            {

                foreach (var obj2 in caseTypeList)

                {
                    if (!listCaseType.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = true;
                        listCaseType.Add(obj2.Name);
                    }

                }
                if (listCaseType.Count != 0)
                {
                    CaseType.ItemsSource = null;
                    CaseType.ItemsSource = caseTypeList;
                    CaseTypeText.Foreground = Brushes.Green;
                    CaseTypeText.Content = listCaseType.Count + " items selected";
                    this.updateCaseTypeBindListBOX(listFunded);
                }
            }
            else
            if (clickedBox.Content != null && (clickedBox.Content.Equals("Select All")) && !(clickedBox.IsChecked.Value))
            {

                foreach (var obj2 in caseTypeList)

                {
                    if (!listCaseType.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = false;

                    }

                }
                CaseType.ItemsSource = null;
                CaseTypeText.Foreground = Brushes.Red;
                CaseTypeText.Content = "0 items selected";
                CaseType.ItemsSource = caseTypeList;
                natureOfClaimList = ComboBoxPopulationController.ClearNatureOfClaim();
                NatureText.Content = "0 items selected";
                NatureText.Foreground = Brushes.Red;
                NatureofClaim.ItemsSource = natureOfClaimList;
                listNatureOfClaim = new List<string>();

                EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();

            }
            else
            {
                foreach (var obj2 in caseTypeList)

                {
                    if (!listCaseType.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        if (obj2.CheckStatus)
                        {
                            listCaseType.Add(obj2.Name);
                        }
                    }

                }
                if (listCaseType.Count != 0 && listAction.Count != 0 && listFirm.Count != 0 && listFunded.Count != 0)
                {
                    CaseType.ItemsSource = null;
                    CaseType.ItemsSource = caseTypeList;
                    CaseTypeText.Foreground = Brushes.Green;
                    CaseTypeText.Content = listCaseType.Count + " items selected";
                    this.updateCaseTypeBindListBOX(listFunded);
                }
            }
            RealitazionDataGrid.Visibility = Visibility.Hidden;
        }


        public void updateCaseTypeBindListBOX(List<int> list)
        {

            enh = reportController.getEnhancedPortfolioDiversificationCaseType(listFunded, listFirm, listAction, listCaseType, dateSelected, dateTimeChecked);
              EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();

            natureOfClaimList = ComboBoxPopulationController.AddNatureOfClaimInList(enh);
            NatureofClaim.ItemsSource = natureOfClaimList;
            NatureText.Foreground = Brushes.Red;
            NatureText.Content = "0 items selected";
        }




        //NatureofClaim
        private void NatureofClaimSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NatureofClaimTextChanged(object sender, TextChangedEventArgs e)
        {
            NatureofClaim.ItemsSource = natureOfClaimList.Where(x => x.Name.StartsWith(NatureofClaim.Text.Trim()));
        }

        private void NatureofClaimCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            NatureofClaimBindListBOX(sender);
        }

        private void NatureofClaimBindListBOX(object sender)
        {
            CheckBox clickedBox = (CheckBox)sender;
            if (clickedBox.Content == null)
            {
                return;
            }
            listNatureOfClaim = new List<string>();

            if ((clickedBox.Content.Equals("Select All")) && (clickedBox.IsChecked.Value))

            {

                foreach (var obj2 in natureOfClaimList)

                {
                    if (!listNatureOfClaim.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = true;
                        listNatureOfClaim.Add(obj2.Name);
                    }

                }
                if (listNatureOfClaim.Count != 0)
                {
                    NatureofClaim.ItemsSource = null;
                    NatureofClaim.ItemsSource = natureOfClaimList;
                    NatureText.Foreground = Brushes.Green;
                    NatureText.Content = listNatureOfClaim.Count + " items selected";
                    this.updateNatureofClaimBindListBOX(listFunded);
                }
            }
            else
            if (clickedBox.Content != null && (clickedBox.Content.Equals("Select All")) && !(clickedBox.IsChecked.Value))
            {

                foreach (var obj2 in natureOfClaimList)

                {
                    if (!listNatureOfClaim.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        obj2.CheckStatus = false;

                    }

                }

                NatureofClaim.ItemsSource = null;
                NatureText.Foreground = Brushes.Red;
                NatureText.Content = "0 items selected";
                NatureofClaim.ItemsSource = natureOfClaimList;
                EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();

            }
            else
            {
                foreach (var obj2 in natureOfClaimList)

                {
                    if (!listNatureOfClaim.Contains(obj2.Name) && !obj2.Name.Equals("Select All"))
                    {
                        if (obj2.CheckStatus)
                        {
                            listNatureOfClaim.Add(obj2.Name);
                        }
                    }

                }
                if (listNatureOfClaim.Count != 0 && listCaseType.Count != 0 && listAction.Count != 0 && listFirm.Count != 0 && listFunded.Count != 0)
                {
                    NatureofClaim.ItemsSource = null;
                    NatureofClaim.ItemsSource = natureOfClaimList;
                    NatureText.Foreground = Brushes.Green;
                    NatureText.Content = listNatureOfClaim.Count + " items selected";
                    this.updateNatureofClaimBindListBOX(listFunded);
                }
            }
            RealitazionDataGrid.Visibility = Visibility.Hidden;
        }

        public void updateNatureofClaimBindListBOX(List<int> list)
        {
            enh = reportController.getEnhancedPortfolioDiversificationNatureOfClaim(listFunded, listFirm, listAction, listCaseType,listNatureOfClaim, dateSelected, dateTimeChecked);
            EnhancedDataGrid.ItemsSource = enh;
        }

       

        public void calculateSummary()
        {
            double numberOfCases = 0;
            double cfFirmNetFees = 0;
            double percentOfCollateral = 0;
            double firmPercentOfCollateral = 0;
            if (EnhancedDataGrid.ItemsSource != null && EnhancedDataGrid.Items.Count > 0)
            {
                int count = enh.Count;
                if (!enh[count - 1].BorrowerName.Equals("Total"))
                {
                    foreach (EnhancedPortfolioDiversification row in EnhancedDataGrid.ItemsSource)
                    {

                        numberOfCases += row.NumberOfCases;
                        cfFirmNetFees += row.CfNetFees;
                        firmPercentOfCollateral += row.PercentageOfFirmCollateral;
                        percentOfCollateral += row.PercentOfTotalCollateral;

                    }
               


                EnhancedPortfolioDiversification total = new EnhancedPortfolioDiversification();
                
                total.NumberOfCases = numberOfCases;
                total.PercentageOfFirmCollateral = Math.Round(firmPercentOfCollateral, 3);
                    total.PercentOfTotalCollateral = Math.Round(percentOfCollateral,3);
                total.CfNetFees = Math.Round(cfFirmNetFees, 3);
                total.BorrowerName = "Total";
                enh.Add(total);
                EnhancedDataGrid.ItemsSource = null;
                EnhancedDataGrid.ItemsSource = enh;

                DataTable dt = new DataTable();

                dt.Columns.Add("Number of Cases");
                dt.Columns.Add("CF Net Fees");
                dt.Columns.Add("Percent Of Firm Collateral");
                dt.Columns.Add("Percent Of Total Collateral");
                  
               
                    dt.Rows.Add(String.Format(new System.Globalization.CultureInfo("en-US"), "{0:n0}", numberOfCases), String.Format(new System.Globalization.CultureInfo("en-US"), "{0:C}", Math.Round(cfFirmNetFees, 3)), String.Format(new System.Globalization.CultureInfo("en-US"), "{0:P}", Math.Round(total.PercentageOfFirmCollateral, 3)), String.Format(new System.Globalization.CultureInfo("en-US"), "{0:P}", Math.Round(total.PercentOfTotalCollateral, 3)));
                RealitazionDataGrid.DataContext = dt;
                RealitazionDataGrid.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Nothing to calculate!");
            }

        }



        private void ExportSummaryClick(object sender, RoutedEventArgs e)
        {
            WriteOutExcelFile writeOutExcelFile = new WriteOutExcelFile();

            
                List<EnhancedPortfolioDiversification> temp = (List<EnhancedPortfolioDiversification >)EnhancedDataGrid.ItemsSource;
           

            if (enh != null && enh.Count > 0 && temp.Count != 0)
            {
                bool res = writeOutExcelFile.ExportToReport(enh);
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
                MessageBox.Show("No data to export.");
            }
        }

        private void CalculateTotalsClick(object sender, RoutedEventArgs e)
        {
            this.calculateSummary();
        }


        private void ClearClick(object sender, RoutedEventArgs e)
        {
            this.ClearDropDowns();
            CalculateByDateCheckBox.IsChecked = false;
            DatePicker.IsEnabled = false;

        }

        private void ClearDropDowns()
        {
            EnhancedDataGrid.ItemsSource = new List<EnhancedPortfolioDiversification>();
            RealitazionDataGrid.Visibility = Visibility.Hidden;
            fundedEntityList = ComboBoxPopulationController.ClearFundedEntity();
            FundedText.Content = "0 items selected";
            FundedText.Foreground = Brushes.Red;
            FundedEntity.ItemsSource = fundedEntityList;
            listFunded = new List<int>();
            firmClaimList = ComboBoxPopulationController.ClearBorrower();
            FirmText.Content = "0 items selected";
            FirmText.Foreground = Brushes.Red;
            Firm.ItemsSource = firmClaimList;
            listFirm = new List<int>();
            caseTypeList = ComboBoxPopulationController.ClearCaseType();
            CaseTypeText.Content = "0 items selected";
            CaseTypeText.Foreground = Brushes.Red;
            CaseType.ItemsSource = caseTypeList;
            listAction = new List<string>();
            typeOfActionList = ComboBoxPopulationController.ClearTypeOfAction();
            ActionText.Content = "0 items selected";
            ActionText.Foreground = Brushes.Red;
            TypeOfAction.ItemsSource = typeOfActionList;
            listCaseType = new List<string>();
            natureOfClaimList = ComboBoxPopulationController.ClearNatureOfClaim();
            NatureText.Content = "0 items selected";
            NatureText.Foreground = Brushes.Red;
            NatureofClaim.ItemsSource = natureOfClaimList;
            listNatureOfClaim = new List<string>();
            //CalculateByDateCheckBox.IsChecked = false;
            //DatePicker.IsEnabled = false;
        }

        private void datetimebuttonclick(object sender, RoutedEventArgs e)
        {
            string dateString = DatePicker.SelectedDate.Value.ToString();
            

            if (!DateTime.TryParse(dateString, out dateSelected))
            {
               
                    System.Windows.MessageBox.Show("Incorrect date");

            }
            else
            {
                if(listFunded == null || listFunded.Count == 0)
                {
                    MessageBox.Show("Please select Funded Entites");
                    return;
                }
                if (listFirm == null || listFirm.Count == 0)
                {
                    MessageBox.Show("Please select Firm Entites");
                    return;
                }
                if (listAction == null || listAction.Count == 0)
                {
                    MessageBox.Show("Please select Type of Actions");
                    return;
                }
                if (listCaseType == null || listCaseType.Count == 0)
                {
                    MessageBox.Show("Please select Case Types");
                    return;
                }
                if (listNatureOfClaim == null || listNatureOfClaim.Count == 0)
                {
                    MessageBox.Show("Please select Nature of Claim");
                    return;
                }
               
                enh = reportController.enhancedPortfolioDiversificationsWithDate(listFunded, listFirm, listAction, listCaseType, listNatureOfClaim,dateSelected.Date);
                EnhancedDataGrid.ItemsSource = enh;
            }
        }

        private void EnhancedDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGrid d = sender as DataGrid;
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (col.Header.ToString().Equals("FundedEntityID"))
            {

                //d.Columns.Remove(col);
                col.Visibility = Visibility.Hidden;

            }
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
           

            CheckBox clickedBox = (CheckBox)sender;
            this.ClearDropDowns();
            if (clickedBox.IsChecked.Value)
            {
                DatePicker.IsEnabled = true;
                //DatePickerButton.IsEnabled = true;
                DatePicker.SelectedDate = DateTime.Today;
                dateSelected = DateTime.Today;
                dateTimeChecked = true;
            }
            else
            {

                DatePicker.IsEnabled = false;
               // DatePickerButton.IsEnabled = false;
                dateTimeChecked = false;
                //enh = reportController.getEnhancedPortfolioDiversificationNatureOfClaim(listFunded, listFirm, listAction, listCaseType, listNatureOfClaim);
                //EnhancedDataGrid.ItemsSource = enh;
            }
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {


            CheckBox clickedBox = (CheckBox)sender;
            this.ClearDropDowns();
            if (clickedBox.IsChecked.Value)
            {
                DatePicker.IsEnabled = true;
               // DatePickerButton.IsEnabled = true;
                DatePicker.SelectedDate = DateTime.Today;
                dateSelected = DateTime.Today;
                dateTimeChecked = true;
            }
            else
            {

                DatePicker.IsEnabled = false;
               // DatePickerButton.IsEnabled = false;
                dateTimeChecked = false;
                //enh = reportController.getEnhancedPortfolioDiversificationNatureOfClaim(listFunded, listFirm, listAction, listCaseType, listNatureOfClaim);
                //EnhancedDataGrid.ItemsSource = enh;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ClearDropDowns();
            if(DatePicker.SelectedDate == null)
            {
              

                dateSelected = DateTime.Now;
                DatePicker.SelectedDate = DateTime.Now;
                return;
            }
            string dateString = DatePicker.SelectedDate.Value.ToString();
            if (!DateTime.TryParse(dateString, out dateSelected))
            {

                System.Windows.MessageBox.Show("Incorrect date");
                dateSelected = DateTime.Now;
                DatePicker.SelectedDate = DateTime.Now;
                return;

            }
        }
    }
}
