using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Models;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class DashBoardRepositry
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public DashBoardModel GetDashBoardData()
        {
            DashBoardModel model = new DashBoardModel();

            var sessions = Context.sessions.ToList();
            model.SessionActiveCount = sessions.Where(x => x.Status == "Approved").Count();
            model.SessionPendingCount = sessions.Where(x => x.Status == "Pending").Count();

            var volunteer = Context.volunteer_profile.ToList();
            model.volunteerAvailableCount = volunteer.Where(x => x.IsApprovedAtLevel1 != null && x.IsApprovedAtLevel1.Value && x.IsApprovedAtLevel2 != null && x.IsApprovedAtLevel2.Value && x.IsApprovedAtLevel3 != null && x.IsApprovedAtLevel3.Value).Count();
            model.volunteerPendingCount = volunteer.Count - model.volunteerAvailableCount;

            var participant = Context.participant_profile.ToList();
            model.ParticipantEnrolledCount = participant.Count;
            model.ParticipantCertificateCount = participant.Sum(y => y.session_participant.Where(x => x.IsCertificateGenerated != null && x.IsCertificateGenerated.Value).Count());

            var school = Context.schools.ToList();
            model.SchoolApprovedCount = school.Where(x=>x.Status == "Approved").Count();
            model.SchoolPendingCount = school.Where(x => x.Status == "Pending").Count();

            model.Sessions = sessions.Where(x => x.Status == "Approved").Select(x => new SessionClander
                                {
                                        title = x.ProgramName,
                                        color = "#4dbd74",
                                        start = x.ActualDateTime.Value.ToString("yyy-MM-dd"),
                                        url = "/Session/Edit/" + x.RowGUID
                                        
            }).ToList();

            model.Sessions.AddRange(sessions.Where(x => x.Status == "Occured").Select(x => new SessionClander
            {
                title = x.ProgramName,
                color = "#11a3eb",
                start = x.ActualDateTime.Value.ToString("yyy-MM-dd"),
                url = "/Session/Edit/" + x.RowGUID
            }).ToList());


            return model;
        }
    }
}