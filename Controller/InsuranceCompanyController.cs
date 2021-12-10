using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OTS.Controller
{
    public class InsuranceCompanyController
    {

        public static List<InsuranceCompany> insuranceCompanyList;
        public static Dictionary<string, string> insuranceCompanyListLookUp;
        private static InsuranceCompanyQueries insuranceCompanyQueries;
        public static int defaultInsIndex;
         public static List<string> insuranceCompanyListDisplay;


        public InsuranceCompanyController()
        {
            insuranceCompanyQueries = new InsuranceCompanyQueries();
          
        }
        public void initialize()
        {
            insuranceCompanyList = getInsuranceCompany();
            insuranceCompanyListLookUp = getInsuranceCompanyLookUp();
            defaultInsIndex = getInsuranceDefaultIndex();
            insuranceCompanyListDisplay = getInsuranceCompanyDisplay();
        }
        public static List<string> getInsuranceCompanyDisplay()
        {

            insuranceCompanyListDisplay = new List<string>();
            foreach (InsuranceCompany ins in insuranceCompanyList)
            {
                insuranceCompanyListDisplay.Add(ins.Description);
            }


            return insuranceCompanyListDisplay;
        }

        public static int getInsuranceDefaultIndex()
        {
            string res;
            int intValue;
            res = getInsuranceCompanyLookUp().FirstOrDefault(x => x.Value == "NA").Key;
            int.TryParse(res, out intValue);
            return intValue;
        }


        //Creates static list to populate ui drop down values
        public static List<InsuranceCompany> getInsuranceCompany()
        {
            insuranceCompanyList = insuranceCompanyQueries.createInsuranceCompanyList();

            return insuranceCompanyList;
        }

        public static Dictionary<string, string> getInsuranceCompanyLookUp()
        {
            insuranceCompanyListLookUp = new Dictionary<string, string>();
            insuranceCompanyList = getInsuranceCompany();
            foreach (InsuranceCompany ins in insuranceCompanyList)
            {
                if (!insuranceCompanyListLookUp.ContainsKey(ins.Id))
                {
                    insuranceCompanyListLookUp.Add(ins.Id, ins.Description);
                }
            }


            return insuranceCompanyListLookUp;
        }

       
    }
}
