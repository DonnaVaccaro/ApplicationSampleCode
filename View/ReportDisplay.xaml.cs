using CaseManager.HelperClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OTS.View
{
    /// <summary>
    /// Interaction logic for ReportDisplay.xaml
    /// </summary>
    public partial class ReportDisplay : Window
    {
        public ReportDisplay()
        {
            InitializeComponent();
        }

        public Window getRealizationRatesReportPage()
        {

            return this;
        }

        public void setDataGrid(DataTable table)
        {
           
            ReportDisplayDataGrid.ItemsSource = table.DefaultView;
            

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
