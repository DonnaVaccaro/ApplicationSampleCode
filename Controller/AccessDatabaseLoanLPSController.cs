using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Controller
{
    public class AccessDatabaseLoanLPSController
    {
        public AccessDatabaseLoanQueriesForLPS accessDatabaseLoanQueriesForLPS;

        //Creates a reference to the access database loan talbe
        public AccessDatabaseLoanLPSController()
        {
            accessDatabaseLoanQueriesForLPS = new AccessDatabaseLoanQueriesForLPS();

        }

        //Gets the Max number of unique database ids
        public string getMaxLoanID()
        {

            string temp = accessDatabaseLoanQueriesForLPS.getMaxLoanID();
            return temp;
        }

        //Adds a new loan to the database
        public bool addLoanToLPS(Loan newLoan)
        {
            bool res = accessDatabaseLoanQueriesForLPS.addLoanToLPS(newLoan);
            return res;
        }

        //Updates an existing loan to the database
        public bool updateLoanToLPS(Loan updateLoan)
        {
            bool res = accessDatabaseLoanQueriesForLPS.updateLoanToLPS(updateLoan);
            return res;

        }

        //Adds a new entry in the loan mod database table
        public bool addLoanModificationToLPS(LoanModification newLoanModification)
        {
            bool res = accessDatabaseLoanQueriesForLPS.addLoanModificationToLPS(newLoanModification);
            return res;
        }
        }
}
