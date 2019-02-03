using System;
using System.Collections.Generic;
using System.Linq;
using TmsWebApp.Common;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class VolunteerRepository : IRepository<volunteer_profile, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<volunteer_profile> Get()
        {
            return Context.volunteer_profile.ToList();
        }

        //Get all collections
        public IEnumerable<volunteer_profile> Get(EnumUserRole role, string status, string filter)
        {
            if (status == "pending")
            {
                if (role == EnumUserRole.Approver1)
                    return Context.volunteer_profile.Where(x =>
                          (x.IsRejected == null || !x.IsRejected.Value)
                      && (x.IsApprovedAtLevel1 == null || !x.IsApprovedAtLevel1.Value)
                      && (x.IsApprovedAtLevel2 == null || !x.IsApprovedAtLevel2.Value)
                      && (x.IsApprovedAtLevel3 == null || !x.IsApprovedAtLevel3.Value)
                      && (string.IsNullOrEmpty(filter) || x.VolunteerName.Contains(filter) || x.Gender.Equals(filter)
                      || x.City.Contains(filter) || x.VolunteerMobile.Contains(filter) || x.VolunteerEmail.Contains(filter)
                      || x.AcademicQualification.Equals(filter) || (filter == "marked" && x.OTAttendenceForVolunteer)
                      || (filter.ToLower() == "not marked" && !x.OTAttendenceForVolunteer) || (filter.ToLower() == "active" && x.IsActive.Value)
                      || (filter.ToLower() == "in active" && !x.IsActive.Value))
                      ).ToList();
                if (role == EnumUserRole.Approver2)
                    return Context.volunteer_profile.Where(x =>
                    (x.IsRejected == null || !x.IsRejected.Value)
                      &&
                       (x.IsApprovedAtLevel1 != null && x.IsApprovedAtLevel1.Value)
                       && (x.IsApprovedAtLevel2 == null || !x.IsApprovedAtLevel2.Value)
                       && (string.IsNullOrEmpty(filter) || x.VolunteerName.Contains(filter) || x.Gender.Equals(filter)
                      || x.City.Contains(filter) || x.VolunteerMobile.Contains(filter) || x.VolunteerEmail.Contains(filter)
                      || x.AcademicQualification.Equals(filter) || (filter == "marked" && x.OTAttendenceForVolunteer)
                      || (filter.ToLower() == "not marked" && !x.OTAttendenceForVolunteer) || (filter.ToLower() == "active" && x.IsActive.Value)
                      || (filter.ToLower() == "in active" && !x.IsActive.Value))

                       ).ToList();
                if (role == EnumUserRole.Approver3)
                    return Context.volunteer_profile.Where(x =>
                    (x.IsRejected == null || !x.IsRejected.Value)
                      &&
                        (x.IsApprovedAtLevel1 != null && x.IsApprovedAtLevel1.Value)
                        && x.IsApprovedAtLevel2 != null && x.IsApprovedAtLevel2.Value
                        && (x.IsApprovedAtLevel3 == null || !x.IsApprovedAtLevel3.Value)
                        && (string.IsNullOrEmpty(filter) || x.VolunteerName.Contains(filter) || x.Gender.Equals(filter)
                      || x.City.Contains(filter) || x.VolunteerMobile.Contains(filter) || x.VolunteerEmail.Contains(filter)
                      || x.AcademicQualification.Equals(filter) || (filter == "marked" && x.OTAttendenceForVolunteer)
                      || (filter.ToLower() == "not marked" && !x.OTAttendenceForVolunteer) || (filter.ToLower() == "active" && x.IsActive.Value)
                      || (filter.ToLower() == "in active" && !x.IsActive.Value))

                      ).ToList();
            }
            if (status == "approved")
            {
                return Context.volunteer_profile.Where(x =>
              
                x.IsApprovedAtLevel3 != null && x.IsApprovedAtLevel3.Value
               
                                      && (string.IsNullOrEmpty(filter) || x.VolunteerName.Contains(filter) || x.Gender.Equals(filter)
                      || x.City.Contains(filter) || x.VolunteerMobile.Contains(filter) || x.VolunteerEmail.Contains(filter)
                      || x.AcademicQualification.Equals(filter) || (filter == "marked" && x.OTAttendenceForVolunteer)
                      || (filter.ToLower() == "not marked" && !x.OTAttendenceForVolunteer) || (filter.ToLower() == "active" && x.IsActive.Value)
                      || (filter.ToLower() == "in active" && !x.IsActive.Value)
                      )

                ).ToList();
            }
            else
            {

                //if (role == EnumUserRole.Approver2)
                //    return Context.volunteer_profile.Where(x =>
                //       x.IsRejected != null && x.IsRejected.Value
                //       && (x.IsApprovedAtLevel1 == null || !x.IsApprovedAtLevel1.Value)
                //       ).ToList();
                //if (role == EnumUserRole.Approver3)
                //    return Context.volunteer_profile.Where(x =>
                //        x.IsRejected != null && x.IsRejected.Value
                //       && x.IsApprovedAtLevel1 != null && x.IsApprovedAtLevel1.Value 
                //      ).ToList();
                return Context.volunteer_profile.Where(x =>
                     (string.IsNullOrEmpty(filter) || x.VolunteerName.Contains(filter) || x.Gender.Equals(filter)
                      || x.City.Contains(filter) || x.VolunteerMobile.Contains(filter) || x.VolunteerEmail.Contains(filter)
                      || x.AcademicQualification.Equals(filter) || (filter == "marked" && x.OTAttendenceForVolunteer)
                      || (filter.ToLower() == "not marked" && !x.OTAttendenceForVolunteer) || (filter.ToLower() == "active" && x.IsActive.Value)
                      || (filter.ToLower() == "in active" && !x.IsActive.Value)
                      || (x.RejectedBy.Contains(filter))
                      )
                      &&
                       x.IsRejected != null && x.IsRejected.Value).ToList();
            }
        }


        public IEnumerable<volunteer_profile> GetApprovedVolunteer()
        {
            return Context.volunteer_profile.Where(x =>
            x.IsApprovedAtLevel3 != null && x.IsApprovedAtLevel3.Value
            && (x.IsRejected == null || x.IsRejected.Value == false)
            && (x.IsActive != null && x.IsActive.Value)
            ).ToList();
        }


        //Get Specific collection based on Id
        public volunteer_profile Get(int id)
        {
            return Context.volunteer_profile.Find(id);
        }
        public volunteer_profile GetbyGuid(Guid id)
        {
            return Context.volunteer_profile.FirstOrDefault(x => x.RowGuid == id);
        }

        public volunteer_profile GetByGoogleId(string id)
        {
            return Context.volunteer_profile.Where(x => x.GoogleSigninId != null && x.GoogleSigninId == id).FirstOrDefault();
        }

        public volunteer_profile GetByUserId(int id)
        {
            return Context.volunteer_profile.Where(x => x.UserId == id).FirstOrDefault();
        }
        public volunteer_profile GetByLinkedInId(string id)
        {
            var aa = Context.volunteer_profile.Where(x => x.LinkedInSignInId != null && x.LinkedInSignInId == id);
            return aa.FirstOrDefault();
        }

        //Create a new collection
        public void Post(volunteer_profile entity)
        {
            Context.volunteer_profile.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, volunteer_profile entity)
        {
            Context.SaveChanges();
        }
        //Update Exisitng collection


        public void Delete(int id)
        {
            var entity = Context.volunteer_profile.Find(id);
            if (entity != null)
            {
                Context.volunteer_profile.Remove(entity);
                Context.SaveChanges();
            }
        }

        internal bool ValidateMobileNumber(string mobNumber)
        {
            return Context.volunteer_profile.Any(x => x.VolunteerMobile == mobNumber);
        }
        internal bool EmailAlreadyExist(string email)
        {
            return Context.users.Any(x => x.Email== email);
        }
        internal bool UserAlreadyExist(string user)
        {
            return Context.users.Any(x => x.Username == user);
        }
    }
}