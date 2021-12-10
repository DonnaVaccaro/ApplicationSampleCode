using System.ComponentModel.DataAnnotations;

namespace CaseManager.Model
{
    public class EnhancedPortfolioDiversification
    {
        private string borrowerName;
        private string natureOfClaim;
        private double numberOfCases;
        private string fundedEntity;
        private double cfNetFees;
        private double percentageOfFirmCollateral;
        private double percentOfTotalCollateral;
        private int fundedEntityID;
        private string caseType;
        private string typeOfAction;
        private int borrowerID;



        public EnhancedPortfolioDiversification()
        {
            borrowerName = "";
            natureOfClaim = "";
            fundedEntity = "";
            caseType = "";
            typeOfAction = "";
            numberOfCases = 0;
            cfNetFees = 0;
            percentageOfFirmCollateral = 0;
            percentOfTotalCollateral = 0;
            fundedEntityID = 0;
            borrowerID = 0;
        }

        public string BorrowerName { get => borrowerName; set => borrowerName = value; }
        public string NatureOfClaim { get => natureOfClaim; set => natureOfClaim = value; }
        public double NumberOfCases { get => numberOfCases; set => numberOfCases = value; }
        public string FundedEntity { get => fundedEntity; set => fundedEntity = value; }

        [System.ComponentModel.DataAnnotations.DataType(DataType.Currency)]
        public double CfNetFees { get => cfNetFees; set => cfNetFees = value; }
        public double PercentageOfFirmCollateral { get => percentageOfFirmCollateral; set => percentageOfFirmCollateral = value; }
        public double PercentOfTotalCollateral { get => percentOfTotalCollateral; set => percentOfTotalCollateral = value; }
        public string CaseType { get => caseType; set => caseType = value; }
        public string TypeOfAction { get => typeOfAction; set => typeOfAction = value; }
        public int FundedEntityID { get => fundedEntityID; set => fundedEntityID = value; }
        public int BorrowerID { get => borrowerID; set => borrowerID = value; }
       
    }
}
