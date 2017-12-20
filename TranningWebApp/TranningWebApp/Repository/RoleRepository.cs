using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class RoleRepository : IRepository<lk_role, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<lk_role> Get()
        {
            return Context.lk_role.ToList();
        }

    
        //Get Specific collection based on Id
        public lk_role Get(int id)
        {
            return Context.lk_role.Find(id);
        }

        //Create a new collection
        public void Post(lk_role entity)
        {
            Context.lk_role.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, lk_role entity)
        {
      
        }

        public void Delete(int id)
        {
                       
        }
      
    }
}