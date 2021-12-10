using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class InsuranceCompany
    {

        private string id;
        private string description;
        private string parentCompany;


        public InsuranceCompany(string id, string description, string parentCompany)
        {
            this.Id = id;
            this.Description = description;
            this.ParentCompany = parentCompany;
        }

        public string Id { get => id; set => id = value; }
        public string Description { get => description; set => description = value; }
        public string ParentCompany { get => parentCompany; set => parentCompany = value; }
    }
}
