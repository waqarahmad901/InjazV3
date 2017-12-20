using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class CertificateRepository : IRepository<certificate, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<certificate> Get()
        {
            return Context.certificates.ToList();
        }


    
        //Get Specific collection based on Id
        public certificate Get(int id)
        {
            return Context.certificates.Find(id);
        }
        public certificate GetByRowId(Guid id)
        {
            return Context.certificates.Where(x=>x.RowGUID == id).First();
        }

        //Create a new collection
        public void Post(certificate entity)
        {
            Context.certificates.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, certificate entity)
        { 
                Context.SaveChanges(); 
        }

        public void Delete(int id)
        {
            var entity = Context.certificates.Find(id);
            if (entity != null)
            {
                Context.certificates.Remove(entity);
                Context.SaveChanges();
            }
        } 
    }
}