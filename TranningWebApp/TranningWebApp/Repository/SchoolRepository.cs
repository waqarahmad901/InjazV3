using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class SchoolRepository : IRepository<school, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<school> Get()
        {
            return Context.schools.ToList();
        }

    
        //Get Specific collection based on Id
        public school Get(int id)
        {
            return Context.schools.Find(id);
        }

        //Create a new collection
        public void Post(school entity)
        {
            Context.schools.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, school entity)
        {
            var dbEntity = Context.schools.Find(id);
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
            var entity = Context.schools.Find(id);
            if (entity != null)
            {
                Context.schools.Remove(entity);
                Context.SaveChanges();
            }
        }
        
     
    }
}