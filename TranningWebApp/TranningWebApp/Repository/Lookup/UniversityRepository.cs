using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository.Lookup
{
    public class UniversityRepository : IRepository<lk_universtie, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<lk_universtie> Get()
        {
            return Context.lk_universtie.ToList();
        }

        public lk_universtie Get(int id)
        {
            throw new NotImplementedException();
        }

        public lk_universtie GetByCity(string name)
        {
            return Context.lk_universtie.FirstOrDefault(x => x.Name == name);
        }

        public void Post(lk_universtie entity)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, lk_universtie entity)
        {
            throw new NotImplementedException();
        }
    }
}