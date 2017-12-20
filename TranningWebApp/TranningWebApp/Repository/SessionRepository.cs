using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using TmsWebApp.Common;
using TmsWebApp.Controllers;
using TmsWebApp.HelpingUtilities;
using TmsWebApp.Models;
using TranningWebApp.Repository.DataAccess;

namespace TranningWebApp.Repository
{
    public class SessionRepository : IRepository<session, int>
    {
        private object oSession;

        //The dendency for the DbContext specified the current class. 
        public TrainingProjectEntities Context { get; set; } = new TrainingProjectEntities();

        //Get all collections
        public IEnumerable<session> Get()
        {
            return Context.sessions.ToList();
        }

        //Get Specific collection based on Id
        public session Get(int id)
        {
            return Context.sessions.Find(id);
        }
        public session GetByRowId(Guid id)
        {
            return Context.sessions.Where(x => x.RowGUID == id).First();
        }

        //Create a new collection
        public void Post(session entity)
        {
            Context.sessions.Add(entity);
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void SetOccuredStatus()
        {
            var sessions = Context.sessions.Where(x => DateTime.Now > x.ActualDateTime.Value && x.Status == "Approved").ToList();
            sessions.ForEach(x => x.Status = SessionStatus.Occured.ToString());
            Context.SaveChanges();
            foreach (var oSession in sessions)
            {
                var cor = oSession.school.coordinator_profile.First();
                var volEmail = oSession.volunteer_profile.VolunteerEmail;
                var user = new AccountRepository().Get(oSession.CreatedBy);
                var bogusController = Util.CreateController<EmailTemplateController>();
                EmailTemplateModel emodel =
                    new EmailTemplateModel
                    {
                        Title = "Notification: Session is occured.",
                        SessionTitle = oSession.ProgramName,
                        
                        User = user.FirstName
                    };
                string body =
                    Util.RenderViewToString(bogusController.ControllerContext, "SessionOccured", emodel);
                EmailSender.SendSupportEmail(body, user.Email);

                emodel =
                       new EmailTemplateModel
                       {
                           Title = "Notification: Session is occured.",
                           SessionTitle = oSession.ProgramName,
                           CoordinatorName = cor.CoordinatorName, 
                       };
                body =
                   Util.RenderViewToString(bogusController.ControllerContext, "SessionOccured", emodel);

                EmailSender.SendSupportEmail(body, cor.CoordinatorEmail);

                emodel =
                   new EmailTemplateModel
                   {
                       Title = "Notification: Session is occured.",
                       SessionTitle = oSession.ProgramName, 
                       VolunteerName = oSession.volunteer_profile.VolunteerName, 
                   };
                body =
                   Util.RenderViewToString(bogusController.ControllerContext, "SessionOccured", emodel);
                EmailSender.SendSupportEmail(body, volEmail);
            }

        }
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        //Update Exisitng collection
        public void Put(int id, session entity)
        {
            var dbEntity = Context.sessions.Find(id);
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

        public void SetPreEvaluationStatus(int participantId, int sessionid)
        {
            var sesionPart = (from aa in Context.sessions
                              join sp in Context.session_participant on aa.Id equals sp.SessionID
                              where sp.ParticipantID == participantId && aa.Id == sessionid
                              select sp
                    ).FirstOrDefault();
            if (sesionPart != null)
            {
                sesionPart.IsPreEvaluated = true;
                Context.SaveChanges();
            }


        }
        public void SetPostEvaluationStatus(int participantId, int sessionid)
        {
            var sesionPart = (from aa in Context.sessions
                              join sp in Context.session_participant on aa.Id equals sp.SessionID
                              where sp.ParticipantID == participantId && aa.Id == sessionid
                              select sp
                    ).FirstOrDefault();
            if (sesionPart != null)
            {
                sesionPart.IsPostEvaluated = true;
                Context.SaveChanges();
            }

        }

        public void Delete(int id)
        {
            var entity = Context.sessions.Find(id);
            if (entity != null)
            {
                Context.sessions.Remove(entity);
                Context.SaveChanges();
            }
        }

        public void RemoveSessionPhoto(int id)
        {
            var photos = Context.session_photo.Where(x => x.SessionID == id).AsEnumerable();
            Context.session_photo.RemoveRange(photos);
            var evalPhotos = Context.session_evaluationform_photo.Where(x => x.SessionID == id).AsEnumerable();
            Context.session_evaluationform_photo.RemoveRange(evalPhotos);
            Context.SaveChanges();

        }

        public void RemoveSessionParticipants(List<session> oSessionUnSelected, int participantId)
        {
            foreach (var item in oSessionUnSelected)
            {
                var sp = item.session_participant.Where(x => x.ParticipantID == participantId).FirstOrDefault();

                if (sp != null)
                {
                    Context.Entry(sp).State = System.Data.Entity.EntityState.Deleted;
                    item.session_participant.Remove(sp);
                }
            }
        }
    }
}