using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository.Lookup
{
    public class SpecializationRepository : IRepository<lk_specialization, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<lk_specialization> Get()
        {
            return Context.lk_specialization.ToList();
        }

        public lk_specialization Get(int id)
        {
            throw new NotImplementedException();
        }

        public lk_specialization GetByCity(string name)
        {
            return Context.lk_specialization.FirstOrDefault(x => x.Name == name);
        }

        public void Post(lk_specialization entity)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, lk_specialization entity)
        {
            throw new NotImplementedException();
        }
    }
}