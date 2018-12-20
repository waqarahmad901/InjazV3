using System;
using System.Collections.Generic;
using System.Linq;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class AccountRepository : IRepository<user, int>
    {
        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<user> Get()
        {
            return Context.users.ToList();
        }

    
        //Get Specific collection based on Id
        public user Get(int id)
        {
            return Context.users.Find(id);
        }

        //Create a new collection
        public void Post(user entity)
        {
            Context.users.Add(entity);
            Context.SaveChanges();
        }


        //Update Exisitng collection
        public void Put(int id, user entity)
        {
            var dbEntity = Context.users.Find(id);
            if (dbEntity != null)
            {
                //dbEntity.Id = entity.Id;
                //dbEntity.RowGuid = entity.RowGuid;
                //dbEntity.userName = entity.userName;

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
            var entity = Context.users.Find(id);
            if (entity != null)
            {
                Context.users.Remove(entity);
                Context.SaveChanges();
            }
        }
        
        public user GetByGuid(Guid rowGuid)
        {
            return Context.users.FirstOrDefault(x => x.RowGuid == rowGuid);
        }

        internal user GetByRoleId(int id)
        {
            return Context.users.FirstOrDefault(x => x.RoleId == id);
        }

        //public userContext GetuserContext(string mobile)
        //{
        //    var context = new userContext();

        //    mobile = mobile.Trim();
        //    if (string.IsNullOrEmpty(mobile))
        //    {
        //        context.ErrorReason = "Invalid Mobile Number";
        //        return context;
        //    }

        //    var user = Context.users.FirstOrDefault(x => mobile.Equals(x.Mobile));

        //    if (user == null)
        //    {
        //        context.ErrorReason = "Record not found";
        //        return context;
        //    }
        //    if (user.IsActive == false)
        //    {
        //        context.ErrorReason = "user is disabled";
        //        return context;
        //    }

        //    context.userGuid = user.RowGuid;
        //    context.IsMobileVerified = user.IsMobileVerified;
        //    context.userId = user.Id;
        //    if (user.IsMobileVerified == false)
        //    {
        //        context.ErrorReason = "Mobile is not verified";
        //        return context;
        //    }


        //    context.FirstName = user.FirstName;
        //    context.FullName = user.FullName;
        //    context.Mobile = mobile;
        //    context.Email = user.Email;

        //    context.userMemberships = new List<userMembership>();

        //    // Company-Staff 
        //    var legalEntityStaffs = Context.legalentity_staff.Where(x => x.userId == user.Id || x.Assistant1Id== user.Id || x.Assistant2Id == user.Id || x.Assistant3Id == user.Id || x.Assistant4Id == user.Id || x.Assistant5Id == user.Id).ToList();

        //    foreach (var legalEntityStaff in legalEntityStaffs)
        //    {
        //        var userMembership = new userMembership();
        //        userMembership.HeadOfficeId = legalEntityStaff.HeadOfficeId;
        //        userMembership.HeadOfficeName = GetLegalEntityName(legalEntityStaff.HeadOfficeId);
        //        userMembership.BranchId = legalEntityStaff.BranchId;
        //        userMembership.StaffId = legalEntityStaff.Id;
        //        if (legalEntityStaff.JobRole == "Principal")
        //        {
        //            if (legalEntityStaff.BranchId != null)
        //            {
        //                userMembership.BranchName = GetLegalEntityName(legalEntityStaff.BranchId.Value);
        //            }
        //            userMembership.PersonRole = EnumPersonRole.Principal;
        //        }
        //        else if (legalEntityStaff.JobRole == "Teacher")
        //        {
        //            userMembership.PersonRole = EnumPersonRole.Staff;
        //            userMembership.BranchName = GetLegalEntityName(legalEntityStaff.BranchId.Value);
        //        }
        //        else
        //        {
        //            throw new Exception("Unknown Staff Role");
        //        }

        //        if(legalEntityStaff.userId != user.Id) // for assistants
        //        {
        //            userMembership.BeneficiaryId = legalEntityStaff.userId;
        //            userMembership.BeneficiaryName = GetuserName(legalEntityStaff.userId);
        //            userMembership.BeneficiaryRecipientRelationshipName = "Boss";
        //        }
        //        context.userMemberships.Add(userMembership);
        //    }

        //    // Company-Members
        //    var legalEntityMembers = Context.legalentity_member.Where(x => x.userId == user.Id || x.FatherId == user.Id || x.MotherId == user.Id || x.GaurdianId == user.Id || x.Gaurdian2Id == user.Id).ToList();
        //    foreach (var legalEntityMember in legalEntityMembers)
        //    {
        //        var userMembership = new userMembership();
        //        userMembership.HeadOfficeId = legalEntityMember.HeadOfficeId;
        //        userMembership.HeadOfficeName = GetLegalEntityName(legalEntityMember.HeadOfficeId);
        //        userMembership.PersonRole = EnumPersonRole.Member;
        //        userMembership.BranchId = legalEntityMember.BranchId;
        //        userMembership.MemberId = legalEntityMember.Id;

        //        if (legalEntityMember.BranchId != null)
        //        {
        //            userMembership.BranchName = GetLegalEntityName(legalEntityMember.BranchId.Value);
        //        }
        //        userMembership.BeneficiaryRecipientRelationshipName = null;

        //        var legalEntity = Context.legalentities.First(x => x.Id == legalEntityMember.HeadOfficeId);
        //        if (legalEntityMember.userId == user.Id)
        //        {
        //            if(legalEntity.CanMembersSeeProfile)
        //                userMembership.BeneficiaryRecipientRelationshipName = "Self";

        //        }
        //        else
        //        {
        //            userMembership.BeneficiaryId = legalEntityMember.userId;
        //            userMembership.BeneficiaryName = GetuserName(legalEntityMember.userId);
        //            if (legalEntityMember.FatherId == user.Id)
        //            {
        //                userMembership.BeneficiaryRecipientRelationshipName = "Father";
        //            }
        //            else if (legalEntityMember.MotherId == user.Id)
        //            {
        //                userMembership.BeneficiaryRecipientRelationshipName = "Mother";
        //            }
        //            else if (legalEntityMember.GaurdianId == user.Id || legalEntityMember.Gaurdian2Id == user.Id)
        //            {
        //                userMembership.BeneficiaryRecipientRelationshipName = "Gaurdian";
        //            }

        //            //userMembership.CanReply
        //        }

        //        var collectionMember = Context.collection_member.Where(x => x.MemberId == legalEntityMember.Id).FirstOrDefault();
        //        if (collectionMember != null)
        //        {
        //            userMembership.ClassName =  Context.collections.Where(x => x.Id == collectionMember.CollectionId).FirstOrDefault().QualifiedName;
        //        }

        //        if (userMembership.BeneficiaryRecipientRelationshipName != null)
        //            context.userMemberships.Add(userMembership);
        //    }
        //    //Sadiq code ends here
        //    return context;

        //}



        //public IEnumerable<DashboardIcon> DashboardIcons(int headofficeId, int? branchId, string role)
        //{
        //    legalentity_subscription subscription = null;
        //    if(branchId == null)
        //        subscription = Context.legalentity_subscription.FirstOrDefault(x => x.HeadOfficeId == headofficeId && x.BranchId == null && x.IsActive == true);
        //    else
        //        subscription = Context.legalentity_subscription.FirstOrDefault(x => x.HeadOfficeId == headofficeId && x.BranchId == branchId.Value && x.IsActive == true);

        //    if (subscription != null)
        //    {
        //        bool hasExpired = false;
        //        if(!subscription.NeverExpires)
        //        {
        //            if(subscription.RenewalEndDate != null)
        //                hasExpired = subscription.RenewalEndDate.Value <= DateTime.Now;
        //        }
        //        if(hasExpired)
        //        {
        //            throw new Exception("Your subscription has expired");
        //        }


        //        var icons = from package_feature in Context.package_feature
        //                    join feature in Context.features on package_feature.FeatureId equals feature.Id
        //                    where package_feature.PackageId == subscription.PackageId && package_feature.Role == role
        //                    orderby package_feature.SortOrder
        //                    select new DashboardIcon
        //                    {
        //                        Title = feature.Title,
        //                        Code = feature.Code,
        //                        IconPath = feature.IconPath,
        //                        Region = feature.Region,
        //                        SortOrder = package_feature.SortOrder,
        //                        Url = package_feature.ActionUrl
        //                    };

        //        return icons;
        //    }
        //    return null;
        //}

        //private string GetuserName(int id)
        //{
        //    var user = Context.users.FirstOrDefault(x => x.Id == id);
        //    if (user == null)
        //        return string.Empty;
        //    return user.FullName;
        //}

        //public string GeneratePin(int userId, bool mustOverwrite = false)
        //{
        //    string pin = null;
        //    var user = Context.users.FirstOrDefault(x => x.Id == userId && x.IsActive == true);
        //    if (user == null)
        //    {
        //        return pin;
        //    }

        //    var securityPins = Context.securitypins.Where(x => x.userId == userId && x.IsActive == true && x.IsVerified == false && !string.IsNullOrEmpty(x.Pin)).ToList();
        //    if (securityPins.Count == 0)
        //    {
        //        pin = new Random().Next(1000, 9999).ToString("D4");
        //        Context.securitypins.Add(new securitypin
        //        {
        //            userId = userId,
        //            IsActive = true,
        //            IsVerified = false,
        //            Pin = pin
        //        });
        //        Context.SaveChanges();
        //    }
        //    else 
        //    {
        //        if (mustOverwrite)
        //        {
        //            foreach(var securityPin in securityPins)
        //            {
        //                securityPin.IsVerified = true;  
        //            }
        //            pin = new Random().Next(1000, 9999).ToString("D4");
        //            Context.securitypins.Add(new securitypin
        //            {
        //                userId = userId,
        //                IsActive = true,
        //                IsVerified = false,
        //                Pin = pin
        //            });
        //            Context.SaveChanges();
        //        }
        //    }

        //    return pin;
        //}

        //public user VerifyMobilePin(Guid userGuid, string pin)
        //{
        //    var user = Context.users.FirstOrDefault(x => x.RowGuid == userGuid && x.IsActive == true && x.IsMobileVerified == false);
        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    var securityPin = Context.securitypins.FirstOrDefault(x => x.userId == user.Id && x.IsActive == true && x.IsVerified == false);

        //    if (securityPin != null)
        //    {
        //        if (!pin.Equals(securityPin.Pin))
        //        {
        //          /// should we restrict one time pin verification expiry
        //            //Context.securitypins.Add(new securitypin
        //            //{
        //            //    userId = userId,
        //            //    IsActive = true,
        //            //    IsVerified = false,
        //            //    Pin = new Random().Next(1000, 9999).ToString("D4")
        //            //});
        //            //Context.SaveChanges();
        //        }
        //        else
        //        {
        //            securityPin.IsVerified = true;
        //            user.IsMobileVerified = true;
        //            Context.SaveChanges();
        //            return user;
        //        }
        //    }
        //    return null;
        //}
        public bool EmailExist(string email)
        {
            return Context.users.Any(x => x.Email.ToLower().Equals(email.ToLower()));
        }
        public bool EmailNationalIdExist(string nationalID)
        {
            return Context.participant_profile.Any(x => x.NationalID.ToLower().Equals(nationalID.ToLower()));
        }
        public bool UserExist(string username)
        {
            return Context.users.Any(x => x.Username.ToLower().Equals(username.ToLower()));
        }

    }
}