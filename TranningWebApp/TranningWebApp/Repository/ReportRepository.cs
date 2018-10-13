using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Models;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class ReportRepository
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();
        public SchoolParticipantModel[] GetSchoolParticipants(string fromDate, string toDate, string city)
        {
            DateTime dateFrom = fromDate == "" ? DateTime.Now : DateTime.Parse(fromDate);
            DateTime dateto = toDate == "" ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.participant_profile.Where(x =>
            (string.IsNullOrEmpty(city) || x.school.City == city) &&
            (string.IsNullOrEmpty(fromDate) || x.school.CreatedAt >= dateFrom) &&
            (string.IsNullOrEmpty(toDate) || x.school.CreatedAt < dateto)
            )

            .GroupBy(x => x.SchoolId).Select(x =>
            new SchoolParticipantModel
            {
                SchoolName = x.FirstOrDefault().school.SchoolName,
                Count = x.Count()
            }).ToArray();
            return result;

        }

        public SessionParticipantModel[] GetSessionParticipants(string fromDate, string toDate)
        {
            DateTime dateFrom = fromDate == "" ? DateTime.Now : DateTime.Parse(fromDate);
            DateTime dateto = toDate == "" ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.session_participant.Where(x =>
            (string.IsNullOrEmpty(fromDate) || x.session.CreatedAt >= dateFrom) &&
            (string.IsNullOrEmpty(toDate) || x.session.CreatedAt < dateto)
            )

            .GroupBy(x => x.SessionID).Select(x =>
            new SessionParticipantModel
            {
                ProgramName = x.FirstOrDefault().session.ProgramName,
                Count = x.Count()
            }).ToArray();
            return result;

        }
        public PartnerFundingModel GetPartnerFunding()
        {
            var result = Context.funder_profile.ToList();
            PartnerFundingModel model = new PartnerFundingModel();
            model.FinancialCount = result.Where(x => x.TypeOfFunding == "Finanical").Count();
            model.InKindCount = result.Where(x => x.TypeOfFunding == "InKind").Count();
            return model;
        }

        public PartnerTypeModel GetPartnerType()
        {

            var result = Context.funder_profile.ToList();
            PartnerTypeModel model = new PartnerTypeModel();
            model.FunderCount = result.Where(x => x.PartnerType == "Funders").Count();
            model.VolunteeringCount = result.Where(x => x.PartnerType == "Volunteering").Count();
            model.StrategicCount = result.Where(x => x.PartnerType == "Stragetic").Count();
            return model;
        }

        public VolunteerDetailModel GetVolunteerDetail()
        {

            var result = Context.sessions.ToList();
            VolunteerDetailModel model = new VolunteerDetailModel();
            model.ParticipatedCount = result.Where(x => (x.VolunteerId != null && x.MarkedCompletedByVolunteer == true)).Count();
            model.NotParticipatedCount = result.Where(x => (x.VolunteerId != null && x.MarkedCompletedByVolunteer == false)).Count();
            return model;
        }

        public TotalSchoolParticipantModel GetTotalSchoolParticipants(string fromDate, string toDate, string city)
        {
            DateTime dateFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Now.AddMonths(-1) : DateTime.Parse(fromDate);
            DateTime dateto = string.IsNullOrEmpty(toDate) ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.participant_profile.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto) &&
            (string.IsNullOrEmpty(city) || x.school.City == city))
            .ToList();
            TotalSchoolParticipantModel model = new TotalSchoolParticipantModel();
            model.SecondaryMaleCount = result.Where(x =>
            (x.school.TypeOfSchool == "Male") &&
            (x.school.StageOfSchool.Trim() == "Secondary")).Count();

            model.SecondaryFemaleCount = result.Where(x =>
            (x.school.TypeOfSchool == "Female") &&
            (x.school.StageOfSchool.Trim() == "Secondary")).Count();

            model.TotalSecondaryCount = model.SecondaryMaleCount + model.SecondaryFemaleCount;

            model.MiddleMaleCount = result.Where(x =>
            (x.school.TypeOfSchool == "Male") &&
            (x.school.StageOfSchool.Trim() == "Middle")).Count();

            model.MiddleMaleCount = result.Where(x =>
            (x.school.TypeOfSchool == "Female") &&
            (x.school.StageOfSchool.Trim() == "Middle")).Count();

            model.TotalMiddleCount = model.MiddleMaleCount + model.MiddleMaleCount;

            model.PrimaryMaleCount = result.Where(x =>
            (x.school.TypeOfSchool == "Male") &&
            (x.school.StageOfSchool.Trim() == "Primary")).Count();

            model.PrimaryFemaleCount = result.Where(x =>
            (x.school.TypeOfSchool == "Female") &&
            (x.school.StageOfSchool.Trim() == "Primary")).Count();

            model.TotalPrimaryCount = model.PrimaryMaleCount + model.PrimaryFemaleCount;

            return model;
        }


        public TotalSessionInSchoolParticipantModel GetTotalSessionInSchoolParticipants(string fromDate, string toDate, string city)
        {
            DateTime dateFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Now.AddMonths(-1) : DateTime.Parse(fromDate);
            DateTime dateto = string.IsNullOrEmpty(toDate) ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.sessions.Include("session_participant").Where(x =>
            (x.ActualDateTime >= dateFrom) &&
            (x.ActualDateTime < dateto) &&
            (x.Status == "Occured") &&
            (string.IsNullOrEmpty(city) || x.school.City == city))
            .Select(x => new
            {
                TypeOfSchool = x.school.TypeOfSchool.Trim(),
                StageOfSchool = x.school.StageOfSchool.Trim(),
                participantCount = x.session_participant.Count()
            })
            .ToList();
            TotalSessionInSchoolParticipantModel model = new TotalSessionInSchoolParticipantModel();
            model.SecondaryMaleCount = result.Where(x =>
            (x.TypeOfSchool == "Male") &&
            (x.StageOfSchool == "Secondary"))
            .Sum(x => x.participantCount);

            model.SecondaryFemaleCount = result.Where(x =>
            (x.TypeOfSchool == "Female") &&
            (x.StageOfSchool == "Secondary")).Sum(x => x.participantCount);


            model.TotalSecondaryCount = model.SecondaryMaleCount + model.SecondaryFemaleCount;

            model.MiddleMaleCount = result.Where(x =>
            (x.TypeOfSchool == "Male") &&
            (x.StageOfSchool == "Middle")).Sum(x => x.participantCount);


            model.MiddleFemaleCount = result.Where(x =>
            (x.TypeOfSchool == "Female") &&
            (x.StageOfSchool == "Middle")).Sum(x => x.participantCount);


            model.TotalMiddleCount = model.MiddleMaleCount + model.MiddleFemaleCount;

            model.PrimaryMaleCount = result.Where(x =>
            (x.TypeOfSchool == "Male") &&
            (x.StageOfSchool == "Primary")).Sum(x => x.participantCount);


            model.PrimaryFemaleCount = result.Where(x =>
            (x.TypeOfSchool == "Female") &&
            (x.StageOfSchool == "Primary")).Sum(x => x.participantCount);


            model.TotalPrimaryCount = model.PrimaryMaleCount + model.PrimaryFemaleCount;

            return model;
        }

        public TotalCourseModel GetTotalCourse(string YearSelected)
        {
            DateTime dateFrom = string.IsNullOrEmpty(YearSelected) ? DateTime.Now : DateTime.Parse(YearSelected);
            var result = Context.sessions.Where(x => x.CreatedAt.Year == dateFrom.Year).ToList();
            TotalCourseModel model = new TotalCourseModel();

            model.SubmittedCount = result.Where(x => x.Status == "Occured").Count();
            model.OutStandingCount = result.Where(x => x.Status == "Pending").Count();
            model.DateChangedCount = result.Where(x => x.Status == "DateChanged").Count();
            return model;

        }
        public StudentsWithVolunteerModel[] GetStudentsWithVolunteer(string fromDate, string toDate)
        {
            DateTime dateFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Now : DateTime.Parse(fromDate);
            DateTime dateto = string.IsNullOrEmpty(toDate) ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.session_participant.Where(x =>
            (x.session.CreatedAt >= dateFrom) &&
            (x.session.CreatedAt < dateto)
            )
              .GroupBy(x => x.session.VolunteerId).Select(x =>
            new StudentsWithVolunteerModel
            {
                VolunteerName = x.FirstOrDefault().session.volunteer_profile.VolunteerName,
                Count = x.Count()
            }).ToArray();
            return result;

        }

        public NumberOfVolunteerSessionModel[] GetNumberOfVolunteerSession(string fromDate, string toDate)
        {
            DateTime dateFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Now : DateTime.Parse(fromDate);
            DateTime dateto = string.IsNullOrEmpty(toDate) ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.sessions.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto)
            )
              .GroupBy(x => x.VolunteerId).Select(x =>
            new NumberOfVolunteerSessionModel
            {
                VolunteerName = x.FirstOrDefault().volunteer_profile.VolunteerName,
                SessionCount = x.Count()
            }).ToArray();
            return result;

        }
        public TotalCourseByStageDataModel GetTotalCourseByStage(string fromDate, string toDate)
        {
            DateTime dateFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Now : DateTime.Parse(fromDate);
            DateTime dateto = string.IsNullOrEmpty(toDate) ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            var result = Context.sessions.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto)
            ).ToList();

            TotalCourseByStageDataModel model = new TotalCourseByStageDataModel();
            model.SecondaryCount = result.Where(x => x.school != null && x.school.StageOfSchool == "Secondary").Count();
            model.MiddleCount = result.Where(x => x.school != null && x.school.StageOfSchool == "Middle").Count();
            model.PrimaryCount = result.Where(x => x.school != null && x.school.StageOfSchool == "Primary").Count();
            return model;

        }

        public GeneralReportModel GetGeneralReport(string fromDate, string toDate, string city)
        {
            DateTime dateFrom = string.IsNullOrEmpty(fromDate) ? DateTime.Now : DateTime.Parse(fromDate);
            DateTime dateto = string.IsNullOrEmpty(toDate) ? DateTime.Now : DateTime.Parse(toDate);
            dateto = dateto.AddDays(1);
            GeneralReportModel model = new GeneralReportModel();

            var schools = Context.schools.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto && x.Status == "Approved") &&
            (city == "" || x.City == city));

            model.SchoolCount = schools.Where(x=>x.StageOfSchool != null).Count();
            model.PrimarySchoolCount = schools.Where(x => x.StageOfSchool == "Primary").Count();
            model.SeconderySchoolCount = schools.Where(x => x.StageOfSchool == "Secondary").Count();
            model.MiddleSchoolCount = schools.Where(x => x.StageOfSchool == "Middle").Count();

            var students = Context.participant_profile.Where(x =>
             (x.CreatedAt >= dateFrom) &&
             (x.CreatedAt < dateto) &&
             (city == "" || x.City == city));

            model.StudentCount = students.Count();
            model.MaleStudentCount = students.Select(x => x.Gender == "Male").Count();
            model.FemaleStudentCount = students.Select(x => x.Gender == "Female").Count();

            model.VolunteerCount = Context.volunteer_profile.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto) &&
            (city == "" || x.City == city)
            ).Count();

            model.SessionCount = Context.sessions.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto) &&
            (city == "" || x.City == city)
            ).Count();

            model.PartnerCount = Context.funder_profile.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto) &&
            (city == "" || x.City == city)
            ).Count();

            model.CoordinatorCount = Context.coordinator_profile.Where(x =>
            (x.CreatedAt >= dateFrom) &&
            (x.CreatedAt < dateto) &&
            (city == "" || x.school.City == city)
            ).Count();


            var resultCPpre = (from e in Context.evaluation_cp_pre
                               join s in Context.sessions on e.SessionId equals s.Id
                               where ((e.CreateAt >= dateFrom && e.CreateAt < dateto)
                               && (city == "" || s.City == city))
                               select e).Count();
            var resultMurshadeepre = (from e in Context.evaluation_murshadee_before
                                      join s in Context.sessions on e.SessionId equals s.Id
                                      where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                      && (city == "" || s.City == city))
                                      select e).Count();
            var resultPFpre = (from e in Context.evaluation_pf_pre
                               join s in Context.sessions on e.SessionId equals s.Id
                               where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                               && (city == "" || s.City == city))
                               select e).Count();
            var resultSafeerpre = (from e in Context.evaluation_safeer_pre
                                   join s in Context.sessions on e.SessionId equals s.Id
                                   where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                   && (city == "" || s.City == city))
                                   select e).Count();
            var resultSYCpre1 = (from e in Context.evaluation_syc_pre_part1
                                 join s in Context.sessions on e.SessionId equals s.Id
                                 where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                 && (city == "" || s.City == city))
                                 select e).Count();
            var resultPFpre2 = (from e in Context.evaluation_syc_pre_part2
                                join s in Context.sessions on e.SessionId equals s.Id
                                where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                && (city == "" || s.City == city))
                                select e).Count();


            model.EvaluationPreCount = resultCPpre + resultMurshadeepre + resultPFpre + resultSafeerpre + resultSYCpre1 + resultPFpre2;


            var resultCPpost = (from e in Context.evaluation_cp_post
                                join s in Context.sessions on e.SessionId equals s.Id
                                where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                && (city == "" || s.City == city))
                                select e).Count();
            var resultMurshadeepost = (from e in Context.evaluation_murshadee_after
                                       join s in Context.sessions on e.SessionId equals s.Id
                                       where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                       && (city == "" || s.City == city))
                                       select e).Count();
            var resultPFpost = (from e in Context.evaluation_pf_post
                                join s in Context.sessions on e.SessionId equals s.Id
                                where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                && (city == "" || s.City == city))
                                select e).Count();
            var resultSafeerpost = (from e in Context.evaluation_safeer_post
                                    join s in Context.sessions on e.SessionId equals s.Id
                                    where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                    && (city == "" || s.City == city))
                                    select e).Count();
            var resultSYCpost1 = (from e in Context.evaluation_syc_post_part1
                                  join s in Context.sessions on e.SessionId equals s.Id
                                  where ((e.CreatedAT >= dateFrom && e.CreatedAT < dateto)
                                  && (city == "" || s.City == city))
                                  select e).Count();
            var resultPFpost2 = (from e in Context.evaluation_syc_post_part2
                                 join s in Context.sessions on e.SessionId equals s.Id
                                 where ((e.CreatedAt >= dateFrom && e.CreatedAt < dateto)
                                 && (city == "" || s.City == city))
                                 select e).Count();
            model.EvaluationPostCount = resultCPpost + resultMurshadeepost + resultPFpost + resultSafeerpost + resultSYCpost1 + resultPFpost2;
            model.CitiesCount = Context.lk_city.Count();

            return model;

        }
        public TotalCourseByYearModel GetTotalCourseByYear(string SelectYear)
        {
            DateTime year = string.IsNullOrEmpty(SelectYear) ? DateTime.Now : DateTime.Parse(SelectYear);



            var result = Context.sessions.Where(x =>
                year.Year == (x.ActualDateTime == null ? x.ProposedDateTime.Year : x.ActualDateTime.Value.Year))
                .ToList();

            var Jan = result.Where(x =>
                1 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var Feb = result.Where(x =>
                2 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var March = result.Where(x =>
               3 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
               .ToList();

            var April = result.Where(x =>
                4 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var May = result.Where(x =>
                5 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var June = result.Where(x =>
                6 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var July = result.Where(x =>
                7 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var Aug = result.Where(x =>
               8 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
               .ToList();

            var Sept = result.Where(x =>
                9 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var Nov = result.Where(x =>
                10 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var Oct = result.Where(x =>
                11 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            var Dec = result.Where(x =>
                12 == (x.ActualDateTime == null ? x.ProposedDateTime.Month : x.ActualDateTime.Value.Month))
                .ToList();

            TotalCourseByYearModel model = new TotalCourseByYearModel();

            model.JanTotalCount = Jan.Count();
            model.FebTotalCount = Feb.Count();
            model.MarchTotalCount = March.Count();
            model.AprilTotalCount = April.Count();
            model.MayTotalCount = May.Count();
            model.JuneTotalCount = June.Count();
            model.JulyTotalCount = July.Count();
            model.AugTotalCount = Aug.Count();
            model.SeptTotalCount = Sept.Count();
            model.NovTotalCount = Nov.Count();
            model.OctTotalCount = Oct.Count();
            model.DecTotalCount = Dec.Count();

            model.JanCountApproved = Jan.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.FebCountApproved = Feb.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.MarchCountApproved = March.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.AprilCountApproved = April.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.MayCountApproved = May.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.JuneCountApproved = June.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.JulyCountApproved = July.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.AugCountApproved = Aug.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.SeptCountApproved = Sept.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.NovCountApproved = Nov.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.OctCountApproved = Oct.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();
            model.DecCountApproved = Dec.Where(x => x.Status == "Occured" || x.Status == "Approved").Count();

            return model;
        }
    }
}