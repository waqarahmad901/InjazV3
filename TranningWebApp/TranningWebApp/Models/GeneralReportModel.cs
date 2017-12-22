using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranningWebApp.Models
{
    public class GeneralReportModel
    {

        public int SchoolCount { get; set; }
        public int StudentCount { get; set; }
        public int MaleStudentCount { get; set; }
        public int FemaleStudentCount { get; set; }
        public int VolunteerCount { get; set; }
        public int SessionCount { get; set; }
        public int PrimarySchoolCount { get; set; }
        public int MiddleSchoolCount { get; set; }
        public int SeconderySchoolCount { get; set; }
        public int PartnerCount { get; set; }
        public int CoordinatorCount { get; set; }
        public int CertificateCount { get; set; }
        public int EvaluationPreCount { get; set; }
        public int EvaluationPostCount { get; set; }
        public int CitiesCount { get; set; }
    }
}