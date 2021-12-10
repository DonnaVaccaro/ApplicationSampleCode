using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class Borrower
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Borrower));
        private string borrowerId;
        private string borrowerName;
        private string borrowerNameLine2;
        private string address1;
        private string address2;
        private string address3;
        private string city;
        private string state;
        private string zip;
        private string notes;
        private string email1;
        private string email2;
        private string email3;
        private string email4;
        private string phoneNumber;
        private string primarycontact;
        private string autopay;
        
        private string customersOrigDate;
        private string userId;
        private string reasonForUpdate;
        private string createdDate;
        private string modifiedDate;


        public Borrower()
        {
            log.Info("Borrower constructor");
            this.borrowerName = "";
            this.borrowerNameLine2 = "";
            this.address1 = "";
            this.address2 = "";
            this.address3 = "";
            this.city = "";
            this.state = "";
            this.zip = "";
            this.email1 = "";
            this.email2 = "";
            this.email3 = "";
            this.email4 = "";
            this.phoneNumber = "";
            this.primarycontact = "";
            this.autopay = "";
            this.customersOrigDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            this.userId = "";
            this.notes = "";
            this.reasonForUpdate = "Added a new Borrower";
            this.createdDate = 
            this.modifiedDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

        }


        public string BorrowerId { get => borrowerId; set => borrowerId = value; }
        public string BorrowerName { get => borrowerName; set => borrowerName = value; }
        public string BorrowerNameLine2 { get => borrowerNameLine2; set => borrowerNameLine2 = value; }
        public string Address1 { get => address1; set => address1 = value; }
        public string Address2 { get => address2; set => address2 = value; }
        public string Address3 { get => address3; set => address3 = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string Zip { get => zip; set => zip = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Primarycontact { get => primarycontact; set => primarycontact = value; }
        public string Email1 { get => email1; set => email1 = value; }
        public string Email2 { get => email2; set => email2 = value; }
        public string Email3 { get => email3; set => email3 = value; }
        public string Email4 { get => email4; set => email4 = value; }
        public string Autopay { get => autopay; set => autopay = value; }
        public string CustomersOrigDate { get => customersOrigDate; set => customersOrigDate = value; }
       
        public string Notes { get => notes; set => notes = value; }

        public string UserId { get => userId; set => userId = value; }
        public string ReasonForUpdate { get => reasonForUpdate; set => reasonForUpdate = value; }
        public string CreatedDate { get => createdDate; set => createdDate = value; }
        public string ModifiedDate { get => modifiedDate; set => modifiedDate = value; }
    }
}
