using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository.Lookup
{
    public class CountryRepository : IRepository<lk_country, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<lk_country> Get()
        {
            return Context.lk_country.ToList();
        }


        public lk_country Get(int id)
        {
            return Context.lk_country.FirstOrDefault(x => x.Id == id);
        }

        public void Post(lk_country entity)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, lk_country entity)
        {
            throw new NotImplementedException();
        }
    }
}