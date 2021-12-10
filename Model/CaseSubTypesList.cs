using System;
using System.Collections.Generic;
using System.Text;

namespace CaseManager.Model
{
    public class CaseSubTypesList
    {
        private string caseSubtypesId;
        private string nature_Of_Claim;
        private string caseType;
        private string typeOfAction;

        public CaseSubTypesList()
        {

        }

        public CaseSubTypesList(string caseSubtypesId, string nature_Of_Claim, string caseType, string typeOfAction)
        {
            this.caseSubtypesId = caseSubtypesId;
            this.nature_Of_Claim = nature_Of_Claim;
            this.caseType = caseType;
            this.typeOfAction = typeOfAction;
        }

        public string CaseSubtypesId { get => caseSubtypesId; set => caseSubtypesId = value; }
        public string Nature_Of_Claim { get => nature_Of_Claim; set => nature_Of_Claim = value; }
        public string CaseType { get => caseType; set => caseType = value; }
        public string TypeOfAction { get => typeOfAction; set => typeOfAction = value; }
    }
}
