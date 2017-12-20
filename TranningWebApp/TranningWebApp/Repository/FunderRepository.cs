using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class FunderRepository : IRepository<funder_profile, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<funder_profile> Get()
        {
            return Context.funder_profile.ToList();
        }


        public funder_profile GetByUserId(int id)
        {
            return Context.funder_profile.Where(x=> x.FunderUserID == id).First();
        }
        //Get Specific collection based on Id
        public funder_profile Get(int id)
        {
            return Context.funder_profile.Find(id);
        }
        public funder_profile GetByRowId(Guid id)
        {
            return Context.funder_profile.Where(x=>x.RowGUID == id).First();
        }

        //Create a new collection
        public void Post(funder_profile entity)
        {
            Context.funder_profile.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, funder_profile entity)
        { 
                Context.SaveChanges();      
        }

        public void Delete(int id)
        {
            var entity = Context.funder_profile.Find(id);
            if (entity != null)
            {
                Context.funder_profile.Remove(entity);
                Context.SaveChanges();
            }
        }

        
    }
}