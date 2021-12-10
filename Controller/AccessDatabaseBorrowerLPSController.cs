using OTS.DataBase;
using OTS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Controller
{
    public class AccessDatabaseBorrowerLPSController
    {
        public AccessDatabaseBorrowerQueriesForLPS accessDatabaseBorrowerQueriesForLPS;

        //Creates a reference to queires for access database borrower table
        public AccessDatabaseBorrowerLPSController()
        {
            accessDatabaseBorrowerQueriesForLPS = new AccessDatabaseBorrowerQueriesForLPS();
        }

        //Gets the Max number of unique database ids
        public string getMaxBorrowerID()
        {
            
            string id = accessDatabaseBorrowerQueriesForLPS.getMaxLPSBorrowerIndex();
            return id;
        }

        //Adds a new borroswer to the database
        public bool addBorrowerToLPS(Borrower newBorrower)
        {
            bool res = accessDatabaseBorrowerQueriesForLPS.addBorrowerToLPS(newBorrower);
            return res;
        }

        //Updates an existing Borower to the database
        public bool updateBorrowerToLPS(Borrower updateBorrower)
        {
            bool res = accessDatabaseBorrowerQueriesForLPS.updateBorrowerToLPS(updateBorrower);
            return res;
        }
        }
}
