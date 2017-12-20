using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class ParticipiantRepository : IRepository<participant_profile, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<participant_profile> Get()
        {
            return Context.participant_profile.Where(x=>x.isActive).ToList();
        }
        public IEnumerable<participant_profile> GetAll()
        {
            return Context.participant_profile.ToList();
        }

        //Get Specific collection based on Id
        public participant_profile Get(int id)
        {
            return Context.participant_profile.Find(id);
        }
        public participant_profile GetByRowId(Guid id)
        {
            return Context.participant_profile.Where(x=>x.RowGuid == id).First();
        }

        //Create a new collection
        public void Post(participant_profile entity)
        {
            Context.participant_profile.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, participant_profile entity)
        {
          
                Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = Context.participant_profile.Find(id);
            if (entity != null)
            {
                Context.participant_profile.Remove(entity);
                Context.SaveChanges();
            }
        }

        public participant_profile GetByUserId(int id)
        {
            return Context.participant_profile.Where(x => x.ParticipantUserID == id).FirstOrDefault();
        }

        public participant_profile GetParticipant( string nationalID)
        {
            return Context.participant_profile.Where(x =>  x.NationalID == nationalID).FirstOrDefault();
        }
    }
}