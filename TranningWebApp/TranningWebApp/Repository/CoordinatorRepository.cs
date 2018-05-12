using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class CoordinatorRepository : IRepository<coordinator_profile, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<coordinator_profile> Get()
        {
            return Context.coordinator_profile.ToList();
        }

        public IEnumerable<coordinator_profile> GetByFilter(string filter)
        {
            return Context.coordinator_profile.
                Where(x => string.IsNullOrEmpty(filter)
                || x.school.Status.ToLower().Equals(filter.ToLower())
                || x.school.SchoolName.Contains(filter)
                || x.school.Region.Contains(filter)
                || x.school.City.Contains(filter)
                || x.school.District.Contains(filter)
                || x.CoordinatorEmail.Contains(filter)

                ).ToList();
        }

        public IEnumerable<coordinator_profile> GetSubCoordinator(int parentId)
        {
            return Context.coordinator_profile.Where(x => x.ParentId == parentId).ToList();
        }

        public coordinator_profile GetByRowId(Guid id)
        {
            return Context.coordinator_profile.Where(x => x.RowGuid == id).First();
        }
        public coordinator_profile GetByUserId(int id)
        {
            return Context.coordinator_profile.Where(x => x.CoordinatorUserID == id).First();
        }
        //Get Specific collection based on Id
        public coordinator_profile Get(int id)
        {
            return Context.coordinator_profile.Find(id);
        }

        public coordinator_profile GetBySchool(int id)
        {
            return Context.coordinator_profile.FirstOrDefault(x => x.SchoolId == id);
        }

        public coordinator_profile GetByEmail(string email)
        {
            return Context.coordinator_profile.FirstOrDefault(x => x.CoordinatorEmail == email);
        }


        //Create a new collection
        public void Post(coordinator_profile entity)
        {
            Context.coordinator_profile.Add(entity);
            Context.SaveChanges();
        }

        public List<school> GetAllSchools()
        {
            return Context.schools.ToList();
        }



        //Update Exisitng collection
        public void Put(int id, coordinator_profile entity)
        {
            Context.SaveChanges();
        }

        public int GetSchoolIdByUserId(int userId)
        {
            if (Context.coordinator_profile.Any(x => x.CoordinatorUserID == userId))
            {
                var coordinatorProfile = Context.coordinator_profile.FirstOrDefault(x => x.CoordinatorUserID == userId);
                if (coordinatorProfile != null)
                    return coordinatorProfile.SchoolId;
            }
            return 0;
        }

        public void Delete(int id)
        {
            var entity = Context.coordinator_profile.Find(id);
            if (entity != null)
            {
                Context.coordinator_profile.Remove(entity);
                Context.SaveChanges();
            }
        }

        public bool EmailExist(string coordinatorEmail)
        {
            return Context.users.Any(x => x.Email.ToLower().Equals(coordinatorEmail.ToLower()));
        }


    }
}