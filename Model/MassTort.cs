using System;
using System.Collections.Generic;
using System.Text;

namespace OTS.Model
{
    public class MassTort
    {

        private string massTortID;
        private string loanID;
        private string caseID;
        private string caseType;
        private string dateOfBirth;
        private string gender;
        private string usCitizen;
        private string drugProductName;
        private string productID;
        private string proofOfUseProductIDExposure;
        private string dateProofOfUseProductIDExposure;
        private string durationUsageImplantExposureMonths;
        private string solExpirationDate;
        private string medicalRecords;
        private string imagingPhotographicEvidence;
        private string diagnosis;
        private string ageAtDiagnosis;
        private string daysHospitalized;
        private string treatment;
        private string totalNumberOfSurgeries;
        private string dateOfFirstSurgery;
        private string extraordinaryInjury;
        private string riskFactorsAndOtherAdjustments;
        private string defendant2;
        private string defendant3;
        private string cbfFeePercent;
        private string cbfFeeDollar;
        private string attorneyFeeForDistribution;
        private string notes;
        private string userID;
        private string massTortCreatedDate;
        private string massTortModifiedDate;
        private string borrowerCaseID;
        private string plaintiff;



        public MassTort()
        {
            this.massTortID = "0";
            this.loanID = "0";
            this.caseID = "0";
            this.caseType = OTS.Controller.ComboBoxPopulationController.defaultNA;
            this.dateOfBirth = "";
            this.gender = "NA";
            this.usCitizen = "NA";
            this.drugProductName = "";
            this.productID = "";
            this.proofOfUseProductIDExposure = "NA";
            this.dateProofOfUseProductIDExposure = "";
            this.durationUsageImplantExposureMonths = "0";
            this.solExpirationDate = "";
            this.medicalRecords = "NA";
            this.imagingPhotographicEvidence = "NA";
            this.diagnosis = "";
            this.ageAtDiagnosis = "";
            this.daysHospitalized = "";
            this.treatment = "";
            this.totalNumberOfSurgeries = "0";
            this.dateOfFirstSurgery = "";
            this.extraordinaryInjury = "";
            this.riskFactorsAndOtherAdjustments = "";
            this.defendant2 = "";
            this.defendant3 = "";
            this.cbfFeePercent = "";
            this.cbfFeeDollar = "";
            this.attorneyFeeForDistribution = "";
            this.notes = "";
            this.userID = "";
            this.massTortCreatedDate = "";
            this.massTortModifiedDate = "";
        }

       
           
      
        public string MassTortID { get => massTortID; set => massTortID = value; }
        public string LoanID { get => loanID; set => loanID = value; }
        public string CaseID { get => caseID; set => caseID = value; }
        public string CaseType { get => caseType; set => caseType = value; }
        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Gender { get => gender; set => gender = value; }
        public string UsCitizen { get => usCitizen; set => usCitizen = value; }
        public string DrugProductName { get => drugProductName; set => drugProductName = value; }
        public string ProductID { get => productID; set => productID = value; }
        public string ProofOfUseProductIDExposure { get => proofOfUseProductIDExposure; set => proofOfUseProductIDExposure = value; }
        public string DateProofOfUseProductIDExposure { get => dateProofOfUseProductIDExposure; set => dateProofOfUseProductIDExposure = value; }
        public string DurationUsageImplantExposureMonths { get => durationUsageImplantExposureMonths; set => durationUsageImplantExposureMonths = value; }
        public string SolExpirationDate { get => solExpirationDate; set => solExpirationDate = value; }
        public string MedicalRecords { get => medicalRecords; set => medicalRecords = value; }
        public string ImagingPhotographicEvidence { get => imagingPhotographicEvidence; set => imagingPhotographicEvidence = value; }
        public string Diagnosis { get => diagnosis; set => diagnosis = value; }
        public string AgeAtDiagnosis { get => ageAtDiagnosis; set => ageAtDiagnosis = value; }
        public string DaysHospitalized { get => daysHospitalized; set => daysHospitalized = value; }
        public string Treatment { get => treatment; set => treatment = value; }
        public string TotalNumberOfSurgeries { get => totalNumberOfSurgeries; set => totalNumberOfSurgeries = value; }
        public string DateOfFirstSurgery { get => dateOfFirstSurgery; set => dateOfFirstSurgery = value; }
        public string ExtraordinaryInjury { get => extraordinaryInjury; set => extraordinaryInjury = value; }
        public string RiskFactorsAndOtherAdjustments { get => riskFactorsAndOtherAdjustments; set => riskFactorsAndOtherAdjustments = value; }
        public string Defendant2 { get => defendant2; set => defendant2 = value; }
        public string Defendant3 { get => defendant3; set => defendant3 = value; }
        public string CbfFeePercent { get => cbfFeePercent; set => cbfFeePercent = value; }
        public string CbfFeeDollar { get => cbfFeeDollar; set => cbfFeeDollar = value; }
        public string AttorneyFeeForDistribution { get => attorneyFeeForDistribution; set => attorneyFeeForDistribution = value; }
        public string Notes { get => notes; set => notes = value; }
        public string UserID { get => userID; set => userID = value; }
        public string MassTortCreatedDate { get => massTortCreatedDate; set => massTortCreatedDate = value; }
        public string MassTortModifiedDate { get => massTortModifiedDate; set => massTortModifiedDate = value; }
        public string BorrowerCaseID { get => borrowerCaseID; set => borrowerCaseID = value; }
        public string Plaintiff { get => plaintiff; set => plaintiff = value; }
    }
}
