using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Repository;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository.Lookup
{
    public class EvaluationFormRepository : IRepository<lk_evaluation_form, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<lk_evaluation_form> Get()
        {
            return Context.lk_evaluation_form.ToList();
        }


        public lk_evaluation_form Get(int id)
        {
            return Context.lk_evaluation_form.FirstOrDefault(x => x.Id == id);
        }

        public void Post(lk_evaluation_form entity)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, lk_evaluation_form entity)
        {
            throw new NotImplementedException();
        }
    }
}