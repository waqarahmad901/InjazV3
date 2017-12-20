using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository.Lookup
{
    public class CityRepository : IRepository<lk_city, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<lk_city> Get()
        {
            return Context.lk_city.ToList();
        }

        public lk_city Get(int id)
        {
            throw new NotImplementedException();
        }

        public lk_city GetByCity(string city)
        {
            return Context.lk_city.FirstOrDefault(x => x.City == city);
        }

        public void Post(lk_city entity)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, lk_city entity)
        {
            throw new NotImplementedException();
        }
    }
}