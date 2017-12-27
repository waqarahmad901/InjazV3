using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TmsWebApp.Common
{
    public enum EnumUserRole
    {
        SuperAdmin = 1001, 
        Coordinator = 1002,
        Volunteer = 1003,
        Approver1= 1004,
        Approver2 = 1005,
        Approver3 = 1006,
        Participant = 1007,
        Funder = 1008
    }

    public enum SessionStatus
    {
        Pending,
        Approved,
        Rejected,
        DateChanges,
        StudentChanges,
        Occured,
        Complete
    }

    public enum CertificateEnum
    {
        NameOfStudent,
        ProgrammYear,
        NameOfVolunteer,
        TranningHour,
        NameOfCoordinator,
        TranningDate 
    }
}