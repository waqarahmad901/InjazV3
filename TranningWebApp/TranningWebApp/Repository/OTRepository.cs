using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class OTRepository : IRepository<orientation_training, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<orientation_training> Get()
        {
            return Context.orientation_training.ToList();
        }


    
        //Get Specific collection based on Id
        public orientation_training Get(int id)
        {
            return Context.orientation_training.Find(id);
        }
        public orientation_training GetByRowId(Guid id)
        {
            return Context.orientation_training.Where(x=>x.RowGuid == id).First();
        }

        //Create a new collection
        public void Post(orientation_training entity)
        {
            Context.orientation_training.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, orientation_training entity)
        {
            var dbEntity = Context.orientation_training.Find(id);
            if (dbEntity != null)
            {
                //dbEntity.Id = entity.Id;
                //dbEntity.RowGuid = entity.RowGuid;
                //dbEntity.UserName = entity.UserName;

                //dbEntity.Salutation = entity.Salutation;
                //dbEntity.FirstName = entity.FirstName;
                //dbEntity.MiddleName = entity.MiddleName;
                //dbEntity.LastName = entity.LastName;
                //dbEntity.FullName = entity.FullName;
                //dbEntity.GenderId = entity.GenderId;
                //dbEntity.AddressLine1 = entity.AddressLine1;
                //dbEntity.AddressLine2 = entity.AddressLine2;
                //dbEntity.Province = entity.Province;
                //dbEntity.City = entity.City;
                //dbEntity.ZipCode = entity.ZipCode;
                //dbEntity.Country = entity.Country;
                //dbEntity.BirthDate = entity.BirthDate;
                //dbEntity.Phone = entity.Phone;
                //dbEntity.IsMobileVerified = entity.IsMobileVerified;
                //dbEntity.Mobile = entity.Mobile;
                //dbEntity.MobileDeviceId = entity.MobileDeviceId;
                //dbEntity.MobileProvider = entity.MobileProvider;
                //dbEntity.Email = entity.Email;
                //dbEntity.AlternateEmail = entity.AlternateEmail;
                //dbEntity.IsEmailVerified = entity.IsEmailVerified;
                //dbEntity.ImagePath = entity.ImagePath;
                //dbEntity.BloodGroup = entity.BloodGroup;
                //dbEntity.IsActive = entity.IsActive;
                //dbEntity.Description = entity.Description;
                //dbEntity.CreatedBy = entity.CreatedBy;
                //dbEntity.CreatedAt = entity.CreatedAt;
                //dbEntity.UpdatedBy = entity.UpdatedBy;
                //dbEntity.UpdatedAt = entity.UpdatedAt;
                //dbEntity.Alergies = entity.Alergies;
                //dbEntity.SpecialInterests = entity.SpecialInterests;
                //dbEntity.SpecialCare = entity.SpecialCare;
                //dbEntity.Disabilities = entity.Disabilities;

                Context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var entity = Context.orientation_training.Find(id);
            if (entity != null)
            {
                Context.orientation_training.Remove(entity);
                Context.SaveChanges();
            }
        }

        public bool RemoveAllTimes(int id)
        {
            var list = Context.ot_time.Where(x => x.OTId == id).ToList();
            Context.ot_time.RemoveRange(list);
            return true;
        }

        public bool IsOtOccures(orientation_training ot)
        {
            var volIds = ot.VolunteersIds != null ? ot.VolunteersIds.Split(',').Select(y => int.Parse(y)).ToList() : new List<int>() ;
            if (volIds.Count == 0)
                return false;
            var volunteers = Context.volunteer_profile.Where(x => volIds.Contains(x.Id)).Select(x => x.OTAttendenceForVolunteer).ToList();
            return !volunteers.Any(x => x == false);
        }
    }
}