using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using OTS.HelperClasses;
using System.Linq;

namespace OTS.Controller
{
    public class BorrowerController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BorrowerController));
        private readonly SQLBorrowerQueries sQLBorrowerQueries;
       
        private Borrower borrower;

        public BorrowerController()
        {
            log.Info("Constructor Borrower Controller");
            borrower = new Borrower();
            sQLBorrowerQueries = new SQLBorrowerQueries();
           
        }

        //Calls the update borrower SQL query to update a borrower
        public bool updateBorrower(Borrower borrower)
        {
            log.Info("Update Borrower");
            if (borrower != null)
            {
                 bool re = sQLBorrowerQueries.udateBorrower(borrower);
                return true;
            }
            else
            {
                log.Error("update borrower and borrower is null");
                return false;
            }


        }

        //Calls the search borrowser by name SQL query to search for borrowers
        // that are like the borrower name.
        public List<Borrower> searchBorrowerName(string name)
        {
            log.Info("Search Borrower by Name");
            if (name == null)
            {
                log.Error("search borrower name, name is null");
                return new List<Borrower>();
            }
            if (name.Trim().Equals(""))
            {
                return new List<Borrower>();
            }
            List<Borrower> borrowerSearchList = sQLBorrowerQueries.searchBorrowerName(name);
            if (borrowerSearchList == null)
            {
                log.Error("borrower search list is null");
                return new List<Borrower>();
            }
            Comparison<Borrower> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            borrowerSearchList.Sort(comparison);
            return borrowerSearchList;
        }

        //Calls the get all borrower SQL query to select all borrowers.
        public List<Borrower> getAllBorrowers()
        {
            log.Info("Get a list of all Borrowers");
            List<Borrower> allBorrowerList = sQLBorrowerQueries.getAllBorrowers();
            if (allBorrowerList == null)
            {
                log.Error("all borrower list is null");
                return new List<Borrower>();
            }
            Comparison<Borrower> comparison = (x, y) => String.Compare(x.BorrowerName, y.BorrowerName);
            allBorrowerList.Sort(comparison);
            return allBorrowerList;
        }

        //Calls the add new borrower SQL to add a new borrower
        //Also looks up borrowers with similar names and addres 1
        //prompts the user with what it finds and verifys if they still want to add
        public bool addBorrowerWithID(Borrower borrower)
        {
            log.Info("add new Borrowers");
            bool addNewBorrowerResult = true;
            string messageBox;
            bool res = true;
            Borrower b = this.getBorrower();
            if (b == null)
            {
                log.Error("Trying to add a new borrower but the borrower is null");
                return false;
            }
            if (b.BorrowerName == null)
            {
                log.Error("Trying to add a new borrower but the borrower name is null");
                return false;
            }
            if (b.Address1 == null)
            {
                log.Error("Trying to add a new borrower but the borrower Address1 is null");
                return false;
            }
            List<Borrower> searchBorrowerByNameAddressList = sQLBorrowerQueries.searchBorrowerNameAddress(b.BorrowerName, b.Address1);
            if (searchBorrowerByNameAddressList == null)
            {
                log.Error("search borrower by name address list is null");
                return false;
            }

            if (searchBorrowerByNameAddressList != null && searchBorrowerByNameAddressList.Count >= 1)
            {
                string title = "Found a possible match in the database.";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxResult result;
                messageBox = "";

                for (int i = 0; i < searchBorrowerByNameAddressList.Count; i++)
                {
                    if (i <= 20)
                    {
                        string name = searchBorrowerByNameAddressList[i].BorrowerName;
                        string address = searchBorrowerByNameAddressList[i].Address1;
                        messageBox = messageBox + Environment.NewLine + "Borrower Name: " + name + "      Street Address 1: " + address;
                    }
                }

                messageBox = messageBox + (Environment.NewLine + Environment.NewLine + "Do you want to proceed with adding a new Borrower?");
                result = MessageBox.Show(messageBox, title, buttons);
                if (result == MessageBoxResult.Yes)
                {
                    //do nothing

                }
                else
                {
                    return false;
                }
            }

            bool ress = sQLBorrowerQueries.IdentityOn();

            res = sQLBorrowerQueries.addExistingBorrower(borrower);


            bool resss = sQLBorrowerQueries.IdentityOff();
           
            return addNewBorrowerResult;
        }

        public List<Borrower> addAllBorrower(List<Borrower> list)
        {
            log.Info("add new Borrowers");
            bool res = true;
            List<Borrower> failInsert = new List<Borrower>();
            bool ress = sQLBorrowerQueries.IdentityOn();
            //if (!ress)
            //{
            //    MessageBox.Show("Identity On returned false");
            //    return null;
            //}
            for (int i = 0; i < list.Count; i++)
            {
                res = sQLBorrowerQueries.addExistingBorrower(list[i]);
                if (!res)
                {
                    failInsert.Add(list[i]);
                }
            }
            bool resss = sQLBorrowerQueries.IdentityOff();
            return failInsert;
        }

        //public bool addBorrowerWithID(Borrower borrower)
        //{
        //    log.Info("add new Borrowers");
        //    bool res = true;
          

        //    bool ress = sQLBorrowerQueries.IdentityOn();
           
        //        res = sQLBorrowerQueries.addExistingBorrower(borrower);
               
          
        //    bool resss = sQLBorrowerQueries.IdentityOff();
        //    return res;
        //}

        public string stripZip(string temp)
        {
            if (temp == null)
            {
                return "";
            }
            temp = temp.Trim(new Char[] { '-', '_' });
            temp = temp.Replace("-", "");
            return temp;
        }

        public string stripPhone(string temp)
        {
            if (temp == null)
            {
                return "";
            }
            temp = temp.Replace("-", "");
            temp = temp.Replace("_", "");
            temp = temp.Replace("(", "");
            temp = temp.Replace(")", "");
            temp = temp.Replace(" ", "");


            return temp;
        }

        public void setBorrower(Borrower borrower)
        {
            this.borrower = borrower;
        }

        public Borrower getBorrower()
        {
            return this.borrower;
        }

        public bool checkZipCodeLength(string title, string value)
        {
            log.Info("check zip for length");
            if (value == null)
            {
                log.Error("value is null");
                return false;
            }
            value = this.stripZip(value);
            if (value.Equals(""))
            {
                return true;
            }
            string temp;
            int num;
            temp = value;
            temp = temp.Trim(new Char[] { '-', '_' });
            temp = temp.Replace("-", "");
            if (!(temp.Length == 5 | temp.Length == 9))
            {
                System.Windows.MessageBox.Show("Please enter either 5 or 9 digits for zip code");
                return false;
            }
            bool res = int.TryParse(temp, out num);
            if (res == false)
            {
                System.Windows.MessageBox.Show("Please enter zip code in the correct format.");
                return false;
            }
            return true;
        }


        public bool checkPhoneLength(string title, string value)
        {
            log.Info("check phone for length");
            if (value == null)
            {
                log.Error("value is null");
                return false;
            }
            string temp;
            int num;
            
            temp = this.stripPhone(value);
            if (temp.Equals(""))
            {
                return true;
            }
            if (temp.Length != 10)
            {
                System.Windows.MessageBox.Show("Please enter either 10 digits for the phone number.");
                return false;
            }
           
            return true;
        }

        public bool checkDateTime(string title, string value)
        {
            log.Info("check date time");
            if (value == null)
            {
                log.Error("value is null");
                System.Windows.MessageBox.Show("When Status is Funded, Customer Orig Date is required.");
                return false;
            }
           
            if (value.Equals(""))
            {
                System.Windows.MessageBox.Show("When Status is Funded, Customer Orig Date is required.");
                return false;
            }
            bool res = Utilities.checkDateRange(title,value);
            if (!res)
            {
                return false;
            }

            return true;
        }

        public bool checkRequiredValue(string title, string value)
        {
            log.Info("Checking to make sure requried values are filled in for adding a borrower");
            bool result = true;

            if (value == null || value.Trim().Equals(""))
            {
                System.Windows.MessageBox.Show("Please enter " + title);
                result = false;
            }
            return result;
        }

        public Borrower getBorrowerById(string borrowerId)
        {
            log.Info("Searching for a borrower by borrower id");
            if (borrowerId == null)
            {
                log.Error("borrower id is null");
                return new Borrower();
            }
            Borrower borrower = new Borrower();

            borrower.BorrowerId = borrowerId;
            List<Borrower> borrowerByBorrowerIDList = sQLBorrowerQueries.getBorrowerByBorrowerID(borrower);

            if (borrowerByBorrowerIDList != null && borrowerByBorrowerIDList.Count == 1)
            {
                borrower = borrowerByBorrowerIDList[0];
            }

            return borrower;
        }

        public string selectsMostRecentInsertIdentity()
        {
            string id = sQLBorrowerQueries.selectsMostRecentInsertIdentity();
            return id;
        }

        public string LookupBorrowerName(string name)
        {

            string myKey = SQLBorrowerQueries.borrowerNameIdLookupTable.FirstOrDefault(x => x.Value == name).Key;
            return myKey;
        }

        public string getMaxCMBorrowerIndex()
        {
            string id = sQLBorrowerQueries.getMaxCMBorrowerIndex();
            return id;
        }

        }
}
