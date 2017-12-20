using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
 
    public class EvaluationCpPostRepository : IRepository<evaluation_cp_post, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_cp_post> Get()
        {
            return Context.evaluation_cp_post.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_cp_post entity)
        {
            Context.evaluation_cp_post.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_cp_post entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_cp_post Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationCpPreRepository : IRepository<evaluation_cp_pre, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_cp_pre> Get()
        {
            return Context.evaluation_cp_pre.ToList();
        }
        

        //Get Specific collection based on Id 
        public void Post(evaluation_cp_pre entity)
        {
            Context.evaluation_cp_pre.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_cp_pre entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_cp_pre Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationMurshadeeAfterRepository : IRepository<evaluation_murshadee_after, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_murshadee_after> Get()
        {
            return Context.evaluation_murshadee_after.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_murshadee_after entity)
        {
            Context.evaluation_murshadee_after.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_murshadee_after entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_murshadee_after Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationMurshadeeBeforeRepository : IRepository<evaluation_murshadee_before, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_murshadee_before> Get()
        {
            return Context.evaluation_murshadee_before.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_murshadee_before entity)
        {
            Context.evaluation_murshadee_before.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_murshadee_before entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_murshadee_before Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationSafeerPostRepository : IRepository<evaluation_safeer_post, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_safeer_post> Get()
        {
            return Context.evaluation_safeer_post.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_safeer_post entity)
        {
            Context.evaluation_safeer_post.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_safeer_post entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_safeer_post Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationSafeerPreRepository : IRepository<evaluation_safeer_pre, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_safeer_pre> Get()
        {
            return Context.evaluation_safeer_pre.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_safeer_pre entity)
        {
            Context.evaluation_safeer_pre.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_safeer_pre entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_safeer_pre Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationSycPostPart1Repository : IRepository<evaluation_syc_post_part1, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_syc_post_part1> Get()
        {
            return Context.evaluation_syc_post_part1.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_syc_post_part1 entity)
        {
            Context.evaluation_syc_post_part1.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_syc_post_part1 entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_syc_post_part1 Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationSycPostPart2Repository : IRepository<evaluation_syc_post_part2, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_syc_post_part2> Get()
        {
            return Context.evaluation_syc_post_part2.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_syc_post_part2 entity)
        {
            Context.evaluation_syc_post_part2.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_syc_post_part2 entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_syc_post_part2 Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationSycPreePart1Repository : IRepository<evaluation_syc_pre_part1, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_syc_pre_part1> Get()
        {
            return Context.evaluation_syc_pre_part1.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_syc_pre_part1 entity)
        {
            Context.evaluation_syc_pre_part1.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_syc_pre_part1 entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_syc_pre_part1 Get(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class EvaluationSycPrePart2Repository : IRepository<evaluation_syc_pre_part2, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_syc_pre_part2> Get()
        {
            return Context.evaluation_syc_pre_part2.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_syc_pre_part2 entity)
        {
            Context.evaluation_syc_pre_part2.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_syc_pre_part2 entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_syc_pre_part2 Get(int id)
        {
            throw new NotImplementedException();
        }

    }

    public class EvaluationPFPreRepository : IRepository<evaluation_pf_pre, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_pf_pre> Get()
        {
            return Context.evaluation_pf_pre.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_pf_pre entity)
        {
            Context.evaluation_pf_pre.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_pf_pre entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_pf_pre Get(int id)
        {
            throw new NotImplementedException();
        }

    }

    public class EvaluationPFPostRepository : IRepository<evaluation_pf_post, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_pf_post> Get()
        {
            return Context.evaluation_pf_post.ToList();
        }

        //Get Specific collection based on Id 
        public void Post(evaluation_pf_post entity)
        {
            Context.evaluation_pf_post.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_pf_post entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_pf_post Get(int id)
        {
            throw new NotImplementedException();
        }

    }

    public class EvaluationVolunteerCoordinatorRepository : IRepository<evaluation_volunteer_coordinator, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_volunteer_coordinator> Get()
        {
            return Context.evaluation_volunteer_coordinator.ToList();
        }

        public evaluation_volunteer_coordinator GetEvaluationForm(int sessionId,bool isCoordinator)
        {
            return Context.evaluation_volunteer_coordinator.Where(x=>x.SessionId == sessionId && x.IsCoordinator == isCoordinator).FirstOrDefault();
        }

 

        //Get Specific collection based on Id 
        public void Post(evaluation_volunteer_coordinator entity)
        {
            Context.evaluation_volunteer_coordinator.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_volunteer_coordinator entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_volunteer_coordinator Get(int id)
        {
            throw new NotImplementedException();
        }

    }



    public class EvaluationCoordinatorRepository : IRepository<evaluation_coordinator, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_coordinator> Get()
        {
            return Context.evaluation_coordinator.ToList();
        }

        public evaluation_coordinator GetEvaluationForm(int sessionId)
        {
            return Context.evaluation_coordinator.Where(x => x.SessionId == sessionId).FirstOrDefault();
        }



        //Get Specific collection based on Id 
        public void Post(evaluation_coordinator entity)
        {
            Context.evaluation_coordinator.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_coordinator entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_coordinator Get(int id)
        {
            throw new NotImplementedException();
        }

    }


    public class EvaluationVolunteerRepository : IRepository<evaluation_volunteer, int>
    {
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        public IEnumerable<evaluation_volunteer> Get()
        {
            return Context.evaluation_volunteer.ToList();
        }

        public evaluation_volunteer GetEvaluationForm(int sessionId)
        {
            return Context.evaluation_volunteer.Where(x => x.SessionId == sessionId ).FirstOrDefault();
        }



        //Get Specific collection based on Id 
        public void Post(evaluation_volunteer entity)
        {
            Context.evaluation_volunteer.Add(entity);
            Context.SaveChanges();
        }

        public void Put(int id, evaluation_volunteer entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public evaluation_volunteer Get(int id)
        {
            throw new NotImplementedException();
        }

    }




}